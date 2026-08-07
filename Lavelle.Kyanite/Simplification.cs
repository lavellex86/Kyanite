using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lavelle.Kyanite
{
    public partial class KyaniteExtensions
    {
        public static KyaniteExpression Simplify(this KyaniteExpression expression)
        {
            var simplified1 = expression switch
            {
                Add(var a, var b) => a.Simplify() + b.Simplify(),
                Multiply(var a, var b) => a.Simplify() * b.Simplify(),

                Power(var x, var e) => x.Simplify().Power(e.Simplify()),
                Sin(var x) => x.Simplify().Sin(),
                Cos(var x) => x.Simplify().Cos(),
                Tan(var x) => x.Simplify().Tan(),
                Log(var x, var b) => x.Simplify().Log(b.Simplify()),

                var x => x
            };

            var simplified2 = simplified1 switch
            {
                Add(Number(0), var x) => x,
                Add(var x, Number(0)) => x,
                Multiply(Number(1), var x) => x,
                Multiply(var x, Number(1)) => x,
                Multiply(Number(0), var _) => 0,
                Multiply(var _, Number(0)) => 0,

                Multiply(Number(-1), Number(var x)) => -x,
                Multiply(Number(var x), Number(-1)) => -x,

                Power(var x, Number(0)) => 1,
                Power(Number(0), var x) => 0,
                Power(var x, Number(1)) => x,

                Add(Number(var a), Number(var b)) => a + b,
                Multiply(Number(var a), Number(var b)) => a + b,
                Power(Number(var a), Number(var b)) => Math.Pow(a, b),

                Sin(Number(0)) => 0,
                Cos(Number(0)) => 1,
                Log(var _, Number(1)) => 1,

                Multiply(Multiply(var x, Number(-1)), Number(-1)) => x,

                var x => x
            };

            // TODO: term collection, polynomials
            // TODO: trig

            return simplified2;
        }

    }
}
