/**
 * 
 * Bounce
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A simple example of creating a game project based upon the GameEngine.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bounce
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