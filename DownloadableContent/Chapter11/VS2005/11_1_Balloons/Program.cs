﻿/**
 * 
 * Balloons
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * 2-D graphics using OpenGL ES
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Balloons
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