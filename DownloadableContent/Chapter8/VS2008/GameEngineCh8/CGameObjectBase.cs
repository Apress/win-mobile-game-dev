/**
 * 
 * CGameObjectBase
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace GameEngineCh8
{
    public abstract class CGameObjectBase
    {

        // A reference to the game engine that contains us
        private CGameEngineBase _gameEngine;

        // Object position
        private float _xpos = 0;
        private float _ypos = 0;
        private float _zpos = 0;
        private float _lastxpos = float.MinValue;
        private float _lastypos = float.MinValue;
        private float _lastzpos = float.MinValue;
        // Object state
        private bool _isnew = true;
        private bool _terminate = false;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CGameObjectBase(CGameEngineBase gameEngine)
        {
            // Store our game engine reference
            _gameEngine = gameEngine;
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Return our internal CGameEngineBase reference
        /// </summary>
        protected CGameEngineBase GameEngine
        {
            get
            {
                return _gameEngine;
            }
        }

        /// <summary>
        /// Sets or returns the current x position of the object
        /// </summary>
        public virtual float XPos
        {
            get
            {
                return _xpos;
            }
            set
            {
                _xpos = value;
            }
        }
        /// <summary>
        /// Sets or returns the current y position of the object
        /// </summary>
        public virtual float YPos
        {
            get
            {
                return _ypos;
            }
            set
            {
                _ypos = value;
            }
        }
        /// <summary>
        /// Sets or returns the current z position of the object
        /// </summary>
        public virtual float ZPos
        {
            get
            {
                return _zpos;
            }
            set
            {
                _zpos = value;
            }
        }
        /// <summary>
        /// Sets or returns the previous x position of the object
        /// </summary>
        protected float LastXPos
        {
            get
            {
                return _lastxpos;
            }
            set
            {
                _lastxpos = value;
            }
        }
        /// <summary>
        /// Sets or returns the previous y position of the object
        /// </summary>
        protected float LastYPos
        {
            get
            {
                return _lastypos;
            }
            set
            {
                _lastypos = value;
            }
        }
        /// <summary>
        /// Sets or returns the previous z position of the object
        /// </summary>
        protected float LastZPos
        {
            get
            {
                return _lastzpos;
            }
            set
            {
                _lastzpos = value;
            }
        }

        /// <summary>
        /// Sets or returns a flag indicating whether this object is due for termination
        /// during the next update.
        /// </summary>
        public bool Terminate
        {
            get
            {
                return _terminate;
            }
            set
            {
                _terminate = value;
            }
        }

        /// <summary>
        /// Sets or returns a flag indicating whether this object has been created since
        /// the last update.
        /// </summary>
        public bool IsNew
        {
            get
            {
                return _isnew;
            }
            set
            {
                _isnew = value;
            }
        }



        //-------------------------------------------------------------------------------------
        // Game engine operations

        /// <summary>
        /// Virtual function to allow the object to render itself to the provided Graphics object (the back buffer)
        /// </summary>
        public virtual void Render(Graphics gfx, float interpFactor)
        {
            // Nothing to do by default
        }


        /// <summary>
        /// Virtual function to allow this object to be updated by one step.
        /// </summary>
        public virtual void Update()
        {
            // Nothing to do by default
        }

        /// <summary>
        /// Copy all positional information for the object into the previous position variables
        /// </summary>
        internal virtual void UpdatePreviousPosition()
        {
            // Move all of the current position information into the previous position variables
            LastXPos = XPos;
            LastYPos = YPos;
            LastZPos = ZPos;
        }

        /// <summary>
        /// Retrieve the actual x position of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXPos(float interpFactor)
        {
            // If we have no previous x position then return the current x position
            if (LastXPos == float.MinValue) return XPos;
            // Otherwise interpolate between the previous position and the current position
            return CGameFunctions.Interpolate(interpFactor, XPos, LastXPos);
        }
        /// <summary>
        /// Retrieve the actual y position of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYPos(float interpFactor)
        {
            // If we have no previous y position then return the current y position
            if (LastYPos == float.MinValue) return YPos;
            // Otherwise interpolate between the previous position and the current position
            return CGameFunctions.Interpolate(interpFactor, YPos, LastYPos);
        }
        /// <summary>
        /// Retrieve the actual z position of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZPos(float interpFactor)
        {
            // If we have no previous z position then return the current z position
            if (LastZPos == float.MinValue) return ZPos;
            // Otherwise interpolate between the previous position and the current position
            return CGameFunctions.Interpolate(interpFactor, ZPos, LastZPos);
        }

    }
}
