// ***********************************************************************
// Assembly         : StarMath
// Author           : Matthew I. Campbell
// Created          : 02-28-2016
// Modified from    : Timothy A. Davis, 2006-2014
// Last Modified By : Matt
// Last Modified On : 02-28-2016
// ***********************************************************************
// <copyright file="CSparseNetMain.cs" company="Design Engineering Lab -- MICampbell">
//     2014
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;

namespace StarMathLib.CSparse
{
    internal static class Main
    {
        private const double tol = 1.0;

        #region solve LU Decomposition

        /// <summary>
        ///     Solve a lower triangular system by forward elimination, Lx=b.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        internal static void SolveLower(CompressedColumnStorage L, double[] x)
        {
            var lp = L.ColumnPointers;
            var li = L.RowIndices;
            var lx = L.Values;

            for (var j = 0; j < L.ncols; j++)
            {
                x[j] /= lx[lp[j]];
                var k = lp[j + 1];
                for (var p = lp[j] + 1; p < k; p++)
                    x[li[p]] -= lx[p]*x[j];
            }
        }

        /// <summary>
        ///     Solve an upper triangular system by backward elimination, Ux=b.
        /// </summary>
        /// <param name="U"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        internal static void SolveUpper(CompressedColumnStorage U, double[] x)
        {
            var up = U.ColumnPointers;
            var ui = U.RowIndices;
            var ux = U.Values;

            for (var j = U.ncols - 1; j >= 0; j--)
            {
                x[j] /= ux[up[j + 1] - 1];
                var k = up[j + 1] - 1;
                for (var p = up[j]; p < k; p++)
                    x[ui[p]] -= ux[p]*x[j];
            }
        }

        internal static SymbolicFactorization SymbolicAnalysisLU(CompressedColumnStorage A)
        {
            var n = A.ncols;
            // Ordering and symbolic analysis
            return new SymbolicFactorization
            {
                // Fill-reducing ordering
                q = ApproximateMinimumDegree.Generate(A),
                // Guess nnz(L) and nnz(U)
                unz = 4*A.ColumnPointers[n] + n,
                lnz = 4*A.ColumnPointers[n] + n
            };
        }

        /// <summary>
        ///     [L,U,pinv] = lu(A, [q lnz unz]). lnz and unz can be guess.
        /// </summary>
        internal static void FactorizeLU(CompressedColumnStorage A, SymbolicFactorization S,
            out CompressedColumnStorage L,
            out CompressedColumnStorage U, out int[] pinv)
        {
            var q = S.q;
            var n = A.ncols;
            var x = new double[n];
            int i;
            var lnz = S.lnz;
            var unz = S.unz;

            L = new CompressedColumnStorage(n, n, lnz);
            U = new CompressedColumnStorage(n, n, unz);
            pinv = new int[n];

            // Workspace
            var xi = new int[2*n];

            for (i = 0; i < n; i++)
            {
                // No rows pivotal yet.
                pinv[i] = -1;
            }

            lnz = unz = 0;

            int ipiv, top, p, col;
            double pivot;
            double a, t;

            int[] li, ui;
            var lp = L.ColumnPointers;
            var up = U.ColumnPointers;
            double[] lx, ux;

            // Now compute L(:,k) and U(:,k)
            for (var k = 0; k < n; k++)
            {
                // Triangular solve
                lp[k] = lnz; // L(:,k) starts here
                up[k] = unz; // U(:,k) starts here

                if (lnz + n > L.Values.Length) L.Resize(2*L.Values.Length + n);
                if (unz + n > U.Values.Length) U.Resize(2*U.Values.Length + n);

                li = L.RowIndices;
                ui = U.RowIndices;
                lx = L.Values;
                ux = U.Values;
                col = q != null ? q[k] : k;
                top = SolveSp(L, A, col, xi, x, pinv, true); // x = L\A(:,col)

                // Find pivot
                ipiv = -1;
                a = -1;
                for (p = top; p < n; p++)
                {
                    i = xi[p]; // x(i) is nonzero
                    if (pinv[i] < 0) // Row i is not yet pivotal
                    {
                        if ((t = Math.Abs(x[i])) > a)
                        {
                            a = t; // Largest pivot candidate so far
                            ipiv = i;
                        }
                    }
                    else // x(i) is the entry U(pinv[i],k)
                    {
                        ui[unz] = pinv[i];
                        ux[unz++] = x[i];
                    }
                }

                if (ipiv == -1 || a <= 0.0)
                {
                    throw new Exception("No pivot element found.");
                }

                if (pinv[col] < 0 && Math.Abs(x[col]) >= a*tol)
                {
                    ipiv = col;
                }

                // Divide by pivot
                pivot = x[ipiv]; // the chosen pivot
                ui[unz] = k; // last entry in U(:,k) is U(k,k)
                ux[unz++] = pivot;
                pinv[ipiv] = k; // ipiv is the kth pivot row
                li[lnz] = ipiv; // first entry in L(:,k) is L(k,k) = 1
                lx[lnz++] = 1.0;
                for (p = top; p < n; p++) // L(k+1:n,k) = x / pivot
                {
                    i = xi[p];
                    if (pinv[i] < 0) // x(i) is an entry in L(:,k)
                    {
                        li[lnz] = i; // save unpermuted row in L
                        lx[lnz++] = x[i]/pivot; // scale pivot column
                    }
                    x[i] = 0.0; // x [0..n-1] = 0 for next k
                }
            }

            // Finalize L and U
            lp[n] = lnz;
            up[n] = unz;
            li = L.RowIndices; // fix row indices of L for final pinv
            for (p = 0; p < lnz; p++)
            {
                li[p] = pinv[li[p]];
            }

            // Remove extra space from L and U
            L.Resize(0);
            U.Resize(0);
        }


