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
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace GemDrops
{
    class CGemDropsGame : GameEngineCh8.CGameEngineGDIBase
    {
        /// <summary>
        /// Our strongly-typed reference to the main game form.
        /// </summary>
        private MainForm _gameForm;

        // The dimensions of the game board (gems across and gems down)
        public const int BOARD_GEMS_ACROSS = 7;
        public const int BOARD_GEMS_DOWN = 15;
        
        // Create an array to hold the game board -- all our dropped gems will appear here
        private CObjGem[,] _gameBoard = new CObjGem[BOARD_GEMS_ACROSS, BOARD_GEMS_DOWN];
        // Declare an array to hold the gems that are currently dropping under player control
        private CObjGem[] _playerGems = new CObjGem[2];
        // Declare an array to hold the next gems that will be brought into play
        private CObjGem[] _playerNextGems = new CObjGem[2];

        // Track whether the game has finished
        private bool _gameOver;
        // Track whether the game is paused
        private bool _paused;

        // Information about the tiles and the board
        private int _gemWidth;
        private int _gemHeight;
        private int _boardLeft;
        private int _boardTop;

        // The player's current score
        private int _playerScore;
        // The score multiplier. When groups are removed which form more groups to form, this will be increased by each subsequent group removal.
        private int _scoreMultiplier;

        // The number of pieces that have dropped into the game. We'll use this to gradually increase the game difficulty
        private int _piecesDropped;

        /// <summary>
        /// An enumeration to allow us to identify the location of the "rotating" gem
        /// relative to the "static" gem around which it rotates.
        /// </summary>
        private enum gemPosition
        {
            Above,
            Right,
            Below,
            Left
        };

        // Gems 0 to 5 are normal colored gems, but gem color 6 is the "rainbow" gem
        // which eliminates all of the gems of the color it lands upon.
        public const int GEMCOLOR_RAINBOW = 6;
        // The GetGemColor can return some special values to indicate
        // that the requested element is empty or off the edge of the board.
        public const int GEMCOLOR_NONE = -1;
        public const int GEMCOLOR_OUTOFBOUNDS = -2;

        // The BASS wrapper instance
        private BassWrapper _bassWrapper = new BassWrapper();

        /// <summary>
        /// Class constructor -- require an instance of the form that we will be running against to be provided
        /// </summary>
        public CGemDropsGame(Form gameForm)
            : base(gameForm)
        {
            // Store a strongly-typed reference to the game form
            _gameForm = (MainForm)gameForm;
        }


        //------------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Allow the game to query or set whether the game is over
        /// </summary>
        public bool GameOver
        {
            get
            {
                return _gameOver;
            }
            private set
            {
                // Set the gameover flag
                _gameOver = true;

                // Tell the form to do whatever it needs to do when the game is over
                _gameForm.GameOver();
            }
        }

        /// <summary>
        /// Set or return the game's Paused state
        /// </summary>
        public bool Paused
        {
            get
            {
                return _paused;
            }
            set
            {
                _paused = value;
            }
        }

        /// <summary>
        /// Return our instance of the BassWrapper class
        /// </summary>
        public BassWrapper BassWrapper
        {
            get
            {
                return _bassWrapper;
            }
        }

        /// <summary>
        /// Return the width in pixels of each gem graphic
        /// </summary>
        public int GemWidth
        {
            get
            {
                return _gemWidth;
            }
        }
        /// <summary>
        /// Return the height in pixels of each gem graphic
        /// </summary>
        public int GemHeight
        {
            get
            {
                return _gemHeight;
            }
        }
        /// <summary>
        /// Return the left coordinate of the game board in pixels
        /// </summary>
        public int BoardLeft
        {
            get
            {
                return _boardLeft;
            }
        }
        /// <summary>
        /// Return the top coordinate of the game board in pixels
        /// </summary>
        public int BoardTop
        {
            get
            {
                return _boardTop;
            }
        }


        //------------------------------------------------------------------------------------------
        // Game methods

        /// <summary>
        /// Prepare the game engine for use.
        /// </summary>
        public override void Prepare()
        {
            // Allow the base class to do its work.
            base.Prepare();

            // Get a reference to our assembly
            Assembly asm = Assembly.GetExecutingAssembly();

            // Initialize graphics library.
            // Which graphics set are we using?
            if (GameForm.ClientRectangle.Height < 480)
            {
                // The screen height is insufficient for the large graphics set,
                // so load the small graphics
                GameGraphics.Clear();
                GameGraphics.Add("Gems", new Bitmap(asm.GetManifestResourceStream("GemDrops.Graphics.Gems16.png")));
                _gemWidth = 21;
                _gemHeight = 16;
            }
            else
            {
                // We have enough space to use the large graphics set
                GameGraphics.Clear();
                GameGraphics.Add("Gems", new Bitmap(asm.GetManifestResourceStream("GemDrops.Graphics.Gems32.png")));
                _gemWidth = 42;
                _gemHeight = 32;
            }

            // Position the board within the window
            _boardLeft = (GameForm.ClientSize.Width - (GemWidth * BOARD_GEMS_ACROSS)) / 2;
            _boardTop = (GameForm.ClientSize.Height - (GemHeight * BOARD_GEMS_DOWN)) / 2;

            // Set the background color
            BackgroundColor = Color.Black;

            // Ensure all the sound effects are loaded
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.Group1.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.Group2.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.Group3.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.Group4.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.Group5.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.RainbowGem.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.NewPiece.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.GameOver.mp3", 2, false);
            _bassWrapper.LoadSoundEmbedded(asm, "GemDrops.Sounds.Doobrey.mod", 1, true);

        }

        /// <summary>
        /// Generate the background image for the game.
        /// </summary>
        protected override Bitmap CreateBackgroundImage()
        {
            Bitmap backImage;
            float y, yadd = 1;
            int barBrightness;
            int i;

            // Create the bitmap that we will paint within
            backImage = new Bitmap(GameForm.Width, GameForm.Height);

            using (Graphics gfx = Graphics.FromImage(backImage))
            {
                // Clear the background
                gfx.Clear(Color.Black);

                // Draw horizontal bars to form the background
                y = _gameForm.lblScore.ClientRectangle.Height;
                while (y < GameForm.Height)
                {
                    // Draw the light bar
                    // First we calculate the brightness of the bar.
                    // Multiply y by the brightness range, then add the brightness lower bound.
                    // Make sure that these don't exceed 255.
                    barBrightness = ((int)(y * 100) / GameForm.Height) + 50;
                    using (Brush b = new SolidBrush(Color.FromArgb((int)(barBrightness * 0.8), (int)(barBrightness * 0.8), barBrightness)))
                    {
                        gfx.FillRectangle(b, 0, (int)y, GameForm.ClientRectangle.Width, (int)yadd + 1);
                    }
                    y += yadd;
                    yadd *= 1.1f;

                    // Draw the dark bar
                    barBrightness = (int)(barBrightness * 0.75f);
                    using (Brush b = new SolidBrush(Color.FromArgb((int)(barBrightness * 0.8), (int)(barBrightness * 0.8), barBrightness)))
                    {
                        gfx.FillRectangle(b, 0, (int)y, GameForm.ClientRectangle.Width, (int)yadd + 1);
                    }
                    y += yadd;
                    yadd *= 1.1f;
                }

                // Draw horizontal lines to separate out the three touchscreen segments
                Color[] segmentColors = { Color.FromArgb(140, 140, 140), Color.FromArgb(180, 180, 180), Color.FromArgb(140, 140, 140) };
                for (i = 0; i < segmentColors.Length; i++)
                {
                    using (Pen p = new Pen(segmentColors[i]))
                    {
                        gfx.DrawLine(p, 0, (GameForm.Height / 3) + i, GameForm.Width, (GameForm.Height / 3) + i);
                        gfx.DrawLine(p, 0, (GameForm.Height * 2 / 3) + i, GameForm.Width, (GameForm.Height * 2 / 3) + i);
                    }
                }

                // Draw the game board outline
                Color[] frameColors = { Color.Black,                            // Innermost color
                                          Color.FromArgb(80, 80, 80),
                                          Color.FromArgb(160, 160, 160),
                                          Color.FromArgb(255, 255, 255),
                                          Color.FromArgb(192, 192, 192),
                                          Color.FromArgb(128, 128, 128),
                                          Color.FromArgb(64, 64, 64) };         // Outermost color
                for (i = frameColors.Length - 1; i >= 0; i--)
                {
                    using (Brush b = new SolidBrush(frameColors[i]))
                    {
                        gfx.FillRectangle(b, BoardLeft - i, BoardTop, BOARD_GEMS_ACROSS * GemWidth + i * 2, (BOARD_GEMS_DOWN) * GemHeight + i);
                    }
                }

            }

            return backImage;
        }

        /// <summary>
        /// Reset the game to its detault state
        /// </summary>
        public override void Reset()
        {
            base.Reset();

            // Reset game variables
            _gameOver = false;
            _playerScore = 0;
            _piecesDropped = 0;

            // Clear any existing game objects
            GameObjects.Clear();

            // Clear the game board
            ClearBoard();

            // Initialize the "next piece" gems
            InitNextGems();

            // Initialize two new gems for the player to control
            InitPlayerGems();

            // Ensure the information displayed on the game form is up to date
            UpdateForm();

            // Start the music playing
            _bassWrapper.PlaySound("GemDrops.Sounds.Doobrey.mod", 0.5f);
        }

        /// <summary>
        /// Advance the simulation by one frame
        /// </summary>
        public override void Advance()
        {
            // If we are paused then do nothing...
            if (_paused) return;

            // Otherwise get the base class to process as normal
            base.Advance();
        }

        /// <summary>
        /// Update the game
        /// </summary>
        public override void Update()
        {
            bool landed = false;

            // Call into the base class to perform its work
            base.Update();

            // If the game has finished then there's nothing more to do
            if (_gameOver) return;

            // Have the gems reached the bottom of the space they were dropping into?
            if (_playerGems[0] != null && _playerGems[0].FallDistance <= 0)
            {
                // They have...
                // Has either gem landed?
                for (int i = 0; i < 2; i++)
                {
                    // See if the gem is at the bottom of the board, or is
                    // immediately above a location that is occupied by another gem.
                    if (_playerGems[i].BoardYPos == CGemDropsGame.BOARD_GEMS_DOWN - 1 || _gameBoard[_playerGems[i].BoardXPos, _playerGems[i].BoardYPos + 1] != null)
                    {
                        landed = true;
                    }
                }
                if (landed)
                {
                    // Yes...
                    // At this stage we need to make the game become part of the main game board
                    MovePlayerGemsToBoard();

                    // Get any gems that are left floating to fall to the bottom
                    DropGems();

                    // We won't Initialize any new player gems at this point, as we want to allow any gems
                    // within the board that are falling to complete their descent first.
                    // The gems will be re-Initialized in the next Update loop once all falling gems have landed.
                }
                else
                {
                    // We haven’t landed. Move the gem to the next row down.
                    _playerGems[0].BoardYPos += 1;
                    _playerGems[1].BoardYPos += 1;
                    // Reset the distance to fall until they reach the bottom of the row.
                    _playerGems[0].FallDistance += 1;
                    _playerGems[1].FallDistance += 1;
                }
            }

            // Are player gems active at the moment?
            if (_playerGems[0] == null)
            {
                // No...
                // Are there any gems currently dropping?
                if (!GemsAreDropping)
                {
                    // No, so we can look for any groups to remove.
                    if (RemoveGems())
                    {
                        // Gems were removed, so allow the next drop to take place
                    }
                    else
                    {
                        // Everything is up to date so we can Initialize some new player gems
                        InitPlayerGems();
                    }
                }
            }
        }

        /// <summary>
        /// When the game determines that one of the player gems has landed,
        /// this function will move the gems out of player control and place
        /// them on to the game board.
        /// </summary>
        private void MovePlayerGemsToBoard()
        {
            CObjGem gem;

            for (int i = 0; i < 2; i++)
            {
                // Create a new gem for the board with the color from the first player gem
                gem = new CObjGem(this, _playerGems[i].BoardXPos, _playerGems[i].BoardYPos, _playerGems[i].GemColor);
                // Set it as an "on the board" gem
                gem.GemType = CObjGem.GemTypes.OnTheBoard;
                // Put the gem object into the board array
                _gameBoard[gem.BoardXPos, gem.BoardYPos] = gem;
                // Add the gem to the game engine's GameObjects collection
                GameObjects.Add(gem);

                // Tell the player gem object to terminate itself.
                // This will remove it from the GameObjects collection but ensures
                // that it is properly re-rendered before disappearing
                _playerGems[i].Terminate = true;

                // Clear the reference to the gem so that we know there are no gems
                // under active player control at the moment
                _playerGems[i] = null;
            }
        }

        /// <summary>
        /// Copy information from the game on to the game form so that it can be
        /// seen by the player.
        /// </summary>
        private void UpdateForm()
        {
            // Make sure we have finished initializing and we have a form to update
            if (_gameForm != null)
            {
                // Set the player score
                _gameForm.SetScore(_playerScore);
            }
        }
        
        /// <summary>
        /// Remove all of the gems present on the board.
        /// </summary>
        private void ClearBoard()
        {
            for (int x=0; x<BOARD_GEMS_ACROSS; x++)
            {
                for (int y=0; y<BOARD_GEMS_DOWN; y++)
                {
                    // Does this location contain a gem?
                    if (_gameBoard[x,y] != null)
                    {
                        // Yes, so instruct it to terminate and then remove it from the board
                        _gameBoard[x, y].Terminate = true;
                        _gameBoard[x,y] = null;
                    }
                }
            }
        }


        /// <summary>
        /// Scan the board looking for gems that have gaps below them. All of these gems are updated
        /// so that they fall as far as they can.
        /// </summary>
        private void DropGems()
        {
            bool gemMoved;

            do
            {
                // Clear the gem moved flag, no gems have been dropped within this iteration yet
                gemMoved = false;

                // Loop for each column of the board
                for (int x = 0; x < BOARD_GEMS_ACROSS; x++)
                {
                    // Loop for each cell within the column.
                    // Note that we loop from the bottom of the board upwards,
                    // and that we don't include the top-most row (as there
                    // are no gems above this to drop)
                    for (int y = BOARD_GEMS_DOWN - 1; y > 0; y--)
                    {
                        // If this cell is empty and the one above it is not...
                        if (_gameBoard[x, y] == null && _gameBoard[x, y - 1] != null)
                        {
                            // ...then drop the gem in the cell above into this cell.
                            // First copy the object to the new array element and remove
                            // it from the original array element
                            _gameBoard[x, y] = _gameBoard[x, y - 1];
                            _gameBoard[x, y - 1] = null;
                            // Update the gem object's position
                            _gameBoard[x, y].BoardYPos = y;
                            // Indicate that it has an (additional) row to fall through
                            _gameBoard[x, y].FallDistance += 1;
                            // Remember that we have moved a gem
                            gemMoved = true;
                        }
                    }
                }
                // Keep looping around until no more gems movements are found.
            } while (gemMoved);
        }


        /// <summary>
        /// Determines whether any of the gems contained within the board and currently
        /// dropping towards their resting positions. Returns true if any gem is still
        /// falling, or false once all gems on the board have landed.
        /// </summary>
        private bool GemsAreDropping
        {
            get
            {
                // Loop for each column of the board
                for (int x = 0; x < BOARD_GEMS_ACROSS; x++)
                {
                    // Loop for each cell within the column.
                    for (int y = 0; y < BOARD_GEMS_DOWN; y++)
                    {
                        // Is there a gem in this location?
                        if (_gameBoard[x, y] != null)
                        {
                            // Is the gem dropping?
                            if (_gameBoard[x, y].FallDistance > 0)
                            {
                                // This gem is dropping so return true
                                return true;
                            }
                        }
                    }
                }

                // None of the gems are dropping so return false
                return false;
            }
        }

        /// <summary>
        /// Locate and remove gems that are ready to be removed from the board.
        /// Add to the player's score as appropriate.
        /// </summary>
        /// <returns>Returns true if any gems were removed, false if not.</returns>
        private bool RemoveGems()
        {
            bool gemsRemoved = false;

            // See if we can remove any linked groups of gems.
            gemsRemoved = RemoveGemGroups();

            // See if we can remove any rainbow gems
            gemsRemoved |= RemoveRainbowGems();

            // If any gems were removed then instruct those remaining gems to fall
            // into their lowest positions.
            if (gemsRemoved)
            {
                // Drop any gems that are now floating in space
                DropGems();
                // Increase the multiplier in case any more groups are formed
                _scoreMultiplier += 1;
                // Update the form to show the player's new score
                UpdateForm();
            }

            // Return a value indicating whether anything happened
            return gemsRemoved;
        }

        /// <summary>
        /// Look for groups of connected gems and remove them.
        /// </summary>
        /// <returns>Returns true if gems were removed, false if not.</returns>
        private bool RemoveGemGroups()
        {
            List<Point> connectedGems;
            Rectangle connectedRect;
            bool gemsRemoved = false;
            int groupScore;

            // Loop for each gem on the board
            for (int x = 0; x < BOARD_GEMS_ACROSS; x++)
            {
                for (int y = 0; y < BOARD_GEMS_DOWN; y++)
                {
                    // Is there a gem here?
                    if (_gameBoard[x, y] != null)
                    {
                        // Initialize a list to store the connected gem positions
                        connectedGems = new List<Point>();
                        // See if we have a group at this location
                        RemoveGemGroups_FindGroup(x, y, connectedGems);
                        // Is the group large enough?
                        if (connectedGems.Count >= 5)
                        {
                            // Remove all of the gems within the group.
                            // Retrieve a rectangle whose dimensions encompass the removed group
                            // (The rectangle will be in board coordinates, not in pixels)
                            connectedRect = RemoveGemGroups_RemoveGroup(connectedGems);
                            // Indicate that gems have been removed from the board
                            gemsRemoved = true;
                            // Add to the player's score
                            groupScore = connectedGems.Count * 10;
                            _playerScore += groupScore * _scoreMultiplier;
                            // Add a score object so that the score "floats" up from the removed group.
                            // Use the rectangle that we got back earlier to position this
                            // in the position that the group had occupied.
                            AddScoreObject((float)(connectedRect.Left + connectedRect.Right) / 2, (float)(connectedRect.Top + connectedRect.Bottom) / 2, groupScore, _scoreMultiplier);
                            // Play a sound effect
                            PlayGroupSound(_scoreMultiplier);
                        }
                    }
                }
            }

            return gemsRemoved;
        }

        /// <summary>
        /// Finds all gems on the game board that are connected to the one specified in the x and y parameters
        /// </summary>
        /// <param name="x">The x position on the board of a gem within a possible group</param>
        /// <param name="y">The y position on the board of a gem within a possible group</param>
        /// <param name="gemGroup">An empty List of Points into which all connected gem position will be added</param>
        private void RemoveGemGroups_FindGroup(int x, int y, List<Point> gemGroup)
        {
            int gemColor;

            // Do we already have an item at this position in the groupGems list?
            foreach (Point pt in gemGroup)
            {
                if (pt.X == x && pt.Y == y)
                {
                    // The gem at this position is already present so don't add it again.
                    return;
                }
            }

            // Add this gem to the list
            gemGroup.Add(new Point(x, y));

            // Read the color of gem at this location
            gemColor = _gameBoard[x, y].GemColor;

            // Are any of the connected gems of the same color?
            // If so, recurse into RemoveGems_RemoveGroup and add their gems to the group.
            if (GetGemColor(x + 1, y) == gemColor) RemoveGemGroups_FindGroup(x + 1, y, gemGroup);
            if (GetGemColor(x - 1, y) == gemColor) RemoveGemGroups_FindGroup(x - 1, y, gemGroup);
            if (GetGemColor(x, y + 1) == gemColor) RemoveGemGroups_FindGroup(x, y + 1, gemGroup);
            if (GetGemColor(x, y - 1) == gemColor) RemoveGemGroups_FindGroup(x, y - 1, gemGroup);
        }

        /// <summary>
        /// Remove all of the gems in the provided list of Points
        /// </summary>
        /// <param name="gemGroup">A list of Points, each of which contains the coordinate of one of the gems to be removed</param>
        /// <returns>Returns a rectangle whose bounds encompass the size of the group removed.</returns>
        private Rectangle RemoveGemGroups_RemoveGroup(List<Point> gemGroup)
        {
            int groupLeft, groupTop, groupRight, groupBottom;

            // Set the group boundaries to match the position of the first gem in the group
            groupLeft = gemGroup[0].X;
            groupTop = gemGroup[0].Y;
            groupRight = gemGroup[0].X;
            groupBottom = gemGroup[0].Y;

            // Do we already have an item at this position in the groupGems list?
            foreach (Point pt in gemGroup)
            {
                // Instruct this gem to terminate and then remove it from the board
                _gameBoard[pt.X, pt.Y].Terminate = true;
                _gameBoard[pt.X, pt.Y] = null;

                // If this position is outside of our group boundary, extend the boundary
                if (pt.X < groupLeft) groupLeft = pt.X;
                if (pt.X > groupRight) groupRight = pt.X;
                if (pt.Y < groupTop) groupTop = pt.Y;
                if (pt.Y > groupBottom) groupBottom = pt.Y;
            }

            // Return a rectangle whose size encompasses the group boundary
            return Rectangle.FromLTRB(groupLeft, groupTop, groupRight, groupBottom);
        }

        /// <summary>
        /// Remove any rainbow gems and all gems of the type that they have landed on.
        /// </summary>
        /// <returns>Returns true if gems were removed, false if not.</returns>
        private bool RemoveRainbowGems()
        {
            bool gemsRemoved = false;
            int gemColor;

            // Now look for landed rainbow gems. These need to be removed, and we'll
            // also remove any matching gems of the color found beneath them.
            // Loop for each gem on the board
            for (int x = 0; x < BOARD_GEMS_ACROSS; x++)
            {
                for (int y = 0; y < BOARD_GEMS_DOWN; y++)
                {
                    // Is this a rainbow gem?
                    if (_gameBoard[x, y] != null && _gameBoard[x, y].GemColor == GEMCOLOR_RAINBOW)
                    {
                        // It is, so remove the gem from the game...
                        _gameBoard[x, y].Terminate = true;
                        _gameBoard[x, y] = null;
                        // Find the color of gem in the location below this one
                        gemColor = GetGemColor(x, y + 1);
                        // Is this a valid gem?
                        if (gemColor >= 0)
                        {
                            // Yes, so remove all gems of this color
                            RemoveGemGroups_RemoveColor(gemColor);
                        }
                        // Gems were removed -- the rainbow gem itself was at the very least
                        gemsRemoved = true;
                        // Play the rainbow gem sound
                        PlaySound("RainbowGem.mp3");
                    }
                }
            }

            return gemsRemoved;
        }

        /// <summary>
        /// Remove all of the gems of the specified color from the board.
        /// This is used to implement the Rainbow gem.
        /// </summary>
        /// <param name="gemColor">The color of gem to be removed</param>
        private int RemoveGemGroups_RemoveColor(int gemColor)
        {
            int x,y;
            int removed = 0;

            for (x = 0; x < BOARD_GEMS_ACROSS; x++)
            {
                for (y = 0; y < BOARD_GEMS_DOWN; y++)
                {
                    // Does this gem match the specified color?
                    if (GetGemColor(x, y) == gemColor)
                    {
                        // It does, so remove the gem from the game...
                        _gameBoard[x, y].Terminate = true;
                        _gameBoard[x, y] = null;
                        removed += 1;
                    }
                }
            }

            // Return the number of gems we removed
            return removed;
        }

        /// <summary>
        /// Determine the color of gem on the board at the specified location.
        /// </summary>
        /// <param name="x">The x coordinate to check</param>
        /// <param name="y">The y coordinate to check</param>
        /// <returns>Returns GEMCOLOR_NONE if the location is empty,
        /// GEMCOLOR_OUTOFBOUNDS if it is out-of-bounds, otherwise the
        /// gem color</returns>
        private int GetGemColor(int x, int y)
        {
            // Is the specified location valid?
            if (x < 0 || x >= BOARD_GEMS_ACROSS || y < 0 || y >= BOARD_GEMS_DOWN)
            {
                // This cell is out of bounds, so return the out of bounds value
                return GEMCOLOR_OUTOFBOUNDS;
            }

            // Is there a gem at the specified location?
            if (_gameBoard[x, y] == null)
            {
                // No, so return the "no gem" value
                return GEMCOLOR_NONE;
            }

            // Return the color of gem at the specified location
            return _gameBoard[x, y].GemColor;
        }

        /// <summary>
        /// Create the two "next piece" gems to display in the corner of the screen.
        /// This should be called just once per game as it is being reset.
        /// </summary>
        private void InitNextGems()
        {
            // Instantiate two new gems.
            // The gems have Y positions of 0 and 1 so that they appear one above the
            // other in the Next Piece display.
            // We also generate initial random colors for the two gems here too.
            _playerNextGems[0] = new CObjGem(this, 0, 0, GenerateRandomGemColor());
            _playerNextGems[1] = new CObjGem(this, 0, 1, GenerateRandomGemColor());

            // These are the 'next' gems -- this affects their position within the screen
            _playerNextGems[0].GemType = CObjGem.GemTypes.NextGem;
            _playerNextGems[1].GemType = CObjGem.GemTypes.NextGem;

            // Add the 'next' gems to the game
            GameObjects.Add(_playerNextGems[0]);
            GameObjects.Add(_playerNextGems[1]);
        }

        /// <summary>
        /// Create two new gems for the player to control.
        /// </summary>
        private void InitPlayerGems()
        {
            // Instantiate and initialize two new gems for the player to control.
            // Set the gem colors to be those colors stored for the next gems.
            _playerGems[0] = new CObjGem(this, BOARD_GEMS_ACROSS / 2, 0, _playerNextGems[0].GemColor);
            _playerGems[1] = new CObjGem(this, BOARD_GEMS_ACROSS / 2, 1, _playerNextGems[1].GemColor);

            // These are the player controlled gems -- this affects their movement within CObjGem
            _playerGems[0].GemType = CObjGem.GemTypes.PlayerControlled;
            _playerGems[1].GemType = CObjGem.GemTypes.PlayerControlled;

            // Set the gems as falling into the position we have set
            _playerGems[0].FallDistance = 1;
            _playerGems[1].FallDistance = 1;

            // Set the drop speed to gradually increase based on the number of pieces already dropped.
            _playerGems[0].FallSpeed = 0.02f + (_piecesDropped * 0.0004f);
            _playerGems[1].FallSpeed = _playerGems[0].FallSpeed;

            // Add the gems to the game
            GameObjects.Add(_playerGems[0]);
            GameObjects.Add(_playerGems[1]);

            // Check that the board space is actually available.
            // If not, the game is finished.
            CheckGameOver();
            if (GameOver)
            {
                // The game is finished, so no further work is required here
                return;
            }

            // Set two new 'next' gems
            _playerNextGems[0].GemColor = GenerateRandomGemColor();
            _playerNextGems[1].GemColor = GenerateRandomGemColor();
            // Flag the gems as having moved so that they are re-rendered in their
            // new colors.
            _playerNextGems[0].HasMoved = true;
            _playerNextGems[1].HasMoved = true;

            // Increase the pieces dropped count
            _piecesDropped += 1;

            // Reset the drop multiplier back to 1
            _scoreMultiplier = 1;

            // Play the "new piece" sound
            PlaySound("NewPiece.mp3");
        }

        /// <summary>
        /// Check to see whether the player gem board position is occupied by
        /// a gem already on the board. If this is the case when the player
        /// gems are first added, the board is full and the game is over.
        /// </summary>
        /// <remarks>If the game is found to be over, the GameOver property
        /// will be set to true.</remarks>
        private void CheckGameOver()
        {
            // Are the positions to which the player gems have been added already occupied?
            if (_gameBoard[_playerGems[1].BoardXPos, _playerGems[1].BoardYPos] != null)
            {
                // They are, so the game is finished...
                // Stop the player gems from moving
                _playerGems[0].FallSpeed = 0;
                _playerGems[1].FallSpeed = 0;

                // Initialize the game over sequence
                GameOver = true;
                PlaySound("GameOver.mp3");
            }
        }

        /// <summary>
        /// Returns a random gem color.
        /// </summary>
        /// <remarks>The range of colors returned will slowly increase as the
        /// game progresses.</remarks>
        private int GenerateRandomGemColor()
        {
            // There are six "normal" gems and the "rainbow" gem available.
            // We'll generate a gem at random based upon how many pieces the player has dropped.

            // For the first few turns, we'll generate just the first four gem colors
            if (_piecesDropped < 20) return Random.Next(0, 4);

            // For the next few turns, we'll generate the first five gem colors
            if (_piecesDropped < 40) return Random.Next(0, 5);

            // After 100 pieces, we'll have a 1-in-200 chance of generating a "rainbow" gem
            if (_piecesDropped >= 100 && Random.Next(200) == 0) return GEMCOLOR_RAINBOW;

            // Otherwise return any of the available gem colors
            return Random.Next(0, 6);
        }


        /// <summary>
        /// Move the player's gems left or right
        /// </summary>
        /// <param name="direction">-1 for left, 1 for right</param>
        public void MovePlayerGems(int direction)
        {
            // Don't allow any interaction if the game is finished or paused
            if (_gameOver || _paused) return;
            // Make sure we have some player gems in action
            if (_playerGems[0] == null) return;

            // Make sure direction only contains a value of -1 or 1.
            if (direction < 0) direction = -1;
            if (direction >= 0) direction = 1;

            // Make sure the board is clear to the left...
            if (GetGemColor(_playerGems[0].BoardXPos + direction, _playerGems[0].BoardYPos) == GEMCOLOR_NONE
                && GetGemColor(_playerGems[1].BoardXPos + direction, _playerGems[1].BoardYPos) == GEMCOLOR_NONE)
            {
                // The board is empty in the requested direction so move the player gems
                _playerGems[0].BoardXPos += direction;
                _playerGems[1].BoardXPos += direction;
            }
        }

        /// <summary>
        /// Rotate the player's gems
        /// </summary>
        /// <param name="clockwise">true to rotate clockwise, false for anti-clockwise</param>
        public void RotatePlayerGems(bool clockwise)
        {
            gemPosition position = gemPosition.Above;
            int newXPos;
            int newYPos;

            // Don't allow any interaction if the game is finished or paused
            if (_gameOver || _paused) return;
            // Make sure we have some player gems in action
            if (_playerGems[0] == null) return;

            // We will rotate gem[1] around gem[0], leaving the position of gem[0] unchanged
            // Determine the current position of gem[1] relative to gem[0].
            if (_playerGems[1].BoardYPos > _playerGems[0].BoardYPos) position = gemPosition.Below;
            if (_playerGems[1].BoardYPos < _playerGems[0].BoardYPos) position = gemPosition.Above;
            if (_playerGems[1].BoardXPos > _playerGems[0].BoardXPos) position = gemPosition.Right;
            if (_playerGems[1].BoardXPos < _playerGems[0].BoardXPos) position = gemPosition.Left;

            // Add to the position to rotate the gem
            position += (clockwise ? 1 : -1);
            // Loop around if we have gone out of bounds
            if (position > gemPosition.Left) position = gemPosition.Above;
            if (position < gemPosition.Above) position = gemPosition.Left;

            // Determine the new gem location. Start at the static gem's location...
            newXPos = _playerGems[0].BoardXPos;
            newYPos = _playerGems[0].BoardYPos;
            // And apply an offset based upon the rotating gem's position
            switch (position)
            {
                case gemPosition.Above:
                    newYPos -= 1;
                    break;
                case gemPosition.Below:
                    newYPos += 1;
                    break;
                case gemPosition.Left:
                    newXPos -= 1;
                    break;
                case gemPosition.Right:
                    newXPos += 1;
                    break;
            }

            // Is the newly requested gem position valid and unoccupied?
            if (GetGemColor(newXPos, newYPos) == GEMCOLOR_NONE)
            {
                // It is, so the rotation is OK to proceed.
                // Set the new position of the rotated gem
                _playerGems[1].BoardXPos = newXPos;
                _playerGems[1].BoardYPos = newYPos;
            }
        }

        /// <summary>
        /// Sets a value indicating whether the player gems are to drop
        /// 'quickly' (i.e., the player is pressing the Down button to speed their descent).
        /// </summary>
        public void DropQuickly(bool beQuick)
        {
            // Don't allow any interaction if the game is finished or paused
            if (_gameOver || _paused) return;
            // Make sure we have some player gems in action
            if (_playerGems[0] == null) return;

            // Tell the gems how we want them to move
            _playerGems[0].IsDroppingQuickly = beQuick;
            _playerGems[1].IsDroppingQuickly = beQuick;
        }


        /// <summary>
        /// Add a floating score object to the game
        /// </summary>
        /// <param name="x">The x position for the centre of the score</param>
        /// <param name="y">The y position for the centre of the score</param>
        /// <param name="score">The score to display</param>
        /// <param name="multiplier">The score multiplier</param>
        /// <remarks>Note that we pass the position as a float rather than as an int as we may position
        /// the score at a halfway position so that it is centered upon its related group of tiles.</remarks>
        private void AddScoreObject(float x, float y, int score, int multiplier)
        {
            CObjScore scoreObj;

            // Instantiate and Initialize a new score object
            scoreObj = new CObjScore(this, x + 0.5f, y, score, multiplier);

            // Add the score object to the game
            GameObjects.Add(scoreObj);
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="SoundName">The sound name within the Sounds resources
        /// folder that is to be played.</param>
        private void PlaySound(string SoundName)
        {
            // Add the resource path
            SoundName = "GemDrops.Sounds." + SoundName;

            // Play the sound
            _bassWrapper.PlaySound(SoundName);
        }

        /// <summary>
        /// Play one of the "group removed" sounds.
        /// </summary>
        /// <param name="Multiplier">The score multiplier for the group being removed</param>
        /// <remarks>This is just a wrapper around PlaySound which allows us to specify
        /// which multiplier group sound to play more easily.</remarks>
        private void PlayGroupSound(int Multiplier)
        {
            // We only play different sound effects up to a multiple of 5,
            // so limit to this value
            if (Multiplier > 5) Multiplier = 5;
            // Play the appropriate group sound
            PlaySound("Group" + Multiplier.ToString() + ".mp3");
        }

    }
}
