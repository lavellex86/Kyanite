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
        public static KyaniteExpression operator +(KyaniteExpression a, KyaniteExpression b) => new Add(a, b);
        /// <summary>
        /// Subtracts two expressions.
        /// </summary>
        public static KyaniteExpression operator -(KyaniteExpression a, KyaniteExpression b) => new Add(a, -b);
        /// <summary>
        /// Multiplies two expressions.
        /// </summary>
        public static KyaniteExpression operator *(KyaniteExpression a, KyaniteExpression b) => new Multiply(a, b);
        /// <summary>
        /// Divides two expressions.
        /// </summary>
        public static KyaniteExpression operator /(KyaniteExpression a, KyaniteExpression b) => new Multiply(a, b.Power(-1));
        /// <summary>
        /// Negates an expression.
        /// </summary>
        public static KyaniteExpression operator -(KyaniteExpression x) => new Multiply(-1, x);

        /// <summary>
        /// Raises the expression to an exponent.
        /// </summary>
        public KyaniteExpression Power(KyaniteExpression e) => new Power(this, e);
        /// <summary>
        /// Takes the sine of the expression.
        /// </summary>
        public KyaniteExpression Sin() => new Sin(this);
        /// <summary>
        /// Takes the cosine of the expression.
        /// </summary>
        /// <returns></returns>
        public KyaniteExpression Cos() => new Cos(this);
        /// <summary>
        /// Takes the tangent of the expression.
        /// </summary>
        public KyaniteExpression Tan() => new Tan(this);
        /// <summary>
        /// Takes the secant of the expression.
        /// </summary>
        public KyaniteExpression Sec() => 1 / Cos();
        /// <summary>
        /// Takes the logarithm of the expression.
        /// </summary>
        public KyaniteExpression Log(KyaniteExpression b) => new Log(this, b);

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
    public record Power(KyaniteExpression X, KyaniteExpression E) : KyaniteExpression;
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
    public record Derivative(KyaniteExpression f, Variable x) : KyaniteExpression;
}
