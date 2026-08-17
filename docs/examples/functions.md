# Functions & Surgicals

{% code overflow="wrap" %}
```csharp
using Lavelle.Kyanite;
// functions allow us to apply rules to expressions and swap them out
Variable x = KMath.V("x"), y = KMath.V("y");
Function fx = KMath.F("f", [x]), fy = KMath.F("f", [y]); // same function f, different parameters
KyaniteExpression sum = fx + fy;
sum = sum.Apply("f", parameters => parameters[0] * 2 + 1); // f(args) = args[0] * 2 + 1
Console.WriteLine(sum.ToLaTeX());

// more generally, functions let variables carry metadata
Function i = KMath.F("i", [3]); // for example, tracking the range of an index
// you can always use .Walk to surgically enter an expression
Function v_i = KMath.F("v_i", [i]), u_i = KMath.F("u_i", [i]);
KyaniteExpression result = v_i.Sq() * u_i + u_i.Sq();
result = result.Walk(x => x.SE(i) ? KMath.F("j", [3]) : x); // replace all i with j manually  using semantic equals
```
{% endcode %}