        /// <summary>
        ///     Solve Gx=b(:,k), where G is either upper (lo=false) or lower (lo=true)
        ///     triangular.
        /// </summary>
        /// <param name="G">lower or upper triangular matrix in column-compressed form</param>
        /// <param name="B">right hand side, b=B(:,k)</param>
        /// <param name="k">use kth column of B as right hand side</param>
        /// <param name="xi">size 2*n, nonzero pattern of x in xi[top..n-1]</param>
        /// <param name="x">size n, x in x[xi[top..n-1]]</param>
        /// <param name="pinv">mapping of rows to columns of G, ignored if null</param>
        /// <param name="lo">true if lower triangular, false if upper</param>
        /// <returns>top, -1 in error</returns>
        private static int SolveSp(CompressedColumnStorage G, CompressedColumnStorage B,
            int k, int[] xi, double[] x, int[] pinv, bool lo)
        {
            if (xi == null || x == null) return -1;

            var gp = G.ColumnPointers;
            var gi = G.RowIndices;
            var gx = G.Values;

            var bp = B.ColumnPointers;
            var bi = B.RowIndices;
            var bx = B.Values;

            var n = G.ncols;

            // xi[top..n-1]=Reach(B(:,k))
            var top = ApproximateMinimumDegree.Reach(gp, gi, bp, bi, n, k, xi, pinv);

            int j, J, p, q, px;

            for (p = top; p < n; p++)
            {
                x[xi[p]] = 0; // clear x
            }

            for (p = bp[k]; p < bp[k + 1]; p++)
            {
                x[bi[p]] = bx[p]; // scatter B
            }

            for (px = top; px < n; px++)
            {
                j = xi[px]; // x(j) is nonzero
                J = pinv != null ? pinv[j] : j; // j maps to col J of G
                if (J < 0) continue; // column J is empty
                x[j] /= gx[lo ? gp[J] : gp[J + 1] - 1]; // x(j) /= G(j,j)
                p = lo ? gp[J] + 1 : gp[J]; // lo: L(j,j) 1st entry
                q = lo ? gp[J + 1] : gp[J + 1] - 1; // up: U(j,j) last entry
                for (; p < q; p++)
                {
                    x[gi[p]] -= gx[p]*x[j]; // x(i) -= G(i,j) * x(j)
                }
            }

            // Return top of stack.
            return top;
        }

