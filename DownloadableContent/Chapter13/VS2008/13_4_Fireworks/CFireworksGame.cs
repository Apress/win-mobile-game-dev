/**
 * 
 * Fireworks
 * 
 * An OpenGL example project demonstraing the use of billboards.
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

namespace Fireworks
{
    class CFireworksGame : GameEngineCh13.CGameEngineOpenGLBase
    {

        private CObjCamera _camera;
        private int _timeToNextFirework = 0;

        //-------------------------------------------------------------------------------------
        // Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CFireworksGame(Form gameForm)
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
                BackgroundColor = Color.FromArgb(20, 20, 30);
            }
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public override void Reset()
        {
            GameEngineCh13.CGameObjectOpenGLBase obj;

            base.Reset();

            // Clear existing objects
            GameObjects.Clear();

            // Add the camera
            _camera = new CObjCamera(this);
            GameObjects.Add(_camera);

            // Add some houses to the game
            obj = new CObjHouse(this);
            obj.XPos = -2;
            obj.ZPos = 2;
            GameObjects.Add(obj);

            obj = new CObjHouse(this);
            obj.XPos = 2;
            obj.ZPos = 2;
            GameObjects.Add(obj);

            obj = new CObjHouse(this);
            obj.XPos = -2;
            obj.ZPos = -2;
            GameObjects.Add(obj);

            obj = new CObjHouse(this);
            obj.XPos = 2;
            obj.ZPos = -2;
            GameObjects.Add(obj);

            obj = new CObjHouse(this);
            obj.XPos = 0;
            obj.ZPos = 0;
            GameObjects.Add(obj);

            // Add some trees to the game
            obj = new CObjTree(this);
            obj.XPos = -2;
            obj.ZPos = 0;
            GameObjects.Add(obj);

            obj = new CObjTree(this);
            obj.XPos = 2;
            obj.ZPos = 0;
            GameObjects.Add(obj);

            obj = new CObjTree(this);
            obj.XPos = 0;
            obj.ZPos = -2;
            GameObjects.Add(obj);

            obj = new CObjTree(this);
            obj.XPos = 0;
            obj.ZPos = 2;
            GameObjects.Add(obj);

            // Reset to the identity matrix
            gl.LoadIdentity();

            // Initialize lighting
            InitLight(false);
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
                SetAmbientLight(new float[] { 0.3f, 0.3f, 0.3f, 1 });

                // Set the material color
                SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_AMBIENT, new float[] { 1, 1, 1, 1 });
                SetMaterialParameter(gl.GL_FRONT_AND_BACK, gl.GL_DIFFUSE, new float[] { 1, 1, 1, 1 });

                // Configure light 0.
                SetLightParameter(gl.GL_LIGHT0, gl.GL_DIFFUSE, new float[] { 0.6f, 0.6f, 0.4f, 1 });
                gl.Enable(gl.GL_LIGHT0);
            }

            // Set the light positions
            SetLightParameter(gl.GL_LIGHT0, gl.GL_POSITION, new float[] { 2, 20, 2, 1 });
        }


        public override void Update()
        {
            base.Update();

            // Time for another firework?
            _timeToNextFirework -= 1;
            if (_timeToNextFirework < 0)
            {
                // Add a firework
                GameObjects.Add(new CObjFirework(this));

                // Reset the timer
                _timeToNextFirework = Random.Next(100, 200);
            }

        }

    }
}
