using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Kyanite
{
    public partial class KyaniteExtensions
    {
        /// <summary>
        /// Walks the expression tree and applies a function.
        /// </summary>
        public static KyaniteExpression Walk(this KyaniteExpression expression, Func<KyaniteExpression, KyaniteExpression> function) => function(expression) switch
        {
            Add(var a, var b) => a.Walk(function) + b.Walk(function),
            Multiply(var a, var b) => a.Walk(function) * b.Walk(function),

            Pow(var x, var e) => x.Walk(function).Pow(e.Walk(function)),
            Sin(var x) => x.Walk(function).Sin(),
            Cos(var x) => x.Walk(function).Cos(),
            Tan(var x) => x.Walk(function).Tan(),
            Log(var x, var b) => x.Walk(function).Log(b.Walk(function)),
            Sinh(var x) => x.Walk(function).Sinh(),
            Cosh(var x) => x.Walk(function).Cosh(),
            Tanh(var x) => x.Walk(function).Tanh(),

            Derivative(var f, var x, true) => new Derivative(f.Walk(function), x, true),
            Derivative(var f, var x, false) => f.Walk(function).D(x),

            Integral(var f, var v) => f.Walk(function).Int(v),

            Function(var name, var parameters) => new Function(name, [.. parameters.Select(x => x.Walk(function))]),

            var x => x
        };
    }
}
