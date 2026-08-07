# Getting Started

{% hint style="info" %}
Check out the repository and README [here](https://github.com/lavellex86/Kyanite/).
{% endhint %}

To get started with Kyanite, let's look at an example script showcasing it's features:

{% code overflow="wrap" %}
```csharp
using Lavelle.Kyanite;

Variable x0 = KMath.C("x_0"), v0 = KMath.C("v_0"), g = KMath.C("g"), t = KMath.V("t");
// the KMath class has three utilities for creating symbols:
// - V, which creates a new variable
// - C, which creates a new constant
// - D, which creates a new symbolic derivative
// Variable inherits from the base class KyaniteExpression

var x = x0 + v0 * t + 0.5 * g * t.Pow(2);
// KyaniteExpression objects have operators and methods attached, so expressions can be written easily
// this is x0 + v0 t + 0.5 g t^2
var v = x.D(t); // KyaniteExpression.D(x) takes the derivative of the expression w.r.t x
var a = v.D(t);
Console.WriteLine("x = " + x.ToLaTeX()); // we can output to LaTeX too
Console.WriteLine("v = " + v.ToLaTeX());
Console.WriteLine("a = " + a.ToLaTeX());

Variable q = KMath.V("q"), qdot = KMath.V("dot{q}"), m = KMath.C("m"), k = KMath.C("k");
// variable names should be written as you'd like to see them in latex; however, to increase readability, names like dot{x} and bar{x} will be escaped automatically into \dot{x} and \bar{x}
var L = 0.5 * m * qdot.Pow(2) - 0.5 * k * q.Pow(2); // lagrangian, 0.5 m qdot^2 - 0.5 k q^2
var dLdq = L.PD(q); // KyaniteExpression.PD(x) takes the partial derivative of the expression w.r.t x
var dLdqdot = L.PD(qdot);
var el = dLdq - dLdqdot.D(t); // Euler-lagrange equation
el = el.ESub(new() { [KMath.D(qdot, t)] = KMath.V("ddot{q}") }).Simplify();
// Kyanite allows you to subsitute expressions using ESub; here we swap derivative out for a variable
// To go the other way, and swap a variable out for an expression, use VSub
// You can also simplify expressions with .Simplify
Console.WriteLine("L = " + L.ToLaTeX());
Console.WriteLine("EL = " + el.ToLaTeX() + " = 0");

Variable p = KMath.V("p"), e = KMath.C("e");
var H = -p * p.Log(e) - (1 - p) * (1 - p).Log(e); // Kyanite currently includes .Power, .Sin, .Cos, .Tan, and .Log
var dHdp = H.D(p); // first derivative, simplifies automatically
Console.WriteLine("H = " + H.ToLaTeX());
Console.WriteLine(@"\frac{dH}{dp} = " + dHdp.ToLaTeX());
Console.WriteLine(@"p = 0.5 \implies \frac{dH}{dp} = " + dHdp.At(new() { ["p"] = 0.5, ["e"] = Math.E })); // .At evaluates an expression using the variable map given
```
{% endcode %}

For more detail on any specific method, check out the API reference.
