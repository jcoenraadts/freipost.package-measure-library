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

        private string InstrTare = "T";
        private string InstrTareReset = "S";
        private string InstrLoadCellOffset = "O";
        private string InstrLoadCellMult = "M";
        private string InstrXMax = "X";
        private string InstrYMax = "Y";
        private string InstrZMax = "Z";
        private string InstrRead = "R";

        private string ResponseOk = "OK";
        private string ResponseError = "ERROR";
        

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
            this.setLoadCellOffset(DEFAULT_LC_OFFSET);
            this.setLoadCellMultiplier(DEFAULT_LC_MULT);

            int offsetValue = (int)getAverageMass();

            // write the value to the device
            this.setLoadCellOffset(offsetValue);
        }

        /// <summary>
        /// Measures the scales with a reference mass, and determines the span calibration factor
        /// Then writes the factor to the device
        /// Assumes the offset calibration is done already
        /// <param name="referenceMass">The mass in grams of the reference mass used e.g. 10000g</param>
        /// </summary>
        public void DoMultiplierCalibration(int referenceMass)
        {
            double rawMass = getAverageMass();
            double multiplier = (double)referenceMass / rawMass * 10000;

            // write the value to the device
            this.setLoadCellMultiplier((int)multiplier);
        }

        public DeviceReading Read()
        {
            string readingStr = this.writeRead(this.InstrRead);
            return new DeviceReading(readingStr);
        }


        #region private methods

        private void write(string instruction, int value = 0)
        {
            this.writeRead(instruction, value);
        }

        private string writeRead(string instruction, int value = 0)
        {
            string message = instruction + value.ToString("X4");
            P.WriteLine(message);
            
            // read the response
            string status = P.ReadLine();
            string response = "";

            // if it is neither error or ok, then it must be a response
            if (!status.Contains(this.ResponseOk) && !status.Contains(this.ResponseError))
            {
                response = status;
                //and we should read the status again
                status = P.ReadLine();
            }

            // throw exception if we have an error
            if (status.Contains(this.ResponseError))
            {
                throw new DeviceException("The device returned an error in response to command: " + instruction + " " + value.ToString());
            }
            
            return response;
        }

        /// <summary>
        /// Executes multiple read commands to get an average for calibration
        /// </summary>
        /// <returns></returns>
        private double getAverageMass()
        {
            double mass = 0;
            for (int i = 0; i < CAL_READING_COUNT; i++)
                mass += this.Read().Mass;
            return mass / CAL_READING_COUNT;
        }

        /// <summary>
        /// Sets the cal values in the device. 
        /// </summary>
        /// <param name="offset">The offset to value to bring the scales to 0 with no mass</param>
        private void setLoadCellOffset(int offset)
        {
            this.write(this.InstrLoadCellOffset, offset);
        }

        /// <summary>
        /// Sets the cal values in the device. 
        /// </summary>
        /// <param name="multiplier">The calibration factor which maps the measured value to a real mass</param>
        private void setLoadCellMultiplier(int multiplier)
        {
            this.write(this.InstrLoadCellMult, multiplier);
        }

        #endregion
    }
}
