using Lavelle.Kyanite;

Variable x0 = KMath.C("x_0"), v0 = KMath.C("v_0"), g = KMath.C("g"), t = KMath.V("t");
var x = x0 + v0 * t + 0.5 * g * t.Pow(2);
var v = x.D(t);
var a = v.D(t);
Console.WriteLine("x = " + x.ToLaTeX());
Console.WriteLine("v = " + v.ToLaTeX());
Console.WriteLine("a = " + a.ToLaTeX());

Variable q = KMath.V("q"), qdot = KMath.V("dot{q}"), m = KMath.C("m"), k = KMath.C("k");
var L = 0.5 * m * qdot.Pow(2) - 0.5 * k * q.Pow(2);
var dLdq = L.PD(q);
var dLdqdot = L.PD(qdot);
var el = dLdq - dLdqdot.D(t);
el = el.ESub(new() { [KMath.D(qdot, t)] = KMath.V("ddot{q}") }).Simplify();
Console.WriteLine("L = " + L.ToLaTeX());
Console.WriteLine("EL = " + el.ToLaTeX() + " = 0");

Variable p = KMath.V("p"), e = KMath.C("e");
var H = -p * p.Log(e) - (1 - p) * (1 - p).Log(e);
var dHdp = H.D(p);
Console.WriteLine("H = " + H.ToLaTeX());
Console.WriteLine(@"\frac{dH}{dp} = " + dHdp.ToLaTeX());
Console.WriteLine(@"p = 0.5 \implies \frac{dH}{dp} = " + dHdp.At(new() { ["p"] = 0.5, ["e"] = Math.E }));