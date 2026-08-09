using System;
using System.Collections.Generic;
using System.Text;

namespace Lavelle.Kyanite
{
    /// <summary>
    /// Math utilities for Kyanite.
    /// </summary>
    public static partial class KMath
    {
        /// <summary>
        /// Creates a new <c>Number</c> expression.
        /// </summary>
        public static Number N(double x) => new Number(x);
        /// <summary>
        /// Creates a new <c>Variable</c> expression.
        /// </summary>
        public static Variable V(string x) => new Variable(x);
        /// <summary>
        /// Creates a new constant <c>Variable</c> expression.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Variable C(string x) => new Variable(x, true);
        /// <summary>
        /// Creates a new symbolic <c>Derivative</c> expression.
        /// </summary>
        public static Derivative D(Variable f, Variable x) => new Derivative(f, x);

        /// <summary>
        /// Creates a new <c>Sin</c> expression.
        /// </summary>
        public static Sin Sin(KyaniteExpression expression) => new Sin(expression);
        /// <summary>
        /// Creates a new <c>Cos</c> expression.
        /// </summary>
        public static Cos Cos(KyaniteExpression expression) => new Cos(expression);
        /// <summary>
        /// Creates a new <c>Tan</c> expression.
        /// </summary>
        public static Tan Tan(KyaniteExpression expression) => new Tan(expression);
        /// <summary>
        /// Creates a new <c>Log</c> expression.
        /// </summary>
        public static Log Log(KyaniteExpression x, KyaniteExpression b) => new Log(x, b);

        /// <summary>
        /// Raises e to the <paramref name="x"/>th power.
        /// </summary>
        public static Pow Exp(KyaniteExpression x) => C("e").Pow(x);

        /// <summary>
        /// Creates a new <c>Sinh</c> expression.
        /// </summary>
        public static Sinh Sinh(KyaniteExpression expression) => new Sinh(expression);
        /// <summary>
        /// Creates a new <c>Cosh</c> expression.
        /// </summary>
        public static Cosh Cosh(KyaniteExpression expression) => new Cosh(expression);
        /// <summary>
        /// Creates a new <c>Tanh</c> expression.
        /// </summary>
        public static Tanh Tanh(KyaniteExpression expression) => new Tanh(expression);
    }
}
