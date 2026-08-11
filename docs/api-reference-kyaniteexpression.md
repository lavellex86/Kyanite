# API Reference - KyaniteExpression

```csharp
public abstract record KyaniteExpression
```

Represents a mathematical expression.

***

### Operators

```csharp
public static Add operator +(KyaniteExpression a, KyaniteExpression b)
```

Adds two expressions.

***

```csharp
public static Add operator -(KyaniteExpression a, KyaniteExpression b)
```

Subtracts two expressions.

***

```csharp
public static Multiply operator *(KyaniteExpression a, KyaniteExpression b)
```

Multiplies two expressions.

***

```csharp
public static Multiply operator /(KyaniteExpression a, KyaniteExpression b)
```

Divides two expressions.

***

```csharp
public static Multiply operator -(KyaniteExpression x)
```

Negates an expression.

***

```csharp
public Multiply this[KyaniteExpression expression] { get => this * expression; }
```
Multiplies two expressions.

### Methods

```csharp
public Pow Pow(KyaniteExpression e)
```

Raises the expression to an exponent.

***

```csharp
public Sin Sin()
```

Takes the sine of the expression.

***

```csharp
public Cos Cos()
```

Takes the cosine of the expression.

***

```csharp
public Tan Tan()
```

Takes the tangent of the expression.

***

```csharp
public Pow Inverse()
```

Takes the inverse of the expression.

***

```csharp
public Pow Sqrt()
```

Takes the square root of the expression.

***

```csharp
public Pow Sq()
```

Takes the square of the expression

***

```csharp
public Log Ln()
```

Takes the natural logarithm of the expression.

***

```csharp
public Multiply Sec()
```

Takes the secant of the expression.

***

```csharp
public Multiply Csc()
```

Takes the cosecant of the expression.

***

```csharp
public Multiply Cot()
```

Takes the cotangent of the expression.

***

```csharp
public Log Log(KyaniteExpression b)
```

Takes the logarithm of the expression.

***

```csharp
public static implicit operator KyaniteExpression(double x)
```

Converts a `double` to a `Number`.

***

```csharp
public static implicit operator KyaniteExpression(string x)
```

Converts a `string` to a `Variable`.

***

```csharp
public Sinh Sinh()
```

Takes the hyperbolic sine of the expression.

***

```csharp
public Cosh Cosh()
```

Takes the hyperbolic cosine of the expression.

***

```csharp
public Tanh Tanh()
```

Takes the hyperbolic tangent of the expression.

***

```csharp

```

### Extension Methods

```csharp
public static KyaniteExpression D(this KyaniteExpression expression, Variable x, bool partial = false)
```

Takes the derivative w.r.t `x`. Takes the partial derivative if `partial` is true.

***

```csharp
public static KyaniteExpression PD(this KyaniteExpression expression, Variable x, List<Variable>? allowed = null)
```

Takes the partial derivative of `expression` w.r.t `x`.
Partial derivatives of variables within `allowed` will be kept and marked as partial derivatives.

***

```csharp
public static KyaniteExpression Simplify(this KyaniteExpression expression)
```

Simplifies an expression.

***

```csharp
public static KyaniteExpression VSub(this KyaniteExpression expression, Dictionary<Variable, KyaniteExpression> env)
```

Substitutes expressions in for variables in an expression.

***

```csharp
public static double Eval(this KyaniteExpression expression)
```

Numerically evaluates an expression.

***

```csharp
public static double At(this KyaniteExpression expression, Dictionary<string, double> values)
```

Numerically evaluates an expression for a given set of variable values.

***

```csharp
public static bool SE(this KyaniteExpression a, KyaniteExpression b)
```

Checks for semantic equality.

***

```csharp
public static string ToLaTeX(this KyaniteExpression expression, int pred = 0)
```

Outputs the expression tree to LaTeX format.

***

```csharp
public static KyaniteExpression ESub(this KyaniteExpression expression, Dictionary<KyaniteExpression, KyaniteExpression> env)
```

Subsitutes an expression in for another expresion.

***

```csharp
public static bool Has(this KyaniteExpression expression, KyaniteExpression x)
```

Checks whether the expression contains `x`.

***

```csharp
public static KyaniteExpression Int(this KyaniteExpression expression, Variable x, Variable? C = null)
```

Integrates an expression w.r.t <paramref name="x"/> with constant of integration <paramref name="C"/>.

***

```csharp
public static Func<Dictionary<string, double>, double> Compile(this KyaniteExpression expression)
```

Compiles an expression to a C# function.