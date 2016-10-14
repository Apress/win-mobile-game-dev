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
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace GameEngine
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
        /// Interpolate between two values.
        /// </summary>
        /// <param name="Factor">The interpolation factor between the two values. 0 = entirely at
        /// the previous value, 1 = entirely at the current value, the range in between
        /// will interpolate between the previous and current value.</param>
        /// <param name="CurrentValue">The value to interpolate towards</param>
        /// <param name="PreviousValue">The value to interpolate from</param>
        internal static float Interpolate(float Factor, float CurrentValue, float PreviousValue)
        {
            // As a shortcut, if both values are the same then no need to perform any further calculation
            if (CurrentValue == PreviousValue) return CurrentValue;
            // If we are at one end or the other of the interpolation range, simply return the appropriate value
            if (Factor == 0) return PreviousValue;
            if (Factor == 1) return CurrentValue;

            // Interpolate between the two values
            return (CurrentValue * Factor) + (PreviousValue * (1 - Factor));
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


        /// <summary>
        /// Generate a full file path and name to which data will be read or written.
        /// </summary>
        /// <param name="filename">The filename to which data will be read or written</param>
        /// <param name="filetype">A description of the file type being accessed. This will be
        /// used to clarify the exception if no filename is provided.</param>
        /// <returns>If the filename already contains a full file path, returns it unchanged.
        /// Otherwise adds the path of the current assembly to the filename.</returns>
        internal static string GetFullFilename(string filename, string filetype)
        {
            string filePath;

            // Do we have a filename?
            if (filename == null || filename.Length == 0)
            {
                // No, so throw an exception
                throw new Exception("No filename has been specified for the " + filetype + " data file");
            }

            // Does our filename have a full path already specified?
            if (Path.IsPathRooted(filename))
            {
                // Yes, so return it unchanged
                return filename;
            }

            // Use our own path to save the file
            filePath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

            // Join the path and the filename
            return Path.Combine(filePath, filename);
        }
    }
}
