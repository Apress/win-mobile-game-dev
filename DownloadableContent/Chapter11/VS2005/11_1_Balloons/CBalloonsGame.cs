/**
 * 
 * Balloons
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * 2-D graphics using OpenGL ES
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace Balloons
{
    class CBalloonsGame : GameEngineCh11.CGameEngineOpenGLBase
    {

        // Keep track of whether OpenGL has been initialized
        //private bool _glInitialized = false;

        // The coordinate system in use within the game
        private RectangleF _orthoCoords;

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


        //-------------------------------------------------------------------------------------
        // Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CBalloonsGame(Form gameForm)
            : base(gameForm)
        {
        }



        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return the game coordinate dimensions
        /// </summary>
        public RectangleF OrthoCoords
        {
            get { return _orthoCoords; }
        }


        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Prepare the game
        /// </summary>
        public override void Prepare()
        {
            // Allow the base class to do its work
            base.Prepare();

            // Make sure OpenGL is initialized before we interact with it
            if (OpenGLInitialized)
            {
                // Set properties
                BackgroundColor = Color.SkyBlue;

                // Load the graphics if not already loaded
                if (GameGraphics.Count == 0)
                {
                    // Load our textures
                    Assembly asm = Assembly.GetExecutingAssembly();
                    Texture tex = Texture.LoadStream(asm.GetManifestResourceStream("Balloons.Graphics.Balloon.png"), true);
                    GameGraphics.Add("Balloon", tex);
                }
            }
        }


        /// <summary>
        /// Override the viewport initialization function to set our own viewport.
        /// We will use an orthographic projection instead of a perspective projection.
        /// This simplifies the positioning of our graphics.
        /// </summary>
        protected override void InitGLViewport()
        {
            float orthoWidth;
            float orthoHeight;

            // Let the base class do anything it needs.
            // This will create the OpenGL viewport and a default projection matrix.
            base.InitGLViewport();

            // Switch OpenGL into Projection mode so that we can set our own projection matrix.
            gl.MatrixMode(gl.GL_PROJECTION);
            // Load the identity matrix
            gl.LoadIdentity();

            // Set the width to be whatever we want
            orthoWidth = 4;
            // Set the height to be the appropriate value based on the aspect
            // ratio of the screen.
            orthoHeight = (float)GameForm.Height / (float)GameForm.Width * orthoWidth;

            // Set the orthoCoords rectangle. This can be retrieved by other
            // code in the game to determine the coordinate that are in use.
            _orthoCoords = new RectangleF(-orthoWidth / 2, orthoHeight / 2, orthoWidth, -orthoHeight);

            // Apply an orthographic projection. Keep (0, 0) in the center of the screen.
            // The x axis will range from -2 to +2, the y axis from whatever values have
            // been calculated based on the screen dimensions.
            gl.Orthof(-orthoWidth / 2, orthoWidth / 2, -orthoHeight / 2, orthoHeight / 2, -1, 1);

            // Switch OpenGL back to ModelView mode so that we can transform objects rather than
            // the projection matrix.
            gl.MatrixMode(gl.GL_MODELVIEW);
            // Load the identity matrix.
            gl.LoadIdentity();
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public override void Reset()
        {
            // Allow the base class to do its work
            base.Reset();

            // Make sure OpenGL has been initialized
            if (!OpenGLInitialized) return;

            // Clear any existing game objects.
            GameObjects.Clear();
        }

        /// <summary>
        /// Update the game
        /// </summary>
        public override void Update()
        {
            // Allow the base class to do its work
            base.Update();

            // Do we have less then 20 balloons active at the moment?
            if (GameObjects.Count < 20)
            {
                // Yes, so add a new balloon to the game engine
                GameObjects.Add(new CObjBalloon(this));
                // Sort the objects. The balloons have an override of CompareTo
                // which will sort those with the smallest size to the beginning
                // of the list, so that they render first and so appears to be
                // at the back.
                GameObjects.Sort();
            }
        }

        /// <summary>
        /// Test whether the supplied x and y screen coordinate is within one of the balloons
        /// </summary>
        /// <param name="x">The x coordinate to test</param>
        /// <param name="y">The y coordinate to test</param>
        unsafe public void TestHit(int x, int y)
        {
            float posX, posY, posZ;
            CObjBalloon balloon;

            // Convert the screen coordinate into a coordinate within OpenGL's
            // coordinate system.
            if (UnProject(x, y, 0, out posX, out posY, out posZ))
            {
                // Loop for each game object.
                // Note that we loop backwards so that objects at the front
                // are considered before those behind.
                for (int i = GameObjects.Count - 1; i >= 0; i--)
                {
                    // Is this object a balloon?
                    if (GameObjects[i] is CObjBalloon)
                    {
                        // Cast the object as a balloon
                        balloon = (CObjBalloon)GameObjects[i];
                        // See if the balloon registers this position as a hit
                        if (balloon.TestHit(posX, posY))
                        {
                            // It does. Terminate the balloon so that it disappears
                            balloon.Terminate = true;
                            PlayPopSound();
                            // Stop looping so that we only pop the frontmost balloon
                            // at this location.
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Play a randon "pop" sound effect
        /// </summary>
        private void PlayPopSound()
        {
            // Which effect shall we play?
            switch (Random.Next(0, 3))
            {
                case 0: PlaySoundEmbedded("Pop1.wav", true); break;
                case 1: PlaySoundEmbedded("Pop2.wav", true); break;
                case 2: PlaySoundEmbedded("Pop3.wav", true); break;
            }
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
            using (Stream sound = Assembly.GetExecutingAssembly().GetManifestResourceStream("Balloons.Sounds." + ResourceFilename))
            {
                // Make sure we found the resource
                if (sound != null)
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
                        // Play the sound synchronously
                        PlaySound(soundData, IntPtr.Zero, SoundFlags.SND_MEMORY | SoundFlags.SND_SYNC);
                    }
                }
            }
        }

    }
}
