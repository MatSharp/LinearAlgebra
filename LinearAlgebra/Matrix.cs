using MatSharp.LinearAlgebra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatSharp.LinearAlgebra
{
    public class Matrix<T>
    {
        /// <summary>
        /// Number of rows of the matrix
        /// </summary>
        public int RowsCount { get; protected set; }
        /// <summary>
        /// Number of columns of the matrix
        /// </summary>
        public int ColumnsCount { get; protected set; }
        /// <summary>
        /// True if the matrix is square, false if otherwise.
        /// </summary>
        public bool IsSquare { get; protected set; }

        /// <summary>
        /// This is the array in which the matrix entries are stored.
        /// </summary>
        protected T[,] data;

        /// <summary>
        /// Creates an empty matrix of dimention (rows, cols) filled with zeros.
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        public Matrix(int rows, int cols)
        {
            this.RowsCount = rows;
            this.ColumnsCount = cols;
            this.IsSquare = (rows == cols);

            data = new T[rows, cols];
        }

        /// <summary>
        /// Creates matrix of dimention (rows, cols) with the specified data
        /// </summary>
        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        /// <param name="data">Data of the matrix</param>
        public Matrix(int rows, int cols, T[,] data)
            : this(rows, cols)
        {
            this.data = data;
        }

        /// <summary>
        /// Calculates the transpose of this matrix
        /// </summary>
        /// <returns>The transpose of the matrix</returns>
        public Matrix<T> Transpose()
        {
            Matrix<T> result = new Matrix<T>(this.ColumnsCount, this.RowsCount);

            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                    result.data[j, i] = this.data[i, j];

            return result;
        }

        /// <summary>
        /// Calculates the inverse of a matrix.
        /// </summary>
        /// <returns>Inverse of the input</returns>
        public Matrix<T> Inverse()
        {
            if (!this.IsSquare)
                throw new InvertibleMatrixException<T>(this);

            Matrix<T> working = new Matrix<T>(this.RowsCount, this.ColumnsCount * 2);
            Matrix<T> identity = Matrix<T>.GetIdentity(this.RowsCount);

            /* Copy the matrix to the left size of the new working matrix. */
            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                {
                    working.data[i, j] = this.data[i, j];
                    working.data[i, j + this.ColumnsCount] = identity.data[i, j];
                }

            GaussElimination(working); /* Perform gauss elimination on the new matrix. */

            Matrix<T> result = new Matrix<T>(this.RowsCount, this.ColumnsCount);

            for (int i = 0; i < result.RowsCount; i++)
                for (int j = 0; j < result.ColumnsCount; j++)
                    result.data[i, j] = working.data[i, j + this.ColumnsCount];

            return result;
        }

        public static void GaussElimination(Matrix<T> matrix)
        {
            for (int k = 0; k < matrix.RowsCount; k++)
            {
                T maxVal = (dynamic)0;
                int maxIndex = k;

                /* Find pivot */
                for (int i = k; i < matrix.RowsCount; i++)
                {
                    if (maxVal < (dynamic)matrix.data[i, k])
                    {
                        maxVal = matrix.data[i, k];
                        maxIndex = i;
                    }
                }

                if (matrix.data[maxIndex, k] == (dynamic)0)
                    throw new InvertibleMatrixException<T>(matrix);

                matrix.SwapRows(k, maxIndex);

                /* Process all rows bellow pivot */
                for (int i = k + 1; i < matrix.RowsCount; i++)
                {
                    for (int j = k + 1; j < matrix.ColumnsCount; j++) /* Process all the remaining elements of the row. */
                        matrix.data[i, j] = matrix.data[i, j] - (matrix.data[k, j] * (matrix.data[i, k] / (dynamic)matrix.data[k, k]));

                    matrix.data[i, k] = (dynamic)0;
                }
            }

            /* Back-substitution */

            for (int row = matrix.RowsCount - 1; row >= 0; row--)
            {
                T pivot = matrix.data[row, row];

                /* Divide row by pivot */
                for (int i = 0; i < matrix.ColumnsCount; i++)
                    matrix.data[row, i] /= (dynamic)pivot;

                // For each element in witch its col is between the pivots col + 1 and nrows
                for (int i = row + 1; i < matrix.RowsCount; i++)
                {
                    T n = matrix.data[row, i];

                    for (int j = 0; j < matrix.ColumnsCount; j++) // Multiply row by another:
                        matrix.data[row, j] = matrix.data[row, j] - matrix.data[i, j] * (dynamic)n;
                }
            }
        }

        /// <summary>
        /// Swaps two rows this matrix
        /// </summary>
        /// <param name="rowIndex1">Index of the first row</param>
        /// <param name="rowIndex2">Index of the second row</param>
        public void SwapRows(int rowIndex1, int rowIndex2)
        {
            if (rowIndex1 == rowIndex2)
                return;

            for (int i = 0; i < this.ColumnsCount; i++)
            {
                T temp = this.data[rowIndex1, i];
                this.data[rowIndex1, i] = this.data[rowIndex2, i];
                this.data[rowIndex2, i] = temp;
            }
        }

        /// <summary>
        /// Calculates the norm of the matrix.
        /// </summary>
        /// <returns>Norm of the matrix</returns>
        public T GetNorm()
        {
            T norm = (dynamic)0;

            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                    norm += this.data[i, j] * (dynamic)this.data[i, j];

            return (dynamic)Math.Sqrt((dynamic)norm);
        }

        /// <summary>
        /// Calculates the sum between two matrices.
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="m2">Matrix</param>
        /// <returns>A matrix with the sum between the two matrices.</returns>
        public static Matrix<T> operator +(Matrix<T> m1, Matrix<T> m2)
        {
            if ((m1.RowsCount != m2.RowsCount) || (m1.ColumnsCount != m2.ColumnsCount))
                throw new MatrixDimensionMismatchException<T>(m1, m2);

            Matrix<T> result = new Matrix<T>(m1.RowsCount, m1.ColumnsCount);

            for (int i = 0; i < m1.RowsCount; i++)
                for (int j = 0; j < m1.ColumnsCount; j++)
                    result.data[i, j] = (dynamic)m1.data[i, j] + m2.data[i, j];

            return result;
        }

        /// <summary>
        /// Calculates the subtraction between two matrices.
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="m2">Matrix</param>
        /// <returns>A matrix with the sum between the two matrices.</returns>
        public static Matrix<T> operator -(Matrix<T> m1, Matrix<T> m2)
        {
            if ((m1.RowsCount != m2.RowsCount) || (m1.ColumnsCount != m2.ColumnsCount))
                throw new MatrixDimensionMismatchException<T>(m1, m2);

            Matrix<T> result = new Matrix<T>(m1.RowsCount, m1.ColumnsCount);

            for (int i = 0; i < m1.RowsCount; i++)
                for (int j = 0; j < m1.ColumnsCount; j++)
                    result.data[i, j] = (dynamic)m1.data[i, j] - m2.data[i, j];

            return result;
        }

        /// <summary>
        /// Product between two matrices.
        /// </summary>
        /// <param name="m1">First Matrix</param>
        /// <param name="m2">Second Matrix</param>
        /// <returns></returns>
        public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1.ColumnsCount != m2.RowsCount)
                throw new IncompatibleDimentionsException<T>(m1, m2);

            Matrix<T> result = new Matrix<T>(m1.RowsCount, m2.ColumnsCount);

            for (int i = 0; i < result.RowsCount; i++)
                for (int j = 0; j < result.ColumnsCount; j++)
                {
                    dynamic sum = 0;

                    for (int k = 0; k < m1.ColumnsCount; k++)
                        sum += (dynamic)m1.data[i, k] * m2.data[k, j];

                    result.data[i, j] = sum;
                }

            return result;
        }

        /// <summary>
        /// Multiplies a matrix by a scallar
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="x">Scallar</param>
        /// <returns>The resulting matrix of the multiplication.</returns>
        public static Matrix<T> operator *(Matrix<T> m1, T x)
        {
            Matrix<T> result = new Matrix<T>(m1.RowsCount, m1.ColumnsCount);

            for (int i = 0; i < m1.RowsCount; i++)
                for (int j = 0; j < m1.ColumnsCount; j++)
                    result.data[i, j] = m1.data[i, j] * (dynamic)x;

            return result;
        }
        public static Matrix<T> operator *(T x, Matrix<T> m1) { return m1 * x; }


        /// <summary>
        /// Divides a matrix by a scallar
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="x">Scallar</param>
        /// <returns>The resulting matrix of the division.</returns>
        public static Matrix<T> operator /(Matrix<T> m1, T x)
        {
            Matrix<T> result = new Matrix<T>(m1.RowsCount, m1.ColumnsCount);

            for (int i = 0; i < m1.RowsCount; i++)
                for (int j = 0; j < m1.ColumnsCount; j++)
                    result.data[i, j] = m1.data[i, j] / (dynamic)x;

            return result;
        }
        public static Matrix<T> operator /(T x, Matrix<T> m1) { return m1 / x; }

        /// <summary>
        /// Compares two matrices
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="m2">Matrix</param>
        /// <returns>True if the matrices are equal, false if otherwise</returns>
        public static bool operator ==(Matrix<T> m1, Matrix<T> m2)
        {
            if ((m1.RowsCount != m2.RowsCount) || (m1.ColumnsCount != m2.ColumnsCount))
                return false;

            for (int i = 0; i < m1.RowsCount; i++)
                for (int j = 0; j < m1.ColumnsCount; j++)
                    if (m1.data[i, j] != (dynamic)m2.data[i, j])
                        return false;

            return true;
        }

        /// <summary>
        /// Creates and returns a new identity matrix of the specified dimention nxn,
        /// with n = dimention
        /// </summary>
        /// <param name="dimention">Dimention of the identity matrix</param>
        /// <returns>Identy matrix</returns>
        public static Matrix<T> GetIdentity(int dimention)
        {
            Matrix<T> identity = new Matrix<T>(dimention, dimention);

            for (int i = 0; i < dimention; i++)
                identity.data[i, i] = (dynamic)1;

            return identity;
        }

        /// <summary>
        /// Compares two matrices
        /// </summary>
        /// <param name="m1">Matrix</param>
        /// <param name="m2">matrix</param>
        /// <returns>False if the matrices are equal, true if otherwise</returns>
        public static bool operator !=(Matrix<T> m1, Matrix<T> m2)
        {
            return !(m1 == m2);
        }

        /// <summary>
        /// Gets the raw matrix data array.
        /// </summary>
        /// <returns>Data array</returns>
        public T[,] GetRawMatrix() { return this.data; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string str = "Rows=" + this.RowsCount + ", Cols=" + this.ColumnsCount + "\r\n";

            for (int i = 0; i < this.RowsCount; i++)
            {
                for (int j = 0; j < this.ColumnsCount; j++)
                    str += this.data[i, j] + " ";

                str += "\r\n";
            }

            return str;
        }
    }
}
