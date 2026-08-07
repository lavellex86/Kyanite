using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lavelle.Kyanite
{
    public partial class KyaniteExtensions
    {
        /// <summary>
        /// Simplifies an expression.
        /// </summary>
        public static KyaniteExpression Simplify(this KyaniteExpression expression)
        {
            KyaniteExpression prev;
            do
            {
                prev = expression;
                expression = expression.SimplifyOnce();
            } while (!expression.SE(prev));
            return expression;
        }

        private static KyaniteExpression SimplifyOnce(this KyaniteExpression expression)
        {
            var simplified = expression switch
            {
                Add(var a, var b) => a.SimplifyOnce() + b.SimplifyOnce(),
                Multiply(var a, var b) => a.SimplifyOnce() * b.SimplifyOnce(),

                Power(var x, var e) => x.SimplifyOnce().Power(e.SimplifyOnce()),
                Sin(var x) => x.SimplifyOnce().Sin(),
                Cos(var x) => x.SimplifyOnce().Cos(),
                Tan(var x) => x.SimplifyOnce().Tan(),
                Log(var x, var b) => x.SimplifyOnce().Log(b.SimplifyOnce()),

                var x => x
            };

            simplified = simplified switch
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
                Multiply(Number(var a), Number(var b)) => a * b,
                Power(Number(var a), Number(var b)) => Math.Pow(a, b),

                Sin(Number(0)) => 0,
                Cos(Number(0)) => 1,
                Log(var _, Number(1)) => 1,

                Multiply(Multiply(var x, Number(-1)), Number(-1)) => x,

                var x => x
            };

            simplified = CollectAdd(simplified);
            simplified = CollectMultiply(simplified);

            // TODO: polynomials
            // TODO: trig

            return simplified;
        }

        private static KyaniteExpression CollectAdd(this KyaniteExpression expression)
        {
            if (expression is not Add) return expression;
            var flattened = FlattenAdd(expression);
            var coeffs = new Dictionary<KyaniteExpression, KyaniteExpression>();
            var order = new List<KyaniteExpression>();

            foreach (var term in flattened)
            {
                var (coeff, trueExpression) = Decompose(term);
                if (coeffs.ContainsKey(trueExpression)) coeffs[trueExpression] += coeff;
                else { coeffs[trueExpression] = coeff; order.Add(trueExpression); }
            }

            var result = new List<KyaniteExpression>();
            foreach (var trueExpression in order) result.Add(coeffs[trueExpression] is Number(1) ? trueExpression : coeffs[trueExpression]);
            return RebuildAdd(result);
        }

        private static KyaniteExpression CollectMultiply(this KyaniteExpression expression)
        {
            if (expression is not Multiply) return expression;
            var flattened = FlattenMultiply(expression);
            var exponents = new Dictionary<KyaniteExpression, KyaniteExpression>();
            var order = new List<KyaniteExpression>();
            var coeff = 1.0;

            foreach (var factor in flattened)
            {
                if (factor is Number(var x)) { coeff *= x; continue; }
                var (y, e) = factor is Power(var b, var exp) ? (b, exp) : (factor, 1);
                if (exponents.ContainsKey(y)) exponents[y] += e;
                else { exponents[y] = e; order.Add(y); }
            }

            var result = new List<KyaniteExpression>();
            if (coeff != 1.0) result.Add(coeff);
            foreach (var y in order) result.Add(exponents[y] is Number(1) ? y : y.Power(exponents[y]));
            return RebuildMultiply(result);
        }

        #region Helpers
        private static List<KyaniteExpression> FlattenAdd(KyaniteExpression expression)
        {
            if (expression is not Add) return [];
            var terms = new List<KyaniteExpression>();
            var stack = new Stack<KyaniteExpression>([expression]);
            
            while (stack.Count > 0)
            {
                var term = stack.Pop();
                if (term is Add add) { stack.Push(add.A); stack.Push(add.B); }
                else terms.Add(term);
            }

            return terms;
        }
        private static List<KyaniteExpression> FlattenMultiply(KyaniteExpression expression)
        {
            if (expression is not Multiply) return [];
            var terms = new List<KyaniteExpression>();
            var stack = new Stack<KyaniteExpression>([expression]);

            while (stack.Count > 0)
            {
                var term = stack.Pop();
                if (term is Multiply multiply) { stack.Push(multiply.A); stack.Push(multiply.B); }
                else terms.Add(term);
            }

            return terms;
        }

        private static KyaniteExpression RebuildAdd(List<KyaniteExpression> terms)
        {
            KyaniteExpression expression = 0;
            foreach (var term in terms) expression += term;
            return expression;
        }
        private static KyaniteExpression RebuildMultiply(List<KyaniteExpression> terms)
        {
            KyaniteExpression expression = 1;
            foreach (var term in terms) expression *= term;
            return expression;
        }

        private static (double, KyaniteExpression) Decompose(KyaniteExpression expression)
        {
            if (expression is Multiply)
            {
                var flattened = FlattenMultiply(expression);
                var numbers = flattened.Where(x => x is Number);
                var rest = flattened.Where(x => x is not Number);
                var coeff = numbers.Any() ? numbers.Select(x => x.Eval()).Aggregate(1.0, (a, b) => a * b) : 1;
                var trueExpression = rest.Any() ? RebuildMultiply([.. rest]) : 1;
                return (coeff, trueExpression);
            }
            return (1, expression);
        }
        #endregion
    }
}
