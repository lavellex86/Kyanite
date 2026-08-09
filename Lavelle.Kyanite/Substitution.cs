namespace Lavelle.Kyanite
{
    public static partial class KyaniteExtensions
    {
        /// <summary>
        /// Subsitutes expressions in for variables in an expression.
        /// </summary>
        public static KyaniteExpression VSub(this KyaniteExpression expression, Dictionary<Variable, KyaniteExpression> env) => expression switch
        {
            Variable x when env.ContainsKey(x) => env[x],

            Add(var a, var b) => a.VSub(env) + b.VSub(env),
            Multiply(var a, var b) => a.VSub(env) * b.VSub(env),

            Pow(var x, var e) => x.VSub(env).Pow(e.VSub(env)),
            Sin(var x) => x.VSub(env).Sin(),
            Cos(var x) => x.VSub(env).Cos(),
            Tan(var x) => x.VSub(env).Tan(),
            Log(var x, var b) => x.VSub(env).Log(b.VSub(env)),
            Sinh(var x) => x.VSub(env).Sinh(),
            Cosh(var x) => x.VSub(env).Cosh(),
            Tanh(var x) => x.VSub(env).Tanh(),

            Derivative(var f, var x) when f is Variable v && env.ContainsKey(v) => env[v].D(x),
            Derivative(var f, var x) => f.VSub(env).D(x),

            var x => x
        };

        /// <summary>
        /// Subsitutes an expression in for another expresion.
        /// </summary>
        public static KyaniteExpression ESub(this KyaniteExpression expression, Dictionary<KyaniteExpression, KyaniteExpression> env) => expression switch
        {
            var x when env.ContainsKey(x) => env[x],

            Add(var a, var b) => a.ESub(env) + b.ESub(env),
            Multiply(var a, var b) => a.ESub(env) * b.ESub(env),

            Pow(var x, var e) => x.ESub(env).Pow(e.ESub(env)),
            Sin(var x) => x.ESub(env).Sin(),
            Cos(var x) => x.ESub(env).Cos(),
            Tan(var x) => x.ESub(env).Tan(),
            Log(var x, var b) => x.ESub(env).Log(b.ESub(env)),
            Sinh(var x) => x.ESub(env).Sinh(),
            Cosh(var x) => x.ESub(env).Cosh(),
            Tanh(var x) => x.ESub(env).Tanh(),

            Derivative(var f, var x) when env.ContainsKey(f) => env[f].D(x),
            Derivative(var f, var x) => f.ESub(env).D(x),

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
            Dictionary<Variable, KyaniteExpression> env = [];
            foreach (var (v, n) in values) env[v] = n;
            return expression.VSub(env).Eval();
        }
    }
}
