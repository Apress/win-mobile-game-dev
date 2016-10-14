/**
 * 
 * IsSmartPhone
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This is a simple demonstration of the IsSmartPhone check, which allows us to tell
 * whether our application is running on a SmartPhone device or on a device which has
 * a touch screen available.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IsSmartPhone
{
    static class Program
    {

        // Declaration of the SystemParametersInfo API function
        [DllImport("Coredll.dll")]
        static extern private int SystemParametersInfo(uint uiAction, uint uiParam, System.Text.StringBuilder pvParam, uint fWinIni);

        // Constants required for SystemParametersInfo
        private const uint SPI_GETPLATFORMTYPE = 257;

        /// <summary>
        /// Determine whether this device is a Smartphone
        /// </summary>
        /// <returns>Returns true for Smartphone devices, false for devices with
        /// touch screens</returns>
        public static bool IsSmartphone()
        {
            // Declare a StringBuilder to receive the platform type string
            StringBuilder platformType = new StringBuilder(255);

            // Call SystemParametersInfo and check the return value
            if (SystemParametersInfo(SPI_GETPLATFORMTYPE, (uint)platformType.Capacity, platformType, 0) == 0)
            {
                // No data returned so we are unable to determine the platform type.
                // Guess that we are not a smartphone as this is the most common
                // device type.
                return false;
            }

            // Return a bool depending upon the string that we have received
            return (platformType.ToString() == "SmartPhone");
        }



        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            // Is this a Smartphone? Check and report as appropriate.
            if (IsSmartphone())
            {
                MessageBox.Show("This device is a Smartphone -- no touch screen is available.");
            }
            else
            {
                MessageBox.Show("This device has a touch screen.");
            }
        }
    }






}