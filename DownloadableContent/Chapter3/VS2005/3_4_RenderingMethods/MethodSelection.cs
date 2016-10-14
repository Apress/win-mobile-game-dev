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
    public partial class MethodSelection : Form
    {
        public MethodSelection()
        {
            InitializeComponent();
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void mnuMain_Methods_SimpleDraw_Click(object sender, EventArgs e)
        {
            // Create an instance of our SimpleDraw form and show it as a dialog window
            using (Method1_SimpleDraw f = new Method1_SimpleDraw())
            {
                f.ShowDialog();
            }
        }

        private void mnuMain_Methods_DoubleBuffer_Click(object sender, EventArgs e)
        {
            // Create an instance of our DoubleBuffering form and show it as a dialog window
            using (Method2_DoubleBuffering f = new Method2_DoubleBuffering())
            {
                f.ShowDialog();
            }
        }

        private void mnuMain_Methods_SmoothDraw_Click(object sender, EventArgs e)
        {
            // Create an instance of our SmoothDraw form and show it as a dialog window
            using (Method3_SmoothDraw f = new Method3_SmoothDraw())
            {
                f.ShowDialog();
            }
        }
    }
}