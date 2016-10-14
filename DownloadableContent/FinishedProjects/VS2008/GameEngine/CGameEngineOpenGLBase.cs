using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace GameEngine
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
        // Track whether the depth buffer is enabled
        private bool _depthBufferEnabled = false;

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
            InitializeOpenGL(false);
        }
        /// <summary>
        /// Create and initialize the OpenGL environment.
        /// </summary>
        /// <param name="enableDepthBuffer">If true, the depth buffer will be enabled; otherwise it will be disabled.</param>
        /// <remarks>If the Capabilities check is being used, it should be
        /// performed prior to calling this method.</remarks>
        public void InitializeOpenGL(bool enableDepthBuffer)
        {
            // Initialize OpenGL
            CreateGL();
            InitGL();

            // Enable the depth buffer for 3d rendering?
            if (enableDepthBuffer) InitGLDepthBuffer();

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
        /// Initialise the depth buffer
        /// </summary>
        private void InitGLDepthBuffer()
        {
            // Set the clear depth to be 1 (the back edge of the buffer)
            gl.ClearDepthf(1.0f);
            // Set the depth function to render values less than or equal to the current depth
            gl.DepthFunc(gl.GL_LEQUAL);
            // Enable the depth test
            gl.Enable(gl.GL_DEPTH_TEST);

            // Remember that the depth buffer is enabled
            _depthBufferEnabled = true;
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

            // Clear the rendering buffers
            if (_depthBufferEnabled)
            {
                // Clear the color and the depth buffer
                gl.Clear(gl.GL_COLOR_BUFFER_BIT | gl.GL_DEPTH_BUFFER_BIT);
            }
            else
            {
                // Clear just the color buffer
                gl.Clear(gl.GL_COLOR_BUFFER_BIT);
            }

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
            CGameObjectOpenGLCameraBase camera;

            // If the game form doesn't have focus, sleep for a moment and return without any further processing
            if (!GameForm.Focused)
            {
                System.Threading.Thread.Sleep(100);
                return;
            }

            // Is the first object in the game objects list a camera?
            if (!(GameObjects[0] is CGameObjectOpenGLCameraBase))
            {
                // If we have a camera object, move it to the head of the object list.
                // This will ensure that the camera moves before any objects are rendered.
                camera = FindCameraObject();
                // Did we find a camera?
                if (camera != null)
                {
                    // Remove the camera from the object list...
                    GameObjects.Remove(camera);
                    // ...and re-add it at the beginning of the list
                    GameObjects.Insert(0, camera);
                }
            }

            // Call base.Advance. This will update all of the objects within our game.
            base.Advance();

            // Swap the buffers so that our updated frame is displayed
            egl.SwapBuffers(_eglDisplay, _eglSurface);
        }

        /// <summary>
        /// Find a camera object (if there is one) within the GameObjects list.
        /// </summary>
        /// <returns>Returns the first camera object identified, or null if
        /// no cameras are present within the list.</returns>
        public CGameObjectOpenGLCameraBase FindCameraObject()
        {
            foreach (CGameObjectBase obj in GameObjects)
            {
                if (obj is CGameObjectOpenGLCameraBase)
                {
                    return (CGameObjectOpenGLCameraBase)obj;
                }
            }

            // No camera was found
            return null;
        }

        /// <summary>
        /// Resets the projection matrix based upon the position of the
        /// camera object within the object list. If no camera is present,
        /// loads the identity matrix instead.
        /// </summary>
        /// <param name="interpFactor">The current render interpolation factor</param>
        public void LoadCameraMatrix(float interpFactor)
        {
            float eyex, eyey, eyez;
            float centerx, centery, centerz;
            float upx, upy, upz;
            CGameObjectOpenGLCameraBase camera;

            // Load the identity matrix.
            gl.LoadIdentity();

            // See if we have a camera object
            camera = FindCameraObject();
            if (camera != null)
            {
                // Do we already have a cached camera matrix?
                if (camera.CachedCameraMatrix == null)
                {
                    // No, so calculate the camera position.
                    // Get the camera's eye position
                    eyex = camera.GetDisplayXPos(interpFactor);
                    eyey = camera.GetDisplayYPos(interpFactor);
                    eyez = camera.GetDisplayZPos(interpFactor);
                    // Get the camera's center position
                    centerx = camera.GetDisplayXCenter(interpFactor);
                    centery = camera.GetDisplayYCenter(interpFactor);
                    centerz = camera.GetDisplayZCenter(interpFactor);
                    // Get the camera's up vector
                    upx = camera.GetDisplayXUp(interpFactor);
                    upy = camera.GetDisplayYUp(interpFactor);
                    upz = camera.GetDisplayZUp(interpFactor);

                    // Calculate the transformation matrix for the camera
                    glu.LookAt(eyex, eyey, eyez, centerx, centery, centerz, upx, upy, upz);

                // Now we will store the calculated matrix into the camera object.
                camera.CachedCameraMatrix = GetModelviewMatrix();
                }
                else
                {
                    // The camera has not moved since its matrix was last calculated
                    // so we can simply restore the cached matrix.
                    SetModelviewMatrix(camera.CachedCameraMatrix);
                }
            }
        }

        /// <summary>
        /// Rotate the current modelview matrix so that it is facing towards
        /// the camera.
        /// </summary>
        public void RotateMatrixToCamera()
        {
            RotateMatrixToCamera(false);
        }
        /// <summary>
        /// Rotate the current modelview matrix so that it is facing towards
        /// the camera.
        /// </summary>
        /// <param name="keepUpVector">If true, the object's Up vector will be left unchanged</param>
        public void RotateMatrixToCamera(bool keepUpVector)
        {
            // Retrieve the current modelview matrix
            float[,] matrix = GetModelviewMatrix();

            // Reset the first 3x3 elements to the identity matrix
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Are we skipping the up vector (the values where i == 1)
                    if (keepUpVector == false || i != 1)
                    {
                        // Set the element to 0 or 1 as required
                        if (i == j)
                        {
                            matrix[i, j] = 1;
                        }
                        else
                        {
                            matrix[i, j] = 0;
                        }
                    }
                }
            }
            
            // Set the updated modelview matrix
            SetModelviewMatrix(matrix);
        }


        /// <summary>
        /// Retrieve the current modelview matrix as a 4x4 array of floats
        /// </summary>
        /// <returns></returns>
        unsafe public float[,] GetModelviewMatrix()
        {
            float[,] ret = new float[4, 4];

            // Fix a pointer to the array
            fixed (float* matrixPointer = &ret[0,0])
            {
                // Retrieve the model view matrix into the array
                gl.GetFloatv(gl.GL_MODELVIEW_MATRIX, matrixPointer);
            }

            return ret;
        }

        /// <summary>
        /// Set the current modelview matrix from a 4x4 array of floats
        /// </summary>
        /// <param name="matrix"></param>
        unsafe public void SetModelviewMatrix(float[,] matrix)
        {
            // Fix a pointer to the array
            fixed (float* matrixPointer = &matrix[0, 0])
            {
                // Load the array data into OpenGL's modelview matrix
                gl.LoadMatrixf(matrixPointer);
            }
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

        
        /// <summary>
        /// Set the scene ambient light to the provided four-element color array
        /// </summary>
        unsafe public void SetAmbientLight(float[] color)
        {
            fixed (float* colorPointer = &color[0])
            {
                gl.LightModelfv(gl.GL_LIGHT_MODEL_AMBIENT, colorPointer);
            }
        }

        /// <summary>
        /// Set a light parameter to the provided value
        /// </summary>
        public void SetLightParameter(uint light, uint parameter, float value)
        {
            gl.Lightf(light, parameter, value);
        }

        /// <summary>
        /// Set a light parameter to the provided value array
        /// </summary>
        unsafe public void SetLightParameter(uint light, uint parameter, float[] value)
        {
            fixed (float* valuePointer = &value[0])
            {
                gl.Lightfv(light, parameter, valuePointer);
            }
        }

        /// <summary>
        /// Set a material parameter to the provided value
        /// </summary>
        public void SetMaterialParameter(uint face, uint parameter, float value)
        {
            gl.Materialf(face, parameter, value);
        }

        /// <summary>
        /// Set a material parameter to the provided value array
        /// </summary>
        unsafe public void SetMaterialParameter(uint face, uint parameter, float[] value)
        {
            fixed (float* valuePointer = &value[0])
            {
                gl.Materialfv(face, parameter, valuePointer);
            }
        }

        /// <summary>
        /// Set a fog parameter to the provided value
        /// </summary>
        public void SetFogParameter(uint parameter, float value)
        {
            gl.Fogf(parameter, value);
        }

        /// <summary>
        /// Set a fog parameter to the provided value
        /// </summary>
        public void SetFogParameter(uint parameter, int value)
        {
            gl.Fogx(parameter, value);
        }

        /// <summary>
        /// Set a fog parameter to the provided value array
        /// </summary>
        unsafe public void SetFogParameter(uint parameter, float[] value)
        {
            fixed (float* valuePointer = &value[0])
            {
                gl.Fogfv(parameter, valuePointer);
            }
        }


    }
}
