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

namespace GemDrops
{
    class CObjGem : GameEngineCh8.CGameObjectGDIBase
    {
        // Our reference to the game engine
        private CGemDropsGame _game;

        // The type of gem represented within this object...
        private GemTypes _gemType;
        // Possible gem types are:
        public enum GemTypes
        {
            OnTheBoard,             // A gem that has been placed on to the game board
            PlayerControlled,       // A gem that is moving under player control
            NextGem                 // A gem in the "next piece" display
        };

        // The color of this gem
        private int _gemColor = 0;
        // The X position within the board (in game units, so from 0 to 6)
        private int _boardXPos = 0;
        // The Y position within the board (in game units, so from 0 to 14)
        private int _boardYPos = 0;
        // The distance we have to fall before we reach the bottom of our current location
        private float _fallDistance = 0;
        // The speed at which we are falling.
        // When under player control, this will be a constant value based
        // on how long the game has been in progress.
        // When on the board, this will be accelerated to simulate gravity
        // so that the gem quickly falls to its destination location on the board.
        private float _fallSpeed = 0;
        // A flag to control whether the gem is falling quickly if under player control
        private bool _isFallingQuickly = false;

        // A value to track the time since the "rainbow gem" image was last cycled
        private float _rainbowGemUpdateTime;
        // The current frame being displayed for the rainbow gem.
        private int _rainbowGemFrame;

