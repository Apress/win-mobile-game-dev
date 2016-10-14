/**
 * 
 * Fog
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

namespace Fog
{
    class CFogGame : GameEngineCh13.CGameEngineOpenGLBase
    {

        private CObjCamera _camera;

        //-------------------------------------------------------------------------------------
        // Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CFogGame(Form gameForm)
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
            CObjHouse house;

            base.Reset();

            // Clear existing objects
            GameObjects.Clear();

            // Add the camera
            _camera = new CObjCamera(this);
            GameObjects.Add(_camera);

            // Add some houses to the game
            house = new CObjHouse(this);
            house.XPos = 0;
            house.ZPos = 4;
            GameObjects.Add(house);

            house = new CObjHouse(this);
            house.XPos = 0;
            house.ZPos = 2;
            GameObjects.Add(house);

            house = new CObjHouse(this);
            house.XPos = 0;
            house.ZPos = 0;
            GameObjects.Add(house);

            house = new CObjHouse(this);
            house.XPos = 0;
            house.ZPos = -2;
            GameObjects.Add(house);

            house = new CObjHouse(this);
            house.XPos = 0;
            house.ZPos = -4;
            GameObjects.Add(house);

            // Reset to the identity matrix
            gl.LoadIdentity();

            // Initialize lighting
            InitLight(false);

            // Initialize fog
            InitFog();
        }

        /// <summary>
        /// Enable and configure the lighting to use within the OpenGL scene
        /// </summary>
        /// <param name="positionOnly">If true, only the light positions will be set</param>
        internal void InitLight(bool positionOnly)
        {
            if (positionOnly == false)
            {
                // Enable lighting
                gl.Enable(gl.GL_LIGHTING);

                // Set the ambient light color to dark gray
                SetAmbientLight(new float[] { 0.1f, 0.1f, 0.1f, 1 });

                // Set the material color
                SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_AMBIENT, new float[] { 1, 1, 1, 1 });
                SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_DIFFUSE, new float[] { 1, 1, 1, 1 });

                // Configure light 0.
                SetLightParameter(gl.GL_LIGHT0, gl.GL_DIFFUSE, new float[] { 0.8f, 0.8f, 0.8f, 1 });
                gl.Enable(gl.GL_LIGHT0);
            }

            // Set the light positions
            SetLightParameter(gl.GL_LIGHT0, gl.GL_POSITION, new float[] { 2, 2, 2, 1 });
        }

        private void InitFog()
        {
            // Enable fog
            gl.Enable(gl.GL_FOG);
            // Set the fog color. We'll set this to match our background color.
            SetFogParameter(gl.GL_FOG_COLOR, ColorToFloat4(BackgroundColor));

            // Use exponential fog
            SetFogParameter(gl.GL_FOG_MODE, gl.GL_EXP);
            SetFogParameter(gl.GL_FOG_DENSITY, 0.25f);

            // Use linear fog
            //SetFogParameter(gl.GL_FOG_MODE, gl.GL_LINEAR);
            //SetFogParameter(gl.GL_FOG_START, 2.0f);
            //SetFogParameter(gl.GL_FOG_END, 4.0f);
        }

    }
}
