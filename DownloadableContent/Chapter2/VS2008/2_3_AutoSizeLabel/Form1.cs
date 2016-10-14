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
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AutoSizeLabel
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Automatically resize the provided label so that its width and height match
        /// the size of its content.
        /// </summary>
        /// <param name="lbl">The label to be resized.</param>
        private void AutoSizeLabel(Label lbl)
        {
            // Create a Graphics object from our form to use to measure the label text
            using (Graphics g = this.CreateGraphics())
            {
                // Determine the required size for the label to hold its text
                SizeF size = g.MeasureString(lbl.Text, lbl.Font);
                // Allow a small amount of padding to account for measurement errors
                size.Width *= 1.05f;

                // Move the label if required to honor the TextAlign
                switch (lbl.TextAlign)
                {
                    case ContentAlignment.TopLeft:
                        // The text is left-aligned and so the left edge remains static, no need to move the control
                        break;
                    case ContentAlignment.TopRight:
                        // The text is right-aligned so move its left edge to maintain the existing right edge position.
                        // Move the left edge right by its current width and then left by the new width.
                        lbl.Left = lbl.Left + lbl.Width - (int)size.Width;
                        break;
                    case ContentAlignment.TopCenter:
                        // The text is centered, so move its left edge to maintain the central position of the label.
                        // Move the left edge right by half its current width (to its center) and then left by
                        // half the new width.
                        lbl.Left = lbl.Left + (lbl.Width - (int)size.Width) / 2;
                        break;
                }
                // Set the label's new size.
                lbl.Size = new Size((int)size.Width, (int)size.Height);
            }
        } 

        private void menuItem1_Click(object sender, EventArgs e)
        {
            // AutoSize the labels
            AutoSizeLabel(label1);
            AutoSizeLabel(label2);
            AutoSizeLabel(label3);
        }
    }
}