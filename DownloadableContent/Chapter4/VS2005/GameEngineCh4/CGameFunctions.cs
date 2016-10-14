/**
 * 
 * CGameFunctions
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace GameEngineCh4
{
    internal class CGameFunctions
    {
        /// <summary>
        /// Private constructor -- this class contains only static functions
        /// </summary>
        private CGameFunctions()
        {
        }


        // Declaration of the SystemParametersInfo API function
        [DllImport("Coredll.dll")]
        static extern private int SystemParametersInfo(uint uiAction, uint uiParam,
            System.Text.StringBuilder pvParam, uint fWinIni);

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
        /// Combine two rectangles into a single rectangle.
        /// </summary>
        /// <param name="r1">The first rectangle to combine</param>
        /// <param name="r2">The second rectangle to combine</param>
        /// <returns>The returned rectangle will completely encompass both
        /// of the provided rectangles. If either rectangle is empty,
        /// the other rectangle will be returned as the entire result.</returns>
        internal static Rectangle CombineRectangles(Rectangle r1, Rectangle r2)
        {
            // If either rectangle is empty, return the other one
            if (r1.IsEmpty) return r2;
            if (r2.IsEmpty) return r1;

            // Return the union of the two rectangles
            return Rectangle.Union(r1, r2);
        }

    }
}
