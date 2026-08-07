using System;
using System.Collections.Generic;
using System.Text;

namespace Lavelle.Kyanite
{
    public abstract record KyaniteExpression
    {
        public static KyaniteExpression operator +(KyaniteExpression a, KyaniteExpression b) => new Add(a, b);
        public static KyaniteExpression operator -(KyaniteExpression a, KyaniteExpression b) => new Add(a, -b);
        public static KyaniteExpression operator *(KyaniteExpression a, KyaniteExpression b) => new Multiply(a, b);
        public static KyaniteExpression operator /(KyaniteExpression a, KyaniteExpression b) => new Multiply(a, b.Power(-1));
        public static KyaniteExpression operator -(KyaniteExpression x) => new Multiply(-1, x);

        public KyaniteExpression Power(KyaniteExpression e) => new Power(this, e);
        public KyaniteExpression Sin() => new Sin(this);
        public KyaniteExpression Cos() => new Cos(this);
        public KyaniteExpression Tan() => new Tan(this);
        public KyaniteExpression Sec() => 1 / Cos();
        public KyaniteExpression Log(KyaniteExpression b) => new Log(this, b);

        public static implicit operator KyaniteExpression(double x) => new Number(x);
        public static implicit operator KyaniteExpression(string x) => new Variable(x);
    }

    public record Number(double Value) : KyaniteExpression
    {
        public static implicit operator Number(double x) => new Number(x);
    }
    public record Variable(string Name) : KyaniteExpression
    {
        public static implicit operator Variable(string x) => new Variable(x);
    }

    public record Add(KyaniteExpression A, KyaniteExpression B) : KyaniteExpression;
    public record Multiply(KyaniteExpression A, KyaniteExpression B) : KyaniteExpression;

    public record Power(KyaniteExpression X, KyaniteExpression E) : KyaniteExpression;
    public record Sin(KyaniteExpression X) : KyaniteExpression;
    public record Cos(KyaniteExpression X) : KyaniteExpression;
    public record Tan(KyaniteExpression X) : KyaniteExpression;
    public record Log(KyaniteExpression X, KyaniteExpression B) : KyaniteExpression;

    public record Derivative(Variable f, Variable x) : KyaniteExpression;
}
