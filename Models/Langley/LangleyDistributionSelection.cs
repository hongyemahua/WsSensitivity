﻿using System;
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
        public abstract double ResponsePointDistribution(double fq, double fsigma);
    }

    public class Normal : LangleyDistributionSelection
    {
        public LangleyMethodStandardSelection Standard { get; set; }

        public override OutputParameters DotDistribution(double[] xArray, int[] vArray)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.norm_MLS_getMLS(xArray, vArray, out outputParameters.μ0_final, out outputParameters.σ0_final, out outputParameters.Maxf, out outputParameters.Mins);
            Class_区间估计.NormalInterval_estimation_渐进法_方差(xArray.Length, xArray, vArray, outputParameters);
            return outputParameters;
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

        public override double ResponsePointDistribution(double fq, double fsigma)
        {
            return pub_function.qnorm(fq) * fsigma;
        }
    }

    public class Logistic : LangleyDistributionSelection
    {
        public LangleyMethodStandardSelection Standard { get; set; }

        public override OutputParameters DotDistribution(double[] xArray, int[] vArray)
        {
            OutputParameters outputParameters = new OutputParameters();
            pub_function.logit_MLS_getMLS(xArray, vArray, out outputParameters.μ0_final, out outputParameters.σ0_final, out outputParameters.Maxf, out outputParameters.Mins);
            Class_区间估计.LogisticInterval_estimation_渐进法_方差(xArray.Length, xArray, vArray, outputParameters);
            return outputParameters;
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

        public override double ResponsePointDistribution(double fq, double fsigma)
        {
            return pub_function.qlogis(fq) * fsigma;
        }
    }
}