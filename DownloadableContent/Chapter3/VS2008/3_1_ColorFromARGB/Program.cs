/**
 * 
 * ColorFromARGB
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This project shows how the Color.FromARGB function can be used to create colors
 * based upon their individual red, green and blue intensities.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace ColorFromARGB
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }
    }
}