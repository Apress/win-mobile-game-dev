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
using System.Text;
using System.Windows.Forms;

namespace AppFocus
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Activated(object sender, EventArgs e)
        {
            // Our form has been activated so the application has the focus.
            Program.AppHasFocus = true;
        }

        private void Form2_Deactivate(object sender, EventArgs e)
        {
            // Our form has been deactivated so the application may no longer have the focus.
            Program.AppHasFocus = false;
        }
    }
}