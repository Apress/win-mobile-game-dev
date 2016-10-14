/**
 * 
 * PlaySound
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of using the PlaySound function for playing sound files.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PlaySound
{
    public partial class Form1 : Form
    {

        /// <summary>
        /// The PlaySound function declaration for embedded resources
        /// </summary>
        [DllImport("coredll.dll")]
        static extern bool PlaySound(byte[] data, IntPtr hMod, SoundFlags sf);
        /// <summary>
        /// The PlaySound function declaration for files
        /// </summary>
        [DllImport("coredll.dll")]
        static extern bool PlaySound(string pszSound, IntPtr hMod, SoundFlags sf);

        /// <summary>
        /// Flags used by PlaySound
        /// </summary>
        [Flags]
        public enum SoundFlags
        {
            SND_SYNC = 0x0000,              // play synchronously (default) 
            SND_ASYNC = 0x0001,             // play asynchronously 
            SND_NODEFAULT = 0x0002,         // silence (!default) if sound not found 
            SND_MEMORY = 0x0004,            // pszSound points to a memory file
            SND_LOOP = 0x0008,              // loop the sound until next sndPlaySound 
            SND_NOSTOP = 0x0010,            // don't stop any currently playing sound 
            SND_NOWAIT = 0x00002000,        // don't wait if the driver is busy 
            SND_ALIAS = 0x00010000,         // name is a registry alias 
            SND_ALIAS_ID = 0x00110000,      // alias is a predefined ID
            SND_FILENAME = 0x00020000,      // name is file name 
            SND_RESOURCE = 0x00040004       // name is resource name or atom 
        }

        public Form1()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Play a sound contained within an embedded resource
        /// </summary>
        /// <param name="Filename">The filename of the resource to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundEmbedded(string ResourceFilename, bool ASync)
        {
            byte[] soundData;

            // Retrieve a stream from our embedded sound resource.
            // The Assembly name is PlaySound, the folder name is Sounds,
            // and the resource filename has been provided as a parameter.
            using (Stream sound = Assembly.GetExecutingAssembly().GetManifestResourceStream("PlaySound.Sounds." + ResourceFilename))
            {
                // Make sure we found the resource
                if (sound == null)
                {
                    MessageBox.Show("Unknown sound file: " + ResourceFilename);
                }
                else
                {
                    // Read the resource into a byte array, as this is what we need to pass
                    // to the PlaySound function
                    soundData = new byte[(int)(sound.Length)];
                    sound.Read(soundData, 0, (int)(sound.Length));

                    // Playing asynchronously?
                    if (ASync)
                    {
                        // Play the sound
                        PlaySound(soundData, IntPtr.Zero, SoundFlags.SND_MEMORY | SoundFlags.SND_ASYNC);
                    }
                    else
                    {
                        // Change the label to indicate that the sound is playing
                        lblTitle.Text = "Playing sound...";
                        lblTitle.Refresh();
                        // Play the sound synchronously
                        PlaySound(soundData, IntPtr.Zero, SoundFlags.SND_MEMORY | SoundFlags.SND_SYNC);
                        // Reset the label text now that we have finished
                        lblTitle.Text = "Please select a sound to play:";
                    }
                }
            }
        }

        /// <summary>
        /// Play a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        private void PlaySoundFile(string Filename, bool ASync)
        {
            // Playing asynchronously?
            if (ASync)
            {
                // Play the sound
                PlaySound(Filename, IntPtr.Zero, SoundFlags.SND_FILENAME | SoundFlags.SND_ASYNC);
            }
            else
            {
                // Change the label to indicate that the sound is playing
                lblTitle.Text = "Playing sound...";
                lblTitle.Refresh();
                // Play the sound synchronously
                PlaySound(Filename, IntPtr.Zero, SoundFlags.SND_FILENAME | SoundFlags.SND_SYNC);
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