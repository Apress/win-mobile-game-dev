/**
 * 
 * Bounce
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A simple example of creating a game project based upon the GameEngine.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Bounce
{
    class CBounceGame : GameEngineCh4.CGameEngineGDIBase
    {

        /// <summary>
        /// Class constructor
        /// </summary>
        public CBounceGame(Form gameForm)
            : base(gameForm)
        {
            // No constructor code required for this project.
        }

        /// <summary>
        /// Prepare the game
        /// </summary>
        public override void Prepare()
        {
            Assembly asm;

            // Allow the base class to do its work
            base.Prepare();

            // Initialise graphics library
            if (GameGraphics.Count == 0)
            {
                asm = Assembly.GetExecutingAssembly();
                GameGraphics.Add("Ball", new Bitmap(asm.GetManifestResourceStream("Bounce.Graphics.Ball.png")));
            }
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        public override void Reset()
        {
            CObjBall ball;

            // Allow the base class to do its work
            base.Reset();

            // Clear any existing game objects.
            GameObjects.Clear();

            // Add some balls
            for (int i = 0; i < 20; i++)
            {
                // Create a new ball. This will automatically generate a random position for itself.
                ball = new CObjBall(this);
                // Add the ball to the game engine
                GameObjects.Add(ball);
            }
        }

    }
}
