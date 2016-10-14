/**
 * 
 * Bitmaps
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * An example of creating bitmaps (both from an embedded resource and by construction
 * from drawn primitives) and displaying them on the screen.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace Bitmaps
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