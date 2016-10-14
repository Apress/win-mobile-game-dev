using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameEngine
{
    public abstract class CGeomLoaderBase
    {

        // The finished arrays of vertices, texture coordinates and vertex normals
        protected float[] _vertices;
        protected float[] _texCoords;
        protected float[] _normals;

        //-------------------------------------------------------------------------------------
        // Property access

        public float[] Vertices
        {
            get { return _vertices; }
        }

        public float[] TexCoords
        {
            get { return _texCoords; }
        }

        public float[] Normals
        {
            get { return _normals; }
        }

        //-------------------------------------------------------------------------------------
        // Object load code

        /// <summary>
        /// Load geometry from a file
        /// </summary>
        public virtual void LoadFromFile(string filename)
        {
            string content;

            // Load the file into a string
            using (StreamReader file = new StreamReader(filename))
            {
                content = file.ReadToEnd();
            }

            // Call LoadFromString to process the string
            LoadFromString(content);
        }

        /// <summary>
        /// Load geometry from a stream
        /// </summary>
        public virtual void LoadFromStream(Stream stream)
        {
            string content;
            byte[] contentBytes;

            // Create space to read the stream
            contentBytes = new byte[stream.Length];
            // Seek to the beginning of the stream
            stream.Seek(0, SeekOrigin.Begin);
            // Read the string into the byte array
            stream.Read(contentBytes, 0, (int)stream.Length);

            // Convert the byte array into text
            content = Encoding.ASCII.GetString(contentBytes, 0, contentBytes.Length);

            // Call LoadFromString to process the string
            LoadFromString(content);
        }

        /// <summary>
        /// Load geometry from a string
        /// </summary>
        public abstract void LoadFromString(string content);


        //-------------------------------------------------------------------------------------
        // Object manipulation

        /// <summary>
        /// Adjust the vertex positions so that the object is centered around
        /// the coordinate (0, 0, 0).
        /// </summary>
        public void CenterObject()
        {
            // Make sure we have some vertices to work with
            if (_vertices == null || _vertices.Length == 0) return;

            float minx = float.MaxValue, miny = float.MaxValue, minz = float.MaxValue;
            float maxx = float.MinValue, maxy = float.MinValue, maxz = float.MinValue;
            float xcenter, ycenter, zcenter;

            // Loop through the vertices getting the minimum and maximum values
            // in each axis
            for (int i = 0; i < _vertices.Length; i += 3)
            {
                if (_vertices[i] < minx) minx = _vertices[i];
                if (_vertices[i] > maxx) maxx = _vertices[i];
                if (_vertices[i + 1] < miny) miny = _vertices[i + 1];
                if (_vertices[i + 1] > maxy) maxy = _vertices[i + 1];
                if (_vertices[i + 2] < minz) minz = _vertices[i + 2];
                if (_vertices[i + 2] > maxz) maxz = _vertices[i + 2];
            }

            // Now we know the box inside which the object resides,
            // subtract the object's current center point from each
            // vertex. This will put the center point at (0, 0, 0)
            xcenter = (minx + maxx) / 2;
            ycenter = (miny + maxy) / 2;
            zcenter = (minz + maxz) / 2;
            // Apply the offset to the vertex coordinates
            for (int i = 0; i < _vertices.Length; i += 3)
            {
                _vertices[i] -= xcenter;
                _vertices[i + 1] -= ycenter;
                _vertices[i + 2] -= zcenter;
            }
        }

        /// <summary>
        /// Scale the object by the specified amounts
        /// </summary>
        /// <param name="scalex">The amount by which to scale on the x axis</param>
        /// <param name="scaley">The amount by which to scale on the y axis</param>
        /// <param name="scalez">The amount by which to scale on the z axis</param>
        public void ScaleObject(float scalex, float scaley, float scalez)
        {
            // Loop through the vertices...
            for (int i = 0; i < _vertices.Length; i += 3)
            {
                // Scale each vertex
                _vertices[i] *= scalex;
                _vertices[i + 1] *= scaley;
                _vertices[i + 2] *= scalez;
            }
        }

    }
}
