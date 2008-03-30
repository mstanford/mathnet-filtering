#region Math.NET Iridium (LGPL) by Ruegg, Shugai
// Math.NET Iridium, part of the Math.NET Project
// http://mathnet.opensourcedotnet.info
//
// Copyright (c) 2004-2008, Christoph R�egg, http://christoph.ruegg.name
//                          Mike Shugai
// 
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published 
// by the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public 
// License along with this program; if not, write to the Free Software
// Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
#endregion

using System;
using System.Text;
using MathNet.Numerics.Properties;
using MathNet.Numerics.Distributions;

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    /// Real Vector
    /// </summary>
    public interface IVector
    {
        /// <summary>Gets the dimensionality of rows.</summary>
        int Length { get; }

        /// <summary>Gets or set the element indexed by <c>i</c>
        /// in the <c>Vector</c>.</summary>
        /// <param name="i">Dimension index.</param>
        double this[int i] { get; set; }
    }

    /// <summary>Real vector.</summary>
    /// <remarks>
    /// The class <c>Vector</c> provides the elementary 
    /// algebraic and conversion operations.
    /// </remarks>
    [Serializable]
    public class Vector :
        IVector,
        ICloneable
    {
        private int _length;

        /// <summary>
        /// Gets dimensionality of the vector.
        /// </summary>
        public int Length
        {
            get { return _length; }
        }

        /// <summary>
        /// Array for internal storage of elements.
        /// </summary>
        private double[] _data;

        /// <summary>
        /// Gets or sets the element indexed by <c>i</c>
        /// in the <c>Vector</c>.
        /// </summary>
        /// <param name="i">Dimension index.</param>
        public double this[int i]
        {
            get { return _data[i]; }
            set { _data[i] = value; }
        }

        #region Constructors and static constructive methods

        /// <summary>
        /// Constructs an n-dimensional vector of zeros.
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        public
        Vector(
            int n
            )
        {
            _length = n;
            _data = new double[_length];
        }

        /// <summary>
        /// Constructs an n-dimensional unit vector for i'th coordinate.
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="i">Coordinate index.</param>
        public
        Vector(
            int n,
            int i
            )
        {
            _length = n;
            _data = new double[_length];
            _data[i] = 1.0;
        }

        /// <summary>
        /// Constructs an n-dimensional constant vector.
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="value">Fill the vector with this scalar value.</param>
        public
        Vector(
            int n,
            double value
            )
        {
            _length = n;
            _data = new double[_length];
            for(int i = 0; i < _length; i++)
            {
                _data[i] = value;
            }
        }

        /// <summary>
        /// Constructs a vector from a 1-D array, directly using
        /// the provided array as internal data structure.
        /// </summary>
        /// <param name="components">One-dimensional array of doubles.</param>
        /// <seealso cref="Create"/>
        public
        Vector(
            double[] components
            )
        {
            _length = components.Length;
            _data = components;
        }

        /// <summary>
        /// Constructs a vector from a copy of a 1-D array.
        /// </summary>
        /// <param name="components">One-dimensional array of doubles.</param>
        public static
        Vector
        Create(
            double[] components
            )
        {
            return (new Vector(components)).Clone();
        }

        /// <summary>
        /// Generates vector with random elements
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="randomDistribution">Continuous Random Distribution or Source</param>
        /// <returns>
        /// An n-dimensional vector with random elements distributed according
        /// to the specified random distribution.
        /// </returns>
        public static
        Vector
        Random(
            int n,
            IContinuousGenerator randomDistribution
            )
        {
            double[] data = new double[n];
            for(int i = 0; i < data.Length; i++)
            {
                data[i] = randomDistribution.NextDouble();
            }
            return new Vector(data);
        }

        /// <summary>
        /// Generates vector with random elements
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <returns>
        /// An n-dimensional vector with uniformly distributed
        /// random elements in <c>[0, 1)</c> interval.
        /// </returns>
        public static
        Vector
        Random(
            int n
            )
        {
            return Random(n, new RandomSources.SystemRandomSource());
        }

        /// <summary>
        /// Generates an n-dimensional vector filled with 1.
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        public static
        Vector
        Ones(
            int n
            )
        {
            return new Vector(n, 1.0);
        }

        /// <summary>
        /// Generates an n-dimensional vector filled with 0.
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        public static
        Vector
        Zeros(
            int n
            )
        {
            return new Vector(n);
        }

        /// <summary>
        /// Generates an n-dimensional unit vector for i'th coordinate.
        /// </summary>
        /// <param name="n">Dimensionality of vector.</param>
        /// <param name="i">Coordinate index.</param>
        public static
        Vector
        BasisVector(
            int n,
            int i
            )
        {
            return new Vector(n, i);
        }

        #endregion

        #region Conversion Operators and conversion to other types

        /// <summary>
        /// Returns a reference to the internel data structure.
        /// </summary>
        public static implicit
        operator double[](
            Vector v
            )
        {
            return v._data;
        }

        /// <summary>
        /// Returns a vector bound directly to a reference of the provided array.
        /// </summary>
        public static implicit
        operator Vector(
            double[] v
            )
        {
            return new Vector(v);
        }

        /// <summary>
        /// Create a matrix based on this vector in column form (one single column).
        /// </summary>
        public
        Matrix
        ToColumnMatrix()
        {
            double[][] m = Matrix.CreateMatrixData(_length, 1);
            for(int i = 0; i < m.Length; i++)
            {
                m[i][0] = _data[i];
            }
            return new Matrix(m);
        }

        /// <summary>
        /// Create a matrix based on this vector in row form (one single row).
        /// </summary>
        public
        Matrix
        ToRowMatrix()
        {
            double[][] m = Matrix.CreateMatrixData(1, _length);
            for(int i = 0; i < m.Length; i++)
            {
                m[0][i] = _data[i];
            }
            return new Matrix(m);
        }

        #endregion

        #region Elementary operations

        // TODO (cdr, 2008-03-30): Consider to provide static variants
        // like "Sum" for addition, for functional use (as deleates).

        /// <summary>
        /// Add another vector to this vector.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>
        /// Vector ret[i] = this[i] + b[i]
        /// </returns>
        /// <remarks>
        /// This method has the same effect as the overloaded + operator.
        /// </remarks>
        /// <seealso cref="AddInplace"/>
        /// <seealso cref="operator + (Vector, Vector)"/>
        public
        Vector
        Add(
            IVector b
            )
        {
            CheckMatchingVectorDimensions(this, b);

            double[] v = new double[_length];
            for(int i = 0; i < v.Length; i++)
            {
                v[i] = _data[i] + b[i];
            }
            return new Vector(v);
        }

        /// <summary>
        /// In place addition of <c>b</c> to this <c>Vector</c>.
        /// </summary>
        /// <remarks>
        /// This method changes this vector.
        /// </remarks>
        /// <seealso cref="Add"/>
        /// <seealso cref="operator + (Vector, Vector)"/>
        public
        void
        AddInplace(
            IVector b
            )
        {
            CheckMatchingVectorDimensions(this, b);

            for(int i = 0; i < _length; i++)
            {
                _data[i] += b[i];
            }
        }

        /// <summary>
        /// Subtract another vector from this vector.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>
        /// Vector ret[i] = this[i] - b[i]
        /// </returns>
        /// <remarks>
        /// This method has the same effect as the overloaded - operator.
        /// </remarks>
        /// <seealso cref="SubtractInplace"/>
        /// <seealso cref="operator - (Vector, Vector)"/>
        public
        Vector
        Subtract(
            IVector b
            )
        {
            CheckMatchingVectorDimensions(this, b);

            double[] v = new double[_length];
            for(int i = 0; i < v.Length; i++)
            {
                v[i] = _data[i] - b[i];
            }
            return new Vector(v);
        }


        /// <summary>
        /// In place subtraction of <c>v</c> to this <c>Vector</c>.
        /// </summary>
        /// <remarks>
        /// This method changes this vector.
        /// </remarks>
        /// <seealso cref="Subtract"/>
        /// <seealso cref="operator - (Vector, Vector)"/>
        public
        void
        SubtractInplace(
            IVector b
            )
        {
            CheckMatchingVectorDimensions(this, b);

            for(int i = 0; i < _length; i++)
            {
                _data[i] -= b[i];
            }
        }

        /// <summary>
        /// Negate this vector.
        /// </summary>
        /// <returns>
        /// Vector ret[i] = -this[i]
        /// </returns>
        /// <remarks>
        /// This method has the same effect as the overloaded - operator.
        /// </remarks>
        /// <seealso cref="NegateInplace"/>
        /// <seealso cref="operator - (Vector)"/>
        public
        Vector
        Negate()
        {
            double[] v = new double[_length];
            for(int i = 0; i < v.Length; i++)
            {
                v[i] = -_data[i];
            }
            return new Vector(v);
        }

        /// <summary>
        /// In place unary minus of the <c>Vector</c>.
        /// </summary>
        /// <remarks>
        /// This method changes this vector.
        /// </remarks>
        /// <seealso cref="Negate"/>
        /// <seealso cref="operator - (Vector)"/>
        public
        void
        NegateInplace()
        {
            for(int i = 0; i < _length; i++)
            {
                _data[i] = -_data[i];
            }
        }

        /// <summary>
        /// Scale this vector with a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to scale with</param>
        /// <returns>
        /// Vector ret[i] = this[i] * scalar
        /// </returns>
        /// <remarks>
        /// This method has the same effect as the overloaded * operator.
        /// </remarks>
        /// <seealso cref="ScaleInplace(double)"/>
        /// <seealso cref="operator * (Vector, double)"/>
        public
        Vector
        Scale(
            double scalar
            )
        {
            double[] v = new double[_length];
            for(int i = 0; i < v.Length; i++)
            {
                v[i] = _data[i] * scalar;
            }
            return new Vector(v);
        }

        /// <summary>
        /// Multiplies in place this <c>Vector</c> by a scalar.
        /// </summary>
        /// <remarks>
        /// This method changes this vector.
        /// </remarks>
        /// <seealso cref="Scale(double)"/>
        /// <seealso cref="operator * (Vector, double)"/>
        public
        void
        ScaleInplace(
            double s
            )
        {
            for(int i = 0; i < _length; i++)
            {
                _data[i] *= s;
            }
        }

        #endregion

        #region Vector Products

        /// <summary>
        /// Scalar product of two vectors.
        /// </summary>
        /// <returns>
        /// Scalar ret = sum(u[i] * v[i])
        /// </returns>
        /// <seealso cref="ScalarMultiply"/>
        /// <seealso cref="operator * (Vector, Vector)"/>
        public static
        double
        ScalarProduct(
            IVector u,
            IVector v
            )
        {
            CheckMatchingVectorDimensions(u, v);

            double sum = 0;
            for(int i = 0; i < u.Length; i++)
            {
                sum += u[i] * v[i];
            }
            return sum;
        }

        /// <summary>
        /// Scalar product of this vector with another vector.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>
        /// Scalar ret = sum(this[i] * b[i])
        /// </returns>
        /// <remarks>
        /// This method has the same effect as the overloaded * operator.
        /// </remarks>
        /// <seealso cref="ScalarProduct"/>
        /// <seealso cref="operator * (Vector, Vector)"/>
        public
        double
        ScalarMultiply(
            IVector b
            )
        {
            return ScalarProduct(this, b);
        }

        /// <summary>
        /// Dyadic Product of two vectors.
        /// </summary>
        /// <returns>
        /// Matrix M[i,j] = u[i] * v[j].
        /// </returns>
        /// <seealso cref="TensorMultiply"/>
        public static
        Matrix
        DyadicProduct(
            IVector u,
            IVector v
            )
        {
            double[][] m = Matrix.CreateMatrixData(u.Length, v.Length);
            for(int i = 0; i < u.Length; i++)
            {
                for(int j = 0; j < v.Length; j++)
                {
                    m[i][j] = u[i] * v[j];
                }
            }
            return new Matrix(m);
        }

        /// <summary>
        /// Tensor Product (Dyadic) of this and another vector.
        /// </summary>
        /// <param name="b">The vector to operate on.</param>
        /// <returns>
        /// Matrix M[i,j] = this[i] * v[j].
        /// </returns>
        /// <seealso cref="DyadicProduct"/>
        public
        Matrix
        TensorMultiply(
            IVector b
            )
        {
            return DyadicProduct(this, b);
        }

        /// <summary>
        /// Cross product of two 3-dimensional vectors.
        /// </summary>
        /// <returns>
        /// Vector ret = (u[2]v[3] - u[3]v[2], u[3]v[1] - u[1]v[3], u[1]v[2] - u[2]v[1]).
        /// </returns>
        /// <seealso cref="CrossMultiply"/>
        public static
        Vector
        CrossProduct(
            IVector u,
            IVector v
            )
        {
            CheckMatchingVectorDimensions(u, v);
            if(3 != u.Length)
            {
                throw new ArgumentOutOfRangeException("u", Resources.ArgumentVectorThreeDimensional);
            }

            Vector product = new Vector(new double[] {
                u[2]*v[3] - u[3]*v[2],
                u[3]*v[1] - u[1]*v[3],
                u[1]*v[2] - u[2]*v[1]
                });

            return product;
        }

        /// <summary>
        /// Cross product of this vector with another vector.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>
        /// Vector ret = (this[2]b[3] - this[3]b[2], this[3]b[1] - this[1]b[3], this[1]b[2] - this[2]b[1]).
        /// </returns>
        /// <seealso cref="CrossProduct"/>
        public
        Vector
        CrossMultiply(
            IVector b
            )
        {
            return CrossProduct(this, b);
        }

        /// <summary>
        /// Array (element-by-element) product of two vectors.
        /// </summary>
        /// <returns>
        /// Vector ret[i] = u[i] * v[i]
        /// </returns>
        /// <seealso cref="ArrayMultiply"/>
        /// <seealso cref="ArrayMultiplyInplace"/>
        public static
        Vector
        ArrayProduct(
            IVector a,
            IVector b
            )
        {
            CheckMatchingVectorDimensions(a, b);

            double[] v = new double[a.Length];
            for(int i = 0; i < v.Length; i++)
            {
                v[i] = a[i] * b[i];
            }
            return new Vector(v);
        }

        /// <summary>
        /// Array (element-by-element) product of this vector and another vector.
        /// </summary>
        /// <returns>
        /// Vector ret[i] = this[i] * b[i]
        /// </returns>
        /// <seealso cref="ArrayProduct"/>
        /// <seealso cref="ArrayMultiplyInplace"/>
        public
        Vector
        ArrayMultiply(
            IVector b
            )
        {
            return ArrayProduct(this, b);
        }

        /// <summary>
        /// Multiply in place (element-by-element) another vector to this vector.
        /// </summary>
        /// <remarks>
        /// This method changes this vector.
        /// </remarks>
        /// <seealso cref="ArrayProduct"/>
        /// <seealso cref="ArrayMultiply"/>
        public
        void
        ArrayMultiplyInplace(
            IVector b
            )
        {
            CheckMatchingVectorDimensions(this, b);

            for(int i = 0; i < _data.Length; i++)
            {
                _data[i] *= b[i];
            }
        }

        /// <summary>
        /// Array (element-by-element) quotient of two vectors.
        /// </summary>
        /// <returns>
        /// Vector ret[i] = u[i] / v[i]
        /// </returns>
        /// <seealso cref="ArrayDivide"/>
        /// <seealso cref="ArrayDivideInplace"/>
        public static
        Vector
        ArrayQuotient(
            IVector a,
            IVector b
            )
        {
            CheckMatchingVectorDimensions(a, b);

            double[] v = new double[a.Length];
            for(int i = 0; i < v.Length; i++)
            {
                v[i] = a[i] / b[i];
            }
            return new Vector(v);
        }

        /// <summary>
        /// Array (element-by-element) quotient of this vector and another vector.
        /// </summary>
        /// <returns>
        /// Vector ret[i] = this[i] / b[i]
        /// </returns>
        /// <seealso cref="ArrayQuotient"/>
        /// <seealso cref="ArrayDivideInplace"/>
        public
        Vector
        ArrayDivide(
            IVector b
            )
        {
            return ArrayQuotient(this, b);
        }

        /// <summary>
        /// Divide in place (element-by-element) this vector by another vector.
        /// </summary>
        /// <remarks>
        /// This method changes this vector.
        /// </remarks>
        /// <seealso cref="ArrayQuotient"/>
        /// <seealso cref="ArrayDivide"/>
        public
        void
        ArrayDivideInplace(
            IVector b
            )
        {
            CheckMatchingVectorDimensions(this, b);

            for(int i = 0; i < _data.Length; i++)
            {
                _data[i] /= b[i];
            }
        }

        #endregion

        #region Vector Norms

        /// <summary>
        /// Euclidean Norm also known as 2-Norm.
        /// </summary>
        /// <returns>
        /// Scalar ret = sqrt(sum(this[i]^2))
        /// </returns>
        public
        double
        Norm()
        {
            double sum = 0;
            for(int i = 0; i < _data.Length; i++)
            {
                sum = Fn.Hypot(sum, _data[i]);
            }
            return sum;
        }

        /// <summary>
        /// Squared Euclidean 2-Norm.
        /// </summary>
        /// <returns>
        /// Scalar ret = sum(this[i]^2)
        /// </returns>
        public
        double
        SquaredNorm()
        {
            double sum = 0;
            for(int i = 0; i < _data.Length; i++)
            {
                sum = Fn.Hypot(sum, _data[i]);
            }
            return sum * sum;
        }

        /// <summary>
        /// 1-Norm also known as Manhattan Norm or Taxicab Norm.
        /// </summary>
        /// <returns>
        /// Scalar ret = sum(abs(this[i]))
        /// </returns>
        public
        double
        Norm1()
        {
            double sum = 0;
            for(int i = 0; i < _data.Length; i++)
            {
                sum += Math.Abs(_data[i]);
            }
            return sum;
        }

        /// <summary>
        /// p-Norm.
        /// </summary>
        /// <returns>
        /// Scalar ret = (sum(abs(this[i])^p))^(1/p)
        /// </returns>
        public
        double
        NormP(
            int p
            )
        {
            if(1 > p)
            {
                throw new ArgumentOutOfRangeException("p");
            }
            if(1 == p)
            {
                return Norm1();
            }
            if(2 == p)
            {
                return Norm();
            }

            double sum = 0;
            for(int i = 0; i < _data.Length; i++)
            {
                sum += Math.Pow(Math.Abs(_data[i]), p);
            }
            return Math.Pow(sum, 1.0 / p);
        }

        /// <summary>
        /// Infinity-Norm.
        /// </summary>
        /// <returns>
        /// Scalar ret = max(abs(this[i]))
        /// </returns>
        public
        double
        NormInf()
        {
            double max = 0;
            for(int i = 0; i < _data.Length; i++)
            {
                max = Math.Max(max, Math.Abs(_data[i]));
            }
            return max;
        }

        /// <summary>
        /// Normalizes this vector to a unit vector with respect to the Eucliden 2-Norm.
        /// </summary>
        public
        Vector
        Normalize()
        {
            double norm = Norm();
            if(Number.AlmostEqual(0.0, norm))
            {
                return Clone();
            }
            return Scale(1 / norm);
        }

        #endregion

        #region Arithmetic Operator Overloading

        /// <summary>
        /// Addition of vectors
        /// </summary>
        public static
        Vector
        operator +(
            Vector u,
            Vector v
            )
        {
            CheckMatchingVectorDimensions(u, v);
            return u.Add(v);
        }

        /// <summary>
        /// Subtraction of vectors
        /// </summary>
        public static
        Vector
        operator -(
            Vector u,
            Vector v
            )
        {
            CheckMatchingVectorDimensions(u, v);
            return u.Subtract(v);
        }

        /// <summary>
        /// Negate a vectors
        /// </summary>
        public static
        Vector
        operator -(
            Vector v
            )
        {
            return v.Negate();
        }

        /// <summary>
        /// Scaling a vector by a scalar.
        /// </summary>
        public static
        Vector
        operator *(
            double scalar,
            Vector vector
            )
        {
            return vector.Scale(scalar);
        }

        /// <summary>
        /// Scaling a vector by a scalar.
        /// </summary>
        public static
        Vector
        operator *(
            Vector vector,
            double scalar
            )
        {
            return vector.Scale(scalar);
        }

        /// <summary>
        /// Scaling a vector by the inverse of a scalar.
        /// </summary>
        public static
        Vector
        operator /(
            Vector vector,
            double scalar
            )
        {
            return vector.Scale(1 / scalar);
        }

        /// <summary>
        /// Scalar/dot product of two vectors.
        /// </summary>
        public static
        double
        operator *(
            Vector u,
            Vector v
            )
        {
            return ScalarProduct(u, v);
        }

        #endregion

        #region Various Helpers & Infrastructure

        /// <summary>Check if size(A) == size(B) *</summary>
        private static
        void
        CheckMatchingVectorDimensions(
            IVector A,
            IVector B
            )
        {
            if(null == A)
            {
                throw new ArgumentNullException("A");
            }
            if(null == B)
            {
                throw new ArgumentNullException("B");
            }
            if(A.Length != B.Length)
            {
                throw new ArgumentException(Resources.ArgumentVectorsSameLengths);
            }
        }

        /// <summary>Returns a deep copy of this instance.</summary>
        public
        Vector
        Clone()
        {
            return Create(_data);
        }

        /// <summary>
        /// Creates an exact copy of this matrix.
        /// </summary>
        object
        ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Returns true if two vectors are almost equal (with some given relative accuracy).
        /// </summary>
        public static
        bool
        AlmostEqual(
            Vector u,
            Vector v,
            double relativeAccuracy
            )
        {
            return Number.AlmostEqualNorm(u.Norm1(), v.Norm1(), (u - v).Norm1(), relativeAccuracy);
        }
        /// <summary>
        /// Returns true if two vectors are almost equal.
        /// </summary>
        public static
        bool
        AlmostEqual(
            Vector u,
            Vector v
            )
        {
            return Number.AlmostEqualNorm(u.Norm1(), v.Norm1(), (u - v).Norm1(), 10 * Number.DefaultRelativeAccuracy);
        }

        /// <summary>
        /// Formats this vector to a human-readable string
        /// </summary>
        public override
        string
        ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for(int i = 0; i < _data.Length; i++)
            {
                if(i != 0)
                {
                    sb.Append(',');
                }
                sb.Append(_data[i]);
            }
            sb.Append("]");
            return sb.ToString();
        }

        #endregion
    }
}