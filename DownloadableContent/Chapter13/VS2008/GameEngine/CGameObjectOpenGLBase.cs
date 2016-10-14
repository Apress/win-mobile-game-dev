using System;
using System.Collections.Generic;
using System.Text;
using OpenGLES;

namespace GameEngineCh13
{
    public class CGameObjectOpenGLBase : CGameObjectBase
    {


        // Object position
        private float _xscale = 0;
        private float _yscale = 0;
        private float _zscale = 0;
        private float _lastxscale = float.MinValue;
        private float _lastyscale = float.MinValue;
        private float _lastzscale = float.MinValue;
        private float _xangle = 0;
        private float _yangle = 0;
        private float _zangle = 0;
        private float _lastxangle = float.MinValue;
        private float _lastyangle = float.MinValue;
        private float _lastzangle = float.MinValue;



        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CGameObjectOpenGLBase(CGameEngineBase gameEngine)
            : base(gameEngine)
        {
            // No constructor code required
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Sets or returns the current x Scale of the object
        /// </summary>
        public virtual float XScale
        {
            get { return _xscale; }
            set { _xscale = value; }
        }
        /// <summary>
        /// Sets or returns the current y Scale of the object
        /// </summary>
        public virtual float YScale
        {
            get { return _yscale; }
            set { _yscale = value; }
        }
        /// <summary>
        /// Sets or returns the current z Scale of the object
        /// </summary>
        public virtual float ZScale
        {
            get { return _zscale; }
            set { _zscale = value; }
        }
        /// <summary>
        /// Sets or returns the previous x Scale of the object
        /// </summary>
        protected float LastXScale
        {
            get { return _lastxscale; }
            set { _lastxscale = value; }
        }
        /// <summary>
        /// Sets or returns the previous y Scale of the object
        /// </summary>
        protected float LastYScale
        {
            get { return _lastyscale; }
            set { _lastyscale = value; }
        }
        /// <summary>
        /// Sets or returns the previous z Scale of the object
        /// </summary>
        protected float LastZScale
        {
            get { return _lastzscale; }
            set { _lastzscale = value; }
        }


        /// <summary>
        /// Sets or returns the current x Angle of the object
        /// </summary>
        public virtual float XAngle
        {
            get { return _xangle; }
            set { _xangle = value; }
        }
        /// <summary>
        /// Sets or returns the current y Angle of the object
        /// </summary>
        public virtual float YAngle
        {
            get { return _yangle; }
            set { _yangle = value; }
        }
        /// <summary>
        /// Sets or returns the current z Angle of the object
        /// </summary>
        public virtual float ZAngle
        {
            get { return _zangle; }
            set { _zangle = value; }
        }
        /// <summary>
        /// Sets or returns the previous x Angle of the object
        /// </summary>
        protected float LastXAngle
        {
            get { return _lastxangle; }
            set { _lastxangle = value; }
        }
        /// <summary>
        /// Sets or returns the previous y Angle of the object
        /// </summary>
        protected float LastYAngle
        {
            get { return _lastyangle; }
            set { _lastyangle = value; }
        }
        /// <summary>
        /// Sets or returns the previous z Angle of the object
        /// </summary>
        protected float LastZAngle
        {
            get { return _lastzangle; }
            set { _lastzangle = value; }
        }



        //-------------------------------------------------------------------------------------
        // OpenGL rendering functions

        /// <summary>
        /// Render a quad at the current location using the provided colors
        /// for the bottom-left, bottom-right, top-left and top-right
        /// corners respectively.
        /// </summary>
        /// <param name="quadColors">An array of four sets of red, green blue
        /// (and optionally alpha) floats.</param>
        unsafe protected void RenderColorQuad(float[] quadColors)
        {
            int elementsPerColor;

            // The vertex positions for a flat unit-size square
            float[] quadVertices = new float[]  {   -0.5f, -0.5f, 0.0f,
                                                     0.5f, -0.5f, 0.0f,
                                                    -0.5f,  0.5f, 0.0f,
                                                     0.5f,  0.5f, 0.0f};
            
            // Determine how many elements were provided for each color
            switch (quadColors.Length)
            {
                case 12: elementsPerColor = 3; break;   // no alpha
                case 16: elementsPerColor = 4; break;   // alpha present
                default: throw new Exception("Unknown content for quadColors -- expected 12 or 16 elements, found " + quadColors.Length.ToString());
            }

            // Fix a pointer to the quad vertices and the quad colors
            fixed (float* quadPointer = &quadVertices[0], colorPointer = &quadColors[0])
            {
                // Enable processing of the vertex and color arrays
                gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                gl.EnableClientState(gl.GL_COLOR_ARRAY);

                // Provide a reference to the vertex array and color arrays
                gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)quadPointer);
                gl.ColorPointer(elementsPerColor, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

                // Draw the quad. We draw a strip of triangles, considering
                // four vertices within the vertex array.
                gl.DrawArrays(gl.GL_TRIANGLE_STRIP, 0, 4);

                // Disable processing of the vertex and color arrays now that we
                // have used them.
                gl.DisableClientState(gl.GL_VERTEX_ARRAY);
                gl.DisableClientState(gl.GL_COLOR_ARRAY);
            }
        }

