# Kyanite
An ergonomic, effective C# CAS library.
## What is Kyanite?
Kyanite is a CAS (Computer Algebra System) library that handles:
- symbolic differentiation
- semantic equality checks
- LaTeX formatting
- expression simplification
- subsitution
- solving equations
- symbolic integration
- lambda compilation of expressions
- surgical editing of expression trees

The library is designed for quick usage and compact scripts, with QoL utilities already supplied. 
The following is an example of a Kyanite script:
```csharp
using Lavelle.Kyanite;

Variable x0 = KMath.C("x_0"), v0 = KMath.C("v_0"), g = KMath.C("g"), t = KMath.V("t");
// the KMath class has three utilities for creating symbols:
// - V, which creates a new variable
// - C, which creates a new constant
// - D, which creates a new symbolic derivative
// - Int, which creates a new symbolic integral
// Variable inherits from the base class KyaniteExpression

var x = x0 + v0[t] + 0.5 * g[t.Sq()];
// KyaniteExpression objects have operators and methods attached, so expressions can be written easily
// this is x0 + v0 t + 0.5 g t^2
var v = x.D(t); // KyaniteExpression.D(x) takes the derivative of the expression w.r.t x
var a = v.D(t);
Console.WriteLine("x = " + x.ToLaTeX()); // we can output to LaTeX too
Console.WriteLine("v = " + v.ToLaTeX());
Console.WriteLine("a = " + a.ToLaTeX());

Variable q = KMath.V("q"), qdot = KMath.V("dot{q}"), m = KMath.C("m"), k = KMath.C("k");
// variable names should be written as you'd like to see them in latex; however, to increase readability, names like dot{x} and bar{x} will be escaped automatically into \dot{x} and \bar{x}
// greek letters will also be escaped, so you can write "mu_{nu}" instead of "\mu_{\nu}"
var L = 0.5 * m[qdot.Sq()] - 0.5 * k[q.Sq()]; // lagrangian, 0.5 m qdot^2 - 0.5 k q^2
var dLdq = L.PD(q); // KyaniteExpression.PD(x) takes the partial derivative of the expression w.r.t x
var dLdqdot = L.PD(qdot);
KyaniteExpression el = dLdq - dLdqdot.D(t); // Euler-lagrange equation
el = el.Sub(new() { [KMath.D(qdot, t)] = KMath.V("ddot{q}") }).Simplify();
// Kyanite allows you to subsitute expressions using Sub; here we swap derivative out for a variable
Console.WriteLine("L = " + L.ToLaTeX());
Console.WriteLine("EL = " + el.ToLaTeX() + " = 0");

Variable p = KMath.V("p");
var H = -p[p.Log("e")] - (1 - p)[(1 - p).Log("e")]; // strings are converted to constant variables
// Kyanite currently includes .Power, .Sin, .Cos, .Tan, and .Log
var dHdp = H.D(p); // first derivative
Console.WriteLine("H = " + H.ToLaTeX());
Console.WriteLine(@"\frac{dH}{dp} = " + dHdp.ToLaTeX());
Console.WriteLine(@"p = 0.5 \implies \frac{dH}{dp} = " + dHdp.At(new() { ["p"] = 0.5 })); // .At evaluates an expression using the variable name -> value map given

Variable N = KMath.C("N"), N0 = KMath.C("N_0"), lambda = KMath.C("lambda");
var decay = N0[KMath.Exp(-lambda * t)]; // exponential decay, N = N_0 e^{-lambda t}
var solution = KMath.Solve(decay, N, t); // solves, returning (lhs, rhs) in the form f(x) = g
// sometimes an expression is to complex to fully solve for x, so it'll reduce as far as possible and give you what it can
Console.WriteLine("N = " + decay.ToLaTeX());
Console.WriteLine(solution.L.ToLaTeX() + " = " + solution.R.ToLaTeX());

Variable n = KMath.C("n"), R = KMath.C("R"), T = KMath.C("T"), V = KMath.V("V");
var P = n[R][T] / V; // perfect gas, P = n R T / V
var W = P.Int(V); // we can integrate with .Int
Console.WriteLine("P = " + P.ToLaTeX());
Console.WriteLine("W = " + W.ToLaTeX());
// expressions simplify on .ToLaTeX
// to simplify manually, call .Simplify

Variable y = KMath.V("y");
var f = y.Cos() - y;
var dfdy = f.D(y);
var compiledF = f.Compile(); // .Compile turns an expression into a C# function
var compiledDerivative = dfdy.Compile(); // this means we can write a math expression and easily generate C# code 
var root = 1.0; // finding the roots with the compiled method
for (int i = 0; i < 100; i++) // unlike .At, which uses the symbolic tree, .Compile uses IL compilation
    root -= compiledF(new() { ["y"] = root }) / compiledDerivative(new() { ["y"] = root }); // this makes it fast in hot loops like this
// passing in arguments is done in the same way as .At- dictionary of variable names to doubles
Console.WriteLine("f(y) = " + f.ToLaTeX());
Console.WriteLine(@"\frac{df}{dy} = " + dfdy.ToLaTeX());
Console.WriteLine("root = " + root);
```
You can view Kyanite's docs [here](https://lavelle.gitbook.io/kyanite-documentation/).
## TODOs
- `.Collapse` and factoring
- Factoring