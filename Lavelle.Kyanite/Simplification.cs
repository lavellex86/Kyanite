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

                Pow(var x, var e) => x.SimplifyOnce().Pow(e.SimplifyOnce()),
                Sin(var x) => x.SimplifyOnce().Sin(),
                Cos(var x) => x.SimplifyOnce().Cos(),
                Tan(var x) => x.SimplifyOnce().Tan(),
                Log(var x, var b) => x.SimplifyOnce().Log(b.SimplifyOnce()),
                Sinh(var x) => x.SimplifyOnce().Sinh(),
                Cosh(var x) => x.SimplifyOnce().Cosh(),
                Tanh(var x) => x.SimplifyOnce().Tanh(),

                Derivative(var f, var x, _) => f.SimplifyOnce().D(x),
                Integral(var f, var x) => f.SimplifyOnce().Int(x),

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
                Add(var a, var b) when a.SE(b) => 2 * a,

                Multiply(Number(-1), Add(var a, var b)) => ((-1) * a) + ((-1) * b),
                Multiply(Number(var c), Add(var a, var b)) => (c * a) + (c * b),
                Multiply(var x, Pow(var y, Number(-1))) when x.SE(y) => 1,
                Multiply(Pow(var x, Number(-1)), var y) when x.SE(y) => 1,
                Multiply(Number(-1), Number(var x)) => -x,
                Multiply(Number(var x), Number(-1)) => -x,

                Pow(var x, Number(0)) => 1,
                Pow(Number(0), var x) => 0,
                Pow(var x, Number(1)) => x,
                Log(Pow(var b1, var exp), var b2) when b1.SE(b2) => exp,

                Add(Number(var a), Number(var b)) => a + b,
                Multiply(Number(var a), Number(var b)) => a * b,
                Pow(Number(var a), Number(var b)) => Math.Pow(a, b),
                Pow(Pow(var x, var a), var b) => x.Pow(a * b),

                Add(Pow(Sin(var x1), Number(2)), Pow(Cos(var x2), Number(2))) when x1.SE(x2) => 1,
                Add(Number(1), Multiply(Number(-1), Pow(Sin(var x), Number(2)))) => x.Cos().Sq(),
                Add(Multiply(Number(-1), Pow(Sin(var x), Number(2))), Number(1)) => x.Cos().Sq(),
                Add(Number(1), Multiply(Number(-1), Pow(Cos(var x), Number(2)))) => x.Sin().Sq(),
                Add(Multiply(Number(-1), Pow(Cos(var x), Number(2))), Number(1)) => x.Sin().Sq(),

                Add(Pow(Tan(var x), Number(2)), Number(1)) => x.Sec().Pow(2),
                Add(Number(1), Pow(Tan(var x), Number(2))) => x.Sec().Pow(2),
                Add(Pow(Multiply(Number(1), Pow(Tan(var x), Number(-1))), Number(2)), Number(1)) => x.Csc().Pow(2),
                Add(Number(1), Pow(Multiply(Number(1), Pow(Tan(var x), Number(-1))), Number(2))) => x.Csc().Pow(2),

                Add(Multiply(Sin(var a1), Cos(var b1)), Multiply(Cos(var a2), Sin(var b2))) when a1.SE(a2) && b1.SE(b2) => (a1 + b1).Sin(),
                Add(Multiply(Cos(var a1), Sin(var b1)), Multiply(Sin(var a2), Cos(var b2))) when a1.SE(a2) && b1.SE(b2) => (a1 + b1).Sin(),
                Add(Multiply(Cos(var a1), Cos(var b1)), Multiply(Number(-1), Multiply(Sin(var a2), Sin(var b2)))) when a1.SE(a2) && b1.SE(b2) => (a1 + b1).Cos(),
                Add(Multiply(Number(-1), Multiply(Sin(var a1), Sin(var b1))), Multiply(Cos(var a2), Cos(var b2))) when a1.SE(a2) && b1.SE(b2) => (a1 + b1).Cos(),
                Multiply(Number(2), Multiply(Sin(var x1), Cos(var x2))) when x1.SE(x2) => (2 * x1).Sin(),
                Multiply(Number(2), Multiply(Cos(var x1), Sin(var x2))) when x1.SE(x2) => (2 * x1).Sin(),
                Multiply(Multiply(Sin(var x1), Cos(var x2)), Number(2)) when x1.SE(x2) => (2 * x1).Sin(),
                Add(Pow(Cos(var x1), Number(2)), Multiply(Number(-1), Pow(Sin(var x2), Number(2)))) when x1.SE(x2) => (2 * x1).Cos(),
                Add(Multiply(Number(-1), Pow(Sin(var x1), Number(2))), Pow(Cos(var x2), Number(2))) when x1.SE(x2) => (2 * x1).Cos(),
                Add(Number(1), Multiply(Number(-2), Pow(Sin(var x), Number(2)))) => (2 * x).Cos(),
                Add(Multiply(Number(-2), Pow(Sin(var x), Number(2))), Number(1)) => (2 * x).Cos(),
                Add(Multiply(Number(2), Pow(Cos(var x), Number(2))), Number(-1)) => (2 * x).Cos(),
                Add(Number(-1), Multiply(Number(2), Pow(Cos(var x), Number(2)))) => (2 * x).Cos(),

                Sin(Number(0)) => 0,
                Cos(Number(0)) => 1,
                Tan(Number(0)) => 0,
                Sin(Variable("pi", true)) => 0,
                Cos(Variable("pi", true)) => -1,

                Log(var x, var b) when x.SE(b) => 1,
                Log(var _, Number(1)) => 1,
                Add(Log(var a, var b1), Log(var c, var b2)) when b1.SE(b2) => (a * c).Log(b1),
                Add(Log(var a, var b1), Multiply(Number(-1), Log(var c, var b2))) when b1.SE(b2) => (a * c.Pow(-1)).Log(b1),
                Add(Multiply(Number(-1), Log(var c, var b1)), Log(var a, var b2)) when b1.SE(b2) => (a * c.Pow(-1)).Log(b1),
                Multiply(Number(var n), Log(var x, var b)) => x.Pow(n).Log(b),
                Multiply(Log(var x, var b), Number(var n)) => x.Pow(n).Log(b),
                Log(Number(1), var _) => 0,

                Multiply(Multiply(var x, Number(-1)), Number(-1)) => x,

                Sinh(Number(0)) => 0,
                Cosh(Number(0)) => 1,
                Tanh(Number(0)) => 0,
                Add(Pow(Cosh(var x1), Number(2)), Multiply(Number(-1), Pow(Sinh(var x2), Number(2)))) when x1.SE(x2) => 1,
                Add(Multiply(Number(-1), Pow(Sinh(var x1), Number(2))), Pow(Cosh(var x2), Number(2))) when x1.SE(x2) => 1,
                Multiply(Number(2), Multiply(Sinh(var x1), Cosh(var x2))) when x1.SE(x2) => (2 * x1).Sinh(),
                Multiply(Number(2), Multiply(Cosh(var x1), Sinh(var x2))) when x1.SE(x2) => (2 * x1).Sinh(),
                Multiply(Multiply(Sinh(var x1), Cosh(var x2)), Number(2)) when x1.SE(x2) => (2 * x1).Sinh(),
                Add(Pow(Cosh(var x1), Number(2)), Pow(Sinh(var x2), Number(2))) when x1.SE(x2) => (2 * x1).Cosh(),
                Add(Pow(Sinh(var x2), Number(2)), Pow(Cosh(var x1), Number(2))) when x1.SE(x2) => (2 * x1).Cosh(),
                Add(Number(1), Multiply(Number(2), Pow(Sinh(var x), Number(2)))) => (2 * x).Cosh(),
                Add(Multiply(Number(2), Pow(Sinh(var x), Number(2))), Number(1)) => (2 * x).Cosh(),
                Add(Multiply(Number(2), Pow(Cosh(var x), Number(2))), Number(-1)) => (2 * x).Cosh(),
                Add(Number(-1), Multiply(Number(2), Pow(Cosh(var x), Number(2)))) => (2 * x).Cosh(),

                var x => x
            };

            simplified = CollectAdd(simplified);
            simplified = CollectMultiply(simplified);

            // TODO: polynomials

            return simplified;
        }

        private static KyaniteExpression CollectAdd(this KyaniteExpression expression)
        {
            if (expression is not Add) return expression;
            var flattened = FlattenAdd(expression);
            var coeffs = new Dictionary<KyaniteExpression, double>();
            var order = new List<KyaniteExpression>();

            foreach (var term in flattened)
            {
                var (coeff, trueExpression) = Decompose(term);
                if (coeffs.ContainsKey(trueExpression)) coeffs[trueExpression] += coeff;
                else { coeffs[trueExpression] = coeff; order.Add(trueExpression); }
            }

            order.Sort((a, b) => GetSortKey(a).CompareTo(GetSortKey(b)));

            var result = new List<KyaniteExpression>();
            foreach (var trueExpression in order)
            {
                var coeff = coeffs[trueExpression];
                if (trueExpression is Number(1)) { result.Add(coeff); continue; }
                result.Add(coeff == 1.0 ? trueExpression : coeff * trueExpression);
            }
            return RebuildAdd(result);
        }

        private static KyaniteExpression CollectMultiply(this KyaniteExpression expression)
        {
            if (expression is not Multiply) return expression;
            var flattened = FlattenMultiply(expression);
            var exponents = new Dictionary<KyaniteExpression, double>();
            var order = new List<KyaniteExpression>();
            var coeff = 1.0;

            foreach (var factor in flattened)
            {
                if (factor is Number(var x)) { coeff *= x; continue; }
                var (y, e) = factor is Pow(var b, Number(var exp)) ? (b, exp) : (factor, 1.0);
                if (exponents.ContainsKey(y)) exponents[y] += e;
                else { exponents[y] = e; order.Add(y); }
            }

            order.Sort((a, b) => GetSortKey(a).CompareTo(GetSortKey(b)));

            var result = new List<KyaniteExpression>();
            if (coeff != 1.0) result.Add(coeff);
            foreach (var y in order) result.Add(exponents[y] == 1.0 ? y : y.Pow(exponents[y]));
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

            terms.Reverse();
            return terms;
        }

        private static KyaniteExpression RebuildAdd(List<KyaniteExpression> terms)
        {
            if (terms.Count == 0) return 0;
            var expression = terms[0];
            foreach (var term in terms.Skip(1)) expression += term;
            return expression;
        }
        private static KyaniteExpression RebuildMultiply(List<KyaniteExpression> terms)
        {
            if (terms.Count == 0) return 1;
            var expression = terms[0];
            foreach (var term in terms.Skip(1)) expression *= term;
            return expression;
        }

        private static (double, KyaniteExpression) Decompose(KyaniteExpression expression)
        {

            if (expression is Number(var n)) return (n, 1);
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

        private static string GetSortKey(KyaniteExpression expression) => expression switch
        {
            Variable(var name, _) => name,
            Pow(var x, _) => GetSortKey(x),
            _ => expression.ToString()
        };
        #endregion
    }
}
