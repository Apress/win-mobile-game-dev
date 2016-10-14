/**
 * 
 * GemDrops
 * 
 * An example game using the Windows Mobile Game Development game engine.
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GemDrops
{
    class CObjScore : GameEngineCh8.CGameObjectGDIBase
    {
        // Our reference to the game engine
        private CGemDropsGame _myGameEngine;

        // The x and y position (measured in game units) within the game board
        private float _boardXPos = 0;
        private float _boardYPos = 0;
        // The text to display within the score object
        private String _scoreText;
        // The distance (in board units) that we have floated up from our original position
        private float _floatDistance = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gameEngine">The GameEngine object instance</param>
        /// <param name="boardXPos">The x position for the score</param>
        /// <param name="boardYPos">The y position for the score</param>
        /// <param name="score">The score that was achieved</param>
        /// <param name="multiplier">The score multiplier</param>
        public CObjScore(CGemDropsGame gameEngine, float boardXPos, float boardYPos, int score, int multiplier)
            : base(gameEngine)
        {
            SizeF textSize;

            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            // Store the parameter values
            _boardXPos = boardXPos;
            _boardYPos = boardYPos;

            // Store the score text
            if (multiplier == 1)
            {
                _scoreText = score.ToString();
            }
            else
            {
                _scoreText = score.ToString() + " x " + multiplier.ToString();
            }

            // Determine the pixel size of the supplied text
            using (Graphics g = _myGameEngine.GameForm.CreateGraphics())
            {
                textSize = g.MeasureString(_scoreText, _myGameEngine.GameForm.Font);
            }
            // Set the game object's dimensions
            Width = (int)(textSize.Width * 1.2f);
            Height = (int)(textSize.Height * 1.2f);
        }

        //-------------------------------------------------------------------------------------
        // Game engine functions

        /// <summary>
        /// Move the object
        /// </summary>
        public override void Update()
        {
            // Allow the base class to perform any processing it needs
            base.Update();

            // Float upwards
            _floatDistance += 0.02f;

            // If we have floated far enough then terminate the object
            if (_floatDistance >= 2)
            {
                Terminate = true;
            }
        }

        /// <summary>
        /// Render the object to the screen
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Create a brush to write our text with
            using (Brush br = new SolidBrush(Color.White))
            {
                // Render the text at the appropriate location
                gfx.DrawString(_scoreText, _myGameEngine.GameForm.Font, br, new RectangleF((float)XPos, (float)YPos, (float)Width, (float)Height));
            }
        }

        /// <summary>
        /// Override the XPos property to calculate our actual position on demand
        /// </summary>
        public override float XPos
        {
            get
            {
                return _boardXPos * _myGameEngine.GemWidth + _myGameEngine.BoardLeft - Width / 2;
            }
        }

        /// <summary>
        /// Override the YPos property to calculate our actual position on demand
        /// </summary>
        public override float YPos
        {
            get
            {
                return (_boardYPos - _floatDistance) * _myGameEngine.GemHeight + _myGameEngine.BoardTop;
            }
        }

    }
}
