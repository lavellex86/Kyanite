# API Reference - KMath

```csharp
public static partial class KMath
```

Math utilities for Kyanite.

***

#### Methods

```csharp
public static Number N(double x)
```

Creates a new `Number` expression.

***

```csharp
public static Variable V(string x)
```

Creates a new `Variable` expression.

***

```csharp
public static Variable C(string x)
```

Creates a new constant `Variable` expression.

***

```csharp
public static Derivative D(Variable f, Variable x)
```

Creates a new symbolic `Derivative` expression.

***

```csharp
public static Derivative PD(Variable f, Variable x)
```

Creates a new symbolic partial `Derivative` expression.

***

```csharp
public static Sin Sin(KyaniteExpression expression)
```

Creates a new `Sin` expression.

***

```csharp
public static Cos Cos(KyaniteExpression expression)
```

Creates a new `Cos` expression.

***

```csharp
public static Tan Tan(KyaniteExpression expression)
```

Creates a new `Tan` expression.

***

```csharp
public static Log Log(KyaniteExpression x, KyaniteExpression b)
```

Creates a new `Log` expression.

***

```csharp
public static Pow Exp(KyaniteExpression x)
```

Raises e to the `x`th power.

***

```csharp
public static (KyaniteExpression L, KyaniteExpression R) Solve(KyaniteExpression l, KyaniteExpression r, KyaniteExpression x)
```

Solves for `x` in an equation `l` = `r`.

***

```csharp
public static Sinh Sinh(KyaniteExpression expression)
```

Creates a new `Sinh` expression.

***

```csharp
public static Cosh Cosh(KyaniteExpression expression)
```

Creates a new `Cosh` expression.

***

```csharp
public static Tanh Tanh(KyaniteExpression expression)
```

Creates a new `Tanh` expression.