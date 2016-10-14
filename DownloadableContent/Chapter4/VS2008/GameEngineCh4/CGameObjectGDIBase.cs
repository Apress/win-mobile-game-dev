/**
 * 
 * CGameObjectGDIBase
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;

namespace GameEngineCh4
{
    public abstract class CGameObjectGDIBase : CGameObjectBase
    {
        // Object size
        private int _width;
        private int _height;
        // Object state
        private bool _hasMoved = false;

        private Rectangle _previousRenderRect;

        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CGameObjectGDIBase(CGameEngineBase gameEngine)
            : base(gameEngine)
        {
            // No constructor code required
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Provide internal access to the object's previous render rectangle
        /// </summary>
        internal Rectangle PreviousRenderRect
        {
            get
            {
                return _previousRenderRect;
            }
            set
            {
                _previousRenderRect = value;
            }
        }

        /// <summary>
        /// Sets or returns the X position of the object
        /// </summary>
        public override float XPos
        {
            get
            {
                return base.XPos;
            }
            set
            {
                // If the position has changed then mark this object as moved so that
                // it gets re-rendered
                if (base.XPos != value)
                {
                    base.XPos = value;
                    HasMoved = true;
                }
            }
        }

        /// <summary>
        /// Sets or returns the Y position of the object
        /// </summary>
        public override float YPos
        {
            get
            {
                return base.YPos;
            }
            set
            {
                // If the position has changed then mark this object as moved so that
                // it gets re-rendered
                if (base.YPos != value)
                {
                    base.YPos = value;
                    HasMoved = true;
                }
            }
        }

        /// <summary>
        /// Sets or returns the width of the object
        /// </summary>
        public virtual int Width
        {
            get
            {
                return _width;
            }
            set
            {
                // If the width has changed then mark this object as moved so that
                // it gets re-rendered
                if (_width != value)
                {
                    HasMoved = true;
                    _width = value;
                }
            }
        }

        /// <summary>
        /// Sets or returns the height of the object
        /// </summary>
        public virtual int Height
        {
            get
            {
                return _height;
            }
            set
            {
                // If the height has changed then mark this object as moved so that
                // it gets re-rendered
                if (Height != value)
                {
                    HasMoved = true;
                    _height = value;
                }
            }
        }

        /// <summary>
        /// Determine whether the object has moved since this property was last reset
        /// </summary>
        public bool HasMoved
        {
            get
            {
                return _hasMoved;
            }
            set
            {
                _hasMoved = value;
            }
        }


        //-------------------------------------------------------------------------------------
        // Game engine operations

        /// <summary>
        /// Determine whether any of our object state has changed in a way that would
        /// require the object to be redrawn. If so, set the HasMoved flag to true.
        /// </summary>
        internal virtual void CheckIfMoved()
        {
            if (XPos != LastXPos) HasMoved = true;
            if (YPos != LastYPos) HasMoved = true;
            if (ZPos != LastZPos) HasMoved = true;
            if (IsNew) HasMoved = true;
            if (Terminate) HasMoved = true;
        }

        /// <summary>
        /// Determine the rectangle inside which the render of the object will take place at its
        /// current position and size.
        /// </summary>
        /// <returns></returns>
        public virtual Rectangle GetRenderRectangle()
        {
            return new Rectangle((int)XPos, (int)YPos, (int)Width, (int)Height);
        }

    }
}
