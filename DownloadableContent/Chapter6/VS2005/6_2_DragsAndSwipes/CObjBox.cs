/**
 * 
 * DragsAndSwipes
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates dragging and throwing game objects.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace DragsAndSwipes
{
    internal class CObjBox : CObjDraggableBase
    {
        // Our reference to the game engine.
        // Note that this is typed as CDragsAndSwipesGame, not as CGameEngineGDIBase
        private CDraggableObjectGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CDragsAndSwipesGame class as a parameter.
        /// </summary>
        public CObjBox(CDraggableObjectGame gameEngine) : base(gameEngine)
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

            // Reduce the width and height to keep within the render bounds
            ballRect.Width -= 2;
            ballRect.Height -= 2;

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
