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
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Windows.Forms;

namespace Bitmaps
{
    public partial class Form1 : Form
    {

        Bitmap resourceBitmap;      // A bitmap that will be loaded with an image from a resource file
        Bitmap primitiveBitmap;     // A bitmap that we will draw upon to create an image

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize our bitmaps so that each one contains an image to be displayed
        /// </summary>
        private void InitializeBitmaps()
        {
            // Create resourceBitmap -- this will contain our spaceship image
            String AssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            String ResourceName = AssemblyName + ".Resources.Rocket.png";
            using (System.IO.Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName))
            {
                resourceBitmap = new Bitmap(str);
            }

            // Create primitiveBitmap -- this will contain a series of concentric filled circles
            primitiveBitmap = new Bitmap(50, 50);
            // Create a Graphics object attached to our bitmap
            using (Graphics gfx = Graphics.FromImage(primitiveBitmap))
            {
                // Fill the entire bitmap in white
                gfx.Clear(Color.White);

                // Now draw some concentric circles within the bitmap
                for (int p = 1; p <= 25; p += 4)
                {
                    // Create a brush and set its color
                    using (Brush b = new SolidBrush(Color.FromArgb(p * 10, p * 10, 0)))
                    {
                        // Fill a circle within our bitmap
                        gfx.FillEllipse(b, p, p, primitiveBitmap.Width - p * 2, primitiveBitmap.Height - p * 2);
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Random rnd = new Random();
            Rectangle renderRect;
            int i;

            // First fill the whole form in blue
            e.Graphics.Clear(Color.SkyBlue);

            // Make sure the bitmaps have been initialized
            if (primitiveBitmap == null) return;

            // Draw the bitmaps three times each
            for (i = 0; i < 3; i++)
            {
                // Draw each of the bitmaps at a random location

                // Create an ImageAttributes object so that we can set our bitmap's color key
                using (ImageAttributes imgAttributes = new ImageAttributes())
                {
                    // Set the color key to Fuschsia (Red=255, Green=0, Blue=255).
                    // This is the color we are using for transparency in our rocket image.
                    imgAttributes.SetColorKey(Color.Fuchsia, Color.Fuchsia);
                    // Determine the rectangle into which the bitmap will be drawn.
                    // This will be at a random location such that the bitmap falls
                    // entirely within the bounds of the form.
                    renderRect = new Rectangle(rnd.Next(0, this.Width - resourceBitmap.Width),
                                                rnd.Next(0, this.Height - resourceBitmap.Height),
                                                resourceBitmap.Width,
                                                resourceBitmap.Height);

                    // Draw the bitmap
                    e.Graphics.DrawImage(resourceBitmap, renderRect, 0, 0, resourceBitmap.Width, resourceBitmap.Height,
                                                GraphicsUnit.Pixel, imgAttributes);
                }

                // Create an ImageAttributes object so that we can set our bitmap's color key
                using (ImageAttributes imgAttributes = new ImageAttributes())
                {
                    // Set the color key to White.
                    // This is the color we are using for transparency in our concentric circle image.
                    imgAttributes.SetColorKey(Color.White, Color.White);
                    // Determine the rectangle into which the bitmap will be drawn.
                    renderRect = new Rectangle(rnd.Next(0, this.Width - primitiveBitmap.Width),
                                rnd.Next(0, this.Height - primitiveBitmap.Height),
                                primitiveBitmap.Width,
                                primitiveBitmap.Height);
                    // Draw the bitmap
                    e.Graphics.DrawImage(primitiveBitmap, renderRect, 0, 0, primitiveBitmap.Width, primitiveBitmap.Height,
                                                GraphicsUnit.Pixel, imgAttributes);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Invalidate the form. This will force it to repaint
            this.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Create the bitmaps that we are going to display
            InitializeBitmaps();

            // Configure and start the timer
            timer1.Interval = 1000;
            timer1.Enabled = true;
        }

    }
}