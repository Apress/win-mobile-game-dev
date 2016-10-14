/**
 * 
 * Fireworks
 * 
 * An OpenGL example project demonstraing the use of billboards.
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using OpenGLES;

namespace Fireworks
{
    class CObjFireworkSpark : GameEngineCh13.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CFireworksGame _myGameEngine;

        // The spark's current velocity
        float _xAdd;
        float _yAdd;
        float _zAdd;

        int _timeToLive;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjFireworkSpark(CFireworksGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set a random velocity
            _xAdd = RandomFloat(-0.03f, 0.03f);
            _yAdd = RandomFloat(-0.03f, 0.03f);
            _zAdd = RandomFloat(-0.03f, 0.03f);

            _timeToLive = _myGameEngine.Random.Next(50, 100);
        }

        public CObjFireworkSpark(CFireworksGame gameEngine, float xPos, float yPos, float zPos)
            : this(gameEngine)
        {
            XPos = xPos;
            YPos = yPos;
            ZPos = zPos;
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
        /// Render the object
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Build the spark colors
            float[] quadColors = new float[]  {   1.0f, 0.8f, 0.2f,
                                                  1.0f, 0.8f, 0.2f,
                                                  1.0f, 0.8f, 0.2f,
                                                  1.0f, 0.8f, 0.2f};

            // Enable hidden surface removal
            gl.Enable(gl.GL_CULL_FACE);
            // Disable lighting
            gl.Disable(gl.GL_LIGHTING);

            // Set the camera position
            _myGameEngine.LoadCameraMatrix(interpFactor);

            // Translate the object
            gl.Translatef(GetDisplayXPos(interpFactor), GetDisplayYPos(interpFactor), GetDisplayZPos(interpFactor));

            // Rotate the matrix to face the camera
            _myGameEngine.RotateMatrixToCamera();

            // Scale the matrix so that the quad is rendered at a smaller size
            gl.Scalef(0.05f, 0.05f, 0.05f);

            // Render a quad
            RenderColorQuad(quadColors);
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public override void Update()
        {
            base.Update();

            // See if the spark's time has elapsed
            _timeToLive -= 1;
            if (_timeToLive < 0 || YPos < 0)
            {
                // Yes, so terminate
                Terminate = true;
                return;
            }

            // Move the spark
            XPos += _xAdd;
            YPos += _yAdd;
            ZPos += _zAdd;

            // Apply gravity
            _yAdd -= 0.0002f;
        }


    }
}
