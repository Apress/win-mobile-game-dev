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



        public static bool UnProject(float winx, float winy, float winz,
                                    float[] modelMatrix, float[] projMatrix,
                                    int[] viewport,
                                    out float objx, out float objy, out float objz)
        {
            float[] finalMatrix = new float[16];
            float[] vin = new float[4];
            float[] vout = new float[4];
            objx = 0;
            objy = 0;
            objz = 0;

            __gluMultMatricesd(modelMatrix, projMatrix, out finalMatrix);
            if (!__gluInvertMatrixd(finalMatrix, out finalMatrix)) return(false);

            vin[0]=winx;
            vin[1]=winy;
            vin[2]=winz;
            vin[3]=1.0f;

            /* Map x and y from window coordinates */
            vin[0] = (vin[0] - viewport[0]) / viewport[2];
            vin[1] = (vin[1] - viewport[1]) / viewport[3];

            /* Map to range -1 to 1 */
            vin[0] = vin[0] * 2 - 1;
            vin[1] = vin[1] * 2 - 1;
            vin[2] = vin[2] * 2 - 1;

            __gluMultMatrixVecd(finalMatrix, vin, out vout);
            if (vout[3] == 0.0) return false;
            vout[0] /= vout[3];
            vout[1] /= vout[3];
            vout[2] /= vout[3];
            objx = vout[0];
            objy = vout[1];
            objz = vout[2];
            return true;
        }

        private static void __gluMultMatricesd(float[] a, float[] b, out float[] r)
        {
            int i, j;
            r = new float[16];

            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    r[i * 4 + j] =
                        a[i * 4 + 0] * b[0 * 4 + j] +
                        a[i * 4 + 1] * b[1 * 4 + j] +
                        a[i * 4 + 2] * b[2 * 4 + j] +
                        a[i * 4 + 3] * b[3 * 4 + j];
                }
            }
        }

        /// <summary>
        /// Invert 4x4 matrix.
        /// </summary>
        private static bool __gluInvertMatrixd(float[] m, out float[] invOut)
        {
            float[] inv = new float[16];
            float det;
            int i;

            invOut = new float[16];

            inv[0] = m[5] * m[10] * m[15] - m[5] * m[11] * m[14] - m[9] * m[6] * m[15]
                     + m[9] * m[7] * m[14] + m[13] * m[6] * m[11] - m[13] * m[7] * m[10];
            inv[4] = -m[4] * m[10] * m[15] + m[4] * m[11] * m[14] + m[8] * m[6] * m[15]
                     - m[8] * m[7] * m[14] - m[12] * m[6] * m[11] + m[12] * m[7] * m[10];
            inv[8] = m[4] * m[9] * m[15] - m[4] * m[11] * m[13] - m[8] * m[5] * m[15]
                     + m[8] * m[7] * m[13] + m[12] * m[5] * m[11] - m[12] * m[7] * m[9];
            inv[12] = -m[4] * m[9] * m[14] + m[4] * m[10] * m[13] + m[8] * m[5] * m[14]
                     - m[8] * m[6] * m[13] - m[12] * m[5] * m[10] + m[12] * m[6] * m[9];
            inv[1] = -m[1] * m[10] * m[15] + m[1] * m[11] * m[14] + m[9] * m[2] * m[15]
                     - m[9] * m[3] * m[14] - m[13] * m[2] * m[11] + m[13] * m[3] * m[10];
            inv[5] = m[0] * m[10] * m[15] - m[0] * m[11] * m[14] - m[8] * m[2] * m[15]
                     + m[8] * m[3] * m[14] + m[12] * m[2] * m[11] - m[12] * m[3] * m[10];
            inv[9] = -m[0] * m[9] * m[15] + m[0] * m[11] * m[13] + m[8] * m[1] * m[15]
                     - m[8] * m[3] * m[13] - m[12] * m[1] * m[11] + m[12] * m[3] * m[9];
            inv[13] = m[0] * m[9] * m[14] - m[0] * m[10] * m[13] - m[8] * m[1] * m[14]
                     + m[8] * m[2] * m[13] + m[12] * m[1] * m[10] - m[12] * m[2] * m[9];
            inv[2] = m[1] * m[6] * m[15] - m[1] * m[7] * m[14] - m[5] * m[2] * m[15]
                     + m[5] * m[3] * m[14] + m[13] * m[2] * m[7] - m[13] * m[3] * m[6];
            inv[6] = -m[0] * m[6] * m[15] + m[0] * m[7] * m[14] + m[4] * m[2] * m[15]
                     - m[4] * m[3] * m[14] - m[12] * m[2] * m[7] + m[12] * m[3] * m[6];
            inv[10] = m[0] * m[5] * m[15] - m[0] * m[7] * m[13] - m[4] * m[1] * m[15]
                     + m[4] * m[3] * m[13] + m[12] * m[1] * m[7] - m[12] * m[3] * m[5];
            inv[14] = -m[0] * m[5] * m[14] + m[0] * m[6] * m[13] + m[4] * m[1] * m[14]
                     - m[4] * m[2] * m[13] - m[12] * m[1] * m[6] + m[12] * m[2] * m[5];
            inv[3] = -m[1] * m[6] * m[11] + m[1] * m[7] * m[10] + m[5] * m[2] * m[11]
                     - m[5] * m[3] * m[10] - m[9] * m[2] * m[7] + m[9] * m[3] * m[6];
            inv[7] = m[0] * m[6] * m[11] - m[0] * m[7] * m[10] - m[4] * m[2] * m[11]
                     + m[4] * m[3] * m[10] + m[8] * m[2] * m[7] - m[8] * m[3] * m[6];
            inv[11] = -m[0] * m[5] * m[11] + m[0] * m[7] * m[9] + m[4] * m[1] * m[11]
                     - m[4] * m[3] * m[9] - m[8] * m[1] * m[7] + m[8] * m[3] * m[5];
            inv[15] = m[0] * m[5] * m[10] - m[0] * m[6] * m[9] - m[4] * m[1] * m[10]
                     + m[4] * m[2] * m[9] + m[8] * m[1] * m[6] - m[8] * m[2] * m[5];

            det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];
            if (det == 0) return false;

            det = 1.0f / det;

            for (i = 0; i < 16; i++)
            {
                invOut[i] = inv[i] * det;
            }

            return true;
        }

        private static void __gluMultMatrixVecd(float[] matrix, float[] vin, out float[] vout)
        {
            int i;
            vout = new float[4];

            for (i = 0; i < 4; i++)
            {
                vout[i] =
                    vin[0] * matrix[0 * 4 + i] +
                    vin[1] * matrix[1 * 4 + i] +
                    vin[2] * matrix[2 * 4 + i] +
                    vin[3] * matrix[3 * 4 + i];
            }
        }

    }
}
