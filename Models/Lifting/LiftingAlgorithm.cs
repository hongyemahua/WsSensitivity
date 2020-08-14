using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WsSensitivity.Models;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace AlgorithmReconstruct
{
    public struct Upanddown
    {
        public int[] result_i;
        public int[] vi;
        public int[] mi;
        public int Data_validity_determination;
        public double μ0_final;
        public double σ0_final;
        public double G;
        public double H;
        public int n;
        public double Sigma_mu;
        public double Sigma_sigma;
        public double A;
        public double B;
        public double M;
        public double b;
        public double p;
    }

    public struct MultigroupTest
    {
        public double μ0_final;
        public double σ0_final;
        public double Sigma_mu;
        public double Sigma_sigma;
        public double prec01;
        public double prec999;
        public double rpse01;
        public double rpse999;
    }
    public class LiftingAlgorithm
    {
        public LiftingAlgorithm(LiftingMethodStandardSelection liftingMethodStandardSelection,LiftingDistributionSelection liftingDistributionSelection)
        {
            LiftingMethodStandardSelection = liftingMethodStandardSelection;
            LiftingDistributionSelection = liftingDistributionSelection;
        }

        public LiftingDistributionSelection LiftingDistributionSelection { get; set; }
        public LiftingMethodStandardSelection LiftingMethodStandardSelection { get; set; }

        //求标准刺激量
        public double GetStandardStimulus(double sq) => LiftingMethodStandardSelection.InverseProcessValue(sq);

        //响应&&计算
        public Upanddown GetReturn(double[] xArray, int[] vArray, double x0, double d,out double z,double reso,out double z1)
        {
            Upanddown upanddown = new Upanddown();
            x0 = LiftingMethodStandardSelection.InverseProcessValue(x0);
            updownMethod.upanddown_getz(xArray.Length, xArray, vArray, x0, d, out z);
            LiftingDistributionSelection.GetUpanddown(xArray,vArray,x0,d,ref upanddown);
            upanddown.μ0_final = LiftingMethodStandardSelection.GetAvgValue(upanddown.μ0_final);
            pub_function.resolution_getReso(upanddown.G, 0.000001, out upanddown.G);
            pub_function.resolution_getReso(upanddown.H, 0.000001, out upanddown.H);
            pub_function.resolution_getReso(upanddown.A, 0.000001, out upanddown.A);
            pub_function.resolution_getReso(upanddown.B, 0.000001, out upanddown.B);
            pub_function.resolution_getReso(upanddown.M, 0.000001, out upanddown.M);
            pub_function.resolution_getReso(upanddown.b, 0.000001, out upanddown.b);
            pub_function.resolution_getReso(SysParam.p, 0.000001, out upanddown.p);
            GetZ1(z, reso,out z1);
            return upanddown;
        }

        //台阶数
        public int StepsNumber(double[] xArray,int[] vArray)
        {
            int tjs;
            updownMethod.upanddown_tjs(xArray.Length - 1,xArray,vArray,out tjs);
            return tjs;
        }

        public void GetZ1(double z, double reso, out double z1) => pub_function.resolution_getReso(LiftingMethodStandardSelection.ProcessValue(z), reso, out z1);

        //
        public double[] GetPrec(double μ0_final,double σ0_final)
        {
            double[] prec = new double[2];
            prec[0] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(μ0_final) + LiftingDistributionSelection.DistributionProcess() * σ0_final);
            prec[1] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(μ0_final) - LiftingDistributionSelection.DistributionProcess() * σ0_final);
            return prec;
        }

        public double[] ResponsePointStandardError(double Sigma_mu,double Sigma_sigma)
        {
            double[] rpse = new double[2];
            rpse[0] = pub_function.resolution_getReso(Math.Sqrt(Math.Pow(Sigma_mu, 2) + Math.Pow(Sigma_sigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(0.001), 2)),0.000001);
            rpse[1] = pub_function.resolution_getReso(Math.Sqrt(Math.Pow(Sigma_mu, 2) + Math.Pow(Sigma_sigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(0.999), 2)), 0.000001);
            return rpse;
        }

        //不分组有效性校验
        public bool ValidityCheck(int[] vArray,Upanddown upanddown)
        {
            int availability_number;
            updownMethod.get_availability_number(vArray.Length,vArray,out availability_number);
            if (availability_number == 30)
                return upanddown.Data_validity_determination == 1 ? true : false;
            return true;
        }

        //响应点计算
        public double ResponsePointCalculation(double fq,double fsimga,double favg)
        {
            double rpc = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + LiftingDistributionSelection.QValue(fq) * fsimga);
            pub_function.resolution_getReso(rpc,0.000001,out rpc);
            return rpc;
        }

        //响应概率计算
        public double ResponseProbabilityCalculation(double fq,double favg,double fsigma)
        {
            double rpc = LiftingDistributionSelection.PValue(LiftingMethodStandardSelection.InverseProcessValue(fq),LiftingMethodStandardSelection.InverseProcessValue(favg),fsigma);
            pub_function.resolution_getReso(rpc,0.000001,out rpc);
            return rpc;
        }

        public IntervalEstimation SingleSideEstimation(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel,double favg, double fsigma) => LiftingDistributionSelection.IntervalDistribution(xArray, vArray, reponseProbability, 2 * confidenceLevel - 1, favg, fsigma);
        public IntervalEstimation DoubleSideEstimation(double[] xArray, int[] vArray, double reponseProbability, double confidenceLevel,double favg, double fsigma) => LiftingDistributionSelection.IntervalDistribution(xArray, vArray, reponseProbability, confidenceLevel, favg, fsigma);

        //响应概率区间估计
        public List<IntervalEstimation> ResponseProbabilityIntervalEstimated(double responseProbability, double confidenceLevel, double[] xArray, int[] vArray,double favg,double fsigma)
        {
            List<IntervalEstimation> intervalEstimations = new List<IntervalEstimation>();
            if (xArray.Length > 0)
                xArray = LiftingMethodStandardSelection.InverseProcessArray(xArray);
            var Single = SingleSideEstimation(xArray, vArray, responseProbability, confidenceLevel, LiftingMethodStandardSelection.InverseProcessValue(favg), fsigma);
            intervalEstimations.Add(GetIntervalEstimationValue(Single));
            var Double = DoubleSideEstimation(xArray, vArray, responseProbability, confidenceLevel, LiftingMethodStandardSelection.InverseProcessValue(favg), fsigma);
            intervalEstimations.Add(GetIntervalEstimationValue(Double));
            return intervalEstimations;
        }

        //响应点区间估计
        public List<IntervalEstimation> ResponsePointIntervalEstimated(double responseProbability, double confidenceLevel, double[] xArray, int[] vArray, double fq, double favg, double fsigma)
        {
            List<IntervalEstimation> intervalEstimations = new List<IntervalEstimation>();
            if (fq != 0)
            {
                responseProbability = LiftingDistributionSelection.PValue(LiftingMethodStandardSelection.InverseProcessValue(fq), LiftingMethodStandardSelection.InverseProcessValue(favg), fsigma);
                pub_function.resolution_getReso(responseProbability,0.000001,out responseProbability);
            }
            xArray = LiftingMethodStandardSelection.InverseProcessArray(xArray);
            var Single = SingleSideEstimation(xArray, vArray, responseProbability, confidenceLevel, LiftingMethodStandardSelection.InverseProcessValue(favg), fsigma);
            intervalEstimations.Add(GetIntervalEstimationValue(Single));
            var Double = DoubleSideEstimation(xArray, vArray, responseProbability, confidenceLevel, LiftingMethodStandardSelection.InverseProcessValue(favg), fsigma);
            intervalEstimations.Add(GetIntervalEstimationValue(Double));
            return intervalEstimations;
        }

        private IntervalEstimation GetIntervalEstimationValue(IntervalEstimation rt)
        {
            rt.Confidence.Down = LiftingMethodStandardSelection.ProcessValue(rt.Confidence.Down);
            rt.Confidence.Up = LiftingMethodStandardSelection.ProcessValue(rt.Confidence.Up);
            rt.Mu.Down = LiftingMethodStandardSelection.ProcessValue(rt.Mu.Down);
            rt.Mu.Up = LiftingMethodStandardSelection.ProcessValue(rt.Mu.Up);
            return rt;
        }

        //方差函数响应概率区间
        public double[] VarianceFunctionResponseProbabilityIntervalEstimated(double responseProbability, double confidenceLevel,int textNumber,double favg,double fsigma,double fsigmaavg,double fsigmasigma)
        {
            double[] ie = new double[8];
            double Tfw = updownMethod.get_t_shuangce(confidenceLevel, textNumber - 1);
            double Tfw_dance = updownMethod.get_t_dance(confidenceLevel, textNumber - 1);
            if (responseProbability != 0)
            {
                ie = GetDoubleArray(Tfw, Tfw_dance, responseProbability, favg, fsigma, fsigmaavg, fsigmasigma);
            }
            return ie;
        }

        //
        public double[] VarianceFunctionResponsePointIntervalEstimated(double confidenceLevel, int textNumber, double fq, double favg, double fsigma, double fsigmaavg, double fsigmasigma)
        {
            double[] ie = new double[8];
            double Tfw = updownMethod.get_t_shuangce(confidenceLevel, textNumber - 1);
            double Tfw_dance = updownMethod.get_t_dance(confidenceLevel, textNumber - 1);
            if (fq != 0)
            {
                fq = LiftingDistributionSelection.PValue(LiftingMethodStandardSelection.InverseProcessValue(fq), LiftingMethodStandardSelection.InverseProcessValue(favg), fsigma);
                ie = GetDoubleArray(Tfw,Tfw_dance,fq,favg,fsigma,fsigmaavg,fsigmasigma);
            }
            return ie;
        }

        private double[] GetDoubleArray(double Tfw,double Tfw_dance, double fq, double favg, double fsigma, double fsigmaavg, double fsigmasigma)
        {
            double[] ie = new double[8];
            ie[0] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + LiftingDistributionSelection.QValue(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(fq), 2)));
            ie[1] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + LiftingDistributionSelection.QValue(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(fq), 2)));
            ie[2] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + LiftingDistributionSelection.QValue(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(fq), 2)));
            ie[3] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + LiftingDistributionSelection.QValue(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(fq), 2)));
            ie[4] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + Tfw * fsigmaavg);
            ie[5] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) - Tfw * fsigmaavg);
            ie[6] = fsigma + Tfw * fsigmasigma;
            ie[7] = fsigma - Tfw * fsigmasigma;
            return ie;
        }

        //拟然比法区间计算绘图
        public SideReturnData QuasiLikelihoodRatioMethod(double Y_Ceiling, double Y_LowerLimit, int Y_PartitionNumber, double ConfidenceLevel, double favg, double fsigma, double[] xArray, int[] vArray, int intervalChoose)
        {
            var sideReturnData = GetSideReturnDataValue(Y_Ceiling, Y_LowerLimit, Y_PartitionNumber);
            for (int i = 0; i < sideReturnData.responseProbability.Length; i++)
            {
                IntervalEstimation ie = new IntervalEstimation();
                if (intervalChoose == 0)
                {
                    ie = SingleSideEstimation(xArray, vArray, sideReturnData.responseProbability[i], ConfidenceLevel,favg ,fsigma );
                }
                else
                {
                    ie = DoubleSideEstimation(xArray, vArray, sideReturnData.responseProbability[i], ConfidenceLevel,favg,fsigma);
                }
                sideReturnData.Y_LowerLimits[i] = ie.Confidence.Down;
                sideReturnData.Y_Ceilings[i] = ie.Confidence.Up;
                double fq = sideReturnData.responseProbability[i];
                sideReturnData.responsePoints[i] = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(favg) + (LiftingDistributionSelection.QValue(fq) * fsigma));
            }
            return sideReturnData;
        }

        //方差函数法区间计算
        public SideReturnData VarianceFunctionMethod(double Y_Ceiling, double Y_LowerLimit, int Y_PartitionNumber, double ConfidenceLevel, double favg, double fsigma,int intervalChoose, int textNumber,double fsigmaavg,double fsigmasigma)
        {
            var sideReturnData = GetSideReturnDataValue(Y_Ceiling, Y_LowerLimit, Y_PartitionNumber);
            for (int i = 0; i < sideReturnData.responseProbability.Length; i++)
            {
                double[] vfrpie = VarianceFunctionResponseProbabilityIntervalEstimated(sideReturnData.responseProbability[i], ConfidenceLevel, textNumber, favg, fsigma, fsigmaavg, fsigmasigma);
                sideReturnData.responsePoints[i] = favg + LiftingDistributionSelection.QValue(sideReturnData.responseProbability[i]) * fsigma;
                if (intervalChoose == 1)
                {
                    sideReturnData.Y_Ceilings[i] = vfrpie[0];
                    sideReturnData.Y_LowerLimits[i] = vfrpie[1];
                }
                else
                {
                    sideReturnData.Y_Ceilings[i] = vfrpie[2];
                    sideReturnData.Y_LowerLimits[i] = vfrpie[3];
                }
            }
            return sideReturnData;
        }

        private SideReturnData GetSideReturnDataValue(double Y_Ceiling, double Y_LowerLimit, int Y_PartitionNumber)
        {
            SideReturnData sideReturnData = new SideReturnData();
            double Y_ScaleLength = (LiftingDistributionSelection.QValue(Y_Ceiling) - LiftingDistributionSelection.QValue(Y_LowerLimit)) / Y_PartitionNumber;
            sideReturnData.responseProbability = new double[Y_PartitionNumber + 1];
            for (int i = 0; i <= Y_PartitionNumber; i++)
            {
                if (i == 0)
                    sideReturnData.responseProbability[i] = Y_LowerLimit;
                else
                    sideReturnData.responseProbability[i] = LiftingDistributionSelection.PValue(LiftingDistributionSelection.QValue(Y_LowerLimit) + i * Y_ScaleLength, 0, 1);
            }
            sideReturnData.Y_Ceilings = new double[sideReturnData.responseProbability.Length];
            sideReturnData.Y_LowerLimits = new double[sideReturnData.responseProbability.Length];
            sideReturnData.responsePoints = new double[sideReturnData.responseProbability.Length];
            return sideReturnData;
        }

        //计算多组试验结果
        public MultigroupTest MultigroupTestResult(int[] nj,double[] Gj,double[] Hj,double[] muj,double[] sigmaj)
        {
            int nfinal = 0;
            var multigroupTest = get_multiGroup_result(nj,muj,sigmaj,Gj,Hj, out nfinal);
            double f001 = Math.Sqrt(Math.Pow(multigroupTest.Sigma_mu, 2) + Math.Pow(multigroupTest.Sigma_sigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(0.001),2));
            pub_function.resolution_getReso(f001, 0.000001,out multigroupTest.prec01);
            double f999 = Math.Sqrt(Math.Pow(multigroupTest.Sigma_mu, 2) + Math.Pow(multigroupTest.Sigma_sigma, 2) * Math.Pow(LiftingDistributionSelection.QValue(0.999), 2));
            pub_function.resolution_getReso(f999, 0.000001, out multigroupTest.prec999);
            double p001 = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(multigroupTest.μ0_final) - LiftingDistributionSelection.DistributionProcess() * multigroupTest.σ0_final);
            pub_function.resolution_getReso(p001,0.000001,out multigroupTest.rpse01);
            double p999 = LiftingMethodStandardSelection.ProcessValue(LiftingMethodStandardSelection.InverseProcessValue(multigroupTest.μ0_final) + LiftingDistributionSelection.DistributionProcess() * multigroupTest.σ0_final);
            pub_function.resolution_getReso(p999,0.000001,out multigroupTest.rpse999);
            return multigroupTest;

        }

        private MultigroupTest get_multiGroup_result(int[] n, double[] mu, double[] sigma, double[] G, double[] H, out int N)
        {
            MultigroupTest multigroupTest = new MultigroupTest();
            double mu_final_zi = 0;
            double mu_final_mu = 0;
            double sigma_final_zi = 0;
            double sigma_final_mu = 0;
            int sumN = 0;
            double[] mu1 = new double[n.Length];
            for (int w = 0;w<n.Length;w++)
            {
                mu1[w] = LiftingMethodStandardSelection.InverseProcessValue(mu[w]);
                mu_final_zi = mu_final_zi + (n[w] * mu1[w]) / Math.Pow(G[w],2);
                mu_final_mu = mu_final_mu + n[w] / Math.Pow(G[w], 2);
                sigma_final_zi = sigma_final_zi + (n[w] * sigma[w]) / Math.Pow(H[w], 2);
                sigma_final_mu = sigma_final_mu + n[w] / Math.Pow(H[w], 2);
                sumN = sumN + n[w];
            }
            multigroupTest.μ0_final = mu_final_zi / mu_final_mu;
            multigroupTest.σ0_final = sigma_final_zi / sigma_final_mu;
            multigroupTest.Sigma_mu = multigroupTest.μ0_final / Math.Sqrt(mu_final_mu);
            multigroupTest.Sigma_sigma = multigroupTest.σ0_final / Math.Sqrt(sigma_final_mu);
            N = sumN;
            multigroupTest.μ0_final = LiftingMethodStandardSelection.ProcessValue(multigroupTest.μ0_final);
            return multigroupTest;
        }

        public string DistributionNameAndMethodStandardName()
        {
            return LiftingDistributionSelection.Distribution() + LiftingMethodStandardSelection.MethodStandard();
        }
    }
}
