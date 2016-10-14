/**
 * 
 * ObjectSelection
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates selecting objects within the engine by tapping them on the screen.
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

namespace ObjectSelection
{
    class CObjectSelectionGame : GameEngineCh6.CGameEngineGDIBase
    {

        /// <summary>
        /// Class constructor
        /// </summary>
        public CObjectSelectionGame(Form gameForm)
            : base(gameForm)
        {
            // No constructor code required for this project.
        }

        /// <summary>
        /// Prepare the game
        /// </summary>
        public override void Prepare()
        {
            // Allow the base class to do its work
            base.Prepare();

            BackgroundColor = Color.LightGray;
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

            // Add some balls
            for (int i = 0; i < 20; i++)
            {
                // What type of object shall we create, a ball or a box?
                if (Random.Next(0, 2) == 0)
                {
                    // Create a new ball. This will automatically generate a random position for itself.
                    gameObj = new CObjBall(this);
                }
                else
                {
                    // Create a new box. This will automatically generate a random position for itself.
                    gameObj = new CObjBox(this);
                }
                // Add the ball to the game engine
                GameObjects.Add(gameObj);
            }
        }

    }
}
