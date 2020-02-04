// ***********************************************************************
// Assembly         : StarMath
// Author           : MICampbell
// Created          : 05-14-2015
//
// Last Modified By : MICampbell
// Last Modified On : 07-07-2015
// ***********************************************************************
// <copyright file="svd.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Runtime.CompilerServices;

namespace StarMathLib
{
    public static partial class StarMath
    {
        /// <summary>
        /// Computes the singular value decomposition of A.
        /// </summary>
        /// <param name="A">The matrix in question, A can be rectangular m-by-n.</param>
        /// <returns>The singular values of A in ascending value, often indicated as sigma (provided as a vector).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] SingularValueDecomposition(this float[,] A)
        {
            float[,] U, V;
            return SingularValueDecomposition(false, A, out U, out V);
        }

        /// <summary>
        /// Computes the singular value decomposition of A.
        /// </summary>
        /// <param name="A">The matrix in question, A can be rectangular [m, n]</param>
        /// <param name="U">The m-by-m uitary matrix that pre-multiplies the singular values.</param>
        /// <param name="V">The n-by-n conjugate transpose matrix of V that post-multiplies the singular values.</param>
        /// <returns>The singular values of A in ascending value, often indicated as sigma (provided as a vector).</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float[] SingularValueDecomposition(this float[,] A, out float[,] U, out float[,] V)
        {
            return SingularValueDecomposition(true, A, out U, out V);
        }


