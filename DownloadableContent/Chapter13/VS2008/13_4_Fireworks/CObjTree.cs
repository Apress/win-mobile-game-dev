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
using System.Reflection;
using System.Text;
using OpenGLES;

namespace Fireworks
{
    class CObjTree : GameEngineCh13.CGameObjectOpenGLBase
    {
        // Our reference to the game engine.
        private CFireworksGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjTree(CFireworksGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Load the tree graphic if not already loaded
            if (!_myGameEngine.GameGraphics.ContainsKey("Tree"))
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                Texture tex = Texture.LoadStream(asm.GetManifestResourceStream("Fireworks.Graphics.Tree.png"), true);
                _myGameEngine.GameGraphics.Add("Tree", tex);
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
            // Enable lighting
            gl.Enable(gl.GL_LIGHTING);

            // Bind to the texture we want to render with
            gl.BindTexture(gl.GL_TEXTURE_2D, _myGameEngine.GameGraphics["Tree"].Name);

            // Set the camera position
            _myGameEngine.LoadCameraMatrix(interpFactor);

            // Translate the object
            gl.Translatef(GetDisplayXPos(interpFactor), GetDisplayYPos(interpFactor), GetDisplayZPos(interpFactor));

            // Rotate the matrix to face the camera
            _myGameEngine.RotateMatrixToCamera(true);

            // Scale the tree
            gl.Scalef(0.25f, 1, 1);

            // Enable alpha blending
            gl.Enable(gl.GL_BLEND);
            gl.BlendFunc(gl.GL_SRC_ALPHA, gl.GL_ONE_MINUS_SRC_ALPHA);

            // Render the tree
            RenderTextureQuad();

            // Disable alpha blending
            gl.Disable(gl.GL_BLEND);
        }

    }
}