        #endregion

        #region solve LDL via Cholesky

        internal static SymbolicFactorization SymbolicAnalysisLDL(CompressedColumnStorage A)
        {
            var n = A.ncols;

            var sym = new SymbolicFactorization();

            var ap = A.ColumnPointers;
            var ai = A.RowIndices;

            // P = amd(A+A') or natural
            var P = ApproximateMinimumDegree.Generate(A);
            var Pinv = Invert(P);

            // Output: column pointers and elimination tree.
            var lp = new int[n + 1];
            var parent = new int[n];

            // Workspace
            var lnz = new int[n];
            var flag = new int[n];

            int i, k, p, kk, p2;

            for (k = 0; k < n; k++)
            {
                // L(k,:) pattern: all nodes reachable in etree from nz in A(0:k-1,k) 
                parent[k] = -1; // parent of k is not yet known 
                flag[k] = k; // mark node k as visited 
                lnz[k] = 0; // count of nonzeros in column k of L 
                kk = P != null ? P[k] : k; // kth original, or permuted, column 
                p2 = ap[kk + 1];
                for (p = ap[kk]; p < p2; p++)
                {
                    // A(i,k) is nonzero (original or permuted A) 
                    i = Pinv != null ? Pinv[ai[p]] : ai[p];
                    if (i < k)
                    {
                        // follow path from i to root of etree, stop at flagged node 
                        for (; flag[i] != k; i = parent[i])
                        {
                            // find parent of i if not yet determined 
                            if (parent[i] == -1) parent[i] = k;
                            lnz[i]++; // L(k,i) is nonzero 
                            flag[i] = k; // mark i as visited 
                        }
                    }
                }
            }

            // construct Lp index array from Lnz column counts 
            lp[0] = 0;
            for (k = 0; k < n; k++)
            {
                lp[k + 1] = lp[k] + lnz[k];
            }

            sym.parent = parent;
            sym.cp = lp;
            sym.q = P;
            sym.pinv = Pinv;
            return sym;
        }

        /// <summary>
        ///     Compute the numeric LDL' factorization of PAP'.
        /// </summary>
        internal static void FactorizeLDL(CompressedColumnStorage A, SymbolicFactorization S, out double[] D,
            out CompressedColumnStorage L)
        {
            var n = A.ncols;

            var ap = A.ColumnPointers;
            var ai = A.RowIndices;
            var ax = A.Values;

            var parent = S.parent;
            var P = S.q;
            var Pinv = S.pinv;

            D = new double[n];
            L = new CompressedColumnStorage(n, n, S.cp[n]);

            Array.Copy(S.cp, L.ColumnPointers, n + 1);

            var lp = L.ColumnPointers;
            var li = L.RowIndices;
            var lx = L.Values;

            // Workspace
            var y = new double[n];
            var pattern = new int[n];
            var flag = new int[n];
            var lnz = new int[n];

            double yi, l_ki;
            int i, k, p, kk, p2, len, top;

            for (k = 0; k < n; k++)
            {
                // compute nonzero Pattern of kth row of L, in topological order
                y[k] = 0.0; // Y(0:k) is now all zero
                top = n; // stack for pattern is empty
                flag[k] = k; // mark node k as visited
                lnz[k] = 0; // count of nonzeros in column k of L
                kk = P != null ? P[k] : k; // kth original, or permuted, column
                p2 = ap[kk + 1];
                for (p = ap[kk]; p < p2; p++)
                {
                    i = Pinv != null ? Pinv[ai[p]] : ai[p]; // get A(i,k)
                    if (i <= k)
                    {
                        y[i] += ax[p]; // scatter A(i,k) into Y (sum duplicates)
                        for (len = 0; flag[i] != k; i = parent[i])
                        {
                            pattern[len++] = i; // L(k,i) is nonzero
                            flag[i] = k; // mark i as visited
                        }
                        while (len > 0)
                        {
                            pattern[--top] = pattern[--len];
                        }
                    }
                }

                // compute numerical values kth row of L (a sparse triangular solve)
                D[k] = y[k]; // get D(k,k) and clear Y(k)
                y[k] = 0.0;
                for (; top < n; top++)
                {
                    i = pattern[top]; // Pattern [top:n-1] is pattern of L(:,k)
                    yi = y[i]; // get and clear Y(i)
                    y[i] = 0.0;
                    p2 = lp[i] + lnz[i];
                    for (p = lp[i]; p < p2; p++)
                    {
                        y[li[p]] -= lx[p]*yi;
                    }
                    l_ki = yi/D[i]; // the nonzero entry L(k,i)
                    D[k] -= l_ki*yi;
                    li[p] = k; // store L(k,i) in column form of L
                    lx[p] = l_ki;
                    lnz[i]++; // increment count of nonzeros in col i
                }

                if (D[k] == 0.0)
                {
                    // failure, D(k,k) is zero
                    throw new Exception("Diagonal element is zero.");
                }
            }
        }

