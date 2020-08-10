using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Controllers.UpDowunMethod
{
    class MLR_polar
    {
        public struct Argument//自变量结构体 
        {
            public double mu;
            public double sigma;
        };

        public static void Likelihood_Ratio_批量计算(double[] xArray, int[] vArray, int xArrayLength, double mu, double sigma, double p_下, double p_上, int y轴_分割份数, double zhixinshuiping, string Distribute, double power, out double[] xiangyinggailv, out double[] 响应点, out double[] 下限, out double[] 上限)
        {

            //xArray = new double[xArrayLength];
            //vArray = new int[xArrayLength];
            double y轴_每个刻度长度 = (pub_function.qnorm(p_上) - pub_function.qnorm(p_下)) / y轴_分割份数;
            xiangyinggailv = new double[y轴_分割份数 + 1];
            for (int i = 0; i <= y轴_分割份数; i++)
            {
                if (i == 0)
                    xiangyinggailv[i] = p_下;
                else
                    xiangyinggailv[i] = pub_function.pnorm(pub_function.qnorm(p_下) + i * y轴_每个刻度长度, 0, 1);
            }

            double[] final_result = new double[8];

            double zhixin_xia, zhixin_shang, mu_xia, mu_shang, sigma_xia, sigma_shang;
            下限 = new double[xiangyinggailv.Length];
            上限 = new double[xiangyinggailv.Length];
            响应点 = new double[xiangyinggailv.Length];

            Double favg, fsigma, fq;
            favg = mu;
            fsigma = sigma;
            for (int i = 0; i < xiangyinggailv.Length; i++)
            {
                MLR_polar.Likelihood_Ratio(xiangyinggailv[i], zhixinshuiping, Distribute, power, xArrayLength, xArray, vArray, out zhixin_xia, out zhixin_shang, out mu_xia, out mu_shang, out sigma_xia, out sigma_shang);
                下限[i] = zhixin_xia;
                上限[i] = zhixin_shang;
                fq = xiangyinggailv[i];
                if (Distribute == "正态分布标准")
                    响应点[i] = favg + pub_function.qnorm(fq) * fsigma;
                if (Distribute == "正态分布Ln")
                    响应点[i] = (Math.Exp(Math.Log(favg) + pub_function.qnorm((fq)) * fsigma));
                if (Distribute == "正态分布log10")
                    响应点[i] = (Math.Pow(10, Math.Log10(favg) + pub_function.qnorm((fq)) * fsigma));
                if (Distribute == "正态分布幂")
                    响应点[i] = (Math.Pow(Math.Pow(favg, Convert.ToDouble(power)) + pub_function.qnorm(fq) * fsigma, 1 / Convert.ToDouble(power)));

                if (Distribute == "逻辑斯谛分布标准")
                    响应点[i] = (favg + pub_function.qlogis(fq) * fsigma);
                if (Distribute == "逻辑斯谛分布Ln")
                    响应点[i] = (Math.Exp(Math.Log(favg) + pub_function.qlogis((fq)) * fsigma));
                if (Distribute == "逻辑斯谛分布log10")
                    响应点[i] = (Math.Pow(10, Math.Log10(favg) + pub_function.qlogis((fq)) * fsigma));
                if (Distribute == "逻辑斯谛分布幂")
                    响应点[i] = (Math.Pow(Math.Pow(favg, Convert.ToDouble(power)) + pub_function.qlogis(fq) * fsigma, 1 / Convert.ToDouble(power)));
            }

        }


        public static void Likelihood_Ratio(double xiangyinggailv, double zhixinshuiping, string fenbu, double power, int xArrayLength, double[] xArray, int[] vArray, out double zhixin_xia, out double zhixin_shang, out double mu_xia, out double mu_shang, out double sigma_xia, out double sigma_shang)
        {
            double mu, sigma, Maxf, Mins, L;
            zhixin_xia = 0;
            zhixin_shang = 0;
            mu_xia = 0;
            mu_shang = 0;
            double[] final_result = new double[8];
            if (fenbu == "正态分布标准")
            {
                double[] xArray_c = new double[xArrayLength];
                int[] vArray_c = new int[xArrayLength];

                pub_function.get_x(xArrayLength, vArray, out vArray_c);
                pub_function.get_x(xArrayLength, xArray, out xArray_c);

                pub_function.norm_MLS_getMLS(xArray_c, vArray_c, out mu, out sigma, out Maxf, out Mins);

                MLR_polar.Likelihood_Ratio_Polar(xArray_c, vArray_c, "normal", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);
                zhixin_xia = final_result[5];
                zhixin_shang = final_result[4];
                mu_xia = final_result[1];
                mu_shang = final_result[0];

            }
            if (fenbu == "正态分布Ln")
            {
                double[] xArray_ln = new double[xArrayLength];
                int[] vArray_ln = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");
                pub_function.get_x(xArrayLength, vArray, out vArray_ln);

                pub_function.norm_MLS_getMLS(xArray_ln, vArray_ln, out mu, out sigma, out Maxf, out Mins);

                MLR_polar.Likelihood_Ratio_Polar(xArray_ln, vArray_ln, "normal", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Exp(final_result[5]);
                zhixin_shang = Math.Exp(final_result[4]);
                mu_xia = Math.Exp(final_result[1]);
                mu_shang = Math.Exp(final_result[0]);

            }
            if (fenbu == "逻辑斯谛分布标准")
            {
                double[] xArray_c = new double[xArrayLength];
                int[] vArray_c = new int[xArrayLength];

                pub_function.get_x(xArrayLength, vArray, out vArray_c);
                pub_function.get_x(xArrayLength, xArray, out xArray_c);

                MLR_polar.Max_Likelihood_Estimate(xArray_c, vArray_c, "logistic", out mu, out sigma, out L);

                MLR_polar.Likelihood_Ratio_Polar(xArray_c, vArray_c, "logistic", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = final_result[5];
                zhixin_shang = final_result[4];
                mu_xia = final_result[1];
                mu_shang = final_result[0];

            }

            if (fenbu == "逻辑斯谛分布Ln")
            {
                double[] xArray_ln = new double[xArrayLength];
                int[] vArray_ln = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");
                pub_function.get_x(xArrayLength, vArray, out vArray_ln);
                MLR_polar.Max_Likelihood_Estimate(xArray_ln, vArray_ln, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_ln, vArray_ln, "logistic", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Exp(final_result[5]);
                zhixin_shang = Math.Exp(final_result[4]);
                mu_xia = Math.Exp(final_result[1]);
                mu_shang = Math.Exp(final_result[0]);
            }
            if (fenbu == "正态分布log10")
            {
                double[] xArray_log10 = new double[xArrayLength];
                int[] vArray_log10 = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_log10);
                pub_function.norm_MLS_getMLS(xArray_log10, vArray_log10, out mu, out sigma, out Maxf, out Mins);
                MLR_polar.Likelihood_Ratio_Polar(xArray_log10, vArray_log10, "normal", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(10, final_result[5]);
                zhixin_shang = Math.Pow(10, final_result[4]);
                mu_xia = Math.Pow(10, final_result[1]);
                mu_shang = Math.Pow(10, final_result[0]);


            }
            if (fenbu == "逻辑斯谛分布log10")
            {
                double[] xArray_log10 = new double[xArrayLength];
                int[] vArray_log10 = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_log10);
                MLR_polar.Max_Likelihood_Estimate(xArray_log10, vArray_log10, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_log10, vArray_log10, "logistic", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(10, final_result[5]);
                zhixin_shang = Math.Pow(10, final_result[4]);
                mu_xia = Math.Pow(10, final_result[1]);
                mu_shang = Math.Pow(10, final_result[0]);

            }
            if (fenbu == "正态分布幂")
            {
                double[] xArray_pow = new double[xArrayLength];
                int[] vArray_pow = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_pow(xArrayLength, power, xArray, out xArray_pow);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_pow);

                pub_function.norm_MLS_getMLS(xArray_pow, vArray_pow, out mu, out sigma, out Maxf, out Mins);
                MLR_polar.Likelihood_Ratio_Polar(xArray_pow, vArray_pow, "normal", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(final_result[5], 1 / power);
                zhixin_shang = Math.Pow(final_result[4], 1 / power);
                mu_xia = Math.Pow(final_result[1], 1 / power);
                mu_shang = Math.Pow(final_result[0], 1 / power);



            }

            if (fenbu == "逻辑斯谛分布幂")
            {
                double[] xArray_pow = new double[xArrayLength];
                int[] vArray_pow = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_pow(xArrayLength, power, xArray, out xArray_pow);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_pow);
                MLR_polar.Max_Likelihood_Estimate(xArray_pow, vArray_pow, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_pow, vArray_pow, "logistic", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(final_result[5], 1 / power);
                zhixin_shang = Math.Pow(final_result[4], 1 / power);
                mu_xia = Math.Pow(final_result[1], 1 / power);
                mu_shang = Math.Pow(final_result[0], 1 / power);
            }

            sigma_xia = final_result[3];
            sigma_shang = final_result[2];
        }

        public static void Likelihood_Ratio(double mu, double sigma, double xiangyinggailv, double zhixinshuiping, string fenbu, double power, int xArrayLength, double[] xArray, int[] vArray, out double zhixin_xia, out double zhixin_shang, out double mu_xia, out double mu_shang, out double sigma_xia, out double sigma_shang)
        {
            double Maxf, Mins, L;
            zhixin_xia = 0;
            zhixin_shang = 0;
            mu_xia = 0;
            mu_shang = 0;
            double[] final_result = new double[8];
            if (fenbu == "正态分布标准")
            {
                double[] xArray_c = new double[xArrayLength];
                int[] vArray_c = new int[xArrayLength];
                pub_function.get_x(xArrayLength, vArray, out vArray_c);
                pub_function.get_x(xArrayLength, xArray, out xArray_c);
                //pub_function.norm_MLS_getMLS(xArray_c, vArray_c, out mu, out sigma, out Maxf, out Mins);

                MLR_polar.Likelihood_Ratio_Polar(xArray_c, vArray_c, "normal", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);
                zhixin_xia = final_result[5];
                zhixin_shang = final_result[4];
                mu_xia = final_result[1];
                mu_shang = final_result[0];

            }
            if (fenbu == "正态分布Ln")
            {
                double[] xArray_ln = new double[xArrayLength];
                int[] vArray_ln = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");
                pub_function.get_x(xArrayLength, vArray, out vArray_ln);
                //pub_function.norm_MLS_getMLS(xArray_ln, vArray_ln, out mu, out sigma, out Maxf, out Mins);
                MLR_polar.Likelihood_Ratio_Polar(xArray_ln, vArray_ln, "normal", Math.Log(mu), sigma, xiangyinggailv, zhixinshuiping, out final_result);
                zhixin_xia = Math.Exp(final_result[5]);
                zhixin_shang = Math.Exp(final_result[4]);
                mu_xia = Math.Exp(final_result[1]);
                mu_shang = Math.Exp(final_result[0]);

            }
            if (fenbu == "正态分布log10")
            {
                double[] xArray_log10 = new double[xArrayLength];
                int[] vArray_log10 = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_log10);
                //pub_function.norm_MLS_getMLS(xArray_log10, vArray_log10, out mu, out sigma, out Maxf, out Mins);
                MLR_polar.Likelihood_Ratio_Polar(xArray_log10, vArray_log10, "normal", Math.Log10(mu), sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(10, final_result[5]);
                zhixin_shang = Math.Pow(10, final_result[4]);
                mu_xia = Math.Pow(10, final_result[1]);
                mu_shang = Math.Pow(10, final_result[0]);


            }
            if (fenbu == "正态分布幂")
            {
                double[] xArray_pow = new double[xArrayLength];
                int[] vArray_pow = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_pow(xArrayLength, power, xArray, out xArray_pow);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_pow);
                //pub_function.norm_MLS_getMLS(xArray_pow, vArray_pow, out mu, out sigma, out Maxf, out Mins);
                MLR_polar.Likelihood_Ratio_Polar(xArray_pow, vArray_pow, "normal", Math.Pow(mu, power), sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(final_result[5], 1 / power);
                zhixin_shang = Math.Pow(final_result[4], 1 / power);
                mu_xia = Math.Pow(final_result[1], 1 / power);
                mu_shang = Math.Pow(final_result[0], 1 / power);



            }
            if (fenbu == "逻辑斯谛分布标准")
            {
                double[] xArray_c = new double[xArrayLength];
                int[] vArray_c = new int[xArrayLength];

                pub_function.get_x(xArrayLength, vArray, out vArray_c);
                pub_function.get_x(xArrayLength, xArray, out xArray_c);
                //MLR_polar.Max_Likelihood_Estimate(xArray_c, vArray_c, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_c, vArray_c, "logistic", mu, sigma, xiangyinggailv, zhixinshuiping, out final_result);
                zhixin_xia = final_result[5];
                zhixin_shang = final_result[4];
                mu_xia = final_result[1];
                mu_shang = final_result[0];

            }
            if (fenbu == "逻辑斯谛分布Ln")
            {
                double[] xArray_ln = new double[xArrayLength];
                int[] vArray_ln = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");
                pub_function.get_x(xArrayLength, vArray, out vArray_ln);
                //MLR_polar.Max_Likelihood_Estimate(xArray_ln, vArray_ln, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_ln, vArray_ln, "logistic", Math.Log(mu), sigma, xiangyinggailv, zhixinshuiping, out final_result);
                zhixin_xia = Math.Exp(final_result[5]);
                zhixin_shang = Math.Exp(final_result[4]);
                mu_xia = Math.Exp(final_result[1]);
                mu_shang = Math.Exp(final_result[0]);
            }

            if (fenbu == "逻辑斯谛分布log10")
            {
                double[] xArray_log10 = new double[xArrayLength];
                int[] vArray_log10 = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_log10);
                //MLR_polar.Max_Likelihood_Estimate(xArray_log10, vArray_log10, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_log10, vArray_log10, "logistic", Math.Log10(mu), sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(10, final_result[5]);
                zhixin_shang = Math.Pow(10, final_result[4]);
                mu_xia = Math.Pow(10, final_result[1]);
                mu_shang = Math.Pow(10, final_result[0]);

            }
            if (fenbu == "逻辑斯谛分布幂")
            {
                double[] xArray_pow = new double[xArrayLength];
                int[] vArray_pow = new int[xArrayLength];
                if (xArrayLength > 0)
                    pub_function.get_xArray_pow(xArrayLength, power, xArray, out xArray_pow);
                else
                    throw new Exception("数据错误！");
                //MessageBox.Show("数据错误！");

                pub_function.get_x(xArrayLength, vArray, out vArray_pow);
                //MLR_polar.Max_Likelihood_Estimate(xArray_pow, vArray_pow, "logistic", out mu, out sigma, out L);
                MLR_polar.Likelihood_Ratio_Polar(xArray_pow, vArray_pow, "logistic", Math.Pow(mu, power), sigma, xiangyinggailv, zhixinshuiping, out final_result);

                zhixin_xia = Math.Pow(final_result[5], 1 / power);
                zhixin_shang = Math.Pow(final_result[4], 1 / power);
                mu_xia = Math.Pow(final_result[1], 1 / power);
                mu_shang = Math.Pow(final_result[0], 1 / power);
            }

            sigma_xia = final_result[3];
            sigma_shang = final_result[2];
        }


        public static void Max_Likelihood_Estimate(double[] xArray, int[] vArray, string type, out double mu, out double sigma, out double L)
        {
            Argument point0;
            if (xArray.Length == vArray.Length)
            {
                max_likelihood_firstvalue(xArray, vArray, type, out point0.mu, out point0.sigma);
                // pub_function.MAX_getMax( pub_function.getF(xArray, vArray))<pub_function.MIN_getMin(pub_function.S_getS(xArray, vArray));

                if (point0.sigma > 0)
                    L = -1 * fminsearch(ref point0, xArray, vArray, type);
                else
                {
                    L = 0;
                    point0.sigma = 0;
                }
                mu = point0.mu;
                sigma = point0.sigma;
            }
            else
            {
                //MessageBox.Show("输入的两数组长度不一致");
                L = 0;
                mu = 0;
                sigma = 0;
                //Application.Exit();
            }

        }

        public static void Likelihood_Ratio_Polar(double[] Z, int[] Result, string type, double mu_final, double sigma_final, double probability, double confidence, out double[] final_result)
        {
            if (Z.Length == Result.Length)
            {
                double alpha;//confidence 
                double up;//正态分布p分位数 
                double L;//对数似然函数极大值 
                double upper = 1e7;
                double lower = 1e-10;


                double upper1 = pub_function.MAX_getMax(Z);
                double lower1 = pub_function.MIN_getMin(Z);

                if (Math.Abs(upper1) > Math.Abs(lower1))
                    upper1 = Math.Abs(upper1);
                else
                    upper1 = Math.Abs(lower1);

                upper1 = upper1 * 100;

                double mu0;
                double sigma0;
                int i, j, k;
                alpha = chi2inv(confidence, 1);
                if (type == "normal")
                {
                    up = norminv(probability, 0, 1);
                }
                else up = logisinv(probability, 0, 1);
                Argument point0;
                point0.mu = mu_final;
                point0.sigma = sigma_final;
                mu0 = point0.mu;
                sigma0 = point0.sigma;
                if (sigma_final != 0)
                {
                    L = -1 * fun(point0, Z, Result, type);
                }
                else
                {
                    L = Math.Log(0.25);
                }


                int N = 100;//极坐标中极角的等份数 
                double[] mu = new double[2 * N];
                double[] sigma = new double[2 * N];
                double a, b;
                i = 0;
                for (j = 0; j < N; j++)
                {
                    double theta = -Math.PI / 2 + j * (Math.PI / N);
                    //计算上侧零点

                    sigma[i] = solve(sigma0, upper, theta, Z, Result, type, alpha, point0.mu, point0.sigma, L);
                    sigma[i + N] = solve(sigma0, lower, theta, Z, Result, type, alpha, point0.mu, point0.sigma, L);
                    if (Math.Abs(theta) == Math.PI / 2)
                    {
                        mu[i] = mu0;
                        mu[i + N] = mu0;
                    }
                    else
                    {
                        if (sigma[i] == upper)
                        {
                            mu[i] = sign(Math.Tan(theta)) * upper;
                        }
                        else
                        {
                            mu[i] = Math.Tan(theta) * (sigma[i] - sigma0) + mu0;
                        }
                        if (sigma[i + N] == lower)
                        {
                            mu[i + N] = Math.Tan(theta) * (-sigma0) + mu0;
                        }
                        else
                        {
                            mu[i + N] = Math.Tan(theta) * (sigma[i + N] - sigma0) + mu0;
                        }
                    }
                    i++;
                }

                //for (j = 0; j < N; j++)
                //{
                //    double theta = -Math.PI / 2 + j * (Math.PI / N);

                //    //计算下侧零点
                //    a = sigma0;
                //    b = lower;
                //    sigma[i] = solve(a, b, theta, Z, Result, type, alpha, point0.mu, point0.sigma, L);

                //    if (Math.Abs(theta) == Math.PI / 2)
                //    {
                //        mu[i] = mu0;
                //    }
                //    else
                //    {
                //        if (sigma[i] == lower)
                //        {
                //            mu[i] = Math.Tan(theta) * (- sigma0) + mu0;
                //        }
                //        else
                //        {
                //            mu[i] = Math.Tan(theta) * (sigma[i] - sigma0) + mu0;
                //        }
                //    }
                //    i++;
                //}

                double[] bounds = new double[8];//依次存放max(mu),min(mu),max(sigma),min(sigma),max(mu+up*sigma),min(mu+up*sigma), max(mu-up*sigma),min(mu-up*sigma)
                int[] Index = new int[8]; //为各个极值在边界数组中对应的位置

                bounds[0] = max(mu, 2 * N, ref Index[0]);
                bounds[1] = min(mu, 2 * N, ref Index[1]);
                bounds[2] = max(sigma, 2 * N, ref Index[2]);
                bounds[3] = min(sigma, 2 * N, ref Index[3]);

                double[] temp = new double[2 * N];
                for (i = 0; i < 2 * N; i++)
                {
                    temp[i] = mu[i] + up * sigma[i];
                }
                bounds[4] = max(temp, 2 * N, ref Index[4]);
                bounds[5] = min(temp, 2 * N, ref Index[5]);

                for (i = 0; i < 2 * N; i++)
                {
                    temp[i] = mu[i] - up * sigma[i];
                }
                bounds[6] = max(temp, 2 * N, ref Index[6]);
                bounds[7] = min(temp, 2 * N, ref Index[7]);


                //二次优化
                double[] tempmu = new double[N];
                double[] tempsigma = new double[N];
                for (i = 0; i < 8; i++)
                {
                    if (Math.Abs(bounds[i]) < upper)
                    {
                        double theta1 = -Math.PI / 2 + (Index[i] % N - 1) * Math.PI / N;
                        double theta2 = -Math.PI / 2 + (Index[i] % N + 1) * Math.PI / N;
                        double delta = theta2 - theta1;

                        j = 0;
                        for (k = 0; k < N; k++)
                        {
                            double theta = theta1 + k * delta / N;
                            a = sigma0;
                            if (Index[i] < N)
                            {
                                b = upper;
                            }
                            else
                            {
                                b = lower;
                            }

                            tempsigma[j] = solve(a, b, theta, Z, Result, type, alpha, point0.mu, point0.sigma, L);

                            if (Math.Abs(theta) == Math.PI / 2)
                            {
                                tempmu[j] = mu0;
                            }
                            else
                            {
                                if (tempsigma[j] == upper)
                                {
                                    tempmu[j] = sign(Math.Tan(theta)) * upper;
                                }
                                else
                                {
                                    tempmu[j] = Math.Tan(theta) * (tempsigma[j] - sigma0) + mu0;
                                }
                            }
                            j++;
                        }

                        double tempbound;
                        int tempI = 0;
                        switch (i)
                        {
                            case 0:
                                tempbound = max(tempmu, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 1:
                                tempbound = min(tempmu, N, ref tempI);
                                if (tempbound < bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 2:
                                tempbound = max(tempsigma, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 3:
                                tempbound = min(tempsigma, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 4:
                                for (j = 0; j < N; j++)
                                {
                                    temp[j] = tempmu[j] + up * tempsigma[j];
                                }
                                tempbound = max(temp, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 5:
                                for (j = 0; j < N; j++)
                                {
                                    temp[j] = tempmu[j] + up * tempsigma[j];
                                }
                                tempbound = min(temp, N, ref tempI);
                                if (tempbound < bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 6:
                                //for (j = 0; j < N; j++)
                                //{
                                //    temp[j] = tempmu[j] - up * tempsigma[j];
                                //}
                                //tempbound = max(temp, N, ref tempI);
                                //if (tempbound > bounds[i])
                                //{
                                //    bounds[i] = tempbound;
                                //    mu[Index[i]] = tempmu[tempI];
                                //    sigma[Index[i]] = tempsigma[tempI];
                                //}
                                break;

                            case 7:
                                //for (j = 0; j < N; j++)
                                //{
                                //    temp[j] = tempmu[j] - up * tempsigma[j];
                                //}
                                //tempbound = min(temp, N, ref tempI);
                                //if (tempbound < bounds[i])
                                //{
                                //    bounds[i] = tempbound;
                                //    mu[Index[i]] = tempmu[tempI];
                                //    sigma[Index[i]] = tempsigma[tempI];
                                //}
                                break;
                                //default : ;
                        }

                        theta2 = theta1 + (tempI + 1) * delta / N;//此两行顺序不能颠倒
                        theta1 = theta1 + (tempI - 1) * delta / N;
                        delta = theta2 - theta1;

                        j = 0;
                        for (k = 0; k < N; k++)
                        {
                            double theta = theta1 + k * delta / N;
                            a = sigma0;
                            if (Index[i] < N)
                            {
                                b = upper;
                            }
                            else
                            {
                                b = lower;
                            }

                            tempsigma[j] = solve(a, b, theta, Z, Result, type, alpha, point0.mu, point0.sigma, L);

                            if (Math.Abs(theta) == Math.PI / 2)
                            {
                                tempmu[j] = mu0;
                            }
                            else
                            {
                                if (tempsigma[j] == upper)
                                {
                                    tempmu[j] = sign(Math.Tan(theta)) * upper;
                                }
                                else
                                {
                                    tempmu[j] = Math.Tan(theta) * (tempsigma[j] - sigma0) + mu0;
                                }
                            }
                            j++;
                        }

                        switch (i)
                        {
                            case 0:
                                tempbound = max(tempmu, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 1:
                                tempbound = min(tempmu, N, ref tempI);
                                if (tempbound < bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 2:
                                tempbound = max(tempsigma, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 3:
                                tempbound = min(tempsigma, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 4:
                                for (j = 0; j < N; j++)
                                {
                                    temp[j] = tempmu[j] + up * tempsigma[j];
                                }
                                tempbound = max(temp, N, ref tempI);
                                if (tempbound > bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 5:
                                for (j = 0; j < N; j++)
                                {
                                    temp[j] = tempmu[j] + up * tempsigma[j];
                                }
                                tempbound = min(temp, N, ref tempI);
                                if (tempbound < bounds[i])
                                {
                                    bounds[i] = tempbound;
                                    mu[Index[i]] = tempmu[tempI];
                                    sigma[Index[i]] = tempsigma[tempI];
                                }
                                break;

                            case 6:
                                //for (j = 0; j < N; j++)
                                //{
                                //    temp[j] = tempmu[j] - up * tempsigma[j];
                                //}
                                //tempbound = max(temp, N, ref tempI);
                                //if (tempbound > bounds[i])
                                //{
                                //    bounds[i] = tempbound;
                                //    mu[Index[i]] = tempmu[tempI];
                                //    sigma[Index[i]] = tempsigma[tempI];
                                //}
                                break;

                            case 7:
                                //for (j = 0; j < N; j++)
                                //{
                                //    temp[j] = tempmu[j] - up * tempsigma[j];
                                //}
                                //tempbound = min(temp, N, ref tempI);
                                //if (tempbound < bounds[i])
                                //{
                                //    bounds[i] = tempbound;
                                //    mu[Index[i]] = tempmu[tempI];
                                //    sigma[Index[i]] = tempsigma[tempI];
                                //}
                                break;
                        }

                    }
                }

                for (i = 0; i < 8; i++)
                {
                    if (bounds[i] >= upper1)
                    {
                        bounds[i] = 1e308 * 100;
                    }
                    if (bounds[i] <= -upper1)
                    {
                        bounds[i] = -1e308 * 100;
                    }
                }
                final_result = bounds;
            }
            else
            {
                //MessageBox.Show("输入的两数组长度不一致");
                double[] bounds = { 0, 0, 0, 0, 0, 0, 0 };
                final_result = bounds;
                //Application.Exit();
            }
        }



        public static void max_likelihood_firstvalue(double[] xArray, int[] vArray, string type, out double mu_firstvalue, out double sigma_firstvalue)
        {
            double MaxF = pub_function.MAX_getMax(pub_function.getF(xArray, vArray));
            double MinS = pub_function.MIN_getMin(pub_function.S_getS(xArray, vArray));
            int Nm = pub_function.getNM(xArray, vArray);
            mu_firstvalue = 0.5 * (MaxF + MinS);
            if (type == "normal")
                sigma_firstvalue = xArray.Length * (MaxF - MinS) / (8 * (Nm + 2));
            else if (type == "logistic")
                sigma_firstvalue = xArray.Length * (MaxF - MinS) / (4 * (Nm + 2));
            else
                sigma_firstvalue = -1;
        }

        public static double fminsearch(ref Argument point, double[] xArray, int[] vArray, string type)
        {

            double delta1 = 0.05;
            double delta2 = 0.00025;
            double eps = 1e-12;

            double rho = 1;
            double chi = 2;
            double psi = 0.5;
            double sigma = 0.5;

            Argument B;
            Argument G;
            Argument W;
            Argument M;
            Argument R;
            Argument E;
            Argument C;
            Argument S;

            int i, j;
            Argument[] vertex = new Argument[3];
            vertex[0].mu = point.mu;
            vertex[0].sigma = point.sigma;

            if (vertex[0].mu != 0) vertex[1].mu = (1 + delta1) * vertex[0].mu;
            else vertex[1].mu = delta2;
            vertex[1].sigma = vertex[0].sigma;

            vertex[2].mu = vertex[0].mu;
            if (vertex[0].sigma != 0) vertex[2].sigma = (1 + delta1) * vertex[0].sigma;
            else vertex[2].sigma = delta2;


            double[] z = { fun(vertex[0], xArray, vArray, type), fun(vertex[1], xArray, vArray, type), fun(vertex[2], xArray, vArray, type) };

            double temp;
            Argument temp2;
            for (i = 0; i < 2; i++)
            {
                for (j = i + 1; j < 3; j++)
                {
                    if (z[i] > z[j])
                    {
                        temp = z[i];
                        z[i] = z[j];
                        z[j] = temp;

                        temp2 = vertex[i];
                        vertex[i] = vertex[j];
                        vertex[j] = temp2;
                    }
                }
            }

            B = vertex[0];
            G = vertex[1];
            W = vertex[2];

            int step = 0;
            double size;

            do
            {
                M.mu = (B.mu + G.mu) / 2.0;
                M.sigma = (B.sigma + G.sigma) / 2.0;

                R.mu = (1 + rho) * M.mu - rho * W.mu;
                R.sigma = (1 + rho) * M.sigma - rho * W.sigma;

                if (fun(R, xArray, vArray, type) < fun(G, xArray, vArray, type))//either reflect or extend
                {
                    if (fun(B, xArray, vArray, type) <= fun(R, xArray, vArray, type))
                    {
                        W = R;
                    }
                    else
                    {
                        E.mu = chi * R.mu + (1 - chi) * M.mu;
                        E.sigma = chi * R.sigma + (1 - chi) * M.sigma;

                        if (fun(E, xArray, vArray, type) < fun(R, xArray, vArray, type))
                        {
                            W = E;
                        }
                        else
                        {
                            W = R;
                        }
                    }
                }
                else//either contract or shrink
                {
                    if (fun(R, xArray, vArray, type) < fun(W, xArray, vArray, type))
                    {
                        C.mu = (1 + psi * rho) * M.mu - rho * psi * W.mu;
                        C.sigma = (1 + psi * rho) * M.sigma - rho * psi * W.sigma;
                        if (fun(C, xArray, vArray, type) <= fun(R, xArray, vArray, type))
                        {
                            W = C;
                        }
                        else
                        {
                            S.mu = B.mu + sigma * (W.mu - B.mu);
                            S.sigma = B.sigma + sigma * (W.sigma - B.sigma);

                            W = S;
                            G = M;
                        }
                    }
                    else
                    {
                        C.mu = (1 - psi) * M.mu + psi * W.mu;
                        C.sigma = (1 - psi) * M.sigma + psi * W.sigma;

                        if (fun(C, xArray, vArray, type) < fun(W, xArray, vArray, type))
                        {
                            W = C;
                        }
                        else
                        {
                            S.mu = B.mu + sigma * (W.mu - B.mu);
                            S.sigma = B.sigma + sigma * (W.sigma - B.sigma);

                            W = S;
                            G = M;
                        }
                    }
                }

                vertex[0] = B;
                vertex[1] = G;
                vertex[2] = W;

                z[0] = fun(vertex[0], xArray, vArray, type);
                z[1] = fun(vertex[1], xArray, vArray, type);
                z[2] = fun(vertex[2], xArray, vArray, type);

                for (i = 0; i < 2; i++)
                {
                    for (j = i + 1; j < 3; j++)
                    {
                        if (z[i] > z[j])
                        {
                            temp = z[i];
                            z[i] = z[j];
                            z[j] = temp;

                            temp2 = vertex[i];
                            vertex[i] = vertex[j];
                            vertex[j] = temp2;
                        }
                    }
                }

                B = vertex[0];
                G = vertex[1];
                W = vertex[2];

                size = Math.Max(Math.Abs(G.mu - B.mu), Math.Abs(G.sigma - B.sigma));
                size = Math.Max(size, Math.Abs(W.mu - B.mu));
                size = Math.Max(size, Math.Abs(W.sigma - B.sigma));

                step++;
            } while (size > eps || (fun(W, xArray, vArray, type) - fun(B, xArray, vArray, type)) > eps);

            point = B;
            return fun(B, xArray, vArray, type);
        }

        public static double fun(Argument point, double[] xArray, int[] vArray, string type)
        {
            double mu = point.mu;
            double sigma = point.sigma;
            double f = 1.0;
            int i = 0;
            double[] y = pub_function.getF(xArray, vArray);
            double[] x = pub_function.S_getS(xArray, vArray);
            if (type == "normal")
            {
                for (i = 0; i < x.Length; i++)
                {
                    f *= normcdf(x[i], mu, sigma);
                }
                for (i = 0; i < y.Length; i++)
                {
                    f *= 1 - normcdf(y[i], mu, sigma);
                }
                return -Math.Log(f);
            }
            else if (type == "logistic")
            {
                for (i = 0; i < x.Length; i++)
                {
                    f *= logispdf(x[i], mu, sigma);
                }
                for (i = 0; i < y.Length; i++)
                {
                    f *= 1 - logispdf(y[i], mu, sigma);
                }
                return -Math.Log(f);
            }
            else
            {
                return 0;
            }
        }


        public static double fun2(double sigma, double theta, double[] xArray, int[] vArray, string type, double alpha, double u0final, double o0final, double L)
        {
            double mu;
            double[] y = pub_function.getF(xArray, vArray);
            double[] x = pub_function.S_getS(xArray, vArray);
            if (Math.Abs(theta) == Math.PI / 2 && theta != 0)
            {
                mu = u0final;
            }
            else
            {
                mu = Math.Tan(theta) * (sigma - o0final) + u0final;
            }

            double f = 1.0;
            int i = 0;
            if (type == "normal")
            {
                for (i = 0; i < x.Length; i++)
                {
                    f *= normcdf(x[i], mu, sigma);
                }
                for (i = 0; i < y.Length; i++)
                {
                    f *= 1 - normcdf(y[i], mu, sigma);
                }
                return Math.Log(f) - L + 0.5 * alpha;
            }
            else if (type == "logistic")
            {
                for (i = 0; i < x.Length; i++)
                {
                    f *= logispdf(x[i], mu, sigma);
                }
                for (i = 0; i < y.Length; i++)
                {
                    f *= 1 - logispdf(y[i], mu, sigma);
                }
                return Math.Log(f) - L + 0.5 * alpha;
            }
            else
            {
                return 0;
            }

        }


        public static double solve(double a, double b, double theta, double[] xArray, int[] vArray, string type, double alpha, double u0final, double o0final, double L)
        {
            double eps = 1e-8;
            double sigma;
            if (fun2(a, theta, xArray, vArray, type, alpha, u0final, o0final, L) * fun2(b, theta, xArray, vArray, type, alpha, u0final, o0final, L) >= 0)
            {
                sigma = b;
                return sigma;
            }

            while (Math.Abs(b - a) > eps)
            {
                if (fun2((a + b) / 2, theta, xArray, vArray, type, alpha, u0final, o0final, L) == 0)
                {
                    sigma = (a + b) / 2;
                    return sigma;
                }
                else if (fun2(a, theta, xArray, vArray, type, alpha, u0final, o0final, L) * fun2((a + b) / 2, theta, xArray, vArray, type, alpha, u0final, o0final, L) < 0)
                {
                    b = (a + b) / 2;
                }
                else
                {
                    a = (a + b) / 2;
                }
            }

            sigma = (a + b) / 2;
            return sigma;
        }





        public static double sign(double x)
        {
            if (x > 0) return 1;
            else if (x < 0) return -1;
            else return 0;
        }



        public static double mean(double[] a)
        {
            int i;
            double aver = 0.0;
            for (i = 0; i < a.Length; i++)
            {
                aver += a[i];
            }
            return aver / a.Length;
        }


        static double normpdf(double x, double mu, double sigma)
        {
            x = (x - mu) / sigma;
            return 0.398942280401433 * Math.Exp(-x * x / 2);
        }

        static double normcdf(double x, double mu, double sigma)
        {
            double result;
            x = (x - mu) / sigma;

            if (x < -7)
                result = normpdf(x, 0, 1) / Math.Sqrt(1 + x * x);
            else if (x > 7)
                result = 1 - normcdf(-x, 0, 1);
            else
            {
                result = 0.2316419;
                double[] a = { 0.31938153, -0.356563782, 1.781477937, -1.821255978, 1.330274429 };
                result = 1 / (1 + result * Math.Abs(x));
                result = 1 - normpdf(x, 0, 1) * (result * (a[0] + result * (a[1] + result * (a[2]
                    + result * (a[3] + result * a[4])))));
                if (x <= 0) result = 1 - result;
            }
            return result;
        }

        //public static double norminv(double q, double mu, double sigma)
        //{
        //    if (sigma <= 0)
        //    {
        //        return 1e+10;
        //    }
        //    if (q < 0 || q > 1)
        //    {
        //        return 1e+10;
        //    }

        //    if (q == 0.5) return mu;
        //    else q = 1.0 - q;

        //    double p;
        //    if (q > 0.0 && q < 0.5)
        //    {
        //        p = q;
        //    }
        //    else
        //    {
        //        if (q == 1.0) p = 1 - 0.9999999;
        //        else p = 1 - q;
        //    }

        //    double t = Math.Sqrt(Math.Log(1.0 / Math.Pow(p, 2.0)));

        //    double c0 = 2.515517;
        //    double c1 = 0.802853;
        //    double c2 = 0.010328;

        //    double d1 = 1.432788;
        //    double d2 = 0.189269;
        //    double d3 = 0.001308;

        //    double x = t - (c0 + c1 * t + c2 * Math.Pow(t, 2.0)) / (1.0 + d1 * t + d2 * Math.Pow(t, 2.0) + d3 * Math.Pow(t, 3.0));

        //    if (q > 0.5) x *= -1.0;

        //    return x * sigma + mu;
        //}
        public static double norminv(double p, double mu, double sigma)
        {
            if (p < 0) return 0;
            double x, y;
            double E = 0.0000000000001;
            double delta;
            double min, max;
            if (p == 0.5) return 0;
            if (p == 0.99) return 2.32634787404084 * sigma + mu;
            if (p == 0.01) return -2.32634787404084 * sigma + mu;
            if (p == 0.999) return 3.09023230616781 * sigma + mu;
            if (p == 0.001) return -3.09023230616781 * sigma + mu;
            if (p == 0.9999) return 3.71901648545571 * sigma + mu;
            if (p == 0.0001) return -3.71901648545571 * sigma + mu;
            if (p == 0.99999) return 4.26489079392384 * sigma + mu;
            if (p == 0.00001) return -4.26489079392384 * sigma + mu;
            if (p == 0.999999) return 4.75342430881709 * sigma + mu;
            if (p == 0.000001) return -4.75342430881709 * sigma + mu;
            if (p == 0.9999999) return 5.19933758229066 * sigma + mu;
            if (p == 0.0000001) return -5.19933758229066 * sigma + mu;
            if (p == 0.99999999) return 5.61200124330550 * sigma + mu;
            if (p == 0.00000001) return -5.61200124330550 * sigma + mu;
            if (p == 0.999999999) return 5.99780701960164 * sigma + mu;
            if (p == 0.000000001) return -5.99780701960164 * sigma + mu;
            if (p == 0.9999999999) return 6.36134088969742 * sigma + mu;
            if (p == 0.0000000001) return -6.36134088969742 * sigma + mu;
            if ((p > 0.999999999999) & (p < 0.000000000001)) return 0;
            delta = 0;
            min = -9;
            max = 9;
            y = p;
            x = normcdf(delta, 0, 1);

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
                x = normcdf((min + max) / 2, 0, 1);
            }
            return ((min + max) / 2) * sigma + mu;
        }

        static double logispdf(double x, double mu, double sigma)
        {
            return 1.0 / (1.0 + Math.Exp(-1.813799364234218 * (x - mu) / sigma));
        }

        public static double logisinv(double x, double mu, double sigma)
        {
            if ((x < 1) && (x > 0))
                return mu - sigma * Math.Log(1 / x - 1);
            else
            {
                //MessageBox.Show("概率必须在0-1");
                return -1;
            }

        }


        public static double gamma(double x)
        {

            int i;
            double y, t, s, u;
            double[] a ={0.0000677106,-0.0003442342,
                         0.0015397681,-0.0024467480,
                         0.0109736958,-0.0002109075,
                         0.0742379071,0.0815782188,
                         0.4118402518,0.4227843370,1.0};
            if (x <= 0.0)
            {
                return (-1.0);
            }
            y = x;
            if (y <= 1.0)
            {
                t = 1.0 / (y * (y + 1.0));
                y = y + 2.0;
            }
            else if (y <= 2.0)
            {
                t = 1.0 / y;
                y = y + 1.0;
            }
            else if (y <= 3.0) t = 1.0;
            else
            {
                t = 1.0;
                while (y > 3.0)
                {
                    y = y - 1.0;
                    t = t * y;
                }
            }
            s = a[0];
            u = y - 2.0;
            for (i = 1; i <= 10; i++)
            {
                s = s * u + a[i];
            }
            s = s * t;
            return (s);
        }

        public static double tpdf(double x, double n)
        {
            return gamma((n + 1.0) / 2.0) / (Math.Sqrt(n * Math.PI) * gamma(n / 2.0)) * Math.Pow(1 + x * x / n, -(n + 1) / 2.0);
        }

        public static double tcdf(double x, double n)
        {
            if (n <= 0)
            {
                return -1.0;
            }

            double delta1 = 1.0;
            double delta2 = 100;
            double[] xk = { -0.7745966692, 0, 0.7745966692 };
            double[] ak = { 0.5555555556, 0.8888888889, 0.5555555556 };

            if (x == 0.0) return 0.5;
            else if (x < 0.0) return 1.0 - tcdf(-x, n);
            else
            {
                if (x <= 100)
                {
                    double p = 0.0;
                    int xn = (int)(x / delta1) + 1;
                    double delta = x / xn;
                    double a, b;
                    int i = 0, j = 0;
                    for (i = 0; i < xn; i++)
                    {
                        a = i * delta;
                        b = a + delta;
                        for (j = 0; j < 3; j++)
                        {
                            p += tpdf((b - a) / 2.0 * xk[j] + (b + a) / 2.0, n) * ak[j] * delta / 2.0;
                        }
                    }
                    return p + 0.5;
                }
                else
                {
                    double p = tcdf(100, n);
                    double y = x - 100;
                    int xn = (int)(y / delta2) + 1;
                    double delta = y / xn;
                    double a, b;
                    int i = 0, j = 0;
                    for (i = 0; i < xn; i++)
                    {
                        a = 100 + i * delta;
                        b = a + delta;
                        for (j = 0; j < 3; j++)
                        {
                            p += tpdf((b - a) / 2.0 * xk[j] + (b + a) / 2.0, n) * ak[j] * delta / 2.0;
                        }
                    }
                    return p;
                }
            }
        }

        public static double tinv(double q, double n)
        {
            if (n <= 0)
            {
                return 1e+10;
            }
            if (q < 0 || q > 1)
            {
                return 1e+10;
            }

            if (q == 0.5) return 0.0;

            double p;
            if (q > 0.0 && q < 0.5)
            {
                p = q;
            }
            else
            {
                if (q == 1.0) p = 1 - 0.9999999;
                else p = 1 - q;
            }

            double eps = 1e-10;
            double x0 = 0.0;
            double delta = 1e-2;
            double x = 0.0;
            while (Math.Abs(tcdf(x, n) - p) > eps)
            {
                double fbar = (tcdf(x0 + delta, n) - tcdf(x0 - delta, n)) / (2 * delta);
                x = x0 - (tcdf(x0, n) - p) / fbar;
                x0 = x;
            }

            if (q > 0.5) x *= -1.0;

            return x;
        }



        public static double chi2pdf(double x, double n)
        {
            if (x <= 0)
                return 0.0;
            else
                return Math.Pow(x, n / 2.0 - 1.0) * Math.Exp(-x / 2.0) / (Math.Pow(2, n / 2.0) * gamma(n / 2.0));
        }

        public static double chi2cdf(double x, double n)
        {


            if (n <= 0)
            {
                return -1.0;
            }
            else if (n < 2)
            {
                if (x >= 50 * n) return 1.0;
            }
            else if (n < 10)
            {
                if (x >= 20 * n) return 1.0;
            }
            else if (n < 50)
            {
                if (x >= 10 * n) return 1.0;
            }
            else
            {
                if (x >= 3 * n) return 1.0;
            }

            if (x <= 0.0) return 0.0;

            double delta1 = 1e-20;
            double delta2 = 1.0;
            double[] xk = { -0.9602898566, -0.7966664774, -0.5255324099, -0.1834346425, 0.9602898566, 0.7966664774, 0.5255324099, 0.1834346425 };
            double[] ak = { 0.1012285363, 0.2223810345, 0.3137066459, 0.3626837834, 0.1012285363, 0.2223810345, 0.3137066459, 0.3626837834 };

            if (n < 2)
            {
                if (x <= 0.1)
                {
                    int i = 0, j = 0, k = 0, xn;
                    double p = 0.0;
                    double a, b, y, delta;
                    int r = -(int)(-Math.Log10(x));
                    if (r < -19) r = -19;

                    double lower = 0.0;
                    for (k = -19; k < r; k++)
                    {
                        y = Math.Pow((double)10, k) - lower;
                        xn = (int)(y / delta1) + 1;
                        delta = y / xn;


                        for (i = 0; i < xn; i++)
                        {
                            a = lower + i * delta;
                            b = a + delta;
                            for (j = 0; j < 8; j++)
                            {
                                p += chi2pdf((b - a) / 2.0 * xk[j] + (b + a) / 2.0, n) * ak[j] * delta / 2.0;
                            }
                        }
                        lower = Math.Pow((double)10, k);
                        delta1 = lower;
                    }

                    y = x - lower;
                    xn = (int)(y / delta1) + 1;
                    delta = y / xn;


                    for (i = 0; i < xn; i++)
                    {
                        a = lower + i * delta;
                        b = a + delta;
                        for (j = 0; j < 8; j++)
                        {
                            p += chi2pdf((b - a) / 2.0 * xk[j] + (b + a) / 2.0, n) * ak[j] * delta / 2.0;
                        }
                    }
                    return p;
                }
                else
                {
                    double p = chi2cdf(0.1, n);
                    double y = x - 0.1;
                    int xn = (int)(y / delta2) + 1;
                    double delta = y / xn;
                    double a, b;
                    int i = 0, j = 0;
                    for (i = 0; i < xn; i++)
                    {
                        a = 0.1 + i * delta;
                        b = a + delta;
                        for (j = 0; j < 8; j++)
                        {
                            p += chi2pdf((b - a) / 2.0 * xk[j] + (b + a) / 2.0, n) * ak[j] * delta / 2.0;
                        }
                    }
                    return p;
                }
            }
            else
            {
                double p = 0.0;
                int xn = (int)(x / delta2) + 1;
                double delta = x / xn;
                double a, b;
                int i = 0, j = 0;
                for (i = 0; i < xn; i++)
                {
                    a = i * delta;
                    b = a + delta;
                    for (j = 0; j < 8; j++)
                    {
                        p += chi2pdf((b - a) / 2.0 * xk[j] + (b + a) / 2.0, n) * ak[j] * delta / 2.0;
                    }
                }
                return p;
            }
        }

        public static double chi2inv(double q, double n)//2010.5.11更改,2013,12,13,再次更改
        {
            if (n <= 0)
            {

                //MessageBox.Show("错误的自由度");
                return 1e+10;
            }
            if (q < 0 || q > 1)
            {
                //MessageBox.Show("概率的范围为0-1");
                return 1e+10;
            }

            if (0 == q) return 0.0;




            if (n == 1 && q == 0.1)
                return 0.01579077;
            else if (n == 1 && q == 0.2)
                return 0.06418475;
            else if (n == 1 && q == 0.3)
                return 0.1484719;
            else if (n == 1 && q == 0.4)
                return 0.2749959;
            else if (n == 1 && q == 0.5)
                return 0.4549364;
            else if (n == 1 && q == 0.6)
                return 0.7083263;
            else if (n == 1 && q == 0.7)
                return 1.074194;
            else if (n == 1 && q == 0.8)
                return 1.642374;
            else if (n == 1 && q == 0.85)
                return 2.072251;
            else if (n == 1 && q == 0.9)
                return 2.705543;
            else if (n == 1 && q == 0.91)
                return 2.874373;
            else if (n == 1 && q == 0.92)
                return 3.064902;
            else if (n == 1 && q == 0.93)
                return 3.283020;
            else if (n == 1 && q == 0.94)
                return 3.537385;
            else if (n == 1 && q == 0.95)
                return 3.841459;
            else if (n == 1 && q == 0.96)
                return 4.217885;
            else if (n == 1 && q == 0.97)
                return 4.709292;
            else if (n == 1 && q == 0.98)
                return 5.411894;
            else if (n == 1 && q == 0.99)
                return 6.634897;
            else if (n == 1 && q == 0.999)
                return 10.82757;
            else
            {
                double eps = 1e-12;
                double x0 = n;
                double delta = 1e-2;
                double x = 0.0;
                while (Math.Abs(chi2cdf(x, n) - q) > eps)
                {
                    double fbar = (chi2cdf(x0 + delta, n) - chi2cdf(x0 - delta, n)) / (2 * delta);
                    x = x0 - (chi2cdf(x0, n) - q) / fbar;
                    x0 = x;

                }

                return x;
            }
        }




        public static double max(double[] a, int n, ref int I)
        {
            int i;
            double maximum = a[0];
            I = 0;
            for (i = 0; i < n; i++)
            {
                if (a[i] > maximum)
                {
                    maximum = a[i];
                    I = i;
                }
            }
            return maximum;
        }
        public static double min(double[] a, int n, ref int I)
        {
            int i;
            double minimum = a[0];
            I = 0;
            for (i = 0; i < n; i++)
            {
                if (a[i] < minimum)
                {
                    minimum = a[i];
                    I = i;
                }
            }
            return minimum;
        }
    }
}