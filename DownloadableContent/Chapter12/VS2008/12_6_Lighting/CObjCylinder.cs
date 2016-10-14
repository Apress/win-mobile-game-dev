/**
 * 
 * Lighting
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

namespace Lighting
{
    class CObjCylinder : GameEngineCh12.CGameObjectOpenGLBase
    {

        // Our reference to the game engine.
        private CLightingGame _myGameEngine;

        // The vertices for the cylinder
        private float[] vertices;
        // The normals for the cylinder
        private float[] normals;


        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjCylinder(CLightingGame gameEngine, bool smooth)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Randomize the rotation angle and position
            XAngle = (float)gameEngine.Random.Next(0, 360);
            ZAngle = (float)gameEngine.Random.Next(0, 360);

            // Build the cylinder
            BuildCylinderObject(smooth);
        }

        //-------------------------------------------------------------------------------------
        // Object functions


        /// <summary>
        /// Builds the vertices and normals for the cylinder
        /// </summary>
        /// <param name="smooth">If true, the normals will be set to interpolate
        /// across the side faces of the cylinder to provide smooth lighting.
        /// If false, the normals will not interpolate giving the side of the
        /// cylinder a 'faceted' appearance.</param>
        private void BuildCylinderObject(bool smooth)
        {
            int vertexindex = 0;
            int normalindex = 0;

            // The number of vetical segments from which the cylinder will be formed
            const int segments = 40;

            // Create the array of vertices
            // We will need four triangles for each segment (one for the top,
            // one for the bottom and two for the side)
            // Triangles: 4 * segments
            // Vertices: 3 * 4 * segments (three vertices for each triangle)
            // Coordinates: 3 * 3 * 4 * segments (three coordinates for each vertex)
            vertices = new float[3 * 3 * 4 * segments];
            // We need a normal for each vertex, so the normal array is the same size
            normals = new float[3 * 3 * 4 * segments];

            for (int i=0; i<segments; i++)
            {
                // First generate the triangle for the top of the cylinder.
                // This has one vertex at (0, 1, 0) and the others at the cylinder edge
                vertices[vertexindex++] = 0;
                vertices[vertexindex++] = 1;
                vertices[vertexindex++] = 0;

                vertices[vertexindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                vertices[vertexindex++] = 1;
                vertices[vertexindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                vertices[vertexindex++] = 1;
                vertices[vertexindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);
                
                // Set the normals -- all pointing upwards
                normals[normalindex++] = 0;
                normals[normalindex++] = 1;
                normals[normalindex++] = 0;

                normals[normalindex++] = 0;
                normals[normalindex++] = 1;
                normals[normalindex++] = 0;

                normals[normalindex++] = 0;
                normals[normalindex++] = 1;
                normals[normalindex++] = 0;

                // Next generate the two edge triangles
                vertices[vertexindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                vertices[vertexindex++] = 1;
                vertices[vertexindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                vertices[vertexindex++] = -1;
                vertices[vertexindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                vertices[vertexindex++] = 1;
                vertices[vertexindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                vertices[vertexindex++] = -1;
                vertices[vertexindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                vertices[vertexindex++] = -1;
                vertices[vertexindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                vertices[vertexindex++] = 1;
                vertices[vertexindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);

                // Set the normals
                if (smooth)
                {
                    // We are using smooth lighting for the cylinder, so set normals that
                    // are point in different directions within the same triangle.
                    // OpenGL will interpolate between the normals providing smooth shading
                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);
                }
                else
                {
                    // We are not using smooth lighting, so provide the same normal for the entire
                    // side of the cylinder.
                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                    normals[normalindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                    normals[normalindex++] = 0;
                    normals[normalindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);
                }

                // Finally generate the bottom triangle
                // This has one vertex at (0, -1, 0) and the others at the cylinder edge
                vertices[vertexindex++] = 0;
                vertices[vertexindex++] = -1;
                vertices[vertexindex++] = 0;

                vertices[vertexindex++] = (float)Math.Sin((float)(i + 1) / segments * 2 * glu.PI);
                vertices[vertexindex++] = -1;
                vertices[vertexindex++] = (float)Math.Cos((float)(i + 1) / segments * 2 * glu.PI);

                vertices[vertexindex++] = (float)Math.Sin((float)i / segments * 2 * glu.PI);
                vertices[vertexindex++] = -1;
                vertices[vertexindex++] = (float)Math.Cos((float)i / segments * 2 * glu.PI);

                // Set the normals -- all pointing downwards
                normals[normalindex++] = 0;
                normals[normalindex++] = -1;
                normals[normalindex++] = 0;

                normals[normalindex++] = 0;
                normals[normalindex++] = -1;
                normals[normalindex++] = 0;

                normals[normalindex++] = 0;
                normals[normalindex++] = -1;
                normals[normalindex++] = 0;
            }
        }


        /// <summary>
        /// Render the Cylinder
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Enable hidden surface removal
            gl.Enable(gl.GL_CULL_FACE);

            // Load the identity matrix
            gl.LoadIdentity();

            // Translate a little way into the distance
            gl.Translatef(0, 0, -3);

            // Rotate the Cylinder
            gl.Rotatef(GetDisplayXAngle(interpFactor), 1, 0, 0);
            gl.Rotatef(GetDisplayZAngle(interpFactor), 0, 0, 1);

            // Render the Cylinder
            RenderTriangles(vertices, null, null, normals);
        }

        /// <summary>
        /// Update the Cylinder
        /// </summary>
        public override void Update()
        {
            base.Update();

            XAngle += 2;
            ZAngle += 2.5f;
        }
    
    }
}
