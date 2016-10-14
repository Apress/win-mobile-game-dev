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
    class CObjCamera : GameEngineCh13.CGameObjectOpenGLCameraBase
    {
        // Our reference to the game engine.
        private CFireworksGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own game class as a parameter.
        /// </summary>
        public CObjCamera(CFireworksGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set the initial camera position
            Update();
        }

        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Set the camera position
            _myGameEngine.LoadCameraMatrix(interpFactor);
            // Set the light positions
            _myGameEngine.InitLight(true);
        }

        public override void Update()
        {
            base.Update();

            // Use the x and z angle to rotate around the y axis
            XAngle += 1.0f;
            ZAngle += 1.0f;
            XPos = (float)Math.Cos(XAngle / 360 * glu.PI * 2) * 10;
            ZPos = (float)Math.Sin(ZAngle / 360 * glu.PI * 2) * 10;

            // Set the camera's vertical position and direction
            YPos = 4;
            YCenter = 3;
        }
    }
}
