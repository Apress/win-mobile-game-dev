/**
 * 
 * BassWrapper
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Utility functions using BASS.dll and BASS.NET for playing sound and music files.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Un4seen.Bass;

namespace GemDrops
{
    class BassWrapper : IDisposable
    {

        private Dictionary<string, int> _sounds = new Dictionary<string, int>();

        /// <summary>
        /// Class constructor. Initialize BASS and BASS.NET ready for use.
        /// </summary>
        public BassWrapper()
        {
            // First pass our license details to BASS.NET
            BassNet.Registration("your_username", "your_authentication_code");

            // Now initialize BASS itself and ensure that this succeeds.
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("BASS failed to initialize.");
            }
        }

    /// <summary>
    /// Load a sound contained within an embedded resource
    /// </summary>
    /// <param name="ResourceAsm">The assembly containing the resource to play</param>
    /// <param name="ResourceName">The name of the resource to play</param>
    /// <param name="Max">The maximum number of concurrent plays of this sound</param>
    /// <param name="Loop">If true, the loaded sound will loop when played.</param>
    public void LoadSoundEmbedded(Assembly ResourceAsm, string ResourceName, int Max, bool Loop)
    {
        byte[] soundData;
        int soundHandle = 0;
        BASSFlag flags = BASSFlag.BASS_DEFAULT;

        // Do we already have this sound loaded?
        if (!_sounds.ContainsKey(ResourceName.ToLower()))
        {
            // Retrieve a stream from our embedded sound resource.
            using (Stream soundStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ResourceName))
            {
                // Make sure we found the resource
                if (soundStream != null)
                {
                    // Read the resource into a byte array, as this is what we need to pass
                    // to the BASS_SampleLoad function
                    soundData = new byte[(int)(soundStream.Length)];
                    soundStream.Read(soundData, 0, (int)(soundStream.Length));

                    // Load the sound into BASS
                    // Is it a music file (tracker module)?
                    if (IsMusic(ResourceName))
                    {
                        // Yes, so use the MusicLoad function
                        // Set flags
                        if (Loop) flags |= BASSFlag.BASS_MUSIC_LOOP;
                        // Load the sound
                        soundHandle = Bass.BASS_MusicLoad(soundData, 0, soundData.Length, flags, 44100);
                    }
                    else
                    {
                        // No, so use the SampleLoad function
                        // Set flags
                        if (Loop) flags |= BASSFlag.BASS_SAMPLE_LOOP;
                        // Load the sound
                        soundHandle = Bass.BASS_SampleLoad(soundData, 0, soundData.Length, Max, flags);
                    }

                    // If we have a valid handle, add it to the dictionary
                    if (soundHandle != 0)
                    {
                        _sounds.Add(ResourceName.ToLower(), soundHandle);
                    }
                }
            }
        }
    }

        /// <summary>
        /// Load a sound contained within an external file
        /// </summary>
        /// <param name="Filename">The filename of the file to play</param>
        /// <param name="Max">The maximum number of concurrent plays of this sound</param>
        /// <param name="Loop">If true, the loaded sound will loop when played.</param>
        public void LoadSoundFile(string Filename, int Max, bool Loop)
        {
            int soundHandle;
            BASSFlag flags = BASSFlag.BASS_DEFAULT;

            // Convert the filename to lowercase so that we don't have to care
            // about mismatched capitalization on subsequent calls
            Filename = Filename.ToLower();

            // Do we already have this sound loaded?
            if (!_sounds.ContainsKey(Path.GetFileName(Filename)))
            {
                // No, so we need to load it now.
                // Is it a music file (tracker module)?
                if (IsMusic(Filename))
                {
                    // Yes, so use the MusicLoad function
                    // Set flags
                    if (Loop) flags |= BASSFlag.BASS_MUSIC_LOOP;
                    // Load the sound
                    soundHandle = Bass.BASS_MusicLoad(Filename, 0, 0, flags, 44100);
                }
                else
                {
                    // No, so use the SampleLoad function
                    // Set flags
                    if (Loop) flags |= BASSFlag.BASS_SAMPLE_LOOP;
                    // Load the sound
                    soundHandle = Bass.BASS_SampleLoad(Filename, 0, 0, 3, flags);
                }
                // If we have a valid handle, add it to the dictionary
                if (soundHandle != 0)
                {
                    _sounds.Add(Path.GetFileName(Filename), soundHandle);
                }
            }
        }

        /// <summary>
        /// Check whether the filename provided is for a recognised tracker music file format
        /// </summary>
        /// <param name="Filename"></param>
        /// <returns></returns>
        private bool IsMusic(string Filename)
        {
            switch (Path.GetExtension(Filename).ToLower())
            {
                case ".mod":
                case ".xm":
                case ".it":
                case ".s3m":
                case ".mtm":
                case ".umx":
                case ".mo3":
                    // This is a tracker music file
                    return true;
                default:
                    // This is not a tracker music file
                    return false;
            }
        }

        /// <summary>
        /// Play a previously loaded sound.
        /// </summary>
        /// <param name="SoundName">The Filename or ResourceName used when loading the sound.</param>
        /// <returns>Returns the activated channel handle if the sound began playing,
        /// or zero if the sound could not be started.</returns>
        public int PlaySound(string SoundName)
        {
            return PlaySound(SoundName, 1.0f);
        }
        /// <summary>
        /// Play a previously loaded sound.
        /// </summary>
        /// <param name="SoundName">The Filename or ResourceName used when loading the sound.</param>
        /// <param name="Volume">The volume level for playback (0 = silent, 1 = full volume)</param>
        /// <returns>Returns the activated channel handle if the sound began playing,
        /// or zero if the sound could not be started.</returns>
        public int PlaySound(string SoundName, float Volume)
        {
            int soundHandle = 0;
            int channel = 0;

            // Try to retrieve this using the SoundName as the dictionary key
            if (_sounds.ContainsKey(SoundName.ToLower()))
            {
                // Found it
                soundHandle = _sounds[SoundName.ToLower()];

                // Is this sound for a music track?
                if (IsMusic(SoundName))
                {
                    // For music, the channel handle is the same as the sound handle
                    channel = soundHandle;
                }
                else
                {
                    // Allocate a channel for playback of the sample
                    channel = Bass.BASS_SampleGetChannel(soundHandle, false);
                }
                // Check we have a channel...
                if (channel != 0)
                {
                    // Play the sample
                    if (Volume < 0) Volume = 0;
                    if (Volume > 1) Volume = 1;
                    if (Volume != 1)
                    {
                        Bass.BASS_ChannelSetAttribute(channel, BASSAttribute.BASS_ATTRIB_VOL, Volume);
                    }
                    Bass.BASS_ChannelPlay(channel, false);
                }
            }

            // Return the channel number (if we have one, zero if not)
            return channel;
        }

        /// <summary>
        /// Check to see whether the specified channel is currently playing a sound.
        /// </summary>
        /// <param name="Channel">The handle of the channel to check</param>
        /// <returns>Returns true if a sound is playing on the channel, or False if the
        /// sound has completed or has been paused or stopped.</returns>
        public bool IsChannelPlaying(int Channel)
        {
            return (Bass.BASS_ChannelIsActive(Channel) == BASSActive.BASS_ACTIVE_PLAYING);
        }

        /// <summary>
        /// Pause the playback of all active channels
        /// </summary>
        public void PauseAllSounds()
        {
            Bass.BASS_Pause();
        }

        /// <summary>
        /// Resume the playback of all active channels
        /// </summary>
        public void ResumeAllSounds()
        {
            Bass.BASS_Start();
        }

        /// <summary>
        /// Stop the playback of all active channels.
        /// </summary>
        /// <remarks>After stopping, the channels cannot be resumed,
        /// use PauseAllSounds if you wish to resume.</remarks>
        public void StopAllSounds()
        {
            // Pause and then stop the sound
            Bass.BASS_Pause();
            Bass.BASS_Stop();
            // Now that everything is stopped, restart so that further sounds can still play
            Bass.BASS_Start();
        }

        /// <summary>
        /// Release all resources used by the class
        /// </summary>
        public void Dispose()
        {
            // Release the sounds
            foreach (string soundKey in _sounds.Keys)
            {
                // Is this a music or a sample file?
                if (IsMusic(soundKey))
                {
                    Bass.BASS_MusicFree(_sounds[soundKey]);
                }
                else
                {
                    Bass.BASS_SampleFree(_sounds[soundKey]);
                }
            }
            // Close BASS
            Bass.BASS_Free();
        }

    }
}
