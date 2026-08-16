using Lavelle.Kyanite;

Variable Omega_ijk = KMath.V("Omega_{ijk}"), Omega_IJK = KMath.V("Omega^{ijk}");
Variable Lambda_ij = KMath.V("Lambda_{ij}"), Lambda_IJ = KMath.V("Lambda^{ij}");
Variable Theta_ij = KMath.V("Theta_{ij}"), Theta_IJ = KMath.V("Theta^{ij}");
Variable Theta_NA = KMath.V("Theta^{na}"), Omega_NAB = KMath.V("Omega^{nab}");
Variable Omega_KLL = KMath.V("Omega^{kll}");
Variable x_a = KMath.V("x_a"), x_b = KMath.V("x_b");
Variable Lambda_IN = KMath.V("Lambda^{in}"), Lambda_AJ = KMath.V("Lambda^{aj}");
Variable delta_iN = KMath.V("delta_{i}^{n}"), delta_jA = KMath.V("delta_{j}^{a}"), delta_kB = KMath.V("delta_{k}^{b}");
Variable delta_nI = KMath.V("delta_{n}^{i}"), delta_aJ = KMath.V("delta_{a}^{j}"), delta_bK = KMath.V("delta_{b}^{k}");
Variable delta_nK = KMath.V("delta_{n}^{k}"), delta_aL = KMath.V("delta_{a}^{l}"), delta_bL = KMath.V("delta_{b}^{l}");
var subs = new Dictionary<KyaniteExpression, KyaniteExpression>()
{
    [KMath.PD(Lambda_IJ, Theta_NA)] = -Lambda_IN[Lambda_AJ],
    [KMath.PD(Omega_ijk, Omega_NAB)] = delta_iN[delta_jA][delta_kB],
    [KMath.PD(Omega_IJK, Omega_NAB)] = delta_nI[delta_aJ][delta_bK],
    [KMath.PD(Omega_KLL, Omega_NAB)] = delta_nK[delta_aL][delta_bL],
    [KMath.PD(Theta_ij, Theta_NA)] = delta_iN[delta_jA],
    [KMath.PD(Theta_IJ, Theta_NA)] = delta_nI[delta_aJ],
};

var list = new List<Variable>([Omega_ijk, Omega_IJK, Lambda_ij, Lambda_IJ, Theta_NA, Omega_KLL, Lambda_IN, Lambda_AJ, Theta_ij, Theta_IJ]);
var thetaList = new List<Variable>([Lambda_ij, Lambda_IJ, Theta_ij, Theta_IJ]);
var omegaList = new List<Variable>([Omega_ijk, Omega_IJK, Omega_KLL]);

KyaniteExpression L = Lambda_IJ[Omega_ijk][Omega_KLL];
var el1 = L.PD(Theta_NA, thetaList).PD(x_a, list).Sub(subs).EvalPDs(list);
var el2 = L.PD(Omega_NAB, omegaList).PD(x_a, list).PD(x_b, list).Sub(subs).EvalPDs(list);
Console.WriteLine("L = " + L.Expand().ToLaTeX());
Console.WriteLine("EL1 = " + el1.Expand().ToLaTeX());
Console.WriteLine("EL2 = " + el2.Expand().ToLaTeX());