        /// <summary>
        ///     Solves a linear system Ax=b, where A is symmetric positive definite.
        /// </summary>
        /// <param name="input">Right hand side b.</param>
        /// <param name="result">Solution vector x.</param>
        internal static double[] SolveLDL(IList<double> input, CompressedColumnStorage L, double[] D,
            SymbolicFactorization S)
        {
            if (input == null) throw new ArgumentNullException("input");

            var n = L.ncols;
            var result = new double[n];

            var x = ApplyInverse(S.pinv, input, n); // x = P*b

            var lx = L.Values;
            var lp = L.ColumnPointers;
            var li = L.RowIndices;
            int end;

            // Solve lower triangular system by forward elimination, x = L\x.
            for (var i = 0; i < n; i++)
            {
                end = lp[i + 1];
                for (var p = lp[i]; p < end; p++)
                {
                    x[li[p]] -= lx[p]*x[i];
                }
            }

            // Solve diagonal system, x = D\x.
            for (var i = 0; i < n; i++)
            {
                x[i] /= D[i];
            }

            // Solve upper triangular system by backward elimination, x = L'\x.
            for (var i = n - 1; i >= 0; i--)
            {
                end = lp[i + 1];
                for (var p = lp[i]; p < end; p++)
                {
                    x[i] -= lx[p]*x[li[p]];
                }
            }

            return Apply(S.pinv, x, n); // b = P'*x
        }

        #endregion

        #region Permutation

        /// <summary>
        ///     Permutes a vector, x=P*b.
        /// </summary>
        /// <param name="p">Permutation vector.</param>
        /// <param name="b">Input vector.</param>
        /// <param name="x">Output vector, x=P*b.</param>
        /// <param name="n">Length of p, b and x.</param>
        /// <remarks>
        ///     p = null denotes identity.
        /// </remarks>
        internal static double[] Apply(int[] p, IList<double> b, int n)
        {
            var x = new double[n];
            for (var k = 0; k < n; k++)

                x[k] = b[p[k]];
            return x;
        }

        /// <summary>
        ///     Permutes a vector, x = P'b.
        /// </summary>
        /// <param name="p">Permutation vector.</param>
        /// <param name="b">Input vector.</param>
        /// <param name="x">Output vector, x = P'b.</param>
        /// <param name="n">Length of p, b, and x.</param>
        /// <remarks>
        ///     p = null denotes identity.
        /// </remarks>
        internal static double[] ApplyInverse(int[] p, IList<double> b, int n)
        {
            var x = new double[n];
            for (var k = 0; k < n; k++)
                x[p[k]] = b[k];
            return x;
        }

        /// <summary>
        ///     Inverts a permutation vector.
        /// </summary>
        /// <param name="p">A permutation vector.</param>
        /// <returns>Returns pinv[i] = k if p[k] = i on input.</returns>
        internal static int[] Invert(int[] p)
        {
            int k, n = p.Length;

            var pinv = new int[n];

            for (k = 0; k < n; k++)
            {
                // Invert the permutation.
                pinv[p[k]] = k;
            }
            return pinv;
        }

        #endregion
    }
}