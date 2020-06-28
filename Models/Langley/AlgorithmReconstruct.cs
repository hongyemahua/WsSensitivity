using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class AlgorithmReconstruct
    {
        public struct OutputParameters
        {
            public double μ0_final;
            public double σ0_final;
            public double Maxf;
            public double Mins;
            public double varmu;
            public double varsigma;
            public double covmusigma;
        }

        public struct SideReturnData
        {
            public double[] responseProbability;
            public double[] Y_Ceilings;
            public double[] Y_LowerLimits;
            public double[] responsePoints;
        }

        public class LangleyAlgorithm
        {
            public LangleyAlgorithm(LangleyDistributionSelection distributionSelection, LangleyMethodStandardSelection standardSelection)
            {
                DistributionSelection = distributionSelection;
                StandardSelection = standardSelection;
            }

            public LangleyDistributionSelection DistributionSelection { get; set; }
            public LangleyMethodStandardSelection StandardSelection { get; set; }
            public OutputParameters GetResult(double[] xArray, int[] vArray)
            {
                OutputParameters outputParameters = new OutputParameters();
                double[] X = new double[xArray.Length];
                int[] V = new int[xArray.Length];
                for (int w = 0; w < xArray.Length; w++)
                {
                    X[w] = xArray[w];
                    V[w] = vArray[w];
                }
                outputParameters = DistributionSelection.DotDistribution(X, V);
                outputParameters.μ0_final = StandardSelection.ProcessValue(outputParameters.μ0_final);
                return outputParameters;
            }
            public List<IntervalEstimation> ResponseProbabilityIntervalEstimate(double[] x, int[] v, double reponseProbability, double confidenceLevel)
            {
                List<IntervalEstimation> intervalEstimations = new List<IntervalEstimation>();
                x = StandardSelection.InverseProcessArray(x);
                var Single = SingleSideEstimation(x, v, reponseProbability, confidenceLevel);
                intervalEstimations.Add(GetIntervalEstimationValue(Single));
                var Double = DoubleSideEstimation(x, v, reponseProbability, confidenceLevel);
                intervalEstimations.Add(GetIntervalEstimationValue(Double));
                return intervalEstimations;
            }

            private IntervalEstimation GetIntervalEstimationValue(IntervalEstimation rt)
            {
                rt.Confidence.Down = StandardSelection.ProcessValue(rt.Confidence.Down);
                rt.Confidence.Up = StandardSelection.ProcessValue(rt.Confidence.Up);
                rt.Mu.Down = StandardSelection.ProcessValue(rt.Mu.Down);
                rt.Mu.Up = StandardSelection.ProcessValue(rt.Mu.Up);
                return rt;
            }


            public List<IntervalEstimation> ResponsePointIntervalEstimate(double[] x, int[] v, double reponseProbability, double confidenceLevel, double fq, double favg, double fsigma)
            {
                List<IntervalEstimation> intervalEstimations = new List<IntervalEstimation>();
                if (fq != 0)
                    reponseProbability = DistributionSelection.PointIntervalDistribution(StandardSelection.InverseProcessValue(fq), StandardSelection.InverseProcessValue(favg), fsigma);
                x = StandardSelection.InverseProcessArray(x);
                var Single = SingleSideEstimation(x, v, reponseProbability, confidenceLevel);
                intervalEstimations.Add(GetIntervalEstimationValue(Single));
                var Double = DoubleSideEstimation(x, v, reponseProbability, confidenceLevel);
                intervalEstimations.Add(GetIntervalEstimationValue(Double));
                return intervalEstimations;
            }

            public double ResponsePointCalculate(double fq, double favg, double fsigma) => StandardSelection.ProcessValue(StandardSelection.InverseProcessValue(favg) + DistributionSelection.ResponsePointDistribution(fq, fsigma));

            public double ResponseProbabilityCalculate(double fq, double favg, double fsigma) => DistributionSelection.PointIntervalDistribution(StandardSelection.InverseProcessValue(fq), StandardSelection.InverseProcessValue(favg), fsigma);
            public IntervalEstimation SingleSideEstimation(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel) => DistributionSelection.IntervalDistribution(xArray, vArray, reponseProbability, 2 * confidenceLevel - 1);
            public IntervalEstimation DoubleSideEstimation(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel) => DistributionSelection.IntervalDistribution(xArray, vArray, reponseProbability, confidenceLevel);

            public SideReturnData ViewData(double Y_Ceiling, double Y_LowerLimit, int Y_PartitionNumber, double ConfidenceLevel, double average, double variance, double[] xArray, int[] vArray)
            {
                SideReturnData sideReturnData = new SideReturnData();
                double Y_ScaleLength = (pub_function.qnorm(Y_Ceiling) - pub_function.qnorm(Y_LowerLimit)) / Y_PartitionNumber;
                sideReturnData.responseProbability = new double[Y_PartitionNumber + 1];
                for (int i = 0; i <= Y_PartitionNumber; i++)
                {
                    if (i == 0)
                        sideReturnData.responseProbability[i] = Y_LowerLimit;
                    else
                        sideReturnData.responseProbability[i] = pub_function.pnorm(pub_function.qnorm(Y_LowerLimit) + i * Y_ScaleLength, 0, 1);
                }
                sideReturnData.Y_Ceilings = new double[sideReturnData.responseProbability.Length];
                sideReturnData.Y_LowerLimits = new double[sideReturnData.responseProbability.Length];
                sideReturnData.responsePoints = new double[sideReturnData.responseProbability.Length];
                double favg = average;
                double fsigma = variance;
                for (int i = 0; i < sideReturnData.responseProbability.Length; i++)
                {
                    var rt = DoubleSideEstimation(xArray, vArray, sideReturnData.responseProbability[i], ConfidenceLevel);
                    ////sideReturnData = SideSelection.GetValue(rt, i, sideReturnData);
                    //if ("单侧")
                    //{
                    //    var ie = SingleSideEstimation(xArray, vArray, sideReturnData.responseProbability[i], ConfidenceLevel);
                    //    sideReturnData.Y_LowerLimits[i] = ie.Confidence.Down;
                    //    sideReturnData.Y_Ceilings[i] = ie.Confidence.Up;
                    //}
                    //else
                    //{
                    //    var ie = DoubleSideEstimation(xArray, vArray, sideReturnData.responseProbability[i], ConfidenceLevel);
                    //    sideReturnData.Y_LowerLimits[i] = ie.Confidence.Down;
                    //    sideReturnData.Y_Ceilings[i] = ie.Confidence.Up;
                    //}
                    double fq = sideReturnData.responseProbability[i];
                    sideReturnData.responsePoints[i] = StandardSelection.ProcessValue(StandardSelection.InverseProcessValue(favg) + pub_function.qnorm(fq) * fsigma);
                }
                return sideReturnData;
            }

            public double CalculateStimulusQuantity(double[] xArray,int[] vArray, double maxStimulusQuantity, double minStimulusQuantity,double reso)
            {
                pub_function.Lanlie_getz(xArray.Length, xArray, vArray, maxStimulusQuantity, minStimulusQuantity, out double z);
                pub_function.resolution_getReso(StandardSelection.ProcessValue(z), reso, out z);
                return z;
            }
        }
    }
}