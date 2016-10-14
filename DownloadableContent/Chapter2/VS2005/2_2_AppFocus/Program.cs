/**
 * 
 * AppFocus
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This is a demonstration of a mechanism for determining whether an application is currently
 * active (in the foreground of the device) or inactive (in the background).
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace AppFocus
{
    static class Program
    {

        // A static variable to track whether our application has focus or not.
        static internal bool AppHasFocus = false;

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