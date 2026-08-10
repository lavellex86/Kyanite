namespace Lavelle.Kyanite
{
    public static partial class KyaniteExtensions
    {
        /// <summary>
        /// Subsitutes an expression in for another expresion.
        /// </summary>
        public static KyaniteExpression Sub(this KyaniteExpression expression, Dictionary<KyaniteExpression, KyaniteExpression> env) => expression switch
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

            Derivative(var f, var x) when env.ContainsKey(f) => env[f].D(x),
            Derivative(var f, var x) => f.Sub(env).D(x),

            Integral(var f, var v) when env.ContainsKey(f) => env[f].Int(v),
            Integral(var f, var v) => f.Sub(env).Int(v),

            var x => x
        };

        /// <summary>
        /// Numerically evaluates an expression.
        /// </summary>
        public static double Eval(this KyaniteExpression expression) => expression switch
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

            _ => throw new Exception("Unbound variables are present in the expression"),
        };

        /// <summary>
        /// Numerically evaluates an expression for a given set of variable values.
        /// </summary>
        public static double At(this KyaniteExpression expression, Dictionary<string, double> values)
        {
            Dictionary<KyaniteExpression, KyaniteExpression> env = [];
            foreach (var (v, n) in values) env[v] = n;
            return expression.Sub(env).Eval();
        }
    }
}