        //This is equivalent to the GESVD LAPACK routine.
        /// <summary>
        /// Singulars the value decomposition.
        /// </summary>
        /// <param name="computeVectors">if set to <c>true</c> [compute vectors].</param>
        /// <param name="A">a.</param>
        /// <param name="u">The u.</param>
        /// <param name="vt">The vt.</param>
        /// <returns>System.Double[].</returns>
        /// <exception cref="System.ArithmeticException">SVD did not converge.</exception>
        private static float[] SingularValueDecomposition(bool computeVectors, float[,] A, out float[,] u,
            out float[,] vt)
        {
            var numRow = A.GetLength(0);
            var numCol = A.GetLength(1);
            u = new float[numRow, numRow];
            var v = new float[numCol, numCol];
            vt = new float[numCol, numCol];
            var sLength = Math.Min(numRow, numCol);
            var s = new float[sLength];
            var work = new float[numRow];
            var e = new float[numCol];
            var stemp = new float[Math.Min(numRow + 1, numCol)];

            int i, j, l, lp1;

            float t;

            var ncu = numRow;

            // Reduce matrix to bidiagonal form, storing the diagonal elements
            // in "s" and the super-diagonal elements in "e".
            var nct = Math.Min(numRow - 1, numCol);
            var nrt = Math.Max(0, Math.Min(numCol - 2, numRow));
            var lu = Math.Max(nct, nrt);

            for (l = 0; l < lu; l++)
            {
                lp1 = l + 1;
                if (l < nct)
                {
                    // Compute the transformation for the l-th column and
                    // place the l-th diagonal in vector s[l].
                    var sum = 0.0f;
                    for (var i1 = l; i1 < numRow; i1++)
                        sum += A[l, i1] * A[l, i1];
                    stemp[l] = MathF.Sqrt(sum);
                    if (!stemp[l].IsNegligible())
                    {
                        if (!A[l, l].IsNegligible())
                            stemp[l] = Absf(stemp[l]) * (A[l, l] / Absf(A[l, l]));
                        // A part of column "l" of Matrix A from row "l" to end multiply by 1.0 / s[l]
                        for (i = l; i < numRow; i++)
                            A[l, i] = A[l, i] * (1.0f / stemp[l]);
                        A[l, l] = 1.0f + A[l, l];
                    }

                    stemp[l] = -stemp[l];
                }

                for (j = lp1; j < numCol; j++)
                {
                    if (l < nct)
                    {
                        if (!stemp[l].IsNegligible())
                        {
                            // Apply the transformation.
                            t = 0.0f;
                            for (i = l; i < numRow; i++)
                                t += A[j, i] * A[l, i];
                            t = -t / A[l, l];
                            for (var ii = l; ii < numRow; ii++)
                                A[j, ii] += t * A[l, ii];
                        }
                    }
                    // Place the l-th row of matrix into "e" for the
                    // subsequent calculation of the row transformation.
                    e[j] = A[j, l];
                }

                if (computeVectors && l < nct)
                    // Place the transformation in "u" for subsequent back multiplication.
                    for (i = l; i < numRow; i++)
                        u[l, i] = A[l, i];

                if (l >= nrt) continue;

                // Compute the l-th row transformation and place the l-th super-diagonal in e(l).
                var enorm = 0.0f;
                for (i = lp1; i < e.Length; i++)
                    enorm += e[i] * e[i];

                e[l] = MathF.Sqrt(enorm);
                if (!e[l].IsNegligible())
                {
                    if (!e[lp1].IsNegligible())
                        e[l] = Absf(e[l]) * (e[lp1] / Absf(e[lp1]));

                    // Scale vector "e" from "lp1" by 1.0 / e[l]
                    for (i = lp1; i < e.Length; i++)
                        e[i] = e[i] * (1.0f / e[l]);

                    e[lp1] = 1.0f + e[lp1];
                }
                e[l] = -e[l];

                if (lp1 < numRow && !e[l].IsNegligible())
                {
                    // Apply the transformation.
                    for (i = lp1; i < numRow; i++)
                        work[i] = 0.0f;

                    for (j = lp1; j < numCol; j++)
                    {
                        for (var ii = lp1; ii < numRow; ii++)
                            work[ii] += e[j] * A[j, ii];
                    }

                    for (j = lp1; j < numCol; j++)
                    {
                        var ww = -e[j] / e[lp1];
                        for (var ii = lp1; ii < numRow; ii++)
                            A[j, ii] += ww * work[ii];
                    }
                }

                if (!computeVectors) continue;
                // Place the transformation in v for subsequent back multiplication.
                for (i = lp1; i < numCol; i++)
                    v[l, i] = e[i];
            }

            // Set up the final bidiagonal matrix or order m.
            var m = Math.Min(numCol, numRow + 1);
            var nctp1 = nct + 1;
            var nrtp1 = nrt + 1;
            if (nct < numCol)
                stemp[nctp1 - 1] = A[nctp1 - 1, nctp1 - 1];

            if (numRow < m)
                stemp[m - 1] = 0.0f;

            if (nrtp1 < m)
                e[nrtp1 - 1] = A[m - 1, nrtp1 - 1];

            e[m - 1] = 0.0f;
            // If required, generate "u".
            if (computeVectors)
            {
                for (j = nctp1 - 1; j < ncu; j++)
                {
                    for (i = 0; i < numRow; i++) u[j, i] = 0.0f;
                    u[j, j] = 1.0f;
                }

                for (l = nct - 1; l >= 0; l--)
                {
                    if (!stemp[l].IsNegligible())
                    {
                        for (j = l + 1; j < ncu; j++)
                        {
                            t = 0.0f;
                            for (i = l; i < numRow; i++)
                                t += u[j, i] * u[l, i];
                            t = -t / u[l, l];
                            for (var ii = l; ii < numRow; ii++)
                                u[j, ii] += t * u[l, ii];
                        }
                        // A part of column "l" of matrix A from row "l" to end multiply by -1.0
                        for (i = l; i < numRow; i++)
                            u[l, i] = u[l, i] * -1.0f;
                        u[l, l] = 1.0f + u[l, l];
                        for (i = 0; i < l; i++)
                            u[l, i] = 0.0f;
                    }
                    else
                    {
                        for (i = 0; i < numRow; i++)
                            u[l, i] = 0.0f;
                        u[l, l] = 1.0f;
                    }
                }
            }

            // If it is required, generate v.
            if (computeVectors)
            {
                for (l = numCol - 1; l >= 0; l--)
                {
                    lp1 = l + 1;
                    if (l < nrt)
                    {
                        if (!e[l].IsNegligible())
                        {
                            for (j = lp1; j < numCol; j++)
                            {
                                t = 0.0f;
                                for (i = lp1; i < numCol; i++)
                                    t += v[j, i] * v[l, i];
                                t = -t / v[l, lp1];
                                for (var ii = l; ii < numCol; ii++)
                                    v[j, ii] += t * v[l, ii];
                            }
                        }
                    }
                    for (i = 0; i < numCol; i++) v[l, i] = 0.0f;
                    v[l, l] = 1.0f;
                }
            }

            // Transform "s" and "e" so that they are float
            for (i = 0; i < m; i++)
            {
                float r;
                if (!stemp[i].IsNegligible())
                {
                    t = stemp[i];
                    r = stemp[i] / t;
                    stemp[i] = t;
                    if (i < m - 1) e[i] = e[i] / r;
                    if (computeVectors)
                        // A part of column "i" of matrix U from row 0 to end multiply by r
                        for (j = 0; j < numRow; j++) u[i, j] = u[i, j] * r;
                }

                // Exit
                if (i == m - 1) break;
                if (e[i].IsNegligible()) continue;
                t = e[i];
                r = t / e[i];
                e[i] = t;
                stemp[i + 1] = stemp[i + 1] * r;
                if (!computeVectors) continue;
                // A part of column "i+1" of matrix VT from row 0 to end multiply by r
                for (j = 0; j < numCol; j++) v[i + 1, j] = v[i + 1, +j] * r;
            }

            // Main iteration loop for the singular values.
            var mn = m;
            var iter = 0;

            while (m > 0)
            {
                // Quit if all the singular values have been found.
                // If too many iterations have been performed throw exception.
                if (iter >= MaxSvDiter)
                {
                    throw new ArithmeticException("SVD did not converge.");
                }

                // This section of the program inspects for negligible elements in the s and e arrays,
                // on completion the variables kase and l are set as follows:
                // kase = 1: if mS[m] and e[l-1] are negligible and l < m
                // kase = 2: if mS[l] is negligible and l < m
                // kase = 3: if e[l-1] is negligible, l < m, and mS[l, ..., mS[m] are not negligible (qr step).
                // kase = 4: if e[m-1] is negligible (convergence).
                float ztest;
                float test;
                for (l = m - 2; l >= 0; l--)
                {
                    test = Absf(stemp[l]) + Absf(stemp[l + 1]);
                    ztest = test + Absf(e[l]);
                    if (IsPracticallySame(ztest, test))
                    {
                        e[l] = 0.0f;
                        break;
                    }
                }

                int kase;
                if (l == m - 2) kase = 4;
                else
                {
                    int ls;
                    for (ls = m - 1; ls > l; ls--)
                    {
                        test = 0.0f;
                        if (ls != m - 1) test = test + Absf(e[ls]);
                        if (ls != l + 1) test = test + Absf(e[ls - 1]);
                        ztest = test + Absf(stemp[ls]);
                        if (IsPracticallySame(ztest, test))
                        {
                            stemp[ls] = 0.0f;
                            break;
                        }
                    }
                    if (ls == l) kase = 3;
                    else if (ls == m - 1) kase = 1;
                    else
                    {
                        kase = 2;
                        l = ls;
                    }
                }

                l = l + 1;

                // Perform the task indicated by kase.
                int k;
                float f;
                float cs;
                float sn;
                switch (kase)
                {
                    // Deflate negligible s[m].
                    case 1:
                        f = e[m - 2];
                        e[m - 2] = 0.0f;
                        float t1;
                        for (var kk = l; kk < m - 1; kk++)
                        {
                            k = m - 2 - kk + l;
                            t1 = stemp[k];

                            fromCartesianToPolar(ref t1, ref f, out cs, out sn);
                            stemp[k] = t1;
                            if (k != l)
                            {
                                f = -sn * e[k - 1];
                                e[k - 1] = cs * e[k - 1];
                            }

                            if (computeVectors)
                            {
                                // Rotate
                                for (i = 0; i < numCol; i++)
                                {
                                    var z = (cs * v[k, i]) + (sn * v[m - 1, i]);
                                    v[m - 1, i] = (cs * v[m - 1, i]) - (sn * v[k, i]);
                                    v[k, i] = z;
                                }
                            }
                        }

                        break;

                    // Split at negligible s[l].
                    case 2:
                        f = e[l - 1];
                        e[l - 1] = 0.0f;
                        for (k = l; k < m; k++)
                        {
                            t1 = stemp[k];
                            fromCartesianToPolar(ref t1, ref f, out cs, out sn);
                            stemp[k] = t1;
                            f = -sn * e[k];
                            e[k] = cs * e[k];
                            if (computeVectors)
                            {
                                // Rotate
                                for (i = 0; i < numRow; i++)
                                {
                                    var z = (cs * u[k, i]) + (sn * u[l - 1, i]);
                                    u[l - 1, i] = (cs * u[l - 1, i]) - (sn * u[k, i]);
                                    u[k, i] = z;
                                }
                            }
                        }

                        break;

                    // Perform one qr step.
                    case 3:

                        // calculate the shift.
                        var scale = 0.0f;
                        scale = Math.Max(scale, Absf(stemp[m - 1]));
                        scale = Math.Max(scale, Absf(stemp[m - 2]));
                        scale = Math.Max(scale, Absf(e[m - 2]));
                        scale = Math.Max(scale, Absf(stemp[l]));
                        scale = Math.Max(scale, Absf(e[l]));
                        var sm = stemp[m - 1] / scale;
                        var smm1 = stemp[m - 2] / scale;
                        var emm1 = e[m - 2] / scale;
                        var sl = stemp[l] / scale;
                        var el = e[l] / scale;
                        var b = (((smm1 + sm) * (smm1 - sm)) + (emm1 * emm1)) / 2.0f;
                        var c = (sm * emm1) * (sm * emm1);
                        var shift = 0.0f;
                        if (!b.IsNegligible() || !c.IsNegligible())
                        {
                            shift = MathF.Sqrt((b * b) + c);
                            if (b < 0.0)
                                shift = -shift;
                            shift = c / (b + shift);
                        }

                        f = ((sl + sm) * (sl - sm)) + shift;
                        var g = sl * el;

                        // Chase zeros
                        for (k = l; k < m - 1; k++)
                        {
                            fromCartesianToPolar(ref f, ref g, out cs, out sn);
                            if (k != l)
                                e[k - 1] = f;
                            f = (cs * stemp[k]) + (sn * e[k]);
                            e[k] = (cs * e[k]) - (sn * stemp[k]);
                            g = sn * stemp[k + 1];
                            stemp[k + 1] = cs * stemp[k + 1];
                            if (computeVectors)
                            {
                                for (i = 0; i < numCol; i++)
                                {
                                    var z = (cs * v[k, i]) + (sn * v[k + 1, i]);
                                    v[k + 1, i] = (cs * v[k + 1, i]) - (sn * v[k, i]);
                                    v[k, i] = z;
                                }
                            }
                            fromCartesianToPolar(ref f, ref g, out cs, out sn);
                            stemp[k] = f;
                            f = (cs * e[k]) + (sn * stemp[k + 1]);
                            stemp[k + 1] = -(sn * e[k]) + (cs * stemp[k + 1]);
                            g = sn * e[k + 1];
                            e[k + 1] = cs * e[k + 1];
                            if (computeVectors && k < numRow)
                            {
                                for (i = 0; i < numRow; i++)
                                {
                                    var z = (cs * u[k, i]) + (sn * u[k + 1, i]);
                                    u[k + 1, i] = (cs * u[k + 1, i]) - (sn * u[k, i]);
                                    u[k, i] = z;
                                }
                            }
                        }

                        e[m - 2] = f;
                        iter = iter + 1;
                        break;

                    // Convergence
                    case 4:

                        // Make the singular value  positive
                        if (stemp[l] < 0.0)
                        {
                            stemp[l] = -stemp[l];
                            if (computeVectors)
                            {
                                // A part of column "l" of matrix VT from row 0 to end multiply by -1
                                for (i = 0; i < numCol; i++)
                                    v[l, i] = v[l, i] * -1.0f;
                            }
                        }

                        // Order the singular value.
                        while (l != mn - 1)
                        {
                            if (stemp[l] >= stemp[l + 1]) break;
                            t = stemp[l];
                            stemp[l] = stemp[l + 1];
                            stemp[l + 1] = t;
                            if (computeVectors && l < numCol)
                            {
                                // Swap columns l, l + 1
                                for (i = 0; i < numCol; i++)
                                {
                                    var z = v[l, i];
                                    v[l, i] = v[l + 1, i];
                                    v[l + 1, i] = z;
                                }
                            }

                            if (computeVectors && l < numRow)
                            {
                                // Swap columns l, l + 1
                                for (i = 0; i < numRow; i++)
                                {
                                    var z = u[l, i];
                                    u[l, i] = u[l + 1, i];
                                    u[l + 1, i] = z;
                                }
                            }
                            l = l + 1;
                        }

                        iter = 0;
                        m = m - 1;
                        break;
                }
            }
            if (computeVectors)
            {
                // Finally transpose "v" to get "vt" matrix
                for (i = 0; i < numCol; i++)
                    for (j = 0; j < numCol; j++)
                        vt[j, i] = v[i, j];
            }
            return s;
        }

