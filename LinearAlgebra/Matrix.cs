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

        protected T[,] data;

        public Matrix(int rows, int cols)
        {
            this.RowsCount = rows;
            this.ColumnsCount = cols;

            data = new T[rows, cols];
        }

        public Matrix(int rows, int cols, T[,] data)
            : this(rows, cols)
        {
            this.data = data;
        }

        public Matrix<T> Transpose()
        {
            Matrix<T> result = new Matrix<T>(this.ColumnsCount, this.RowsCount);

            for (int i = 0; i < this.RowsCount; i++)
                for (int j = 0; j < this.ColumnsCount; j++)
                    result.data[j, i] = this.data[i, j];

            return result;
        }

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
        /// Product between two matrixes.
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


        //public static Matrix<T> operator /(T x, Matrix<T> m1) {
        //    return m1;
        //}

        public T[,] GetRawMatrix() { return this.data; }

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
