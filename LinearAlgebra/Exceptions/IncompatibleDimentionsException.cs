using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatSharp.LinearAlgebra.Exceptions
{
    public class IncompatibleDimentionsException<T> : Exception
    {
        public IncompatibleDimentionsException(Matrix<T> m1, Matrix<T> m2)
        {

        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
