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
        /// Checks for semantic equality.
        /// </summary>
        public static bool SE(this KyaniteExpression a, KyaniteExpression b) => (a, b) switch
        {
            (Function f, Function g) => f.Parameters.All(g.Parameters.Contains) && f.Name == g.Name,
            (Number(var x), Number(var y)) => x == y,
            (Variable(var x, _), Variable(var y, _)) => x == y,

            (Add x, Add y) => (SE(x.A, y.A) && SE(x.B, y.B)) || (SE(x.A, y.B) && SE(x.B, y.A)),
            (Multiply x, Multiply y) => (SE(x.A, y.A) && SE(x.B, y.B)) || (SE(x.A, y.B) && SE(x.B, y.A)),

            (Pow x, Pow y) => SE(x.X, y.X) && SE(x.E, y.E),
            (Sin x, Sin y) => SE(x.X, y.X),
            (Cos x, Cos y) => SE(x.X, y.X),
            (Tan x, Tan y) => SE(x.X, y.X),
            (Log x, Log y) => SE(x.X, y.X) && SE(x.B, y.B),
            (Sinh x, Sinh y) => SE(x.X, y.X),
            (Cosh x, Cosh y) => SE(x.X, y.X),
            (Tanh x, Tanh y) => SE(x.X, y.X),

            (Derivative x, Derivative y) => SE(x.F, y.F) && SE(x.X, y.X) && x.Partial == y.Partial,
            (Integral x, Integral y) => SE(x.F, y.F) && SE(x.X, y.X),

            _ => false
        };

        /// <summary>
        /// Checks whether the expression contains <paramref name="x"/>.
        /// </summary>
        public static bool Has(this KyaniteExpression expression, KyaniteExpression x) => expression switch
        {
            var y when x.SE(y) => true,

            Add(var a, var b) => a.Has(x) || b.Has(x),
            Multiply(var a, var b) => a.Has(x) || b.Has(x),

            Pow(var y, var e) => y.Has(x) || e.Has(x),
            Sin(var y) => y.Has(x),
            Cos(var y) => y.Has(x),
            Tan(var y) => y.Has(x),
            Log(var y, var b) => y.Has(x) || b.Has(x),

            Derivative(var f, var y, _) => f.Has(x) || y == x,
            Integral(var f, var y) => f.Has(x) || y == x,

            Function f => f.Parameters.Contains(x),

            _ => false
        };
    }
}
