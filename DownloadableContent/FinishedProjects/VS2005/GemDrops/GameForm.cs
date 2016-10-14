using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GemDrops
{
    public partial class GameForm : Form
    {


        CGemDropsGame _game;
        Point _mouseDownPos;

        public GameForm()
        {
            InitializeComponent();

            // Instantiate our game and set its form to be this form
            _game = new CGemDropsGame(this);
            _game.Reset();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't allow the background to paint.
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {

            // Method 2: advance in the paint method. Disables all other event processing
            //_game.Advance();

            _game.RenderForeground(e.Graphics);

        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (this.Focused == false) return;

            gameTimer.Enabled = false;

            _game.Advance();

            gameTimer.Enabled = true;

        }

        private void GameForm_Load(object sender, EventArgs e)
        {
            // Method 1: advance using a timer. Enables event processing but slightly slower updates.
            fpsTimer.Enabled = true;
            gameTimer.Enabled = true;

            
            //this.Show();
            //this.Invalidate();
            //do
            //{
            //    _game.Update();
            //    Application.DoEvents();
            //} while (true);

        }

        private void mnuReset_Click(object sender, EventArgs e)
        {
            //_game = new CGemDropsGame(this);
            _game.Reset();

        }

        private void fpsTimer_Tick(object sender, EventArgs e)
        {
            if (_game != null && _game.FramesPerSecond > 0)
            {
                lblFPS.Text = "Frames per second: " + _game.FramesPerSecond.ToString();
                //lblFPS.Text = _game.FramesPerSecond.ToString();
            }
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            // Which segment of the screen has the user pressed in?
            // - The top third will be used for rotating the piece
            // - The middle third will be used for moving the piece left and right
            // - The bottom third will be used to drop the piece quickly

            switch ((int)(e.Y * 3 / this.ClientRectangle.Height))
            {
                case 0:
                    // Top third
                    _mouseDownPos = new Point(e.X, e.Y);
                    break;

                case 1:
                    // Middle third, so we will deal with moving the player's gems
                    // Did the user tap the left or right side of the window?
                    if (e.X < this.ClientRectangle.Width / 2)
                    {
                        // Left side
                        _game.MovePlayerGems(-1);
                    }
                    else
                    {
                        // Right side
                        _game.MovePlayerGems(1);
                    }
                    break;

                case 2:
                    // Bottom third, so we will deal with dropping the gem more quickly
                    _game.DropQuickly = true;
                    break;

            }

        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            int xDist;

            // Cancel any "drop quickly" that may be in operation
            _game.DropQuickly = false;

            // Which segment of the screen has the user released in?
            // - The top third will be used for rotating the piece
            switch ((int)(e.Y * 3 / this.ClientRectangle.Height))
            {
                case 0:
                    // Top third

                    // Find the distance on the x axis that the mouse has moved
                    xDist = e.X - _mouseDownPos.X;
                    // Does this account for more than 10% of the display width?
                    if (Math.Abs(xDist) >= this.ClientSize.Width * .1)
                    {
                        // Yes, so rotate the piece of we can.
                        if (xDist > 0)
                        {
                            _game.RotatePlayerGems(1);
                        }
                        else
                        {
                            _game.RotatePlayerGems(-1);
                        }
                    }

                    break;

            }
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _game.RotatePlayerGems(1);
                    break;
                case Keys.Left:
                    _game.MovePlayerGems(-1);
                    break;
                case Keys.Right:
                    _game.MovePlayerGems(1);
                    break;
                case Keys.Down:
                    _game.DropQuickly = true;
                    break;
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Down:
                    _game.DropQuickly = false;
                    break;
            }
        }


    }
}