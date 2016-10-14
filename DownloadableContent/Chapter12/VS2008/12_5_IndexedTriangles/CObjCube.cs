/**
 * 
 * IndexedTriangles
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

namespace IndexedTriangles
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

            // Define the vertices for the cube
            float[] vertices = new float[]
            {
                -0.5f, -0.5f,  0.5f,    // front bottom left
                 0.5f, -0.5f,  0.5f,    // front bottom right
                -0.5f,  0.5f,  0.5f,    // front top left
                 0.5f,  0.5f,  0.5f,    // front top right

                 0.5f, -0.5f, -0.5f,    // back bottom right
                -0.5f, -0.5f, -0.5f,    // back bottom left
                 0.5f,  0.5f, -0.5f,    // back top right
                -0.5f,  0.5f, -0.5f,    // back top left
            };

            // Define the colors for the cube
            float[] colors = new float[]
            {
                0.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                1.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                1.0f, 0.0f, 1.0f,
                0.0f, 1.0f, 1.0f,
                1.0f, 1.0f, 1.0f,
            };

            // Define the indices for the cube
            short[] indices = new short[]
            {
                // Front face
                0, 1, 2,
                1, 3, 2,
                // Right face
                4, 6, 1,
                6, 3, 1,
                // Back face
                4, 5, 6,
                5, 7, 6,
                // Left face
                0, 2, 5,
                2, 7, 5,
                // Top face
                2, 3, 7,
                3, 6, 7,
                // Bottom face
                1, 0, 4,
                0, 5, 4,
            };
      
            // Render the cube
            RenderIndexedTriangles(vertices, colors, null, null, indices);
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
