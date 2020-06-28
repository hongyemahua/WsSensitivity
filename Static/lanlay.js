var r;
var vArray = [];
vArray.push(vArray);
var xArray = [];
xArray.push(xArray);
var xArrayLength = 0;
function getMode(mumin, mumax) {
    var reso = document.getElementById("Instrument").value;
    var getModes = document.getElementById("layuiadmin-form-useradmin").sex;
    for (var i = 0; i < getModes.length; i++) {
        if (getModes[i].checked) {
            sex = getModes[i].value
        }
    }
    if (sex == "0") {
        Lanlie_getz(xArrayLength, xArray, vArray, mumin, mumax);
        resolution_getReso(r, reso);
    }
    if (sex == "1") {
        var mumin = Math.log(mumin);
        var mumax = Math.log(mumax);
        Lanlie_getz(xArrayLength, xArray, vArray, mumin, mumax);
        resolution_getReso(Math.exp(r), reso);
    }
    if (sex == "2") {
        var mumin = Math.log10(mumin);
        var mumax = Math.log10(mumax);
        Lanlie_getz(xArrayLength, xArray, vArray, mumin, mumax);
        resolution_getReso(Math.pow(10, r), reso);
    }
    if (sex == "3") {
        var pow = document.getElementById("getPow").value;
        var mumin = Math.pow(mumin, pow);
        var mumax = Math.pow(mumax, pow);
        Lanlie_getz(xArrayLength, xArray, vArray, mumin, mumax);
        resolution_getReso(Math.pow(r, 1 / 2), reso);
    }
    xArrayLength++;
    return r;
}
function Lanlie_getz(xArrayLength, xArray, vArray, mumin, mumax) {
    var z;
    if (xArrayLength == 0) {
        z = (mumin + mumax) / 2;
    } else {
        var xarray1;
        var j, k1, k2;
        k1 = 0;
        k2 = 0;
        for (j = xArrayLength - 1; j >= 0; j--) {
            if (vArray[j] == 0)
                k1 = k1 + 1;
            else
                k2 = k2 + 1;
            if (k1 == k2)
                break;
        } if (j >= 0) {
            xarray1 = xArray[j];
        } else {
            if (vArray[xArrayLength - 1] == 0) {
                xarray1 = mumax;
            } else { xarray1 = mumin; }
        }
        var X = [];
        X.push(xArrayLength);
        var V = [];
        V.push(xArrayLength);
        for (var w = 0; w < xArrayLength; w++) {
            X[w] = xArray[w];
            V[w] = vArray[w];
        }
        var z = 0.5 * (parseFloat(xArray[xArrayLength - 1]) + parseFloat(xarray1));
    }
    return r = z;
}
function resolution_getReso(r, reso) {
    if (reso == 0) {
        var z_reso = Math.Round(r / 0.000001) * 0.000001;
        return;
    }
    var z_reso = Math.round(r / reso) * reso;
    return r = z_reso;
}
