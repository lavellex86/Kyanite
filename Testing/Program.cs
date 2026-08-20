using Lavelle.Kyanite;

Variable eps = KMath.V("epsilon"), x_K = KMath.V("x^k"), x_k = KMath.V("x_k"), x_i = KMath.V("x_i"), x_M = KMath.V("x^m"), x_N = KMath.V("x^n");
Variable delta_ik = KMath.V("delta_{ik}");
Variable magx = KMath.V(@"|\mathbf{x}|"), magdotx = KMath.V(@"|\dot{\mathbf{x}}|");

KyaniteExpression alpha_i = x_i["kappa"] / magx[magdotx.Sq()];
KyaniteExpression alpha_K = x_K["kappa"] / magx[magdotx.Sq()];

List<Variable> xlist = [x_K, x_k, x_i, x_M, x_N, magx];

var Lambda_ik = delta_ik - eps[alpha_i.PD(x_k, xlist)];
var Omega_KMN = eps[alpha_K.PD(x_M, xlist).PD(x_N, xlist)];

var l = Lambda_ik[Omega_KMN].Expand().Sub(new() { [eps.Sq()] = 0 });

Console.WriteLine(l.ToLaTeX());
