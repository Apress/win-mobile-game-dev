/**
 * 
 * CHighScores
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace GameEngineCh13
{
    public class CHighScores
    {

        // The usercontrol used to display the scores
        private CHighScoreControl _highscoreControl;

        // A Dictionary of all the known highscore tables.
        private Dictionary<string, CHighScoreTable> _highscoreTables;

        // The filename to use for the highscore table data file. Provide a sensible default.
        private string _filename = "Scores.dat";
        // The key to use to encrypt and decrypt the score data
        private string _encryptionKey;

        // The most recently entered name
        private string _lastEnteredName;

        // Presentational properties for the highscore table
        private Color _backColor = SystemColors.Control;
        private Color _textColor = SystemColors.ControlText;
        private Color _tableBackColor = SystemColors.ControlLight;
        private Color _tableTextColor1 = SystemColors.ControlText;
        private Color _tableTextColor2 = SystemColors.ControlText;
        private Color _newEntryBackColor = SystemColors.Highlight;
        private Color _newEntryTextColor = SystemColors.HighlightText;
        private bool _showTableBorder = false;

        // P/Invoke declarations and constants to hide the ListView border
        const int GWL_STYLE = -16;
        const int WS_BORDER = 0x00800000;
        const int SWP_NOSIZE = 0x1;
        const int SWP_NOMOVE = 0x2;
        const int SWP_FRAMECHANGED = 0x20;
        [DllImport("coredll.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("coredll.dll")]
        private extern static void SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("coredll.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uflags);

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor. Scope is internal so external code cannot create instances.
        /// </summary>
        internal CHighScores()
        {
            // Initialize the highscore tables
            Clear();
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// The filename to and from which the highscore data will be written.
        /// This can be either a fully specified path and filename, or just
        /// a filename alone (in which case the file will be written to the
        /// game engine assembly directory).
        /// </summary>
        public string FileName
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// The encryption key to use when loading and saving the scores. Use
        /// an empty string (or leave uninitialized) to disable encryption.
        /// </summary>
        public string EncryptionKey
        {
            get { return _encryptionKey; }
            set { _encryptionKey = value; }
        }

        /// <summary>
        /// The back color for the highscore dialog
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set { _backColor = value; }
        }

        /// <summary>
        /// The text color for the highscore dialog
        /// </summary>
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; }
        }

        /// <summary>
        /// The back color for the score entries table
        /// </summary>
        public Color TableBackColor
        {
            get { return _tableBackColor; }
            set { _tableBackColor = value; }
        }

        /// <summary>
        /// The color for the first entry in the score entries table. The 
        /// table will fade colors between TableTextColor1 andTableTextColor2
        /// </summary>
        public Color TableTextColor1
        {
            get { return _tableTextColor1; }
            set { _tableTextColor1 = value; }
        }

        /// <summary>
        /// The color for the last entry in the score entries table. The 
        /// table will fade colors between TableTextColor1 andTableTextColor2
        /// </summary>
        public Color TableTextColor2
        {
            get { return _tableTextColor2; }
            set { _tableTextColor2 = value; }
        }

        /// <summary>
        /// The back color for the entry that has just been added to the table
        /// </summary>
        public Color NewEntryBackColor
        {
            get { return _newEntryBackColor; }
            set { _newEntryBackColor = value; }
        }

        /// <summary>
        /// The text color for the entry that has just been added to the table
        /// </summary>
        public Color NewEntryTextColor
        {
            get { return _newEntryTextColor; }
            set { _newEntryTextColor = value; }
        }

        /// <summary>
        /// Show or hide the border around the score entries table
        /// </summary>
        public bool ShowTableBorder
        {
            get { return _showTableBorder; }
            set { _showTableBorder = value; }
        }

        /// <summary>
        /// Return the name that was last entered into the high score table
        /// </summary>
        public string LastEnteredName
        {
            get { return _lastEnteredName; }
        }

        /// <summary>
        /// Returns a value indicating whether the dialog is currently being displayed.
        /// </summary>
        internal bool IsDisplayed
        {
            get
            {
                return (_highscoreControl != null && _highscoreControl.Visible);
            }
        }
            

        //-------------------------------------------------------------------------------------
        // Class functions

        /// <summary>
        /// Initialize a named high score table
        /// </summary>
        /// <param name="tableName">The name for the table to initialize</param>
        /// <param name="tableSize">The number of entries to store in this table</param>
        public void InitializeTable(string tableName, int tableSize)
        {
            // Delegate to the other version of this function
            InitializeTable(tableName, tableSize, "");
        }
        /// <summary>
        /// Initialize a named high score table
        /// </summary>
        /// <param name="tableName">The name for the table to initialize</param>
        /// <param name="tableSize">The number of entries to store in this table</param>
        /// <param name="tableDescription">A description of this table to show to the player</param>
        public void InitializeTable(string tableName, int tableSize, string tableDescription)
        {
            if (!_highscoreTables.ContainsKey(tableName))
            {
                _highscoreTables.Add(tableName, new CHighScoreTable(tableSize, tableDescription));
            }
        }

        /// <summary>
        /// Show the highscore dialog
        /// </summary>
        /// <param name="targetForm">The form for which the dialog is to be displayed</param>
        /// <param name="tableName">The name of the highscore table to display</param>
        public void ShowDialog(Form targetForm, string tableName)
        {
            ShowDialog(targetForm, tableName, 0, null);
        }
        /// <summary>
        /// Show the highscore dialog, adding a new entry if the provided score qualifies
        /// </summary>
        /// <param name="targetForm">The form for which the dialog is to be displayed</param>
        /// <param name="tableName">The name of the highscore table to display</param>
        /// <param name="score">The score that has been achieved</parparam>
        /// <param name="defaultName">The initial name to display in the "enter your name" box</param>
        public void ShowDialog(Form targetForm, string tableName, int score, string defaultName)
        {
            CHighScoreTable table;
            MainMenu originalMenu;
            MainMenu hsMenu;
            MenuItem hsMenuItem;

            // Find the table we are displaying
            table = GetTable(tableName);
            // Make sure we found it
            if (table == null) return;

            // Create and initialize the usercontrol to fill the target form
            _highscoreControl = new CHighScoreControl();
            _highscoreControl.Location = new Point(0, 0);
            _highscoreControl.Size = new Size(targetForm.Width, targetForm.Height);

            // Set the table description
            _highscoreControl.SetTableDescription(table.TableDescription);
            // Set the colors of the control and the scores
            SetControlColors(null);

            // Add the usercontrol to the game form
            targetForm.Controls.Add(_highscoreControl);
            _highscoreControl.BringToFront();

            // Update the game form's menu.
            // First store the existing menu...
            originalMenu = targetForm.Menu;
            // Now build our menu
            hsMenu = new MainMenu();
            hsMenuItem = new MenuItem();
            hsMenuItem.Text = "Continue";
            hsMenuItem.Click += new System.EventHandler(Menu_Continue_Click);
            hsMenu.MenuItems.Add(hsMenuItem);
            // Set the menu into the form
            targetForm.Menu = hsMenu;

            // Store the last entered name.
            // Even if no name is entered, we'll return back the provided default name
            _lastEnteredName = defaultName;

            // Add the score items
            _highscoreControl.ShowScores(table, null);
            // Set the colors of the control and the scores
            SetControlColors(null);

            // Has the player achieved a high score?
            if (score > table.Entries[table.Entries.Count - 1].Score)
            {
                // Yes, so display the "enter your name" panel
                _lastEnteredName = AddNewEntry(table, score, defaultName);
            }

            // Wait until the Continue menu item is clicked...
            do
            {
                System.Threading.Thread.Sleep(0);
                Application.DoEvents();
            } while (_highscoreControl.Visible);

            // Restore the game's menu
            targetForm.Menu = originalMenu;
            // Remove the highscore control from the game form
            targetForm.Controls.Remove(_highscoreControl);
            // Release resources. Set class control variable to null
            // before disposing so we don't have to worry about attempting
            // to interact with the disposed control.
            Control controlTemp = _highscoreControl;
            _highscoreControl = null;
            controlTemp.Dispose();
            
            // Ensure the form has focus
            targetForm.Focus();
        }

        /// <summary>
        /// The user clicked the Continue menu item.
        /// </summary>
        /// <param name="sender"></param>
        private void Menu_Continue_Click(object sender, EventArgs e)
        {
            // If the EnterName panel is open, hide it to indicate the name was entered
            if (_highscoreControl.pnlEnterName.Visible)
            {
                _highscoreControl.pnlEnterName.Visible = false;
            }
            else
            {
                // Hide the usercontrol to indicate we are finished
                _highscoreControl.Visible = false;
            }
        }

        /// <summary>
        /// Add a new entry to a highscore table
        /// </summary>
        /// <param name="table">The table to which the score is to be added</param>
        /// <param name="score">The score that has been achieved</parparam>
        /// <param name="defaultName">The initial name to display in the "enter your name" box</param>
        /// <returns></returns>
        private string AddNewEntry(CHighScoreTable table, int score, string defaultName)
        {
            CHighScoreEntry highlightEntry = null;
            ListViewItem highlightItem = null;
            string newName;

            // Display and initialize the EnterName panel
            _highscoreControl.pnlEnterName.Visible = true;
            _highscoreControl.txtName.Text = _lastEnteredName;
            _highscoreControl.txtName.Focus();
            _highscoreControl.txtName.SelectAll();

            // Wait until the Continue menu item is clicked...
            do
            {
                //System.Threading.Thread.Sleep(0);
                Application.DoEvents();
            } while (_highscoreControl.pnlEnterName.Visible);

            // Hide the SIP
            _highscoreControl.ShowSIP(false);

            // Retrieve the entered name
            newName = _highscoreControl.txtName.Text;
            // Remove any invalid characters
            newName = newName.Replace("\n", "");
            newName = newName.Replace("\r", "");

            // Add the name to the table
            highlightEntry = table.AddEntry(newName, score);
            // Add the updated score items
            highlightItem = _highscoreControl.ShowScores(table, highlightEntry);

            // Save the updated table
            SaveScores();

            // Set the colors of the control and the scores
            SetControlColors(highlightItem);

            // Return the name that was added
            return newName;
        }


        /// <summary>
        /// Set the colors for all of the content on the highscore control
        /// </summary>
        /// <param name="highlightItem"></param>
        private void SetControlColors(ListViewItem highlightItem)
        {
            int i = 0;
            int r1, g1, b1, r2, g2, b2, itemR, itemG, itemB;
            
            // Set the form colors
            _highscoreControl.BackColor = _backColor;
            // Set the label colors
            _highscoreControl.lblTitle.ForeColor = _textColor;
            _highscoreControl.lblTableDescription.ForeColor = _textColor;
            // Set the listview colors.
            // Set the ForeColor to be the same as the BackColor so that we cannot
            // see the items while the listview is being populated. We will override
            // these against each individual item once the scores have been added.
            _highscoreControl.lvwScores.BackColor = _tableBackColor;
            _highscoreControl.lvwScores.ForeColor = _tableBackColor;
            // Set the EnterName panel colors
            _highscoreControl.pnlEnterName.BackColor = _backColor;
            _highscoreControl.lblEnterNameText1.ForeColor = _textColor;
            _highscoreControl.lblEnterNameText2.ForeColor = _textColor;
            _highscoreControl.lblEnterNameText3.ForeColor = _textColor;

            // Are there items in the list yet?
            if (_highscoreControl.lvwScores.Items.Count > 0)
            {
                // Fade between the two text colors?
                if (_tableTextColor1.ToArgb() == _tableTextColor2.ToArgb())
                {
                    // The colors are the same so just set the ForeColor
                    _highscoreControl.lvwScores.ForeColor = _tableTextColor1;
                }
                else
                {
                    // Fade between the colors
                    r1 = _tableTextColor1.R;
                    g1 = _tableTextColor1.G;
                    b1 = _tableTextColor1.B;
                    r2 = _tableTextColor2.R;
                    g2 = _tableTextColor2.G;
                    b2 = _tableTextColor2.B;
                    // Loop for each item
                    foreach (ListViewItem lvwItem in _highscoreControl.lvwScores.Items)
                    {
                        // Don't alter the highlight item
                        if (lvwItem != highlightItem)
                        {
                            // Interpolate between the first and second red, green and blue values
                            itemR = (int)(CGameFunctions.Interpolate((float)i / _highscoreControl.lvwScores.Items.Count, r2, r1));
                            itemG = (int)(CGameFunctions.Interpolate((float)i / _highscoreControl.lvwScores.Items.Count, g2, g1));
                            itemB = (int)(CGameFunctions.Interpolate((float)i / _highscoreControl.lvwScores.Items.Count, b2, b1));
                            // Set the item color
                            lvwItem.ForeColor = Color.FromArgb(itemR, itemG, itemB);
                        }
                        else
                        {
                            lvwItem.BackColor = _newEntryBackColor;
                            lvwItem.ForeColor = _newEntryTextColor;
                        }
                        // Increase the index to update the interpolation factor
                        i += 1;
                    }
                }
            }

            // Hide the listview border?
            if (!_showTableBorder)
            {
                IntPtr handle = _highscoreControl.lvwScores.Handle;
                int style = GetWindowLong(handle, GWL_STYLE);
                style &= ~WS_BORDER;
                SetWindowLong(handle, GWL_STYLE, style);
                SetWindowPos(handle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
            }
        }

        /// <summary>
        /// Retrieve the high score table with the specified name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public CHighScoreTable GetTable(string tableName)
        {
            if (_highscoreTables.ContainsKey(tableName))
            {
                return _highscoreTables[tableName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Remove all high score tables from the object
        /// </summary>
        /// <remarks>To clear the scores for an individual table, retrieve the
        /// table object using GetTable and call the Clear method on that instead.</remarks>
        public void Clear()
        {
            // Create the table dictionary if it doesn't already exist
            if (_highscoreTables == null)
            {
                _highscoreTables = new Dictionary<string, CHighScoreTable>();
            }

            // Tell any known tables to clear their content
            foreach (CHighScoreTable table in _highscoreTables.Values)
            {
                table.Clear();
            }
        }

        /// <summary>
        /// Load the high scores from the storage file
        /// </summary>
        /// <remarks>Ensure that the tables have been created using InitializeTable
        /// prior to loading the scores.</remarks>
        public void LoadScores()
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement xmlTable;
            string filedata;
            string filename;

            try
            {
                // Clear any existing high score tables.
                Clear();

                // Get the full path and filename
                filename = CGameFunctions.GetFullFilename(_filename, "high score");

                // Make sure the score file exists
                if (!File.Exists(filename))
                {
                    // Cannot load
                    return;
                }

                // Load the xml data
                using (StreamReader sr = File.OpenText(filename))
                {
                    filedata = sr.ReadToEnd();
                }

                // Is the data encrypted?
                if (_encryptionKey != null && _encryptionKey.Length > 0)
                {
                    // We have an encryption key, so use it to decrypt
                    filedata = CEncryption.Decrypt(filedata, _encryptionKey);
                }

                // Load the xml data
                xmlDoc.LoadXml(filedata);

            // Loop for each known highscore table
            foreach (string tableName in _highscoreTables.Keys)
            {
                // See if we can find the element for this table
                xmlTable = (XmlElement)xmlDoc.SelectSingleNode("HighScores/Table[Name='" + tableName + "']");
                // Did we find one?
                if (xmlTable != null)
                {
                    // Yes, so load its data into the table object
                    LoadScores_LoadTable(_highscoreTables[tableName], xmlTable);
                }
            }

            }
            catch
            {
                // Something went wrong.
                // Abandon the attempt to load and clear the tables
                Clear();
                // Don't re-throw the exception, we'll carry on regardless
            }
        }

        /// <summary>
        /// Load the scores from an individual table within the provided
        /// XML definition.
        /// </summary>
        /// <param name="table">The table to load</param>
        /// <param name="xmlTable">The XML score entries to load</param>
        private void LoadScores_LoadTable(CHighScoreTable table, XmlElement xmlTable)
        {
            int score;
            string name;
            DateTime date;

            // Loop for each entry
            foreach (XmlElement xmlEntry in xmlTable.SelectNodes("Entries/Entry"))
            {
                // Retrieve the entry information
                score = int.Parse(xmlEntry.SelectSingleNode("Score").InnerText);
                name = xmlEntry.SelectSingleNode("Name").InnerText;
                date = DateTime.Parse(xmlEntry.SelectSingleNode("Date").InnerText);
                // Add the entry to the table.
                table.AddEntry(name, score, date);
            }
        }

        /// <summary>
        /// Save the scores to the storage file
        /// </summary>
        public void SaveScores()
        {
            CHighScoreTable table;
            string scoreData;

            // Create and initialize the objects required to build the high scores XML string
            using (MemoryStream scoresStream = new MemoryStream())
            {
                using (XmlTextWriter xmlScores = new XmlTextWriter(scoresStream, Encoding.Default))
                {

                    // Write the HighScores root element
                    xmlScores.WriteStartElement("HighScores");

                    // Loop for each table
                    foreach (string tableName in _highscoreTables.Keys)
                    {
                        // Retrieve the table object for this table name
                        table = _highscoreTables[tableName];

                        // Write the Table element
                        xmlScores.WriteStartElement("Table");
                        // Write the table Name element
                        xmlScores.WriteStartElement("Name");
                        xmlScores.WriteString(tableName);
                        xmlScores.WriteEndElement();

                        // Create the Entries element
                        xmlScores.WriteStartElement("Entries");

                        // Loop for each entry
                        foreach (CHighScoreEntry entry in table.Entries)
                        {
                            // Make sure the entry is not blank
                            if (entry.Date != DateTime.MinValue)
                            {
                                // Write the Entry element
                                xmlScores.WriteStartElement("Entry");
                                // Write the score, name and date
                                xmlScores.WriteStartElement("Score");
                                xmlScores.WriteString(entry.Score.ToString());
                                xmlScores.WriteEndElement();
                                xmlScores.WriteStartElement("Name");
                                xmlScores.WriteString(entry.Name);
                                xmlScores.WriteEndElement();
                                xmlScores.WriteStartElement("Date");
                                xmlScores.WriteString(entry.Date.ToString("yyyy-MM-ddTHH:mm:ss"));
                                xmlScores.WriteEndElement();
                                // End the Entry element
                                xmlScores.WriteEndElement();
                            }
                        }

                        // End the Entries element
                        xmlScores.WriteEndElement();

                        // End the Table element
                        xmlScores.WriteEndElement();
                    }

                    // End the root element
                    xmlScores.WriteEndElement();

                    // Close the XML writer
                    xmlScores.Close();

                    // Transfer the generated XML into a string
                    byte[] scoreBytes = scoresStream.ToArray();
                    scoreData = Encoding.Default.GetString(scoreBytes, 0, scoreBytes.Length);

                    // Are we to encrypt the data?
                    if (_encryptionKey != null && _encryptionKey.Length > 0)
                    {
                        // We have an encryption key, so use it to encrypt
                        scoreData = CEncryption.Encrypt(scoreData, _encryptionKey);
                    }

                    // Write the scores file
                    using (StreamWriter fileWriter = File.CreateText(CGameFunctions.GetFullFilename(_filename, "high score")))
                    {
                        fileWriter.Write(scoreData);
                    }
                }
            }
        }

    }
}
