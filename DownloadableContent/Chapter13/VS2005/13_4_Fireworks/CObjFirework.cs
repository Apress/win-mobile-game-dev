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
    class CObjFirework : GameEngineCh13.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CFireworksGame _myGameEngine;

        // The firework's current vertical velocity
        float _yAdd;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjFirework(CFireworksGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Position the firework
            XPos = RandomFloat(-2, 2);
            YPos = 0;
            XPos = RandomFloat(-2, 2);

            // Set a random take-off velocity
            _yAdd = RandomFloat(0.13f, 0.18f);
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

            // Build the firework colors
            float[] quadColors = new float[]  {   1.0f, 1.0f, 0.3f,
                                                  1.0f, 1.0f, 0.3f,
                                                  1.0f, 1.0f, 0.3f,
                                                  1.0f, 1.0f, 0.3f};

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
            gl.Scalef(0.15f, 0.15f, 0.15f);

            // Render a quad
            RenderColorQuad(quadColors);
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Move the firework
            YPos += _yAdd;
            // Apply gravity
            _yAdd -= 0.002f;

            // Have we reached the explosion point?
            if (_yAdd < -0.04)
            {
                // Kill the firework
                Terminate = true;

                // Add the sparks
                for (int i = 0; i < 50; i++)
                {
                    _myGameEngine.GameObjects.Add(new CObjFireworkSpark(_myGameEngine, XPos, YPos, ZPos));
                }
            }

        }


    }
}
