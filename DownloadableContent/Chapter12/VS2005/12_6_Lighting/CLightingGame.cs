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
using System.Windows.Forms;
using OpenGLES;

namespace Lighting
{
    class CLightingGame : GameEngineCh12.CGameEngineOpenGLBase
    {


        //-------------------------------------------------------------------------------------
        // Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CLightingGame(Form gameForm)
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
                BackgroundColor = Color.DarkGray;
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
            // Add a cube to the game
            GameObjects.Add(new CObjCube(this));

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
            SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_SHININESS, 20);

            // Configure light 0.
            // Note that we don't enable it here, we will do that in the form
            // when the appropriate menu item is selected
            // This is a green directional light at (1, 0, 0)
            SetLightParameter(gl.GL_LIGHT0, gl.GL_DIFFUSE, new float[] { 1, 1, 0, 1 });
            SetLightParameter(gl.GL_LIGHT0, gl.GL_SPECULAR, new float[] { 0, 0, 0, 1 });
            SetLightParameter(gl.GL_LIGHT0, gl.GL_POSITION, new float[] { 1, 0, 0, 0 });

            // Configure light 1.
            // Note that we don't enable it here, we will do that in the form
            // when the appropriate menu item is selected.
            // This is a red point light at (0, 0, 2) with white specular color
            SetLightParameter(gl.GL_LIGHT1, gl.GL_DIFFUSE, new float[] {1, 0, 0, 1});
            SetLightParameter(gl.GL_LIGHT1, gl.GL_SPECULAR, new float[] { 1, 1, 1, 1 });
            SetLightParameter(gl.GL_LIGHT1, gl.GL_POSITION, new float[] { 0, 0, 2, 1 });
        }

    }
}
