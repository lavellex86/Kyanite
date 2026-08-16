namespace Lavelle.Kyanite
{
    public static partial class KyaniteExtensions
    {
        /// <summary>
        /// Subsitutes an expression in for another expresion.
        /// </summary>
        public static KyaniteExpression Sub(this KyaniteExpression expression, Dictionary<KyaniteExpression, KyaniteExpression> env) => expression.Simplify() switch
        {
            var x when env.ContainsKey(x) => env[x],

            Add(var a, var b) => a.Sub(env) + b.Sub(env),
            Multiply(var a, var b) => a.Sub(env) * b.Sub(env),

            Pow(var x, var e) => x.Sub(env).Pow(e.Sub(env)),
            Sin(var x) => x.Sub(env).Sin(),
            Cos(var x) => x.Sub(env).Cos(),
            Tan(var x) => x.Sub(env).Tan(),
            Log(var x, var b) => x.Sub(env).Log(b.Sub(env)),
            Sinh(var x) => x.Sub(env).Sinh(),
            Cosh(var x) => x.Sub(env).Cosh(),
            Tanh(var x) => x.Sub(env).Tanh(),

            Derivative(var f, var x, true) when env.ContainsKey(f) => new Derivative(env[f], x, true),
            Derivative(var f, var x, true) => new Derivative(f.Sub(env), x, true),
            Derivative(var f, var x, false) when env.ContainsKey(f) => env[f].D(x),
            Derivative(var f, var x, false) => f.Sub(env).D(x),

            Integral(var f, var v) when env.ContainsKey(f) => env[f].Int(v),
            Integral(var f, var v) => f.Sub(env).Int(v),

            Function(var name, var parameters) => new Function(name, [.. parameters.Select(x => env.TryGetValue(x, out var value) ? value : x)]),

            var x => x
        };

        /// <summary>
        /// Numerically evaluates an expression.
        /// </summary>
        public static double Eval(this KyaniteExpression expression) => expression.Simplify() switch
        {
            Number(var x) => x,
            Variable("pi", true) => Math.PI,
            Variable("e", true) => Math.E,

            Add(var a, var b) => a.Eval() + b.Eval(),
            Multiply(var a, var b) => a.Eval() * b.Eval(),

            Pow(var x, var e) => Math.Pow(x.Eval(), e.Eval()),
            Sin(var x) => Math.Sin(x.Eval()),
            Cos(var x) => Math.Cos(x.Eval()),
            Tan(var x) => Math.Tan(x.Eval()),
            Log(var x, var b) => Math.Log(x.Eval(), b.Eval()),

            var x => throw new Exception("Unbound variables are present in the expression: " + x.ToLaTeX()),
        };

        /// <summary>
        /// Numerically evaluates an expression for a given set of variable values.
        /// </summary>
        public static double At(this KyaniteExpression expression, Dictionary<string, double> values)
        {
            expression = expression.Sub(new()
            {
                ["e"] = Math.E,
                ["pi"] = Math.PI
            }).Simplify();
            Dictionary<KyaniteExpression, KyaniteExpression> env = [];
            foreach (var (v, n) in values) env[KMath.V(v)] = n;
            return expression.Sub(env).Eval();
        }

        /// <summary>
        /// Swaps expressions using a predicate and swapper. When the predicate matches, the swapper will be run on the expression matched.
        /// </summary>
        public static KyaniteExpression Swap(this KyaniteExpression expression, Func<KyaniteExpression, bool> predicate, Func<KyaniteExpression, KyaniteExpression> swapper) => expression switch
        {
            var x when predicate(x) => swapper(x),

            Add(var a, var b) => a.Swap(predicate, swapper) + b.Swap(predicate, swapper),
            Multiply(var a, var b) => a.Swap(predicate, swapper) * b.Swap(predicate, swapper),

            Pow(var x, var e) => x.Swap(predicate, swapper).Pow(e.Swap(predicate, swapper)),
            Sin(var x) => x.Swap(predicate, swapper).Sin(),
            Cos(var x) => x.Swap(predicate, swapper).Cos(),
            Tan(var x) => x.Swap(predicate, swapper).Tan(),
            Log(var x, var b) => x.Swap(predicate, swapper).Log(b.Swap(predicate, swapper)),
            Sinh(var x) => x.Swap(predicate, swapper).Sinh(),
            Cosh(var x) => x.Swap(predicate, swapper).Cosh(),
            Tanh(var x) => x.Swap(predicate, swapper).Tanh(),

            var x => x
        };

        /// <summary>
        /// Applies a rule to all functions with names containing <c>name</c>.
        /// </summary>
        public static KyaniteExpression Apply(this KyaniteExpression expression, string name, Func<List<KyaniteExpression>, KyaniteExpression> rule) =>
            expression.Swap(ex => ex is Function f && f.Name.Contains(name), ex => rule(((Function)ex).Parameters));
    }
}