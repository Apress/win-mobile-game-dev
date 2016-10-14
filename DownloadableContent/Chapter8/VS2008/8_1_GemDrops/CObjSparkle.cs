using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GemDrops
{

    class CObjSparkle : GameEngine.CGameObjectGDIBase
    {

        private CGemDropsGame _myGameEngine;

        private int _boardXPos = 0;
        private int _boardYPos = 0;

        private int _animFrame = 0;
        private int _animFrameDelay;
        private int _xoffset = 0;
        private int _yoffset = 0;

        private Bitmap sparkleGraphic;

        private CObjGem attachedGem;

        /// <summary>
        /// Constructor
        /// </summary>
        public CObjSparkle(CGemDropsGame gameEngine, int boardX, int boardY)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _myGameEngine = gameEngine;

            _boardXPos = boardX;
            _boardYPos = boardY;

            // Determine the offset (in pixels) from the centre of the gem image
            _xoffset = _myGameEngine.Random.Next(-_myGameEngine.tileWidth / 3, _myGameEngine.tileWidth / 3) + _myGameEngine.tileWidth / 2;
            _yoffset = _myGameEngine.Random.Next(-_myGameEngine.tileHeight / 3, _myGameEngine.tileHeight / 3) + _myGameEngine.tileHeight / 2;

            // Pick one of the two sparkle animations at random
            if (_myGameEngine.Random.Next(0, 2) == 0)
            {
                sparkleGraphic = _myGameEngine.GameGraphics["Sparkle"];
            }
            else
            {
                sparkleGraphic = _myGameEngine.GameGraphics["Twinkle"];
            }

            // Determine the width and height for this object
            Width = sparkleGraphic.Width / 8;
            Height = sparkleGraphic.Height;

            // Find the gem we are attached to
            attachedGem = _myGameEngine._gameBoard[boardX, boardY];

        }

   
        public override void Update()
        {
            // Allow the base class to perform any processing it needs
            base.Update();

            // If ever we find that our attached gem has moved or been terminated, terminate the sparkle
            if (attachedGem.Terminate || attachedGem.boardXPos != _boardXPos || attachedGem.boardYPos != _boardYPos)
            {
                // Terminate the sparkle
                Terminate = true;
                return;
            }

            // Move to the next animation frame
            _animFrameDelay += 1;
            if (_animFrameDelay == 10)
            {
                _animFrameDelay = 0;
                _animFrame += 1;
                HasMoved = true;
            }

            // If we have finished the animation then terminate
            if (_animFrame >= 8)
            {
                Terminate = true;
            }
        }


        public override void Render(Graphics gfx, float framePosition)
        {
            base.Render(gfx, framePosition);

            //// Create a brush to write our text with
            //using (Brush br = new SolidBrush(Color.White))
            //{
            //    // Render the text at the appropriate location
            //    gfx.DrawString(ScoreText, _myGameEngine.GameForm.Font, br, new RectangleF((float)XPos, (float)YPos, (float)Width, (float)Height));
            //}


            System.Drawing.Imaging.ImageAttributes imgAttributes;

            base.Render(gfx, framePosition);

            // Move transparency keys to a collection within CGameEngine2DBase?
            imgAttributes = new System.Drawing.Imaging.ImageAttributes();
            imgAttributes.SetColorKey(Color.Black, Color.Black);

            gfx.DrawImage(sparkleGraphic, RenderRectangle, _animFrame * (int)this.Width, 0, (int)this.Width, (int)this.Height, GraphicsUnit.Pixel, imgAttributes);

            imgAttributes.Dispose();

        }

        /// <summary>
        /// Override the XPos property to calculate our actual position on demand
        /// </summary>
        public override float XPos
        {
            get
            {
                return _myGameEngine.boardLeft + (_boardXPos * _myGameEngine.tileWidth) + _xoffset - (Width / 2);
            }
            set
            {
                base.XPos = value;
            }
        }

        /// <summary>
        /// Override the YPos property to calculate our actual position on demand
        /// </summary>
        public override float YPos
        {
            get
            {
                return _myGameEngine.boardTop + (_boardYPos * _myGameEngine.tileHeight) + _yoffset - (Height / 2);
            }
            set
            {
                base.YPos = value;
            }
        }


    }
}
