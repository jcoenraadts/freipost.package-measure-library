using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Freipost.HAL;

namespace Freipost.TestApp
{
    class Program
    {
        private static Device device;

        static void Main(string[] args)
        {
            device = new Device();
            device.Connect("COM6");

            Console.WriteLine("Remove mass from scales and press Enter");
            Console.ReadLine();

            device.DoOffsetCalibration();

            Console.WriteLine("Place reference 2kg mass on scales and press Enter");
            Console.ReadLine();

            device.DoMultiplierCalibration(2000);

            //read the cal
            var cal = device.ReadCalibration();
            Console.WriteLine("mult " + cal.Multiplier.ToString() + " offset " + cal.Offset.ToString() + " tare val " + cal.TareValue.ToString());

            Console.WriteLine("Enter key to exit");
            Console.ReadLine();
        }
    }
}
