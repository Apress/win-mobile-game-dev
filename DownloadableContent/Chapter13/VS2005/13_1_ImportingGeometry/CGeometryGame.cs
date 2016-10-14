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
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace Geometry
{
    class CGeometryGame : GameEngineCh13.CGameEngineOpenGLBase
    {


        //-------------------------------------------------------------------------------------
        // Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CGeometryGame(Form gameForm)
            : base(gameForm)
        {
        }



        //-------------------------------------------------------------------------------------
        // Game functions

        /// <summary>
        /// Prepare the game
        /// </summary>
        public override void Prepare()
        {
            // Allow the base class to do its work
            base.Prepare();

            // Make sure OpenGL is initialized before we interact with it
            if (OpenGLInitialized)
            {
                // Set properties
                BackgroundColor = Color.LightBlue;
            }
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            // Clear existing objects
            GameObjects.Clear();
            // Add a house to the game
            GameObjects.Add(new CObjHouse(this));

            // Reset to the identity matrix
            gl.LoadIdentity();

            // Initialize lighting
            InitLight();
        }

        /// <summary>
        /// Enable and configure the lighting to use within the OpenGL scene
        /// </summary>
        private void InitLight()
        {
            // Enable lighting
            gl.Enable(gl.GL_LIGHTING);

            // Default the ambient light color to black (i.e., no light)
            SetAmbientLight(new float[] { 0, 0, 0, 1 });

            // Set the material color to fully reflect ambient, diffuse and specular light
            SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_AMBIENT, new float[] { 1, 1, 1, 1 });
            SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_SPECULAR, new float[] { 1, 1, 1, 1 });
            SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_DIFFUSE, new float[] { 1, 1, 1, 1 });
            // Set the material shininess (0 = extremely shiny, 128 = not at all shiny)
            SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_SHININESS, 120);

            // Configure light 0.
            // This is a white point light at (0, 0, 2)
            SetLightParameter(gl.GL_LIGHT0, gl.GL_DIFFUSE, new float[] { 1, 1, 1, 1 });
            SetLightParameter(gl.GL_LIGHT0, gl.GL_POSITION, new float[] { 0, 0, 2, 1 });
            gl.Enable(gl.GL_LIGHT0);
        }

    }
}
