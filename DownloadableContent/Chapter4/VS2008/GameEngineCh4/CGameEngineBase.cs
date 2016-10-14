/**
 * 
 * CGameEngineBase
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace GameEngineCh4
{
    public abstract class CGameEngineBase
    {

        /// <summary>
        /// Store a reference to the form against which this GameEngine instance is running
        /// </summary>
        private Form _gameForm;

        /// <summary>
        /// Store a collection of game objects that are currently active
        /// </summary>
        private System.Collections.Generic.List<CGameObjectBase> _gameObjects;

        /// <summary>
        /// A system-wide random number generator object
        /// </summary>
        private Random _random;

        /// <summary>
        /// The time at which we last measured the frames per second
        /// </summary>
        private DateTime _fpsLastTime = DateTime.MinValue;
        /// <summary>
        /// The number of frames that have elapsed since we last measured the frames per second
        /// </summary>
        private int _fpsFrameCount = 0;
        /// <summary>
        /// The most recent measurement of the frames per second
        /// </summary>
        private int _fpsLastFPS = 0;


        /// <summary>
        /// Capabilities flags for the CheckCapabilities and ReportMissingCapabilities functions
        /// </summary>
        [Flags()]
        public enum Capabilities
        {
            SquareQVGA = 1,
            QVGA = 2,
            WQVGA = 4,
            SquareVGA = 8,
            VGA = 16,
            WVGA = 32,
            TouchScreen = 64,
            WindowsMobile2003SE = 128,
            WindowsMobile5 = 256,
            WindowsMobile6 = 512
        }


        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor -- require an instance of the form that we will be running against to be provided
        /// </summary>
        /// <param name="f"></param>
        public CGameEngineBase(Form gameForm)
        {
            // Store a reference to the form
            _gameForm = gameForm;

            // Create a new list of game objects
            _gameObjects = new System.Collections.Generic.List<CGameObjectBase>();

            // Prepare the engine
            Prepare();

            // Initialize the game for action
            Reset();
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return our game form object
        /// </summary>
        public Form GameForm
        {
            get
            {
                // Return our game form instance
                return _gameForm;
            }
        }

        /// <summary>
        /// Return our random number generator
        /// </summary>
        public Random Random
        {
            get
            {
                // If the generator has not yet been created, create and seed it
                if (_random == null) _random = new Random();     //DateTime.Now.Millisecond);
                // Return the generator instance
                return _random;
            }
        }

        /// <summary>
        /// Return the most recent FramesPerSecond measurement
        /// </summary>
        public int FramesPerSecond
        {
            get
            {
                return _fpsLastFPS;
            }
        }

        //-------------------------------------------------------------------------------------
        // Game engine operations

        /// <summary>
        /// Virtual function to allow the game to prepare itself for rendering.
        /// This includes loading graphics, setting coordinate systems, etc.
        /// </summary>
        public virtual void Prepare()
        {
            // Nothing to do for the base class.
        }

        /// <summary>
        /// Virtual function to allow the game state within the object to be reset to its initial state.
        /// </summary>
        public virtual void Reset()
        {
            // Nothing to do for the base class.
        }

        /// <summary>
        /// Virtual function to allow the game's objects to be rendered in the game window.
        /// </summary>
        /// <param name="gfx"></param>
        public virtual void Render()
        {
            // Nothing to do for the base class.
        }

        /// <summary>
        /// Virtual function to allow the game itself and all objects within the game to be updated.
        /// </summary>
        public virtual void Advance()
        {
            // Update the game
            Update();

            // Update all objects that remain within our collection.
            // Once again, this performs a discrete full game frame update.
            // No partial frame updates are considered here.
            foreach (CGameObjectBase gameObj in _gameObjects)
            {
                // Ignore objects that have been flagged as terminated
                // (We need to retain these however until the next render to ensures
                // that the space they leave behind is re-rendered properly)
                if (!gameObj.Terminate)
                {
                    // Update the object's last position
                    gameObj.UpdatePreviousPosition();

                    // Perform any update processing required upon the object
                    gameObj.Update();
                }
            }

            // Now that everything is updated, render the scene
            Render();

            // Remove any objects which have been requested for termination
            RemoveTerminatedObjects();

            // Update the FramesPerSecond information
            UpdateFPS();
        }

        private void UpdateFPS()
        {
            if (_fpsLastTime == DateTime.MinValue)
            {
                // The FPS timer has not been initialized.
                // Set it to the current time
                _fpsLastTime = DateTime.Now;
            }
            else
            {
                // Has 1 second passed since we started timing the FPS?
                if (DateTime.Now.Subtract(_fpsLastTime).TotalMilliseconds >= 1000)
                {
                    // It has, so we now know how many frames elapsed in the last second.
                    // Store this value so that the game can retrieve it.
                    _fpsLastFPS = _fpsFrameCount;
                    // Reset the frame count ready for the next 1-second interval
                    _fpsFrameCount = 0;
                    // Reset the timer so we can detect when the next second has elapsed
                    _fpsLastTime = DateTime.Now;
                }
            }
            // A frame has been displayed so update the frame count
            _fpsFrameCount += 1;
        }

        /// <summary>
        /// Virtual function called once per update to allow the game logic to be updated.
        /// </summary>
        public virtual void Update()
        {
            // Nothing to do for the base class.
        }


        //-------------------------------------------------------------------------------------
        // Game object functions


        /// <summary>
        /// Retrieves the list of active game objects
        /// </summary>
        /// <returns></returns>
        public virtual System.Collections.Generic.List<CGameObjectBase> GameObjects
        {
            get
            {
                return _gameObjects;
            }
        }

        /// <summary>
        /// Remove all objects whose Terminate property is set to true.
        /// </summary>
        protected void RemoveTerminatedObjects()
        {
            int i;

            // Locate any game objects whose Terminate flag is True and remove them from the collection.
            // Loop backwards so that modifying the collection doesn't cause us to miss any objects.
            for (i = _gameObjects.Count - 1; i >= 0; i--)
            {
                if (_gameObjects[i].Terminate)
                {
                    _gameObjects.RemoveAt(i);
                }
            }
        }



        //-------------------------------------------------------------------------------------
        // Device capabilities functions

        /// <summary>
        /// Identify required device capabilities that are not available on the current hardware.
        /// </summary>
        /// <param name="requiredCaps">A bit-mask of the required capabilities</param>
        /// <returns>Returns a bit-mask of those capabilities requested which are not available on this device.
        /// If zero is returned then all requested capabilities are available.</returns>
        public Capabilities CheckCapabilities(Capabilities requiredCaps)
        {
            Capabilities missingCaps = 0;

            // Retrieve the actual dimensions of the screen
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            // Ensure we are in portrait mode (taller rather than wide)
            if (screenWidth > screenHeight)
            {
                // Swap the values over
                int temp = screenWidth;
                screenWidth = screenHeight;
                screenHeight = temp;
            }

            // Check each of the screen size capability requirements
            if ((requiredCaps & Capabilities.SquareQVGA) > 0 && (screenWidth < 240 || screenHeight < 240)) missingCaps |= Capabilities.SquareQVGA;
            if ((requiredCaps & Capabilities.QVGA) > 0 && (screenWidth < 240 || screenHeight < 320)) missingCaps |= Capabilities.QVGA;
            if ((requiredCaps & Capabilities.WQVGA) > 0 && (screenWidth < 240 || screenHeight < 400)) missingCaps |= Capabilities.WQVGA;
            if ((requiredCaps & Capabilities.SquareVGA) > 0 && (screenWidth < 480 || screenHeight < 480)) missingCaps |= Capabilities.SquareVGA;
            if ((requiredCaps & Capabilities.VGA) > 0 && (screenWidth < 480 || screenHeight < 640)) missingCaps |= Capabilities.VGA;
            if ((requiredCaps & Capabilities.WVGA) > 0 && (screenWidth < 480 || screenHeight < 800)) missingCaps |= Capabilities.WVGA;

            // Check input capabilities
            if ((requiredCaps & Capabilities.TouchScreen) > 0 && CGameFunctions.IsSmartphone()) missingCaps |= Capabilities.TouchScreen;

            // Check OS version.
            // We can use the Environment.OSVersion.Version values to check this:
            // - WM2003SE is reported as version 4.x
            // - WM5 is reported as version 5.1
            // - WM6 is reported as version 5.2
            if ((requiredCaps & Capabilities.WindowsMobile2003SE) > 0 && Environment.OSVersion.Version.CompareTo(new Version("4.0.0.0")) < 0) missingCaps |= Capabilities.WindowsMobile2003SE;
            if ((requiredCaps & Capabilities.WindowsMobile5) > 0 && Environment.OSVersion.Version.CompareTo(new Version("5.1.0.0")) < 0) missingCaps |= Capabilities.WindowsMobile5;
            if ((requiredCaps & Capabilities.WindowsMobile6) > 0 && Environment.OSVersion.Version.CompareTo(new Version("5.2.0.0")) < 0) missingCaps |= Capabilities.WindowsMobile6;

            // We have now identified those capabilities that are not available.
            // Return this back to the calling procedure
            return missingCaps;
        }

        /// <summary>
        /// Generates a readable description of the capabilities specified in the missingCaps parameter.
        /// </summary>
        /// <param name="missingCaps">The capabilities for which a description is to be generated.</param>
        /// <returns>Returns a string listing the missing capabilities in a readable form.</returns>
        public String ReportMissingCapabilities(Capabilities missingCaps)
        {
            StringBuilder ret = new StringBuilder();

            // First check the resolution capabilities. We'll report only upon the lowest resolution
            // that is present (as this is the one that the game actually needs).
            if ((missingCaps & Capabilities.SquareQVGA) > 0)
                ret.AppendLine("- requires a Square QVGA screen resolution (240 x 240 pixels or greater).");
            else if ((missingCaps & Capabilities.QVGA) > 0)
                ret.AppendLine("- requires a QVGA screen resolution (240 x 320 pixels or greater).");
            else if ((missingCaps & Capabilities.WQVGA) > 0)
                ret.AppendLine("- requires a WQVGA screen resolution (240 x 400 pixels or greater).");
            else if ((missingCaps & Capabilities.SquareVGA) > 0)
                ret.AppendLine("- requires a Square VGA screen resolution (480 x 480 pixels or greater).");
            else if ((missingCaps & Capabilities.VGA) > 0)
                ret.AppendLine("- requires a VGA screen resolution (480 x 640 pixels or greater).");
            else if ((missingCaps & Capabilities.WVGA) > 0)
                ret.AppendLine("- requires a WVGA screen resolution (480 x 800 pixels or greater).");

            // Check input capabilities
            if ((missingCaps & Capabilities.TouchScreen) > 0)
                ret.AppendLine("- requires a touch-screen.");

            // Check O/S version. Again we'll report only the lowest reported problem.
            if ((missingCaps & Capabilities.WindowsMobile2003SE) > 0)
                ret.AppendLine("- requires Windows Mobile 2003SE or later.");
            else if ((missingCaps & Capabilities.WindowsMobile5) > 0)
                ret.AppendLine("- requires Windows Mobile 5.0 or later.");
            else if ((missingCaps & Capabilities.WindowsMobile6) > 0)
                ret.AppendLine("- requires Windows Mobile 6.0 or later.");

            // Return the description we have built
            return ret.ToString();
        }

    }
}
