using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lavelle.Kyanite
{
    public partial class KyaniteExtensions
    {
        /// <summary>
        /// Outputs the expression tree to LaTeX format.
        /// </summary>
        public static string ToLaTeX(this KyaniteExpression expression, int pred = 0) => expression.Simplify() switch
        {
            Number(var x) => x.ToString("G6"),
            Variable(var x, var _) => EscapeVariable(x),
            Function(var name, _) => EscapeVariable(name),

            Add(var a, var b) when Negated(b, out var trueB) => P($"{a.ToLaTeX(1)} - {trueB.ToLaTeX(1)}", pred > 1),
            Add(var a, var b) => P($"{a.ToLaTeX(1)} + {b.ToLaTeX(1)}", pred > 1),

            Multiply(Number(-1), Multiply(var a, var b)) => P($"-{a.ToLaTeX(2)} {b.ToLaTeX(2)}", pred > 2),
            Multiply(Multiply(var a, var b), Number(-1)) => P($"-{a.ToLaTeX(2)} {b.ToLaTeX(2)}", pred > 2),
            Multiply(var a, Pow(var b, Number(-1))) => $@"\frac{{ {a.ToLaTeX(0)} }}{{ {b.ToLaTeX(0)} }}",
            Multiply(var x, Number(-1)) => P($"-{x.ToLaTeX(3)}", pred > 3),
            Multiply(Number(-1) , var x) => P($"-{x.ToLaTeX(3)}", pred > 3),
            Multiply(var a, Number(var b)) => P($@"{N(b)} {a.ToLaTeX(2)}", pred > 2),
            Multiply(Number(var a), var b) => P($@"{N(a)} {b.ToLaTeX(2)}", pred > 2),
            Multiply(var a, var b) => P($@"{a.ToLaTeX(2)} {b.ToLaTeX(2)}", pred > 2),

            Pow(var x, Number(var e)) when e == 0.5 => $@"\sqrt{{ {x.ToLaTeX(0)} }}",
            Pow(var x, Number(var e)) when e < 0 => $@"\frac{{ 1 }}{{ {P(x)}^{{ {N(e)} }} }}",
            Pow(var x, var e) => $"{P(x)}^{{ {e.ToLaTeX(0)} }}",
            Sin(var x) => $@"\sin \left( {x.ToLaTeX(0)} \right)",
            Cos(var x) => $@"\cos \left( {x.ToLaTeX(0)} \right)",
            Tan(var x) => $@"\tan \left( {x.ToLaTeX(0)} \right)",
            Log(var x, Variable("e", true)) => $@"\ln \left( {x.ToLaTeX(0)} \right)",
            Log(var x, var b) => $@"\log_{{ {b.ToLaTeX(0)} }} \left( {x.ToLaTeX(0)} \right)",
            Sinh(var x) => $@"\sinh \left( {x.ToLaTeX(0)} \right)",
            Cosh(var x) => $@"\cosh \left( {x.ToLaTeX(0)} \right)",
            Tanh(var x) => $@"\tanh \left( {x.ToLaTeX(0)} \right)",

            Derivative(Derivative(var f, var x, false), var y, false) when x == y && f is Variable => $@"\frac{{ d^{{2}}{f.ToLaTeX(0)} }}{{ d{x.ToLaTeX(0)}^{{2}} }}",
            Derivative(Derivative(var f, var x, true), var y, true) when x == y && f is Variable => $@"\frac{{ \partial^{{2}}{f.ToLaTeX(0)} }}{{ \partial {x.ToLaTeX(0)}^{{2}} }}",
            Derivative(Derivative(var f, var x, false), var y, false) when x == y => $@"\frac{{ d^{{2}} }}{{ d{x.ToLaTeX(0)}^{{2}} }}\left( {f.ToLaTeX(0)} \right)",
            Derivative(Derivative(var f, var x, true), var y, true) when x == y => $@"\frac{{ \partial^{{2}} }}{{ \partial {x.ToLaTeX(0)}^{{2}} }}\left( {f.ToLaTeX(0)} \right)",

            Derivative(Derivative(var f, var x, true), var y, true) when f is Variable => $@"\frac{{ \partial^{{2}}{f.ToLaTeX(0)} }}{{\partial {y.ToLaTeX()} \partial {x.ToLaTeX(0)}}}",
            Derivative(Derivative(var f, var x, true), var y, true) => $@"\frac{{ \partial^{{2}} }}{{\partial {y.ToLaTeX()} \partial {x.ToLaTeX(0)}}}\left( {f.ToLaTeX(0)} \right)",

            Derivative(var f, var x, false) when f is Variable => $@"\frac{{ d{f.ToLaTeX(0)} }}{{ d{x.ToLaTeX(0)} }}",
            Derivative(var f, var x, false) => $@"\frac{{ d }}{{ d{x.ToLaTeX(0)} }}\left( {f.ToLaTeX(0)} \right)",
            Derivative(var f, var x, true) when f is Variable => $@"\frac{{ \partial {f.ToLaTeX(0)} }}{{ \partial {x.ToLaTeX(0)} }}",
            Derivative(var f, var x, true) => $@"\frac{{ \partial }}{{ \partial {x.ToLaTeX(0)} }}\left( {f.ToLaTeX(0)} \right)",

            Integral(var f, var x) => $@"\int {f.ToLaTeX(0)} \, d{x.ToLaTeX(0)}",

            _ => throw new Exception("Expression is not of any Kyanite-supplied type")
        };
        private static string P(string s, bool use) => use ? $@"\left( {s} \right)" : s;
        private static string P(KyaniteExpression expression) => expression switch
        {
            Number _ or Variable _ => expression.ToLaTeX(0),
            _ => $@"\left( {expression.ToLaTeX(0)} \right)"
        };
        private static string N(double number) => number.ToString("G6");
        private static bool Negated(KyaniteExpression expression, out KyaniteExpression trueX)
        {
            if (expression is Number(var n) && n < 0) { trueX = new Number(-n); return true; }
            if (expression is Multiply(Number(-1), var x)) { trueX = x; return true; }
            if (expression is Multiply(var y, Number(-1))) { trueX = y; return true; }
            if (expression is Multiply(Number(var z), var w) && z < 0)
            {
                trueX = new Multiply(new Number(-z), w);
                return true;
            }
            if (expression is Multiply(var a, Number(var b)) && b < 0)
            {
                trueX = new Multiply(a, new Number(-b));
                return true;
            }
            trueX = expression;
            return false;
        }

        private static string EscapeVariable(string name)
        {
            var prefixes = new string[] { "ddot", "dot", "bar" }; 

            foreach (var prefix in prefixes)
            {
                if (name.StartsWith(prefix))
                {
                    name = $@"\{prefix}{{{name[prefix.Length..]}}}";
                    break;
                }
            }

            var greeks = new Dictionary<string, string>
            {
                {"Theta", @"\Theta"}, {"theta", @"\theta"}, 
                {"pi", @"\pi"}, {"alpha", @"\alpha"}, {"beta", @"\beta"},
                {"gamma", @"\gamma"}, {"delta", @"\delta"}, {"epsilon", @"\epsilon"},
                 {"lambda", @"\lambda"}, {"mu", @"\mu"},
                {"sigma", @"\sigma"}, {"omega", @"\omega"}, {"phi", @"\phi"},
                {"psi", @"\psi"}, {"rho", @"\rho"}, {"tau", @"\tau"},
                {"eta", @"\eta"}, {"nu", @"\nu"}, {"xi", @"\xi"},
                {"zeta", @"\zeta"}, {"Pi", @"\Pi"}, {"Gamma", @"\Gamma"},
                {"Delta", @"\Delta"},  {"Lambda", @"\Lambda"},
                {"Sigma", @"\Sigma"}, {"Omega", @"\Omega"}, {"Phi", @"\Phi"},
                {"Psi", @"\Psi"}, {"Xi", @"\Xi"}, {"Upsilon", @"\Upsilon"}
            };

            var subscriptIndex = name.IndexOf('_');
            var superscriptIndex = name.IndexOf('^');
            var variable = name.Substring(0, superscriptIndex >= 0 ? superscriptIndex : name.Length);
            variable = variable.Substring(0, subscriptIndex >= 0 ? subscriptIndex : variable.Length);

            var bracketIndex = variable.IndexOf('{');
            variable = variable[(bracketIndex >= 0 ? bracketIndex : 0)..].Replace("}", "");

            if (greeks.TryGetValue(variable, out var escaped)) name = name.Replace(variable, escaped);
            return name;
        }
    }
}
