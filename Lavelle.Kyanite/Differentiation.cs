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
        /// Takes the derivative  w.r.t <paramref name="x"/>.
        /// </summary>
        public static KyaniteExpression D(this KyaniteExpression expression, Variable x) => expression.Simplify() switch
        {
            Number _ => new Number(0),
            Variable v when v == x => new Number(1),
            Variable v => v.Constant ? 0 : new Derivative(v, x, false),

            Add(var a, var b) => a.D(x) + b.D(x),
            Multiply(var a, var b) => a.D(x) * b + a * b.D(x),

            Pow(var y, Number(var e)) => new Number(e) * y.Pow(e - 1) * y.D(x),
            Pow(var y, var e) => expression * (e.D(x) * y.Log("e") + e * y.D(x) / y),
            Sin(var y) => y.Cos() * y.D(x),
            Cos(var y) => -y.Sin() * y.D(x),
            Tan(var y) => y.Sec().Pow(2) * y.D(x),
            Log(var y, var b) => y.D(x) / (y * b.Log("e")),
            Sinh(var y) => y.Cosh() * y.D(x),
            Cosh(var y) => y.Sinh() * y.D(x),
            Tanh(var y) => y.Sech() * y.D(x),

            Integral(var f, var y) when y == x => f,

            var y => new Derivative(y, x, false)
        };

        /// <summary>
        /// Takes the partial derivative w.r.t <paramref name="x"/>.
        /// Partial derivatives of variables within <paramref name="allowed"/> will be kept and marked as partial derivatives.
        /// </summary>
        public static KyaniteExpression PD(this KyaniteExpression expression, Variable x, List<Variable>? allowed = null) => expression.Simplify() switch
        {
            Number _ => new Number(0),
            Variable v when v == x => new Number(1),
            Variable v => !allowed?.Contains(v) ?? true ? 0 : new Derivative(v, x, true),

            Add(var a, var b) => a.PD(x, allowed) + b.PD(x, allowed),
            Multiply(var a, var b) => a.PD(x, allowed) * b + a * b.PD(x, allowed),

            Pow(var y, Number(var e)) => new Number(e) * y.Pow(e - 1) * y.PD(x, allowed),
            Pow(var y, var e) => expression * (e.PD(x, allowed) * y.Log("e") + e * y.PD(x, allowed) / y),
            Sin(var y) => y.Cos() * y.PD(x, allowed),
            Cos(var y) => -y.Sin() * y.PD(x, allowed),
            Tan(var y) => y.Sec().Pow(2) * y.PD(x, allowed),
            Log(var y, var b) => y.PD(x, allowed) / (y * b.Log("e")),
            Sinh(var y) => y.Cosh() * y.PD(x, allowed),
            Cosh(var y) => y.Sinh() * y.PD(x, allowed),
            Tanh(var y) => y.Sech() * y.PD(x, allowed),

            var y => new Derivative(y, x, true)
        };
    }
}
