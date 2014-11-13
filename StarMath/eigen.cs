﻿/*************************************************************************
*     This file & class is part of the StarMath Project
*     Copyright 2014 Matthew Ira Campbell, PhD.
*
*     StarMath is free software: you can redistribute it and/or modify
*     it under the terms of the GNU General Public License as published by
*     the Free Software Foundation, either version 3 of the License, or
*     (at your option) any later version.
*  
*     StarMath is distributed in the hope that it will be useful,
*     but WITHOUT ANY WARRANTY; without even the implied warranty of
*     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*     GNU General Public License for more details.
*  
*     You should have received a copy of the GNU General Public License
*     along with StarMath.  If not, see <http://www.gnu.org/licenses/>.
*     
*     Please find further details and contact information on StarMath
*     at http://starmath.codeplex.com/.
*************************************************************************/

using System;

namespace StarMathLib
{
    public static partial class StarMath
    {
        /// <summary>
        /// Gets the eigenvalues for matrix, A.
        /// </summary>
        /// <param name="A">the matrix in question, A.</param>
        /// <returns></returns>
        public static double[][] GetEigenValues(double[,] A)
        {
            return GetEigenValues(A, A.GetLength(0));
        }

        /// <summary>                   
        /// Gets the eigenvalues for matrix, A.
        /// </summary>
        /// <param name="A">the matrix in question, A.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static double[][] GetEigenValues(double[,] A, int length)
        {
            double[][] eigenVectors;
            return GetEigenValuesAndVectors(A, out eigenVectors, length);
        }

        /// <summary>
        /// Gets the eigenvalues and eigenvectors for matrix, A.
        /// </summary>
        /// <param name="A">the matrix in question, A.</param>
        /// <param name="eigenVectors">The eigen vectors.</param>
        /// <returns></returns>
        public static double[][] GetEigenValuesAndVectors(double[,] A, out double[][] eigenVectors)
        {
            return GetEigenValuesAndVectors(A, out eigenVectors, A.GetLength(0));
        }

