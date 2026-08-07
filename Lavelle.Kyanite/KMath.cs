using System;
using System.Collections.Generic;
using System.Text;

namespace Lavelle.Kyanite
{
    /// <summary>
    /// Math utilities for Kyanite.
    /// </summary>
    public static class KMath
    {
        /// <summary>
        /// Creates a new `Number` expression.
        /// </summary>
        public static Number N(double x) => new Number(x);
        /// <summary>
        /// Creates a new `Variable` expression.
        /// </summary>
        public static Variable V(string x) => new Variable(x);
        /// <summary>
        /// Creates a new constant `Variable` expression.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static Variable C(string x) => new Variable(x, true);
        /// <summary>
        /// Creates a new symbolic `Derivative` expression.
        /// </summary>
        public static Derivative D(Variable f, Variable x) => new Derivative(f, x);

        /// <summary>
        /// Creates a new `Sin` expression.
        /// </summary>
        public static Sin Sin(KyaniteExpression expression) => new Sin(expression);
        /// <summary>
        /// Creates a new `Cos` expression.
        /// </summary>
        public static Cos Cos(KyaniteExpression expression) => new Cos(expression);
        /// <summary>
        /// Creates a new `Tan` expression.
        /// </summary>
        public static Tan Tan(KyaniteExpression expression) => new Tan(expression);
        /// <summary>
        /// Creates a new `Log` expression.
        /// </summary>
        public static Log Log(KyaniteExpression x, KyaniteExpression b) => new Log(x, b);

        public static Pow Exp(KyaniteExpression x) => C("e").Pow(x);
    }
}
