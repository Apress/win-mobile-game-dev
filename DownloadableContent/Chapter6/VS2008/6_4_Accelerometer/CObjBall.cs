/**
 * 
 * Accelerometer
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of keyboard and button handling using both Form events and also the
 * GetAsyncKeyState function.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Accelerometer
{
    internal class CObjBall : GameEngineCh6.CGameObjectGDIBase
    {
        // Our reference to the game engine.
        // Note that this is typed as CAccelerometerGame, not as CGameEngineGDIBase
        private CAccelerometerGame _myGameEngine;

        // The velocity of the object
        private float _xVelocity = 0;
        private float _yVelocity = 0;

        /// <summary>
        /// Constructor. Require an instance of our own CAccelerometerGame class as a parameter.
        /// </summary>
        public CObjBall(CAccelerometerGame gameEngine) : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set the width and height of the ball.
            Width = _myGameEngine.GameGraphics["Ball"].Width;
            Height = _myGameEngine.GameGraphics["Ball"].Height;

            // Set a random position for the ball's starting location.
            XPos = _myGameEngine.Random.Next(0, (int)(_myGameEngine.GameForm.Width - Width));
            YPos = _myGameEngine.Random.Next(0, (int)(_myGameEngine.GameForm.Height - Height));
        }


        //-------------------------------------------------------------------------------------
        // Property access

        public float XVelocity
        {
            get
            {
                return _xVelocity;
            }
            set
            {
                _xVelocity = value;
            }
        }

        public float YVelocity
        {
            get
            {
                return _yVelocity;
            }
            set
            {
                _yVelocity = value;
            }
        }


        //-------------------------------------------------------------------------------------
        // Object functions

        /// <summary>
        /// Render the ball to the provided Graphics object
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Create an ImageAttributes object so that we can set a transparency color key
            ImageAttributes imgAttributes = new ImageAttributes();

            // The color key is Fuchsia (Red=255, Green=0, Blue=255).
            // This is the color that is used within the ball graphic to
            // represent the transparent background.
            imgAttributes.SetColorKey(Color.Fuchsia, Color.Fuchsia);
            // Draw the ball to the current render rectangle
            gfx.DrawImage(_myGameEngine.GameGraphics["Ball"], GetRenderRectangle(interpFactor), 0, 0, Width, Height, GraphicsUnit.Pixel, imgAttributes);

        }


        /// <summary>
        /// Provide all of the functionality necessary to implement
        /// motion within the object.
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Apply any velocity set for this object
            if (_xVelocity != 0 || _yVelocity != 0)
            {
                XPos += _xVelocity;
                YPos += _yVelocity;
                // Apply friction to the velocity
                _xVelocity *= 0.98f;
                _yVelocity *= 0.98f;
                // Once the velocity falls to a small enough value, cancel it completely
                if (Math.Abs(_xVelocity) < 0.05f) _xVelocity = 0;
                if (Math.Abs(_yVelocity) < 0.05f) _yVelocity = 0;

                // Keep within the bounds of the form and bounce from the edges
                if (XPos < 0)
                {
                    XPos = 0;
                    _xVelocity *= -0.5f;
                }
                if (XPos > GameEngine.GameForm.ClientRectangle.Width - Width)
                {
                    XPos = GameEngine.GameForm.ClientRectangle.Width - Width;
                    _xVelocity *= -0.5f;
                }
                if (YPos < 0)
                {
                    YPos = 0;
                    _yVelocity *= -0.5f;
                }
                if (YPos > GameEngine.GameForm.ClientRectangle.Height - Height)
                {
                    YPos = GameEngine.GameForm.ClientRectangle.Height - Height;
                    _yVelocity *= -0.5f;
                }

            }
        }    

    }
}
