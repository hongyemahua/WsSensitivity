using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Models.LangleyAndDOptimize
{
    public class DoptimizationAlgorithm : LangleyAlgorithm
    {
        public DoptimizationAlgorithm(IDoptimizationDistributionSelection distributionSelection, LangleyMethodStandardSelection standardSelection) : base(distributionSelection, standardSelection)
        {
            DopDistributionSelection = distributionSelection;
            StandardSelection = standardSelection;
        }

        public IDoptimizationDistributionSelection DopDistributionSelection { get; set; }

        private void GetDistribution(double[] xArray, int[] vArray, double mumin, double mumax, double reso,ref OutputParameters outputParameters, out double z, double sigmaguess)
        {
            mumin = StandardSelection.InverseProcessValue(mumin);
            mumax = StandardSelection.InverseProcessValue(mumax);
            xArray = StandardSelection.InverseProcessArray(xArray);
            if (xArray.Length == 0)
            {
                pub_function.resolution_getReso((mumin + mumax) / 2, reso, out z);
                outputParameters.μ0_final = 0;
                outputParameters.σ0_final = 0;
            }
            else
            {
                int sum = 0;
                for (int j = 0; j < xArray.Length; j++)
                    sum += vArray[j];
                if (sum == 0 || sum == xArray.Length)
                {
                    z = xArray[xArray.Length - 1];
                    double[] sumi = { (mumin + z) / 2, z - 2 * sigmaguess, 2 * z - xArray[0] };
                    double[] sum0 = { (mumax + z) / 2, z + 2 * sigmaguess, 2 * z - xArray[0] };
                    if (sum == xArray.Length)
                        pub_function.resolution_getReso(pub_function.MIN_getMin(sumi), reso, out z);
                    else
                        pub_function.resolution_getReso(pub_function.MAX_getMax(sum0), reso, out z);
                    outputParameters.μ0_final = 0;
                    outputParameters.σ0_final = 0;
                }
                else
                {
                    double[] xArray_change = new double[xArray.Length];
                    int[] vArray_change = new int[xArray.Length];
                    int ij;
                    for (ij = 0; ij < xArray.Length; ij++)
                    {
                        xArray_change[ij] = xArray[ij];
                        vArray_change[ij] = vArray[ij];
                    }
                    outputParameters.Maxf = pub_function.MAX_getMax(pub_function.getF(xArray_change, vArray_change));
                    outputParameters.Mins = pub_function.MIN_getMin(pub_function.S_getS(xArray_change, vArray_change));
                    double[] FZ = pub_function.getF(xArray_change, vArray_change);//失败的刺激量
                    double[] SZ = pub_function.S_getS(xArray_change, vArray_change);//成功的刺激量
                    double Diff = outputParameters.Mins - outputParameters.Maxf;
                    if (DopDistributionSelection.IsValueSize(Diff, sigmaguess))
                    {
                        pub_function.resolution_getReso((outputParameters.Mins + outputParameters.Maxf) / 2, reso, out z);
                        outputParameters.μ0_final = z;
                        outputParameters.σ0_final = 0;
                    }
                    else
                    {
                        if (outputParameters.Mins > outputParameters.Maxf)
                        {
                            outputParameters.μ0_final = (outputParameters.Mins + outputParameters.Maxf) / 2;
                            outputParameters.σ0_final = 0;
                            pub_function.resolution_getReso(DopDistributionSelection.Fisher_getZ_new(xArray_change, vArray_change, outputParameters.μ0_final, sigmaguess), reso, out z);
                            outputParameters.sigmaguess = 0.8 * sigmaguess;
                        }
                        else
                        {
                            outputParameters = DistributionSelection.MLS_getMLS(xArray_change, vArray_change);
                            outputParameters.sigmaguess = sigmaguess;
                            pub_function.resolution_getReso(DopDistributionSelection.Fisher_getZ_new(xArray_change, vArray_change, outputParameters.μ0_final, outputParameters.σ0_final), reso, out z);
                        }
                    }
                }
            }
            DistributionSelection.Interval_estimation(xArray, vArray, ref outputParameters);
        }

        public OutputParameters GetResult(double[] xArray, int[] vArray, double mumin, double mumax, double reso, out double z, double sigmaguess)
        {
            OutputParameters outputParameters = new OutputParameters();
            outputParameters.sigmaguess = sigmaguess;
            GetDistribution(xArray, vArray, mumin, mumax, 0.000000000000001,ref outputParameters, out z, sigmaguess);
            pub_function.resolution_getReso(StandardSelection.ProcessValue(z), reso, out z);
            outputParameters.μ0_final = StandardSelection.GetAvgValue(outputParameters.μ0_final);
            return outputParameters;
        }
    }
}