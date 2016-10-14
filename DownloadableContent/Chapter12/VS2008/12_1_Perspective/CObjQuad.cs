/**
 * 
 * Perspective
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * 3-D graphics using OpenGL ES
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenGLES;

namespace Perspective
{
    class CObjQuad : GameEngineCh12.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CPerspectiveGame _myGameEngine;

        // The color for this quad
        private float[] _colors = new float[3];

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjQuad(CPerspectiveGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Generate a random position on all three axes
            XPos = RandomFloat(-10, 10);
            YPos = RandomFloat(-15, 15);
            ZPos = RandomFloat(-100, 0);

            // Generate a random rotation angle
            ZAngle = RandomFloat(0, 360);

            // Generate a random color
            _colors[0] = RandomFloat(0, 1);
            _colors[1] = RandomFloat(0, 1);
            _colors[2] = RandomFloat(0, 1);
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
        /// Render the Cube
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Load the identity matrix
            gl.LoadIdentity();

            // Translate into position
            gl.Translatef(GetDisplayXPos(interpFactor), GetDisplayYPos(interpFactor), GetDisplayZPos(interpFactor));

            // Rotate around the z axis
            gl.Rotatef(GetDisplayZAngle(interpFactor), 0, 0, 1);

            // Generate an array of colors.
            float[] CubeColors = new float[] { _colors[0], _colors[1], _colors[2], 1,
                                               _colors[0], _colors[1], _colors[2], 1,
                                               _colors[0], _colors[1], _colors[2], 1,
                                               _colors[0], _colors[1], _colors[2], 1 };

            // Render the Cube
            RenderColorTextureQuad(CubeColors);
        }

        /// <summary>
        /// Update the Cube
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Increase the z position
            ZPos += 1;
            // Have we reached the front or our z range?
            if (ZPos >= 1)
            {
                // Yes, so move to the back
                ZPos -= 100;
                // Jump immediately to the back, don't interpolate
                UpdatePreviousPosition();
            }

            // Rotate to look pretty too
            ZAngle += 1;
        }



    }
}
