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
- JIT compilation of expressions

The library is designed for quick usage and compact scripts, with QoL utilities already supplied. 
The following is an example of a Kyanite script:
```csharp
using Lavelle.Kyanite;

Variable x0 = C("x_0"), v0 = C("v_0"), g = C("g"), t = V("t");
var x = x0 + v0 * t + 0.5 * g * t.Sq();
var v = x.D(t);
var a = v.D(t);
Console.WriteLine("x = " + x.ToLaTeX());
Console.WriteLine("v = " + v.ToLaTeX());
Console.WriteLine("a = " + a.ToLaTeX());

Variable q = V("q"), qdot = V("dot{q}"), m = C("m"), k = C("k");
var L = 0.5 * m * qdot.Sq() - 0.5 * k * q.Sq();
var dLdq = L.PD(q);
var dLdqdot = L.PD(qdot);
KyaniteExpression el = dLdq - dLdqdot.D(t);
el = el.ESub(new() { [D(qdot, t)] = V("ddot{q}") }).Simplify();
Console.WriteLine("L = " + L.ToLaTeX());
Console.WriteLine("EL = " + el.ToLaTeX() + " = 0");

Variable p = V("p"), e = C("e");
var H = -p * p.Log(e) - (1 - p) * (1 - p).Log(e);
var dHdp = H.D(p);
Console.WriteLine("H = " + H.ToLaTeX());
Console.WriteLine(@"\frac{dH}{dp} = " + dHdp.ToLaTeX());
Console.WriteLine(@"p = 0.5 \implies \frac{dH}{dp} = " + dHdp.At(new() { ["p"] = 0.5, ["e"] = Math.E }));

Variable N = C("N"), N0 = C("N_0"), lambda = C("lambda");
var decay = N0 * Exp(-lambda * t);
var solution = Solve(decay, N, t);
Console.WriteLine("N = " + decay.ToLaTeX());
Console.WriteLine(solution.L.ToLaTeX() + " = " + solution.R.ToLaTeX());

Variable n = C("n"), R = C("R"), T = C("T"), V = V("V");
var P = n[R][T] / V;
var W = P.Int(V);
Console.WriteLine("P = " + P.ToLaTeX());
Console.WriteLine("W = " + W.ToLaTeX());

Variable y = V("y");
var f = y.Cos() - y;
var dfdy = f.D(y);
var compiledF = f.Compile();
var compiledDerivative = dfdy.Compile();
var root = 1.0;
for (int i = 0; i < 100; i++)
    root -= compiledF(new() { ["y"] = root }) / compiledDerivative(new() { ["y"] = root });
Console.WriteLine("f(y) = " + f.ToLaTeX());
Console.WriteLine(@"\frac{df}{dy} = " + dfdy.ToLaTeX());
Console.WriteLine("root = " + root);
```
You can view Kyanite's docs [here](https://lavelle.gitbook.io/kyanite-documentation/).
## TODOs
- LaTeX cleanup
- Expand method
- Summation and tensors
- Factoring & polynomial standard form