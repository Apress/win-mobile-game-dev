/**
 * 
 * CMessageBoxControl
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GameEngineCh9
{
    internal partial class CMessageBoxControl : UserControl
    {
        // P/Invoke declaration and constant to find the number of lines of text displayed
        // within a Textbox control.
        [DllImport("coredll")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        private const int EM_GETLINECOUNT = 0xBA;


        //-------------------------------------------------------------------------------------
        // Class constructor

        public CMessageBoxControl()
        {
            InitializeComponent();

            // Initialize the usercontrol
            this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            // Hide the focus box so that the player cannot see it
            txtFocusBox.Top = -100;
        }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Set the content of the messagebox text fields
        /// </summary>
        internal void SetContent(string Title, string Message)
        {
            // Set the title
            lblTitle.Text = Title;
            // Set the text
            txtMessage.Text = Message;
            // Update the layout
            LayoutControl();
        }

        /// <summary>
        /// Set the positions of the UI elements within the control.
        /// Also determine whether the message needs a vertical scrollbar.
        /// </summary>
        private void LayoutControl()
        {
            int textLines;
            int fontHeight;

            // Set the message width
            txtMessage.Width = this.Width - txtMessage.Left * 2;

            // Obtain a Graphics object for the control
            using (Graphics gfx = this.CreateGraphics())
            {
                // First remove the textbox scrollbar
                txtMessage.ScrollBars = ScrollBars.None;
                // Find the height of the message font by measuring a single character
                fontHeight = (int)(gfx.MeasureString(" ", txtMessage.Font).Height);
                // Find the number of lines of displayed text in the messagebox
                textLines = (int)SendMessage(txtMessage.Handle, EM_GETLINECOUNT, 0, 0) + 1;
                // Set the message height
                txtMessage.Height = textLines * fontHeight;
                // Is this too large to fit on the screen?
                if (txtMessage.Height > this.Height - txtMessage.Left - txtMessage.Top)
                {
                    // Yes, so show the vertical scrollbar and reduce the height
                    txtMessage.ScrollBars = ScrollBars.Vertical;
                    txtMessage.Height = this.Height - txtMessage.Top - txtMessage.Left;
                }
            }
        }

        private void CMessageBoxControl_Resize(object sender, EventArgs e)
        {
            // When the control resizes, update UI element positions
            LayoutControl();
        }

        private void txtMessage_GotFocus(object sender, EventArgs e)
        {
            // Don't allow the message to receive the focus and show the input caret;
            // if we get focus, set it immediately to another (hidden) textbox.
            txtFocusBox.Focus();
        }


    }
}



