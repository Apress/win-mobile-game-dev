/**
 * 
 * BassDLL
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of using the BASS.dll and BASS.NET for playing sound and music files.
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

namespace BassDLL
{
    public partial class Form1 : Form
    {

        private BassWrapper _bassWrapper;


        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Play a sound contained within an embedded resource
        /// </summary>
        /// <param name="ResourceFilename">The filename of the resource to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundEmbedded(string ResourceFilename)
        {
            // Get the full resource name
            string ResourceName = "BassDLL.Sounds." + ResourceFilename;
            // Load the sound if not already loaded
            _bassWrapper.LoadSoundEmbedded(Assembly.GetExecutingAssembly(), ResourceName, 3, false);
            // Play the sound
            _bassWrapper.PlaySound(ResourceName);
        }

        /// <summary>
        /// Play a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundFile(string Filename)
        {
            // Load the sound if not already loaded
            _bassWrapper.LoadSoundFile(Filename, 3, false);
            // Play the sound
            _bassWrapper.PlaySound(System.IO.Path.GetFileName(Filename));
        }

        /// <summary>
        /// Initialize the form: set up the listview and add all sound resources to it
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Instantiate the basswrapper object
            _bassWrapper = new BassWrapper();

            // Configure the listview
            lvwSounds.Columns.Add(new ColumnHeader());
            lvwSounds.Columns[0].Text = "Sounds";
            // Display the sounds that are available in the listview
            lvwSounds.Items.Add(new ListViewItem("Chainsaw.mp3"));
            lvwSounds.Items.Add(new ListViewItem("Disillusion.mod"));
            lvwSounds.Items.Add(new ListViewItem("MagicSpell.wav"));
            lvwSounds.Items.Add(new ListViewItem("Motorbike.mp3"));
            lvwSounds.Items.Add(new ListViewItem("Piano.mp3"));
            // Select the first item in the list
            lvwSounds.Items[0].Selected = true;
        }

        /// <summary>
        /// Deal with the form closing: release resources used by the basswrapper class
        /// </summary>
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Dispose of the basswrapper object so that it can release its resources
            _bassWrapper.Dispose();
        }

        /// <summary>
        /// Play the currently selected sound
        /// </summary>
        private void mnuMain_Play_Click(object sender, EventArgs e)
        {
            string soundName;

            // Make sure we have exactly one sound selected
            if (lvwSounds.SelectedIndices.Count == 1)
            {
                // Get the sound name
                soundName = lvwSounds.Items[lvwSounds.SelectedIndices[0]].Text;

                // Is this a file?
                if (System.IO.File.Exists(soundName))
                {
                    // Yes, so play the file
                    PlaySoundFile(soundName);
                }
                else
                {
                    // No, so play an embedded resource
                    PlaySoundEmbedded(soundName);
                }
            }
        }

        /// <summary>
        /// Allow the user to add a file to the list of sounds
        /// </summary>
        private void mnuMain_Menu_OpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Supported sound files|*.wav;*.mp3;*.ogg;*.mod;*.it;*.xm;*.s3m;*.mo3|All files|*.*";
                ofd.FilterIndex = 0;
                ofd.InitialDirectory = "/";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // First make sure this isn't already in the listview
                    foreach (ListViewItem lvi in lvwSounds.Items)
                    {
                        if (lvi.Text.ToLower() == ofd.FileName.ToLower())
                        {
                            lvi.Selected = true;
                            return;
                        }
                    }
                    // Add the item to the listview
                    lvwSounds.Items.Add(new ListViewItem(ofd.FileName));
                }
            }
        }

        private void mnuMain_Menu_Pause_Click(object sender, EventArgs e)
        {
            // Are we already paused?
            if (!mnuMain_Menu_Pause.Checked)
            {
                // No, so pause now
                mnuMain_Menu_Pause.Checked = true;
                _bassWrapper.PauseAllSounds();
            }
            else
            {
                // Yes, so resume
                mnuMain_Menu_Pause.Checked = false;
                _bassWrapper.ResumeAllSounds();
            }
        }

        private void mnuMain_Menu_Stop_Click(object sender, EventArgs e)
        {
            _bassWrapper.StopAllSounds();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            _bassWrapper.PauseAllSounds();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            _bassWrapper.ResumeAllSounds();
        }

    }
}