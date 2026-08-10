using Lavelle.Kyanite;

Variable x = KMath.V("x");
var f = x * 2;
var action = f.Compile();
Console.WriteLine(action(new() { ["x"] = 1 }));