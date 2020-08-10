using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WsSensitivity.Controllers.UpDowunMethod.Functions;

namespace WsSensitivity.Controllers.UpDowunMethod
{
    public class UpDownData
    {
        public double z = 0;
        public int Data_validity_determination = 0;
        public double μ0_final = 0;
        public double σ0_final = 0;
        public double G = 0;
        public double H = 0;
        public double n = 0;
        public double Sigma_mu = 0;
        public double Sigma_sigma = 0;
        public int[] i;
        public int[] vi;
        public int[] mi;
        public double A = 0;
        public double B = 0;
        public double M = 0;
        public double b = 0;
        public double p = 0;

        public int N;
    }
    public abstract class UpDownIn
    {
        public Function Function { get; set; } = null;
    }
}