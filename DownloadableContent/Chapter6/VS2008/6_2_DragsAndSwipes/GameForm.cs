/**
 * 
 * DragsAndSwipes
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Demonstrates dragging and throwing game objects.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DragsAndSwipes
{
    public partial class GameForm : Form
    {

        // The form's instance of our game
        CDraggableObjectGame _game;
        // Keep track of whether the form has been closed. This indicates to the rest
        // of the form code that it should terminate whatever it is doing.
        // Note: in .NET CF 3.5 we can use the form's IsDisposed instead of having
        // to set a flag, but this is not available in .NET CF 2.0.
        bool _formClosed = false;

        // The object currently being dragged (or null if no dragging is active)
        CObjDraggableBase _dragObject;
        // The most recent touch coordinate for dragging
        Point _dragPoint;

        /// <summary>
        /// Constructor
        /// </summary>
        public GameForm()
        {
            InitializeComponent();

            // Instantiate and initialize the game
            _game = new CDraggableObjectGame(this);
            _game.Reset();
        }


        /// <summary>
        /// Select the object at the specified position
        /// </summary>
        /// <param name="testPoint"></param>
        private CObjDraggableBase SelectObject(Point testPoint)
        {
            GameEngineCh6.CGameObjectGDIBase touchedObject;

            // Find the object at the position (or null if nothing is there)
            touchedObject = _game.GetObjectAtPoint(testPoint);

            // Select or deselect each objects as needed
            foreach (GameEngineCh6.CGameObjectGDIBase obj in _game.GameObjects)
            {
                // Is this a selectable object?
                if (obj is CObjDraggableBase)
                {
                    if (((CObjDraggableBase)obj).Selected && obj != touchedObject)
                    {
                        // This object was selected but is no longer, so redraw it
                        ((CObjDraggableBase)obj).Selected = false;
                        obj.HasMoved = true;
                    }
                    if (!((CObjDraggableBase)obj).Selected && obj == touchedObject)
                    {
                        // This object was not selected but now is, so redraw it
                        ((CObjDraggableBase)obj).Selected = true;
                        obj.HasMoved = true;
                    }
                }
            }

            // Return whatever object we found, or null if no object was selected
            if (touchedObject is CObjDraggableBase)
            {
                // A selectable object was touched, so return a reference to it
                return (CObjDraggableBase)touchedObject;
            }
            else
            {
                // Either no object selected or not a selectable object, so return null
                return null;
            }
        }

        /// <summary>
        /// Begin dragging an object
        /// </summary>
        /// <param name="touchPoint">The point containing an object to drag. If an object is found
        /// here then dragging will begin, otherwise the call will be ignored.</param>
        private void DragBegin(Point touchPoint)
        {
            // Select the object at the specified point and store a reference6
            // to it in _dragObject (or null if nothing is at this location)
            _dragObject = SelectObject(touchPoint);
            // Store the point that was touched
            _dragPoint = touchPoint;

            // If an object is selected, move it to the front
            if (_dragObject != null)
            {
                _dragObject.MoveToFront();
                _dragObject.InitializeKineticDragging();
                if (mnuMain_Menu_LowFriction.Checked) _dragObject.KineticFriction = 2;
                if (mnuMain_Menu_NormalFriction.Checked) _dragObject.KineticFriction = 20;
                if (mnuMain_Menu_HighFriction.Checked) _dragObject.KineticFriction = 50;
            }
        }

        /// <summary>
        /// Move an object being dragged
        /// </summary>
        /// <param name="touchPoint"></param>
        private void DragMove(Point touchPoint)
        {
            // Are we currently dragging an object? If not, there is nothing more to do
            if (_dragObject == null) return;

            // Update the object position based on how far the touch point has moved
            _dragObject.XPos += (touchPoint.X - _dragPoint.X);
            _dragObject.YPos += (touchPoint.Y - _dragPoint.Y);

            // Update the stored drag point for the next movement
            _dragPoint = touchPoint;
        }

        /// <summary>
        /// Finish dragging an object
        /// </summary>
        /// <param name="releasePoint"></param>
        private void DragEnd(Point releasePoint)
        {
            // If we are dragging an object and kinetic movement is activated...
            if (_dragObject != null && mnuMain_Menu_Kinetic.Checked)
            {
                // ...then apply the kinetic movement to the object we have dragged
                _dragObject.ApplyKineticDragging();
            }

            // Release the dragged object reference
            _dragObject = null;
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
            DragBegin(new Point(e.X, e.Y));
        }

        private void GameForm_MouseMove(object sender, MouseEventArgs e)
        {
            DragMove(new Point(e.X, e.Y));
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e)
        {
            DragEnd(new Point(e.X, e.Y));
        }

        private void mnuMain_Menu_Kinetic_Click(object sender, EventArgs e)
        {
            // Toggle the checked status
            mnuMain_Menu_Kinetic.Checked = !mnuMain_Menu_Kinetic.Checked;

            // Update the other menu items
            mnuMain_Menu_LowFriction.Enabled = mnuMain_Menu_Kinetic.Checked;
            mnuMain_Menu_NormalFriction.Enabled = mnuMain_Menu_Kinetic.Checked;
            mnuMain_Menu_HighFriction.Enabled = mnuMain_Menu_Kinetic.Checked;
        }

        private void mnuMain_Menu_LowFriction_Click(object sender, EventArgs e)
        {
            // Activate low friction
            mnuMain_Menu_LowFriction.Checked = true;
            mnuMain_Menu_NormalFriction.Checked = false;
            mnuMain_Menu_HighFriction.Checked = false;
        }

        private void mnuMain_Menu_NormalFriction_Click(object sender, EventArgs e)
        {
            // Activate normal friction
            mnuMain_Menu_LowFriction.Checked = false;
            mnuMain_Menu_NormalFriction.Checked = true;
            mnuMain_Menu_HighFriction.Checked = false;
        }

        private void mnuMain_Menu_HighFriction_Click(object sender, EventArgs e)
        {
            // Activate high friction
            mnuMain_Menu_LowFriction.Checked = false;
            mnuMain_Menu_NormalFriction.Checked = false;
            mnuMain_Menu_HighFriction.Checked = true;
        }

        private void mnuMain_Menu_Popup(object sender, EventArgs e)
        {
            // Make sure we repaint after showing the menu so that the
            // screen is refreshed.
            _game.ForceRepaint();
        }
        
    }
}