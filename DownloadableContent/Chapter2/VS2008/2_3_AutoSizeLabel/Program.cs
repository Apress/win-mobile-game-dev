/**
 * 
 * AutoSizeLabel
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This project demonstrates a simple mechanism for automatically sizing labels to match
 * the size of their text. This takes the text alignment into account to ensure that the
 * label text remains in the required position.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace AutoSizeLabel
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