        /// <summary>
        /// Gets the eigenvalues and eigenvectors for matrix, A.
        /// </summary>
        /// <param name="A">the matrix in question, A.</param>
        /// <param name="eigenVectors">The eigen vectors.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static double[][] GetEigenValuesAndVectors(double[,] A, out double[][] eigenVectors, int length)
        {
            if (length != A.GetLength(0) || length != A.GetLength(1))
                throw new Exception("Matrix, A, must be square.");
            eigenVectors = new double[length][];
            /* start out with the eigenvectors assigned to unit vectors */
            for (int i = 0; i < length; i++)
            {
                var eigenVector = new double[length];
                eigenVector[i] = 1.0;
                eigenVectors[i] = eigenVector;
            }
            var eigenvaluesReal = new double[length];
            var eigenvaluesImag = new double[length];
            var B = (double[,])A.Clone();

            #region Reduce to Hessenberg form

            // This is derived from the Algol procedures orthes and ortran,
            // by Martin and Wilkinson, Handbook for Auto. Comp.,
            // Vol.ii-Linear Algebra, and the corresponding
            // Fortran subroutines in EISPACK.
            var ort = new double[length];
            int high = length - 1;
            for (int m = 1; m <= high - 1; m++)
            {
                int mm1 = m - 1;
                // Scale column.
                double scale = 0.0;
                for (int i = m; i <= high; i++)
                    scale += Math.Abs(B[mm1, i]);

                if (scale != 0.0)
                {
                    // Compute Householder transformation.
                    double h = 0.0;
                    for (int i = high; i >= m; i--)
                    {
                        ort[i] = B[mm1, i] / scale;
                        h += ort[i] * ort[i];
                    }

                    double g = Math.Sqrt(h);
                    if (ort[m] > 0) g = -g;
                    h = h - (ort[m] * g);
                    ort[m] = ort[m] - g;

                    // Apply Householder similarity transformation
                    // H = (I-u*u'/h)*H*(I-u*u')/h)
                    for (int j = m; j < length; j++)
                    {
                        double f = 0.0;
                        for (int i = length - 1; i >= m; i--)
                            f += ort[i] * B[j, i];
                        f = f / h;
                        for (int i = m; i <= high; i++)
                            B[j, i] -= f * ort[i];
                    }

                    for (int i = 0; i <= high; i++)
                    {
                        double f = 0.0;
                        for (int j = high; j >= m; j--)
                            f += ort[j] * B[j, i];
                        f = f / h;
                        for (int j = m; j <= high; j++)
                            B[j, i] -= f * ort[j];
                    }

                    ort[m] = scale * ort[m];
                    B[mm1, m] = scale * g;
                }
            }

            // Accumulate transformations (Algol's ortran).
            for (int m = high - 1; m >= 1; m--)
            {
                int mm1 = m - 1;
                if (B[mm1, m] != 0.0)
                {
                    for (int i = m + 1; i <= high; i++)
                        ort[i] = B[mm1, i];
                    for (int j = m; j <= high; j++)
                    {
                        double g = 0.0;
                        for (int i = m; i <= high; i++)
                            g += ort[i] * eigenVectors[j][i];
                        // Double division avoids possible underflow
                        g = (g / ort[m]) / B[mm1, m];
                        for (int i = m; i <= high; i++)
                            eigenVectors[j][i] += g * ort[i];
                    }
                }
            }

            #endregion

            #region now convert to real Schur form
            // Initialize
            int n = length - 1;
            double eps = Math.Pow(2.0, -52.0);
            double exshift = 0.0;
            double p = 0, q = 0, r = 0, s = 0, z = 0;
            double w, x, y;

            // Store roots isolated by balanc and compute matrix norm
            double norm = 0.0;
            for (int i = 0; i < length; i++)
                for (int j = Math.Max(i - 1, 0); j < length; j++)
                    norm = norm + Math.Abs(B[j, i]);

            // Outer loop over eigenvalue index
            int iter = 0;
            while (n >= 0)
            {
                // Look for single small sub-diagonal element
                int l = n;
                while (l > 0)
                {
                    int lm1 = l - 1;
                    s = Math.Abs(B[lm1, lm1]) + Math.Abs(B[l, l]);
                    if (s == 0.0) s = norm;
                    if (Math.Abs(B[lm1, l]) < eps * s)
                        break;
                    l--;
                }

                // Check for convergence
                // One root found
                if (l == n)
                {
                    B[n, n] += exshift;
                    eigenvaluesReal[n] = B[n, n];
                    eigenvaluesImag[n] = 0.0;
                    n--;
                    iter = 0;

                    // Two roots found
                }
                else if (l == n - 1)
                {
                    int nm1 = n - 1;
                    w = B[nm1, n] * B[n, nm1];
                    p = (B[nm1, nm1] - B[n, n]) / 2.0;
                    q = (p * p) + w;
                    z = Math.Sqrt(Math.Abs(q));

                    B[n, n] += exshift;
                    B[nm1, nm1] += exshift;
                    x = B[n, n];

                    // Real pair
                    if (q >= 0)
                    {
                        z = (p >= 0) ? p + z : p - z;
                        eigenvaluesReal[nm1] = x + z;

                        eigenvaluesReal[n] = eigenvaluesReal[nm1];
                        if (z != 0.0) eigenvaluesReal[n] = x - (w / z);
                        eigenvaluesImag[n - 1] = 0.0;
                        eigenvaluesImag[n] = 0.0;
                        x = B[nm1, n];
                        s = Math.Abs(x) + Math.Abs(z);
                        p = x / s;
                        q = z / s;
                        r = Math.Sqrt((p * p) + (q * q));
                        p = p / r;
                        q = q / r;

                        // Row modification
                        for (int j = n - 1; j < length; j++)
                        {
                            z = B[j, nm1];
                            B[j, nm1] = (q * z) + (p * B[j, n]);
                            B[j, n] = (q * B[j, n]) - (p * z);
                        }
                        // Column modification
                        for (int i = 0; i <= n; i++)
                        {
                            z = B[nm1, i];
                            B[nm1, i] = (q * z) + (p * B[n, i]);
                            B[n, i] = (q * B[n, i]) - (p * z);
                        }
                        // Accumulate transformations
                        for (int i = 0; i < length; i++)
                        {
                            z = eigenVectors[nm1][i];
                            eigenVectors[nm1][i] = (q * z) + (p * eigenVectors[n][i]);
                            eigenVectors[n][i] = (q * eigenVectors[n][i]) - (p * z);
                        }
                        // Complex pair
                    }
                    else
                    {
                        eigenvaluesReal[n - 1] = x + p;
                        eigenvaluesReal[n] = x + p;
                        eigenvaluesImag[n - 1] = z;
                        eigenvaluesImag[n] = -z;
                    }
                    n = n - 2;
                    iter = 0;
                    // No convergence yet
                }
                else
                {
                    int nm1 = n - 1;
                    // Form shift
                    x = B[n, n];
                    y = 0.0;
                    w = 0.0;
                    if (l < n)
                    {
                        y = B[nm1, nm1];
                        w = B[nm1, n] * B[n, nm1];
                    }

                    // Wilkinson's original ad hoc shift
                    if (iter == 10)
                    {
                        exshift += x;
                        for (int i = 0; i <= n; i++) B[i, i] -= x;
                        s = Math.Abs(B[nm1, n]) + Math.Abs(B[(n - 2), nm1]);
                        x = y = 0.75 * s;
                        w = (-0.4375) * s * s;
                    }

                    // MATLAB's new ad hoc shift
                    if (iter == 30)
                    {
                        s = (y - x) / 2.0;
                        s = (s * s) + w;
                        if (s > 0)
                        {
                            s = Math.Sqrt(s);
                            if (y < x) s = -s;
                            s = x - (w / (((y - x) / 2.0) + s));
                            for (int i = 0; i <= n; i++) B[i, i] -= s;
                            exshift += s;
                            x = y = w = 0.964;
                        }
                    }

                    iter = iter + 1;
                    if (iter >= 30 * length)
                    {
                        throw new Exception("Eigen decomposition does not converge.");
                    }

                    // Look for two consecutive small sub-diagonal elements
                    int m = n - 2;
                    while (m >= l)
                    {
                        int mp1 = m + 1;
                        int mm1 = m - 1;
                        z = B[m, m];
                        r = x - z;
                        s = y - z;
                        p = (((r * s) - w) / B[m, mp1]) + B[mp1, m];
                        q = B[mp1, mp1] - z - r - s;
                        r = B[mp1, (m + 2)];
                        s = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                        p = p / s;
                        q = q / s;
                        r = r / s;

                        if ((m == l) || (Math.Abs(B[mm1, m]) * (Math.Abs(q) + Math.Abs(r)) <
                            eps * (Math.Abs(p) * (Math.Abs(B[mm1, mm1]) + Math.Abs(z) + Math.Abs(B[mp1, mp1])))))
                            break;
                        m--;
                    }

                    int mp2 = m + 2;
                    for (int i = mp2; i <= n; i++)
                    {
                        B[(i - 2), i] = 0.0;
                        if (i > mp2) B[(i - 3), i] = 0.0;
                    }

                    // Double QR step involving rows l:n and columns m:n
                    for (int k = m; k <= n - 1; k++)
                    {
                        bool notlast = k != n - 1;
                        int km1 = k - 1;
                        int kp1 = k + 1;
                        int kp2 = k + 2;
                        if (k != m)
                        {
                            p = B[km1, k];
                            q = B[km1, kp1];
                            r = notlast ? B[km1, kp2] : 0.0;
                            x = Math.Abs(p) + Math.Abs(q) + Math.Abs(r);
                            if (x == 0.0) continue;

                            p = p / x;
                            q = q / x;
                            r = r / x;
                        }

                        s = Math.Sqrt((p * p) + (q * q) + (r * r));
                        if (p < 0) s = -s;

                        if (s != 0.0)
                        {
                            if (k != m) B[km1, k] = (-s) * x;
                            else if (l != m) B[km1, k] = -B[km1, k];
                            p = p + s;
                            x = p / s;
                            y = q / s;
                            z = r / s;
                            q = q / p;
                            r = r / p;

                            // Row modification
                            for (int j = k; j < length; j++)
                            {
                                p = B[j, k] + (q * B[j, kp1]);
                                if (notlast)
                                {
                                    p = p + (r * B[j, kp2]);
                                    B[j, kp2] -= (p * z);
                                }

                                B[j, k] -= (p * x);
                                B[j, kp1] -= (p * y);
                            }

                            // Column modification
                            for (int i = 0; i <= Math.Min(n, k + 3); i++)
                            {
                                p = (x * B[k, i]) + (y * B[kp1, i]);

                                if (notlast)
                                {
                                    p = p + (z * B[kp2, i]);
                                    B[kp2, i] -= (p * r);
                                }

                                B[k, i] -= p;
                                B[kp1, i] -= (p * q);
                            }

                            // Accumulate transformations
                            for (int i = 0; i < length; i++)
                            {
                                p = (x * eigenVectors[k][i]) + (y * eigenVectors[kp1][i]);

                                if (notlast)
                                {
                                    p = p + (z * eigenVectors[kp2][i]);
                                    eigenVectors[kp2][i] -= p * r;
                                }

                                eigenVectors[k][i] -= p;
                                eigenVectors[kp1][i] -= p * q;
                            }
                        } // (s != 0)
                    } // k loop
                } // check convergence
            } // while (n >= low)

            // Backsubstitute to find vectors of upper triangular form
            if (norm == 0.0)
            {
                throw new Exception("Eigen decomposition failed due to norm = 0.");
            }

            for (n = length - 1; n >= 0; n--)
            {
                int nm1 = n - 1;
                p = eigenvaluesReal[n];
                q = eigenvaluesImag[n];
                // Real vector
                double t;
                if (q == 0.0)
                {
                    int l = n;
                    B[n, n] = 1.0;
                    for (int i = n - 1; i >= 0; i--)
                    {
                        int ip1 = i + 1;
                        w = B[i, i] - p;
                        r = 0.0;
                        for (int j = l; j <= n; j++)
                        {
                            r = r + (B[j, i] * B[n, j]);
                        }

                        if (eigenvaluesImag[i] < 0.0)
                        {
                            z = w;
                            s = r;
                        }
                        else
                        {
                            l = i;
                            if (eigenvaluesImag[i] == 0.0)
                            {
                                if (w != 0.0)
                                {
                                    B[n, i] = (-r) / w;
                                }
                                else
                                {
                                    B[n, i] = (-r) / (eps * norm);
                                }

                                // Solve real equations
                            }
                            else
                            {
                                x = B[ip1, i];
                                y = B[i, ip1];
                                q = ((eigenvaluesReal[i] - p) * (eigenvaluesReal[i] - p)) +
                                    (eigenvaluesImag[i] * eigenvaluesImag[i]);
                                t = ((x * s) - (z * r)) / q;
                                B[n, i] = t;
                                if (Math.Abs(x) > Math.Abs(z))
                                {
                                    B[n, ip1] = (-r - (w * t)) / x;
                                }
                                else
                                {
                                    B[n, ip1] = (-s - (y * t)) / z;
                                }
                            }

                            // Overflow control
                            t = Math.Abs(B[n, i]);
                            if ((eps * t) * t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    B[n, j] /= t;
                                }
                            }
                        }
                    }

                    // Complex vector
                }
                else if (q < 0)
                {
                    int l = n - 1;

                    // Last vector component imaginary so matrix is triangular
                    if (Math.Abs(B[nm1, n]) > Math.Abs(B[n, nm1]))
                    {
                        B[nm1, nm1] = q / B[nm1, n];
                        B[n, nm1] = (-(B[n, n] - p)) / B[nm1, n];
                    }
                    else
                    {
                        double[] res = complexNumberDivide(0.0, -B[n, nm1], B[nm1, nm1] - p, q);
                        B[nm1, nm1] = res[0];
                        B[n, nm1] = res[1];
                    }

                    B[nm1, n] = 0.0;
                    B[n, n] = 1.0;
                    for (int i = n - 2; i >= 0; i--)
                    {
                        int ip1 = i + 1;
                        double ra = 0.0;
                        double sa = 0.0;
                        for (int j = l; j <= n; j++)
                        {
                            ra = ra + (B[j, i] * B[nm1, j]);
                            sa = sa + (B[j, i] * B[n, j]);
                        }

                        w = B[i, i] - p;

                        if (eigenvaluesImag[i] < 0.0)
                        {
                            z = w;
                            r = ra;
                            s = sa;
                        }
                        else
                        {
                            l = i;
                            if (eigenvaluesImag[i] == 0.0)
                            {
                                double[] res = complexNumberDivide(-ra, -sa, w, q);
                                B[nm1, i] = res[0];
                                B[n, i] = res[1];
                            }
                            else
                            {
                                // Solve complex equations
                                x = B[ip1, i];
                                y = B[i, ip1];

                                double vr = ((eigenvaluesReal[i] - p) * (eigenvaluesReal[i] - p)) +
                                            (eigenvaluesImag[i] * eigenvaluesImag[i]) - (q * q);
                                double vi = (eigenvaluesReal[i] - p) * 2.0 * q;
                                if ((vr == 0.0) && (vi == 0.0))
                                    vr = eps * norm * (Math.Abs(w) + Math.Abs(q) + Math.Abs(x) + Math.Abs(y) + Math.Abs(z));
                                double[] res = complexNumberDivide((x * r) - (z * ra) + (q * sa), (x * s) - (z * sa) - (q * ra), vr,
                                    vi);
                                B[nm1, i] = res[0];
                                B[n, i] = res[1];
                                if (Math.Abs(x) > (Math.Abs(z) + Math.Abs(q)))
                                {
                                    B[nm1, ip1] = (-ra - (w * B[nm1, i]) + (q * B[n, i])) / x;
                                    B[n, ip1] = (-sa - (w * B[n, i]) - (q * B[nm1, i])) / x;
                                }
                                else
                                {
                                    res = complexNumberDivide(-r - (y * B[nm1, i]), -s - (y * B[n, i]), z, q);
                                    B[nm1, ip1] = res[0];
                                    B[n, ip1] = res[1];
                                }
                            }

                            // Overflow control
                            t = Math.Max(Math.Abs(B[nm1, i]), Math.Abs(B[n, i]));
                            if ((eps * t) * t > 1)
                            {
                                for (int j = i; j <= n; j++)
                                {
                                    B[nm1, j] /= t;
                                    B[n, j] /= t;
                                }
                            }
                        }
                    }
                }
            }

            // Back transformation to get eigenvectors of original matrix
            for (int j = length - 1; j >= 0; j--)
            {
                for (int i = 0; i < length; i++)
                {
                    z = 0.0;
                    for (int k = 0; k <= j; k++)
                        z = z + (eigenVectors[k][i] * B[j, k]);
                    eigenVectors[j][i] = z;
                }
            }

            #endregion

            return new[] { eigenvaluesReal, eigenvaluesImag };
        }

        private static double[] complexNumberDivide(double xreal, double ximag, double yreal, double yimag)
        {
            if (Math.Abs(yimag) < Math.Abs(yreal))
            {
                return new[]
                {
                    (xreal + (ximag*(yimag/yreal)))/(yreal + (yimag*(yimag/yreal))),
                    (ximag - (xreal*(yimag/yreal)))/(yreal + (yimag*(yimag/yreal)))
                };
            }
            return new[]
            {
                (ximag + (xreal*(yreal/yimag)))/(yimag + (yreal*(yreal/yimag))),
                (-xreal + (ximag*(yreal/yimag)))/(yimag + (yreal*(yreal/yimag)))
            };
        }
    }
}