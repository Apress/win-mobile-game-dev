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
using System.Drawing.Imaging;
using System.Text;

namespace ObjectSelection
{
    internal class CObjBox : CObjSelectableBase
    {
        // Our reference to the game engine.
        // Note that this is typed as CObjectSelectionGame, not as CGameEngineGDIBase
        private CObjectSelectionGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CObjectSelectionGame class as a parameter.
        /// </summary>
        public CObjBox(CObjectSelectionGame gameEngine) : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Set the width and height of the ball.
            Width = _myGameEngine.GameForm.ClientRectangle.Width / 5;
            Height = Width;

            // Set a random position for the ball's starting location.
            XPos = _myGameEngine.Random.Next(0, (int)(_myGameEngine.GameForm.Width - Width));
            YPos = _myGameEngine.Random.Next(0, (int)(_myGameEngine.GameForm.Height - Height));
        }


        //-------------------------------------------------------------------------------------
        // Object functions
        
        /// <summary>
        /// Render the ball to the provided Graphics object
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);
            Rectangle ballRect;

            // Get the object's RenderRectangle
            ballRect = GetRenderRectangle(interpFactor);

            // Reduce the width and height by 1 pixel to keep within the render bounds
            ballRect.Width -= 1;
            ballRect.Height -= 1;

            using (Brush b = new SolidBrush(Selected ? Color.White : Color.Violet))
            {
                gfx.FillRectangle(b, ballRect);
            }
            using (Pen p = new Pen(Color.Black))
            {
                gfx.DrawRectangle(p, ballRect);
            }
        }

     }
}
