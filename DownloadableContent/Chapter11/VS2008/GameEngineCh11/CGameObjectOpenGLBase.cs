using System;
using System.Collections.Generic;
using System.Text;
using OpenGLES;

namespace GameEngineCh11
{
    public class CGameObjectOpenGLBase : CGameObjectBase
    {


        // Object position
        private float _xscale = 0;
        private float _yscale = 0;
        private float _zscale = 0;
        private float _lastxscale = float.MinValue;
        private float _lastyscale = float.MinValue;
        private float _lastzscale = float.MinValue;
        private float _xangle = 0;
        private float _yangle = 0;
        private float _zangle = 0;
        private float _lastxangle = float.MinValue;
        private float _lastyangle = float.MinValue;
        private float _lastzangle = float.MinValue;



        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CGameObjectOpenGLBase(CGameEngineBase gameEngine)
            : base(gameEngine)
        {
            // No constructor code required
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Sets or returns the current x Scale of the object
        /// </summary>
        public virtual float XScale
        {
            get { return _xscale; }
            set { _xscale = value; }
        }
        /// <summary>
        /// Sets or returns the current y Scale of the object
        /// </summary>
        public virtual float YScale
        {
            get { return _yscale; }
            set { _yscale = value; }
        }
        /// <summary>
        /// Sets or returns the current z Scale of the object
        /// </summary>
        public virtual float ZScale
        {
            get { return _zscale; }
            set { _zscale = value; }
        }
        /// <summary>
        /// Sets or returns the previous x Scale of the object
        /// </summary>
        protected float LastXScale
        {
            get { return _lastxscale; }
            set { _lastxscale = value; }
        }
        /// <summary>
        /// Sets or returns the previous y Scale of the object
        /// </summary>
        protected float LastYScale
        {
            get { return _lastyscale; }
            set { _lastyscale = value; }
        }
        /// <summary>
        /// Sets or returns the previous z Scale of the object
        /// </summary>
        protected float LastZScale
        {
            get { return _lastzscale; }
            set { _lastzscale = value; }
        }


        /// <summary>
        /// Sets or returns the current x Angle of the object
        /// </summary>
        public virtual float XAngle
        {
            get { return _xangle; }
            set { _xangle = value; }
        }
        /// <summary>
        /// Sets or returns the current y Angle of the object
        /// </summary>
        public virtual float YAngle
        {
            get { return _yangle; }
            set { _yangle = value; }
        }
        /// <summary>
        /// Sets or returns the current z Angle of the object
        /// </summary>
        public virtual float ZAngle
        {
            get { return _zangle; }
            set { _zangle = value; }
        }
        /// <summary>
        /// Sets or returns the previous x Angle of the object
        /// </summary>
        protected float LastXAngle
        {
            get { return _lastxangle; }
            set { _lastxangle = value; }
        }
        /// <summary>
        /// Sets or returns the previous y Angle of the object
        /// </summary>
        protected float LastYAngle
        {
            get { return _lastyangle; }
            set { _lastyangle = value; }
        }
        /// <summary>
        /// Sets or returns the previous z Angle of the object
        /// </summary>
        protected float LastZAngle
        {
            get { return _lastzangle; }
            set { _lastzangle = value; }
        }



        //-------------------------------------------------------------------------------------
        // OpenGL rendering functions

        /// <summary>
        /// Render a quad at the current location using the provided colors
        /// for the bottom-left, bottom-right, top-left and top-right
        /// corners respectively.
        /// </summary>
        /// <param name="quadColors">An array of four sets of red, green blue
        /// (and optionally alpha) floats.</param>
        unsafe protected void RenderColorQuad(float[] quadColors)
        {
            int elementsPerColor;

            // The vertex positions for a flat unit-size square
            float[] quadVertices = new float[]  {   -0.5f, -0.5f, 0.0f,
                                                     0.5f, -0.5f, 0.0f,
                                                    -0.5f,  0.5f, 0.0f,
                                                     0.5f,  0.5f, 0.0f};
            
            // Determine how many elements were provided for each color
            switch (quadColors.Length)
            {
                case 12: elementsPerColor = 3; break;   // no alpha
                case 16: elementsPerColor = 4; break;   // alpha present
                default: throw new Exception("Unknown content for quadColors -- expected 12 or 16 elements, found " + quadColors.Length.ToString());
            }

            // Fix a pointer to the quad vertices and the quad colors
            fixed (float* quadPointer = &quadVertices[0], colorPointer = &quadColors[0])
            {
                // Enable processing of the vertex and color arrays
                gl.EnableClientState(gl.GL_VERTEX_ARRAY);
                gl.EnableClientState(gl.GL_COLOR_ARRAY);

                // Provide a reference to the vertex array and color arrays
                gl.VertexPointer(3, gl.GL_FLOAT, 0, (IntPtr)quadPointer);
                gl.ColorPointer(elementsPerColor, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

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
        protected void RenderTextureQuad()
        {
            // Build the default set of texture coordinates for the quad
            float[] texCoords = new float[] { 0.0f, 1.0f,
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
        /// <param name="texCoords">An array of texture coordinates.</param>
        unsafe protected void RenderTextureQuad(float[] texCoords)
        {
            // The vertex positions for a flat unit-size square
            float[] quadVertices = new float[]  {  -0.5f, -0.5f, 0.0f,
                                                    0.5f, -0.5f, 0.0f,
                                                   -0.5f,  0.5f, 0.0f,
                                                    0.5f,  0.5f, 0.0f};

            // Check the texCoords array
            if (texCoords.Length != 8) throw new Exception("Unknown content for texCoords -- expected 8 elements, found " + texCoords.Length.ToString());

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

        /// <summary>
        /// Render a quad using the full texture and colors specified
        /// including an alpha component
        /// </summary>
        /// <param name="quadColors">An array of four four-element colors (16 elements in total)</param>
        protected void RenderColorTextureQuad(float[] quadColors)
        {
            // Build the default set of texture coordinates for the quad
            float[] texCoords = new float[] { 0.0f, 1.0f,
                                              1.0f, 1.0f,
                                              0.0f, 0.0f,
                                              1.0f, 0.0f };
            // Render the quad
            RenderColorTextureQuad(quadColors, texCoords);
        }
        /// <summary>
        /// Render a quad using the full texture and colors specified
        /// including an alpha component
        /// </summary>
        /// <param name="texCoords">An array of four two-element texture coordinates (8 elements in total)</param>
        /// <param name="quadColors">An array of four four-element colors (16 elements in total)</param>
        unsafe protected void RenderColorTextureQuad(float[] quadColors, float[] texCoords)
        {
            int elementsPerColor;

            // Determine how many elements were provided for each color
            switch (quadColors.Length)
            {
                case 12: elementsPerColor = 3; break;   // no alpha
                case 16: elementsPerColor = 4; break;   // alpha present
                default: throw new Exception("Unknown content for quadColors -- expected 12 or 16 elements, found " + quadColors.Length.ToString());
            }

            // Fix a pointer to the quad vertices and the quad colors
            fixed (float* colorPointer = &quadColors[0])
            {
                // Enable processing of the color array
                gl.EnableClientState(gl.GL_COLOR_ARRAY);

                // Provide a reference to the color array
                gl.ColorPointer(elementsPerColor, gl.GL_FLOAT, 0, (IntPtr)colorPointer);

                // Draw the quad using the existing RenderTextureQuad function
                RenderTextureQuad(texCoords);

                // Disable processing of the color array now that we have used it.
                gl.DisableClientState(gl.GL_COLOR_ARRAY);
            }
        }

        //-------------------------------------------------------------------------------------
        // Game engine operations

        /// <summary>
        /// In addition to the positions maintained by the base class, update
        /// the previous positions maintained by this class too (scale
        /// and rotation for each axis)
        /// </summary>
        internal override void UpdatePreviousPosition()
        {
            // Let the base class do its work
            base.UpdatePreviousPosition();

            // Update OpenGL-specific values
            LastXScale = XScale;
            LastYScale = YScale;
            LastZScale = ZScale;

            LastXAngle = XAngle;
            LastYAngle = YAngle;
            LastZAngle = ZAngle;
        }




        /// <summary>
        /// Retrieve the actual x Scale of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXScale(float interpFactor)
        {
            // If we have no previous x Scale then return the current x Scale
            if (LastXScale == float.MinValue) return XScale;
            // Otherwise interpolate between the previous Scale and the current Scale
            return CGameFunctions.Interpolate(interpFactor, XScale, LastXScale);
        }
        /// <summary>
        /// Retrieve the actual y Scale of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYScale(float interpFactor)
        {
            // If we have no previous y Scale then return the current y Scale
            if (LastYScale == float.MinValue) return YScale;
            // Otherwise interpolate between the previous Scale and the current Scale
            return CGameFunctions.Interpolate(interpFactor, YScale, LastYScale);
        }
        /// <summary>
        /// Retrieve the actual z Scale of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZScale(float interpFactor)
        {
            // If we have no previous z Scale then return the current z Scale
            if (LastZScale == float.MinValue) return ZScale;
            // Otherwise interpolate between the previous Scale and the current Scale
            return CGameFunctions.Interpolate(interpFactor, ZScale, LastZScale);
        }


        /// <summary>
        /// Retrieve the actual x Angle of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXAngle(float interpFactor)
        {
            // If we have no previous x Angle then return the current x Angle
            if (LastXAngle == float.MinValue) return XAngle;
            // Otherwise interpolate between the previous Angle and the current Angle
            return CGameFunctions.Interpolate(interpFactor, XAngle, LastXAngle);
        }
        /// <summary>
        /// Retrieve the actual y Angle of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYAngle(float interpFactor)
        {
            // If we have no previous y Angle then return the current y Angle
            if (LastYAngle == float.MinValue) return YAngle;
            // Otherwise interpolate between the previous Angle and the current Angle
            return CGameFunctions.Interpolate(interpFactor, YAngle, LastYAngle);
        }
        /// <summary>
        /// Retrieve the actual z Angle of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZAngle(float interpFactor)
        {
            // If we have no previous z Angle then return the current z Angle
            if (LastZAngle == float.MinValue) return ZAngle;
            // Otherwise interpolate between the previous Angle and the current Angle
            return CGameFunctions.Interpolate(interpFactor, ZAngle, LastZAngle);
        }


        /// <summary>
        /// Move this object to the front of all the objects being rendered
        /// </summary>
        public void MoveToFront()
        {
            // Remove the object from the list...
            GameEngine.GameObjects.Remove(this);
            // ...and then re-add it at the end of the list so that it is rendered last
            GameEngine.GameObjects.Add(this);
        }

        /// <summary>
        /// Move this object to the back of all the objects being rendered
        /// </summary>
        public void MoveToBack()
        {
            // Remove the object from the list...
            GameEngine.GameObjects.Remove(this);
            // ...and then re-add it at the start of the list so that it is rendered first
            GameEngine.GameObjects.Insert(0, this);
        }

        /// <summary>
        /// Provide default sorting for OpenGL objects so that they
        /// are ordered from those with the highest z position first
        /// (furthest away) to those with the lowest z position last
        /// (nearest).
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override int CompareTo(CGameObjectBase other)
        {
            // Sort by z position
            return ((CGameObjectOpenGLBase)other).ZPos.CompareTo(ZPos);
        }

    }
}
