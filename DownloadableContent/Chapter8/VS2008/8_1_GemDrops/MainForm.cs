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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GemDrops
{
    public partial class MainForm : Form
    {

        // Our instance of the GemDrops game
        CGemDropsGame _game;

        // Keep track of whether the form has been closed. This indicates to the rest
        // of the form code that it should terminate whatever it is doing.
        // Note: in .NET CF 3.5 we can use the form's IsDisposed instead of having
        // to set a flag, but this is not available in .NET CF 2.0.
        bool _formClosed = false;

        /// <summary>
        /// Form constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Instantiate our game and set its game form to be this form
            _game = new CGemDropsGame(this);
            _game.Reset();
        }


        /// <summary>
        /// Display the score on the screen
        /// </summary>
        /// <param name="score">The player's new score</param>
        internal void SetScore(int score)
        {
            lblScore.Text = "Score: " + score.ToString();
        }

        /// <summary>
        /// Pause or resume the game.
        /// </summary>
        /// <param name="paused"></param>
        private void Pause(bool paused)
        {
            // Cancel any drop quickly that may be in effect
            _game.DropQuickly(false);

            // Tell the game if we are paused
            _game.Paused = paused;

            // Have we paused or unpaused?
            if (paused)
            {
                // Paused. Configure the UI as required
                lblInfoTitle.Text = "Paused";
                pnlInfoPanel.Visible = true;
                mnuMain_Pause.Text = "Resume";
            }
            else
            {
                // Unpaused.
                lblInfoTitle.Visible = true;
                pnlInfoPanel.Visible = false;
                mnuMain_Pause.Text = "Pause";
                // Force a repaint so that the pause panel is overdrawn
                _game.ForceRepaint();
            }
        }

        /// <summary>
        /// Drive the game
        /// </summary>
        private void RenderLoop()
        {
            do
            {
                // If we lose focus, stop rendering
                if (!this.Focused)
                {
                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                    // Advance the game
                    _game.Advance();
                }

                // Process pending events
                Application.DoEvents();

                // If our window has been closed then return without doing anything more
                if (_formClosed) return;

                // Loop forever (or at least, until our game form is closed).
            } while (true);

        }

        /// <summary>
        /// Update the form to show the Game Over message.
        /// </summary>
        internal void GameOver()
        {
            // Display the gameover panel
            lblInfoTitle.Text = "Game Over";
            pnlInfoPanel.Visible = true;
        }


        /// <summary>
        /// Paint the background (or not, in our case)
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't allow the background to paint.
        }

        /// <summary>
        /// Draw the game
        /// </summary>
        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            _game.Present(e.Graphics);
        }

        /// <summary>
        /// Initialize the game form and begin rendering the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameForm_Load(object sender, EventArgs e)
        {
            // Enable timers
            timerPause.Interval = 700;
            timerPause.Enabled = true;

            // Show the game form and ensure that it repaints itself
            this.Show();
            this.Invalidate();

            // Begin the game's render loop
            RenderLoop();
        }

        /// <summary>
        /// Respond to the form resizing
        /// </summary>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Reposition form controls
            pnlInfoPanel.Location = new Point(this.ClientRectangle.Width - pnlInfoPanel.ClientRectangle.Width, this.ClientRectangle.Height - pnlInfoPanel.ClientRectangle.Height);
        }

        /// <summary>
        /// The form is about to close
        /// </summary>
        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
            _game.BassWrapper.Dispose();
        }

        /// <summary>
        /// The form has lost focus
        /// </summary>
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            // Pause the game
            if (!_game.GameOver) Pause(true);

            // Pause the sounds and music
            _game.BassWrapper.PauseAllSounds();
        }

        /// <summary>
        /// The form has gained focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Activated(object sender, EventArgs e)
        {
            _game.BassWrapper.ResumeAllSounds();
        }

        /// <summary>
        /// Process the player making contact with the screen
        /// </summary>
        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Which segment of the screen has the user pressed in?
            // - The top third will be used for rotating the piece
            // - The middle third will be used for moving the piece left and right
            // - The bottom third will be used to drop the piece quickly

            switch ((int)(e.Y * 3 / this.ClientRectangle.Height))
            {
                case 0:
                    // Top third, so we will deal with rotating the player's gems
                    // Did the user tap the left or right side of the window?
                    if (e.X < this.ClientRectangle.Width / 2)
                    {
                        // Left side, rotate anticlockwise
                        _game.RotatePlayerGems(false);
                    }
                    else
                    {
                        // Right side, rotate clockwise
                        _game.RotatePlayerGems(true);
                    }
                    break;

                case 1:
                    // Middle third, so we will deal with moving the player's gems
                    // Did the user tap the left or right side of the window?
                    if (e.X < this.ClientRectangle.Width / 2)
                    {
                        // Left side, move left
                        _game.MovePlayerGems(-1);
                    }
                    else
                    {
                        // Right side, move right
                        _game.MovePlayerGems(1);
                    }
                    break;

                case 2:
                    // Bottom third, so we will deal with dropping the gem more quickly
                    _game.DropQuickly(true);
                    break;

            }

        }

        /// <summary>
        /// Process the player releasing contact with the screen.
        /// </summary>
        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            // Cancel any "drop quickly" that may be in operation
            _game.DropQuickly(false);
        }

        /// <summary>
        /// Process the player pressing a key
        /// </summary>
        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    // Rotate the gems when Up is pressed
                    _game.RotatePlayerGems(true);
                    break;
                case Keys.Left:
                    // Move to the left with Left is pressed
                    _game.MovePlayerGems(-1);
                    break;
                case Keys.Right:
                    // Move to the right with Left is pressed
                    _game.MovePlayerGems(1);
                    break;
                case Keys.Down:
                    // Set the DropQuickly flag when down is pressed
                    _game.DropQuickly(true);
                    break;
            }
        }

        /// <summary>
        /// Process the user releasing a key
        /// </summary>
        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    // Clear the DropQuickly flag to return the gem to its normal speed
                    _game.DropQuickly(false);
                    break;
            }
        }

        /// <summary>
        /// The form has closed
        /// </summary>
        private void MainForm_Closed(object sender, EventArgs e)
        {
            // Flag the fact that the game form has closed. This indicates to the rest
            // of the form code that it should terminate whatever it is doing.
            _formClosed = true;
        }

        /// <summary>
        /// Process the "pause" timer. This is used to flash the "Paused" text on
        /// and off like the symbol displayed on a VCR.
        /// </summary>
        private void timerPause_Tick(object sender, EventArgs e)
        {
            // Is the game paused?
            if (_game.Paused && !_formClosed)
            {
                // Yes, so toggle the visibility of the Pause item
                lblInfoTitle.Visible = !lblInfoTitle.Visible;
            }
        }

        /// <summary>
        /// When the menu pops up, force a repaint of the game so that the menu image
        /// doesn't get left behind on the screen.
        /// </summary>
        private void mnuMain_Menu_Popup(object sender, EventArgs e)
        {
            _game.ForceRepaint();
        }

        /// <summary>
        /// The player has selected to pause or resume the game.
        /// </summary>
        private void mnuMain_Pause_Click(object sender, EventArgs e)
        {
            // Can't pause when the game is finished
            if (_game.GameOver) return;

            // Is the game already paused?
            if (_game.Paused)
            {
                // Yes, so resume
                Pause(false);
            }
            else
            {
                // No, so pause
                Pause(true);
            }
        }

        /// <summary>
        /// Allow the user to begin a new game.
        /// </summary>
        /// <param name="sender"></param>
        private void mnuMain_Menu_NewGame_Click(object sender, EventArgs e)
        {
            // Hide the information panel if it is displayed (game over/pause)
            pnlInfoPanel.Visible = false;
            // Make sure the game is not paused
            Pause(false);
            // Reset the game
            _game.Reset();
        }

        /// <summary>
        /// Allow the user to exit the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuMain_Menu_Quit_Click(object sender, EventArgs e)
        {
            // Close the form to end the game
            this.Close();
        }

    }
}