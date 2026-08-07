using System;
using System.Collections.Generic;
using System.Text;

namespace Lavelle.Kyanite
{
    public static class KMath
    {
        public static Number N(double x) => new Number(x);
        public static Variable V(string x) => new Variable(x);

        public static Sin Sin(KyaniteExpression expression) => new Sin(expression);
        public static Cos Cos(KyaniteExpression expression) => new Cos(expression);
        public static Tan Tan(KyaniteExpression expression) => new Tan(expression);
        public static Log Log(KyaniteExpression x, KyaniteExpression b) => new Log(x, b);
    }
}
