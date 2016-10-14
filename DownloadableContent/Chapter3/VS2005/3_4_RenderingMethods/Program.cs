/**
 * 
 * RenderingMethods
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates the problems that we need to overcome in order to achieve smooth
 * animation on the screen, and solutions to allow us to achieve this goal.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RenderingMethods
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [MTAThread]
        static void Main()
        {
            Application.Run(new MethodSelection());
        }
    }
}