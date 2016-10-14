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
using System.Text;
using OpenGLES;

namespace Balloons
{
    class CObjBalloon : GameEngineCh11.CGameObjectOpenGLBase
    {

        // The size of this balloon
        private float _size;
        // The balloon's color
        private Color _color;
        // A value to help calculate how the balloon drifts across the screen
        private int _drift = 0;

        // Our reference to the game engine.
        private CBalloonsGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjBalloon(CBalloonsGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set a random size between 0.5 and 1.0
            _size =  RandomFloat(0.5f, 1.0f);

            // Set a random position
            // The x position must ensure that the balloon fits entirely within
            // the horizontal extent of the screen.
            XPos = RandomFloat(_myGameEngine.OrthoCoords.Left + _size / 2, _myGameEngine.OrthoCoords.Right - _size / 2);
            // The y position first locates the balloon so that its top is exactly
            // at the bottom edge of the screen, and then subtracts a random
            // value between 0 and 4 to randomize its start position.
            YPos = _myGameEngine.OrthoCoords.Bottom - _size / 2 - RandomFloat(0, 4);

            // Set a random color
            switch (_myGameEngine.Random.Next(0, 5))
            {
                case 0: _color = Color.RoyalBlue; break;
                case 1: _color = Color.Red; break;
                case 2: _color = Color.Yellow; break;
                case 3: _color = Color.Magenta; break;
                case 4: _color = Color.Orange; break;
            }

            // Randomize the drift value
            _drift = _myGameEngine.Random.Next(0, 1000);
        }

        /// <summary>
        /// Generate a random float between two two provided values.
        /// </summary>
        /// <param name="min">The minimum permitted value</param>
        /// <param name="max">The maximum permitted value</param>
        /// <returns></returns>
        private float RandomFloat(float min, float max)
        {
            return (float)_myGameEngine.Random.NextDouble() * (max - min) + min;
        }



        //-------------------------------------------------------------------------------------
        // Object functions

        /// <summary>
        /// Render the balloon
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Bind to the texture we want to render with
            gl.BindTexture(gl.GL_TEXTURE_2D, _myGameEngine.GameGraphics["Balloon"].Name);

            // Load the identity matrix
            gl.LoadIdentity();

            // Translate into position
            gl.Translatef(GetDisplayXPos(interpFactor), GetDisplayYPos(interpFactor), 0);

            // Rotate as required
            gl.Rotatef(GetDisplayZAngle(interpFactor), 0, 0, 1);

            // Scale according to our size
            gl.Scalef(_size, _size, 1);

            // Enable alpha blending
            gl.Enable(gl.GL_BLEND);
            gl.BlendFunc(gl.GL_SRC_ALPHA, gl.GL_ONE_MINUS_SRC_ALPHA);

            // Generate an array of colors for the balloon.
            // The alpha component is included but set to 1 for each vertex.
            float[] color3 = _myGameEngine.ColorToFloat3(_color);
            float[] balloonColors = new float[] { color3[0], color3[1], color3[2], 1,
                                                  color3[0], color3[1], color3[2], 1,
                                                  color3[0], color3[1], color3[2], 1,
                                                  color3[0], color3[1], color3[2], 1 };

            // Render the balloon
            RenderColorTextureQuad(balloonColors);

            // Disable alpha blending
            gl.Disable(gl.GL_BLEND);
        }

        /// <summary>
        /// Update the balloon
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Move the balloon up the screen.
            // The bigger the balloon, the faster it moves.
            YPos += _size / 40.0f;
            // Have we passed the top of the screen?
            if (YPos > _myGameEngine.OrthoCoords.Top + _size / 2)
            {
                // Terminate this balloon
                Terminate = true;
            }

            // Increment the drift value.
            // We'll use this to rotate the balloon and to
            // slowly move it from side to side.
            _drift += 1;

            // Make the balloon drift horizontally
            XPos += (float)Math.Sin(_drift / 20.0f) * 0.01f;

            // Make the balloon slowly rotate back and forward
            ZAngle = (float)Math.Sin(_drift / 30.0f) * 14;
        }


        /// <summary>
        /// Test whether the supplied coordinate (provided in the game's
        /// coordinate system) is within the boundary of this balloon.
        /// </summary>
        /// <returns></returns>
        internal bool TestHit(float x, float y)
        {
            // Calculate the bounds of the balloon
            float left, right, bottom, top;
            left = XPos - _size / 2;
            right = XPos + _size / 2; ;
            bottom = YPos - _size / 2;
            top = YPos + _size / 2;

            // Return true if the x and y positions both fall between
            // the calculated coordinates
            return (left < x && right > x && bottom < y && top > y);
        }

        /// <summary>
        /// Allow the balloons to be sorted by their size, so that the smallest
        /// balloons come first in the object list
        /// </summary>
        public override int CompareTo(GameEngineCh11.CGameObjectBase other)
        {
            // Are we comparing with another balloon?
            if (other is CObjBalloon)
            {
                // Yes, so compare the sizes
                return -((CObjBalloon)other)._size.CompareTo(_size);
            }
            else
            {
                // No, so let the base class handle the comparison
                return base.CompareTo(other);
            }
        }


    }
}
