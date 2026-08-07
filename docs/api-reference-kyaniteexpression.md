# API Reference - KyaniteExpression

```csharp
public abstract record KyaniteExpression
```

Represents a mathematical expression.

***

### Operators

```csharp
public static KyaniteExpression operator +(KyaniteExpression a, KyaniteExpression b)
```

Adds two expressions.

***

```csharp
public static KyaniteExpression operator -(KyaniteExpression a, KyaniteExpression b)
```

Subtracts two expressions.

***

```csharp
public static KyaniteExpression operator *(KyaniteExpression a, KyaniteExpression b)
```

Multiplies two expressions.

***

```csharp
public static KyaniteExpression operator /(KyaniteExpression a, KyaniteExpression b)
```

Divides two expressions.

***

```csharp
public static KyaniteExpression operator -(KyaniteExpression x)
```

Negates an expression.

***

### Methods

```csharp
public KyaniteExpression Pow(KyaniteExpression e)
```

Raises the expression to an exponent.

***

```csharp
public KyaniteExpression Sin()
```

Takes the sine of the expression.

***

```csharp
public KyaniteExpression Cos()
```

Takes the cosine of the expression.

***

```csharp
public KyaniteExpression Tan()
```

Takes the tangent of the expression.

***

```csharp
public KyaniteExpression Sec()
```

Takes the secant of the expression.

***

```csharp
public KyaniteExpression Log(KyaniteExpression b)
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

### Extension Methods

```csharp
public static KyaniteExpression D(this KyaniteExpression expression, Variable x, bool partial = false)
```

Takes the derivative of `expression` w.r.t `x`. Takes the partial derivative if `partial` is true.

***

```csharp
public static KyaniteExpression PD(this KyaniteExpression expression, Variable x)
```

Takes the partial derivative of `expression` w.r.t `x`.

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
public static KyaniteExpression DSub(this KyaniteExpression expression, Dictionary<Derivative, KyaniteExpression> env)
```

Substitutes expressions in for derivatives in an expression.

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
