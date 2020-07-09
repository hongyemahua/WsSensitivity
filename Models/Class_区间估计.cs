using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Models
{
    class Class_区间估计
    {
        public static OutputParameters NormalInterval_estimation_渐进法_方差(int xArrayLength, double[] xArray, int[] vArray, OutputParameters outputParameters)
        {
            if ((outputParameters.σ0_final < 0.00000000000000000000000001) && (outputParameters.σ0_final > -0.00000000000000000000000001))
            {
                outputParameters.varmu = double.NaN;
                outputParameters.varsigma = double.NaN;
                outputParameters.covmusigma = double.NaN;
            }
            else
            {
                double I11, I00, I01, J = 0;
                double z0 = 0;
                int j, i;
                i = xArrayLength;
                I11 = 0; I00 = 0; I01 = 0;
                for (j = 0; j < i; j++)
                {
                    z0 = (xArray[j] - outputParameters.μ0_final) / outputParameters.σ0_final;
                    J = (pub_function.pnorm(z0, 0, 1) * (1 - pub_function.pnorm(z0, 0, 1)) * Math.Pow(outputParameters.σ0_final, 2));
                    if (J == 0)
                        continue;
                    J = Math.Pow(pub_function.dnorm_normpdf(z0, 0, 1), 2) / J;
                    I11 = I11 + J * Math.Pow(z0, 2);
                    I00 = I00 + J;
                    I01 = I01 + J * z0;
                }
                outputParameters.varmu = I11 / (I00 * I11 - I01 * I01);
                outputParameters.varsigma = I00 / (I00 * I11 - I01 * I01);
                outputParameters.covmusigma = -I01 / (I00 * I11 - I01 * I01);
            }
            return outputParameters;
        }


        public static OutputParameters LogisticInterval_estimation_渐进法_方差(int xArrayLength, double[] xArray, int[] vArray, OutputParameters outputParameters)
        {
            if ((outputParameters.σ0_final < 0.00000000000000000000000001) && (outputParameters.σ0_final > -0.00000000000000000000000001))
            {
                outputParameters.varmu = double.NaN;
                outputParameters.varsigma = double.NaN;
                outputParameters.covmusigma = double.NaN;
            }
            else
            {
                double I11, I00, I01, J = 0;
                double z0 = 0;
                int j, i;
                double σ0_final = outputParameters.σ0_final * (Math.Pow(3, 0.5) / Math.PI);
                i = xArrayLength;
                I11 = 0; I00 = 0; I01 = 0;
                for (j = 0; j < i; j++)
                {
                    z0 = (xArray[j] - outputParameters.μ0_final) / σ0_final;
                    J = (pub_function.plogis(z0, 0, 1) * (1 - pub_function.plogis(z0, 0, 1)) * Math.Pow(σ0_final, 2));
                    if (J == 0)
                        continue;
                    J = Math.Pow(pub_function.dlogis(z0, 0, 1), 2) / J;
                    I11 = I11 + J * Math.Pow(z0, 2);
                    I00 = I00 + J;
                    I01 = I01 + J * z0;
                }
                outputParameters.varmu = I11 / (I00 * I11 - I01 * I01);
                outputParameters.varsigma = (I00 / (I00 * I11 - I01 * I01)) * Math.Pow(Math.PI, 2) / 3;
                outputParameters.covmusigma = (-I01 / (I00 * I11 - I01 * I01)) * Math.PI / Math.Pow(3, 0.5);
            }
            return outputParameters;
        }


        //渐进法求解感度试验数据均值和标准差的方差及协方差,计算给定的xArrayLength位置的方差
        public static void interval_estimation_渐进法_方差(int xArrayLength, double[] xArray, int[] vArray, OutputParameters outputParameters, string data_type, out double varmu, out double varsigma, out double covmusigma)
        {
            if ((outputParameters.σ0_final < 0.00000000000000000000000001) && (outputParameters.σ0_final > -0.00000000000000000000000001))
            {
                varmu = double.NaN;
                varsigma = double.NaN;
                covmusigma = double.NaN;
            }
            else
            {

                if (data_type == "normal")
                {
                    double I11, I00, I01, J = 0;
                    double z0 = 0;
                    int j, i;
                    i = xArrayLength;
                    I11 = 0; I00 = 0; I01 = 0;
                    for (j = 0; j < i; j++)
                    {
                        z0 = (xArray[j] - outputParameters.μ0_final) / outputParameters.σ0_final;
                        J = (pub_function.pnorm(z0, 0, 1) * (1 - pub_function.pnorm(z0, 0, 1)) * Math.Pow(outputParameters.σ0_final, 2));
                        if (J == 0)
                            continue;
                        J = Math.Pow(pub_function.dnorm_normpdf(z0, 0, 1), 2) / J;
                        I11 = I11 + J * Math.Pow(z0, 2);
                        I00 = I00 + J;
                        I01 = I01 + J * z0;
                    }
                    varmu = I11 / (I00 * I11 - I01 * I01);
                    varsigma = I00 / (I00 * I11 - I01 * I01);
                    covmusigma = -I01 / (I00 * I11 - I01 * I01);
                }
                else
                {
                    double I11, I00, I01, J = 0;
                    double z0 = 0;
                    int j, i;
                    outputParameters.σ0_final = outputParameters.σ0_final * (Math.Pow(3, 0.5) / Math.PI);
                    i = xArrayLength;
                    I11 = 0; I00 = 0; I01 = 0;
                    for (j = 0; j < i; j++)
                    {
                        z0 = (xArray[j] - outputParameters.μ0_final) / outputParameters.σ0_final;
                        J = (pub_function.plogis(z0, 0, 1) * (1 - pub_function.plogis(z0, 0, 1)) * Math.Pow(outputParameters.σ0_final, 2));
                        if (J == 0)
                            continue;
                        J = Math.Pow(pub_function.dlogis(z0, 0, 1), 2) / J;
                        I11 = I11 + J * Math.Pow(z0, 2);
                        I00 = I00 + J;
                        I01 = I01 + J * z0;
                    }
                    varmu = I11 / (I00 * I11 - I01 * I01);
                    varsigma = (I00 / (I00 * I11 - I01 * I01)) * Math.Pow(Math.PI, 2) / 3;
                    covmusigma = (-I01 / (I00 * I11 - I01 * I01)) * Math.PI / Math.Pow(3, 0.5);

                }
            }
        }
    }
}