using MatSharp.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatSharp.LinearAlgebra.Exceptions
{
    public class InvertibleMatrixException<T> : Exception
    {
        public Matrix<T> M { get; protected set; }

        public InvertibleMatrixException(Matrix<T> matrix) { this.M = matrix; }

        public override string ToString()
        {
            return "The matrix is not invertible";
        }
    }
}
