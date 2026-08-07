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
        /// <param name="expression"></param>
        /// <returns></returns>
        public static KyaniteExpression Simplify(this KyaniteExpression expression)
        {
            var simplified = expression switch
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
                Multiply(Number(var a), Number(var b)) => a + b,
                Power(Number(var a), Number(var b)) => Math.Pow(a, b),

                Sin(Number(0)) => 0,
                Cos(Number(0)) => 1,
                Log(var _, Number(1)) => 1,

                Multiply(Multiply(var x, Number(-1)), Number(-1)) => x,

                var x => x
            };

            simplified = CollectAdd(simplified);

            // TODO: term collection, polynomials
            // TODO: trig

            return simplified;
        }

        #region Flattens
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
        #endregion

        private static KyaniteExpression CollectAdd(this KyaniteExpression expression)
        {
            if (expression is not Add) return expression;
            var flattened = FlattenAdd(expression);
            var coeffs = new Dictionary<KyaniteExpression, KyaniteExpression>();

            foreach (var term in flattened)
            {
                if (term is Multiply multiply)
                {
                    var flattenedMultiply = FlattenMultiply(multiply);
                    foreach (var factor in flattenedMultiply)
                    {
                        if (coeffs.ContainsKey(factor))
                        {
                            flattenedMultiply.Remove(factor);
                            coeffs[factor] += RebuildMultiply(flattenedMultiply);
                        }
                    }
                }
            }

            var result = new List<KyaniteExpression>();
            foreach (var (coeff, term) in coeffs) result.Add(coeff * term);
            return RebuildAdd(result);
        }
    }
}
