using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models.LangleyAndDOptimize
{
    public interface IDoptimizationDistributionSelection : LangleyDistributionSelection
    {
        double Fisher_getZ_new(double[] xArray_change, int[] vArray_change, double μ0_final, double value);
        bool IsValueSize(double Diff, double sigmaguess);
    }

    class Dop_Noraml : Normal, IDoptimizationDistributionSelection
    {
        public double Fisher_getZ_new(double[] xArray_change, int[] vArray_change, double μ0_final, double value) => pub_function.norm_fisher_getZ_new(xArray_change, vArray_change, μ0_final, value);

        public bool IsValueSize(double Diff, double sigmaguess) => (Diff - sigmaguess) > Math.Pow(10, -8);
    }

    class Dop_Logistic : Logistic, IDoptimizationDistributionSelection
    {
        public double Fisher_getZ_new(double[] xArray_change, int[] vArray_change, double μ0_final, double value) => pub_function.logit_fisher_getZ_new(xArray_change, vArray_change, μ0_final, value);

        public bool IsValueSize(double Diff, double sigmaguess) => Diff > sigmaguess;
    }
}