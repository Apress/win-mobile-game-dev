/**
 * 
 * NestedQuads
 * 
 * An OpenGL example project.
 * 
 * Copyright (c) 2009-2010, Adam Dawes.
 * 
 * Part of "Windows Mobile Game Development" by Apress.
 * 
 **/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace NestedQuads
{
    public partial class MainForm : Form
    {

        // Variables required by the Embedded Graphics Library (EGL)
        private EGLDisplay _eglDisplay;
        private EGLSurface _eglSurface;
        private EGLContext _eglContext;

        // A flag to keep track of whether OpenGL has been successfully initialized
        private bool _glInitialized;

        // The angle at which we will display our quad
        private float _rotation = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create the OpenGL environment
        /// </summary>
        /// <returns></returns>
        private void CreateGL()
        {
            // Try to create an EGL display for our game form
            try
            {
                _eglDisplay = egl.GetDisplay(new EGLNativeDisplayType(this));
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
            _eglSurface = egl.CreateWindowSurface(_eglDisplay, config, this.Handle, null);
            // Create a context from the config
            _eglContext = egl.CreateContext(_eglDisplay, config, EGLContext.None, null);
        
            // Activate the display, surface and context that has been created so that
            // we can render to the window.
            egl.MakeCurrent(_eglDisplay, _eglSurface, _eglSurface, _eglContext);

            // At this stage the OpenGL environment itself has been created.
        }

        /// <summary>
        /// Destroy the EGL objects and release their resources
        /// </summary>
        private void DestroyGL()
        {
            egl.DestroySurface(_eglDisplay, _eglSurface);
            egl.DestroyContext(_eglDisplay, _eglContext);
            egl.Terminate(_eglDisplay);
        }

        /// <summary>
        /// Initialize the OpenGL environment ready for us to start rendering
        /// </summary>
        private void InitGL()
        {
            // We can now configure OpenGL ready for rendering.

            // Set the background color for the window
            gl.ClearColor(0.0f, 0.0f, 0.25f, 0.0f);

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
        private void InitGLViewport()
        {
            // Set the viewport that we are rendering to
            gl.Viewport(this.ClientRectangle.Left, this.ClientRectangle.Top, this.ClientRectangle.Width, this.ClientRectangle.Height);

            // Switch OpenGL into Projection mode so that we can set the projection matrix.
            gl.MatrixMode(gl.GL_PROJECTION);
            // Load the identity matrix
            gl.LoadIdentity();
            // Apply a perspective projection
            glu.Perspective(45, (float)this.ClientRectangle.Width / (float)this.ClientRectangle.Height, .1f, 100);
            // Translate the viewpoint a little way back, out of the screen
            gl.Translatef(0, 0, -3);

            // Switch OpenGL back to ModelView mode so that we can transform objects rather than
            // the projection matrix.
            gl.MatrixMode(gl.GL_MODELVIEW);
            // Load the identity matrix.
            gl.LoadIdentity();
        }


        /// <summary>
        /// Render the OpenGL scene
        /// </summary>
        void Render()
        {
            // Clear the color and depth buffers
            gl.Clear(gl.GL_COLOR_BUFFER_BIT);

            // Generate an array of colors for our quad.
            // Each triplet contains the red, green and blue values
            // for one of the vertices of the quads, specified from
            // 0 (no intensity) to 1 (full intensity).
            float[] quadColors = new float[]    {   1.0f, 0.0f, 0.0f,       // red
                                                    0.0f, 1.0f, 0.0f,       // green
                                                    0.0f, 0.0f, 1.0f,       // blue
                                                    1.0f, 1.0f, 1.0f};      // white

            // Load the identity matrix
            gl.LoadIdentity();
            // Rotate by the angle required for this frame
            gl.Rotatef(_rotation, 0.0f, 0.0f, 1.0f);

            // Draw a series of quads, each at a slightly smaller size
            for (int i = 0; i < 20; i++)
            {
                // Scale the size down by 5%
                gl.Scalef(0.95f, 0.95f, 1);
                // Rotate by our rotation angle
                gl.Rotatef(_rotation, 0.0f, 0.0f, 1.0f);
                // Render the colored quad
                RenderColorQuad(quadColors);
            } 

            // Increase the rotation angle for the next render
            _rotation += 1f;
        }

        /// <summary>
        /// Render a quad at the current location using the provided colors
        /// for the bottom-left, bottom-right, top-left and top-right
        /// corners respectively.
        /// </summary>
        /// <param name="quadColors">An array of four sets of Red, Green and Blue floats.</param>
        unsafe private void RenderColorQuad(float[] quadColors)
        {
            // The vertex positions for a flat unit-size square
            float[] quadVertices = new float[]  {   -0.5f, -0.5f, 0.0f,
                                                     0.5f, -0.5f, 0.0f,
                                                    -0.5f,  0.5f, 0.0f,
                                                     0.5f,  0.5f, 0.0f};

            // Fix a pointer to the quad vertices and the quad colors
            fixed (float* quadPointer = &quadVertices[0], colorPointer = &quadColors[0])
            {
                // Enable processing of the vertex and color arrays
                gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                gl.EnableClientState(gl.GL_COLOR_ARRAY);

                // Provide a reference to the vertex array and color arrays
                gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)quadPointer);
                gl.ColorPointer(3, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

                // Draw the quad. We draw a strip of triangles, considering
                // four vertices within the vertex array.
                gl.DrawArrays(gl.GL_TRIANGLE_STRIP, 0, 4);

                // Disable processing of the vertex and color arrays now that we
                // have used them.
                gl.DisableClientState(gl.GL_VERTEX_ARRAY);
                gl.DisableClientState(gl.GL_COLOR_ARRAY);
            }
        }

   
        /// <summary>
        /// Prevent the background from painting
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Don't call into the base class
        }

        /// <summary>
        /// Process the form load
        /// </summary>
        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                // Create and initialize the OpenGL environment
                CreateGL();
                InitGL();
                // Indicate that initialization was successful
                _glInitialized = true;
            }
            catch (Exception ex)
            {
                // Something went wrong
                MessageBox.Show(ex.Message);
                // Close the application
                this.Close();
            }
        }

        /// <summary>
        /// Paint the form
        /// </summary>
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // Make sure OpenGL is initialized -- we cannot
            // render if it is not.
            if (_glInitialized)
            {
                // Render to the back buffer
                Render();
                // Swap the buffers so that our updated frame is displayed
                egl.SwapBuffers(_eglDisplay, _eglSurface);
            }

            // Invalidate the whole form to force another immediate repaint
            Invalidate();
        }

        /// <summary>
        /// The form is closing so release all resources that we have allocated
        /// </summary>
        private void MainForm_Closing(object sender, CancelEventArgs e)
        {

            if (_glInitialized)
            {
                DestroyGL();
            }
        }

        /// <summary>
        /// Allow the user to close the application
        /// </summary>
        private void myExitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// The form has been resized
        /// </summary>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            // If OpenGL is ready, re-initialize the viewport for the new window size
            if (_glInitialized)
            {
                InitGLViewport();
            }

        }

    }
}