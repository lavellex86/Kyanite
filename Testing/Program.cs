using Lavelle.Kyanite;

Function i = KMath.F("i", [3]);
Function v_i = KMath.F("v_i", [i]), u_i = KMath.F("u_i", [i]);
KyaniteExpression result = v_i.Sq() * u_i + u_i.Sq();
result = result.WalkDown(x => x.SE(i) ? KMath.F("j", [3]) : x);