        /// <summary>
        /// Render a quad at the current location using with the current
        /// bound texture mapped across it.
        /// </summary>
        protected void RenderTextureQuad()
        {
            // Build the default set of texture coordinates for the quad
            float[] texCoords = new float[] { 0.0f, 1.0f,
                                              1.0f, 1.0f,
                                              0.0f, 0.0f,
                                              1.0f, 0.0f };
            // Render the quad
            RenderTextureQuad(texCoords);
        }
        /// <summary>
        /// Render a quad at the current location using the provided texture
        /// coordinates for the bottom-left, bottom-right, top-left and top-right
        /// corners respectively.
        /// </summary>
        /// <param name="texCoords">An array of texture coordinates.</param>
        unsafe protected void RenderTextureQuad(float[] texCoords)
        {
            // The vertex positions for a flat unit-size square
            float[] quadVertices = new float[]  {  -0.5f, -0.5f, 0.0f,
                                                    0.5f, -0.5f, 0.0f,
                                                   -0.5f,  0.5f, 0.0f,
                                                    0.5f,  0.5f, 0.0f};

            // Check the texCoords array
            if (texCoords.Length != 8) throw new Exception("Unknown content for texCoords -- expected 8 elements, found " + texCoords.Length.ToString());

            // Fix a pointer to the quad vertices and the texture coordinates
            fixed (float* quadPointer = &quadVertices[0], texPointer = &texCoords[0])
            {
                // Enable textures
                gl.Enable(gl.GL_TEXTURE_2D);

                // Enable processing of the vertex and texture arrays
                gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                gl.EnableClientState(gl.GL_TEXTURE_COORD_ARRAY);

                // Provide a reference to the vertex and texture arrays
                gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)quadPointer);
                gl.TexCoordPointer(2, gl.GL_FLOAT, 0, (IntPtr)texPointer);

                // Draw the quad. We draw a strip of triangles, considering
                // four vertices within the vertex array.
                gl.DrawArrays(gl.GL_TRIANGLE_STRIP, 0, 4);

                // Disable processing of the vertex and texture arrays now that we
                // have used them.
                gl.DisableClientState(gl.GL_VERTEX_ARRAY);
                gl.DisableClientState(gl.GL_TEXTURE_COORD_ARRAY);

