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
            (Number(var x), Number(var y)) => x == y,
            (Variable(var x, _), Variable(var y, _)) => x == y,

            (Add x, Add y) => (SE(x.A, y.A) && SE(x.B, y.B)) || (SE(x.A, y.B) && SE(x.B, y.A)),
            (Multiply x, Multiply y) => (SE(x.A, y.A) && SE(x.B, y.B)) || (SE(x.A, y.B) && SE(x.B, y.A)),

            (Power x, Power y) => SE(x.X, y.X) && SE(x.E, y.E),
            (Sin x, Sin y) => SE(x.X, y.X),
            (Cos x, Cos y) => SE(x.X, y.X),
            (Tan x, Tan y) => SE(x.X, y.X),
            (Log x, Log y) => SE(x.X, y.X) && SE(x.B, y.B),

            _ => false
        };
    }
}
