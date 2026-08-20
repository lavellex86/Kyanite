using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            (Variable(var x, var constx), Variable(var y, var consty)) => x == y && constx == consty,

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

        /// <summary>
        /// Semantic hashes the expression.
        /// </summary>
        public static int Hash(this KyaniteExpression expression)
        {
            var hash = 0;
            expression.WalkUp(ex =>
            {
                hash = HashCode.Combine(ex switch {
                    Function(var name, var parameters) => HashCode.Combine(OpType.Func, name, parameters.Select(p => p.GetHashCode()).Aggregate(0, (acc, h) => acc ^ h)),
                    Variable (var name, _) => HashCode.Combine(OpType.Var, name),
                    Number (var n) => HashCode.Combine(OpType.Num, n),

                    Add (var a, var b) => HashCode.Combine(OpType.Add, a.GetHashCode() ^ b.GetHashCode()),
                    Multiply (var a, var b) => HashCode.Combine(OpType.Mul, a.GetHashCode() ^ b.GetHashCode()),

                    Pow (var x, var e) => HashCode.Combine(OpType.Pow, x, e),
                    Sin (var x) => HashCode.Combine(OpType.Sin, x),
                    Cos (var x) => HashCode.Combine(OpType.Cos, x),
                    Tan (var x) => HashCode.Combine(OpType.Tan, x),
                    Sinh (var x) => HashCode.Combine(OpType.Sinh, x),
                    Cosh (var x) => HashCode.Combine(OpType.Cosh, x),
                    Tanh (var x) => HashCode.Combine(OpType.Tanh, x),
                    Log (var x, var b) => HashCode.Combine(OpType.Log, x, b),

                    Derivative (var f, var x, var p) => HashCode.Combine(OpType.Derivative, f, x, p),
                    Integral (var f, var x) => HashCode.Combine(OpType.Integral, f, x),
                    _ => 0
                }, hash);
                return ex;
            });
            return hash;
        }

        public class SEComparer : IEqualityComparer<KyaniteExpression>
        {
            public bool Equals(KyaniteExpression? x, KyaniteExpression? y) => x?.SE(y) ?? false;
            public int GetHashCode([DisallowNull] KyaniteExpression obj) => obj.Hash();
        }
    }

    internal enum OpType
    {
        Var, Num,
        Add, Mul, Pow,
        Sin, Cos, Tan,
        Sinh, Cosh, Tanh,
        Log,
        Derivative, Integral, Func
    }
}
