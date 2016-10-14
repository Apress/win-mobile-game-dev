using System;
using System.Collections.Generic;
using System.Text;

namespace GameEngineCh13
{
    public class CGeomLoaderObj : CGeomLoaderBase
    {

        // The defined vertices, texture coords and normals read from the obj file
        private List<float> _objDefinedVertices;
        private List<float> _objDefinedTexCoords;
        private List<float> _objDefinedNormals;

        // The vertices, texture coords and normals that build the triangles
        // of the finished object
        private List<float> _outputVertices;
        private List<float> _outputTexCoords;
        private List<float> _outputNormals;

        /// <summary>
        /// Load a .obj file from the provided string
        /// </summary>
        /// <param name="content"></param>
        public override void LoadFromString(string content)
        {
            string contentLineClean;
            int lineNumber = 0;

            // Initialize the arrays
            _vertices = null;
            _texCoords = null;
            _normals = null;
            _objDefinedVertices = new List<float>();
            _objDefinedTexCoords = new List<float>();
            _objDefinedNormals = new List<float>();
            _outputVertices = new List<float>();
            _outputTexCoords = new List<float>();
            _outputNormals = new List<float>();

            // Ensure that line breaks are represented as carriage returns,
            // regardless of how they were stored in the file content.
            content = content.Replace("\r\n", "\r");
            content = content.Replace("\n", "\r");

            // Loop for each line within the file to read in the vertices, tex coords,
            // normals and faces that are defined within.
            foreach (string contentLine in content.Split('\r'))
            {
                // Add to the line number so that we can provide useful information if a problem
                // occurs reading the content.
                lineNumber += 1;

                // Trim white space from the start and end of the line
                contentLineClean = contentLine.Trim();

                // Make sure we have some content and that the line contains a space
                // somewhere after the first character
                if (contentLineClean.Length > 0 && contentLineClean.IndexOf(' ') > 0)
                {
                    // What type of data is stored on this line?
                    // Read the data before the first space character to find out.
                    switch (contentLineClean.Split(' ')[0].ToLower())
                    {
                        case "v":
                            // This is a vertex definition
                            ReadVertex(contentLineClean, lineNumber);
                            break;
                        case "vt":
                            // This is a texture coordinate definition
                            ReadTexCoord(contentLineClean, lineNumber);
                            break;
                        case "vn":
                            // This is a vertex normal definition
                            ReadVertexNormal(contentLineClean, lineNumber);
                            break;
                        case "f":
                            // This is a face definition
                            ReadFace(contentLineClean, lineNumber);
                            break;
                        default:
                            // Whatever is stored here is unrecognised so ignore it.
                            break;
                    }
                }
            }

            // Transfer the generated data into the arrays so that they can be
            // read back to the calling code.
            _vertices = new float[_outputVertices.Count];
            _outputVertices.CopyTo(_vertices);
            if (_outputTexCoords.Count > 0)
            {
                _texCoords = new float[_outputTexCoords.Count];
                _outputTexCoords.CopyTo(_texCoords);
            }
            if (_outputNormals.Count > 0)
            {
                _normals = new float[_outputNormals.Count];
                _outputNormals.CopyTo(_normals);
            }

            // Clear the lists now that we have finished using them.
            // This releases any resources that have allocated.
            _objDefinedVertices = null;
            _objDefinedTexCoords = null;
            _objDefinedNormals = null;
            _outputVertices = null;
            _outputTexCoords = null;
            _outputNormals = null;

            // Finished.
        }

        /// <summary>
        /// Read a vertex definition from the supplied string
        /// </summary>
        private void ReadVertex(string contentLineClean, int lineNumber)
        {
            string[] contentElements;
            float x, y, z;

            // The vertex definition is in the following form:
            //  v (xpos) (ypos) (zpos)
            // For example:
            //  v 0.0 10.0 0.0
            // To create the vertex, we can simply read out elements 1, 2 and 3
            // from the array retrieved by splitting the string on its spaces,
            // and add these to the vertex array.
            contentElements = contentLineClean.Split(' ');

            // Make sure we have exactly four elements
            if (contentElements.Length != 4)
            {
                throw new Exception("Cannot parse obj file: invalid number of elements found for vertex on line " + lineNumber.ToString());
            }

            // Parse the vertex coordinate
            x = float.Parse(contentElements[1]);
            y = float.Parse(contentElements[2]);
            z = float.Parse(contentElements[3]);

            // Add the coordinate to the vertices list
            _objDefinedVertices.Add(x);
            _objDefinedVertices.Add(y);
            _objDefinedVertices.Add(z);
        }

