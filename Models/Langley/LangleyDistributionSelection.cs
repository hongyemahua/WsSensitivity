using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Models
{
    public abstract class LangleyDistributionSelection
    {
        public abstract OutputParameters DotDistribution(double[] xArray, int[] vArray);
        public abstract IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel);
        public abstract double PointIntervalDistribution(double fq, double favg, double fsigma);
        public abstract double CorrectionDistribution(int count);
        public abstract double QnormAndQlogisDistribution(double value);
        public abstract double[] PrecValues(double value ,double fsigma);
        public abstract string DistributionSelection();
    }

    public class Normal : LangleyDistributionSelection
    {
        public override string DistributionSelection() => "正态分布";

        public override double CorrectionDistribution(int count)
        {
            return Langlie.get_langlie_sigma_norm_correct(count);
        }

        public override OutputParameters DotDistribution(double[] xArray, int[] vArray)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.norm_MLS_getMLS(xArray, vArray, out outputParameters.μ0_final, out outputParameters.σ0_final, out outputParameters.Maxf, out outputParameters.Mins);
            
            return Class_区间估计.NormalInterval_estimation_渐进法_方差(xArray.Length, xArray, vArray, outputParameters);
        }
        public override IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.norm_MLS_getMLS(xArray, vArray, out outputParameters.varmu, out outputParameters.varsigma, out outputParameters.Maxf, out outputParameters.Mins);

            MLR_polar.Likelihood_Ratio_Polar(xArray, vArray, "normal", outputParameters.varmu, outputParameters.varsigma, reponseProbability, confidenceLevel, out var final_result);
            IntervalEstimation ret = new IntervalEstimation();
            ret.Confidence.Down = final_result[5];
            ret.Confidence.Up = final_result[4];
            ret.Mu.Down = final_result[1];
            ret.Mu.Up = final_result[0];
            ret.Sigma.Down = final_result[3];
            ret.Sigma.Up = final_result[2];
            return ret;
        }

        public override double PointIntervalDistribution(double fq, double favg, double fsigma)
        {
            return pub_function.pnorm(fq, favg, fsigma);
        }

        public override double[] PrecValues(double value, double fsigma)
        {
            double[] ds = new double[2];
            ds[0] = value + 3.090232 * fsigma;
            ds[1] = value - 3.090232 * fsigma;
            return ds;
        }

        public override double QnormAndQlogisDistribution(double value)
        {
            return pub_function.qnorm(value);
        }
    }

    public class Logistic : LangleyDistributionSelection
    {
        public LangleyMethodStandardSelection Standard { get; set; }

        public override string DistributionSelection() => "逻辑斯蒂分布";

        public override double CorrectionDistribution(int count)
        {
            return Langlie.get_langlie_sigma_logis_correct(count);
        }

        public override OutputParameters DotDistribution(double[] xArray, int[] vArray)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.logit_MLS_getMLS(xArray, vArray, out outputParameters.μ0_final, out outputParameters.σ0_final, out outputParameters.Maxf, out outputParameters.Mins);
           
            return Class_区间估计.LogisticInterval_estimation_渐进法_方差(xArray.Length, xArray, vArray, outputParameters);
        }

        public override IntervalEstimation IntervalDistribution(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel)
        {
            MLR_polar.Max_Likelihood_Estimate(xArray, vArray, "logistic", out var mu, out var sigma, out var L);
            MLR_polar.Likelihood_Ratio_Polar(xArray, vArray, "logistic", mu, sigma, reponseProbability, confidenceLevel, out var final_result);
            IntervalEstimation ret = new IntervalEstimation();
            ret.Confidence.Down = final_result[5];
            ret.Confidence.Up = final_result[4];
            ret.Mu.Down = final_result[1];
            ret.Mu.Up = final_result[0];
            ret.Sigma.Down = final_result[3];
            ret.Sigma.Up = final_result[2];
            return ret;
        }

        public override double PointIntervalDistribution(double fq, double favg, double fsigma)
        {
            return pub_function.plogis(fq, favg, fsigma);
        }

        public override double[] PrecValues(double value, double fsigma)
        {
            double[] ds = new double[2];
            ds[0] = value + 6.906755 * fsigma;
            ds[1] = value - 6.906755 * fsigma;
            return ds;
        }

        public override double QnormAndQlogisDistribution(double value)
        {
            return pub_function.qlogis(value);
        }
    }
}