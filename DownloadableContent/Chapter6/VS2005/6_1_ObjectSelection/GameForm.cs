/**
 * 
 * ObjectSelection
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates selecting objects within the engine by tapping them on the screen.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ObjectSelection
{
    public partial class GameForm : Form
    {

        // The form's instance of our game
        CObjectSelectionGame _game;
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
            _game = new CObjectSelectionGame(this);
            _game.Reset();
        }

        /// <summary>
        /// Select the object at the specified position
        /// </summary>
        /// <param name="testPoint"></param>
        private CObjSelectableBase SelectObject(Point testPoint)
        {
            GameEngineCh6.CGameObjectGDIBase touchedObject;

            // Find the object at the position (or null if nothing is there)
            touchedObject = _game.GetObjectAtPoint(testPoint);

            // Select or deselect each objects as needed
            foreach (GameEngineCh6.CGameObjectGDIBase obj in _game.GameObjects)
            {
                // Is this a selectable object?
                if (obj is CObjSelectableBase)
                {
                    if (((CObjSelectableBase)obj).Selected && obj != touchedObject)
                    {
                        // This object was selected but is no longer, so redraw it
                        ((CObjSelectableBase)obj).Selected = false;
                        obj.HasMoved = true;
                    }
                    if (!((CObjSelectableBase)obj).Selected && obj == touchedObject)
                    {
                        // This object was not selected but now is, so redraw it
                        ((CObjSelectableBase)obj).Selected = true;
                        obj.HasMoved = true;
                    }
                }
            }

            // Return whatever object we found, or null if no object was selected
            if (touchedObject is CObjSelectableBase)
            {
                // A selectable object was touched, so return a reference to it
                return (CObjSelectableBase)touchedObject;
            }
            else
            {
                // Either no object selected or not a selectable object, so return null
                return null;
            }
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
        private void mnuReset_Click(object sender, EventArgs e)
        {
            _game.Reset();
        }

        private void GameForm_Closed(object sender, EventArgs e)
        {
            // Flag the fact that the game form has closed. This indicates to the rest
            // of the form code that it should terminate whatever it is doing.
            _formClosed = true;
        }


        private void GameForm_MouseDown(object sender, MouseEventArgs e)
        {
            SelectObject(new Point(e.X, e.Y));
        }
        
    }
}