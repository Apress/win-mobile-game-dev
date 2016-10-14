/**
 * 
 * SoundPlayer
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of using the SoundPlayer class for playing sound files.
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

namespace SoundPlayer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Play a sound contained within an embedded resource
        /// </summary>
        /// <param name="ResourceFilename">The filename of the resource to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundEmbedded(string ResourceFilename, bool ASync)
        {
            //Instantiate the player wrapper
            SoundPlayerWrapper sndplayer = new SoundPlayerWrapper();

            // Play the sound
            if (ASync)
            {
                // Play asynchronously
                sndplayer.PlaySoundEmbedded(Assembly.GetExecutingAssembly(), "SoundPlayer.Sounds." + ResourceFilename, true);
            }
            else
            {
                // Change the label to indicate that the sound is playing
                lblTitle.Text = "Playing sound...";
                lblTitle.Refresh();
                // Play synchronously
                sndplayer.PlaySoundEmbedded(Assembly.GetExecutingAssembly(), "SoundPlayer.Sounds." + ResourceFilename, false);
                // Reset the label text now that we have finished
                lblTitle.Text = "Please select a sound to play:";
            }
        }

        /// <summary>
        /// Play a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundFile(string Filename, bool ASync)
        {
            //Instantiate the player wrapper
            SoundPlayerWrapper sndplayer = new SoundPlayerWrapper();

            // Play the sound
            if (ASync)
            {
                // Play asynchronously
                sndplayer.PlaySoundFile(Filename, true);
            }
            else
            {
                // Change the label to indicate that the sound is playing
                lblTitle.Text = "Playing sound...";
                lblTitle.Refresh();
                // Play synchronously
                sndplayer.PlaySoundFile(Filename, false);
                // Reset the label text now that we have finished
                lblTitle.Text = "Please select a sound to play:";
            }
        }

        /// <summary>
        /// Initialize the form: set up the listview and add all sound resources to it
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Configure the listview
            lvwSounds.Columns.Add(new ColumnHeader());
            lvwSounds.Columns[0].Text = "Sounds";
            // Display the sounds that are available in the listview
            lvwSounds.Items.Add(new ListViewItem("EnergySound.wav"));
            lvwSounds.Items.Add(new ListViewItem("MagicSpell.wav"));
            lvwSounds.Items.Add(new ListViewItem("Motorbike.wav"));
            // Select the first item in the list
            lvwSounds.Items[0].Selected = true;
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
                    PlaySoundFile(soundName, mnuMain_Menu_Async.Checked);
                }
                else
                {
                    // No, so play an embedded resource
                    PlaySoundEmbedded(soundName, mnuMain_Menu_Async.Checked);
                }
            }
        }

        /// <summary>
        /// Toggle the Async flag
        /// </summary>
        private void mnuMain_Menu_Async_Click(object sender, EventArgs e)
        {
            mnuMain_Menu_Async.Checked = !mnuMain_Menu_Async.Checked;
        }

        /// <summary>
        /// Allow the user to add a file to the list of sounds
        /// </summary>
        private void mnuMain_Menu_OpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Wav files (*.wav)|*.wav|All files|*.*";
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

    }
}