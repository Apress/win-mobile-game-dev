/**
 * 
 * AlphaBlending
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
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using OpenGLES;

namespace AlphaBlending
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
        private float _alphaFader = 0;
        // The texture to use for rendering
        private Texture _texture;

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
            if (_texture != null) _texture.Dispose();
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
            gl.ClearColor(0.3f, 0.3f, 0.3f, 0.0f);

            // Enable smooth shading so that colors are interpolated across the
            // surface of our rendered shapes.
            gl.ShadeModel(gl.GL_SMOOTH);

            // Disable depth testing
            gl.Disable(gl.GL_DEPTH_TEST);

            // Initialize the OpenGL viewport
            InitGLViewport();

            // Enable alpha blending
            gl.Enable(gl.GL_BLEND);
            // Set the blending function
            gl.BlendFunc(gl.GL_SRC_ALPHA, gl.GL_ONE);

            // Load our textures
            Assembly asm = Assembly.GetExecutingAssembly();
            _texture = Texture.LoadStream(asm.GetManifestResourceStream("AlphaBlending.Graphics.ColorFade.png"), true);

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

            float redAlpha = (float)Math.Sin(_alphaFader / 20) / 2 + 0.5f;
            float greenAlpha = (float)Math.Sin(_alphaFader / 24) / 2 + 0.5f;
            float blueAlpha = (float)Math.Sin(_alphaFader / 28) / 2 + 0.5f;
            _alphaFader += 1;

            // Generate an array of colors for each of our quads
            float[] quadRed = new float[]      {   1.0f, 0.0f, 0.0f, redAlpha,
                                                   1.0f, 0.0f, 0.0f, redAlpha,
                                                   1.0f, 0.0f, 0.0f, redAlpha,
                                                   1.0f, 0.0f, 0.0f, redAlpha};
            float[] quadGreen = new float[]    {   0.0f, 1.0f, 0.0f, greenAlpha,
                                                   0.0f, 1.0f, 0.0f, greenAlpha,
                                                   0.0f, 1.0f, 0.0f, greenAlpha,
                                                   0.0f, 1.0f, 0.0f, greenAlpha};
            float[] quadBlue = new float[]     {   0.0f, 0.0f, 1.0f, blueAlpha,
                                                   0.0f, 0.0f, 1.0f, blueAlpha, 
                                                   0.0f, 0.0f, 1.0f, blueAlpha,
                                                   0.0f, 0.0f, 1.0f, blueAlpha};
            // Also create three fading white color arrays using for convenience
            // the red, green and blue alpha values we have already calculated
            float[] quadWhite1 = new float[]   {   1.0f, 1.0f, 1.0f, redAlpha,
                                                   1.0f, 1.0f, 1.0f, redAlpha, 
                                                   1.0f, 1.0f, 1.0f, redAlpha,
                                                   1.0f, 1.0f, 1.0f, redAlpha};
            float[] quadWhite2 = new float[]   {   1.0f, 1.0f, 1.0f, greenAlpha,
                                                   1.0f, 1.0f, 1.0f, greenAlpha, 
                                                   1.0f, 1.0f, 1.0f, greenAlpha,
                                                   1.0f, 1.0f, 1.0f, greenAlpha};
            float[] quadWhite3 = new float[]   {   1.0f, 1.0f, 1.0f, blueAlpha,
                                                   1.0f, 1.0f, 1.0f, blueAlpha, 
                                                   1.0f, 1.0f, 1.0f, blueAlpha,
                                                   1.0f, 1.0f, 1.0f, blueAlpha};


            // Load the identity matrix
            gl.LoadIdentity();
            // Translate a little up the screen
            gl.Translatef(0, 0.4f, 0);
            // Render the red quad
            if (!mnuMain_Menu_Textures.Checked)
            {
                RenderColorAlphaQuad(quadRed);
            }
            else
            {
                RenderColorAlphaTextureQuad(quadWhite1);
            }

            // Load the identity matrix
            gl.LoadIdentity();
            // Rotate a third of the way around a circle
            gl.Rotatef(120, 0, 0, 1);
            // Translate a little up the screen
            gl.Translatef(0, 0.4f, 0);
            // Render the green quad
            if (!mnuMain_Menu_Textures.Checked)
            {
                RenderColorAlphaQuad(quadGreen);
            }
            else
            {
                RenderColorAlphaTextureQuad(quadWhite2);
            }

            // Load the identity matrix
            gl.LoadIdentity();
            // Rotate two thirds of the way around a circle
            gl.Rotatef(240, 0, 0, 1);
            // Translate a little up the screen
            gl.Translatef(0, 0.4f, 0);
            // Render the blue quad
            if (!mnuMain_Menu_Textures.Checked)
            {
                RenderColorAlphaQuad(quadBlue);
            }
            else
            {
                RenderColorAlphaTextureQuad(quadWhite3);
            }

        }

        /// <summary>
        /// Render a quad at the current location using the provided colors
        /// for the bottom-left, bottom-right, top-left and top-right
        /// corners respectively.
        /// </summary>
        /// <param name="quadColors">An array of four sets of Red, Green, Blue and Alpha floats.</param>
        unsafe private void RenderColorAlphaQuad(float[] quadColors)
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
                gl.ColorPointer(4, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

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
        /// Render a quad at the current location using with the current
        /// bound texture mapped across it.
        /// </summary>
        private void RenderTextureQuad()
        {
            // Build the default set of texture coordinates for the quad
            float[] texCoords = new float[]  {   0.0f, 1.0f,
                                             1.0f, 1.0f,
                                             0.0f, 0.0f,
                                             1.0f, 0.0f };
            // Render the quad
            RenderTextureQuad(texCoords);
        }
        /// <summary>
        /// Render a quad at the current location using the provided texture
        /// coordinates for the bottom-left, bottom-right, top-left and top-right
        /// corners respectively.
        /// </summary>
        /// <param name="texCoords">An array of texture coordinates</param>
        unsafe private void RenderTextureQuad(float[] texCoords)
        {
            // The vertex positions for a flat unit-size square
            float[] quadVertices = new float[]  {   -0.5f, -0.5f, 0.0f,
                                                 0.5f, -0.5f, 0.0f,
                                                -0.5f,  0.5f, 0.0f,
                                                 0.5f,  0.5f, 0.0f};

            // Fix a pointer to the quad vertices and the texture coordinates
            fixed (float* quadPointer = &quadVertices[0], texPointer = &texCoords[0])
            {
                // Enable textures
                gl.Enable(gl.GL_TEXTURE_2D);

                // Enable processing of the vertex and texture arrays
                gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                gl.EnableClientState(gl.GL_TEXTURE_COORD_ARRAY);

                // Provide a reference to the vertex and texture arrays
                gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)quadPointer);
                gl.TexCoordPointer(2, gl.GL_FLOAT, 0, (IntPtr)texPointer);

                // Draw the quad. We draw a strip of triangles, considering
                // four vertices within the vertex array.
                gl.DrawArrays(gl.GL_TRIANGLE_STRIP, 0, 4);

                // Disable processing of the vertex and texture arrays now that we
                // have used them.
                gl.DisableClientState(gl.GL_VERTEX_ARRAY);
                gl.DisableClientState(gl.GL_TEXTURE_COORD_ARRAY);

                // Disable textures
                gl.Disable(gl.GL_TEXTURE_2D);
            }
        }

        private void RenderColorAlphaTextureQuad(float[] quadColors)
        {
            // Build the default set of texture coordinates for the quad
            float[] texCoords = new float[]  {   0.0f, 1.0f,
                                             1.0f, 1.0f,
                                             0.0f, 0.0f,
                                             1.0f, 0.0f };
            // Render the quad
            RenderColorAlphaTextureQuad(texCoords, quadColors);
        }
        unsafe private void RenderColorAlphaTextureQuad(float[] texCoords, float[] quadColors)
        {
            // Fix a pointer to the quad vertices and the quad colors
            fixed (float* colorPointer = &quadColors[0])
            {
                // Enable processing of the color array
                gl.EnableClientState(gl.GL_COLOR_ARRAY);

                // Provide a reference to the color array
                gl.ColorPointer(4, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

                // Draw the quad using the existing RenderTextureQuad function
                RenderTextureQuad(texCoords);

                // Disable processing of the color array now that we have used it.
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
        private void mnuMain_Exit_Click(object sender, EventArgs e)
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

        private void mnuMain_Menu_Textures_Click(object sender, EventArgs e)
        {
            mnuMain_Menu_Textures.Checked = !mnuMain_Menu_Textures.Checked;
        }

    }
}