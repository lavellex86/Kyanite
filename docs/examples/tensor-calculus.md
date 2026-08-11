# Tensor Calculus

{% code overflow="wrap" %}
```csharp
using Lavelle.Kyanite;

Variable g_munu = KMath.V("g_{mu nu}"), xdot_MU = KMath.C("dot{x}^{mu}"), xdot_NU = KMath.C("dot{x}^{nu}"), x_MU = KMath.C("x^{mu}"), tau = KMath.C("tau");
// as a convention, raised indices can be represented in uppercase and lowered indices in lowercase

// Lagrangian for free particle
var L = g_munu[xdot_MU][xdot_NU];
var dLdx = L.D(x_MU);
var dLdxdot = L.D(xdot_MU); // using derivative to capture all derivatives, later we'll sub out the ones we don't want
var ddxdot_dtaudL = dLdxdot.D(tau);

var EL = dLdx - ddxdot_dtaudL;
var subbedEL = EL.Sub(new() { [KMath.D(g_munu, xdot_MU)] = 0 }); // g_munu is constnat w.r.t velocity, 
Console.WriteLine("EL = " + subbedEL.ToLaTeX() + " = 0"); // final result
```
{% endcode %}
