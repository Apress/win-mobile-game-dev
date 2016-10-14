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
using System.Text;
using System.Windows.Forms;

namespace ButtonPresses
{
    public partial class GameForm : Form
    {

        // The form's instance of our game
        CButtonPressesGame _game;
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

            // Instantiate and initialize the game
            _game = new CButtonPressesGame(this);
            _game.Reset();
        }


        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't allow the background to paint.
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            // Get the game to present itself to the form
            _game.Present(e.Graphics);
        }

        /// <summary>
        /// The main render loop. This will loop constantly all the time the game
        /// is running, ensuring that everything is updated and displayed within
        /// the form.
        /// </summary>
        private void RenderLoop()
        {
            do
            {
                // If we lose focus, pause for a moment instead of advancing the game
                if (!this.Focused)
                {
                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                    // We are still focused so advance the game
                    _game.Advance();
                }

                // Process pending events. This allows all the other form events to be processed.
                Application.DoEvents();

                // If our window has been closed then return without doing anything more
                if (_formClosed) return;

                // Loop forever (at at least, until our game form is closed).
            } while (true);
        }

        /// <summary>
        /// The form has loaded, so perform final initialization and then start the render loop.
        /// </summary>
        private void GameForm_Load(object sender, EventArgs e)
        {
            // Ensure the form is displayed and repainted
            this.Show();
            this.Focus();
            this.Invalidate();

            // Enter the render loop to drive the game
            RenderLoop();
        }

        /// <summary>
        /// Reset the game
        /// </summary>
        private void mnuMain_Reset_Click(object sender, EventArgs e)
        {
            _game.Reset();
        }

        private void GameForm_Closed(object sender, EventArgs e)
        {
            // Flag the fact that the game form has closed. This indicates to the rest
            // of the form code that it should terminate whatever it is doing.
            _formClosed = true;
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            // Are we using the KeyDown input method?
            if (mnuMain_Menu_KeyDown.Checked)
            {
                // Move up?
                if (e.KeyCode == System.Windows.Forms.Keys.Up) _game.MoveBall(0, -5);
                // Move down?
                if (e.KeyCode == System.Windows.Forms.Keys.Down) _game.MoveBall(0, 5);
                // Move left?
                if (e.KeyCode == System.Windows.Forms.Keys.Left) _game.MoveBall(-5, 0);
                // Move right?
                if (e.KeyCode == System.Windows.Forms.Keys.Right) _game.MoveBall(5, 0);
            }
        }

        private void mnuMain_Menu_KeyDown_Click(object sender, EventArgs e)
        {
            mnuMain_Menu_KeyDown.Checked = true;
            mnuMain_Menu_KeyState.Checked = false;
        }

        private void mnuMain_Menu_KeyState_Click(object sender, EventArgs e)
        {
            mnuMain_Menu_KeyDown.Checked = false;
            mnuMain_Menu_KeyState.Checked = true;
        }

        private void mnuMain_Menu_Popup(object sender, EventArgs e)
        {
            // Repaint the form so that the menu imagery is not left behind
            _game.ForceRepaint();
        }

    }
}