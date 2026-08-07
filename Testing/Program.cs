using Lavelle.Kyanite;
using static Lavelle.Kyanite.KMath;

var ex = 1 + 2 * Sin(2 * V("x") + 3 * V("x").Power(2));
Console.WriteLine(ex.ToLaTeX());
var dex = ex.D("x").Simplify();
Console.WriteLine(dex.ToLaTeX());