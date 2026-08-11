using Lavelle.Kyanite;

Variable g_munu = KMath.V("g_{mu nu}"), xdot_MU = KMath.C("dot{x}^{mu}"), xdot_NU = KMath.C("dot{x}^{nu}"), x_MU = KMath.C("x^{mu}"), tau = KMath.C("tau");
// as a convention, raised indices can be represented in uppercase and lowered indices in lowercase

// Lagrangian for free particle
var L = g_munu[xdot_MU][xdot_NU];
var dLdx = L.D(x_MU);
var dLdxdot = L.D(xdot_MU); // using derivative to capture all derivatives, later we'll sub out the ones we don't want
var ddxdot_dtaudL = dLdxdot.D(tau);

var EL = dLdx - ddxdot_dtaudL;
var subbedEL = EL.Sub(new() { [KMath.D(g_munu, xdot_MU)] = 0 }); // g_munu is constnat w.r.t velocity, 
Console.WriteLine("EL = " + subbedEL.ToLaTeX() + " = 0"); // final result
// the genius of Einstein summation is that it turns what was a multiply (ab) into a sum(A^i B_i)
// no new nodes are needed for differential geometry

// we can also do more classical things
Variable x_i = KMath.V("x_i"), x_j = KMath.V("x_j");
var f_i = x_i.Sq() + 3;
var jacobian = f_i.D(x_j);
var subbedJacobian = jacobian.Sub(new() { [KMath.D(x_i, x_j)] = "delta_{ij}" }); // subsitute the identity
Console.WriteLine("J_ij = " + subbedJacobian.ToLaTeX());