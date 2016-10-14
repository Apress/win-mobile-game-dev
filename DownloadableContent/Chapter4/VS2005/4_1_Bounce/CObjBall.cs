/**
 * 
 * Bounce
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A simple example of creating a game project based upon the GameEngine.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Bounce
{
    class CObjBall : GameEngineCh4.CGameObjectGDIBase
    {
        // The velocity of the ball in the x and y axes
        private float _xadd = 0;
        private float _yadd = 0;

        // Our reference to the game engine.
        // Note that this is typed as CBounceGame, not as CGameEngineGDIBase
        private CBounceGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CBounceGame class as a parameter.
        /// </summary>
        public CObjBall(CBounceGame gameEngine) : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set the width and height of the ball. Retrieve these from the loaded bitmap.
            Width = _myGameEngine.GameGraphics["Ball"].Width;
            Height = _myGameEngine.GameGraphics["Ball"].Height;

            // Set a random position for the ball's starting location.
            XPos = _myGameEngine.Random.Next(0, (int)(_myGameEngine.GameForm.Width - Width));
            YPos = _myGameEngine.Random.Next(0, (int)(_myGameEngine.GameForm.Height / 2));

            // Set a random x velocity. Keep looping until we get a non-zero value.
            while (_xadd == 0)
            {
                _xadd = _myGameEngine.Random.Next(-4, 4);
            }
            // Set a random y velocity. Zero values don't matter here as this value will
            // be affected by our simulation of gravity.
            _yadd = (_myGameEngine.Random.Next(5, 15)) / 10;
        }



    /// <summary>
    /// Render the ball to the provided Graphics object
    /// </summary>
    public override void Render(Graphics gfx)
    {
        base.Render(gfx);
        
        // Create an ImageAttributes object so that we can set a transparency color key
        ImageAttributes imgAttributes = new ImageAttributes();

        // The color key is Fuchsia (Red=255, Green=0, Blue=255).
        // This is the color that is used within the ball graphic to
        // represent the transparent background.
        imgAttributes.SetColorKey(Color.Fuchsia, Color.Fuchsia);
        // Draw the ball to the current render rectangle
        gfx.DrawImage(_myGameEngine.GameGraphics["Ball"], GetRenderRectangle(), 0, 0, Width, Height, GraphicsUnit.Pixel, imgAttributes);
    }

        /// <summary>
        /// Update the state of the ball
        /// </summary>
        public override void Update()
        {
            // Allow the base class to perform any processing it needs
            base.Update();

            // Add the ball's velocity in each axis to its position
            XPos += _xadd;
            YPos += _yadd;

            // If we have passed the left edge of the window, reset back to the edge
            // and reverse the x velocity.
            if (XPos < 0)
            {
                _xadd = -_xadd;
                XPos = 0;
            }
            // If we have passed the right edge of the window, reset back to the edge
            // and reverse the x velocity.
            if (XPos > _myGameEngine.GameForm.ClientSize.Width - Width)
            {
                _xadd = -_xadd;
                XPos = _myGameEngine.GameForm.ClientSize.Width - Width;
            }
            // If we have passed the bottom of the form, reset back to the bottom
            // and reverse the y velocity. This time we also reduce the velocity
            // slightly to simulate drag. The ball will eventually run out of
            // vertical velocity and stop bouncing.
            if (YPos + Height > _myGameEngine.GameForm.ClientSize.Height)
            {
                _yadd = _yadd * -0.9f;
                YPos = _myGameEngine.GameForm.ClientSize.Height - Height;
            }

            // This is our very simple gravity simulation.
            // We just modify the vertical velocity to move it slightly into
            // the downward direction.
            _yadd += 0.25f;
        }

    }
}
