/**
 * 
 * CGameEngineGDIBase
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GameEngineCh8
{
    public abstract class CGameEngineGDIBase : CGameEngineBase
    {
        // The back buffer into which we will render the game
        private Bitmap _backBuffer;

        // The rectangle which bounds the current set of rendered objects
        private Rectangle _renderRect;
        // The rectangle containing all updates to be used when presenting to the form
        private Rectangle _presentRect;
        // A flag indicating to Present that the full window should be presented
        private bool _forceRepaint;
        // A flag indicating to Render that the full window should be re-rendered
        private bool _forceRerender;

        // The background color (when no _backgroundImage is in use)
        private Color _backgroundColor = Color.White;
        // The background image to display behind the game objects
        private Bitmap _backgroundImage;

        // A dictionary of graphics for use by the game
        private Dictionary<String, Bitmap> _gameGraphics = new Dictionary<string, Bitmap>();

        // Declare an InputPanel variable so we can monitor the SIP display
        private Microsoft.WindowsCE.Forms.InputPanel _inputPanel;


        /// <summary>
        /// Class constructor -- require an instance of the form that we will be running against to be provided
        /// </summary>
        /// <param name="f"></param>
        public CGameEngineGDIBase(Form gameForm)
            : base(gameForm)   
        {
            // If we are running on a touch-screen device, instantiate the inputpanel
            if (!IsSmartphone)
            {
                _inputPanel = new Microsoft.WindowsCE.Forms.InputPanel();
                // Add the event handler
                _inputPanel.EnabledChanged += new System.EventHandler(SIPEnabledChanged);
            }

            // Add an Activated handler for the game form
            gameForm.Activated += new System.EventHandler(GameFormActivated);
            // Add a Resize handler for the game form
            gameForm.Resize += new System.EventHandler(GameFormResize);
        }

    
        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Set or return the current game background color.
        /// </summary>
        public Color BackgroundColor
        {
            get
            {
                return _backgroundColor;
            }
            set
            {
                _backgroundColor = value;
            }
        }

        /// <summary>
        ///  Return the game engine's dictionary of game graphics
        /// </summary>
        public Dictionary<String, Bitmap> GameGraphics
        {
            get
            {
                return _gameGraphics;
            }
        }


        //-------------------------------------------------------------------------------------
        // Game engine operations

        /// <summary>
        /// Allow the game to prepare itself for rendering.
        /// This includes loading graphics, setting coordinate systems, etc.
        /// </summary>
        public override void Prepare()
        {
            // Allow the base class to prepare itself.
            base.Prepare();

            // If we have a back buffer that doesn't match the current form size, destroy it
            if (_backBuffer != null)
            {
                // Has the size changed?
                if (_backBuffer.Width != GameForm.ClientSize.Width || _backBuffer.Height != GameForm.ClientSize.Height)
                {
                    // Yes, so destroy the back buffer. This will cause the next Render call to recreate it.
                    _backBuffer.Dispose();
                    _backBuffer = null;
                    // Destroy the background image if there is one, too
                    if (_backgroundImage != null)
                    {
                        // This will cause the background image to be repainted too
                        _backgroundImage.Dispose();
                        _backgroundImage = null;
                    }
                }
            }

            // Force a repaint of the form in case we have made any significant
            // changes to the game environment.
            ForceRepaint();
        }

        /// <summary>
        /// Reset the game to its initial state (start a new game).
        /// </summary>
        public override void Reset()
        {
            // Allow the base class to reset itself
            base.Reset();

            // Repaint the entire window. This will ensure that everything is re-rendered
            // so that no graphics from the pre-reset state remain on the screen.
            ForceRepaint();
        }

        /// <summary>
        /// Force a repaint of the entire screen.
        /// </summary>
        public void ForceRepaint()
        {
            // Invalidate the whole form so that it repaints in its entirety
            GameForm.Invalidate();
            // Set the _forceRepaint flag so that the next call to Present updates the whole form
            _forceRepaint = true;
            // Set the _forceRerender flag so that the next call to Render updates the whole form
            _forceRerender = true;
        }

        /// <summary>
        /// A virtual function which can be overridden in order to generate and return
        /// a background image to display behind the game objects, if one is required.
        /// </summary>
        /// <returns>Returns the generated bitmap, or null if no bitmap is required.</returns>
        protected virtual Bitmap CreateBackgroundImage()
        {
            // No background image bitmap by default
            return null;
        }

        /// <summary>
        /// Render all required graphics in order to update the game display.
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        public override void Render(float interpFactor)
        {
            Graphics backGfx = null;
            //Rectangle fullRenderRect;
            Region clipRegion = null;

            try
            {
                // Allow the base class to perform any processing it needs to do
                base.Render(interpFactor);

                // Are we forcing a repaint?
                if (_forceRerender)
                {
                    // Yes, so re-render the entire game area
                    _renderRect = GameForm.ClientRectangle;
                    _forceRerender = false;
                }
                else
                {
                    // Calculate the render rectangle required for the current object positions.
                    _renderRect = FindCurrentRenderRectangle(interpFactor);
                }

                // Ensure the back buffer is initialised
                InitBackBuffer();

                // Do we have a background image?
                if (_backgroundImage == null)
                {
                    // No, so see if we can initialize it
                    _backgroundImage = CreateBackgroundImage();
                    // Do we have one now?
                    if (_backgroundImage != null)
                    {
                        // Yes, so ensure we repaint the entire screen in order to display it
                        ForceRepaint();
                    }
                }

                // Obtain a reference to the graphics object for the back buffer.
                backGfx = Graphics.FromImage(_backBuffer);

                // Do we have anything to paint?
                if (!_renderRect.IsEmpty)
                {
                    // Set the clip region of the window so that any changes outside of this
                    // rectangle are ignored.
                    clipRegion = new Region(_renderRect);
                    backGfx.Clip = clipRegion;

                    // Clear the background.
                    // Do we have a background image?
                    if (_backgroundImage != null)
                    {
                        // Yes, so copy it to the foreground.
                        backGfx.DrawImage(_backgroundImage, _renderRect, _renderRect, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        // No, so just clear using the background color.
                        backGfx.Clear(BackgroundColor);
                    }

                    // Render all game objects which are included within the render rectangle
                    foreach (CGameObjectGDIBase gameObject in GameObjects)
                    {
                        // Is this object terminated? Don't draw it if not so that it disappears
                        if (!gameObject.Terminate)
                        {
                            // Does this object overlap the area we are updating?
                            if (gameObject.GetRenderRectangle(interpFactor).IntersectsWith(_renderRect))
                            {
                                gameObject.Render(backGfx, interpFactor);
                            }
                        }
                    }

                    // Debug: draw a rectangle around the clip area so we can see what we have redrawn
                    //using (Pen p = new Pen(Color.Blue))
                    //{
                    //    backGfx.DrawRectangle(p, _renderRect.X, _renderRect.Y, _renderRect.Width - 1, _renderRect.Height - 1);
                    //}

                    // Invalidate the render rectangle on the form so that it repaints
                    GameForm.Invalidate(_renderRect);

                    // Add the render rect to the rectangle to be presented
                    _presentRect = CGameFunctions.CombineRectangles(_presentRect, _renderRect);
                }

            }
            finally
            {
                // Release any resources we have allocated
                if (clipRegion != null) clipRegion.Dispose();
                if (backGfx != null) backGfx.Dispose();
            }

        }

        /// <summary>
        /// If not already ready, creates and initialises the back buffer that we use
        /// for off-screen rendering.
        /// </summary>
        private void InitBackBuffer()
        {
            // Make a new back buffer if needed.
            if (_backBuffer == null)
            {
                _backBuffer = new Bitmap(GameForm.ClientSize.Width, GameForm.ClientSize.Height);
                // Ensure we repaint the whole form
                ForceRepaint();
            }
        }

        /// <summary>
        /// Calculate the bounds of the rectangle inside which all of the moving objects reside.
        /// </summary>
        /// <returns></returns>
        private Rectangle FindCurrentRenderRectangle(float interpFactor)
        {
            Rectangle renderRect = new Rectangle();
            Rectangle objectRenderRect;

            // Loop through all items, combining the positions of those that have moved or been created into the render rectangle
            foreach (CGameObjectGDIBase gameObj in GameObjects)
            {
                // Has this object been moved (or created) since the last update?
                gameObj.CheckIfMoved();
                if (gameObj.HasMoved)
                {
                    // The object has moved so we need to add the its rectangle to the render rectangle.
                    // Retrieve its current rectangle
                    objectRenderRect = gameObj.GetRenderRectangle(interpFactor);
                    // Add to the overall rectangle
                    renderRect = CGameFunctions.CombineRectangles(renderRect, objectRenderRect);
                    // Include the object's previous rectangle too.
                    // (We can't rely on its LastX/Y position as it may have been updated multiple times
                    // since the last render)
                    renderRect = CGameFunctions.CombineRectangles(renderRect, gameObj.PreviousRenderRect);
                    // Store the current render rectangle into the object as its previous rectangle for the next call.
                    gameObj.PreviousRenderRect = objectRenderRect;

                    // Clear the HasMoved flag now that we have observed this object's movement
                    gameObj.HasMoved = false;
                }

                // This object has now been processed so it is no longer new
                gameObj.IsNew = false;
            }

            return renderRect;
        }


        /// <summary>
        /// Copy the updated region of the back buffer to the form.
        /// </summary>
        /// <param name="gfx">The form's Graphics object</param>
        public virtual void Present(Graphics gfx)
        {
            // Is this the first presentation since a call to ForceRepaint?
            if (_forceRepaint)
            {
                // Yes, so present the entire game area
                _presentRect = GameForm.ClientRectangle;
                // Clear the repaint flag
                _forceRepaint = false;
            }

            // Assuming we have something to copy, draw the back buffer to the form.
            if (!_presentRect.IsEmpty && _backBuffer != null)
            {
                gfx.DrawImage(_backBuffer, _presentRect, _presentRect, GraphicsUnit.Pixel);
            }

            // Clear the present rectangle ready for further renders
            _presentRect = new Rectangle();
        }


        /// <summary>
        /// Advance the simulation by one frame
        /// </summary>
        public override void Advance()
        {
            // If the game form doesn't have focus, sleep for a moment and return without any further processing
            if (!GameForm.Focused)
            {
                System.Threading.Thread.Sleep(100);
                return;
            }

            // Determine whether each of our objects has moved since the last call to Advance
            foreach (CGameObjectGDIBase gameObj in GameObjects)
            {
                // Determine whether the object has moved since we last advanced.
                // We need to do this as it may move during this advance but not
                // in a subsequent advance. If no Render is called between these
                // two, the most recent advance will indicate that the object did
                // not more, and so it wouldn't be re-rendered.
                gameObj.CheckIfMoved();
            }

            // Call base.Advance. This will update all of the objects within our game.
            base.Advance();
        }

        //-------------------------------------------------------------------------------------
        // Engine interrogation

        /// <summary>
        /// Find which object is at the specified location.
        /// </summary>
        /// <param name="testPoint">The point to check</param>
        /// <returns>If an object exists at the testPoint location then it will
        /// be returned; otherwise returns null.</returns>
        public CGameObjectGDIBase GetObjectAtPoint(Point testPoint)
        {
            float interpFactor;
            CGameObjectGDIBase gameObj;

            // Get the most recent render interpolation factor
            interpFactor = GetInterpFactor();

            // Scan all objects within the object collection.
            // Loop backwards so that the frontmost objects are processed first.
            for (int i = GameObjects.Count - 1; i >= 0; i--)
            {
                // Get a reference to the object at this position.
                gameObj = (CGameObjectGDIBase)GameObjects[i];

                // Ignore objects that have been flagged as terminated
                if (!gameObj.Terminate)
                {
                    // Ask the object whether it contains the specified point
                    if (gameObj.IsPointInObject(interpFactor, testPoint))
                    {
                        // The point is contained within this object
                        return gameObj;
                    }
                }
            }

            // The point was not contained within any of the game objects.
            return null;
        }

        /// <summary>
        /// Find all objects at the specified location.
        /// </summary>
        /// <param name="testPoint">The point to check</param>
        /// <returns>Returns a List of CGameObjectGDIBase objects. All game objects that exist
        /// at the testPoint location (if any) will be included within the List.</returns>
        public List<CGameObjectGDIBase> GetAllObjectsAtPoint(Point testPoint)
        {
            float interpFactor;
            CGameObjectGDIBase gameObj;
            List<CGameObjectGDIBase> objectsAtPoint = new List<CGameObjectGDIBase>();

            // Get the most recent render interpolation factor
            interpFactor = GetInterpFactor();

            // Scan all objects within the object collection.
            // Loop backwards so that the frontmost objects are processed first.
            for (int i = GameObjects.Count - 1; i >= 0; i--)
            {
                // Get a reference to the object at this position.
                gameObj = (CGameObjectGDIBase)GameObjects[i];

                // Ignore objects that have been flagged as terminated
                if (!gameObj.Terminate)
                {
                    // Ask the object whether it contains the specified point
                    if (gameObj.IsPointInObject(interpFactor, testPoint))
                    {
                        // The point is contained within this object
                        objectsAtPoint.Add(gameObj);
                    }
                }
            }

            // Return the list of objects
            return objectsAtPoint;
        }

        //-------------------------------------------------------------------------------------
        // UI events

        /// <summary>
        /// Respond to the game form Activate event
        /// </summary>
        private void GameFormActivated(object sender, EventArgs e)
        {
            // Force the whole form to repaint
            ForceRepaint();
        }

        /// <summary>
        /// Respond to the game form resize event
        /// </summary>
        private void GameFormResize(object sender, EventArgs e)
        {
            // Assuming we have a back buffer...
            if (_backBuffer != null)
            {
                // ...does its size now differ from that of the game form?
                if (GameForm.ClientSize.Width != _backBuffer.Width || GameForm.ClientSize.Height != _backBuffer.Height)
                {
                    // Yes, so we need to respond to the new form size.
                    // Re-prepare the game.
                    Prepare();
                    // Force the whole form to repaint
                    ForceRepaint();
                }
            }
        }

        /// <summary>
        /// Respond to the SIP opening or closing
        /// </summary>
        private void SIPEnabledChanged(object sender, EventArgs e)
        {
            // Has the input panel enabled state changed to false?
            if (_inputPanel != null && _inputPanel.Enabled == false)
            {
                // The SIP has closed so force a repaint of the whole window.
                // Otherwise the SIP imagery is left behind on the screen.
                ForceRepaint();
            }
        }

    }
}
