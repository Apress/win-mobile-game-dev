/**
 * 
 * Bounce
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * A simple example of creating a game project based upon the GameEngine.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bounce
{
    public partial class GameForm : Form
    {

        // An instance of our game engine
        CBounceGame _game;

        // Keep track of whether the form has been closed. This indicates to the rest
        // of the form code that it should terminate whatever it is doing.
        // Note: in .NET CF 3.5 we can use the form's IsDisposed instead of having
        // to set a flag, but this is not available in .NET CF 2.0.
        bool _formClosed = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameForm()
        {
            InitializeComponent();

            // Instantiate our game and set its form to be this form
            _game = new CBounceGame(this);
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

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't allow the background to paint.
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Present the background to the form
            _game.Present(e.Graphics);
        }

        private void fpsTimer_Tick(object sender, EventArgs e)
        {
            // Assuming the game is initialized and providing a valid FPS reading...
            if (_game != null && _game.FramesPerSecond > 0)
            {
                // ...read the FPS and update the FPS label to display the value
                lblFPS.Text = "Frames per second: " + _game.FramesPerSecond.ToString();
                lblFPS.Refresh();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Enable both of the timers used within the form
            fpsTimer.Enabled = true;

            // Show the game form and ensure that it repaints itself
            this.Show();
            this.Invalidate();

            // Begin the game's render loop
            RenderLoop();
        }

        private void mnuReset_Click(object sender, EventArgs e)
        {
            // When the Reset menu button is selected, reset the game
            _game.Reset();
        }

        private void GameForm_Closed(object sender, EventArgs e)
        {
            // Flag the fact that the game form has closed. This indicates to the rest
            // of the form code that it should terminate whatever it is doing.
            _formClosed = true;
        }

    }
}