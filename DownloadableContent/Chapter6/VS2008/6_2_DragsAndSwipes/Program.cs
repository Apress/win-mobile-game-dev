/**
 * 
 * DragsAndSwipes
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates dragging and throwing game objects.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace DragsAndSwipes
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