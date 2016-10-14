/**
 * 
 * GDIShapes
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This project demonstrates the different primitive drawing commands that are provided
 * by the GDI Graphics object.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GDIShapes
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