        /// <summary>
        /// Given the Cartesian coordinates (x, y) of a point p, these function return the parameters da, db, c, and s
        /// associated with the Givens rotation that zeros the y-coordinate of the point.
        /// </summary>
        /// <param name="x">Provides the x-coordinate of the point p. On exit contains the parameter r associated with the Givens
        /// rotation</param>
        /// <param name="y">Provides the y-coordinate of the point p. On exit contains the parameter z associated with the Givens
        /// rotation</param>
        /// <param name="cosAngle">Contains the parameter c associated with the Givens rotation</param>
        /// <param name="sinAngle">Contains the parameter s associated with the Givens rotation</param>
        /// <remarks>This is equivalent to the DROTG LAPACK routine.</remarks>
        private static void fromCartesianToPolar(ref float x, ref float y, out float cosAngle, out float sinAngle)
        {
            var absX = Absf(x);
            var absY = Absf(y);
            var scale = absX + absY;

            if (scale.IsNegligible())
            {
                cosAngle = 1.0f;
                sinAngle = 0.0f;
                x = 0.0f;
                y = 0.0f;
                return;
            }
            var sign = Math.Sign((absX > absY) ? x : y);
            var sda = x / scale;
            var sdb = y / scale;
            var r = sign * scale * MathF.Sqrt((sda * sda) + (sdb * sdb));
            cosAngle = x / r;
            sinAngle = y / r;
            x = r;

            if (absX > absY)
                y = sinAngle;
            else if (absY >= absX && !cosAngle.IsNegligible())
                y = 1.0f / cosAngle;
            else y = 1;
        }
    }
}