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
        /// Walks the expression tree downward and applies a function.
        /// </summary>
        public static KyaniteExpression WalkDown(this KyaniteExpression expression, Func<KyaniteExpression, KyaniteExpression> function) => function(expression) switch
        {
            Add(var a, var b) => a.WalkDown(function) + b.WalkDown(function),
            Multiply(var a, var b) => a.WalkDown(function) * b.WalkDown(function),

            Pow(var x, var e) => x.WalkDown(function).Pow(e.WalkDown(function)),
            Sin(var x) => x.WalkDown(function).Sin(),
            Cos(var x) => x.WalkDown(function).Cos(),
            Tan(var x) => x.WalkDown(function).Tan(),
            Log(var x, var b) => x.WalkDown(function).Log(b.WalkDown(function)),
            Sinh(var x) => x.WalkDown(function).Sinh(),
            Cosh(var x) => x.WalkDown(function).Cosh(),
            Tanh(var x) => x.WalkDown(function).Tanh(),

            Derivative(var f, var x, true) => new Derivative(f.WalkDown(function), x, true),
            Derivative(var f, var x, false) => f.WalkDown(function).D(x),

            Integral(var f, var v) => f.WalkDown(function).Int(v),

            Function(var name, var parameters) => new Function(name, [.. parameters.Select(x => x.WalkDown(function))]),

            var x => x
        };

        /// <summary>
        /// Walks the expression tree upward and applies a function.
        /// </summary>
        public static KyaniteExpression WalkUp(this KyaniteExpression expression, Func<KyaniteExpression, KyaniteExpression> function)
        {
            var ex = expression switch
            {
                Add(var a, var b) => a.WalkUp(function) + b.WalkUp(function),
                Multiply(var a, var b) => a.WalkUp(function) * b.WalkUp(function),

                Pow(var x, var e) => x.WalkUp(function).Pow(e.WalkUp(function)),
                Sin(var x) => x.WalkUp(function).Sin(),
                Cos(var x) => x.WalkUp(function).Cos(),
                Tan(var x) => x.WalkUp(function).Tan(),
                Log(var x, var b) => x.WalkUp(function).Log(b.WalkUp(function)),
                Sinh(var x) => x.WalkUp(function).Sinh(),
                Cosh(var x) => x.WalkUp(function).Cosh(),
                Tanh(var x) => x.WalkUp(function).Tanh(),

                Derivative(var f, var x, true) => new Derivative(f.WalkUp(function), x, true),
                Derivative(var f, var x, false) => f.WalkUp(function).D(x),

                Integral(var f, var v) => f.WalkUp(function).Int(v),

                Function(var name, var parameters) => new Function(name, [.. parameters.Select(x => x.WalkUp(function))]),

                var x => x
            };
            return function(ex);
        }
    }
}
