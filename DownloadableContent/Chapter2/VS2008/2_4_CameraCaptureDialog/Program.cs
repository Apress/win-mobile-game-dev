/**
 * 
 * CameraCaptureDialog
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * An example of using the camera to capture an image, and then transfer
 * that image into a PictureBox. For WM5+ devices only.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace CameraCaptureDialog
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