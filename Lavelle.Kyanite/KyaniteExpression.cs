using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lavelle.Kyanite
{
    /// <summary>
    /// Represents a mathematical expression.
    /// </summary>
    public abstract record KyaniteExpression
    {
        /// <summary>
        /// Adds two expressions.
        /// </summary>
        public static Add operator +(KyaniteExpression a, KyaniteExpression b) => new Add(a, b);
        /// <summary>
        /// Subtracts two expressions.
        /// </summary>
        public static Add operator -(KyaniteExpression a, KyaniteExpression b) => new Add(a, -b);
        /// <summary>
        /// Multiplies two expressions.
        /// </summary>
        public static Multiply operator *(KyaniteExpression a, KyaniteExpression b) => new Multiply(a, b);
        /// <summary>
        /// Divides two expressions.
        /// </summary>
        public static Multiply operator /(KyaniteExpression a, KyaniteExpression b) => new Multiply(a, b.Pow(-1));
        /// <summary>
        /// Negates an expression.
        /// </summary>
        public static Multiply operator -(KyaniteExpression x) => new Multiply(-1, x);

        /// <summary>
        /// Multiples two expressions.
        /// </summary>
        public Multiply this[KyaniteExpression expression] { get => this * expression; }

        /// <summary>
        /// Raises the expression to an exponent.
        /// </summary>
        public Pow Pow(KyaniteExpression e) => new Pow(this, e);
        /// <summary>
        /// Takes the sine of the expression.
        /// </summary>
        public Sin Sin() => new Sin(this);
        /// <summary>
        /// Takes the cosine of the expression.
        /// </summary>
        /// <returns></returns>
        public Cos Cos() => new Cos(this);
        /// <summary>
        /// Takes the tangent of the expression.
        /// </summary>
        public Tan Tan() => new Tan(this);
        /// <summary>
        /// Takes the logarithm of the expression.
        /// </summary>
        public Log Log(KyaniteExpression b) => new Log(this, b);
        /// <summary>
        /// Takes the hyperbolic sine of the expression.
        /// </summary>
        public Sinh Sinh() => new(this);
        /// <summary>
        /// Takes the hyperbolic cosine of the operation.
        /// </summary>
        /// <returns></returns>
        public Cosh Cosh() => new(this);
        /// <summary>
        /// Takes the hyperbolic tangent of the expression.
        /// </summary>
        /// <returns></returns>
        public Tanh Tanh() => new(this);

        /// <summary>
        /// Takes the inverse of the expression.
        /// </summary>
        /// <returns></returns>
        public Pow Inverse() => Pow(-1); 
        /// <summary>
        /// Takes the square root of the expression.
        /// </summary>
        /// <returns></returns>
        public Pow Sqrt() => Pow(0.5);
        /// <summary>
        /// Takes the square of the expression.
        /// </summary>
        public Pow Sq () => Pow(2);
        /// <summary>
        /// Takes the natural logarithm of the expression.
        /// </summary>
        /// <returns></returns>
        public Log Ln() => Log(KMath.C("e"));

        /// <summary>
        /// Takes the secant of the expression.
        /// </summary>
        public Multiply Sec() => 1 / Cos();
        /// <summary>
        /// Takes the cosecant of the expression.
        /// </summary>
        public Multiply Csc() => 1 / Sin();
        /// <summary>
        /// Takes the cotangent of the expression.
        /// </summary>
        public Multiply Cot() => 1 / Tan();
        /// <summary>
        /// Takes the hyperbolic secant of the expression.
        /// </summary>
        public Multiply Sech() => 1 / Cosh();
        /// <summary>
        /// Takes the hyperbolic cosecant of the expression.
        /// </summary>
        public Multiply Csch() => 1 / Sinh();
        /// <summary>
        /// Takes the hyperbolic cotangent of the expression.
        /// </summary>
        public Multiply Coth() => 1 / Tanh();

        /// <summary>
        /// Converts a `double` to a `Number`.
        /// </summary>
        public static implicit operator KyaniteExpression(double x) => new Number(x);
        /// <summary>
        /// Converts a `string` to a `Variable`.
        /// </summary>
        public static implicit operator KyaniteExpression(string x) => new Variable(x);
    }

    /// <summary>
    /// Represents a numerical value.
    /// </summary>
    public record Number(double Value) : KyaniteExpression
    {
        /// <summary>
        /// Converts a `double` to a `Number`.
        /// </summary>
        public static implicit operator Number(double x) => new Number(x);
    }
    /// <summary>
    /// Represents a variable.
    /// </summary>
    public record Variable(string Name, bool Constant = false) : KyaniteExpression
    {
        /// <summary>
        /// Converts a `string` to a `Variable`.
        /// </summary>
        public static implicit operator Variable(string x) => new Variable(x);
    }

    /// <summary>
    /// Represents a sum.
    /// </summary>
    public record Add(KyaniteExpression A, KyaniteExpression B) : KyaniteExpression;
    /// <summary>
    /// Represents a product.
    /// </summary>
    public record Multiply(KyaniteExpression A, KyaniteExpression B) : KyaniteExpression;

    /// <summary>
    /// Represents an exponentiation operaton.
    /// </summary>
    public record Pow(KyaniteExpression X, KyaniteExpression E) : KyaniteExpression;
    /// <summary>
    /// Represents a sine operation.
    /// </summary>
    public record Sin(KyaniteExpression X) : KyaniteExpression;
    /// <summary>
    /// Represents a cosine operation.
    /// </summary>
    public record Cos(KyaniteExpression X) : KyaniteExpression;
    /// <summary>
    /// Represents a tangent operation.
    /// </summary>
    /// <param name="X"></param>
    public record Tan(KyaniteExpression X) : KyaniteExpression;
    /// <summary>
    /// Represents a logarithm operation.
    /// </summary>
    public record Log(KyaniteExpression X, KyaniteExpression B) : KyaniteExpression;
    
    /// <summary>
    /// Represents a derivative operation.
    /// </summary>
    public record Derivative(KyaniteExpression F, Variable X, bool Partial) : KyaniteExpression;

    /// <summary>
    /// Represents a hyperbolic sine operation.
    /// </summary>
    public record Sinh(KyaniteExpression X) : KyaniteExpression;
    /// <summary>
    /// Represents a hyperbolic cosine operation.
    /// </summary>
    public record Cosh(KyaniteExpression X) : KyaniteExpression;
    /// <summary>
    /// Represents a hyperbolic tangent operation.
    /// </summary>
    public record Tanh(KyaniteExpression X) : KyaniteExpression;

    /// <summary>
    /// Represents an integral.
    /// </summary>
    public record Integral(KyaniteExpression F, Variable X) : KyaniteExpression;
}
