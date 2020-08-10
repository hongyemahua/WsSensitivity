using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Models
{
    public interface LangleyDistributionSelection
    {
        IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel);
        double PointIntervalDistribution(double fq, double favg, double fsigma);
        double CorrectionDistribution(int count);
        double QnormAndQlogisDistribution(double value);
        double PrecValues();
        string DistributionSelection();
        OutputParameters MLS_getMLS(double[] xArray_change, int[] vArray_change);
        void Interval_estimation(double[] xArray, int[] vArray, ref OutputParameters outputParameters);
    }

    public class Normal : LangleyDistributionSelection
    {
        public string DistributionSelection() => "正态分布";

        public double CorrectionDistribution(int count) => Langlie.get_langlie_sigma_norm_correct(count);

        public IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel)
        {
            var outputParameters = MLS_getMLS(xArray,vArray);
            MLR_polar.Likelihood_Ratio_Polar(xArray, vArray, "normal", outputParameters.μ0_final, outputParameters.σ0_final, reponseProbability, confidenceLevel, out var final_result);
            return IntervalEstimation.Parse(final_result);
        }

        public double PointIntervalDistribution(double fq, double favg, double fsigma) => pub_function.pnorm(fq, favg, fsigma);

        public double PrecValues() => 3.090232;

        public double QnormAndQlogisDistribution(double value) => pub_function.qnorm(value);

        public OutputParameters MLS_getMLS(double[] xArray_change, int[] vArray_change)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.norm_MLS_getMLS(xArray_change, vArray_change, out outputParameters.μ0_final, out outputParameters.σ0_final, out outputParameters.Maxf, out outputParameters.Mins);
            return outputParameters;
        }

        public void Interval_estimation(double[] xArray, int[] vArray, ref OutputParameters outputParameters) => Class_区间估计.NormalInterval_estimation_渐进法_方差(xArray.Length, xArray, vArray,ref outputParameters);
    }

    public class Logistic : LangleyDistributionSelection
    {
        public string DistributionSelection() => "逻辑斯蒂分布";

        public double CorrectionDistribution(int count) => Langlie.get_langlie_sigma_logis_correct(count);

        public IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel)
        {
            MLR_polar.Max_Likelihood_Estimate(xArray, vArray, "logistic", out var mu, out var sigma, out var L);
            MLR_polar.Likelihood_Ratio_Polar(xArray, vArray, "logistic", mu, sigma, reponseProbability, confidenceLevel, out var final_result);
            return IntervalEstimation.Parse(final_result);
        }

        public double PointIntervalDistribution(double fq, double favg, double fsigma) => pub_function.plogis(fq, favg, fsigma);

        public double PrecValues() => 6.906755;

        public double QnormAndQlogisDistribution(double value) => pub_function.qlogis(value);

        public OutputParameters MLS_getMLS(double[] xArray_change, int[] vArray_change)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.logit_MLS_getMLS(xArray_change, vArray_change, out outputParameters.μ0_final, out outputParameters.σ0_final, out outputParameters.Maxf, out outputParameters.Mins);
            return outputParameters;
        }

        public void Interval_estimation(double[] xArray, int[] vArray, ref OutputParameters outputParameters) => Class_区间估计.LogisticInterval_estimation_渐进法_方差(xArray.Length, xArray, vArray,ref outputParameters);
    }
}