/**
 * 
 * DragsAndSwipes
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates dragging and throwing game objects.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;

namespace DragsAndSwipes
{
    /// <summary>
    /// CObjSelectable is an abstract class that adds a "Selected" property to the
    /// CGameObjectGDIBase class.
    /// </summary>
    internal abstract class CObjDraggableBase : GameEngineCh6.CGameObjectGDIBase
    {
        // Is this object currently selected by the user?
        private bool _selected = false;

        // The friction to apply to kinetic object movement (1 == virtually none, 99 == extremely high).
        private float _kineticFriction = 10;
        // The kinetic velocity of the object,
        private float _xVelocity = 0;
        private float _yVelocity = 0;
        // The most recent positions of the object -- used to calculate the direction
        // for kinetic movement of the object.
        private Queue<Point> _dragPositions = new Queue<Point>();
        // The number of points we want to track in the dragPositions queue.
        private const int _DraggableQueueSize = 5;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CObjDraggableBase(CDraggableObjectGame gameEngine)
            : base(gameEngine)
        {
        }

        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Set or return a value indicating whether this object is selected
        /// </summary>
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
            }
        }

        /// <summary>
        /// Set or return the friction to apply when kinetic movement is applied to
        /// objects.
        /// </summary>
        /// <remarks>Must be in the range of 1 to 100. A value of 1 provides virtually
        /// no friction at all, whereas 100 will stop objects moving immediately.
        /// Comfortable user interface values are usually in the 10-20 range.</remarks>
        public float KineticFriction
        {
            get
            {
                return _kineticFriction;
            }
            set
            {
                _kineticFriction = value;
                // Ensure the value is within acceptable range
                if (_kineticFriction < 1) _kineticFriction = 1;
                if (_kineticFriction > 100) _kineticFriction = 100;
            }
        }

        //-------------------------------------------------------------------------------------
        // Object functions

        /// <summary>
        /// Prepare the object for kinetic motion. Call this when the object enters drag mode.
        /// </summary>
        internal void InitializeKineticDragging()
        {
            // Clear the queue of recent object positions
            _dragPositions = new Queue<Point>();

            // Cancel any existing kinetic movement
            _xVelocity = 0;
            _yVelocity = 0;
        }

        /// <summary>
        /// Apply kinetic motion to the object. Call this when the object leaves drag mode.
        /// </summary>
        internal void ApplyKineticDragging()
        {
            Point first, last;

            // Make sure there is something in the point queue...
            // We need at least 2 points in order to determine the movement direction
            if (_dragPositions.Count < 2)
            {
                // There is not, so no kinetic force is available
                return;
            }

            // Retrieve the oldest position from the drag position queue
            first = _dragPositions.Dequeue();
            // Remove all the other positions until we obtain the most recent
            do
            {
                last = _dragPositions.Dequeue();
            } while (_dragPositions.Count > 0);

            // Set the x and y velocity based upon the difference between these
            // two points. As these represent the last five object positions, divide the
            // distance by one less than this (four) to maintain the same actual speed.
            _xVelocity = (last.X - first.X) / (_DraggableQueueSize - 1);
            _yVelocity = (last.Y - first.Y) / (_DraggableQueueSize - 1);
        }

    /// <summary>
    /// Provide all of the functionality necessary to implement kinetic
    /// motion within the object.
    /// </summary>
    public override void Update()
    {
        base.Update();

        // Write the current object position into the drag positions queue.
        // This will allow us to apply kinetic movement to the object
        // when it is released from dragging.
        _dragPositions.Enqueue(new Point((int)XPos, (int)YPos));
        if (_dragPositions.Count > _DraggableQueueSize) _dragPositions.Dequeue();

        // Apply any existing kinetic velocity set for this object
        if (_xVelocity != 0 || _yVelocity != 0)
        {
            XPos += _xVelocity;
            YPos += _yVelocity;
            // Apply friction to the velocity
            _xVelocity *= (1 - (KineticFriction / 100));
            _yVelocity *= (1 - (KineticFriction / 100));
            // Once the velocity falls to a small enough value, cancel it completely
            if (Math.Abs(_xVelocity) < 0.25f) _xVelocity = 0;
            if (Math.Abs(_yVelocity) < 0.25f) _yVelocity = 0;
        }
    }


    }
}
