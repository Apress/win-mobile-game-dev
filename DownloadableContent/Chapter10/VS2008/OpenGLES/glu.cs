using System;

using System.Collections.Generic;
using System.Text;

namespace OpenGLES
{
    public class glu
    {

        public const float PI = 3.1415926535f;

        /// <summary>
        /// Set up a perspective projection matrix
        /// </summary>
        /// <param name="fovy">Specifies the field of view angle, in degrees, in the y direction.</param>
        /// <param name="aspect">Specifies the aspect ratio that determines the field of view in the x direction.
        /// The aspect ratio is the ratio of x (width) to y (height).</param>
        /// <param name="zNear">Specifies the distance from the viewer to the near clipping plane (always positive).</param>
        /// <param name="zFar">Specifies the distance from the viewer to the far clipping plane (always positive).</param>
        public static void Perspective(float fovy, float aspect, float zNear, float zFar)
        {
            float xmin, xmax, ymin, ymax;

            ymax = zNear * (float)Math.Tan(fovy * PI / 360.0);
            ymin = -ymax;
            xmin = ymin * aspect;
            xmax = ymax * aspect;

            gl.Frustumf(xmin, xmax, ymin, ymax, zNear, zFar);
        }

    }
}
