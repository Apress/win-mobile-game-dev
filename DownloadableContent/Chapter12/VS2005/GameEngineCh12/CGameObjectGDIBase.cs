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

namespace GameEngineCh12
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
        /// current position for specified interpolation factor.
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public virtual Rectangle GetRenderRectangle(float interpFactor)
        {
            Rectangle ret;

            // Generate the rectangle
            ret = new Rectangle((int)GetDisplayXPos(interpFactor), (int)GetDisplayYPos(interpFactor), Width, Height);

            // If we have no previous render rectangle, set it to
            // this one to ensure that the object is re-drawn properly.
            if (_previousRenderRect.IsEmpty) _previousRenderRect = ret;

            // Return the object's render rectangle
            return ret;
        }

        /// <summary>
        /// Determine whether the specified point falls within the boundaries of the object.
        /// </summary>
        /// <param name="interpFactor">The current interpolation factor</param>
        /// <param name="testPoint">The point to test</param>
        /// <remarks>The implementation in this function simply looks to see if the point is within the
        /// object's render rectangle. To perform more sophisticated functionality, the function
        /// can be overridden in the derived object class.</remarks>
        protected internal virtual bool IsPointInObject(float interpFactor, Point testPoint)
        {
            // By default we'll simply see if the point falls within the current render rectangle
            return (GetRenderRectangle(interpFactor).Contains(testPoint));
        }

        /// <summary>
        /// Move this object to the front of all the objects being rendered
        /// </summary>
        public void MoveToFront()
        {
            // Remove the object from the list...
            GameEngine.GameObjects.Remove(this);
            // ...and then re-add it at the end of the list so that it is rendered last
            GameEngine.GameObjects.Add(this);
            // Mark it as moved so that it is redrawn
            this.HasMoved = true;
        }

        /// <summary>
        /// Move this object to the back of all the objects being rendered
        /// </summary>
        public void MoveToBack()
        {
            // Remove the object from the list...
            GameEngine.GameObjects.Remove(this);
            // ...and then re-add it at the start of the list so that it is rendered first
            GameEngine.GameObjects.Insert(0, this);
            // Mark it as moved so that it is redrawn
            this.HasMoved = true;
        }

    }
}
