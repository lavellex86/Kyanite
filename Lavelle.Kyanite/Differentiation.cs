using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lavelle.Kyanite
{
    public static partial class KyaniteExtensions
    {
        /// <summary>
        /// Takes the derivative of <paramref name="expression"/> w.r.t <paramref name="x"/>.
        /// </summary>
        public static KyaniteExpression D(this KyaniteExpression expression, Variable x) => expression switch
        {
            Number _ => new Number(0),
            Variable v when v == x => new Number(1),
            Variable v => new Derivative(v, x),

            Add(var a, var b) => a.D(x) + b.D(x),
            Multiply(var a, var b) => a.D(x) * b + a * b.D(x),
            
            Power(var y, var e) => expression * (e.D(x) * y.Log("e") + e * y.D(x) / y),
            Sin(var y) => y.Cos() * y.D(x),
            Cos(var y) => -y.Sin() * y.D(x),
            Tan(var y) => y.Sec().Power(2) * y.D(x),
            Log(var y, var b) => (y.D(x) * b - y * b.D(x)) / (y * b * b.Log("e")),

            _ => throw new Exception("Expression is not of any Kyanite-supplied type")
        };
    }
}
