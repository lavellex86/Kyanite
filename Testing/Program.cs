using Lavelle.Kyanite;

Variable x0 = KMath.C("x_0"), v0 = KMath.C("v_0"), g = KMath.C("g"), t = KMath.V("t");
var x = x0 + v0 * t + 0.5 * g * t.Power(2);
Console.WriteLine(x.ToLaTeX());

var v = x.D(t);
Console.WriteLine(v.ToLaTeX());

var a = v.D(t);
Console.WriteLine(a.ToLaTeX());