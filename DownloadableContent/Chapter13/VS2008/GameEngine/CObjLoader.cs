using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameEngine
{
    public class CObjLoader
    {

        // The finished arrays of vertices, texture coordinates and vertex normals
        private float[] _vertices;
        private float[] _texCoords;
        private float[] _normals;
        private int[] _indices;

        // The vertices, tex coords and normals read from the obj file
        private List<float> _objVertices;
        private List<float> _objTexCoords;
        private List<float> _objNormals;



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

        public int[] Indices
        {
            get { return _indices; }
        }


        //-------------------------------------------------------------------------------------
        // Object load code


        public void LoadFromFile(string filename)
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


        public void LoadFromStream(Stream stream)
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


        public void LoadFromString(string content)
        {
            string contentLineClean;
            int lineNumber = 0;

            // Initialize the arrays
            _vertices = null;
            _texCoords = null;
            _normals = null;
            _indices = null;
            _objVertices = new List<float>();
            _objTexCoords = new List<float>();
            _objNormals = new List<float>();

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

                // Make sure we have some content and that the line contains a space after the first character
                if (contentLineClean.Length > 0 && contentLineClean.IndexOf(' ') > 0)
                {
                    // What type of data is stored on this line?
                    // Read the data before the first space character to find out.
                    switch (contentLineClean.Split(' ')[0].ToLower())
                    {
                        case "v":
                            ReadVertex(contentLineClean, lineNumber);
                            break;
                        case "vt":
                            ReadTexCoord(contentLineClean, lineNumber);
                            break;
                        case "vn":
                            ReadVertexNormal(contentLineClean, lineNumber);
                            break;
                        case "f":
                            ReadFace(contentLineClean, lineNumber);
                            break;
                        default:
                            // Whatever is stored here is either unrecognised so ignore it.
                            break;
                    }
                }

            }

        }


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
            _objVertices.Add(x);
            _objVertices.Add(y);
            _objVertices.Add(z);
        }

        private void ReadTexCoord(string contentLineClean, int lineNumber)
        {
            string[] contentElements;
            float s, t;

            // The texcoord definition is in the following form:
            //  vt (s) (t)  '!!!!!!!!!!!!!!!!!!!!! check the names here !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // For example:
            //  vt 0.1 0.5
            // To create the texcoord, we can simply read out elements 1 and 2
            // from the array retrieved by splitting the string on its spaces,
            // and add these to the texcoords array.

            contentElements = contentLineClean.Split(' ');

            // Make sure we have exactly three elements
            if (contentElements.Length != 3)
            {
                throw new Exception("Cannot parse obj file: invalid number of elements found for texcoord on line " + lineNumber.ToString());
            }

            // Parse the vertex coordinate
            s = float.Parse(contentElements[1]);
            t = float.Parse(contentElements[2]);

            // Add the texcoord to the texcoords list
            _objVertices.Add(s);
            _objVertices.Add(t);
        }

        private void ReadVertexNormal(string contentLineClean, int lineNumber)
        {
            string[] contentElements;
            float x, y, z;

            // The normal definition is in the following form:
            //  vn (xpos) (ypos) (zpos)
            // For example:
            //  vn 0.0 1.0 0.0
            // To create the normal, we can simply read out elements 1, 2 and 3
            // from the array retrieved by splitting the string on its spaces,
            // and add these to the normal array.

            contentElements = contentLineClean.Split(' ');

            // Make sure we have exactly four elements
            if (contentElements.Length != 4)
            {
                throw new Exception("Cannot parse obj file: invalid number of elements found for vertex normal on line " + lineNumber.ToString());
            }

            // Parse the vertex coordinate
            x = float.Parse(contentElements[1]);
            y = float.Parse(contentElements[2]);
            z = float.Parse(contentElements[3]);

            // Add the coordinate to the vertices list
            _objNormals.Add(x);
            _objNormals.Add(y);
            _objNormals.Add(z);
        }


        private void ReadFace(string contentLineClear, int lineNumber)
        {


        }


    }
}
