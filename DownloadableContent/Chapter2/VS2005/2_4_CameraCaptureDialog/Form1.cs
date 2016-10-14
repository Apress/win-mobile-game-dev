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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CameraCaptureDialog
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = null;

            try
            {
                // Create a new instance of CameraCaptureDialog
                using (Microsoft.WindowsMobile.Forms.CameraCaptureDialog ccd = new Microsoft.WindowsMobile.Forms.CameraCaptureDialog())
                {
                    // Set the properties of the capture
                    ccd.Mode = Microsoft.WindowsMobile.Forms.CameraCaptureMode.Still;
                    ccd.Owner = this;
                    // Show the dialog and see if we get a picture from the user
                    if (ccd.ShowDialog() == DialogResult.OK)
                    {
                        // The user took a picture so let's load it into our picturebox
                        pictureBox1.Image = new Bitmap(ccd.FileName);
                        // Now that we have retrieved the image, delete the photo file from disk
                        System.IO.File.Delete(ccd.FileName);
                    }
                    else
                    {
                        MessageBox.Show("Image capture cancelled.");
                    }
                }
            }
            finally
            {
                if (bmp != null) bmp.Dispose();
            }
        }
    }
}