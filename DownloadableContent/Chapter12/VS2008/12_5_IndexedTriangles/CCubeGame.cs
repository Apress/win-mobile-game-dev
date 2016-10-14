/**
 * 
 * IndexedTriangles
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

namespace IndexedTriangles
{
    class CCubeGame : GameEngineCh12.CGameEngineOpenGLBase
    {


        //-------------------------------------------------------------------------------------
        // Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        public CCubeGame(Form gameForm)
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
                BackgroundColor = Color.LightGray;
            }
        }



        public override void Reset()
        {
            base.Reset();

            // Clear existing objects
            GameObjects.Clear();
            // Add a cube to the game
            GameObjects.Add(new CObjCube(this));
        }


    }
}
