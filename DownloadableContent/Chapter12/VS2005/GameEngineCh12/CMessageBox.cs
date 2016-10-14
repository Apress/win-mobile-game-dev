/**
 * 
 * CMessageBox
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GameEngineCh12
{
    public class CMessageBox
    {

        // The usercontrol used to display the message
        private CMessageBoxControl _msgboxControl;

        // The selected menu item (0 = left, 1 = right)
        private int _selectedItem;

        // Presentational properties for the dialog
        private Color _backColor = SystemColors.Control;
        private Color _titleBackColor = SystemColors.Window;
        private Color _titleTextColor = SystemColors.ControlText;
        private Color _messageTextColor = SystemColors.ControlText;


        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal CMessageBox()
        {
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// The back color for the dialog
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// The Back color for the title
        /// </summary>
        public Color TitleBackColor
        {
            get { return _titleBackColor; }
            set { _titleBackColor = value; }
        }

        /// <summary>
        /// The text color for the title
        /// </summary>
        public Color TitleTextColor
        {
            get { return _titleTextColor; }
            set { _titleTextColor = value; }
        }

        /// <summary>
        /// The text color for the message
        /// </summary>
        public Color MessageTextColor
        {
            get { return _messageTextColor; }
            set { _messageTextColor = value; }
        }

        /// <summary>
        /// Returns a value indicating whether the dialog is currently being displayed.
        /// </summary>
        internal bool IsDisplayed
        {
            get
            {
                return (_msgboxControl != null && _msgboxControl.Visible);
            }
        }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Displays the MessageBox dialog
        /// </summary>
        /// <param name="targetForm">The form over which the dialog will be displayed</param>
        /// <param name="Title">The title for the dialog</param>
        /// <param name="Message">The message for the dialog (multi-line text accepted)</param>
        /// <param name="MenuItemLeft">The caption for the left menu item</param>
        public void ShowDialog(Form targetForm, string Title, string Message, string MenuItemLeft)
        {
            ShowDialog(targetForm, Title, Message, MenuItemLeft, "");
        }
        /// <summary>
        /// Displays the MessageBox dialog
        /// </summary>
        /// <param name="targetForm">The form over which the dialog will be displayed</param>
        /// <param name="Title">The title for the dialog</param>
        /// <param name="Message">The message for the dialog (multi-line text accepted)</param>
        /// <param name="MenuItemLeft">The caption for the left menu item</param>
        /// <param name="MenuItemRight">The caption for the right menu item</param>
        /// <returns>Returns 0 if the left menu item was selected, 1 if the right menu item was selected.</returns>
        public int ShowDialog(Form targetForm, string Title, string Message, string MenuItemLeft, string MenuItemRight)
        {
            MainMenu originalMenu;
            MainMenu msgMenu;
            MenuItem msgMenuItem;

            // Tidy up linebreaks in the message
            Message = Message.Replace("\r\n", "\r");
            Message = Message.Replace("\n", "\r");
            Message = Message.Replace("\r", "\r\n");

            // Create and initialize the usercontrol to fill the target form
            _msgboxControl = new CMessageBoxControl();
            _msgboxControl.Location = new Point(0, 0);
            _msgboxControl.Size = new Size(targetForm.Width, targetForm.Height);

            // Set the dialog colors
            _msgboxControl.BackColor = _backColor;
            _msgboxControl.pnlTitle.BackColor = _titleBackColor;
            _msgboxControl.lblTitle.BackColor = _titleBackColor;
            _msgboxControl.lblTitle.ForeColor = _titleTextColor;
            _msgboxControl.txtMessage.BackColor = _backColor;
            _msgboxControl.txtMessage.ForeColor = _messageTextColor;

            // Set the dialog content
            _msgboxControl.SetContent(Title, Message);

            // Add the usercontrol to the game form
            targetForm.Controls.Add(_msgboxControl);
            _msgboxControl.BringToFront();

            // Update the game form's menu.
            // First store the existing menu...
            originalMenu = targetForm.Menu;
            // Now build our menu
            msgMenu = new MainMenu();
            // The left item...
            msgMenuItem = new MenuItem();
            msgMenuItem.Text = MenuItemLeft;
            msgMenuItem.Click += new System.EventHandler(Menu_ItemLeft_Click);
            msgMenu.MenuItems.Add(msgMenuItem);
            // The right item...
            if (MenuItemRight != null && MenuItemRight.Length > 0)
            {
                msgMenuItem = new MenuItem();
                msgMenuItem.Text = MenuItemRight;
                msgMenuItem.Click += new System.EventHandler(Menu_ItemRight_Click);
                msgMenu.MenuItems.Add(msgMenuItem);
            }
            // Set the menu into the form
            targetForm.Menu = msgMenu;

            // Wait until one of the messagebox menu items is selected...
            do
            {
                System.Threading.Thread.Sleep(0);
                Application.DoEvents();
            } while (_msgboxControl.Visible);

            // Restore the game's menu
            targetForm.Menu = originalMenu;
            // Remove the messagebox control from the game form
            targetForm.Controls.Remove(_msgboxControl);
            // Release resources. Set class control variable to null
            // before disposing so we don't have to worry about attempting
            // to interact with the disposed control.
            Control controlTemp = _msgboxControl;
            _msgboxControl = null;
            controlTemp.Dispose();

            // Ensure the form has focus
            targetForm.Focus();

            // Return a value identifying which menu item was selected
            return _selectedItem;
        }

        /// <summary>
        /// The user selected the left menu item.
        /// </summary>
        /// <param name="sender"></param>
        private void Menu_ItemLeft_Click(object sender, EventArgs e)
        {
            // Set the selected menu index
            _selectedItem = 0;
            // Hide the usercontrol to indicate we are finished
            _msgboxControl.Visible = false;
        }

        /// <summary>
        /// The user selected the right menu item.
        /// </summary>
        /// <param name="sender"></param>
        private void Menu_ItemRight_Click(object sender, EventArgs e)
        {
            // Set the selected menu index
            _selectedItem = 1;
            // Hide the usercontrol to indicate we are finished
            _msgboxControl.Visible = false;
        }

    }
}
