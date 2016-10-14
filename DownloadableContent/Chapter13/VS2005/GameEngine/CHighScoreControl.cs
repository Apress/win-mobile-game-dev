/**
 * 
 * CHighScoreControl
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

namespace GameEngineCh13
{
    internal partial class CHighScoreControl : UserControl
    {

        // Constants to identify the columns within the listview
        private const int COLUMN_POSITION = 0;
        private const int COLUMN_NAME = 1;
        private const int COLUMN_SCORE = 2;

        // Declare an InputPanel variable so we can control the SIP
        private Microsoft.WindowsCE.Forms.InputPanel _inputPanel;

        //-------------------------------------------------------------------------------------
        // Class constructor

        public CHighScoreControl()
        {
            InitializeComponent();

            // Initialize the usercontrol
            this.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;

            // Initialize the ListView
            lvwScores.View = View.Details;
            lvwScores.HeaderStyle = ColumnHeaderStyle.None;
            lvwScores.FullRowSelect = true;

            // Add the highscore columns
            lvwScores.Columns.Add("Position", 10, HorizontalAlignment.Left);
            lvwScores.Columns.Add("Name", 10, HorizontalAlignment.Left);
            lvwScores.Columns.Add("Score", 10, HorizontalAlignment.Right);

            // Initialize the input panel
            if (!CGameFunctions.IsSmartphone())
            {
                _inputPanel = new Microsoft.WindowsCE.Forms.InputPanel();
            }
        }

        //-------------------------------------------------------------------------------------
        // Class functions

        internal void SetTableDescription(string tableDescription)
        {
            if (tableDescription == null || tableDescription.Length == 0)
            {
                // No description was provided, so high the description label
                lblTableDescription.Visible = false;
                // Move the listview to fill the space left by the label
                lvwScores.Top -= lblTableDescription.Height;
                lvwScores.Height += lblTableDescription.Height;
            }
            else
            {
                // Set the description
                lblTableDescription.Text = tableDescription;
            }
        }

        /// <summary>
        /// Populate the ListView with all of the scores from the provided table
        /// </summary>
        /// <param name="table">The high score table to display</param>
        /// <param name="highlightEntry">An entry within the table to highlight (just added)</param>
        /// <returns></returns>
        internal ListViewItem ShowScores(CHighScoreTable table, CHighScoreEntry highlightEntry)
        {
            ListViewItem item;
            ListViewItem highlightItem = null;
            int position = 0;
            int highlightPosition = 0;

            // Remove any existing items in the listview
            lvwScores.Items.Clear();

            // Add all entries in the table to the listview
            foreach (CHighScoreEntry entry in table.Entries)
            {
                // Increment the position
                position += 1;
                // Create a new item
                item = new ListViewItem(new string[] { position.ToString() + ".", entry.Name, "    " + entry.Score.ToString() });
                // Remember if this is the score just added
                if (entry == highlightEntry)
                {
                    // Store a reference to this item so that we can return it
                    highlightItem = item;
                    highlightPosition = position;
                }
                // Add to the listview
                lvwScores.Items.Add(item);
            }

            // Set the column widths for the added data
            SetColumnWidths();

            // If we added a new item, ensure that it can be seen
            if (highlightItem != null) lvwScores.EnsureVisible(highlightPosition - 1);

            // Return the highlighted listview item if we have one
            return highlightItem;
        }

        /// <summary>
        /// Set the widths of the listview columns to match their content
        /// </summary>
        private void SetColumnWidths()
        {
            // Auto-size the position and score columns.
            // A width of -1 auto-sizes the column
            lvwScores.Columns[COLUMN_POSITION].Width = -1;
            lvwScores.Columns[COLUMN_SCORE].Width = -1;

            // Set the width of the name column.
            // This is the space remaining once the score and position widths
            // have been subtracted from the overall listview width.
            lvwScores.Columns[COLUMN_NAME].Width = lvwScores.ClientRectangle.Width - lvwScores.Columns[COLUMN_POSITION].Width - lvwScores.Columns[COLUMN_SCORE].Width - 20;

            // A width of -2 additionally extends the column to the right edge
            // of the listview. This ensures that all available space is used.
            lvwScores.Columns[COLUMN_SCORE].Width = -2;
        }

        /// <summary>
        /// Show or hide the SIP, if available
        /// </summary>
        /// <param name="visible">Specify true to show the SIP, false to hide it</param>
        internal void ShowSIP(bool visible)
        {
            // Display the SIP if one is available
            if (_inputPanel != null) _inputPanel.Enabled = visible;
        }

        /// <summary>
        /// Don't allow any items in the listview to receive the focus.
        /// This serves no use and makes the list look ugly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwScores_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (lvwScores.SelectedIndices.Count > 0)
            {
                lvwScores.Items[lvwScores.SelectedIndices[0]].Focused = false;
                lvwScores.Items[lvwScores.SelectedIndices[0]].Selected = false;
            }
        }

        /// <summary>
        /// The control has been re-sized. Ensure everything within it is
        /// updated appropriately.
        /// </summary>
        private void HighScoreControl_Resize(object sender, EventArgs e)
        {
            int sipHeight = 0;

            // Make sure we have been added to a form
            if (this.Parent != null)
            {
                // Resize the listview
                lvwScores.Width = this.ClientSize.Width - lvwScores.Left * 2;
                lvwScores.Height = this.ClientSize.Height - lvwScores.Top - lvwScores.Left;

                // Resize the columns
                SetColumnWidths();

                // Resize the EnterName panel
                pnlEnterName.Height = txtName.Top + txtName.Height + 20;
                // Reposition the EnterName panel
                pnlEnterName.Left = (this.Width - pnlEnterName.Width) / 2;
                if (_inputPanel != null) sipHeight = _inputPanel.Bounds.Height;
                pnlEnterName.Top = (this.Height - sipHeight - pnlEnterName.Height) / 2;
                pnlEnterName.BringToFront();

            }
        }

        /// <summary>
        /// Process keypresses within the name textbox
        /// </summary>
        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Did the player press Enter?
            if (e.KeyChar == '\r' || e.KeyChar == '\n')
            {
                // Prevent the character from being entered
                e.Handled = true;
                // Hide the panel to indicate we are finished with it
                pnlEnterName.Visible = false;
            }
        }

        private void txtName_GotFocus(object sender, EventArgs e)
        {
            // Display the SIP if one is available
            ShowSIP(true);
        }

        private void txtName_LostFocus(object sender, EventArgs e)
        {
            // Hide the SIP if one is available
            ShowSIP(false);
        }

        private void pnlEnterName_Paint(object sender, PaintEventArgs e)
        {
            // Draw a shaded border around the edge of the "enter your name" panel
            Pen p = new Pen(Color.Black);

            for (int i = 0; i < 3; i++)
            {
                p.Color = Color.FromArgb(i * 80, i * 80, i * 80);
                e.Graphics.DrawRectangle(p, i, i, pnlEnterName.Width - 1 - i * 2, pnlEnterName.Height - 1 - i * 2);
                //e.Graphics.DrawRectangle(p, 2, 2, pnlEnterName.Width - 5, pnlEnterName.Height - 5);
            }
        }

    }
}
