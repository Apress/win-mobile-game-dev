/**
 * 
 * ColorFromARGB
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This project shows how the Color.FromARGB function can be used to create colors
 * based upon their individual red, green and blue intensities.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ColorFromARGB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int y;
            int colorIntensity;
            int red = 0;
            int green = 0;
            int blue = 0;

            using (Pen linePen = new Pen(Color.Black))
            {
                for (y = 0; y < ClientRectangle.Height; y++)
                {
                    // Scale the color intensity into the range 0 to 255
                    colorIntensity = y * 255 / ClientRectangle.Height;

                    // Determine which color components to use
                    if (mnuMain_Red.Checked) red = colorIntensity;
                    if (mnuMain_Green.Checked) green = colorIntensity;
                    if (mnuMain_Blue.Checked) blue = colorIntensity;

                    // Set the pen color as appropriate
                    linePen.Color = Color.FromArgb(red, green, blue);
                    // Draw a horizontal line in the new color
                    e.Graphics.DrawLine(linePen, 0, y, ClientRectangle.Width, y);
                }

            }


        }

        private void mnuMain_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuMain_Red_Click(object sender, EventArgs e)
        {
            // Toggle the checked status of the menu item
            mnuMain_Red.Checked = !mnuMain_Red.Checked;
            // Invalidate the form so that it is repainted
            this.Invalidate();
        }

        private void mnuMain_Green_Click(object sender, EventArgs e)
        {
            // Toggle the checked status of the menu item
            mnuMain_Green.Checked = !mnuMain_Green.Checked;
            // Invalidate the form so that it is repainted
            this.Invalidate();
        }

        private void mnuMain_Blue_Click(object sender, EventArgs e)
        {
            // Toggle the checked status of the menu item
            mnuMain_Blue.Checked = !mnuMain_Blue.Checked;
            // Invalidate the form so that it is repainted
            this.Invalidate();
        }
    }
}