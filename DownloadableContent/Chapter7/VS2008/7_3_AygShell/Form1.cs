/**
 * 
 * AygShell
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of using the AygShell audio functions for playing sound files.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AygShell
{
    public partial class Form1 : Form
    {

        private string _soundsFolder;

        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Play a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundFile(string Filename, bool ASync)
        {
            //Instantiate the player wrapper
            AygShellSoundPlayer sp = new AygShellSoundPlayer();

            // Play the sound
            if (ASync)
            {
                // Play asynchronously
                sp.PlaySoundFile(Filename, true);
            }
            else
            {
                // Confirm if this is a MIDI file
                if (Path.GetExtension(Filename).ToLower() == ".mid")
                {
                    if (MessageBox.Show("MIDI files may take a very long time " +
                        "to complete playback, and the application will not " +
                        "respond while playing in synchronous mode. Are you " +
                        "sure you wish to continue?", "Synchronous MIDI playback",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                            == DialogResult.No)
                    {
                        return;
                    }
                }

                // Change the label to indicate that the sound is playing
                lblTitle.Text = "Playing sound...";
                lblTitle.Refresh();
                // Play synchronously
                sp.PlaySoundFile(Filename, false);
                // Reset the label text now that we have finished
                lblTitle.Text = "Please select a sound to play:";
            }
        }

        /// <summary>
        /// Initialize the form: set up the listview and add all sound resources to it
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            // Determine the location of the sound files
            _soundsFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            _soundsFolder = Path.Combine(_soundsFolder, "Sounds");

            // Configure the listview
            lvwSounds.Columns.Add(new ColumnHeader());
            lvwSounds.Columns[0].Text = "Sounds";
            // Display the sounds that are available in the listview
            foreach (string filename in Directory.GetFiles(_soundsFolder))
            {
                switch (Path.GetExtension(filename).ToLower())
                {
                    case ".wav":
                    case ".mp3":
                    case ".mid":
                        // Add these supported file types to the listview
                        lvwSounds.Items.Add(new ListViewItem(Path.GetFileName(filename)));
                        break;
                }
            }
            // Select the first item in the list
            if (lvwSounds.Items.Count > 0) lvwSounds.Items[0].Selected = true;
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

                // Is this an absolute file path?
                if (!Path.IsPathRooted(soundName))
                {
                    // No, so play it from the Sounds directory
                    soundName = Path.Combine(_soundsFolder, soundName);
                }
                // Play the sound file
                PlaySoundFile(soundName, mnuMain_Menu_Async.Checked);
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
                ofd.Filter = "Sound files (*.wav; *.mp3; *.mid)|*.wav;*.mp3;*.mid|All files|*.*";
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

        private void mnuMain_Menu_StopPlayback_Click(object sender, EventArgs e)
        {
            AygShellSoundPlayer.StopAllSounds();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            // Stop sound playback when the form loses focus
            AygShellSoundPlayer.StopAllSounds();
        }

    }
}