/**
 * 
 * HiddenSurfaceRemoval
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

namespace HiddenSurfaceRemoval
{
    class CObjCube : GameEngineCh12.CGameObjectOpenGLBase
    {

        // Our reference to the game engine.
        private CCubeGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjCube(CCubeGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Randomize the rotation angle and position
            XAngle = (float)gameEngine.Random.Next(0, 360);
            ZAngle = (float)gameEngine.Random.Next(0, 360);
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

            // Rotate the cube
            gl.Rotatef(GetDisplayXAngle(interpFactor), 1, 0, 0);
            gl.Rotatef(GetDisplayZAngle(interpFactor), 0, 0, 1);

            // Define the vertices for the cube (front face missing)
            float[] vertices = new float[]
            {
                // Right face vertices
                 0.5f, -0.5f, -0.5f,
                 0.5f,  0.5f, -0.5f,
                 0.5f, -0.5f,  0.5f,
                 0.5f,  0.5f, -0.5f,
                 0.5f,  0.5f,  0.5f,
                 0.5f, -0.5f,  0.5f,
                // Back face vertices
                 0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                 0.5f,  0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f,  0.5f, -0.5f,
                 0.5f,  0.5f, -0.5f,
                // Left face vertices
                -0.5f, -0.5f,  0.5f,
                -0.5f,  0.5f,  0.5f,
                -0.5f, -0.5f, -0.5f,
                -0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f, -0.5f,
                -0.5f, -0.5f, -0.5f,
                // Top face vertices
                -0.5f,  0.5f,  0.5f,
                 0.5f,  0.5f,  0.5f,
                -0.5f,  0.5f, -0.5f,
                 0.5f,  0.5f,  0.5f,
                 0.5f,  0.5f, -0.5f,
                -0.5f,  0.5f, -0.5f,
                // Bottom face vertices
                 0.5f, -0.5f,  0.5f,
                -0.5f, -0.5f,  0.5f,
                 0.5f, -0.5f, -0.5f,
                -0.5f, -0.5f,  0.5f,
                -0.5f, -0.5f, -0.5f,
                 0.5f, -0.5f, -0.5f,
            };

            // Define the colors for the cube (front face missing)
            float[] colors = new float[]
            {
                // Right face colors
                1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                // Back face colors
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,
                // Right face colors
                1.0f, 0.3f, 1.0f,
                1.0f, 0.3f, 1.0f,
                1.0f, 0.3f, 1.0f,
                1.0f, 0.3f, 1.0f,
                1.0f, 0.3f, 1.0f,
                1.0f, 0.3f, 1.0f,
                // Top face colors
                1.0f, 0.5f, 0.0f,
                1.0f, 0.5f, 0.0f,
                1.0f, 0.5f, 0.0f,
                1.0f, 0.5f, 0.0f,
                1.0f, 0.5f, 0.0f,
                1.0f, 0.5f, 0.0f,
                // Bottom face colors
                0.0f, 0.8f, 0.0f,
                0.0f, 0.8f, 0.0f,
                0.0f, 0.8f, 0.0f,
                0.0f, 0.8f, 0.0f,
                0.0f, 0.8f, 0.0f,
                0.0f, 0.8f, 0.0f,
            };

            // Render the cube
            RenderTriangles(vertices, colors, null, null);
        }

        /// <summary>
        /// Update the Cube
        /// </summary>
        public override void Update()
        {
            base.Update();

            XAngle += 2;
            ZAngle += 2;
        }



    }
}
