using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Kyanite
{
    public partial class KMath
    {
        /// <summary>
        /// Solves for <paramref name="x"/> in an equation <paramref name="l"/> = <paramref name="r"/>.
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static (KyaniteExpression L, KyaniteExpression R) Solve(KyaniteExpression l, KyaniteExpression r, KyaniteExpression x) => l switch
        {
            var y when x.SE(y) => (x, r.Simplify()),

            Add(var a, var b) when a.Has(x) => Solve(a, r - b, x),
            Add(var a, var b) when b.Has(x) => Solve(b, r - a, x),
            Multiply(var a, var b) when a.Has(x) => Solve(a, r / b, x),
            Multiply(var a, var b) when b.Has(x) => Solve(b, r / a, x),

            Pow(var y, Number(var e)) when y.Has(x) => Solve(y, r.Pow(1 / e), x),
            Pow(var y, var e) when e.Has(x) => Solve(e, r / l.Log("e"), x),

            Log(var y, var b) when y.Has(x) => Solve(y, b.Pow(r), x),

            _ => (l.Simplify(), r.Simplify())
        };
    }
}
