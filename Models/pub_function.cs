using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    class pub_function
    {

        /// 得到正态分布密度函数
        public static double dnorm_normpdf(double x, double mu, double sigma)
        {
            double xx;
            xx = (x - mu) / sigma;
            return 0.398942280401433 * Math.Exp(-xx * xx / 2) / sigma;
        }

        /// 得到逻辑斯蒂分布密度函数
        public static double dlogis(double x, double mu, double sigma)
        {
            x = (x - mu) / sigma;
            return Math.Exp(x) / Math.Pow((1 + Math.Exp(x)), 2) / sigma;
        }

        /// 得到逻辑斯蒂分布概率函数
        public static double plogis(double x, double mu, double sigma)
        {
            x = (x - mu) / sigma;
            return 1 / (1 + Math.Exp(-x));
        }

        //正态EM最大似然算法
        public static void getEM(double[] xArray, int[] vArray, out double μ0_final, out double σ0_final)
        {
            double μ0, σ0, param8, E, mu, sigma;
            double MaxF = MAX_getMax(pub_function.getF(xArray, vArray));
            double MinS = MIN_getMin(pub_function.S_getS(xArray, vArray));
            double[] FZ = pub_function.getF(xArray, vArray);//失败的刺激量
            double[] SZ = pub_function.S_getS(xArray, vArray);//成功的刺激量
            int Nm = pub_function.getNM(xArray, vArray);
            int times = 0;

            param8 = 8.0;
            E = 0.0000001;
            μ0_final = 0;
            σ0_final = 0;

            μ0 = 0.5 * (MaxF + MinS);
            σ0 = xArray.Length * (MaxF - MinS) / (param8 * (Nm + 2));
            mu = μ0;
            sigma = σ0;
            μ0 = 0;
            σ0 = 0;


            while (Math.Abs(μ0 - mu) + Math.Abs(σ0 - sigma) > E)
            {

                μ0 = mu;
                σ0 = sigma;
                EE(FZ, SZ, ref mu, ref sigma, FZ.Length, SZ.Length);
                times++;
                if (times > 500)
                {
                    mu = 0.5 * (MaxF + MinS);
                    sigma = 0;
                    goto continue1;
                };
            }
        continue1:
            μ0_final = mu;
            σ0_final = sigma;


        }
        public static void EE(double[] F, double[] S, ref double mu2, ref double sigma2, int j1, int j2)
        {
            double E1 = 0, E2 = 0, a, b, c, pi = 3.1415926535897932;
            double mu = mu2, sigma = sigma2;
            int j;
            c = (sigma2) * 1000;
            for (j = 0; j < j1; j++)
            {
                a = F[j]; b = a + c;
                E1 = E1 + (-2 * mu * sigma / Math.Sqrt(2 * pi) * (Math.Exp(-(b - mu) * (b - mu) / 2 / sigma / sigma)
                    - Math.Exp(-(a - mu) * (a - mu) / 2 / sigma / sigma))
                    - sigma * sigma / Math.Sqrt(2 * pi) * ((b - mu) / sigma * Math.Exp(-(b - mu) * (b - mu) / 2 / sigma / sigma)
                    - (a - mu) / sigma * Math.Exp(-(a - mu) * (a - mu) / 2 / sigma / sigma)) +
                    +(mu * mu + sigma * sigma) * (pub_function.pnorm(b, mu, sigma)
                    - pub_function.pnorm(a, mu, sigma))) / (1 - pub_function.pnorm(a, mu, sigma));

                E2 = E2 + (-sigma / Math.Sqrt(2 * pi) * (Math.Exp(-(b - mu) * (b - mu) / 2 / sigma / sigma)
                    - Math.Exp(-(a - mu) * (a - mu) / 2 / sigma / sigma)) +
                    mu * (pub_function.pnorm(b, mu, sigma) - pub_function.pnorm(a, mu, sigma))) / (1 - pub_function.pnorm(a, mu, sigma));
            }
            for (j = 0; j < j2; j++)
            {
                b = S[j]; a = b - c;
                E1 = E1 + (-2 * mu * sigma / Math.Sqrt(2 * pi) * (Math.Exp(-(b - mu) * (b - mu) / 2 / sigma / sigma)
                    - Math.Exp(-(a - mu) * (a - mu) / 2 / sigma / sigma))
                    - sigma * sigma / Math.Sqrt(2 * pi) * ((b - mu) / sigma * Math.Exp(-(b - mu) * (b - mu) / 2 / sigma / sigma)
                    - (a - mu) / sigma * Math.Exp(-(a - mu) * (a - mu) / 2 / sigma / sigma)) +
                    +(mu * mu + sigma * sigma) * (pub_function.pnorm(b, mu, sigma)
                    - pub_function.pnorm(a, mu, sigma))) / (pub_function.pnorm(b, mu, sigma));

                E2 = E2 + (-sigma / Math.Sqrt(2 * pi) * (Math.Exp(-(b - mu) * (b - mu) / 2 / sigma / sigma)
                    - Math.Exp(-(a - mu) * (a - mu) / 2 / sigma / sigma)) +
                    mu * (pub_function.pnorm(b, mu, sigma) - pub_function.pnorm(a, mu, sigma))) / (pub_function.pnorm(b, mu, sigma));
            }
            mu2 = E2 / (j1 + j2);
            sigma2 = Math.Pow(E1 / (j1 + j2) - Math.Pow(mu2, 2), 0.5);
        }


        ///失败所对应的刺激量
        public static double[] getF(double[] xArray, int[] vArray)
        {
            int iii = 0;
            foreach (int v0 in vArray)
            {
                if (v0 == 0)
                {
                    iii++;
                }
            }
            int[] result = new int[iii];
            int ii = 0;
            int i = 0;
            int j = 0;
            foreach (int v0 in vArray)
            {
                if (v0 == 0)
                {
                    result[ii] = i;
                    ii++;
                }
                i++;
            }
            double[] F = new double[result.Length];
            foreach (int lo in result)
            {
                F[j] = xArray[lo];
                j++;
            }
            return F;
        }

        //逻辑斯蒂信息矩阵取得下一刺激量
        public static double logit_fisher_getZ(double[] xArray, int[] vArray, double mu, double sigma)
        {
            double I11, I00, I01, C0, N0, J;
            double z0, z;
            int j, i;
            sigma = sigma * (Math.Pow(3, 0.5) / Math.PI);
            i = xArray.Length;
            I11 = 0; I00 = 0; I01 = 0;
            for (j = 0; j < i; j++)
            {
                z0 = xArray[j];
                J = Math.Pow(pub_function.dlogis(z0, mu, sigma), 2) / (pub_function.plogis(z0, mu, sigma)
                        * (1 - pub_function.plogis(z0, mu, sigma)));
                if (J == 0)
                    continue;
                I11 = I11 + J * Math.Pow(z0, 2);
                I00 = I00 + J;
                I01 = I01 + J * z0;
            }

            N0 = -100000000;
            C0 = 0;
            double diff;
            if (mu > 100)
                diff = 1;
            else
                diff = 0.01;
            //半区域搜索最大值
            //if (vArray[vArray.Length - 1] ==0)
            //{
            //for (z0 = xArray[xArray.Length - 1]; z0 < (mu + Math.Max(mu, 3 * sigma)); z0 = z0 + diff)
            //{
            //    J = Math.Pow(pub_function.dlogis(z0, mu, sigma), 2) / (pub_function.plogis(z0, mu, sigma)
            //        * (1 - pub_function.plogis(z0, mu, sigma)));
            //    if (J > 10000000000 || J < -100000000000)
            //        break;
            //    if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
            //    {
            //        N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
            //        C0 = z0;
            //    }
            //}
            //}
            //else
            //{
            //    for (z0 = xArray[xArray.Length - 1]; z0 > (mu - Math.Max(mu, 3 * sigma)); z0 = z0 - diff)
            //    {
            //        J = Math.Pow(pub_function.dlogis(z0, mu, sigma), 2) / (pub_function.plogis(z0, mu, sigma)
            //            * (1 - pub_function.plogis(z0, mu, sigma)));
            //        if (J > 10000000000 || J < -100000000000)
            //            break;
            //        if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
            //        {
            //            N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
            //            C0 = z0;
            //        }
            //    }
            //}
            //全区域搜索最大值
            for (z0 = (mu - Math.Max(mu, 3 * sigma)); z0 < (mu + Math.Max(mu, 3 * sigma)); z0 = z0 + diff)
            {
                J = Math.Pow(pub_function.dlogis(z0, mu, sigma), 2) / (pub_function.plogis(z0, mu, sigma)
                    * (1 - pub_function.plogis(z0, mu, sigma)));
                if (J > 10000000000 || J < -100000000000)
                    break;
                if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
                {
                    N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
                    C0 = z0;
                }
            }
            double reso = diff;
            double z1;
            z0 = C0;
        case1:
            for (z1 = z0 - reso; z1 < (z0 + reso); z1 = z1 + reso / 10)
            {
                J = Math.Pow(pub_function.dlogis(z1, mu, sigma), 2) / (pub_function.plogis(z1, mu, sigma)
                        * (1 - pub_function.plogis(z1, mu, sigma)));
                if (J > 10000000000 || J < -100000000000)
                    break;
                if (N0 < (I00 + J) * (I11 + J * Math.Pow(z1, 2)) - Math.Pow((I01 + J * z1), 2))
                {
                    N0 = (I00 + J) * (I11 + J * Math.Pow(z1, 2)) - Math.Pow((I01 + J * z1), 2);
                    C0 = z1;
                }
            }
            z0 = C0;
            reso = reso / 10;
            if (reso > 0.0000000001)
            {
                goto case1;
            }
            else
            {
                z = C0;
            }
            return z;
        }

        //逻辑斯蒂最大似然估计综合算法
        public static void logit_MLS_getMLS(double[] xArray, int[] vArray, out double μ0_final, out double σ0_final, out double Maxf, out double Mins)

        {
            int sum, j;
            double MaxF = 0;
            double MinS = 0;
            sum = 0;
            for (j = 0; j < xArray.Length; j++)
                sum += vArray[j];
            if (sum == 0 || sum == xArray.Length)
            {
                μ0_final = 0;
                σ0_final = 0;
            }
            else
            {

                double L;
                //MLR_polar.Max_Likelihood_Estimate(xArray, vArray, "logistic", out μ0_final, out σ0_final, out L);
                int weishu;
                double[] xArray_temp;
                max_number_change(xArray, out weishu, out xArray_temp);
                MLR_polar.Max_Likelihood_Estimate(xArray_temp, vArray, "logistic", out μ0_final, out σ0_final, out L);
                μ0_final = μ0_final * Math.Pow(10, weishu);
                σ0_final = σ0_final * Math.Pow(10, weishu);


                MaxF = pub_function.MAX_getMax(pub_function.getF(xArray, vArray));
                MinS = pub_function.MIN_getMin(pub_function.S_getS(xArray, vArray));

            }
            Maxf = MaxF;
            Mins = MinS;
            /*
             int sum,j;
             double iiii = 0;
             double MaxF = 0;
             double MinS = 0;
             sum=0;
             for (j = 0; j < xArray.Length; j++)
                 sum +=vArray[j];
             if (sum==0||sum==xArray.Length)
             {
                 μ0_final = 0;
                 σ0_final = 0;
             }
             else
             {
                 //把刺激量变小
                iiii = 0;
                 int iii = 0;
                 while (MAX_getMax(xArray) > 100)
                 {
                     iii = 0;
                     foreach (double x in xArray)
                     {
                         xArray[iii] = x / 10;
                         iii++;
                     }
                     iiii++;
                 }
                 double mu_j, sigma_j, mu_r, sigma_r;
                  MaxF = MAX_getMax(pub_function.getF(xArray, vArray));
                MinS = MIN_getMin(pub_function.S_getS(xArray, vArray));
                 if (MaxF > MinS)
                 {
                     logit_MLS_jn_getMLS(xArray, vArray, out mu_j, out sigma_j);
                     logit_MLS_reserch_getMLS(xArray, vArray, out mu_r, out sigma_r);
                     double[] fvArray = new double[xArray.Length];
                     double sigma_j1 = sigma_j * (Math.Pow(3, 0.5) / Math.PI);
                     double sigma_r1 = sigma_r * (Math.Pow(3, 0.5) / Math.PI);

                     int i = 0;
                     foreach (double x in vArray)
                     {
                         if (vArray[i] == 0) fvArray[i] = 1; else fvArray[i] = 0;
                         i++;
                     }

                     double mls_val_j = 1;
                     int ik;
                     for (ik = 0; ik < xArray.Length; ik++)
                     {
                         mls_val_j = mls_val_j * Math.Pow(1 / (1 + Math.Exp((mu_j - xArray[ik]) / sigma_j1)), vArray[ik]) * Math.Pow(1 - 1 / (1 + Math.Exp((mu_j - xArray[ik]) / sigma_j1)), fvArray[ik]);
                     }

                     double mls_val_r = 1;

                     for (ik = 0; ik < xArray.Length; ik++)
                     {
                         mls_val_r = mls_val_r * Math.Pow(1 / (1 + Math.Exp((mu_r - xArray[ik]) / sigma_r1)), vArray[ik]) * Math.Pow(1 - 1 / (1 + Math.Exp((mu_r - xArray[ik]) / sigma_r1)), fvArray[ik]);
                     }
                     if (mls_val_j > mls_val_r)
                     {
                         μ0_final = mu_j * Math.Pow(10, iiii);
                         σ0_final = sigma_j * Math.Pow(10, iiii);
                     }
                     else
                     {
                         μ0_final = mu_r * Math.Pow(10, iiii);
                         σ0_final = sigma_r * Math.Pow(10, iiii);
                     }
                 }
                 else
                 {
                     μ0_final = ((MaxF + MinS) / 2) * Math.Pow(10, iiii);
                     σ0_final = 0;
                 }

                 iii = 0;
                 int jk=0;
                 while (jk<iiii)
                 {
                     iii = 0;
                     foreach (double x in xArray)
                     {
                         xArray[iii] = x * 10;
                         iii++;
                     }
                     jk++;
                 }
             }
             Maxf = MaxF * Math.Pow(10, iiii);
             Mins = MinS * Math.Pow(10, iiii);
             */
        }

        //逻辑斯蒂系统所最大似然算法

        public static void logit_MLS_jn_getMLS(double[] xArray, int[] vArray, out double μ0_final, out double σ0_final)
        {

            double σ0_Last;
            double mu, sigma, sigma1, mu1, mudel, mudd;
            double gamma0, mu0, mud, gammad;
            double w, w1, w2;
            double mudel2, max_x, min_x;
            int judge = 0;

            mud = 0;
            gammad = 0;

            mu1 = 0.0;
            sigma1 = 0.0;
            max_x = MAX_getMax(xArray);
            min_x = MIN_getMin(xArray);



            double MaxF = MAX_getMax(pub_function.getF(xArray, vArray));
            double MinS = MIN_getMin(pub_function.S_getS(xArray, vArray));
            int Nm = pub_function.getNM(xArray, vArray);
            if (MinS >= MaxF)
            {
                mu = (MinS + MaxF) / 2;
                sigma = 0;
            }
            else
            {
                mu = (MinS + MaxF) / 2;
                sigma = (xArray.Length * (MaxF - MinS)) / (16 * (Nm + 2));
                mu1 = mu;
                sigma1 = sigma;
                mudd = mu1;
                gamma0 = sigma1 * Math.Pow(3, 0.5) / Math.PI;
                w2 = -10000000000000;
                mudel2 = 0.01;//0.01
                int iiii1 = 0;
                while (1 > 0)
                {
                    w1 = -10000000000;
                    iiii1++;
                    if (iiii1 > 500)
                    {
                        judge = 1;
                        goto case5;
                    }
                    mu0 = mudd;
                    mudel = 0.01;//0.01
                    while (1 > 0)
                    {
                        w = 0;
                        for (int j = 0; j < xArray.Length; j++)
                        {
                            w = w - Math.Log(1 + Math.Exp(-(xArray[j] - mu0) / gamma0));
                        }
                        for (int j = 0; j < pub_function.getF(xArray, vArray).Length; j++)
                        {
                            w = w - (pub_function.getF(xArray, vArray)[j] - mu0) / gamma0;
                        }
                        if (w > w1)
                        {
                            w1 = w;
                            mud = mu0;
                            gammad = gamma0;
                            mu0 = mu0 - mudel;
                        }
                        else
                        {
                            if (mudel == 0.01)
                            {
                                mudel = -0.01;
                                mu0 = mu0 - 2 * mudel;
                            }
                            else break;

                        }
                    }
                    if (w1 > w2)
                    {
                        mu1 = mud;
                        sigma1 = gammad * Math.PI / Math.Pow(3, 0.5);
                        w2 = w1;
                        gamma0 = gamma0 - mudel2;
                    }
                    else
                    {
                        if (mudel2 == 0.01)
                        {
                            mudel2 = -0.01;
                            gamma0 = gamma0 - 2 * mudel2;
                        }
                        else break;
                    }

                }

                mudd = mu1 - 0.01;
                gamma0 = (sigma1) * Math.Pow(3, 0.5) / Math.PI - 0.01;
                w2 = -10000000000;
                mudel2 = 0.0001;
                iiii1 = 0;
                while (1 > 0)
                {
                    w1 = -10000000000;
                    mu0 = mudd;
                    mudel = 0.0001;
                    iiii1++;
                    if (iiii1 > 500)
                    {
                        judge = 1;
                        goto case5;
                    }
                    while (1 > 0)
                    {
                        w = 0;
                        for (int j = 0; j < vArray.Length; j++)
                        {
                            w = w - Math.Log(1 + Math.Exp(-(xArray[j] - mu0) / gamma0));
                        }
                        for (int j = 0; j < pub_function.getF(xArray, vArray).Length; j++)
                        {
                            w = w - (pub_function.getF(xArray, vArray)[j] - mu0) / gamma0;
                        }
                        if (w > w1)
                        {
                            w1 = w;
                            mud = mu0;
                            gammad = gamma0;
                            mu0 = mu0 - mudel;
                        }
                        else
                        {
                            if (mudel == 0.0001)
                            {
                                mudel = -0.0001;
                                mu0 = mu0 - 2 * mudel;
                            }
                            else break;
                        }
                    }
                    if (w1 > w2)
                    {
                        mu1 = mud;
                        sigma1 = gammad * Math.PI / Math.Pow(3, 0.5);
                        w2 = w1;
                        gamma0 = gamma0 - mudel2;
                    }
                    else
                    {
                        if (mudel2 == 0.0001)
                        {
                            mudel2 = -0.0001;
                            gamma0 = gamma0 - 2 * mudel2;
                        }
                        else break;
                    }
                }
                if (mu1 > max_x) mu1 = max_x;
                if (mu1 < min_x) mu1 = min_x;
            }
        case5:
            mu = mu1;
            sigma = sigma1;
            if (judge == 1)
            {
                mu = (MinS + MaxF) / 2;
                sigma = 0;
            }
            μ0_final = mu;
            σ0_Last = sigma;
            σ0_final = σ0_Last;

        }

        //逻辑斯蒂最大似然估计查找算法
        public static void logit_MLS_reserch_getMLS(double[] xArray, int[] vArray, out double μ0_final, out double σ0_final)
        {
            double mu, sigma, mu1 = 0, sigma1 = 0, max_val = -100000000;
            double mls_val = 1;
            int ik, i = 0;
            double olow, ohigh, ulow, uhigh;
            double ud = 1, od = 1;
            double[] fvArray = new double[xArray.Length];
            foreach (double x in vArray)
            {
                if (vArray[i] == 0) fvArray[i] = 1; else fvArray[i] = 0;
                i++;
            }
            double test;

            olow = 0.00000001;
            ohigh = 10;
            ulow = MIN_getMin(xArray);
            uhigh = MAX_getMax(xArray);
            if ((uhigh - ulow) < 2)
                ud = 0.1;
            call1:
            for (mu = ulow; mu < (uhigh + ud); mu = mu + ud)
            {
                for (sigma = olow; sigma < (ohigh + od); sigma = sigma + od)
                {
                    mls_val = 1;
                    for (ik = 0; ik < xArray.Length; ik++)
                    {
                        mls_val = mls_val * Math.Pow(1 / (1 + Math.Exp((mu - xArray[ik]) / sigma)), vArray[ik]) * Math.Pow(1 - 1 / (1 + Math.Exp((mu - xArray[ik]) / sigma)), fvArray[ik]);
                        test = Math.Pow(1 / (1 + Math.Exp((mu - xArray[ik]) / sigma)), vArray[ik]) * Math.Pow(1 - 1 / (1 + Math.Exp((mu - xArray[ik]) / sigma)), fvArray[ik]);
                        //if (Math.Abs(mls_val) < Math.Pow(10, -100)) mls_val = 1;
                    }
                    if (mls_val > max_val)
                    {
                        max_val = mls_val;
                        mu1 = mu;
                        sigma1 = sigma;
                    }
                }
            }
            olow = sigma1 - od; ohigh = sigma1 + od; ; od = od / 10;
            ulow = mu1 - ud; uhigh = mu1 + ud; ud = ud / 10;
            if (ud > 0.0000000000001) goto call1;
            μ0_final = mu1;
            σ0_final = sigma1 / (Math.Pow(3, 0.5) / Math.PI);
        }

        //得到数组的最大值
        public static double MAX_getMax(double[] xArray)
        {
            int i = 0;
            double xArrayMax = xArray[0];
            foreach (double v0 in xArray)
            {
                if (v0 > xArrayMax)
                {
                    xArrayMax = v0;
                }
                i++;
            }
            return xArrayMax;
        }

        //得到数组的最小值
        public static double MIN_getMin(double[] xArray)
        {
            int i = 0;
            double xArrayMin = xArray[0];
            foreach (double v0 in xArray)
            {
                if (v0 < xArrayMin)
                {
                    xArrayMin = v0;
                }
                i++;
            }
            return xArrayMin;
        }
        //得到数组的最大值
        public static double MAX_getMax_int(int[] xArray)
        {
            int i = 0;
            int xArrayMax = xArray[0];
            foreach (int v0 in xArray)
            {
                if (v0 > xArrayMax)
                {
                    xArrayMax = v0;
                }
                i++;
            }
            return xArrayMax;
        }

        //得到数组的最小值
        public static double MIN_getMin_int(int[] xArray)
        {
            int i = 0;
            int xArrayMin = xArray[0];
            foreach (int v0 in xArray)
            {
                if (v0 < xArrayMin)
                {
                    xArrayMin = v0;
                }
                i++;
            }
            return xArrayMin;
        }

        //混合区间个数
        public static int getNM(double[] xArray, int[] vArray)
        {
            double MaxF;
            double MinS;
            MaxF = pub_function.MAX_getMax(pub_function.getF(xArray, vArray));
            MinS = pub_function.MIN_getMin(pub_function.S_getS(xArray, vArray));
            int i = 0;
            foreach (double v0 in xArray)
            {
                if ((v0 < MaxF) & (v0 > MinS))
                    i++;
            }
            return i;
        }

        //正态fisher信息矩阵求解下一刺激量
        public static double norm_fisher_getZ(double[] xArray, int[] vArray, double mu, double sigma)
        {
            double I11, I00, I01, C0, N0, J;
            double z0, z;
            int j, i;

            i = xArray.Length;
            I11 = 0; I00 = 0; I01 = 0;
            for (j = 0; j < i; j++)
            {
                z0 = xArray[j];
                J = (pub_function.pnorm(z0, mu, sigma) * (1 - pub_function.pnorm(z0, mu, sigma)));
                if (J == 0)
                    continue;
                J = Math.Pow(pub_function.dnorm_normpdf(z0, mu, sigma), 2) / J;
                I11 = I11 + J * Math.Pow(z0, 2);
                I00 = I00 + J;
                I01 = I01 + J * z0;
            }

            N0 = -100000000;
            C0 = 0;
            double diff;
            if (mu > 100)
                diff = 1;
            else
                diff = 0.01;
            //半区域搜索最大值
            if (Math.Abs(vArray[vArray.Length - 1]) < Math.Pow(10, -7))
            {
                for (z0 = xArray[xArray.Length - 1]; z0 < (mu + Math.Max(mu, 3 * sigma)); z0 = z0 + diff)
                {
                    J = Math.Pow(pub_function.dnorm_normpdf(z0, mu, sigma), 2) / (pub_function.pnorm(z0, mu, sigma)
                        * (1 - pub_function.pnorm(z0, mu, sigma)));
                    if (J > 10000000000 || J < -100000000000)
                        break;
                    if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
                    {
                        N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
                        C0 = z0;
                    }
                }
            }
            else
            {
                for (z0 = xArray[xArray.Length - 1]; z0 > (mu - Math.Max(mu, 3 * sigma)); z0 = z0 - diff)
                {
                    J = Math.Pow(pub_function.dnorm_normpdf(z0, mu, sigma), 2) / (pub_function.pnorm(z0, mu, sigma)
                        * (1 - pub_function.pnorm(z0, mu, sigma)));
                    if (J > 10000000000 || J < -100000000000)
                        break;
                    if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
                    {
                        N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
                        C0 = z0;
                    }
                }
            }
            //全区域搜索最大值
            //for (z0 = (mu - Math.Max(mu, 3 * sigma)); z0 < (mu + Math.Max(mu, 3 * sigma)); z0 = z0 + diff)
            //    {
            //        J = Math.Pow(pub_function.dnorm_normpdf(z0, mu, sigma), 2) / (pub_function.pnorm(z0, mu, sigma)
            //            * (1 - pub_function.pnorm(z0, mu, sigma)));
            //        if (J > 10000000000 || J < -100000000000)
            //            break;
            //        if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
            //        {
            //            N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
            //            C0 = z0;
            //        }
            //    }            
            double reso = diff;
            double z1;
            z0 = C0;


        case1:
            for (z1 = z0 - reso; z1 < (z0 + reso); z1 = z1 + reso / 10)
            {
                J = Math.Pow(pub_function.dnorm_normpdf(z1, mu, sigma), 2) / (pub_function.pnorm(z1, mu, sigma)
                    * (1 - pub_function.pnorm(z1, mu, sigma)));
                if (J > 10000000000 || J < -100000000000)
                    break;
                if (N0 < (I00 + J) * (I11 + J * Math.Pow(z1, 2)) - Math.Pow((I01 + J * z1), 2))
                {
                    N0 = (I00 + J) * (I11 + J * Math.Pow(z1, 2)) - Math.Pow((I01 + J * z1), 2);
                    C0 = z1;
                }
            }
            z0 = C0;
            reso = reso / 10;
            if (reso > 0.0000000001)
            {
                goto case1;
            }
            else
            {
                z = C0;
            }
            return z;
        }


        //正态分布最大似然估计综合算法
        public static void norm_MLS_getMLS(double[] xArray, int[] vArray, out double μ0_final, out double σ0_final, out double Maxf, out double Mins)
        {
            MLR_polar.Argument point0;


            int sum, j;
            double MaxF = 0;
            double MinS = 0;
            double L, L1;
            sum = 0;
            for (j = 0; j < xArray.Length; j++)
                sum += vArray[j];
            if (sum == 0 || sum == xArray.Length)
            {
                μ0_final = 0;
                σ0_final = 0;
            }
            else
            {
                double mu, sigma, mu1 = 0, sigma1 = 0;
                MaxF = pub_function.MAX_getMax(pub_function.getF(xArray, vArray));
                MinS = pub_function.MIN_getMin(pub_function.S_getS(xArray, vArray));
                if (MaxF > MinS)
                {

                    pub_function.NR_getNR(xArray, vArray, out mu, out sigma);
                    if (Math.Abs(sigma) < Math.Pow(10, -10) || Math.Abs(sigma) > Math.Pow(10, 10))
                    {
                        pub_function.getEM(xArray, vArray, out mu, out sigma);
                        if (sigma != 0)
                        {
                            point0.mu = mu;
                            point0.sigma = sigma;
                            L = -1 * MLR_polar.fun(point0, xArray, vArray, "normal");
                            MLR_polar.Max_Likelihood_Estimate(xArray, vArray, "normal", out mu1, out sigma1, out L);
                            point0.mu = mu1;
                            point0.sigma = sigma1;

                            L1 = -1 * MLR_polar.fun(point0, xArray, vArray, "normal");
                            if (L1 > L)
                            {
                                mu = mu1;
                                sigma = sigma1;
                            }
                        }
                    }



                    μ0_final = Math.Max(pub_function.MIN_getMin(xArray), Math.Min(mu, pub_function.MAX_getMax(xArray)));
                    σ0_final = Math.Min(sigma, pub_function.MAX_getMax(xArray) - pub_function.MIN_getMin(xArray));
                }
                else
                {
                    μ0_final = (MaxF + MinS) / 2;
                    σ0_final = 0;
                }
            }
            Maxf = MaxF;
            Mins = MinS;
        }

        //正态NR法最大似然估计
        public static void NR_getNR(double[] xArray, int[] vArray, out double μ0_final, out double σ0_final)
        {

            double μ0, σ0, param8, E;
            double Δμ, Δσ;
            double μ0_Last;
            double σ0_Last;
            double p1, p2, p3, p4, p5, p6;
            double MaxF = pub_function.MAX_getMax(pub_function.getF(xArray, vArray));
            double MinS = pub_function.MIN_getMin(pub_function.S_getS(xArray, vArray));


            double[] ui = new double[xArray.Length];
            double[] zi = new double[xArray.Length];
            double[] hi = new double[xArray.Length];
            double[] pi = new double[xArray.Length];
            double[] FZ = pub_function.getF(xArray, vArray);//失败的刺激量
            double[] SZ = pub_function.S_getS(xArray, vArray);//成功的刺激量


            int Nm = pub_function.getNM(xArray, vArray);

            int calTimes;//计算次数,最大控制在1000次以内

            param8 = 8.0;


            calTimes = 0;
            E = 0.00001;//用来控制精确度的
            σ0 = 0;
            μ0_final = 0;
            σ0_final = 0;
            p1 = 0;
            p2 = 0;
            p3 = 0;
            p4 = 0;
            p5 = 0;
            p6 = 0;
            if (MaxF > MinS)
            {


            continue1:

                try
                {
                    μ0 = 0.5 * (MaxF + MinS); ;
                    σ0 = xArray.Length * (MaxF - MinS) / (param8 * (Nm + 2));
                //开始计算
                continue2:
                    getUIArray(ref ui, xArray, vArray, μ0, σ0);

                    if (getPIArray(ref pi, ui) == false)
                    {
                        param8 = param8 - 0.01;
                        if (param8 <= 0)
                        {
                            μ0_Last = 0.5 * (MaxF + MinS);
                            σ0_Last = 0;
                            goto continue3;
                        }
                        goto continue1;
                    }



                    getZi_NormalArray(ref zi, ui, pi);
                    getHIArray(ref hi, vArray, pi);


                    p1 = getG(ui, zi, hi);
                    p2 = getFσ(ui, zi, hi, σ0);
                    p3 = getf(zi, hi);
                    p4 = getGσ(ui, zi, hi, p1, σ0);
                    p5 = getFμ(zi, hi, p1, σ0);
                    p6 = getGμ(p3, p2, σ0);

                    Δμ = (p1 * p2 - p3 * p4) / (p5 * p4 - p6 * p2);
                    if (double.IsNaN(Δμ))
                    {
                        param8 = param8 - 0.01;
                        if (param8 <= 0)
                        {
                            μ0_Last = 0.5 * (MaxF + MinS);
                            σ0_Last = 0;
                            goto continue3;
                        }
                        goto continue1;
                    }

                    Δσ = (p1 * p5 - p3 * p6) / (p2 * p6 - p4 * p5);
                    if (double.IsNaN(Δσ))
                    {
                        param8 = param8 - 0.01;
                        if (param8 <= 0)
                        {
                            μ0_Last = 0.5 * (MaxF + MinS);
                            σ0_Last = 0;
                            goto continue3;
                        }
                        goto continue1;
                    }

                    if ((Math.Abs(Δμ) + Math.Abs(Δσ) < E))
                    {
                        //终止计算
                        μ0_Last = μ0 + Δμ;
                        σ0_Last = σ0 + Δσ;

                    }
                    else
                    {
                        calTimes++;
                        // 最大叠加500次,如果不收敛的话就退出
                        if (calTimes > 1000)
                        {
                            μ0_Last = 0.5 * (MaxF + MinS);
                            σ0_Last = 0;
                            goto continue3;
                        }
                        μ0 = μ0 + Δμ;
                        if (μ0 < -100000000)
                        {
                            μ0_Last = 0.5 * (MaxF + MinS);
                            σ0_Last = 0;
                            goto continue3;
                        }
                        σ0 = σ0 + Δσ;
                        goto continue2;
                    }
                    //  continue3:
                }
                catch
                {
                    param8 = param8 - 0.01;
                    if (param8 <= 0)
                    {
                        μ0_Last = 0.5 * (MaxF + MinS);
                        σ0_Last = 0;
                        goto continue3;
                    }
                    goto continue1;
                }
            continue3:
                μ0_final = μ0_Last;
                σ0_final = σ0_Last;
            }
            else
            {

                μ0_final = 0.5 * (MaxF + MinS);
                σ0_final = 0;
            }

        }
        public static double getUi(double Xi, double muO, double delta0)
        {
            return (Xi - muO) / delta0;
        }
        public static double getZi_Normal(double ui)
        {
            double result;
            result = 1 / Math.Sqrt(2 * Math.PI) * Math.Exp(-0.5 * ui * ui);
            return result;
        }
        public static double getHi(int vi, double pi)
        {
            double result;
            result = vi / pi - (1 - vi) / (1 - pi);
            return result;
        }
        public static void getUIArray(ref double[] UIArray, double[] xAarry, int[] vAarry, double mu0, double delta0)
        {
            for (int i = 0; i < xAarry.Length; i++)
            {
                UIArray[i] = getUi(xAarry[i], mu0, delta0);
            }
        }
        public static void getZi_NormalArray(ref double[] ZIArray, double[] UIArray, double[] PIArray)
        {
            for (int i = 0; i < UIArray.Length; i++)
            {
                ZIArray[i] = getZi_Normal(UIArray[i]);
            }
        }
        public static void getHIArray(ref double[] HIArray, int[] vArray, double[] PIArray)
        {
            for (int i = 0; i < PIArray.Length; i++)
            {
                HIArray[i] = getHi(vArray[i], PIArray[i]);
            }
        }
        public static Boolean getPIArray(ref double[] PIArray, double[] UIArray)
        {


            for (int i = 0; i < UIArray.Length; i++)
            {

                PIArray[i] = pub_function.pnorm(UIArray[i], 0, 1);
                if (PIArray[i].Equals(1))
                    return false;
            }
            return true;
        }
        public static double getG(double[] UIAarry, double[] ZIAarry, double[] HIAarry)
        {
            double result;
            result = 0;
            for (int i = 0; i < UIAarry.Length; i++)
            {

                result = result + UIAarry[i] * ZIAarry[i] * HIAarry[i];
            }

            return result;
        }
        public static double getf(double[] ZIAarry, double[] HIAarry)
        {
            double result;


            result = 0;
            for (int i = 0; i < ZIAarry.Length; i++)
            {

                result = result + ZIAarry[i] * HIAarry[i];
            }

            return result;
        }
        public static double getFμ(double[] ZIAarry, double[] HIAarry, double g, double delta0)
        {
            double result;
            result = 0;

            for (int i = 0; i < ZIAarry.Length; i++)
            {

                result = result + ZIAarry[i] * HIAarry[i] * ZIAarry[i] * HIAarry[i];

            }
            result = 1 / delta0 * (g + result);
            return result;
        }
        public static double getFσ(double[] UIAarry, double[] ZIAarry, double[] HIAarry, double delta0)
        {
            double result;

            result = 0;

            for (int i = 0; i < UIAarry.Length; i++)
            {

                result = result + UIAarry[i] * ZIAarry[i] * HIAarry[i] * (UIAarry[i] + ZIAarry[i] * HIAarry[i]);


            }
            result = 1 / delta0 * result;
            return result;
        }
        public static double getGμ(double f, double fσ, double delta0)
        {
            double result;
            result = 0;
            result = 1 / delta0 * f + fσ;

            return result;
        }
        public static double getGσ(double[] UIAarry, double[] ZIAarry, double[] HIAarry, double g, double delta0)
        {
            double result;
            result = 0;
            for (int i = 0; i < UIAarry.Length; i++)
            {

                result = result + UIAarry[i] * UIAarry[i] * ZIAarry[i] * HIAarry[i] * (UIAarry[i] +
                    ZIAarry[i] * HIAarry[i]);
            }
            result = 1 / delta0 * (result - g);

            return result;
        }

        // 得到正态分布分布函数
        public static double pnorm(double x, double mu, double sigma)
        {
            double result;
            double[] a = {0.31938153,-0.356563782,
            1.781477937,-1.821255978,1.330274429};
            x = (x - mu) / sigma;

            if (x < -7.0)
                result = pub_function.dnorm_normpdf(x, 0, 1) / Math.Sqrt(1.0 + x * x);
            else if (x > 7.0)
                result = 1.0 - pub_function.dnorm_normpdf(-x, 0, 1);
            else
            {
                result = 0.2316419;

                result = 1.0 / (1 + result * Math.Abs(x));
                result = 1 - pub_function.dnorm_normpdf(x, 0, 1) * (result * (a[0] + result * (a[1] + result * (a[2]
                    + result * (a[3] + result * a[4])))));
                if (x <= 0.0) result = 1.0 - result;
            }
            return result;

        }

        //精度运算
        public static void resolution_getReso(double z, double reso, out double z_reso)
        {
            //string sz;
            if (reso.Equals(0))
            {


                z_reso = Math.Round(z / 0.000001) * 0.000001;
                return;

            }

            z_reso = Math.Round(z / reso) * reso;
        }

        public static double resolution_getReso(double z, double reso)
        {
            if (reso.Equals(0))
            {

                reso = Math.Round(z / 0.000001) * 0.000001; ;

            }
            else

                reso = Math.Round(z / reso) * reso;
            return reso;

        }


        //成功所对应的刺激量
        public static double[] S_getS(double[] xArray, int[] vArray)
        {
            int iii = 0;
            int len = 0;
            foreach (int v0 in vArray)
            {
                if (v0 == 1)
                {
                    iii++;
                }
            }
            int[] result = new int[iii];
            int ii = 0;
            int i = 0;
            int j = 0;
            foreach (int v0 in vArray)
            {
                if (v0 == 1)
                {
                    result[ii] = i;
                    ii++;
                }
                i++;
            }
            if (result.Length == 0)
                len = 1;
            else
                len = result.Length;


            double[] S = new double[len];
            foreach (int lo in result)
            {
                S[j] = xArray[lo];
                j++;
            }
            return S;
        }

        //将xArray，变为log(xArray)
        public static void get_xArray_ln(int xArrayLength, double[] xArray, out double[] xArray_ln)
        {
            xArray_ln = new double[xArrayLength];
            int ij;
            for (ij = 0; ij < xArrayLength; ij++)
            {
                xArray_ln[ij] = Math.Log(xArray[ij]);
            }
        }
        //将xArray，变为log(xArray,10)
        public static void get_xArray_log10(int xArrayLength, double[] xArray, out double[] xArray_log10)
        {
            xArray_log10 = new double[xArrayLength];
            int ij;
            for (ij = 0; ij < xArrayLength; ij++)
            {
                xArray_log10[ij] = Math.Log10(xArray[ij]);
            }
        }
        //将xArray，变为pow(xArray,y)
        public static void get_xArray_pow(int xArrayLength, double y, double[] xArray, out double[] xArray_pow)
        {
            xArray_pow = new double[xArrayLength];
            int ij;
            for (ij = 0; ij < xArrayLength; ij++)
            {
                xArray_pow[ij] = Math.Pow(xArray[ij], y);
            }
        }
        //兰利法得到下一刺激量
        public static void Lanlie_getz(int xArrayLength, double[] xArray, int[] vArray, double mumin, double mumax, out double z)
        {
            if (xArrayLength == 0)
                z = (mumin + mumax) / 2;
            else
            {
                double xarray1;

                int j, k1, k2;
                k1 = 0;
                k2 = 0;
                for (j = xArrayLength - 1; j >= 0; j--)
                {
                    if (vArray[j] == 0)
                        k1 = k1 + 1;
                    else
                        k2 = k2 + 1;
                    if (k1 == k2)
                        break;


                }
                if (j >= 0)
                { xarray1 = xArray[j]; }
                else
                {
                    if (vArray[xArrayLength - 1] == 0)
                    { xarray1 = mumax; }
                    else
                    { xarray1 = mumin; }
                }
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    X[w] = xArray[w];
                    V[w] = vArray[w];
                }
                z = 0.5 * (xArray[xArrayLength - 1] + xarray1);
            }
        }

        //正态P 分位数
        public static double qnorm(double p)
        {
            if (p < 0) return 0;
            double x, y;
            double E = 0.0000000000001;
            double delta;
            double min, max;
            if (p == 0.5) return 0;
            if (p == 0.99) return 2.32634787404084;
            if (p == 0.01) return -2.32634787404084;
            if (p == 0.999) return 3.09023230616781;
            if (p == 0.001) return -3.09023230616781;
            if (p == 0.9999) return 3.71901648545571;
            if (p == 0.0001) return -3.71901648545571;
            if (p == 0.99999) return 4.26489079392384;
            if (p == 0.00001) return -4.26489079392384;
            if (p == 0.999999) return 4.75342430881709;
            if (p == 0.000001) return -4.75342430881709;
            if (p == 0.9999999) return 5.19933758229066;
            if (p == 0.0000001) return -5.19933758229066;
            if (p == 0.99999999) return 5.61200124330550;
            if (p == 0.00000001) return -5.61200124330550;
            if (p == 0.999999999) return 5.99780701960164;
            if (p == 0.000000001) return -5.99780701960164;
            if (p == 0.9999999999) return 6.36134088969742;
            if (p == 0.0000000001) return -6.36134088969742;
            if ((p > 0.999999999999) & (p < 0.000000000001)) return 0;
            delta = 0;
            min = -9;
            max = 9;
            y = p;
            x = pnorm(delta, 0, 1);

            //如果没有达到精确度，就继续查找
            while (Math.Abs(x - y) > E)
            {
                //若果选定的值大于预定的概率，delta缩小
                if (x > y)
                {
                    max = (min + max) / 2;
                }

                //若果选定的值小于预定的概率，delta增大
                if (x < y)
                {
                    min = (min + max) / 2;
                }
                x = pnorm((min + max) / 2, 0, 1);
            }
            return (min + max) / 2;
        }




        //逻辑斯蒂P 分位数
        public static double qlogis(double p)
        {
            double p1;
            p1 = Math.Log(p / (1 - p));
            return p1;
            //if (p < 0) return 0;
            //double x, y;
            //double E = 0.0000001;
            //double delta;
            //double min, max;
            //if (p == 0.5) return 0;
            //if (p == 0.999) return 6.906755;
            //if (p == 0.001) return -6.906755;
            //delta = 0;
            //min = 0;
            //max = 5;
            //y = p;
            //x = plogis(delta, 0, 1);

            ////如果没有达到精确度，就继续查找
            //while (Math.Abs(x - y) > E)
            //{
            //    //若果选定的值大于预定的概率，delta缩小
            //    if (x > y)
            //    {

            //        max = delta;
            //        delta = delta - (delta - min) * 0.5;
            //    }

            //    //若果选定的值小于预定的概率，delta增大
            //    if (x < y)
            //    {
            //        min = delta;
            //        delta = delta + (max - delta) * 0.5;

            //    }
            //    x = plogis(delta, 0, 1);
            //}
            //return delta;
        }
        //////////////////////////////////////////////////////
        ////得到小数点后的位数
        /// <summary>
        /// /////////////////////////////////
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        public static void get_point(double a, out int i)
        {
            double x0;
            i = 0;
            while (1 == 1)
            {
                x0 = Math.Abs(a * Math.Pow(10, i) - Math.Floor(a * Math.Pow(10, i)));
                if (x0 < Math.Pow(10, -7)) break;
                i = i + 1;
            }
        }
        //////////////////////////////////////////////////////
        ////得到小数点后的位数
        /// <summary>
        /// /////////////////////////////////
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        static int Decimalnumble1(double x)
        {
            int i = 0;
            while (Convert.ToDecimal(x * Math.Pow(10, i)) != Convert.ToDecimal(Math.Floor(Math.Pow(10, i) * x)))
            { i++; }
            return i;
        }

        //大数转换函数20100512
        public static void max_number_change(double[] xArray, out int weishu, out double[] xArray_temp)
        {
            double max;
            max = MAX_getMax(xArray);
            double[] xArray_temp_1 = new double[xArray.Length];
            if (max < 500)
            {
                weishu = 0;
                xArray_temp_1 = xArray;
            }
            else
            {
                double max_temp = max;
                weishu = 0;
                do
                {
                    max_temp = max_temp / 10;
                    weishu++;
                } while (max_temp > 500);
                for (int i = 0; i < xArray.Length; i++)
                {
                    xArray_temp_1[i] = xArray[i] * Math.Pow(10, -weishu);
                }
            }
            xArray_temp = xArray_temp_1;
        }
        //搜索区间最大值20100512
        public static double norm_fisher_getZ_new(double[] xArray, int[] vArray, double mu, double sigma)
        {
            double I11, I00, I01, C0 = 0, N0 = 0, J = 0;
            double z0 = 0;
            int j, i;

            i = xArray.Length;
            I11 = 0; I00 = 0; I01 = 0;
            for (j = 0; j < i; j++)
            {
                z0 = (xArray[j] - mu) / sigma;
                J = (pub_function.pnorm(z0, 0, 1) * (1 - pub_function.pnorm(z0, 0, 1)) * Math.Pow(sigma, 2));
                if (J == 0)
                    continue;
                J = Math.Pow(pub_function.dnorm_normpdf(z0, 0, 1), 2) / J;
                I11 = I11 + J * Math.Pow(z0, 2);
                I00 = I00 + J;
                I01 = I01 + J * z0;
            }

            double varmu, varsigma, covmusigma;
            varmu = I11 / (I00 * I11 - I01 * I01);
            varsigma = I00 / (I00 * I11 - I01 * I01);
            covmusigma = -I01 / (I00 * I11 - I01 * I01);

            N0 = -100000000;
            C0 = 0;
            double diff;
            double left, right, middle = 0, xielv = 0;
            //半区域搜索最大值

            //if (Math.Abs(vArray[vArray.Length - 1]) < Math.Pow(10, -7))
            //{
            //    left = xArray[xArray.Length - 1];
            //    right = (mu + Math.Max(mu, 3 * sigma));
            //    diff = (right - left) / 200;
            //    for (z0 = left; z0 < right; z0 = z0 + diff)
            //    {
            //        J = Math.Pow(pub_function.dnorm_normpdf(z0, mu, sigma), 2) / (pub_function.pnorm(z0, mu, sigma)
            //            * (1 - pub_function.pnorm(z0, mu, sigma)));
            //        if (J > 10000000000 || J < -100000000000)
            //            break;
            //        if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
            //        {
            //            N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
            //            C0 = z0;
            //        }
            //    }
            //    left = C0 - diff;
            //    right = C0 + diff;
            //    while (Math.Abs(middle - (left + right) / 2) > Math.Pow(10, -12))
            //    {
            //        middle = (left + right) / 2;
            //        xielv = Fisher_norm_dc(middle, mu, sigma, I00, I01, I11);
            //        if (xielv > 0)
            //           left = middle;
            //        else if (xielv < 0)
            //            right = middle;
            //    }
            //}
            //else
            //{
            //    left = (mu - Math.Max(mu, 3 * sigma));
            //    right = xArray[xArray.Length - 1];
            //    diff = (right - left) / 200;
            //    for (z0 = right; z0 > left; z0 = z0 - diff)
            //    {
            //        J = Math.Pow(pub_function.dnorm_normpdf(z0, mu, sigma), 2) / (pub_function.pnorm(z0, mu, sigma)
            //            * (1 - pub_function.pnorm(z0, mu, sigma)));
            //        if (J > 10000000000 || J < -100000000000)
            //            break;
            //        if (N0 < (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2))
            //        {
            //            N0 = (I00 + J) * (I11 + J * Math.Pow(z0, 2)) - Math.Pow((I01 + J * z0), 2);
            //            C0 = z0;
            //        }
            //    }
            //    left = C0 - diff;
            //    right = C0 + diff;
            //    while (Math.Abs(middle - (left + right) / 2) >Math.Pow(10, -12))
            //    {
            //        middle = (left + right) / 2;
            //        xielv = Fisher_norm_dc(middle, mu, sigma, I00, I01, I11);
            //        if (xielv > 0)
            //            left = middle;
            //        else if (xielv < 0)
            //            right = middle;
            //    }

            //}
            //全区域搜索最大值
            left = mu - Math.Max(mu, 3 * sigma);
            right = mu + Math.Max(mu, 3 * sigma);
            diff = (right - left) / 200;
            for (z0 = left; z0 < right; z0 = z0 + diff)
            {
                J = Math.Pow(pub_function.dnorm_normpdf((z0 - mu) / sigma, 0, 1), 2) / (pub_function.pnorm((z0 - mu) / sigma, 0, 1)
                    * (1 - pub_function.pnorm((z0 - mu) / sigma, 0, 1)) * Math.Pow(sigma, 2));
                if (J > 10000000000 || J < -100000000000)
                    break;
                if (N0 < (I00 + J) * (I11 + J * Math.Pow((z0 - mu) / sigma, 2)) - Math.Pow((I01 + J * (z0 - mu) / sigma), 2))
                {
                    N0 = (I00 + J) * (I11 + J * Math.Pow((z0 - mu) / sigma, 2)) - Math.Pow((I01 + J * (z0 - mu) / sigma), 2);
                    C0 = z0;
                }
            }
            left = C0 - diff;
            right = C0 + diff;

            //double temp;

            //while (Math.Abs(middle - (left + right) / 2) > Math.Pow(10, -12))
            //{

            //} 



            while (Math.Abs(middle - (left + right) / 2) > Math.Pow(10, -12))
            {
                middle = (left + right) / 2;
                xielv = Fisher_norm_dc(middle, mu, sigma, I00, I01, I11);
                if (xielv > 0)
                    left = middle;
                else if (xielv < 0)
                    right = middle;
            }


            return middle;
        }

        public static double logit_fisher_getZ_new(double[] xArray, int[] vArray, double mu, double sigma)
        {
            double I11, I00, I01, C0 = 0, N0 = 0, J = 0;
            double z0 = 0;
            int j, i;
            sigma = sigma * (Math.Pow(3, 0.5) / Math.PI);
            i = xArray.Length;
            I11 = 0; I00 = 0; I01 = 0;
            for (j = 0; j < i; j++)
            {
                z0 = (xArray[j] - mu) / sigma;
                J = (pub_function.plogis(z0, 0, 1) * (1 - pub_function.plogis(z0, 0, 1)) * Math.Pow(sigma, 2));
                if (J == 0)
                    continue;
                J = Math.Pow(pub_function.dlogis(z0, 0, 1), 2) / J;
                I11 = I11 + J * Math.Pow(z0, 2);
                I00 = I00 + J;
                I01 = I01 + J * z0;
            }

            double varmu, varsigma, covmusigma;
            varmu = I11 / (I00 * I11 - I01 * I01);
            varsigma = (I00 / (I00 * I11 - I01 * I01)) * Math.Pow(Math.PI, 2) / 3;
            covmusigma = (-I01 / (I00 * I11 - I01 * I01)) * Math.PI / Math.Pow(3, 0.5);





            N0 = -100000000;
            C0 = 0;
            double diff;
            double left, right, middle = 0, xielv = 0;

            //全区域搜索最大值
            left = mu - Math.Max(mu, 3 * sigma);
            right = mu + Math.Max(mu, 3 * sigma);
            diff = (right - left) / 200;
            for (z0 = left; z0 < right; z0 = z0 + diff)
            {
                J = Math.Pow(pub_function.dlogis((z0 - mu) / sigma, 0, 1), 2) / (pub_function.plogis((z0 - mu) / sigma, 0, 1)
                    * (1 - pub_function.plogis((z0 - mu) / sigma, 0, 1)) * Math.Pow(sigma, 2));
                if (J > 10000000000 || J < -100000000000)
                    break;
                if (N0 < (I00 + J) * (I11 + J * Math.Pow((z0 - mu) / sigma, 2)) - Math.Pow((I01 + J * (z0 - mu) / sigma), 2))
                {
                    N0 = (I00 + J) * (I11 + J * Math.Pow((z0 - mu) / sigma, 2)) - Math.Pow((I01 + J * (z0 - mu) / sigma), 2);
                    C0 = z0;
                }
            }
            left = C0 - diff;
            right = C0 + diff;
            while (Math.Abs(middle - (left + right) / 2) > Math.Pow(10, -12))
            {
                middle = (left + right) / 2;
                xielv = Fisher_logis_dc(middle, mu, sigma, I00, I01, I11);
                if (xielv > 0)
                    left = middle;
                else if (xielv < 0)
                    right = middle;
            }




            return middle;
        }
        public static double Fisher_norm_dc(double z, double mu, double sigma, double I00, double I01, double I11)
        {
            double d, d1, p, p1, J1, J;
            z = (z - mu) / sigma;
            d = pub_function.dnorm_normpdf(z, 0, 1);
            d1 = -z * d;
            p = pub_function.pnorm(z, 0, 1);
            p1 = d;
            J = Math.Pow(pub_function.dnorm_normpdf(z, 0, 1), 2) / (pub_function.pnorm(z, 0, 1) * (1 - pub_function.pnorm(z, 0, 1)) * Math.Pow(sigma, 2));
            J1 = (2 * d * d1 * p * (1 - p) - Math.Pow(d, 3) + 2 * p * Math.Pow(d, 3)) / (Math.Pow((p * (1 - p)), 2) * Math.Pow(sigma, 2));
            //  (-2 * ((z - mu) / Math.Pow(sigma, 2)) * Math.Pow(d, 2) * p * (1 - p) - Math.Pow(d, 3) + 2 * p * Math.Pow(d, 3)) / Math.Pow((p * (1 - p)), 2) / Math.Pow(sigma, 2);
            return I00 * (J1 * Math.Pow(z, 2) + 2 * J * z) - 2 * I01 * (J1 * z + J) + I11 * J1;
        }
        public static double Fisher_logis_dc(double z, double mu, double sigma, double I00, double I01, double I11)
        {
            double d, d1, p, p1, J1, J;
            z = (z - mu) / sigma;

            d = pub_function.dlogis(z, 0, 1);

            d1 = (Math.Exp(-z) * (Math.Exp(-z) - 1)) / Math.Pow(1 + Math.Exp(-z), 3);

            //d1 = Math.Exp((z - mu) / sigma) * (1 - Math.Exp((z - mu) / sigma)) / Math.Pow((Math.Exp((z - mu) / sigma) + 1),3)/Math.Pow(sigma,2);
            p = pub_function.plogis(z, 0, 1);
            p1 = d;

            J = Math.Pow(pub_function.dlogis(z, 0, 1), 2) / (pub_function.plogis(z, 0, 1) * (1 - pub_function.plogis(z, 0, 1)) * Math.Pow(sigma, 2));

            //J = Math.Pow(pub_function.dlogis(z, mu, sigma), 2) / (pub_function.plogis(z, mu, sigma) * (1 - pub_function.plogis(z, mu, sigma)));
            J1 = (2 * d * d1 * p * (1 - p) - Math.Pow(d, 3) + 2 * p * Math.Pow(d, 3)) / (Math.Pow((p * (1 - p)), 2) * Math.Pow(sigma, 2));

            // J1 = (2 * d1 * d * p * (1 - p) - Math.Pow(d, 3) + 2 * p * Math.Pow(d, 3)) / Math.Pow((p * (1 - p)), 2);
            return I00 * (J1 * Math.Pow(z, 2) + 2 * J * z) - 2 * I01 * (J1 * z + J) + I11 * J1;
        }
        //////////////////////////////////////////////////////
        ////得到数的有效数字
        /// <summary>
        /// /////////////////////////////////
        /// </summary>
        /// <param name="a"></param>
        /// <param name="i"></param>
        public static void get_youxiaoshuzi(double a, out int i)
        {
            double temp1;
            pub_function.get_point(a, out i);
            temp1 = a;
            while (temp1 > 0.9999999999)
            {
                temp1 = temp1 / 10;
                i = i + 1;
            }
        }


        public static double xiangyingdianjisuan(double gailv, double final_mu, double final_sigma, string fenbu, double power)
        {
            double favg, fsigma, fq;

            favg = final_mu;
            fsigma = final_sigma;
            fq = gailv;
            if (fsigma == 0)
            {
                return 0;
            }
            else
            {
                if (gailv > 1 || gailv < 0.000000000000001)
                {
                    return 0;

                }
                else
                {

                    if (fenbu == "正态分布标准")
                        return (favg + pub_function.qnorm(fq) * fsigma);
                    if (fenbu == "正态分布Ln" || fenbu == "正态分布Ln")
                        return (Math.Exp(Math.Log(favg) + pub_function.qnorm((fq)) * fsigma));
                    if (fenbu == "正态分布log10" || fenbu == "正态分布log10")
                        return (Math.Pow(10, Math.Log10(favg) + pub_function.qnorm((fq)) * fsigma));
                    if (fenbu == "正态分布幂")
                        return (Math.Pow(Math.Pow(favg, power) + pub_function.qnorm(fq) * fsigma, 1 / power));

                    if (fenbu == "逻辑斯谛分布标准")
                        return (favg + pub_function.qlogis(fq) * fsigma);
                    if (fenbu == "逻辑斯谛分布Ln" || fenbu == "逻辑斯谛分布Ln")
                        return (Math.Exp(Math.Log(favg) + pub_function.qlogis((fq)) * fsigma));
                    if (fenbu == "逻辑斯谛分布log10" || fenbu == "逻辑斯谛分布log10")
                        return (Math.Pow(10, Math.Log10(favg) + pub_function.qlogis((fq)) * fsigma));
                    if (fenbu == "逻辑斯谛分布幂")
                        return (Math.Pow(Math.Pow(favg, power) + pub_function.qlogis(fq) * fsigma, 1 / power));
                    else
                        return 0;
                }

            }
        }

        public static string updown_biaozhuncha_hou(string fenbu)
        {

            if (fenbu == "正态分布Ln" || fenbu == "逻辑斯谛分布Ln")
            {
                return " (ln)";
            }
            else if (fenbu == "正态分布log10" || fenbu == "逻辑斯谛分布log10")
            {

                return " (log10)";

            }
            else if (fenbu == "正态分布幂" || fenbu == "逻辑斯谛分布幂")
            {

                return " (幂)";

            }
            else
            {
                return " ";
            }

        }

        public static string D_langlie_biaozhuncha_hou(string fenbu)
        {
            if (fenbu == "正态分布标准")
            {

                return " ";
            }
            else if (fenbu == "正态分布Ln")
            {

                return "(ln)";
            }
            else if (fenbu == "正态分布log10")
            {


                return "(log10)";


            }
            else if (fenbu == "正态分布幂")
            {


                return "(幂)";
            }
            else if (fenbu == "逻辑斯谛分布标准")
            {

                return " ";

            }
            else if (fenbu == "逻辑斯谛分布Ln")
            {

                return "(ln)";
            }

            else if (fenbu == "逻辑斯谛分布log10")
            {

                return "(log10)";

            }
            else if (fenbu == "逻辑斯谛分布幂")
            {

                return "(幂)";
            }
            else
                return " ";

        }











        public static void get_zhixinqujian(double gailv, double zxsp, int test_n, string fenbu, double favg, double fsigma, double fsigmaavg, double fsigmasigma, out double xiangyingdian, out double xiangyingdian_shangxian, out double xiangyingdian_xiaxian, out double junzhishangxian, out double junzhixiaxian, out double biaozhunchashangxian, out double biaozhunchaxiaxian, out double xiangyingdian_dance_shangxian, out double xiangyingdian_dance_xiaxian)
        {
            // Double favg, fsigma, fsigmaavg, fsigmasigma;
            Double fq, Tfw, Tfw_dance;
            fq = gailv;
            Tfw = updownMethod.get_t_shuangce(zxsp, test_n - 1);//T分位数
            Tfw_dance = updownMethod.get_t_dance(zxsp, test_n - 1);//T分位数
            xiangyingdian = 0;


            // 置信上限：
            xiangyingdian_shangxian = 0;

            // 置信下限：
            xiangyingdian_xiaxian = 0;
            xiangyingdian_dance_shangxian = 0;
            xiangyingdian_dance_xiaxian = 0;
            // 均值置信上限
            junzhishangxian = 0;

            // 均值置信下限
            junzhixiaxian = 0;
            biaozhunchashangxian = 0;

            //标准差置信下限
            biaozhunchaxiaxian = 0;
            if (fenbu == "正态分布标准")
            {
                xiangyingdian = favg + pub_function.qnorm(fq) * fsigma;


                // 置信上限：
                xiangyingdian_shangxian = favg + pub_function.qnorm(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2));


                // 置信下限：
                xiangyingdian_xiaxian = favg + pub_function.qnorm(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2));
                xiangyingdian_dance_shangxian = favg + pub_function.qnorm(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2));
                xiangyingdian_dance_xiaxian = favg + pub_function.qnorm(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2));
                // 均值置信上限
                junzhishangxian = favg + Tfw * fsigmaavg;


                // 均值置信下限
                junzhixiaxian = favg - Tfw * fsigmaavg;

            }
            else
                if (fenbu == "正态分布Ln")
            {
                xiangyingdian = Math.Exp(Math.Log(favg) + pub_function.qnorm((fq)) * fsigma);

                // 置信上限：
                xiangyingdian_shangxian = Math.Exp(Math.Log(favg) + pub_function.qnorm(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));


                // 置信下限：
                xiangyingdian_xiaxian = Math.Exp(Math.Log(favg) + pub_function.qnorm(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));

                xiangyingdian_dance_shangxian = Math.Exp(Math.Log(favg) + pub_function.qnorm(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));
                xiangyingdian_dance_xiaxian = Math.Exp(Math.Log(favg) + pub_function.qnorm(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));
                // 均值置信上限
                junzhishangxian = Math.Exp(Math.Log(favg) + Tfw * fsigmaavg);

                // 均值置信下限
                junzhixiaxian = Math.Exp(Math.Log(favg) - Tfw * fsigmaavg);

            }
            else
                    if (fenbu == "正态分布log10")
            {
                xiangyingdian = Math.Pow(10, Math.Log10(favg) + pub_function.qnorm((fq)) * fsigma);

                // 置信上限：
                xiangyingdian_shangxian = Math.Pow(10, Math.Log10(favg) + pub_function.qnorm(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));
                // 置信下限：
                xiangyingdian_xiaxian = Math.Pow(10, Math.Log10(favg) + pub_function.qnorm(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));

                xiangyingdian_dance_shangxian = Math.Pow(10, Math.Log10(favg) + pub_function.qnorm(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));
                xiangyingdian_dance_xiaxian = Math.Pow(10, Math.Log10(favg) + pub_function.qnorm(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)));

                // 均值置信上限
                junzhishangxian = Math.Pow(10, Math.Log10(favg) + Tfw * fsigmaavg);

                // 均值置信下限
                junzhixiaxian = Math.Pow(10, Math.Log10(favg) - Tfw * fsigmaavg);

            }
            else
                        if (fenbu == "正态分布幂")
            {
                xiangyingdian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qnorm(fq) * fsigma, 1 / SysParam.updown_Power);

                // 置信上限：
                xiangyingdian_shangxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qnorm(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)), 1 / SysParam.updown_Power);

                // 置信下限：
                xiangyingdian_xiaxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qnorm(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)), 1 / SysParam.updown_Power);
                xiangyingdian_dance_shangxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qnorm(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)), 1 / SysParam.updown_Power);
                xiangyingdian_dance_xiaxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qnorm(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qnorm(fq), 2)), 1 / SysParam.updown_Power);

                // 均值置信上限
                junzhishangxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + Tfw * fsigmaavg, 1 / SysParam.updown_Power);

                // 均值置信下限
                junzhixiaxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) - Tfw * fsigmaavg, 1 / SysParam.updown_Power);

            }
            else
                            if (fenbu == "逻辑斯谛分布标准")
            {
                xiangyingdian = (favg + pub_function.qlogis(fq) * fsigma);

                // 置信上限：
                xiangyingdian_shangxian = favg + pub_function.qlogis(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2));

                // 置信下限：
                xiangyingdian_xiaxian = favg + pub_function.qlogis(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2));

                xiangyingdian_dance_shangxian = favg + pub_function.qlogis(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2));
                xiangyingdian_dance_xiaxian = favg + pub_function.qlogis(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2));
                // 均值置信上限
                junzhishangxian = favg + Tfw * fsigmaavg;

                // 均值置信下限
                junzhixiaxian = favg - Tfw * fsigmaavg;

            }
            else
                                if (fenbu == "逻辑斯谛分布Ln")
            {
                xiangyingdian = (Math.Exp(Math.Log(favg) + pub_function.qlogis((fq)) * fsigma));

                // 置信上限：
                xiangyingdian_shangxian = Math.Exp(Math.Log(favg) + pub_function.qlogis(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));

                // 置信下限：
                xiangyingdian_xiaxian = Math.Exp(Math.Log(favg) + pub_function.qlogis(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));

                xiangyingdian_dance_shangxian = Math.Exp(Math.Log(favg) + pub_function.qlogis(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));
                xiangyingdian_dance_xiaxian = Math.Exp(Math.Log(favg) + pub_function.qlogis(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));
                // 均值置信上限
                junzhishangxian = Math.Exp(Math.Log(favg) + Tfw * fsigmaavg);

                // 均值置信下限
                junzhixiaxian = Math.Exp(Math.Log(favg) - Tfw * fsigmaavg);

            }
            else
                                    if (fenbu == "逻辑斯谛分布log10")
            {
                xiangyingdian = Math.Pow(10, Math.Log10(favg) + pub_function.qlogis((fq)) * fsigma);

                // 置信上限：
                xiangyingdian_shangxian = Math.Pow(10, Math.Log10(favg) + pub_function.qlogis(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));

                // 置信下限：
                xiangyingdian_xiaxian = Math.Pow(10, Math.Log10(favg) + pub_function.qlogis(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));

                xiangyingdian_dance_shangxian = Math.Pow(10, Math.Log10(favg) + pub_function.qlogis(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));
                xiangyingdian_dance_xiaxian = Math.Pow(10, Math.Log10(favg) + pub_function.qlogis(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)));
                // 均值置信上限
                junzhishangxian = Math.Pow(10, Math.Log10(favg) + Tfw * fsigmaavg);

                // 均值置信下限
                junzhixiaxian = Math.Pow(10, Math.Log10(favg) - Tfw * fsigmaavg);

            }
            else
                                        if (fenbu == "逻辑斯谛分布幂")
            {
                xiangyingdian = (Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qlogis(fq) * fsigma, 1 / SysParam.updown_Power));


                // 置信上限：
                xiangyingdian_shangxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qlogis(fq) * fsigma + Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)), 1 / SysParam.updown_Power);

                // 置信下限：
                xiangyingdian_xiaxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qlogis(fq) * fsigma - Tfw * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)), 1 / SysParam.updown_Power);

                xiangyingdian_dance_shangxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qlogis(fq) * fsigma + Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)), 1 / SysParam.updown_Power);
                xiangyingdian_dance_xiaxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + pub_function.qlogis(fq) * fsigma - Tfw_dance * Math.Sqrt(Math.Pow(fsigmaavg, 2) + Math.Pow(fsigmasigma, 2) * Math.Pow(pub_function.qlogis(fq), 2)), 1 / SysParam.updown_Power);
                // 均值置信上限
                junzhishangxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) + Tfw * fsigmaavg, 1 / SysParam.updown_Power);

                // 均值置信下限
                junzhixiaxian = Math.Pow(Math.Pow(favg, SysParam.updown_Power) - Tfw * fsigmaavg, 1 / SysParam.updown_Power);

            }

            //标准差置信上限
            biaozhunchashangxian = fsigma + Tfw * fsigmasigma;

            //标准差置信下限
            biaozhunchaxiaxian = fsigma - Tfw * fsigmasigma;

            //}
            //else
            //{

            //}

        }

        public static void get_x(int xArrayLength, double[] xArray, out double[] xArray_change)
        {
            int ij;
            xArray_change = new double[xArrayLength];
            for (ij = 0; ij < xArrayLength; ij++)
            {
                xArray_change[ij] = xArray[ij];
            }

        }
        public static void get_x(int xArrayLength, int[] xArray, out int[] xArray_change)
        {
            int ij;
            xArray_change = new int[xArrayLength];
            for (ij = 0; ij < xArrayLength; ij++)
            {
                xArray_change[ij] = xArray[ij];
            }

        }
        public static char change_number(char a)
        {
            if (Convert.ToInt64(a) >= 65295 && Convert.ToInt64(a) <= 65306)
                a = Convert.ToChar(Convert.ToInt64(a) - 65248);
            if (Convert.ToInt64(a) == 12290)
                a = '.';
            return a;
        }
        public static void datagrid_change_save(string fangfa, int a, int b)
        {



        }
    }
}