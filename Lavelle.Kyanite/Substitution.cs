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

            Derivative(var f, var x) when f is Variable v && env.ContainsKey(v) => env[v].D(x),

            var x => x
        };

        /// <summary>
        /// Subsitutes expressions in for derivatives in an expression.
        /// </summary>
        public static KyaniteExpression DSub(this KyaniteExpression expression, Dictionary<Derivative, KyaniteExpression> env) => expression switch
        {
            Add(var a, var b) => a.DSub(env) + b.DSub(env),
            Multiply(var a, var b) => a.DSub(env) * b.DSub(env),

            Pow(var x, var e) => x.DSub(env).Pow(e.DSub(env)),
            Sin(var x) => x.DSub(env).Sin(),
            Cos(var x) => x.DSub(env).Cos(),
            Tan(var x) => x.DSub(env).Tan(),
            Log(var x, var b) => x.DSub(env).Log(b.DSub(env)),

            Derivative d when env.ContainsKey(d) => env[d],

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
