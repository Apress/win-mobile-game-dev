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
using System.IO;
using System.Reflection;
using System.Threading;

namespace SoundPlayer
{
    class SoundPlayerWrapper
    {
        // Our local SoundPlayer instance
        private System.Media.SoundPlayer _soundPlayer;

        /// <summary>
        /// Play a sound contained within an embedded resource
        /// </summary>
        /// <param name="ResourceAsm">The assembly containing the resource to play</param>
        /// <param name="ResourceName">The name of the resource to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        public void PlaySoundEmbedded(Assembly ResourceAsm, string ResourceName, bool ASync)
        {
            // Clear up any existing sound that may be playing
            DisposeSoundPlayer();

            // Create and verify the sound stream
            Stream soundStream = ResourceAsm.GetManifestResourceStream(ResourceName);
            if (soundStream != null)
            {
                // Create the SoundPlayer passing the stream to its constructor
                _soundPlayer = new System.Media.SoundPlayer(soundStream);

                // Play the sound
                if (ASync)
                {
                    // Play asynchronously by using a separate thread
                    Thread playThread = new Thread(PlaySoundAndDispose);
                    playThread.Start();
                }
                else
                {
                    // Play synchronously
                    PlaySoundAndDispose();
                }
            }
        }


        /// <summary>
        /// Play a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="ASync">A flag indicating whether to play asynchronously</param>
        public void PlaySoundFile(string Filename, bool ASync)
        {
            // Clear up any existing sound that may be playing
            DisposeSoundPlayer();

            // Create the SoundPlayer passing the filename to its constructor
            _soundPlayer = new System.Media.SoundPlayer(Filename);

            // Play the sound
            if (ASync)
            {
                // Play asynchronously by using a separate thread
                Thread playThread = new Thread(PlaySoundAndDispose);
                playThread.Start();
            }
            else
            {
                // Play synchronously
                PlaySoundAndDispose();
            }
        }

        /// <summary>
        /// Play the loaded sound synchronously and then dispose of the player and release its resources
        /// </summary>
        private void PlaySoundAndDispose()
        {
            _soundPlayer.PlaySync();
            DisposeSoundPlayer();
        }

        /// <summary>
        /// Dispose of the sound player and release its resources
        /// </summary>
        private void DisposeSoundPlayer()
        {
            // Make sure we have a player to dispose
            if (_soundPlayer != null)
            {
                // Stop any current playback
                _soundPlayer.Stop();
                // If we have a stream, dispose of it too
                if (_soundPlayer.Stream != null) _soundPlayer.Stream.Dispose();
                // Dispose of the player
                _soundPlayer.Dispose();
                // Remove the object reference so that we cannot re-use it
                _soundPlayer = null;
            }
        }

    }
}
