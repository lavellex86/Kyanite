using Lavelle.Kyanite;

Variable x = KMath.V("x"), y = KMath.V("y"), k = KMath.C("k");
var l = (2 * k * x).Sin().D(x) + k * y;
var r = (y * k).D(k);
var solution = KMath.Solve(l, r, x);
Console.WriteLine(l.ToLaTeX() + " = " + r.ToLaTeX());
Console.WriteLine(solution.L.ToLaTeX() + " = " + solution.R.ToLaTeX());