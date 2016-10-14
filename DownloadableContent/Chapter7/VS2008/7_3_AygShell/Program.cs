/**
 * 
 * AygShell
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of using the AygShell audio functions for playing sound files.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AygShell
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