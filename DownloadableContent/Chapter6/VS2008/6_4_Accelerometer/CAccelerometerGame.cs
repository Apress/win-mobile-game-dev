/**
 * 
 * Accelerometer
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A demonstration of keyboard and button handling using both Form events and also the
 * GetAsyncKeyState function.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Accelerometer
{
    class CAccelerometerGame : GameEngineCh6.CGameEngineGDIBase
    {
        // A variable to hold our accelerometer sensor object
        Sensors.IGSensor _gSensor;


        /// <summary>
        /// Class constructor
        /// </summary>
        public CAccelerometerGame(Form gameForm)
            : base(gameForm)
        {
            // Create the GSensor object that will be used by this class
            _gSensor = Sensors.GSensorFactory.CreateGSensor();
        }

        /// <summary>
        /// Prepare the game
        /// </summary>
        public override void Prepare()
        {
            Assembly asm;

            // Allow the base class to do its work
            base.Prepare();

            BackgroundColor = Color.White;

            // Initialise graphics library
            if (GameGraphics.Count == 0)
            {
                asm = Assembly.GetExecutingAssembly();
                GameGraphics.Add("Ball", new Bitmap(asm.GetManifestResourceStream("Accelerometer.Graphics.ShinyBall.png")));
            }

        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public override void Reset()
        {
            GameEngineCh6.CGameObjectGDIBase gameObj;

            // Allow the base class to do its work
            base.Reset();

            // Clear any existing game objects.
            GameObjects.Clear();

            // Create a new ball. This will automatically generate a random position for itself.
            gameObj = new CObjBall(this);
            // Add the ball to the game engine
            GameObjects.Add(gameObj);
        }

        /// <summary>
        /// Update the game
        /// </summary>
        public override void Update()
        {
            // Get the base class to perform its work
            base.Update();

            // Retrieve the GSensor vector
            Sensors.GVector gVector = _gSensor.GetGVector();
            // Normalize the vector so that it has a length of 1
            gVector = gVector.Normalize();

            // Display the vector on the game form
            ((GameForm)GameForm).lblAccelerometerVector.Text = gVector.ToString();

            // Add the x and y values to the ball's velocity
            ((CObjBall)GameObjects[0]).XVelocity += (float)gVector.X * 3;
            ((CObjBall)GameObjects[0]).YVelocity += (float)gVector.Y * 3;
        }

    }
}
