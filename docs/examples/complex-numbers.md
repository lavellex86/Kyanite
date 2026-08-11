# Complex Numbers

{% code overflow="wrap" %}
```csharp
using Lavelle.Kyanite;

Variable a = KMath.V("a"), b = KMath.V("b"), c = KMath.V("c"), d = KMath.V("d");
// we can represent a complex number using a constant i
var z1 = a + b["i"]; // strings convert to constant variables
var z2 = c + d["i"]; // this is using the juxtaposition multiplication convention; a[b] = a * b
var sum = z1 + z2; // we can add and multiply easily
var product = z1[z2].Expand(); // expand so it does out the multiplication

// define Im(z) = partial z / partial i, the coefficient
KyaniteExpression Im(KyaniteExpression z) => z.PD("i");
// Re is then just z - Im(z) i
KyaniteExpression Re(KyaniteExpression z) => z - Im(z)["i"];

var conjugate = Re(z1) - Im(z1)["i"]; // we can take the conjugate
var modulus = (Re(z1).Sq() + Im(z1).Sq()).Sqrt(); // and the modulus

// throughout all of these, we'll end up with i^2 terms
// to get rid of them, we just sub i^2 = -1
var env = new Dictionary<KyaniteExpression, KyaniteExpression>() { [KMath.C("i").Sq()] = -1 };
var simplifiedProduct = product.Sub(env);

Console.WriteLine("z1 = " + z1.ToLaTeX());
Console.WriteLine("z2 = " + z2.ToLaTeX());
Console.WriteLine("sum = " + sum.ToLaTeX());
Console.WriteLine("product = " + simplifiedProduct.ToLaTeX());
Console.WriteLine("conjugate = " + conjugate.ToLaTeX());
Console.WriteLine("modulus = " + modulus.ToLaTeX());
```
{% endcode %}
