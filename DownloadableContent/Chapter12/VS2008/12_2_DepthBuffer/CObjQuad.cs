/**
 * 
 * DepthBuffer
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

namespace DepthBuffer
{
    class CObjQuad : GameEngineCh12.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CDepthBufferGame _myGameEngine;

        // The color for this quad
        private float[] _colors = new float[3];

        // A value to allow the z position of the object to be calculated
        float _movePos = 0;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjQuad(CDepthBufferGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Generate a random color
            _colors[0] = RandomFloat(0, 1);
            _colors[1] = RandomFloat(0, 1);
            _colors[2] = RandomFloat(0, 1);
        }

        public CObjQuad(CDepthBufferGame gameEngine, float movePos, float XPos, float YPos)
            : this(gameEngine)
        {
            this.XPos = XPos;
            this.YPos = YPos;
            this._movePos = movePos;
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

            // Translate a way into the distance
            gl.Translatef(0, 0, -5);

            // Scale up to make the quad larger
            gl.Scalef(2.5f, 2.5f, 2.5f);

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

            // Calculate the z position
            _movePos += 0.1f;
            ZPos = (float)Math.Sin(_movePos) * 5;
        }



    }
}