                // Disable textures
                gl.Disable(gl.GL_TEXTURE_2D);
            }
        }

        /// <summary>
        /// Render a quad using the full texture and colors specified
        /// including an alpha component
        /// </summary>
        /// <param name="quadColors">An array of four four-element colors (16 elements in total)</param>
        protected void RenderColorTextureQuad(float[] quadColors)
        {
            // Build the default set of texture coordinates for the quad
            float[] texCoords = new float[] { 0.0f, 1.0f,
                                              1.0f, 1.0f,
                                              0.0f, 0.0f,
                                              1.0f, 0.0f };
            // Render the quad
            RenderColorTextureQuad(quadColors, texCoords);
        }
        /// <summary>
        /// Render a quad using the full texture and colors specified
        /// including an alpha component
        /// </summary>
        /// <param name="texCoords">An array of four two-element texture coordinates (8 elements in total)</param>
        /// <param name="quadColors">An array of four four-element colors (16 elements in total)</param>
        unsafe protected void RenderColorTextureQuad(float[] quadColors, float[] texCoords)
        {
            int elementsPerColor;

            // Determine how many elements were provided for each color
            switch (quadColors.Length)
            {
                case 12: elementsPerColor = 3; break;   // no alpha
                case 16: elementsPerColor = 4; break;   // alpha present
                default: throw new Exception("Unknown content for quadColors -- expected 12 or 16 elements, found " + quadColors.Length.ToString());
            }

            // Fix a pointer to the quad vertices and the quad colors
            fixed (float* colorPointer = &quadColors[0])
            {
                // Enable processing of the color array
                gl.EnableClientState(gl.GL_COLOR_ARRAY);

                // Provide a reference to the color array
                gl.ColorPointer(elementsPerColor, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

                // Draw the quad using the existing RenderTextureQuad function
                RenderTextureQuad(texCoords);

                // Disable processing of the color array now that we have used it.
                gl.DisableClientState(gl.GL_COLOR_ARRAY);
            }
        }


        /// <summary>
        /// Render a series of triangles
        /// </summary>
        /// <param name="vertices">Vertices of the triangles. Each vertex consists of three floats,
        /// and each triangle is formed by three vertices.</param>
        /// <param name="colors">Colors for the vertices or null if no colors are required. Each
        /// color consists of three or four floats (if alpha is present).</param>
        /// <param name="texCoords">Texture coordinates for the vertices or null if no texture
        /// coordinates are required. Each texture coord consists of two floats.</param>
        /// <param name="normals">Normals for the vertices or null if no normals are required.
        /// Each normal consists of three vector values.</param>
        unsafe protected void RenderTriangles(float[] vertices, float[] colors, float[] texCoords, float[] normals)
        {
            int vertexCount = 0;
            int triangleCount = 0;
            int texCoordCount = 0;
            int elementsPerColor = 0;
            int normalCount = 0;

            // Make sure we have some coordinates
            if (vertices == null || vertices.Length == 0) throw new Exception("No vertices provided to RenderTriangles");
            // Find the number of vertices (3 floats per vertex for x, y and z)
            vertexCount = (int)(vertices.Length / 3);
            // Find the number of triangles (3 vertices per triangle)
            triangleCount = (int)(vertexCount / 3);

            // Do we have color values?
            if (colors == null)
            {
                // No, so create an empty single-element array instead.
                // We need this so that we can fix a pointer to it in a moment.
                colors = new float[1];
            }
            else
            {
                // Find the number of colors specified.
                // We have either three or four (including alpha) per vertex...
                if (colors.Length == vertexCount * 3)
                {
                    elementsPerColor = 3;   // no alpha
                }
                else if (colors.Length == vertexCount * 4)
                {
                    elementsPerColor = 4;   // alpha
                }
                else
                {
                    throw new Exception("Number of colors provided does not match number of vertices provided");
                }
            }

            // Do we have texture coordinates?
            if (texCoords == null)
            {
                // No, so create an empty single-element array instead.
                // We need this so that we can fix a pointer to it in a moment.
                texCoords = new float[1];
            }
            else
            {
                // Find the number of texture coordinates. We have two per vertex.
                texCoordCount = (int)(texCoords.Length / 2);
                // Check the tex coord length matches that of the vertices
                if (texCoordCount > 0 && texCoordCount != vertexCount)
                {
                    throw new Exception("Number of texture coordinates provided does not match number of vertices provided");
                }
            }

            // Do we have vertex normals?
            if (normals == null)
            {
                // No, so create an empty single-element array instead.
                // We need this so that we can fix a pointer to it in a moment.
                normals = new float[1];
            }
            else
            {
                // Find the number of vertex normals. We have three values per vertex.
                normalCount = (int)(normals.Length / 3);
                // Check the tex coord length matches that of the vertices
                if (normalCount > 0 && normalCount != vertexCount)
                {
                    throw new Exception("Number of vertex normals provided does not match number of vertices provided");
                }
            }

            // Fix pointers to the vertices, colors, texture coordinates and normals
            fixed (float* verticesPointer = &vertices[0], colorPointer = &colors[0], texPointer = &texCoords[0], normalPointer = &normals[0])
            {
                // Are we using vertex colors?
                if (elementsPerColor > 0)
                {
                    // Enable colors
                    gl.EnableClientState(gl.GL_COLOR_ARRAY);
                    // Provide a reference to the color array
                    gl.ColorPointer(elementsPerColor, gl.GL_FLOAT, 0, (IntPtr)colorPointer);
                }

                // Are we using texture coordinates
                if (texCoordCount > 0)
                {
                    // Enable textures
                    gl.Enable(gl.GL_TEXTURE_2D);
                    // Enable processing of the texture array
                    gl.EnableClientState(gl.GL_TEXTURE_COORD_ARRAY);
                    // Provide a reference to the texture array
                    gl.TexCoordPointer(2, gl.GL_FLOAT, 0, (IntPtr)texPointer);
                }

                // Are we using vertex normals?
                if (normalCount > 0)
                {
                    // Enable normals
                    gl.EnableClientState(gl.GL_NORMAL_ARRAY);
                    // Provide a reference to the normals array
                    gl.NormalPointer(gl.GL_FLOAT, 0, (IntPtr)normalPointer);
                }

                // Enable processing of the vertex array
                gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                // Provide a reference to the vertex array
                gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)verticesPointer);

                // Draw the triangles
                gl.DrawArrays(gl.GL_TRIANGLES, 0, vertexCount);

                // Disable processing of the vertex array
                gl.DisableClientState(gl.GL_VERTEX_ARRAY);

                // Disable processing of the texture, color and normal arrays if we used them
                if (normalCount > 0)
                {
                    gl.DisableClientState(gl.GL_NORMAL_ARRAY);
                }
                if (texCoordCount > 0)
                {
                    gl.DisableClientState(gl.GL_TEXTURE_COORD_ARRAY);
                    gl.Disable(gl.GL_TEXTURE_2D);
                }
                if (elementsPerColor > 0)
                {
                    gl.DisableClientState(gl.GL_COLOR_ARRAY);
                }
            }
        }

        /// <summary>
        /// Render a series of triangles using vertex indices
        /// </summary>
        /// <param name="vertices">Vertices of the triangles. Each vertex consists of three floats,
        /// and each triangle is formed by three vertices.</param>
        /// <param name="colors">Colors for the vertices or null if no colors are required. Each
        /// color consists of three or four floats (if alpha is present).</param>
        /// <param name="texCoords">Texture coordinates for the vertices or null if no texture
        /// coordinates are required. Each texture coord consists of two floats.</param>
        /// <param name="normals">Normals for the vertices or null if no normals are required.
        /// Each normal consists of three vector values.</param>
        unsafe protected void RenderIndexedTriangles(float[] vertices, float[] colors, float[] texCoords, float[] normals, short[] indices)
        {
            int vertexCount = 0;
            int triangleCount = 0;
            int texCoordCount = 0;
            int elementsPerColor = 0;
            int normalCount = 0;

            // Make sure we have some coordinates
            if (vertices == null || vertices.Length == 0) throw new Exception("No vertices provided to RenderIndexedTriangles");
            // Find the number of vertices (3 floats per vertex for x, y and z)
            vertexCount = (int)(vertices.Length / 3);

            // Make sure we have some indices
            if (indices == null || indices.Length == 0) throw new Exception("No indices provided to RenderIndexedTriangles");
            // Find the number of triangles (3 indices per triangle)
            triangleCount = (int)(indices.Length / 3);

            // Do we have color values?
            if (colors == null)
            {
                // No, so create an empty single-element array instead.
                // We need this so that we can fix a pointer to it in a moment.
                colors = new float[1];
            }
            else
            {
                // Find the number of colors specified.
                // We have either three or four (including alpha) per vertex...
                if (colors.Length == vertexCount * 3)
                {
                    elementsPerColor = 3;   // no alpha
                }
                else if (colors.Length == vertexCount * 4)
                {
                    elementsPerColor = 4;   // alpha
                }
                else
                {
                    throw new Exception("Number of colors provided does not match number of vertices provided");
                }
            }

            // Do we have texture coordinates?
            if (texCoords == null)
            {
                // No, so create an empty single-element array instead.
                // We need this so that we can fix a pointer to it in a moment.
                texCoords = new float[1];
            }
            else
            {
                // Find the number of texture coordinates. We have two per vertex.
                texCoordCount = (int)(texCoords.Length / 2);
                // Check the tex coord length matches that of the vertices
                if (texCoordCount > 0 && texCoordCount != vertexCount)
                {
                    throw new Exception("Number of texture coordinates provided does not match number of vertices provided");
                }
            }

            // Do we have vertex normals?
            if (normals == null)
            {
                // No, so create an empty single-element array instead.
                // We need this so that we can fix a pointer to it in a moment.
                normals = new float[1];
            }
            else
            {
                // Find the number of vertex normals. We have three values per vertex.
                normalCount = (int)(normals.Length / 3);
                // Check the tex coord length matches that of the vertices
                if (normalCount > 0 && normalCount != vertexCount)
                {
                    throw new Exception("Number of vertex normals provided does not match number of vertices provided");
                }
            }

            // Fix pointers to the vertices, colors, texture coordinates and normals
            fixed (float* verticesPointer = &vertices[0], colorPointer = &colors[0], texPointer = &texCoords[0], normalPointer = &normals[0])
            {
                // Fix a pointer to the indices
                fixed (short* indexPointer = &indices[0])
                {
                    // Are we using vertex colors?
                    if (elementsPerColor > 0)
                    {
                        // Enable colors
                        gl.EnableClientState(gl.GL_COLOR_ARRAY);
                        // Provide a reference to the color array
                        gl.ColorPointer(elementsPerColor, gl.GL_FLOAT, 0, (IntPtr)colorPointer);
                    }

                    // Are we using texture coordinates
                    if (texCoordCount > 0)
                    {
                        // Enable textures
                        gl.Enable(gl.GL_TEXTURE_2D);
                        // Enable processing of the texture array
                        gl.EnableClientState(gl.GL_TEXTURE_COORD_ARRAY);
                        // Provide a reference to the texture array
                        gl.TexCoordPointer(2, gl.GL_FLOAT, 0, (IntPtr)texPointer);
                    }

                    // Are we using vertex normals?
                    if (normalCount > 0)
                    {
                        // Enable normals
                        gl.EnableClientState(gl.GL_NORMAL_ARRAY);
                        // Provide a reference to the normals array
                        gl.NormalPointer(gl.GL_FLOAT, 0, (IntPtr)normalPointer);
                    }

                    // Enable processing of the vertex array
                    gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                    // Provide a reference to the vertex array
                    gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)verticesPointer);

                    // Draw the triangles
                    gl.DrawElements(gl.GL_TRIANGLES, indices.Length, gl.GL_UNSIGNED_SHORT, (IntPtr)indexPointer);

                    // Disable processing of the vertex array
                    gl.DisableClientState(gl.GL_VERTEX_ARRAY);

                    // Disable processing of the texture, color and normal arrays if we used them
                    if (normalCount > 0)
                    {
                        gl.DisableClientState(gl.GL_NORMAL_ARRAY);
                    }
                    if (texCoordCount > 0)
                    {
                        gl.DisableClientState(gl.GL_TEXTURE_COORD_ARRAY);
                        gl.Disable(gl.GL_TEXTURE_2D);
                    }
                    if (elementsPerColor > 0)
                    {
                        gl.DisableClientState(gl.GL_COLOR_ARRAY);
                    }
                }
            }
        }


        //-------------------------------------------------------------------------------------
        // OpenGL utility functions

        /// <summary>
        /// Builds an array of vertex normals for the array of provided triangle vertices.
        /// </summary>
        /// <param name="vertices">An array of vertices forming the triangles to render. Each
        /// set of three vertices will be treated as the next triangle in the object.</param>
        /// <returns>Returns an array of vertex normals, one normal for each of the provided
        /// vertices. The normals will be normalized.</returns>
        protected float[] GenerateTriangleNormals(float[] vertices)
        {
            int[] indices;

            // Build an array that allows us to treat the vertices as if they were indexed.
            // As the triangles are drawn sequentially, the indexes are actually just
            // an increasing sequence of numbers: the first triangle is formed from
            // vertices 0, 1 and 2, the second triangle from vertices 3, 4 and 5, etc.

            // First create the array with an element for each vertex
            indices = new int[vertices.Length / 3];

            // Then set the elements within the array so that each contains
            // the next sequential vertex index
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }

            // Finally use the indexed normal calculation function to do the work.
            return GenerateIndexedTriangleNormals(vertices, indices);
        }


        /// <summary>
        /// Builds an array of vertex normals for the array of provided vertices and triangle
        /// vertex indices.
        /// </summary>
        /// <param name="vertices">An array of vertices that are used to form the rendered object.</param>
        /// <param name="indices">An array of vertex indices describing the triangles to render. Each set of three
        /// indices will be treated as the next triangle in the object.</param>
        /// <returns>Returns an array of vertex normals, one normal for each of the provided vertices.
        /// The normals will be normalized.</returns>
        protected float[] GenerateIndexedTriangleNormals(float[] vertices, int[] indices)
        {
            // Three arrays to hold the vertices of the triangle we are working with
            float[] vertex0 = new float[3];
            float[] vertex1 = new float[3];
            float[] vertex2 = new float[3];

            // The two vectors that run along the edges of the triangle and a third
            // vector to store the calculated triangle normal
            float[] va = new float[3];
            float[] vb = new float[3];
            float[] vn = new float[3];
            float vnLength;

            // The array of normals. As each vertex has a corresponding normal
            // and both vertices and normals consist of three floats, the normal
            // array will be the same size as the vertex array.
            float[] normals = new float[vertices.Length];

        // Loop for each triangle (each triangle uses three indices)
        for (int index = 0; index < indices.Length; index += 3)
        {
            // Copy the coordinates for the three vertices of this triangle
            // into our vertex arrays
            Array.Copy(vertices, indices[index] * 3, vertex0, 0, 3);
            Array.Copy(vertices, indices[index+1] * 3, vertex1, 0, 3);
            Array.Copy(vertices, indices[index+2] * 3, vertex2, 0, 3);

            // Create the a and b vectors from the vertices
            // First the a vector from vertices 0 and 1
            va[0] = vertex0[0] - vertex1[0];
            va[1] = vertex0[1] - vertex1[1];
            va[2] = vertex0[2] - vertex1[2];
            // Then the b vector from vertices 1 and 2
            vb[0] = vertex1[0] - vertex2[0];
            vb[1] = vertex1[1] - vertex2[1];
            vb[2] = vertex1[2] - vertex2[2];

                // Now perform a cross product operation on the two vectors
                // to generate the normal vector.
                vn[0] = (va[1] * vb[2]) - (va[2] * vb[1]);
                vn[1] = (va[2] * vb[0]) - (va[0] * vb[2]);
                vn[2] = (va[0] * vb[1]) - (va[1] * vb[0]);

                // Now we have the normal vector but it is not normalized.
                // Find its length...
                vnLength = (float)Math.Sqrt((vn[0] * vn[0]) + (vn[1] * vn[1]) + (vn[2] * vn[2]));
                // Make sure the length is non-zero (and if its length is 1 it's already normalized)
                if (vnLength > 0 && vnLength != 1)
                {
                    // Scale the normal vector by its length
                    vn[0] /= vnLength;
                    vn[1] /= vnLength;
                    vn[2] /= vnLength;
                }

                // The normal for this triangle has been calculated.
                // Write it to the normal array for all three vertices
                Array.Copy(vn, 0, normals, indices[index] * 3, 3);
                Array.Copy(vn, 0, normals, indices[index + 1] * 3, 3);
                Array.Copy(vn, 0, normals, indices[index + 2] * 3, 3);
            }

            // Finished, return back the generated array
            return normals;
        }


        //-------------------------------------------------------------------------------------
        // Game engine operations

        /// <summary>
        /// In addition to the positions maintained by the base class, update
        /// the previous positions maintained by this class too (scale
        /// and rotation for each axis)
        /// </summary>
        protected internal override void UpdatePreviousPosition()
        {
            // Let the base class do its work
            base.UpdatePreviousPosition();

            // Update OpenGL-specific values
            LastXScale = XScale;
            LastYScale = YScale;
            LastZScale = ZScale;

            LastXAngle = XAngle;
            LastYAngle = YAngle;
            LastZAngle = ZAngle;
        }




        /// <summary>
        /// Retrieve the actual x Scale of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXScale(float interpFactor)
        {
            // If we have no previous x Scale then return the current x Scale
            if (LastXScale == float.MinValue) return XScale;
            // Otherwise interpolate between the previous Scale and the current Scale
            return CGameFunctions.Interpolate(interpFactor, XScale, LastXScale);
        }
        /// <summary>
        /// Retrieve the actual y Scale of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYScale(float interpFactor)
        {
            // If we have no previous y Scale then return the current y Scale
            if (LastYScale == float.MinValue) return YScale;
            // Otherwise interpolate between the previous Scale and the current Scale
            return CGameFunctions.Interpolate(interpFactor, YScale, LastYScale);
        }
        /// <summary>
        /// Retrieve the actual z Scale of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZScale(float interpFactor)
        {
            // If we have no previous z Scale then return the current z Scale
            if (LastZScale == float.MinValue) return ZScale;
            // Otherwise interpolate between the previous Scale and the current Scale
            return CGameFunctions.Interpolate(interpFactor, ZScale, LastZScale);
        }


        /// <summary>
        /// Retrieve the actual x Angle of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXAngle(float interpFactor)
        {
            // If we have no previous x Angle then return the current x Angle
            if (LastXAngle == float.MinValue) return XAngle;
            // Otherwise interpolate between the previous Angle and the current Angle
            return CGameFunctions.Interpolate(interpFactor, XAngle, LastXAngle);
        }
        /// <summary>
        /// Retrieve the actual y Angle of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYAngle(float interpFactor)
        {
            // If we have no previous y Angle then return the current y Angle
            if (LastYAngle == float.MinValue) return YAngle;
            // Otherwise interpolate between the previous Angle and the current Angle
            return CGameFunctions.Interpolate(interpFactor, YAngle, LastYAngle);
        }
        /// <summary>
        /// Retrieve the actual z Angle of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZAngle(float interpFactor)
        {
            // If we have no previous z Angle then return the current z Angle
            if (LastZAngle == float.MinValue) return ZAngle;
            // Otherwise interpolate between the previous Angle and the current Angle
            return CGameFunctions.Interpolate(interpFactor, ZAngle, LastZAngle);
        }


        /// <summary>
        /// Move this object to the front of all the objects being rendered
        /// </summary>
        public void MoveToFront()
        {
            // Remove the object from the list...
            GameEngine.GameObjects.Remove(this);
            // ...and then re-add it at the end of the list so that it is rendered last
            GameEngine.GameObjects.Add(this);
        }

        /// <summary>
        /// Move this object to the back of all the objects being rendered
        /// </summary>
        public void MoveToBack()
        {
            // Remove the object from the list...
            GameEngine.GameObjects.Remove(this);
            // ...and then re-add it at the start of the list so that it is rendered first
            GameEngine.GameObjects.Insert(0, this);
        }

        /// <summary>
        /// Provide default sorting for OpenGL objects so that they
        /// are ordered from those with the highest z position first
        /// (furthest away) to those with the lowest z position last
        /// (nearest).
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override int CompareTo(CGameObjectBase other)
        {
            // Sort by z position
            return ((CGameObjectOpenGLBase)other).ZPos.CompareTo(ZPos);
        }

    }
}