        /// <summary>
        /// Read a texture coordinate definition from the supplied string
        /// </summary>
        private void ReadTexCoord(string contentLineClean, int lineNumber)
        {
            string[] contentElements;
            float s, t;

            // The texcoord definition is in the following form:
            //  vt (s) (t)
            // For example:
            //  vt 0.1 0.5
            contentElements = contentLineClean.Split(' ');

            // Make sure we have exactly three elements
            if (contentElements.Length != 3)
            {
                throw new Exception("Cannot parse obj file: invalid number of elements found for texcoord on line " + lineNumber.ToString());
            }

            // Parse the texture coordinate
            s = float.Parse(contentElements[1]);
            t = -float.Parse(contentElements[2]);

            // Add the texcoord to the texcoords list
            _objDefinedTexCoords.Add(s);
            _objDefinedTexCoords.Add(t);
        }

        /// <summary>
        /// Read a normal definition from the supplied string
        /// </summary>
        private void ReadVertexNormal(string contentLineClean, int lineNumber)
        {
            string[] contentElements;
            float x, y, z;

            // The normal definition is in the following form:
            //  vn (xpos) (ypos) (zpos)
            // For example:
            //  vn 0.0 1.0 0.0
            contentElements = contentLineClean.Split(' ');

            // Make sure we have exactly four elements
            if (contentElements.Length != 4)
            {
                throw new Exception("Cannot parse obj file: invalid number of elements found for vertex normal on line " + lineNumber.ToString());
            }

            // Parse the normal
            x = float.Parse(contentElements[1]);
            y = float.Parse(contentElements[2]);
            z = float.Parse(contentElements[3]);

            // Add the coordinate to the vertices list
            _objDefinedNormals.Add(x);
            _objDefinedNormals.Add(y);
            _objDefinedNormals.Add(z);
        }

        /// <summary>
        /// Read a triangular face definition from the supplied string
        /// </summary>
        private void ReadFace(string contentLineClean, int lineNumber)
        {
            string[] faceElements;

            // The triangle is definition is in the following form:
            //  f vertex1 vertex2 vertex3
            // Each vertex is defined as:
            //  (vertexindex)/(texcoordindex)/(normalindex)
            // Only the vertex index is mandatory, the other values may be
            // missing.

            // Break the face apart into its individual coordinate references
            faceElements = contentLineClean.Split(' ');

            // Make sure we have exactly four elements
            if (faceElements.Length != 4)
            {
                throw new Exception("Cannot parse obj file: invalid number of elements found for face on line " + lineNumber.ToString());
            }

            // Process each of the elements
            for (int i = 1; i < 4; i++)
            {
                ReadFace_ProcessVertex(faceElements[i]);
            }
        }

        /// <summary>
        /// Process a face vertex specification from the supplied string
        /// </summary>
        private void ReadFace_ProcessVertex(string faceElement)
        {
            string[] vertexElements;
            int vertexIndex = -1;
            int texCoordIndex = -1;
            int normalIndex = -1;

            // Split the element into its component parts, separated by / characters
            vertexElements = faceElement.Split('/');

            // The first element tells us which vertex coordinate to use...
            vertexIndex = int.Parse(vertexElements[0]) - 1;

            // If we have a second element, it will us which tex coord to use...
            if (vertexElements.Length >= 2 && vertexElements[1].Length > 0)
            {
                texCoordIndex = int.Parse(vertexElements[1]) - 1;
            }

            // If we have a third element, it will us which normal to use...
            if (vertexElements.Length >= 3 && vertexElements[2].Length > 0)
            {
                normalIndex = int.Parse(vertexElements[2]) - 1;
            }

            // Add the vertex coordinate to the output data
            for (int i = 0; i < 3; i++)
            {
                _outputVertices.Add(_objDefinedVertices[vertexIndex * 3 + i]);
            }

            // If we have a texture coordinate...
            if (texCoordIndex >= 0)
            {
                // Add the texture coordinate to the output data
                for (int i = 0; i < 2; i++)
                {
                    _outputTexCoords.Add(_objDefinedTexCoords[texCoordIndex * 2 + i]);
                }
            }

            // If we have a normal...
            if (normalIndex >= 0)
            {
                // Add the normal to the output data
                for (int i = 0; i < 3; i++)
                {
                    _outputNormals.Add(_objDefinedNormals[normalIndex * 3 + i]);
                }
            }
        }




    }
}
