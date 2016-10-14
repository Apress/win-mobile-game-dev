/**
 * 
 * CAboutBox
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GameEngineCh9
{
    public class CAboutBox
    {

        // A list containing all the items to display in the About box
        private List<CAboutItem> _items = new List<CAboutItem>();

        // A panel control that will be used to display the About box
        private Panel _aboutControl;

        // Dialog property variables
        private Color _backColor;
        private Color _textColor;

        // A reference to the game assembly
        private Assembly _gameAssembly;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal CAboutBox()
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
        /// The default text color for items in the dialog
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// Returns a value indicating whether the dialog is currently being displayed.
        /// </summary>
        internal bool IsDisplayed
        {
            get
            {
                return (_aboutControl != null && _aboutControl.Visible);
            }
        }

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Add a text item to the About dialog
        /// </summary>
        public CAboutItem AddItem(string Text)
        {
            // Create a new item with the provided text
            CAboutItem item = new CAboutItem(Text);
            // Set the default text color
            item.TextColor = TextColor;
            // Add to the item list
            _items.Add(item);
            // Return the item so that it may be further customised
            return item;
        }

        /// <summary>
        /// Add a picture item to the About dialog
        /// </summary>
        public CAboutItem AddItem(Bitmap Picture)
        {
            // Create a new item with the provided picture
            CAboutItem item = new CAboutItem(Picture);
            // Add to the item list
            _items.Add(item);
            // Return the item so that it may be further customised
            return item;
        }

        /// <summary>
        /// Clear all of the items in the About dialog
        /// </summary>
        private void ClearItems()
        {
            // First clear up all the existing items...
            foreach (CAboutItem item in _items)
            {
                // Does this item have an associated control?
                if (item.ItemControl != null)
                {
                    // Yes, so dispose of the control and remove our reference to it
                    item.ItemControl.Dispose();
                    item.ItemControl = null;
                }
            }

            // Clear the item collection
            _items.Clear();
        }


        /// <summary>
        /// Show the About dialog
        /// </summary>
        /// <param name="targetForm">The form in which the dialog is to be displayed</param>
        public void ShowDialog(Form targetForm)
        {
            MainMenu originalMenu;
            MainMenu msgMenu;
            MenuItem msgMenuItem;

            // Make sure there are some items to show
            if (_items.Count == 0) return;

            // Store a reference to the game assembly. This will be used by the ReplaceTokens function
            _gameAssembly = Assembly.GetCallingAssembly();

            // Create and initialize the panel to fill the target form
            _aboutControl = new Panel();
            _aboutControl.Location = new Point(0, 0);
            _aboutControl.Size = new Size(targetForm.Width, targetForm.Height);
            _aboutControl.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            _aboutControl.AutoScroll = true;

            // Add a Resize handler for the panel
            _aboutControl.Resize += new System.EventHandler(PanelResize);

            // Set the dialog colors
            _aboutControl.BackColor = _backColor;
            // Create the items within the panel
            CreateAboutItems(_aboutControl);
            // Layout the items
            LayoutAboutItems(_aboutControl);

            // Add the usercontrol to the game form
            targetForm.Controls.Add(_aboutControl);
            _aboutControl.BringToFront();

            // Update the game form's menu.
            // First store the existing menu...
            originalMenu = targetForm.Menu;
            // Now build our menu
            msgMenu = new MainMenu();
            msgMenuItem = new MenuItem();
            msgMenuItem.Text = "Continue";
            msgMenuItem.Click += new System.EventHandler(Menu_Continue_Click);
            msgMenu.MenuItems.Add(msgMenuItem);
            // Set the menu into the form
            targetForm.Menu = msgMenu;

            // Wait until the Continue menu item is selected...
            do
            {
                System.Threading.Thread.Sleep(0);
                Application.DoEvents();
            } while (_aboutControl.Visible);

            // Destroy and clear the about items
            ClearItems();

            // Restore the game's menu
            targetForm.Menu = originalMenu;
            // Remove the messagebox control from the game form
            targetForm.Controls.Remove(_aboutControl);
            // Release resources. Set class control variable to null
            // before disposing so we don't have to worry about attempting
            // to interact with the disposed control.
            Control controlTemp = _aboutControl;
            _aboutControl = null;
            controlTemp.Dispose();

            // Ensure the form has focus
            targetForm.Focus();
        }

        /// <summary>
        /// Respond to the panel's resize event
        /// </summary>
        private void PanelResize(object sender, EventArgs e)
        {
            // Recalculate the item layout
            LayoutAboutItems(_aboutControl);
        }

        /// <summary>
        /// The user selected the Continue menu item.
        /// </summary>
        /// <param name="sender"></param>
        private void Menu_Continue_Click(object sender, EventArgs e)
        {
            // Hide the panel to indicate we are finished
            _aboutControl.Visible = false;
        }

        /// <summary>
        /// Create Label and Picturebox controls within the provided container control
        /// for each of the About items that has been added to the items collection.
        /// </summary>
        /// <param name="containerControl">The container into which the about item controls
        /// will be added.</param>
        private void CreateAboutItems(Control containerControl)
        {
            PictureBox pictureItem;
            Label textItem;

            using (Graphics gfx = containerControl.CreateGraphics())
            {
                // Create and initialize a control for each item
                foreach (CAboutItem item in _items)
                {
                    // Is this a picture?
                    if (item.Picture != null)
                    {
                        // Create and initialize the PictureBox
                        pictureItem = new PictureBox();
                        pictureItem.Image = item.Picture;
                        pictureItem.Width = item.Picture.Width;
                        pictureItem.Height = item.Picture.Height;
                        // Store a reference in the AboutItem
                        item.ItemControl = pictureItem;
                    }
                    else
                    {
                        // Create and initialize the Label
                        textItem = new Label();
                        textItem.Text = ReplaceTokens(item.Text);
                        textItem.Font = new Font(FontFamily.GenericSansSerif, item.FontSize, item.FontStyle);
                        textItem.BackColor = (item.BackColor == Color.Transparent ? containerControl.BackColor : item.BackColor);
                        textItem.ForeColor = (item.TextColor);
                        textItem.Height = (int)(gfx.MeasureString(" ", textItem.Font).Height);
                        textItem.TextAlign = ContentAlignment.TopCenter;
                        // Store a reference in the AboutItem
                        item.ItemControl = textItem;
                    }

                    // Add the control to the container
                    containerControl.Controls.Add(item.ItemControl);
                }
            }
        }

        /// <summary>
        /// Calculate the vertical position of each control within the about dialog.
        /// </summary>
        /// <param name="containerControl">The container into which the controls have been placed.</param>
        /// <param name="ItemSpacing">An additional amount of space (in pixels) to insert between each item.</param>
        /// <returns>Returns the total amount of vertical space required for the about item controls.</returns>
        private int CalculatePositions(Control containerControl, int ItemSpacing)
        {
            int totalHeight = 0;

            using (Graphics gfx = containerControl.CreateGraphics())
            {
                foreach (CAboutItem item in _items)
                {
                    // Set the item's top position
                    item.TopPosition = totalHeight;

                    // Work out how much space this item occupies.
                    // Is this a picture item?
                    if (item.Picture != null)
                    {
                        // Yes, so add the picture height
                        totalHeight += item.Picture.Height;
                    }
                    else
                    {
                        // No, so add the text height
                        using (Font f = new Font(FontFamily.GenericSansSerif, item.FontSize, item.FontStyle))
                        {
                            totalHeight += (int)(gfx.MeasureString(" ", f).Height);
                        }
                    }
                    // Add the individual item spacing
                    totalHeight += item.SpaceAfter;
                    // Add the overall spacing
                    totalHeight += ItemSpacing;
                }
            }

            // Return the height that has been calculated
            return totalHeight;
        }

        /// <summary>
        /// Determine and set the position of each of the about item controls.
        /// </summary>
        /// <param name="containerControl">The container inside which the controls have been added.</param>
        private void LayoutAboutItems(Control containerControl)
        {
            int itemsHeight;
            int itemsSpacing;

            // Ensure there are items in the collection
            if (_items.Count == 0) return;

            // Work out the minimum height required for the items
            itemsHeight = CalculatePositions(containerControl, 0);

            // Is this less than the height of the panel?
            if (itemsHeight < containerControl.Height)
            {
                // It is, so find the spacing that we can apply.
                // This is the remaining space divided between each of the items
                itemsSpacing = (containerControl.Height - itemsHeight) / _items.Count;
                // Cap the item spacing so that the items aren't too far apart
                if (itemsSpacing > containerControl.Height / 40) itemsSpacing = containerControl.Height / 40;
                CalculatePositions(containerControl, itemsSpacing);
            }

            // Put each control into the appropriate position
            foreach (CAboutItem item in _items)
            {
                // Make sure the item's control has been initialized
                if (item.ItemControl != null)
                {
                    // Set the top position
                    item.ItemControl.Top = item.TopPosition;

                    // Is this a picture?
                    if (item.Picture != null)
                    {
                        // Yes, so centralise the picture
                        item.ItemControl.Left = (containerControl.ClientSize.Width - item.Picture.Width) / 2;
                    }
                    else
                    {
                        // No, so set the label to match the container width
                        item.ItemControl.Left = 0;
                        item.ItemControl.Width = containerControl.ClientSize.Width;
                    }
                }
            }
        }

        /// <summary>
        /// Replace any special tokens within the item text with their final values.
        /// </summary>
        /// <remarks>The following tokens will be replaced:
        /// {AssemblyName} will be replaced with the game assembly's name.
        /// {AssemblyVersion} will be replaced with the game assembly's version.
        /// Note that these replacements are case-sensitive.</remarks>
        private string ReplaceTokens(string itemText)
        {
            // Are there any tokens within the string?
            if (itemText.IndexOf("{") >= 0)
            {
                // Yes, so replace any that we recognise with their final values
                itemText = itemText.Replace("{AssemblyName}", _gameAssembly.GetName().Name);
                itemText = itemText.Replace("{AssemblyVersion}", _gameAssembly.GetName().Version.ToString());
            }

            // Return the final text
            return itemText;
        }
     
    }
}
