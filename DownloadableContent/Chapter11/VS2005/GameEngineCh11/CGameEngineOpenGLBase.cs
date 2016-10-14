using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace GameEngineCh11
{
    public class CGameEngineOpenGLBase : CGameEngineBase, IDisposable
    {

        // Variables required by the Embedded Graphics Library (EGL)
        private EGLDisplay _eglDisplay;
        private EGLSurface _eglSurface;
        private EGLContext _eglContext;

        // A flag to keep track of whether OpenGL has been successfully initialized
        private bool _glInitialized;

        // The background color
        private Color _backgroundColor = Color.Black;

        // A dictionary of graphics for use by the game
        private Dictionary<String, Texture> _gameGraphics = new Dictionary<string, Texture>();


        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor -- require an instance of the form that we will be running against to be provided
        /// </summary>
        /// <param name="f"></param>
        public CGameEngineOpenGLBase(Form gameForm)
            : base(gameForm)   
        {
            // Add a Resize handler for the game form.
            // This will allow us to easily re-initialize the viewport when the form resizes.
            gameForm.Resize += new System.EventHandler(GameFormResize);
        }


        //-------------------------------------------------------------------------------------
        // OpenGL functions

        /// <summary>
        /// Create and initialize the OpenGL environment.
        /// </summary>
        /// <remarks>If the Capabilities check is being used, it should be
        /// performed prior to calling this method.</remarks>
        public void InitializeOpenGL()
        {
            // Initialize OpenGL
            CreateGL();
            InitGL();

            // Flag that OpenGL has been successfully initialized
            _glInitialized = true;

            // Prepare and reset the game
            Prepare();
            Reset();
        }


        /// <summary>
        /// Create the OpenGL environment ready for use.
        /// </summary>
        private void CreateGL()
        {
            // Try to create an EGL display for our game form
            try
            {
                _eglDisplay = egl.GetDisplay(new EGLNativeDisplayType(GameForm));
            }
            catch
            {
                throw new ApplicationException("Unable to initialise an OpenGL display: this device may not support OpenGL ES applications");
            }

            // Initialize EGL for the display that we have created
            int major, minor;
            egl.Initialize(_eglDisplay, out major, out minor);

            // Set the attributes that we wish to use for our OpenGL configuration
            int[] attribList = new int[] 
            { 
                egl.EGL_RED_SIZE, 4,
                egl.EGL_GREEN_SIZE, 5,
                egl.EGL_BLUE_SIZE, 4,
                egl.EGL_NONE
            };

            // Declare an array to hold configuration details for the matching attributes
            EGLConfig[] configs = new EGLConfig[1];
            int numConfig;
            // Ensure that we are able to find a configuration for the requested attributes
            if (!egl.ChooseConfig(_eglDisplay, attribList, configs, configs.Length, out numConfig) || numConfig < 1)
            {
                throw new InvalidOperationException("Unable to choose config.");
            }
            // Retrieve the first returned configuration
            EGLConfig config = configs[0];

            // Create a surface from the config.
            _eglSurface = egl.CreateWindowSurface(_eglDisplay, config, GameForm.Handle, null);
            // Create a context from the config
            _eglContext = egl.CreateContext(_eglDisplay, config, EGLContext.None, null);

            // Activate the display, surface and context that has been created so that
            // we can render to the window.
            egl.MakeCurrent(_eglDisplay, _eglSurface, _eglSurface, _eglContext);

            // At this stage the OpenGL environment itself has been created.
        }

  
        /// <summary>
        /// Initialize the OpenGL environment ready for us to start rendering
        /// </summary>
        private void InitGL()
        {
            // We can now configure OpenGL ready for rendering.

            // Set the background color for the window
            BackgroundColor = _backgroundColor;

            // Enable smooth shading so that colors are interpolated across the
            // surface of our rendered shapes.
            gl.ShadeModel(gl.GL_SMOOTH);

            // Disable depth testing
            gl.Disable(gl.GL_DEPTH_TEST);

            // Initialize the OpenGL viewport
            InitGLViewport();

            // All done
        }

        /// <summary>
        /// Set up OpenGL's viewport
        /// </summary>
        protected virtual void InitGLViewport()
        {
            // Set the viewport that we are rendering to
            gl.Viewport(GameForm.ClientRectangle.Left, GameForm.ClientRectangle.Top, GameForm.ClientRectangle.Width, GameForm.ClientRectangle.Height);

            // Switch OpenGL into Projection mode so that we can set the projection matrix.
            gl.MatrixMode(gl.GL_PROJECTION);
            // Load the identity matrix
            gl.LoadIdentity();
            // Apply a perspective projection
            glu.Perspective(45, (float)GameForm.ClientRectangle.Width / (float)GameForm.ClientRectangle.Height, .1f, 100);
            // Translate the viewpoint a little way back, out of the screen
            gl.Translatef(0, 0, -3);

            // Switch OpenGL back to ModelView mode so that we can transform objects rather than
            // the projection matrix.
            gl.MatrixMode(gl.GL_MODELVIEW);
            // Load the identity matrix.
            gl.LoadIdentity();
        }

        /// <summary>
        /// Destroy the OpenGL objects and release their resources
        /// </summary>
        public void Dispose()
        {
            // Make sure OpenGL has actually been initialized
            if (_glInitialized)
            {
                // Dispose of the textures
                foreach (Texture t in GameGraphics.Values)
                {
                    t.Dispose();
                }
                GameGraphics.Clear();

                // Dispose of OpenGL
                egl.DestroySurface(_eglDisplay, _eglSurface);
                egl.DestroyContext(_eglDisplay, _eglContext);
                egl.Terminate(_eglDisplay);
            }
        }

        /// <summary>
        /// "Unproject" the provided screen coordinates back into OpenGL's projection
        /// coordinates.
        /// </summary>
        /// <param name="x">The screen x coordinate to unproject</param>
        /// <param name="y">The screen y coordinate to unproject</param>
        /// <param name="z">The game world z coordinate to unproject</param>
        /// <param name="xPos">Returns the x position in game world coordinates</param>
        /// <param name="yPos">Returns the y position in game world coordinates</param>
        /// <param name="zPos">Returns the z position in game world coordinates</param>
        unsafe public bool UnProject(int x, int y, float z, out float objx, out float objy, out float objz)
        {
            int[] viewport = new int[4];
            float[] modelview = new float[16];
            float[] projection = new float[16];
            float winX, winY, winZ;

            // Load the identity matrix so that the coordinates are reset rather than calculated
            // against any existing transformation that has been left in place.
            gl.LoadIdentity();

            // Retrieve the modelview and projection matrices
            fixed (float* modelviewPointer = &modelview[0], projectionPointer = &projection[0])
            {
                gl.GetFloatv(gl.GL_MODELVIEW_MATRIX, modelviewPointer);
                gl.GetFloatv(gl.GL_PROJECTION_MATRIX, projectionPointer);
            }
            // Retrieve the viewport dimensions
            fixed (int* viewportPointer = &viewport[0])
            {
                gl.GetIntegerv(gl.GL_VIEWPORT, viewportPointer);
            }

            // Prepare the coordinates to be passed to glu.UnProject
            winX = (float)x;
            winY = (float)viewport[3] - (float)y;
            winZ = z;

            // Call UnProject with the values we have calculated.
            // The unprojected values will be returned in the
            // xPos, yPos and zPos variables, and in turn returned
            // back to the calling procedure.
            return glu.UnProject(winX, winY, winZ, modelview, projection, viewport, out objx, out objy, out objz);
        }


        //-------------------------------------------------------------------------------------
        // Property access

        public bool OpenGLInitialized
        {
            get { return _glInitialized; }
        }

        /// <summary>
        /// Set or return the current game background color.
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                // Store the background color
                _backgroundColor = value;
                // Set the background color for the window
                if (_glInitialized)
                {
                    float[] backgroundColor = ColorToFloat3(value);
                    gl.ClearColor(backgroundColor[0], backgroundColor[1], backgroundColor[2], 0);
                }
            }
        }

        /// <summary>
        ///  Return the game engine's dictionary of game graphics
        /// </summary>
        public Dictionary<String, Texture> GameGraphics
        {
            get { return _gameGraphics; }
        }


        //-------------------------------------------------------------------------------------
        // Game engine operations


        /// <summary>
        /// Render all required graphics in order to update the game display.
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        public override void Render(float interpFactor)
        {
            // Allow the base class to perform any processing it needs to do
            base.Render(interpFactor);

            // Make sure OpenGL has been initialized
            if (!_glInitialized) throw new Exception("Cannot Render: OpenGL has not been initialized");

            // Clear the background.
            gl.Clear(gl.GL_COLOR_BUFFER_BIT);

            // Render all game objects
            foreach (CGameObjectOpenGLBase gameObject in GameObjects)
            {
                // Is this object terminated? Don't draw if it is
                if (!gameObject.Terminate)
                {
                    gameObject.Render(null, interpFactor);
                }
            }
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

            // Call base.Advance. This will update all of the objects within our game.
            base.Advance();

            // Swap the buffers so that our updated frame is displayed
            egl.SwapBuffers(_eglDisplay, _eglSurface);
        }




        //-------------------------------------------------------------------------------------
        // UI events

        /// <summary>
        /// Respond to the game form resize event
        /// </summary>
        private void GameFormResize(object sender, EventArgs e)
        {
            if (_glInitialized)
            {
                // Re-prepare the game.
                Prepare();
                // Re-initialize the viewport
                InitGLViewport();
            }
        }


        //-------------------------------------------------------------------------------------
        // Utility functions

        /// <summary>
        /// Convert a Color to an array of 3 floats for OpenGL (no alpha component)
        /// </summary>
        public float[] ColorToFloat3(Color color)
        {
            float[] ret = new float[3];
            ret[0] = (float)color.R / 255.0f;
            ret[1] = (float)color.G / 255.0f;
            ret[2] = (float)color.B / 255.0f;
            return ret;
        }
        /// <summary>
        /// Convert a Color to an array of 4 floats for OpenGL (alpha component included)
        /// </summary>
        public float[] ColorToFloat4(Color color)
        {
            float[] ret = new float[4];
            ret[0] = (float)color.R / 255.0f;
            ret[1] = (float)color.G / 255.0f;
            ret[2] = (float)color.B / 255.0f;
            ret[3] = 1;
            return ret;
        }
        /// <summary>
        /// Convert an array of 3 or 4 floats into a Color
        /// </summary>
        public Color FloatToColor(float[] floats)
        {
            int r = (int)(floats[0] * 255);
            int g = (int)(floats[1] * 255);
            int b = (int)(floats[2] * 255);
            return Color.FromArgb(r, g, b);
        }

    }
}
