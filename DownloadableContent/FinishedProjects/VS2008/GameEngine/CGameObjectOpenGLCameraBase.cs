using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GameEngine
{
    public class CGameObjectOpenGLCameraBase : CGameObjectOpenGLBase
    {

        // Properties to store the current and previous camera center position
        private float _xcenter = 0;
        private float _ycenter = 0;
        private float _zcenter = 0;
        private float _lastxcenter = float.MinValue;
        private float _lastycenter = float.MinValue;
        private float _lastzcenter = float.MinValue;

        // Properties to store the current and previous camera up vector
        private float _xup = 0;
        private float _yup = 1;
        private float _zup = 0;
        private float _lastxup = float.MinValue;
        private float _lastyup = float.MinValue;
        private float _lastzup = float.MinValue;

        // Store the current camera state and its matrix
        private float[,] _cachedCameraMatrix;


        //-------------------------------------------------------------------------------------
        // Class constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="gameEngine"></param>
        public CGameObjectOpenGLCameraBase(CGameEngineBase gameEngine)
            : base(gameEngine)
        {
            // No constructor code required
        }


        //-------------------------------------------------------------------------------------
        // Property access

        /// <summary>
        /// Sets or returns the current x center of the object 
        /// </summary>
        public virtual float XCenter
        {
            get { return _xcenter; }
            set { _xcenter = value; }
        }
        /// <summary>
        /// Sets or returns the current y center of the object 
        /// </summary>
        public virtual float YCenter
        {
            get { return _ycenter; }
            set { _ycenter = value; }
        }
        /// <summary>
        /// Sets or returns the current z center of the object 
        /// </summary>
        public virtual float ZCenter
        {
            get { return _zcenter; }
            set { _zcenter = value; }
        }
        /// <summary>
        /// Sets or returns the previous x center of the object 
        /// </summary>
        protected float LastXCenter
        {
            get { return _lastxcenter; }
            set { _lastxcenter = value; }
        }
        /// <summary>
        /// Sets or returns the previous y center of the object 
        /// </summary>
        protected float LastYCenter
        {
            get { return _lastycenter; }
            set { _lastycenter = value; }
        }
        /// <summary>
        /// Sets or returns the previous z center of the object 
        /// </summary>
        protected float LastZCenter
        {
            get { return _lastzcenter; }
            set { _lastzcenter = value; }
        }



        /// <summary>
        /// Sets or returns the current x up of the object
        /// </summary>
        public virtual float XUp
        {
            get { return _xup; }
            set { _xup = value; }
        }
        /// <summary>
        /// Sets or returns the current y up of the object
        /// </summary>
        public virtual float YUp
        {
            get { return _yup; }
            set { _yup = value; }
        }
        /// <summary>
        /// Sets or returns the current z up of the object
        /// </summary>
        public virtual float ZUp
        {
            get { return _zup; }
            set { _zup = value; }
        }
        /// <summary>
        /// Sets or returns the previous x up of the object
        /// </summary>
        protected float LastXUp
        {
            get { return _lastxup; }
            set { _lastxup = value; }
        }
        /// <summary>
        /// Sets or returns the previous y up of the object
        /// </summary>
        protected float LastYUp
        {
            get { return _lastyup; }
            set { _lastyup = value; }
        }
        /// <summary>
        /// Sets or returns the previous z up of the object
        /// </summary>
        protected float LastZUp
        {
            get { return _lastzup; }
            set { _lastzup = value; }
        }

        internal float[,] CachedCameraMatrix
        {
            get { return _cachedCameraMatrix; }
            set { _cachedCameraMatrix = value; }
        }


        //-------------------------------------------------------------------------------------
        // Game engine operations


        public override void Render(System.Drawing.Graphics gfx, float interpFactor)
        {
            base.Render(gfx, interpFactor);

            // Clear the cached camera transformation matrix
            _cachedCameraMatrix = null;
        }

        /// <summary>
        /// In addition to the positions maintained by the base class, update
        /// the camera positions maintained by this class too
        /// </summary>
        protected internal override void UpdatePreviousPosition()
        {
            // Let the base class do its work
            base.UpdatePreviousPosition();

            // Update camera-specific values
            LastXCenter = XCenter;
            LastYCenter = YCenter;
            LastZCenter = ZCenter;

            LastXUp = XUp;
            LastYUp = YUp;
            LastZUp = ZUp;
        }


        /// <summary>
        /// Retrieve the actual x center of the object  based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXCenter(float interpFactor)
        {
            // If we have no previous x Center then return the current x Center
            if (LastXCenter == float.MinValue) return XCenter;
            // Otherwise interpolate between the previous Center and the current Center
            return CGameFunctions.Interpolate(interpFactor, XCenter, LastXCenter);
        }
        /// <summary>
        /// Retrieve the actual y center of the object  based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYCenter(float interpFactor)
        {
            // If we have no previous y Center then return the current y Center
            if (LastYCenter == float.MinValue) return YCenter;
            // Otherwise interpolate between the previous Center and the current Center
            return CGameFunctions.Interpolate(interpFactor, YCenter, LastYCenter);
        }
        /// <summary>
        /// Retrieve the actual z center of the object  based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZCenter(float interpFactor)
        {
            // If we have no previous z Center then return the current z Center
            if (LastZCenter == float.MinValue) return ZCenter;
            // Otherwise interpolate between the previous Center and the current Center
            return CGameFunctions.Interpolate(interpFactor, ZCenter, LastZCenter);
        }



        /// <summary>
        /// Retrieve the actual x up of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayXUp(float interpFactor)
        {
            // If we have no previous x Up then return the current x Up
            if (LastXUp == float.MinValue) return XUp;
            // Otherwise interpolate between the previous Up and the current Up
            return CGameFunctions.Interpolate(interpFactor, XUp, LastXUp);
        }
        /// <summary>
        /// Retrieve the actual y up of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayYUp(float interpFactor)
        {
            // If we have no previous y Up then return the current y Up
            if (LastYUp == float.MinValue) return YUp;
            // Otherwise interpolate between the previous Up and the current Up
            return CGameFunctions.Interpolate(interpFactor, YUp, LastYUp);
        }
        /// <summary>
        /// Retrieve the actual z up of the object based on the interpolation factor for the current update
        /// </summary>
        /// <param name="interpFactor">The interpolation factor for the current update</param>
        /// <returns></returns>
        public float GetDisplayZUp(float interpFactor)
        {
            // If we have no previous z Up then return the current z Up
            if (LastZUp == float.MinValue) return ZUp;
            // Otherwise interpolate between the previous Up and the current Up
            return CGameFunctions.Interpolate(interpFactor, ZUp, LastZUp);
        }
    }
}
