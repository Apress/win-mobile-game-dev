/**
 * 
 * ImportingGeometry
 * 
 * An OpenGL example project.
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

namespace Geometry
{
    class CObjHouse : GameEngineCh13.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CGeometryGame _myGameEngine;

        // Object vertices, texture coordinates, normals and indices
        private float[] _vertices;
        private float[] _texCoords;
        private float[] _normals;

        /// <summary>
        /// Constructor. Require an instance of our own game class as a parameter.
        /// </summary>
        public CObjHouse(CGeometryGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set the initial object state
            XAngle = 90;
            YAngle = 180;
            ZAngle = 0;

            // Load the object
            GameEngineCh13.CGeomLoaderObj objLoader = new GameEngineCh13.CGeomLoaderObj();
            Assembly asm = Assembly.GetExecutingAssembly();
            objLoader.LoadFromStream(asm.GetManifestResourceStream("ImportingGeometry.Geometry.House.obj"));

            // Center the object
            objLoader.CenterObject();
            // Scale the object to 1.5% of its original size
            objLoader.ScaleObject(0.015f, 0.015f, 0.015f);

            // Read the object details
            _vertices = objLoader.Vertices;
            _texCoords = objLoader.TexCoords;
            _normals = objLoader.Normals;

            // Were normals provided by the object?
            if (_normals == null)
            {
                // No, so generate them ourselves
                _normals = GenerateTriangleNormals(_vertices);
            }

            // Have we loaded our house texture?
            if (!_myGameEngine.GameGraphics.ContainsKey("House"))
            {
                // No, so load it now
                Texture tex = Texture.LoadStream(asm.GetManifestResourceStream("ImportingGeometry.Graphics.House.jpg"), false);
                _myGameEngine.GameGraphics.Add("House", tex);
            }
        }

        //-------------------------------------------------------------------------------------
        // Object functions

        /// <summary>
        /// Render the object
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Enable hidden surface removal
            gl.Enable(gl.GL_CULL_FACE);

            // Load the identity matrix
            gl.LoadIdentity();

            // Rotate the object
            gl.Rotatef(GetDisplayXAngle(interpFactor), 1, 0, 0);
            gl.Rotatef(GetDisplayYAngle(interpFactor), 0, 1, 0);
            gl.Rotatef(GetDisplayZAngle(interpFactor), 0, 0, 1);

            // Enable textures
            gl.Enable(gl.GL_TEXTURE_2D);
            // Bind to the House texture
            gl.BindTexture(gl.GL_TEXTURE_2D, _myGameEngine.GameGraphics["House"].Name);

            // Render the object
            RenderTriangles(_vertices, null, _texCoords, _normals);

            // Disable textures
            gl.Disable(gl.GL_TEXTURE_2D);
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public override void Update()
        {
            base.Update();

            ZAngle += 1.0f;
        }

    }
}
