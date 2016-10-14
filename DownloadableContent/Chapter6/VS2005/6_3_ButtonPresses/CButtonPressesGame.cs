/**
 * 
 * ButtonPresses
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

namespace ButtonPresses
{
    class CButtonPressesGame : GameEngineCh6.CGameEngineGDIBase
    {

        /// <summary>
        /// The P/Invoke declaration for the GetAsyncKeyState function
        /// </summary>
        /// <param name="vkey">The key whose state is to be queried</param>
        [DllImport("coredll.dll")]
        public static extern int GetAsyncKeyState(Keys vkey); 


        /// <summary>
        /// Class constructor
        /// </summary>
        public CButtonPressesGame(Form gameForm)
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

            // Create a new ball. This will automatically generate a random position for itself.
            gameObj = new CObjBall(this);
            // Add the ball to the game engine
            GameObjects.Add(gameObj);
        }

        /// <summary>
        /// Update the state of the game
        /// </summary>
        public override void Update()
        {
            // Allow the base class to do its work
            base.Update();

            // Are we using the GetAsyncKeyState input method?
            if (((GameForm)GameForm).mnuMain_Menu_KeyState.Checked)
            {
                if (GetAsyncKeyState(Keys.Up) != 0) MoveBall(0, -5);
                if (GetAsyncKeyState(Keys.Down) != 0) MoveBall(0, 5);
                if (GetAsyncKeyState(Keys.Left) != 0) MoveBall(-5,0);
                if (GetAsyncKeyState(Keys.Right) != 0) MoveBall(5,0);
            }


        }

        /// <summary>
        /// Move the ball in the direction specified
        /// </summary>
        /// <param name="xAdd">The distance to move along the x axis</param>
        /// <param name="yAdd">The distance to move along the y axis</param>
        public void MoveBall(int xAdd, int yAdd)
        {
            // Update the position of the ball
            GameObjects[0].XPos += xAdd;
            GameObjects[0].YPos += yAdd;
        }

    }
}
