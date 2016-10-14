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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RenderingMethods
{
    public partial class Method3_SmoothDraw : Form
    {

        private Bitmap backBuffer = null;

        // The position of our box
        private int xpos = 0, ypos = 0;
        // The direction in which our box is moving
        private int xadd = 2, yadd = 2;
        // The size of our box
        private const int boxSize = 50;

        public Method3_SmoothDraw()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the position of the box that we are rendering
        /// </summary>
        private void UpdateScene()
        {
            // Add the velocity to the box's position
            xpos += xadd;
            ypos += yadd;

            // If the box has reached the left or right edge of the screen,
            // reverse its horizontal velocity so that it bounces back into the screen.
            if (xpos <= 0) xadd = -xadd;
            if (xpos + boxSize >= this.Width) xadd = -xadd;
            // If the box has reached the top or bottom edge of the screen,
            // reverse its vertical velocity so that it bounces back into the screen.
            if (ypos <= 0) yadd = -yadd;
            if (ypos + boxSize >= this.Height) yadd = -yadd;
        }

        /// <summary>
        /// Draw all of the graphics for our scene
        /// </summary>
        /// <param name="gfx"></param>
        private void DrawScene(Graphics gfx)
        {
            // Have we initialised our back buffer yet?
            if (backBuffer == null)
            {
                // We haven't, so initialise it now.
                backBuffer = new Bitmap(this.Width, this.Height);
            }

            // Create a graphics object for the backbuffer
            using (Graphics buffergfx = Graphics.FromImage(backBuffer))
            {
                // Clear the back buffer
                buffergfx.Clear(Color.White);

                // Draw our box at its current location
                using (Brush b = new SolidBrush(Color.Blue))
                {
                    buffergfx.FillRectangle(b, xpos, ypos, boxSize, boxSize);
                }
            }

            // Finally, copy the content of the entire backbuffer to the window
            gfx.DrawImage(backBuffer, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't call into the base class.
            // This prevents any automatic painting of the form from taking place.
            //base.OnPaint(e);
        }

        private void Method3_SmoothDraw_Paint(object sender, PaintEventArgs e)
        {
            // We are repainting, so call DrawScene to do the drawing for us
            DrawScene(e.Graphics);
        }

        private void Method3_SmoothDraw_Load(object sender, EventArgs e)
        {
            // Initialise and start the timer
            timer1.Interval = 10;
            timer1.Enabled = true;

        }

        private void Method3_SmoothDraw_Closing(object sender, CancelEventArgs e)
        {
            // Disable the timer so that no further updates are attempted
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Update the position of the box
            UpdateScene();
            // Invalidate the form so that the window is repainted
            this.Invalidate();
        }
        
    }
}