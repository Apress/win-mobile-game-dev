/**
 * 
 * AppFocus
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * This is a demonstration of a mechanism for determining whether an application is currently
 * active (in the foreground of the device) or inactive (in the background).
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Windows.Forms;

namespace AppFocus
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Enable the timer.
            timer1.Enabled = true;
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            // Our form has been activated so the application has the focus.
            Program.AppHasFocus = true;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            // Our form has been deactivated so the application may no longer have the focus.
            Program.AppHasFocus = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Display a debug message indicating whether the application has focus or not.
            if (Program.AppHasFocus)
            {
                System.Diagnostics.Debug.WriteLine("The application is currently active.");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("The application is not currently active.");
            }
        }

        private void menuItem1_Click(object sender, EventArgs e)
        {
            // Display Form2 as a dialog window
            using (Form2 f2 = new Form2())
            {
                f2.ShowDialog();
            }
        }
    }
}