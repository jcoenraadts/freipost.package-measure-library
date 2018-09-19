using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Freipost.HAL
{
    public class DeviceException : Exception
    {
        public DeviceException(string message) : base(message)
        {

        }
    }

    public class DeviceReading
    {
        public int Mass { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public DeviceReading() { }

        public DeviceReading(string rawReading)
        {
            // parse the response <mass g>,<x mm>,<y mm>,<z mm>
            try
            {
                char[] separators = { ',' };
                string[] parts = rawReading.Split(separators);

                this.Mass = Int32.Parse(parts[0]);
                this.X = Int32.Parse(parts[1]);
                this.Y = Int32.Parse(parts[2]);
                this.Z = Int32.Parse(parts[3]);
            }
            catch (Exception e)
            {
            }
        }
    }

    public class CalibrationReading
    {
        public int Multiplier { get; set; }
        public int Offset { get; set; }
        public int TareValue { get; set; }

        public CalibrationReading() { }

        public CalibrationReading(string rawReading)
        {
            // parse the response <mult>,<offset>,<tare value>,<0>
            try
            {
                char[] separators = { ',' };
                string[] parts = rawReading.Split(separators);

                this.Multiplier = Int32.Parse(parts[0]);
                this.Offset = Int32.Parse(parts[1]);
                this.TareValue = Int32.Parse(parts[2]);
            }
            catch (Exception e)
            {
            }
        }
    }
    /// <summary>
    /// The cal process is as follows:
    /// 1. Reset the Tare Value with TareReset()
    /// 1. Set the cal values to offset=0, multiplier=10000
    /// 2. Read the load cell with no mass on board
    /// 3. Set the offset as the value measured in step 2, and the multiplier as 10000
    /// 4. Place a known mass on the load cell (e.g. 10kg)
    /// 5. Read the load cell
    /// 6. Set the offset as per the value in step 3, and set the multipler as follows:
    ///     multiplier = known-mass-in-grams / measured-mass-in-grams * 10000
    /// </summary>
    public class Device
    {
        private SerialPort P;
        private const int BAUD_RATE = 9600;
        private const int TIMEOUT = 5000;    //ms

        private const int DEFAULT_LC_MULT = 10000;
        private const int DEFAULT_LC_OFFSET = 0;
        private const int CAL_READING_COUNT = 8;
        private const int COMMS_DELAY = 5; //ms
        private const int AVERAGE_READING_DELAY = 100; //ms
        private const int COMMS_RETRY_COUNT = 3;

        private string InstrTare = "T";
        private string InstrTareReset = "S";
        private string InstrLoadCellOffset = "O";
        private string InstrLoadCellMult = "M";
        private string InstrXMax = "X";
        private string InstrYMax = "Y";
        private string InstrZMax = "Z";
        private string InstrRead = "R";
        private string InstrReadCal = "L";

        private string ResponseOk = "OK";
        private string ResponseErrorBadCommand = "ERROR_CMD";
        private string ResponseErrorIncorrectLength = "ERROR_LENGTH";

        public Device()
        {
            
        }

        /// <summary>
        /// Opens the serial port to connect the device. Throws serial port exceptions on failure
        /// </summary>
        /// <param name="comport"></param>
        public void Connect(string comport)
        {
            P = new SerialPort(comport, BAUD_RATE);
            P.ReadTimeout = TIMEOUT;
            P.NewLine = "\r";
            P.Open();
        }

        /// <summary>
        /// Closes the serial port. This is preferred when shutting down to avoid hanging resources
        /// </summary>
        public void Disconnect()
        {
            if (P.IsOpen)
                P.Close();
        }

        /// <summary>
        /// Sets the maximum dimensions for the machine. This is required for correct computation of package size,
        /// as the sensors measure the distance to the package, and subtract that from these values
        /// </summary>
        /// <param name="x">Max distance in X direction in mm</param>
        /// <param name="y">Max distance in Y direction in mm</param>
        /// <param name="z">Max distance in Z direction in mm</param>
        public void SetMaxDimensions(int x, int y, int z)
        {
            write(InstrXMax, x);
            write(InstrYMax, y);
            write(InstrZMax, y);
        }

        /// <summary>
        /// Executes a TARE function on the device as the button on the device does
        /// </summary>
        public void Tare()
        {
            write(InstrTare);
        }

        /// <summary>
        /// Resets the tare value to zero, so the scales can be calibrated
        /// </summary>
        public void TareReset()
        {
            write(InstrTareReset);
        }
        
        /// <summary>
        /// Call this function after setting default cal values, and with no mass on
        /// the scales. Determines the correct zero-mass offset
        /// </summary>
        public void DoOffsetCalibration()
        {
            this.TareReset();
            this.SetLoadCellOffset(DEFAULT_LC_OFFSET);
            this.SetLoadCellMultiplier(DEFAULT_LC_MULT);
            System.Threading.Thread.Sleep(200);  //give it time to get a reading

            int offsetValue = (int)GetAverageMass();

            // write the value to the device
            this.SetLoadCellOffset(offsetValue);
        }

        /// <summary>
        /// Measures the scales with a reference mass, and determines the span calibration factor
        /// Then writes the factor to the device
        /// Assumes the offset calibration is done already
        /// <param name="referenceMass">The mass in grams of the reference mass used e.g. 10000g</param>
        /// </summary>
        public void DoMultiplierCalibration(int referenceMass)
        {
            System.Threading.Thread.Sleep(200);  //give it time to get a reading
            double rawMass = GetAverageMass();
            double multiplier = (double)referenceMass / rawMass * 10000;

            // write the value to the device
            this.SetLoadCellMultiplier((int)multiplier);
        }

        public CalibrationReading ReadCalibration()
        {
            string readingStr = this.writeRead(this.InstrReadCal);
            return new CalibrationReading(readingStr);
        }

        public DeviceReading Read()
        {
            string readingStr = this.writeRead(this.InstrRead);
            return new DeviceReading(readingStr);
        }

        /// <summary>
        /// Sets the cal values in the device. 
        /// </summary>
        /// <param name="offset">The offset to value to bring the scales to 0 with no mass</param>
        public void SetLoadCellOffset(int offset)
        {
            this.write(this.InstrLoadCellOffset, offset);
        }

        /// <summary>
        /// Sets the cal values in the device. 
        /// </summary>
        /// <param name="multiplier">The calibration factor which maps the measured value to a real mass</param>
        public void SetLoadCellMultiplier(int multiplier)
        {
            this.write(this.InstrLoadCellMult, multiplier);
        }

        public double GetAverageMass()
        {
            double mass = 0;
            for (int i = 0; i < CAL_READING_COUNT; i++)
            {
                mass += this.Read().Mass;
                System.Threading.Thread.Sleep(AVERAGE_READING_DELAY);
            }
            return mass / CAL_READING_COUNT;
        }


        #region private methods

        private void write(string instruction, int value = 0)
        {
            writeRead(instruction, value);
        }

        /// <summary>
        /// Firmware process:
        /// 1. Write message to device with format <instruction-1><sign-1><value-8><newline-1>
        /// 2. Read full message echo - should match
        /// 3. Read status, can be: "OK", "ERROR_LENGTH" or "ERROR_CMD"
        /// 4. If read is required (e.g. for status request) read the message
        /// </summary>
        /// <param name="instruction"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string writeRead(string instruction, int value = 0)
        {
            string response = "";
            // 3 retries
            for (int retryIndex = 1; retryIndex <= COMMS_RETRY_COUNT; retryIndex++)
            {
                Console.WriteLine("\nRetry: " + retryIndex.ToString());

                // write the instruction and data
                string sign = value >= 0 ? "+" : "-";
                string message = instruction + sign + (Math.Abs(value)).ToString("X8") + "\r";
                Console.WriteLine("Writing: " + message);

                byte[] bytes = Encoding.ASCII.GetBytes(message);
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte[] tempBytes = { bytes[i] };
                    P.Write(tempBytes, 0, 1);
                    System.Threading.Thread.Sleep(COMMS_DELAY);
                }

                // read the echo
                string echo = P.ReadLine();
                Console.WriteLine("Echo: " + echo);

                //read the status - if error, no more data will follow
                string status = P.ReadLine();

                // check for response
                if (!status.Contains("ERROR") && !status.Contains("OK"))
                {
                    response = status;
                    Console.WriteLine("Response: " + response);
                    status = P.ReadLine();
                }

                Console.WriteLine("Status: " + status);

                if (status.Contains("ERROR"))
                {
                    throw new DeviceException("The device returned an error in response to command: " + message + " with echo: " + echo);
                }

                // check if the echo was correct, then exit
                if(message.Trim() == echo)
                {
                    break;
                }

                // if retires have expired
                if (retryIndex >= COMMS_RETRY_COUNT)
                {
                    throw new DeviceException("The device failed to parse message " + COMMS_RETRY_COUNT.ToString() + " times");
                }
            }

            return response;
        }


        //private string writeRead(string instruction, int value = 0)
        //{
        //    this.write(instruction, value);

        //    // read the response - only for status right now
        //    string response = P.ReadLine();
        //    Console.WriteLine("Response: " + response);

        //    //char[] separators = { ',' };
        //    //string[] parts = retVal.Split(separators);
        //    //int returnedInt = Int32.Parse(parts[0]);
        //    //if (returnedInt != value)
        //    //{
        //    //    value++;
        //    //}

        //    //// read the response
        //    //string status = P.ReadLine();
        //    //string response = "";

        //    // if it is neither error or ok, then it must be a response
        //    //if (!status.Contains(this.ResponseOk) && !status.Contains(this.ResponseError))
        //    //{
        //    //    response = status;
        //    //    //and we should read the status again
        //    //    status = P.ReadLine();
        //    //}

        //    // throw exception if we have an error
        //    //if (status.Contains(this.ResponseError))
        //    //{
        //    //    throw new DeviceException("The device returned an error in response to command: " + instruction + " " + value.ToString());
        //    //}

        //    return response;
        //}

        /// <summary>
        /// Executes multiple read commands to get an average for calibration
        /// </summary>
        /// <returns></returns>
        

        

        #endregion
    }
}