        /// <summary>
        /// Constructor
        /// </summary>
        public CObjGem(CGemDropsGame gameEngine)
            : base(gameEngine)
        {
            // Store a reference to the game engine as its derived type
            _game = gameEngine;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boardX">The x position for the gem</param>
        /// <param name="boardY">The y position for the gem</param>
        /// <param name="color">The color for the gem</param>
        public CObjGem(CGemDropsGame gameEngine, int boardX, int boardY, int color)
            : this(gameEngine)
        {
            _boardXPos = boardX;
            _boardYPos = boardY;
            _gemColor = color;
        }


        //-------------------------------------------------------------------------------------
        // Property access


        public GemTypes GemType
        {
            get
            {
                return _gemType;
            }
            set
            {
                _gemType = value;
            }
        }
        
        public int GemColor
        {
            get
            {
                return _gemColor;
            }
            set
            {
                _gemColor = value;
            }
        }

        public int BoardXPos
        {
            get
            {
                return _boardXPos;
            }
            set
            {
                _boardXPos = value;
            }
        }

        public int BoardYPos
        {
            get
            {
                return _boardYPos;
            }
            set
            {
                _boardYPos = value;
            }
        }

        public float FallDistance
        {
            get
            {
                return _fallDistance;
            }
            set
            {
                _fallDistance = value;
            }
        }

        public float FallSpeed
        {
            get
            {
                return _fallSpeed;
            }
            set
            {
                _fallSpeed = value;
            }
        }

        public bool IsDroppingQuickly
        {
            get
            {
                return _isFallingQuickly;
            }
            set
            {
                _isFallingQuickly = value;
            }
        }


        //-------------------------------------------------------------------------------------
        // Game engine functions

        /// <summary>
        /// Draw the gem to the screen
        /// </summary>
        public override void Render(Graphics gfx, float interpFactor)
        {
            System.Drawing.Imaging.ImageAttributes imgAttributes;

            base.Render(gfx, interpFactor);

            // Create the transparency key for the gem graphics
            imgAttributes = new System.Drawing.Imaging.ImageAttributes();
            imgAttributes.SetColorKey(Color.Black, Color.Black);

            // If this is not a rainbow gem...
            if (_gemColor != CGemDropsGame.GEMCOLOR_RAINBOW)
            {
                // Then just draw its image
                gfx.DrawImage(_game.GameGraphics["Gems"], GetRenderRectangle(interpFactor), _gemColor * (int)Width, 0, (int)Width, (int)Height, GraphicsUnit.Pixel, imgAttributes);
            }
            else
            {
                // This is a rainbow gem so we need to cycle its color.
                // Add the elapsed time to our class variable
                _rainbowGemUpdateTime += GameEngine.TimeSinceLastRender;
                // Has 0.1 seconds elapsed?
                if (_rainbowGemUpdateTime > 0.1f)
                {
                    // Yes, so advance to the next color
                    _rainbowGemUpdateTime = 0;
                    _rainbowGemFrame += 1;
                    // If we reach the rainbow gem position (pass the final
                    // actual gem color), move back to the first
                    if (_rainbowGemFrame == CGemDropsGame.GEMCOLOR_RAINBOW) _rainbowGemFrame = 0;
                }
                // Ensure the gem is considered as moved so that it re-renders in its new color.
                // This is important so that it still cycles when in the "next gem"
                // display which isn't otherwise moving.
                HasMoved = true;
                // Draw its image
                gfx.DrawImage(_game.GameGraphics["Gems"], GetRenderRectangle(interpFactor), _rainbowGemFrame * (int)Width, 0, (int)Width, (int)Height, GraphicsUnit.Pixel, imgAttributes);
            }
        }

        /// <summary>
        /// Update the gem's position
        /// </summary>
        public override void Update()
        {
            // Allow the base class to perform any processing it needs
            base.Update();

            switch (_gemType)
            {
                case GemTypes.NextGem:
                    // If this gem is part of the "next piece" display then there
                    // is nothing for us to do as these gems don't move
                    break;

                case GemTypes.OnTheBoard:
                    // If we have some falling to do, apply gravity now
                    if (_fallDistance > 0)
                    {
                        // Reduce the fall distance
                        _fallDistance -= _fallSpeed;
                        // Add to the fall speed to simulate gravity
                        _fallSpeed += 0.025f;

                        // Have we landed?
                        if (_fallDistance < 0)
                        {
                            // Yes, so ensure we don't pass our landing point, and cancel any further falling
                            _fallDistance = 0;
                            _fallSpeed = 0;

                        }
                    }
                    break;

                case GemTypes.PlayerControlled:
                    // This gem is under player control so allow it to gently drop towards the bottom of the board.
                    // We'll let the game itself work out how to deal with it landing, etc.

                    // Are we dropping quickly or at normal speed?
                    if (_isFallingQuickly)
                    {
                        // Quickly, subtract a fairly fast constant value from _fallDistance.
                        _fallDistance -= 0.4f;
                    }
                    else
                    {
                        // Normal speed, subtract the gem’s _fallSpeed from _fallDistance.
                        _fallDistance -= _fallSpeed;
                    }
                    break;
            }
        }

        /// <summary>
        /// Override the XPos property to calculate our actual position on demand
        /// </summary>
        public override float XPos
        {
            get
            {
                switch (_gemType)
                {
                    case GemTypes.NextGem:
                        // This is a "next piece" gem so determine its position within the form
                        return ((MainForm)(_game.GameForm)).ClientRectangle.Width - Width;
                    default:
                        // This is an "in-board" gem so determine its position within the board
                        return _game.BoardLeft + (_boardXPos * Width);
                }
            }
        }

        /// <summary>
        /// Override the YPos property to calculate our actual position on demand
        /// </summary>
        public override float YPos
        {
            get
            {
                switch (_gemType)
                {
                    case GemTypes.NextGem:
                        // This is a "next piece" gem so determine its position within the form
                        return ((MainForm)(_game.GameForm)).lblNextPiece.ClientRectangle.Bottom + (_boardYPos * Height);
                    default:
                        // This is an "in-board" gem so determine its position within the board
                        return _game.BoardTop + ((_boardYPos - _fallDistance) * Height);
                }
            }
        }

        /// <summary>
        /// Return the Width by querying the gem width from the game engine.
        /// This means that if the gem width changes (e.g., the screen orientation
        /// is changed resulting in new graphics being loaded), we always return
        /// the correct value.
        /// </summary>
        public override int Width
        {
            get
            {
                return _game.GemWidth;
            }
        }
        /// <summary>
        /// Return the Width by querying the gem width from the game engine.
        /// This means that if the gem width changes (e.g., the screen orientation
        /// is changed resulting in new graphics being loaded), we always return
        /// the correct value.
        /// </summary>
        public override int Height
        {
            get
            {
                return _game.GemHeight;
            }
        }

    }
}
