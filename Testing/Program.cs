using Lavelle.Kyanite;

Variable x = KMath.V("x"), y = KMath.V("y");
Function fx = KMath.F("f", [x]), fy = KMath.F("f", [y]);
KyaniteExpression sum = fx + fy;
sum = sum.Apply("f", parameters => parameters[0] * 2 + 1);
Console.WriteLine(sum.ToLaTeX());