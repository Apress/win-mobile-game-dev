using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace UpdateCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void mnuMain_LookForUpdates_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        /// <summary>
        /// Check to see if a newer version of this game is available online.
        /// </summary>
        private void CheckForUpdate()
        {
            XmlDocument xmlDoc = new XmlDocument();
            int latestRevision;

                // Make sure the user is OK with this
                if (MessageBox.Show("This will check for updates by connecting to the internet. "
                    + "You may incur charges from your data provider as a result of this. "
                    + "Are you sure you wish to continue?", "Update Check",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                    MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                // Abort
                return;
            }

            try
            {
                // Display the wait cursor while we check for an update
                Cursor.Current = Cursors.WaitCursor;

                // Try to open the xml file at the specified URL
                xmlDoc.Load("http://www.adamdawes.com/windowsmobile/LatestVersion.xml");

                // Remove the wait cursor
                Cursor.Current = Cursors.Default;

                // Read the revision from the retrieved document
                latestRevision = int.Parse(xmlDoc.SelectSingleNode("/LatestVersion/Revision").InnerText);

                // Is the retrieved version later than this assembly version?
                if (latestRevision > Assembly.GetExecutingAssembly().GetName().Version.Revision)
                {
                    // Yes, so notify the user and allow them to visit a web page with more information.
                    if (MessageBox.Show("A new version of this application is available. "
                        + "Would you like to see more information?", "New Version",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        // Open the information page
                        Process.Start("http://www.adamdawes.com", "");
                    }
                }
                else
                {
                    // No newer version is available
                    MessageBox.Show("You are already running the latest version.");
                }
            }
            catch
            {
                // Something went wrong, tell the user to try again
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Unable to retrieve update information at the moment. "
                    + "Please check your internet connection or try again later.");
            }
        }

    }
}