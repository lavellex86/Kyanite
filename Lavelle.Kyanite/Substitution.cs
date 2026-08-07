namespace Lavelle.Kyanite
{
    public static partial class KyaniteExtensions
    {
        public static KyaniteExpression Sub(this KyaniteExpression expression, Dictionary<Variable, KyaniteExpression> env) => expression switch
        {
            Variable x when env.ContainsKey(x) => env[x],

            Add(var a, var b) => a.Sub(env) + b.Sub(env),
            Multiply(var a, var b) => a.Sub(env) * b.Sub(env),

            Power(var x, var e) => x.Sub(env).Power(e.Sub(env)),
            Sin(var x) => x.Sub(env).Sin(),
            Cos(var x) => x.Sub(env).Cos(),
            Tan(var x) => x.Sub(env).Tan(),
            Log(var x, var b) => x.Sub(env).Log(b.Sub(env)),

            Derivative(var f, var x) when env.ContainsKey(f) => env[f].D(x),

            var x => x
        };

        public static double Eval(this KyaniteExpression expression) => expression switch
        {
            Number(var x) => x,
            Variable("pi") => Math.PI,
            Variable("e") => Math.E,

            Add(var a, var b) => a.Eval() + b.Eval(),
            Multiply(var a, var b) => a.Eval() * b.Eval(),

            Power(var x, var e) => Math.Pow(x.Eval(), e.Eval()),
            Sin(var x) => Math.Sin(x.Eval()),
            Cos(var x) => Math.Cos(x.Eval()),
            Tan(var x) => Math.Tan(x.Eval()),
            Log(var x, var b) => Math.Log(x.Eval(), b.Eval()),

            _ => throw new Exception("Unbound variables are present in the expression"),
        };

        public static double At(this KyaniteExpression expression, (Variable, Number)[] values)
        {
            Dictionary<Variable, KyaniteExpression> env = [];
            foreach (var (v, n) in values) env[v] = n;
            return expression.Sub(env).Eval();
        }
    }
}
