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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace AygShell
{
    class AygShellSoundPlayer
    {

        [DllImport("aygshell.dll")]
        static extern int SndOpen(string pszSoundFile, ref IntPtr phSound);

        [DllImport("aygshell.dll")]
        static extern int SndPlayAsync(IntPtr hSound, int dwFlags);

        [DllImport("aygshell.dll")]
        static extern int SndClose(IntPtr hSound);

        [DllImport("aygshell.dll")]
        static extern int SndStop(int SoundScope, IntPtr hSound);

        [DllImport("aygshell.dll")]
        static extern int SndPlaySync(string pszSoundFile, int dwFlags);

        // The SoundScope value to pass to SndStop
        const int SND_SCOPE_PROCESS = 0x1;

        // The filename of the sound to play
        private string _soundFile;

        /// <summary>
        /// Play a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        public void PlaySoundFile(string Filename, bool ASync)
        {
            // Store the sound filename
            _soundFile = Filename;

            // Play the sound
            if (ASync)
            {
                // Play asynchronously by using a separate thread
                Thread playThread = new Thread(PlaySound);
                playThread.Start();
            }
            else
            {
                // Play synchronously
                PlaySound();
            }
        }

        /// <summary>
        /// Play the specified sound synchronously
        /// </summary>
        private void PlaySound()
        {
            SndPlaySync(_soundFile, 0);
        }

        /// <summary>
        /// Stop all sounds that are currently playing
        /// </summary>
        public static void StopAllSounds()
        {
            SndStop(SND_SCOPE_PROCESS, IntPtr.Zero);
        }

    }
}
