using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lavelle.Kyanite
{
    public partial class KyaniteExtensions
    {
        /// <summary>
        /// Compiles an expression to a C# function.
        /// </summary>
        public static Func<Dictionary<string, double>, double> Compile(this KyaniteExpression expression)
        {
            expression = expression.Sub(new()
            {
                ["e"] = Math.E,
                ["pi"] = Math.PI
            }).Simplify();
            var indices = new Dictionary<Variable, int>();
            expression.FindVariables(indices);
            var parameter = Expression.Parameter(typeof(double[]), "variables");

            var function = expression.Emit(parameter, indices);
            var lambda = Expression.Lambda<Func<double[], double>>(function, parameter);
            var compiled = lambda.Compile();
            
            double ToReturn(Dictionary<string, double> parameters)
            {
                var missing = indices.Keys.Except(parameters.Keys.Select(x => new Variable(x, false))).ToList();
                if (missing.Count > 0)
                    throw new ArgumentException($"Missing variables: {string.Join(", ", missing.Select(v => v.Name))}");
                foreach (var variable in parameters.Keys) { if (!indices.ContainsKey(KMath.V(variable))) throw new Exception("Extra variable: " + variable); }

                var sorted = parameters.OrderBy(x => indices[KMath.V(x.Key)]).ToDictionary();
                return compiled([.. sorted.Values]);
            }

            return ToReturn;
        }

        private static Expression Emit(this KyaniteExpression expression, ParameterExpression parameter, Dictionary<Variable, int> indices) => expression switch
        {
            Number(var x) => Expression.Constant(x),
            Variable x => Expression.ArrayIndex(parameter, Expression.Constant(indices[x])),

            Add(var a, var b) => Expression.Add(a.Emit(parameter, indices), b.Emit(parameter, indices)),
            Multiply(var a, var b) => Expression.Multiply(a.Emit(parameter, indices), b.Emit(parameter, indices)),

            Pow(var x, var e) => Call("Pow", x.Emit(parameter, indices), e.Emit(parameter, indices)),
            Sin(var x) => Call("Sin", x.Emit(parameter, indices)),
            Cos(var x) => Call("Cos", x.Emit(parameter, indices)),
            Tan(var x) => Call("Tan", x.Emit(parameter, indices)),
            Log(var x, var b) => Call("Log", x.Emit(parameter, indices), b.Emit(parameter, indices)),
            Sinh(var x) => Call("Sinh", x.Emit(parameter, indices)),
            Cosh(var x) => Call("Cosh", x.Emit(parameter, indices)),
            Tanh(var x) => Call("Tanh", x.Emit(parameter, indices)),
            _ => throw new Exception("Some indefinite expression reached emit!")
        };

        private static Expression Call(string name, params Expression[] parameters) => Expression.Call(typeof(Math).GetMethod(name, [.. parameters.Select(x => x.Type)])!, parameters);

        private static void FindVariables(this KyaniteExpression expression, Dictionary<Variable, int> indices)
        {
            switch (expression)
            {
                case Variable y when !indices.ContainsKey(y):
                    indices[y] = indices.Count;
                    break;
                case Add(var a, var b):
                    a.FindVariables(indices);
                    b.FindVariables(indices);
                    break;
                case Multiply(var a, var b):
                    a.FindVariables(indices);
                    b.FindVariables(indices);
                    break;
                case Pow(var x, var e):
                    x.FindVariables(indices);
                    e.FindVariables(indices);
                    break;
                case Sin(var x):
                    x.FindVariables(indices);
                    break;
                case Cos(var x):
                    x.FindVariables(indices);
                    break;
                case Tan(var x):
                    x.FindVariables(indices);
                    break;
                case Sinh(var x):
                    x.FindVariables(indices);
                    break;
                case Cosh(var x):
                    x.FindVariables(indices);
                    break;
                case Tanh(var x):
                    x.FindVariables(indices);
                    break;
                case Log(var x, var b):
                    x.FindVariables(indices);
                    b.FindVariables(indices);
                    break;
                case Derivative(var f, var x, _):
                    f.FindVariables(indices);
                    x.FindVariables(indices);
                    break;
                case Integral(var f, var x):
                    f.FindVariables(indices);
                    x.FindVariables(indices);
                    break;
            }
        }
    }
}
