using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Kyanite
{
    public partial class KyaniteExtensions
    {
        /// <summary>
        /// Integrates an expression w.r.t <paramref name="x"/> with constant of integration <paramref name="C"/>.
        /// </summary>
        public static KyaniteExpression Int(this KyaniteExpression expression, Variable x, Variable? C = null)
        {
            KyaniteExpression i = expression switch
            {
                Number(var y) => y * x,
                Variable y when y.Constant && y != x => y * x,

                Add(var a, var b) => a.Int(x) + b.Int(x),
                Multiply => FlattenMultiply(expression) is var factors &&
                    factors.Where(f => f.IsConstant(x)) is var constants &&
                    factors.Where(f => !f.IsConstant(x)) is var nonConstants &&
                    nonConstants.Any()
                    ? RebuildMultiply([.. constants]) * RebuildMultiply([.. nonConstants]).Int(x)
                    : new Integral(expression, x),

                Pow(var y, var e) when y == x && e != -1 => x.Pow(e + 1) / (e + 1),
                Pow(var y, var e) when y == x && e == -1 => x.Ln(),

                Pow(var y, var e) when e == x => y.Pow(x) / y.Ln(),
                Log(var y, var e) when y == x && e == KMath.C("e") => x * x.Ln() - x,

                Sin(var y) when y == x => -x.Cos(),
                Cos(var y) when y == x => x.Sin(),
                Tan(var y) when y == x => -x.Cos().Ln(),
                Sinh(var y) when y == x => x.Cosh(),
                Cosh(var y) when y == x => x.Sinh(),
                Tanh(var y) when y == x => x.Cosh().Ln(),

                Derivative(var f, var y, true) when y == x => f,

                var y => new Integral(y, x)
            };
            return i + (C is null ? 0 : C);
        }

        private static bool IsConstant(this KyaniteExpression expression, KyaniteExpression x) => expression switch
        {

            Function(_, var parameters) => parameters.All(y => y.IsConstant(x)),
            Number(_) => true,
            Variable v => v.Constant || v != x,
            Add(var a, var b) => a.IsConstant(x) && b.IsConstant(x),
            Multiply(var a, var b) => a.IsConstant(x) && b.IsConstant(x),

            Pow(var a, var b) => a.IsConstant(x) && b.IsConstant(x),
            Sin(var a) => a.IsConstant(x),
            Cos(var a) => a.IsConstant(x),
            Tan(var a) => a.IsConstant(x),
            Sinh(var a) => a.IsConstant(x),
            Cosh(var a) => a.IsConstant(x),
            Tanh(var a) => a.IsConstant(x),
            Log(var a, var b) => a.IsConstant(x) && b.IsConstant(x),

            Integral(var f, var v) => v != x && f.IsConstant(x),
            Derivative(var f, var v, _) => v != x && f.IsConstant(x),

            _ => false
        };
    }
}
