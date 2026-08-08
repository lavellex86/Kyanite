using Lavelle.Kyanite;

Variable x = KMath.V("x"), y = KMath.V("y"), k = KMath.C("k"), c = KMath.C("c");
var l = k * x.Sq() + y * x + c;
var r = KMath.N(0);
var solution = KMath.Solve(l, r, x);
Console.WriteLine(l.ToLaTeX() + " = " + r.ToLaTeX());
Console.WriteLine(solution.L.ToLaTeX() + " = " + solution.R.ToLaTeX());