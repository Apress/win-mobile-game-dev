/**
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
    class CObjCamera : GameEngineCh13.CGameObjectOpenGLCameraBase
    {
        // Our reference to the game engine.
        private CCameraGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own game class as a parameter.
        /// </summary>
        public CObjCamera(CCameraGame gameEngine)
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
            XAngle += 1.5f;
            ZAngle += 1.27f;
            XPos = (float)Math.Cos(XAngle / 360 * glu.PI * 2) * 5;
            ZPos = (float)Math.Sin(ZAngle / 360 * glu.PI * 2) * 5;

            // Use the y angle to raise and lower the camera elevation
            YAngle += 1.0f;
            YPos = ((float)Math.Sin(YAngle / 360 * glu.PI * 2) + 1) * 5;
        }


    }
}
