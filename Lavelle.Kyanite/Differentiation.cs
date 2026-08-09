using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lavelle.Kyanite
{
    /// <summary>
    /// Extensions for `KyaniteExpression`.
    /// </summary>
    public static partial class KyaniteExtensions
    {
        /// <summary>
        /// Takes the derivative of <paramref name="expression"/> w.r.t <paramref name="x"/>.
        /// Takes the partial derivative if <paramref name="partial"/> is true.
        /// </summary>
        public static KyaniteExpression D(this KyaniteExpression expression, Variable x, bool partial = false)
        {
            KyaniteExpression d = expression switch
            {
                Number _ => new Number(0),
                Variable v when v == x => new Number(1),
                Variable v => partial || v.Constant ? 0 : new Derivative(v, x),

                Add(var a, var b) => a.D(x, partial) + b.D(x, partial),
                Multiply(var a, var b) => a.D(x, partial) * b + a * b.D(x, partial),

                Pow(var y, Number(var e)) => new Number(e) * y.Pow(e - 1) * y.D(x, partial),
                Pow(var y, var e) => expression * (e.D(x, partial) * y.Log("e") + e * y.D(x, partial) / y),
                Sin(var y) => y.Cos() * y.D(x, partial),
                Cos(var y) => -y.Sin() * y.D(x, partial),
                Tan(var y) => y.Sec().Pow(2) * y.D(x, partial),
                Log(var y, var b) => y.D(x, partial) / (y * b.Log("e")),
                Sinh(var y) => y.Cosh() * y.D(x, partial),
                Cosh(var y) => y.Sinh() * y.D(x, partial),
                Tanh(var y) => y.Sech() * y.D(x, partial),

                var y => new Derivative(y, x)
            };
            return d.Simplify();
        }

        /// <summary>
        /// Takes the partial derivative of <paramref name="expression"/> w.r.t <paramref name="x"/>.
        /// </summary>
        public static KyaniteExpression PD(this KyaniteExpression expression, Variable x) => expression.D(x, true);
    }
}
