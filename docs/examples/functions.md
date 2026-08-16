# Functions

{% code overflow="wrap" %}
```csharp
using Lavelle.Kyanite;
// functions allow us to apply rules to expressions and swap them out
Variable x = KMath.V("x"), y = KMath.V("y");
Function fx = KMath.F("f", [x]), fy = KMath.F("f", [y]); // same function f, different parameters
KyaniteExpression sum = fx + fy;
sum = sum.Apply("f", parameters => parameters[0] * 2 + 1); // f(args) = args[0] * 2 + 1
Console.WriteLine(sum.ToLaTeX());
```
{% endcode %}
