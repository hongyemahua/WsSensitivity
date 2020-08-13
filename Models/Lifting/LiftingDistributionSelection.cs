using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WsSensitivity.Models;

namespace AlgorithmReconstruct
{
    public interface LiftingDistributionSelection
    {
        void GetUpanddown(double[] xArray, int[] vArray, double x0, double d,ref Upanddown upanddown);

        double DistributionProcess();
        double QValue(double value);
        double PValue(double fq,double favg,double fsigma);
        IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel,double favg,double fsigma);
        string Distribution();
    }

    public abstract class LiftingNormal : LiftingDistributionSelection
    {
        public abstract string Distribution();

        public double DistributionProcess() => 3.090232;

        public abstract void GetUpanddown(double[] xArray, int[] vArray, double x0, double d,ref Upanddown upanddown);

        public IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel, double favg, double fsigma)
        {
            MLR_polar.Likelihood_Ratio_Polar(xArray, vArray, "normal", favg, fsigma, reponseProbability, confidenceLevel, out var final_result);
            return IntervalEstimation.Parse(final_result);
        }

        public double PValue(double fq, double favg, double fsigma) => pub_function.pnorm(fq,favg,fsigma);

        public double QValue(double value) => pub_function.qnorm(value);
    }

    public class LiftingLogistic : LiftingDistributionSelection
    {
        public string Distribution() => "逻辑斯蒂分布";

        public double DistributionProcess() => 6.906755;

        public void GetUpanddown(double[] xArray, int[] vArray, double x0, double d,ref Upanddown upanddown)
        {
            updownMethod.upanddown_logis(xArray.Length, xArray, vArray, x0, d, out upanddown.Data_validity_determination, out upanddown.μ0_final, out upanddown.σ0_final, out upanddown.G, out upanddown.H, out upanddown.n, out upanddown.Sigma_mu, out upanddown.Sigma_sigma, out upanddown.result_i, out upanddown.vi, out upanddown.mi, out upanddown.A, out upanddown.B, out upanddown.M, out upanddown.b);
        }

        public IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel, double favg, double fsigma)
        {
            MLR_polar.Likelihood_Ratio_Polar(xArray, vArray, "logistic", favg, fsigma, reponseProbability, confidenceLevel, out var final_result);
            return IntervalEstimation.Parse(final_result);
        }

        public double PValue(double fq, double favg, double fsigma) => pub_function.plogis(fq,favg,fsigma);

        public double QValue(double value) => pub_function.qlogis(value);
    }

    //传统法
    public class TraditionalMethod : LiftingNormal
    {
        public override string Distribution() => "正态分布/传统法";

        public override void GetUpanddown(double[] xArray, int[] vArray, double x0, double d,ref Upanddown upanddown)
        {
            updownMethod.upanddown_norm_tradition(xArray.Length, xArray, vArray, x0, d,out upanddown.Data_validity_determination, out upanddown.μ0_final, out upanddown.σ0_final,out upanddown.G, out upanddown.H, out upanddown.n, out upanddown.Sigma_mu, out upanddown.Sigma_sigma,out upanddown.result_i, out upanddown.vi, out upanddown.mi, out upanddown.A, out upanddown.B, out upanddown.M, out upanddown.b);
        }
    }

    //结合法
    public class CombinationMethod : LiftingNormal
    {
        public override string Distribution() => "正态分布/结合法";

        public override void GetUpanddown(double[] xArray,int[] vArray,double x0,double d,ref Upanddown upanddown)
        {
            updownMethod.upanddown_norm_conbination(xArray.Length, xArray, vArray, x0, d, out upanddown.Data_validity_determination, out upanddown.μ0_final, out upanddown.σ0_final, out upanddown.G, out upanddown.H, out upanddown.n, out upanddown.Sigma_mu, out upanddown.Sigma_sigma, out upanddown.result_i, out upanddown.vi, out upanddown.mi, out upanddown.A, out upanddown.B, out upanddown.M, out upanddown.b);
        }

    }

    //修正法
    class AmendmentMethod : LiftingNormal
    {
        public override string Distribution() => "正态分布/修正法";

        public override void GetUpanddown(double[] xArray, int[] vArray, double x0, double d,ref Upanddown upanddown)
        {
            updownMethod.upanddown_norm_correct(xArray.Length, xArray, vArray, x0, d, out upanddown.Data_validity_determination, out upanddown.μ0_final, out upanddown.σ0_final, out upanddown.G, out upanddown.H, out upanddown.n, out upanddown.Sigma_mu, out upanddown.Sigma_sigma, out upanddown.result_i, out upanddown.vi, out upanddown.mi, out upanddown.A, out upanddown.B, out upanddown.M, out upanddown.b);
        }
    }
}
