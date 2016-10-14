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
using System.Drawing.Imaging;
using System.Text;

namespace ButtonPresses
{
    internal class CObjBall : GameEngineCh6.CGameObjectGDIBase
    {
        // Our reference to the game engine.
        // Note that this is typed as CButtonPressesGame, not as CGameEngineGDIBase
        private CButtonPressesGame _myGameEngine;

        /// <summary>
        /// Constructor. Require an instance of our own CButtonPressesGame class as a parameter.
        /// </summary>
        public CObjBall(CButtonPressesGame gameEngine) : base(gameEngine)
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

            using (Brush b = new SolidBrush(Color.White))
            {
                gfx.FillEllipse(b, ballRect);
            }
            using (Pen p = new Pen(Color.Black))
            {
                gfx.DrawEllipse(p, ballRect);
            }
        }


        /// <summary>
        /// Determine whether the testPoint falls inside the circular area of the object
        /// </summary>
        protected override bool IsPointInObject(float interpFactor, Point testPoint)
        {
            Rectangle rect;
            Point center;
            float radius;
            int xdist, ydist;
            float pointDistance;
            
            // See if we are in the render rectangle. If not, we can exit without
            // processing any further. The base class can check this for us.
            if (!base.IsPointInObject(interpFactor, testPoint))
            {
                return false;
            }

            // Find the current render rectangle
            rect = GetRenderRectangle(interpFactor);
            // Calculate the center point of the rectangle (and therefore of the circle)
            center = new Point(rect.X + rect.Width / 2 - 1, rect.Y + rect.Height / 2 - 1);
            // The radius is half the width of the circle
            radius = rect.Width / 2;

            // Find the distance along the x and y axis between the test point and the center
            xdist = testPoint.X - center.X;
            ydist = testPoint.Y - center.Y;

            // Find the distance between the touch point and the center of the circle
            pointDistance = (float)Math.Sqrt(xdist * xdist + ydist * ydist);

            // Return true if this is less than or equal to the radius, false if it is greater
            return (pointDistance <= radius);
        }

    }
}
