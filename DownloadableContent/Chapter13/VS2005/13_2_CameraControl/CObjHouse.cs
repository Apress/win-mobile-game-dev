﻿/**
 * 
 * CameraControl
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

namespace CameraControl
{
    class CObjHouse : GameEngineCh13.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CCameraGame _myGameEngine;

        // Object vertices, texture coordinates, normals and indices
        private float[] _vertices;
        private float[] _texCoords;
        private float[] _normals;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjHouse(CCameraGame gameEngine)
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
            objLoader.LoadFromStream(asm.GetManifestResourceStream("CameraControl.Geometry.House.obj"));

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

            // Set the camera position
            _myGameEngine.LoadCameraMatrix(interpFactor);

            // Translate the object
            gl.Translatef(GetDisplayXPos(interpFactor), GetDisplayYPos(interpFactor), GetDisplayZPos(interpFactor));

            // Rotate the object
            gl.Rotatef(GetDisplayXAngle(interpFactor), 1, 0, 0);
            gl.Rotatef(GetDisplayYAngle(interpFactor), 0, 1, 0);
            gl.Rotatef(GetDisplayZAngle(interpFactor), 0, 0, 1);

            // Render the object
            RenderTriangles(_vertices, null, null, _normals);
        }

        /// <summary>
        /// Update the object
        /// </summary>
        public override void Update()
        {
            base.Update();

            XAngle = 90;
            YAngle = 180;
            ZAngle = 0;
        }

    }
}
