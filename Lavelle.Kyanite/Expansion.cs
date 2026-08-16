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
        /// Expands an expression.
        /// </summary>
        public static KyaniteExpression Expand(this KyaniteExpression expression)
        {
            KyaniteExpression prev;
            do
            {
                prev = expression;
                expression = expression.ExpandOnce();
            } while (!expression.SE(prev));
            return expression;
        }

        private static KyaniteExpression ExpandOnce(this KyaniteExpression expression)
        {
            var expanded = expression switch
            {
                Add(var a, var b) => a.ExpandOnce() + b.ExpandOnce(),
                Multiply(var a, var b) => a.ExpandOnce() * b.ExpandOnce(),

                Pow(var x, var e) => x.ExpandOnce().Pow(e.ExpandOnce()),
                Sin(var x) => x.ExpandOnce().Sin(),
                Cos(var x) => x.ExpandOnce().Cos(),
                Tan(var x) => x.ExpandOnce().Tan(),
                Log(var x, var b) => x.ExpandOnce().Log(b.ExpandOnce()),
                Sinh(var x) => x.ExpandOnce().Sinh(),
                Cosh(var x) => x.ExpandOnce().Cosh(),
                Tanh(var x) => x.ExpandOnce().Tanh(),

                Derivative(var f, var x, true) => new Derivative(f.ExpandOnce(), x, true),
                Derivative(var f, var x, false) => f.ExpandOnce().D(x),
                Integral(var f, var x) => f.ExpandOnce().Int(x),

                var x => x
            };

            expanded = expanded switch
            {
                Sin(Add(var a, var b)) => (a.Sin() * b.Cos()) + (a.Cos() * b.Sin()),
                Cos(Add(var a, var b)) => (a.Cos() * b.Cos()) + ((-1) * (a.Sin() * b.Sin())),

                Sin(Multiply(Number(2), var x)) => 2 * (x.Sin() * x.Cos()),
                Cos(Multiply(Number(2), var x)) => x.Cos().Sq() + ((-1) * x.Sin().Sq()),

                Pow(Add(var a, var b), Number(var n)) when n == Math.Floor(n) && n > 1 => Enumerable.Repeat<KyaniteExpression>(a + b, (int)n).Aggregate((x, y) => x * y),

                Multiply(var a, Add(var b, var c)) => (a * b) + (a * c),
                Multiply(Add(var a, var b), var c) => (a * c) + (b * c),

                Log(Multiply(var a, var b), var bas) => a.Log(bas) + b.Log(bas),
                Log(Pow(var x, var n), var bas) => n * x.Log(bas),

                Sinh(Add(var a, var b)) => (a.Sinh() * b.Cosh()) + (a.Cosh() * b.Sinh()),
                Cosh(Add(var a, var b)) => (a.Cosh() * b.Cosh()) + (a.Sinh() * b.Sinh()),

                Sinh(Multiply(Number(2), var x)) => 2 * (x.Sinh() * x.Cosh()),
                Cosh(Multiply(Number(2), var x)) => x.Cosh().Sq() + x.Sinh().Sq(),

                var x => x
            };

            return expanded;
        }
    }
}
