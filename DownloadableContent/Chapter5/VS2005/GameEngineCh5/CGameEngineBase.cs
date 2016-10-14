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

namespace GameEngineCh5
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
        /// The total number of updates that have taken place since the game launched
        /// </summary>
        private int _updateCount;

        /// <summary>
        /// Timer functions
        /// </summary>
        /// <returns></returns>
        [DllImport("coredll.dll", EntryPoint = "QueryPerformanceFrequency")]
        private static extern bool QueryPerformanceFrequency(out long countsPerSecond);
        [DllImport("coredll.dll", EntryPoint = "QueryPerformanceCounter")]
        private static extern bool QueryPerformanceCounter(out long count);
        /// <summary>
        /// The high performance counter's frequency, if available, or zero if not.
        /// Dividing the timer values by this will give us a value in seconds.
        /// </summary>
        private float _timerFrequency;
        /// <summary>
        /// The time value that was returned in the previous timer call
        /// </summary>
        private long _timerLastTime;
        /// <summary>
        /// The amount of time (in seconds) that has elapsed since the last render
        /// </summary>
        private float _renderElapsedTime;
        /// <summary>
        /// The amount of time (in seconds) that has elapsed since the last game update
        /// </summary>
        private float _updateElapsedTime;
        /// <summary>
        /// The number of game updates to perform per second
        /// </summary>
        private int _updatesPerSecond;
        /// <summary>
        /// The duration (in seconds) for each game update.
        /// This is calculated as 1 / _updatesPerSecond
        /// </summary>
        private float _updateTime;

        /// <summary>
        /// The amount of time (in seconds) since we last measured the frames per second
        /// </summary>
        private float _fpsElapsedTime = 0;
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

            // Default to 30 updates per second
            UpdatesPerSecond = 30;

            // Initialise the game timer
            InitTimer();

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
        /// Sets or returns the desired number of updates per second at which the game engine should run.
        /// </summary>
        public int UpdatesPerSecond
        {
            get
            {
                return _updatesPerSecond;
            }
            set
            {
                // Keep the value within a sensible range -- we'll limit it to between
                // 1 and 200 updates per second.
                if (value < 1) value = 1;
                if (value > 200) value = 200;
                // Set the updates per second value.
                _updatesPerSecond = value;
                // Determine the time (in seconds) that each update will last for.
                // This is simply 1 second divided by the number of updates within each second.
                _updateTime = (1.0f / _updatesPerSecond);
            }
        }


        /// <summary>
        /// Returns the number of game updates that have occurred since game processing began
        /// </summary>
        public int UpdateCount
        {
            get
            {
                return _updateCount;
            }
        }

        /// <summary>
        /// Returns the amount of time that has elapsed since the last render occurred
        /// </summary>
        public float TimeSinceLastRender
        {
            get
            {
                return _renderElapsedTime;
            }
        }

        /// <summary>
        /// Return a value indicating whether this is a smartphone, or a touch screen device.
        /// </summary>
        public bool IsSmartphone
        {
            get
            {
                return CGameFunctions.IsSmartphone();
            }
        }


        //-------------------------------------------------------------------------------------
        // Timer functions

        /// <summary>
        /// Initialise the system timer that we will use for our game
        /// </summary>
        private void InitTimer()
        {
            long Frequency;

            // Do we have access to a high-performance timer?
            if (QueryPerformanceFrequency(out Frequency))
            {
                // High-performance timer available -- calculate the time scale value
                _timerFrequency = Frequency;
                // Obtain the current time
                QueryPerformanceCounter(out _timerLastTime);
            }
            else
            {
                // No high-performance timer is available so we'll use tick counts instead
                _timerFrequency = 0;
                // Obtain the current time
                _timerLastTime = Environment.TickCount;
            }

            // We are exactly at the beginning of the next game update
            _updateElapsedTime = 0;
        }

        /// <summary>
        /// Determine how much time (in seconds) has elapsed since the previous call to GetElapsedTime().
        /// </summary>
        /// <returns></returns>
        protected float GetElapsedTime()
        {
            long newTime;
            float fElapsed;

            // Do we have a high performance timer?
            if (_timerFrequency > 0)
            {
                // Yes, so get the new performance counter
                QueryPerformanceCounter(out newTime);
                // Scale accordingly to give us a value in seconds
                fElapsed = (float)(newTime - _timerLastTime) / _timerFrequency;

            }
            else
            {
                // No, so get the tick count
                newTime = Environment.TickCount;
                // Scale from 1000ths of a second to seconds
                fElapsed = (float)(newTime - _timerLastTime) / 1000;
            }

            // Save the new time so that we can query against it in the next call
            _timerLastTime = newTime;

            // Don't allow negative times
            if (fElapsed < 0) fElapsed = 0;
            // Don't allow excessively large times (cap at 0.25 seconds)
            if (fElapsed > 0.25f) fElapsed = 0.25f;


            // Update the frames per second
            _fpsElapsedTime += fElapsed;
            _fpsFrameCount += 1;
            if (_fpsElapsedTime >= 1)
            {
                // A second has elapsed, so store the number of frames that were rendered during that second.
                _fpsLastFPS = _fpsFrameCount;
                // Reset the FPS information.
                _fpsElapsedTime = 0;
                _fpsFrameCount = 0;
            }

            // Return the amount of elapsed time
            return fElapsed;
        }

        /// <summary>
        /// Return the most recent frames-per-second measurement
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
        /// Virtual function to allow the game to be reset to its initial state
        /// (i.e., start a new game)
        /// </summary>
        public virtual void Reset()
        {
            // Nothing to do for the base class.
        }

        /// <summary>
        /// Virtual function to allow the game's objects to be rendered in the game window.
        /// </summary>
        /// <param name="gfx"></param>
        public virtual void Render(float interpFactor)
        {
            // Nothing to do for the base class.
        }

        /// <summary>
        /// Virtual function to allow the game itself and all objects within the game to be updated.
        /// </summary>
        public virtual void Advance()
        {
            // Work out how much time has elaspsed since the last call to Advance
            _renderElapsedTime = GetElapsedTime();
            // Add this to any partial elapsed time that was already present from the last update.
            _updateElapsedTime += _renderElapsedTime;

            // Has sufficient time has passed for us to render a new frame?
            while (_updateElapsedTime >= _updateTime)
            {
                // Increment the update counter
                _updateCount += 1;

                // Update the game
                // Note that this performs a discrete full game update.
                Update();

                // Update all objects that remain within our collection.
                // Once again, this performs a discrete full game update.
                foreach (CGameObjectBase gameObj in _gameObjects)
                {
                    // Ignore objects that have been flagged as terminated
                    // (We need to retain these however until the next render to ensures
                    // that the space they leave behind is re-rendered properly)
                    if (!gameObj.Terminate)
                    {
                        // Update the object's last position (copy its current position)
                        gameObj.UpdatePreviousPosition();

                        // Perform any update processing required upon the object
                        gameObj.Update();
                    }
                }

                // Subtract the frame time from the elapsed time
                _updateElapsedTime -= _updateTime;

                // If we still have elapsed time in excess of the update interval,
                // loop around and process another update.
            }

            // Now that everything is updated, render the scene.
            // Pass the interpolation factor as the proportion of the update time that has elapsed.
            Render(_updateElapsedTime / _updateTime);

            // Remove any objects which have been requested for termination
            RemoveTerminatedObjects();
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
            if ((requiredCaps & Capabilities.TouchScreen) > 0 && IsSmartphone) missingCaps |= Capabilities.TouchScreen;

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
                ret.Append("- requires a Square QVGA screen resolution (240 x 240 pixels or greater).\n");
            else if ((missingCaps & Capabilities.QVGA) > 0)
                ret.Append("- requires a QVGA screen resolution (240 x 320 pixels or greater).\n");
            else if ((missingCaps & Capabilities.WQVGA) > 0)
                ret.Append("- requires a WQVGA screen resolution (240 x 400 pixels or greater).\n");
            else if ((missingCaps & Capabilities.SquareVGA) > 0)
                ret.Append("- requires a Square VGA screen resolution (480 x 480 pixels or greater).\n");
            else if ((missingCaps & Capabilities.VGA) > 0)
                ret.Append("- requires a VGA screen resolution (480 x 640 pixels or greater).\n");
            else if ((missingCaps & Capabilities.WVGA) > 0)
                ret.Append("- requires a WVGA screen resolution (480 x 800 pixels or greater).\n");

            // Check input capabilities
            if ((missingCaps & Capabilities.TouchScreen) > 0)
                ret.Append("- requires a touch-screen.\n");

            // Check O/S version. Again we'll report only the lowest reported problem.
            if ((missingCaps & Capabilities.WindowsMobile2003SE) > 0)
                ret.Append("- requires Windows Mobile 2003SE or later.\n");
            else if ((missingCaps & Capabilities.WindowsMobile5) > 0)
                ret.Append("- requires Windows Mobile 5.0 or later.\n");
            else if ((missingCaps & Capabilities.WindowsMobile6) > 0)
                ret.Append("- requires Windows Mobile 6.0 or later.\n");

            // Return the description we have built
            return ret.ToString();
        }

    }
}
