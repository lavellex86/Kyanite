# API Reference - Expression Types

### Number

```csharp
public record Number(double Value) : KyaniteExpression
```

Represents a numerical value.

***

### Variable

```csharp
public record Variable(string Name, bool Constant = false) : KyaniteExpression
```

Represents a variable.

***

### Add

```csharp
public record Add(KyaniteExpression A, KyaniteExpression B) : KyaniteExpression
```

Represents a sum.

***

### Multiply

```csharp
public record Multiply(KyaniteExpression A, KyaniteExpression B) : KyaniteExpression
```

Represents a product.

***

### Pow

```csharp
public record Pow(KyaniteExpression X, KyaniteExpression E) : KyaniteExpression
```

Represents an exponentiation operation.

***

### Sin

```csharp
public record Sin(KyaniteExpression X) : KyaniteExpression
```

Represents a sine operation.

***

### Cos

```csharp
public record Cos(KyaniteExpression X) : KyaniteExpression
```

Represents a cosine operation.

***

### Tan

```csharp
public record Tan(KyaniteExpression X) : KyaniteExpression
```

Represents a tangent operation.

***

### Log

```csharp
public record Log(KyaniteExpression X, KyaniteExpression B) : KyaniteExpression
```

Represents a logarithm operation.

***

### Derivative

```csharp
public record Derivative(KyaniteExpression f, Variable x, bool Partial = false) : KyaniteExpression
```

Represents a derivative operation.

***

### Sinh

```csharp
public record Sinh(KyaniteExpression X) : KyaniteExpression
```

Represents a hyperbolic sine operation.

***

### Cosh

```csharp
public record Cosh(KyaniteExpression X) : KyaniteExpression
```

Represents a hyperbolic cosine operation.

***

### Tanh

```csharp
public record Tanh(KyaniteExpression X) : KyaniteExpression
```

Represents a hyperbolic tangent operation.

***

### Integral

```csharp
public record Integral(KyaniteExpression F, Variable X) : KyaniteExpression;
```

Represents an integral.

***

### Function

```csharp
public record Function(string Name, List<KyaniteExpression> Parameters) : KyaniteExpression;
```

Represents a function.