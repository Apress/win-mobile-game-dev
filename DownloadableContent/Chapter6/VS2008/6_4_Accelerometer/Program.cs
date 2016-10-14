/**
 * 
 * Accelerometer
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of keyboard and button handling using both Form events and also the
 * GetAsyncKeyState function.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace Accelerometer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new GameForm());
        }
    }
}