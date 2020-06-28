using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    class updownMethod
    {
        public static void upanddown_tjs(int xArrayLength, double[] xArray, int[] vArray, out int tjs)
        {
            tjs = 1;



            {
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                //V[V.Length-1] = 0;
                for (int w = 0; w < xArrayLength; w++)
                {
                    V[w] = vArray[w];
                    X[w] = xArray[w];
                }
                //产生每步的i;
                int step = 0;
                int[] i_step = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    i_step[w] = step;
                    if (V[w] == 0)
                        step++;
                    else
                        step--;
                }
                int[] i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                // i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i[0] = Convert.ToInt16(pub_function.MAX_getMax_int(i_step));
                for (int w = 1; w < (i.Length); w++)
                {
                    i[w] = i[w - 1] - 1;
                }
                tjs = i.Length;
            }

        }
        //升降法试探计数
        //out int availability_number，为试探计数
        public static void get_availability_number(int xArrayLength, int[] vArray, out int availability_number)
        {
            int fail_num = 0;
            for (int w = 0; w < xArrayLength; w++)
            {
                try
                {
                    if (vArray[w + 1] == vArray[w])
                        fail_num = fail_num + 1;
                    else
                        break;
                }
                catch
                { }
            }
            availability_number = xArrayLength - fail_num;
        }

        //升降法标准正态分布 结合方法
        //out int Data_validity_determination 有效性判定函数，当其为1，数据有效，可以计算标准差。当其为0数据无效，不可以计算标准差。
        public static void get_norm_combination(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            upanddown_getz(xArrayLength, xArray, vArray, x0, d, out z);
            upanddown_norm_conbination(xArrayLength, xArray, vArray, x0, d, out Data_validity_determination,
                out μ0_final, out σ0_final, out G, out H, out n, out Sigma_mu, out Sigma_sigma, out SysParam.result_i,
                out SysParam.vi, out SysParam.mi, out A, out B, out M, out b);

            xArrayLength++;
        }

        //升降法ln正态分布 结合方法
        public static void get_norm_ln_combination(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {

            upanddown_norm_conbination(xArrayLength, xArray, vArray, Math.Log(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);

            upanddown_getz(xArrayLength, xArray, vArray, Math.Log(x0), d, out z);

            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Exp(μ0_final);
            xArrayLength++;
        }

        //升降法log10正态分布 结合方法
        public static void get_norm_log10_combination(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {

            upanddown_norm_conbination(xArrayLength, xArray, vArray, Math.Log10(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log10(x0), d, out z);

            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(10, μ0_final);

            xArrayLength++;
        }
        //升降法幂正态分布 结合方法
        public static void get_norm_pow_combination(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso, double pow,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {

            upanddown_norm_conbination(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d, out z);

            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(μ0_final, 1 / pow);
            xArrayLength++;
        }




        //升降法标准正态分布 传统方法
        //out int Data_validity_determination 有效性判定函数，当其为1，数据有效，可以计算标准差。当其为0数据无效，不可以计算标准差。
        public static void get_norm_tradition(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            upanddown_getz(xArrayLength, xArray, vArray, x0, d, out z);
            upanddown_norm_tradition(xArrayLength, xArray, vArray, x0, d, out Data_validity_determination,
                out μ0_final, out σ0_final, out G, out H, out n, out Sigma_mu, out Sigma_sigma, out SysParam.result_i,
                out SysParam.vi, out SysParam.mi, out A, out B, out M, out b);
            xArrayLength++;
        }

        //升降法ln正态分布 传统方法
        public static void get_norm_ln_tradition(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_ln = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //  pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
            upanddown_norm_tradition(xArrayLength, xArray, vArray, Math.Log(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            //pub_function.resolution_getReso(Math.Exp(d), reso, out d1);
            //pub_function.resolution_getReso(x0, reso, out x1);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log(x0), d, out z);
            // pub_function.resolution_getReso(Math.Exp(z), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Exp(μ0_final);
            xArrayLength++;
        }
        //升降法log10正态分布 传统方法
        public static void get_norm_log10_tradition(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_log10 = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            // pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
            upanddown_norm_tradition(xArrayLength, xArray, vArray, Math.Log10(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log10(x0), d, out z);
            //pub_function.resolution_getReso(Math.Pow(10, z), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(10, μ0_final);

            xArrayLength++;
        }
        //升降法幂正态分布 传统方法
        public static void get_norm_pow_tradition(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso, double pow,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_pow = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //pub_function.get_xArray_pow(xArrayLength, pow, xArray, out xArray_pow);
            upanddown_norm_tradition(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d, out z);
            //  pub_function.resolution_getReso(Math.Pow(z, 1 / pow), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(μ0_final, 1 / pow);
            xArrayLength++;
        }

        //升降法标准正态分布 修正方法
        public static void get_norm_correct(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            upanddown_getz(xArrayLength, xArray, vArray, x0, d, out z);
            upanddown_norm_correct(xArrayLength, xArray, vArray, x0, d, out Data_validity_determination, out μ0_final,
                out σ0_final, out G, out H, out n, out Sigma_mu, out Sigma_sigma
                , out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                               out A, out B, out M, out b);
            xArrayLength++;
        }
        //升降法ln正态分布 修正方法
        public static void get_norm_ln_correct(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_ln = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
            upanddown_norm_correct(xArrayLength, xArray, vArray, Math.Log(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log(x0), d, out z);
            //  pub_function.resolution_getReso(Math.Exp(z), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Exp(μ0_final);
            xArrayLength++;
        }
        //升降法log10正态分布 修正方法
        public static void get_norm_log10_correct(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_log10 = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
            upanddown_norm_correct(xArrayLength, xArray, vArray, Math.Log10(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log10(x0), d, out z);
            // pub_function.resolution_getReso(Math.Pow(10, z), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(10, μ0_final);
            xArrayLength++;
        }
        //升降法幂正态分布 修正方法
        public static void get_norm_pow_correct(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso, double pow,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_pow = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            // pub_function.get_xArray_pow(xArrayLength, pow, xArray, out xArray_pow);
            upanddown_norm_correct(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d, out z);
            //  pub_function.resolution_getReso(Math.Pow(z, 1 / pow), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(μ0_final, 1 / pow);
            xArrayLength++;
        }

        //升降法标准逻辑斯蒂分布
        public static void get_logis(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                   out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                   out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            upanddown_getz(xArrayLength, xArray, vArray, x0, d, out z);
            upanddown_logis(xArrayLength, xArray, vArray, x0, d, out Data_validity_determination, out μ0_final,
                out σ0_final, out G, out H, out n, out Sigma_mu, out Sigma_sigma
                , out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            xArrayLength++;
        }
        //升降法 ln 逻辑斯蒂分布
        public static void get_logis_ln(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_ln = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //  pub_function.get_xArray_ln(xArrayLength, xArray, out xArray_ln);
            upanddown_logis(xArrayLength, xArray, vArray, Math.Log(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log(x0), d, out z);
            // pub_function.resolution_getReso(Math.Exp(z), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Exp(μ0_final);
            xArrayLength++;
        }
        //升降法 log10 逻辑斯蒂分布
        public static void get_logis_log10(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso,
                                 out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                 out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_log10 = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //pub_function.get_xArray_log10(xArrayLength, xArray, out xArray_log10);
            upanddown_logis(xArrayLength, xArray, vArray, Math.Log10(x0), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Log10(x0), d, out z);
            // pub_function.resolution_getReso(Math.Pow(10, z), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(10, μ0_final);
            xArrayLength++;
        }
        //升降法 幂 逻辑斯蒂分布
        public static void get_logis_pow(ref int xArrayLength, double[] xArray, int[] vArray, double x0, double d, double reso, double pow,
                                  out double z, out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                  out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                  out double A, out double B, out double M, out double b)
        {
            //double[] xArray_pow = new double[xArrayLength];
            //double[] X = new double[xArrayLength];
            //int[] V = new int[xArrayLength];
            //double d1, x1;
            //for (int w = 0; w < xArrayLength; w++)
            //{
            //    V[w] = vArray[w];
            //    X[w] = xArray[w];
            //}
            //pub_function.get_xArray_pow(xArrayLength, pow, xArray, out xArray_pow);
            upanddown_logis(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d,
                                     out Data_validity_determination, out μ0_final, out σ0_final,
                                     out G, out H, out n, out Sigma_mu, out Sigma_sigma,
                                     out SysParam.result_i, out SysParam.vi, out SysParam.mi,
                                     out A, out B, out M, out b);
            upanddown_getz(xArrayLength, xArray, vArray, Math.Pow(x0, pow), d, out z);
            // pub_function.resolution_getReso(Math.Pow(z, 1 / pow), reso, out z);
            if (Math.Abs(μ0_final) < Math.Pow(10, -10))
                μ0_final = 0;
            else
                μ0_final = Math.Pow(μ0_final, 1 / pow);
            xArrayLength++;
        }


        //升降法 多组试验数据处理
        //int Group_number:总组数， n:各组中的n, double[] mu:各组中的均值                                          
        public static void get_multiGroup_result(int Group_number, int[] n, double[] mu, double[] sigma, double[] G, double[] H,
                                                 out double μ0_final, out double σ0_final, out double Sigma_mu, out double Sigma_sigma, out int N)
        {
            double mu_final_zi = 0, mu_final_mu = 0, sigma_final_zi = 0, sigma_final_mu = 0;
            N = 0;
            int sumN = 0;

            double[] mu1 = new double[Group_number];


            for (int w = 0; w < Group_number; w++)
            {
                mu1[w] = mu[w];
                if (SysParam.store_upDown_Distribute == "正态分布Ln" || SysParam.store_upDown_Distribute == "逻辑斯谛分布Ln")
                {
                    mu1[w] = Math.Log(mu[w]);

                }
                else if (SysParam.store_upDown_Distribute == "正态分布log10" || SysParam.store_upDown_Distribute == "逻辑斯谛分布log10")
                {
                    mu1[w] = Math.Log10(mu[w]);
                }
                else if (SysParam.store_upDown_Distribute == "正态分布幂" || SysParam.store_upDown_Distribute == "逻辑斯谛分布幂")
                {
                    mu1[w] = Math.Pow(mu[w], SysParam.updown_Power);
                }

                mu_final_zi = mu_final_zi + (n[w] * mu1[w]) / Math.Pow(G[w], 2);
                mu_final_mu = mu_final_mu + n[w] / Math.Pow(G[w], 2);
                sigma_final_zi = sigma_final_zi + (n[w] * sigma[w]) / Math.Pow(H[w], 2);
                sigma_final_mu = sigma_final_mu + n[w] / Math.Pow(H[w], 2);
                sumN = sumN + n[w];
            }
            μ0_final = mu_final_zi / mu_final_mu;
            σ0_final = sigma_final_zi / sigma_final_mu;
            Sigma_mu = σ0_final / Math.Sqrt(mu_final_mu);
            Sigma_sigma = σ0_final / Math.Sqrt(sigma_final_mu);
            N = sumN;

            if (SysParam.store_upDown_Distribute == "正态分布Ln" || SysParam.store_upDown_Distribute == "逻辑斯谛分布Ln")
            {
                μ0_final = Math.Exp(μ0_final);

            }
            else if (SysParam.store_upDown_Distribute == "正态分布log10" || SysParam.store_upDown_Distribute == "逻辑斯谛分布log10")
            {
                μ0_final = Math.Pow(10, μ0_final);
            }
            else if (SysParam.store_upDown_Distribute == "正态分布幂" || SysParam.store_upDown_Distribute == "逻辑斯谛分布幂")
            {
                μ0_final = Math.Pow(μ0_final, 1 / SysParam.updown_Power);
            }
        }



        //升降法得到下一刺激量；
        public static void upanddown_getz(int xArrayLength, double[] xArray, int[] vArray, double x0, double d, out double z)
        {
            if (xArrayLength == 0)
                z = x0;
            else
            {
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    X[w] = xArray[w];
                    V[w] = vArray[w];
                }
                if (V[xArrayLength - 1] == 1)
                    z = (X[xArrayLength - 1] - d);
                else
                    z = (X[xArrayLength - 1] + d);
            }
        }

        //正态分布传统方法处理数据
        public static void upanddown_norm_tradition(int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                                    out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                                    out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                                    out int[] i, out int[] vi, out int[] mi, out double A, out double B, out double M, out double b)
        {
            double ρ = 0;
            if (xArrayLength > 2)
            {
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    V[w] = vArray[w];
                    X[w] = xArray[w];
                }
                //产生每步的i;
                int step = 0;
                int[] i_step = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    i_step[w] = step;
                    if (V[w] == 0)
                        step++;
                    else
                        step--;
                }
                //定义i的长度
                // int[] i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i[0] = Convert.ToInt16(pub_function.MAX_getMax_int(i_step));
                for (int w = 1; w < (i.Length); w++)
                {
                    i[w] = i[w - 1] - 1;
                }
                // 求数组i的平方
                int[] i_square = new int[i.Length];
                for (int w = 0; w < (i.Length); w++)
                {
                    i_square[w] = i[w] * i[w];
                }
                //得到vi,mi
                //将V放入，实验数据表
                int[,] v_table_1 = new int[i.Length, xArrayLength];

                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    v_table_1[step, w] = V[w];
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                int[,] v_table_2 = new int[i.Length, xArrayLength];
                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    if (V[w] == 0)
                        v_table_2[step, w] = 1;
                    else
                        v_table_2[step, w] = 0;
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                //int[] vi = new int[i.Length];
                // int[] mi = new int[i.Length];
                vi = new int[i.Length];
                mi = new int[i.Length];
                for (int k = 0; k < i.Length; k++)
                {
                    vi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        vi[k] = v_table_1[k, w] + vi[k];
                    }
                }
                for (int k = 0; k < i.Length; k++)
                {
                    mi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        mi[k] = v_table_2[k, w] + mi[k];
                    }
                }

                //得到数组vi,mi的和;
                int sumvi = 0, summi = 0;
                for (int w = 0; w < (vi.Length); w++)
                {
                    sumvi = vi[w] + sumvi;
                    summi = mi[w] + summi;
                }

                int[] vi_1 = new int[i.Length];//vi_1为vi一瞥
                int mu_mid, iii;//计算均值中用到的+-号确定；
                if (sumvi <= summi)
                {
                    iii = 0;
                    foreach (int l1 in vi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = -1;
                }
                else
                {
                    iii = 0;
                    foreach (int l1 in mi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = 1;
                }


                //得到试探数n;
                n = 0;
                for (int w = 0; w < (vi_1.Length); w++)
                {
                    n = vi_1[w] + n;
                }

                //得到A，B，M，b
                A = 0; B = 0;
                M = 0;
                for (int w = 0; w < (i.Length); w++)
                {
                    A = i[w] * vi_1[w] + A;
                }
                for (int w = 0; w < (i.Length); w++)
                {
                    B = i_square[w] * vi_1[w] + B;
                }

                if (Math.Pow(n, 2) < double.Epsilon)
                {
                    M = 0;
                }
                else
                {
                    M = (n * B - Math.Pow(A, 2)) / Math.Pow(n, 2);
                }
                double b_1;
                if (n != 0)
                {
                    b_1 = Math.Abs((A / n) - 0.5);
                }
                else
                {
                    b_1 = 0;
                }
                b_1 = Math.Round(b_1 - Math.Floor(b_1), 1);
                if (b_1 <= 0.5)
                    b = b_1;
                else
                    b = 1 - b_1;
                μ0_final = x0 + ((A / n) + mu_mid * 0.5) * d;

                if (M > 0.3)
                {
                    ρ = 1.620 * (M + 0.029);
                }
                else
                {
                    ρ = getA1ρmb(M, b);
                }
                σ0_final = ρ * d;

                if ((M > 0.25) && (i.Length > 3) && (i.Length < 8))
                    Data_validity_determination = 1;
                else
                    Data_validity_determination = 0;
                G = getA3_G(ρ, b);
                H = getA4_H(ρ, b);
                Sigma_mu = (G / Math.Sqrt(n)) * σ0_final;
                Sigma_sigma = (H / Math.Sqrt(n)) * σ0_final;
                if (n == 0)
                {
                    μ0_final = 0;
                    σ0_final = 0;
                    Data_validity_determination = 0;
                    G = 0;
                    H = 0;
                    n = 0;
                    Sigma_mu = 0;
                    Sigma_sigma = 0;
                    vi = new int[1];
                    vi[0] = 0;

                    mi = new int[1];
                    mi[0] = 0;

                    i = new int[1];
                    i[0] = 0;
                }
            }
            else
            {
                μ0_final = 0;
                σ0_final = 0;
                Data_validity_determination = 0;
                G = 0;
                H = 0;
                n = 0;
                A = 0;
                B = 0;
                M = 0;
                b = 0;
                Sigma_mu = 0;
                Sigma_sigma = 0;
                vi = new int[1];
                vi[0] = 0;

                mi = new int[1];
                mi[0] = 0;

                i = new int[1];
                i[0] = 0;
            }
            SysParam.p = ρ;
        }

        //正态分布修正方法处理数据
        public static void upanddown_norm_correct(int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                                    out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                                    out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma
                                                    , out int[] i, out int[] vi, out int[] mi,
                               out double A, out double B, out double M, out double b)
        {
            A = 0; B = 0;
            M = 0;
            b = 0;
            double ρ = 0;
            if (xArrayLength > 2)
            {
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    V[w] = vArray[w];
                    X[w] = xArray[w];
                }
                //产生每步的i;
                int step = 0;
                int[] i_step = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    i_step[w] = step;
                    if (V[w] == 0)
                        step++;
                    else
                        step--;
                }
                //定义i的长度
                // int[] i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];

                i[0] = Convert.ToInt16(pub_function.MAX_getMax_int(i_step));
                for (int w = 1; w < (i.Length); w++)
                {
                    i[w] = i[w - 1] - 1;
                }
                // 求数组i的平方
                int[] i_square = new int[i.Length];
                for (int w = 0; w < (i.Length); w++)
                {
                    i_square[w] = i[w] * i[w];
                }
                //得到vi,mi
                //将V放入，实验数据表
                int[,] v_table_1 = new int[i.Length, xArrayLength];

                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    v_table_1[step, w] = V[w];
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                int[,] v_table_2 = new int[i.Length, xArrayLength];
                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    if (V[w] == 0)
                        v_table_2[step, w] = 1;
                    else
                        v_table_2[step, w] = 0;
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                // int[] vi = new int[i.Length];
                // int[] mi = new int[i.Length];
                vi = new int[i.Length];
                mi = new int[i.Length];
                for (int k = 0; k < i.Length; k++)
                {
                    vi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        vi[k] = v_table_1[k, w] + vi[k];
                    }
                }
                for (int k = 0; k < i.Length; k++)
                {
                    mi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        mi[k] = v_table_2[k, w] + mi[k];
                    }
                }
                //得到数组vi,mi的和;
                int sumvi = 0, summi = 0;
                for (int w = 0; w < (vi.Length); w++)
                {
                    sumvi = vi[w] + sumvi;
                    summi = mi[w] + summi;
                }

                int[] vi_1 = new int[i.Length];//vi_1为vi一瞥
                int mu_mid, iii;//计算均值中用到的+-号确定；
                if (sumvi <= summi)
                {
                    iii = 0;
                    foreach (int l1 in vi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = -1;
                }
                else
                {
                    iii = 0;
                    foreach (int l1 in mi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = 1;
                }
                //得到试探数n;
                n = 0;
                for (int w = 0; w < (vi_1.Length); w++)
                {
                    n = vi_1[w] + n;
                }
                //得到A，B，M，b

                A = 0; B = 0;
                M = 0;
                for (int w = 0; w < (i.Length); w++)
                {
                    A = i[w] * vi_1[w] + A;
                }
                for (int w = 0; w < (i.Length); w++)
                {
                    B = i_square[w] * vi_1[w] + B;
                }

                if (Math.Pow(n, 2) < double.Epsilon)
                {
                    M = 0;
                }
                else
                {
                    M = (n * B - Math.Pow(A, 2)) / Math.Pow(n, 2);
                }
                double b_1;
                if (n != 0)
                {
                    b_1 = Math.Abs((A / n) - 0.5);
                }
                else
                {
                    b_1 = 0;
                }
                b_1 = Math.Round(b_1 - Math.Floor(b_1), 1);
                if (b_1 <= 0.5)
                    b = b_1;
                else
                    b = 1 - b_1;
                μ0_final = x0 + ((A / n) + mu_mid * 0.5) * d;

                if ((n > 11) && (n < 30))
                {
                    switch (n)
                    {

                        case 12:
                            ρ = getA2ρ_12(M, b);
                            break;
                        case 13:
                            ρ = getA2ρ_13(M, b);
                            break;
                        case 14:
                            ρ = getA2ρ_14(M, b);
                            break;
                        case 15:
                            ρ = getA2ρ_15(M, b);
                            break;
                        case 16:
                            ρ = getA2ρ_16(M, b);
                            break;
                        case 17:
                            ρ = getA2ρ_17(M, b);
                            break;
                        case 18:
                            ρ = getA2ρ_18(M, b);
                            break;
                        case 19:
                            ρ = getA2ρ_19(M, b);
                            break;
                        case 20:
                            ρ = getA2ρ_20(M, b);
                            break;
                        case 21:
                            ρ = getA2ρ_21(M, b);
                            break;
                        case 22:
                            ρ = getA2ρ_22(M, b);
                            break;
                        case 23:
                            ρ = getA2ρ_23(M, b);
                            break;
                        case 24:
                            ρ = getA2ρ_24(M, b);
                            break;
                        case 25:
                            ρ = getA2ρ_25(M, b);
                            break;
                        case 26:
                            ρ = getA2ρ_26(M, b);
                            break;
                        case 27:
                            ρ = getA2ρ_27(M, b);
                            break;
                        case 28:
                            ρ = getA2ρ_28(M, b);
                            break;
                        case 29:
                            ρ = getA2ρ_29(M, b);
                            break;
                        default:
                            break;
                    }

                    if (ρ == -1)
                        σ0_final = 0;
                    else
                        σ0_final = ρ * d;
                    if ((M > 0.25) && (i.Length > 3) && (i.Length < 8))
                        Data_validity_determination = 1;
                    else
                        Data_validity_determination = 0;
                    G = getA3_G(ρ, b);
                    H = getA4_H(ρ, b);
                    Sigma_mu = (G / Math.Sqrt(n)) * σ0_final;
                    Sigma_sigma = (H / Math.Sqrt(n)) * σ0_final;

                }
                else
                {
                    σ0_final = 0;
                    Data_validity_determination = 0;
                    G = 0;
                    H = 0;
                    //n = 0;
                    //A = 0;
                    //B = 0;
                    //M = 0;
                    //b = 0;
                    Sigma_mu = 0;
                    Sigma_sigma = 0;
                    //vi = new int[1];
                    //vi[0] = 0;

                    //mi = new int[1];
                    //mi[0] = 0;

                    //i = new int[1];
                    //i[0] = 0;
                }
            }
            else
            {
                μ0_final = 0;
                σ0_final = 0;
                Data_validity_determination = 0;
                G = 0;
                H = 0;
                n = 0;
                Sigma_mu = 0;
                Sigma_sigma = 0;
                vi = new int[1];
                vi[0] = 0;

                mi = new int[1];
                mi[0] = 0;

                i = new int[1];
                i[0] = 0;
            }
            SysParam.p = ρ;
        }


        //正态分布结合方法处理数据
        public static void upanddown_norm_conbination(int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                                    out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                                    out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma,
                                                    out int[] i, out int[] vi, out int[] mi, out double A, out double B, out double M, out double b)
        {
            double ρ = 0;
            if (xArrayLength > 2)
            {
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    V[w] = vArray[w];
                    X[w] = xArray[w];
                }
                //产生每步的i;
                int step = 0;
                int[] i_step = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    i_step[w] = step;
                    if (V[w] == 0)
                        step++;
                    else
                        step--;
                }
                //定义i的长度
                // int[] i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i[0] = Convert.ToInt16(pub_function.MAX_getMax_int(i_step));
                for (int w = 1; w < (i.Length); w++)
                {
                    i[w] = i[w - 1] - 1;
                }
                // 求数组i的平方
                int[] i_square = new int[i.Length];
                for (int w = 0; w < (i.Length); w++)
                {
                    i_square[w] = i[w] * i[w];
                }
                //得到vi,mi
                //将V放入，实验数据表
                int[,] v_table_1 = new int[i.Length, xArrayLength];

                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    v_table_1[step, w] = V[w];
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                int[,] v_table_2 = new int[i.Length, xArrayLength];
                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    if (V[w] == 0)
                        v_table_2[step, w] = 1;
                    else
                        v_table_2[step, w] = 0;
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                //int[] vi = new int[i.Length];
                // int[] mi = new int[i.Length];
                vi = new int[i.Length];
                mi = new int[i.Length];
                for (int k = 0; k < i.Length; k++)
                {
                    vi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        vi[k] = v_table_1[k, w] + vi[k];
                    }
                }
                for (int k = 0; k < i.Length; k++)
                {
                    mi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        mi[k] = v_table_2[k, w] + mi[k];
                    }
                }

                //得到数组vi,mi的和;
                int sumvi = 0, summi = 0;
                for (int w = 0; w < (vi.Length); w++)
                {
                    sumvi = vi[w] + sumvi;
                    summi = mi[w] + summi;
                }

                int[] vi_1 = new int[i.Length];//vi_1为vi一瞥
                int mu_mid, iii;//计算均值中用到的+-号确定；
                if (sumvi <= summi)
                {
                    iii = 0;
                    foreach (int l1 in vi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = -1;
                }
                else
                {
                    iii = 0;
                    foreach (int l1 in mi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = 1;
                }


                //得到试探数n;
                n = 0;
                for (int w = 0; w < (vi_1.Length); w++)
                {
                    n = vi_1[w] + n;
                }

                //得到A，B，M，b
                A = 0; B = 0;
                M = 0;
                for (int w = 0; w < (i.Length); w++)
                {
                    A = i[w] * vi_1[w] + A;
                }
                for (int w = 0; w < (i.Length); w++)
                {
                    B = i_square[w] * vi_1[w] + B;
                }

                if (Math.Pow(n, 2) < double.Epsilon)
                {
                    M = 0;
                }
                else
                {
                    M = (n * B - Math.Pow(A, 2)) / Math.Pow(n, 2);
                }
                double b_1;
                if (n != 0)
                {
                    b_1 = Math.Abs((A / n) - 0.5);
                }
                else
                {
                    b_1 = 0;
                }
                b_1 = Math.Round(b_1 - Math.Floor(b_1), 1);
                if (b_1 <= 0.5)
                    b = b_1;
                else
                    b = 1 - b_1;
                μ0_final = x0 + ((A / n) + mu_mid * 0.5) * d;

                if (M >= 0.56)
                {
                    ρ = 1.620 * (M + 0.029);
                }
                else
                {
                    ρ = -1;
                    if ((n > 11) && (n < 30))
                    {
                        switch (n)
                        {

                            case 12:
                                ρ = getA2ρ_12(M, b);
                                break;
                            case 13:
                                ρ = getA2ρ_13(M, b);
                                break;
                            case 14:
                                ρ = getA2ρ_14(M, b);
                                break;
                            case 15:
                                ρ = getA2ρ_15(M, b);
                                break;
                            case 16:
                                ρ = getA2ρ_16(M, b);
                                break;
                            case 17:
                                ρ = getA2ρ_17(M, b);
                                break;
                            case 18:
                                ρ = getA2ρ_18(M, b);
                                break;
                            case 19:
                                ρ = getA2ρ_19(M, b);
                                break;
                            case 20:
                                ρ = getA2ρ_20(M, b);
                                break;
                            case 21:
                                ρ = getA2ρ_21(M, b);
                                break;
                            case 22:
                                ρ = getA2ρ_22(M, b);
                                break;
                            case 23:
                                ρ = getA2ρ_23(M, b);
                                break;
                            case 24:
                                ρ = getA2ρ_24(M, b);
                                break;
                            case 25:
                                ρ = getA2ρ_25(M, b);
                                break;
                            case 26:
                                ρ = getA2ρ_26(M, b);
                                break;
                            case 27:
                                ρ = getA2ρ_27(M, b);
                                break;
                            case 28:
                                ρ = getA2ρ_28(M, b);
                                break;
                            case 29:
                                ρ = getA2ρ_29(M, b);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (M > 0.3)
                        {
                            ρ = 1.620 * (M + 0.029);
                        }
                        else
                        {
                            ρ = getA1ρmb(M, b);
                        }
                    }
                }
                if (ρ == -1)
                    σ0_final = 0;
                else
                    σ0_final = ρ * d;


                Data_validity_determination = 1;

                G = getA3_G(ρ, b);
                H = getA4_H(ρ, b);
                Sigma_mu = (G / Math.Sqrt(n)) * σ0_final;
                Sigma_sigma = (H / Math.Sqrt(n)) * σ0_final;
                if (n == 0)
                {
                    μ0_final = 0;
                    σ0_final = 0;
                    Data_validity_determination = 0;
                    G = 0;
                    H = 0;
                    n = 0;
                    Sigma_mu = 0;
                    Sigma_sigma = 0;
                    vi = new int[1];
                    vi[0] = 0;

                    mi = new int[1];
                    mi[0] = 0;

                    i = new int[1];
                    i[0] = 0;
                }
            }
            else
            {
                μ0_final = 0;
                σ0_final = 0;
                Data_validity_determination = 0;
                G = 0;
                H = 0;
                n = 0;
                A = 0;
                B = 0;
                M = 0;
                b = 0;
                Sigma_mu = 0;
                Sigma_sigma = 0;
                vi = new int[1];
                vi[0] = 0;

                mi = new int[1];
                mi[0] = 0;

                i = new int[1];
                i[0] = 0;
            }
            SysParam.p = ρ;
        }



        //逻辑斯蒂分布处理数据
        public static void upanddown_logis(int xArrayLength, double[] xArray, int[] vArray, double x0, double d,
                                                    out int Data_validity_determination, out double μ0_final, out double σ0_final,
                                                    out double G, out double H, out int n, out double Sigma_mu, out double Sigma_sigma
                                                    , out int[] i, out int[] vi, out int[] mi,
                               out double A, out double B, out double M, out double b)
        {

            A = 0; B = 0;
            M = 0;
            b = 0;
            double ρ = 0;
            if (xArrayLength > 2)
            {
                double[] X = new double[xArrayLength];
                int[] V = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    V[w] = vArray[w];
                    X[w] = xArray[w];
                }
                //产生每步的i;
                int step = 0;
                int[] i_step = new int[xArrayLength];
                for (int w = 0; w < xArrayLength; w++)
                {
                    i_step[w] = step;
                    if (V[w] == 0)
                        step++;
                    else
                        step--;
                }
                //定义i的长度
                //  int[] i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];
                i = new int[Convert.ToInt16(pub_function.MAX_getMax_int(i_step)) - Convert.ToInt16(pub_function.MIN_getMin_int(i_step)) + 1];

                i[0] = Convert.ToInt16(pub_function.MAX_getMax_int(i_step));
                for (int w = 1; w < (i.Length); w++)
                {
                    i[w] = i[w - 1] - 1;
                }
                // 求数组i的平方
                int[] i_square = new int[i.Length];
                for (int w = 0; w < (i.Length); w++)
                {
                    i_square[w] = i[w] * i[w];
                }
                //得到vi,mi
                //将V放入，实验数据表
                int[,] v_table_1 = new int[i.Length, xArrayLength];

                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    v_table_1[step, w] = V[w];
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                int[,] v_table_2 = new int[i.Length, xArrayLength];
                step = Convert.ToInt16(pub_function.MAX_getMax_int(i));
                for (int w = 0; w < xArrayLength; w++)
                {
                    if (V[w] == 0)
                        v_table_2[step, w] = 1;
                    else
                        v_table_2[step, w] = 0;
                    if (V[w] == 0)
                        step--;
                    else
                        step++;
                }
                //int[] vi = new int[i.Length];
                // int[] mi = new int[i.Length];
                vi = new int[i.Length];
                mi = new int[i.Length];
                for (int k = 0; k < i.Length; k++)
                {
                    vi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        vi[k] = v_table_1[k, w] + vi[k];
                    }
                }
                for (int k = 0; k < i.Length; k++)
                {
                    mi[k] = 0;
                    for (int w = 0; w < xArrayLength; w++)
                    {
                        mi[k] = v_table_2[k, w] + mi[k];
                    }
                }

                //得到数组vi,mi的和;
                int sumvi = 0, summi = 0;
                for (int w = 0; w < (vi.Length); w++)
                {
                    sumvi = vi[w] + sumvi;
                    summi = mi[w] + summi;
                }

                int[] vi_1 = new int[i.Length];//vi_1为vi一瞥
                int mu_mid, iii;//计算均值中用到的+-号确定；
                if (sumvi <= summi)
                {
                    iii = 0;
                    foreach (int l1 in vi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = -1;
                }
                else
                {
                    iii = 0;
                    foreach (int l1 in mi)
                    {
                        vi_1[iii] = l1;
                        iii++;
                    }
                    mu_mid = 1;
                }
                //得到试探数n;
                n = 0;
                for (int w = 0; w < (vi_1.Length); w++)
                {
                    n = vi_1[w] + n;
                }
                //得到A，B，M，b
                A = 0; B = 0;
                M = 0;
                for (int w = 0; w < (i.Length); w++)
                {
                    A = i[w] * vi_1[w] + A;
                }
                for (int w = 0; w < (i.Length); w++)
                {
                    B = i_square[w] * vi_1[w] + B;
                }

                if (Math.Pow(n, 2) < double.Epsilon)
                {
                    M = 0;
                }
                else
                {
                    M = (n * B - Math.Pow(A, 2)) / Math.Pow(n, 2);
                }
                double b_1;
                if (n != 0)
                {
                    b_1 = Math.Abs((A / n) - 0.5);
                }
                else
                {
                    b_1 = 0;
                }
                b_1 = Math.Round(b_1 - Math.Floor(b_1), 1);
                if (b_1 <= 0.5)
                    b = b_1;
                else
                    b = 1 - b_1;
                μ0_final = x0 + ((A / n) + mu_mid * 0.5) * d;



                if ((n > 11) && (n < 30))
                {
                    switch (n)
                    {

                        case 12:
                            ρ = getA5_H_ρ_12(M, b);
                            break;
                        case 13:
                            ρ = getA5_H_ρ_13(M, b);
                            break;
                        case 14:
                            ρ = getA5_H_ρ_14(M, b);
                            break;
                        case 15:
                            ρ = getA5_H_ρ_15(M, b);
                            break;
                        case 16:
                            ρ = getA5_H_ρ_16(M, b);
                            break;
                        case 17:
                            ρ = getA5_H_ρ_17(M, b);
                            break;
                        case 18:
                            ρ = getA5_H_ρ_18(M, b);
                            break;
                        case 19:
                            ρ = getA5_H_ρ_19(M, b);
                            break;
                        case 20:
                            ρ = getA5_H_ρ_20(M, b);
                            break;
                        case 21:
                            ρ = getA5_H_ρ_21(M, b);
                            break;
                        case 22:
                            ρ = getA5_H_ρ_22(M, b);
                            break;
                        case 23:
                            ρ = getA5_H_ρ_23(M, b);
                            break;
                        case 24:
                            ρ = getA5_H_ρ_24(M, b);
                            break;
                        case 25:
                            ρ = getA5_H_ρ_25(M, b);
                            break;
                        case 26:
                            ρ = getA5_H_ρ_26(M, b);
                            break;
                        case 27:
                            ρ = getA5_H_ρ_27(M, b);
                            break;
                        case 28:
                            ρ = getA5_H_ρ_28(M, b);
                            break;
                        case 29:
                            ρ = getA5_H_ρ_29(M, b);
                            break;
                        default:
                            break;
                    }
                    if (ρ == -1)
                        σ0_final = 0;
                    else
                        σ0_final = ρ * d;
                    if ((M >= 0.3) && (i.Length > 3) && (i.Length < 8))
                        Data_validity_determination = 1;
                    else
                        Data_validity_determination = 0;
                    G = getA6_G(ρ, b);
                    H = getA7_H(ρ, b);
                    Sigma_mu = (G / Math.Sqrt(n)) * σ0_final;
                    Sigma_sigma = (H / Math.Sqrt(n)) * σ0_final;
                }
                else
                {
                    σ0_final = 0;
                    Data_validity_determination = 0;
                    G = 0;
                    H = 0;
                    //n = 0;
                    //A = 0;
                    //B = 0;
                    //M = 0;
                    //b = 0;
                    Sigma_mu = 0;
                    Sigma_sigma = 0;
                    //vi = new int[1];
                    //vi[0] = 0;

                    //mi = new int[1];
                    //mi[0] = 0;

                    //i = new int[1];
                    //i[0] = 0;
                }
            }
            else
            {
                μ0_final = 0;
                σ0_final = 0;
                Data_validity_determination = 0;
                G = 0;
                H = 0;
                n = 0;
                Sigma_mu = 0;
                Sigma_sigma = 0;
                vi = new int[1];
                vi[0] = 0;

                mi = new int[1];
                mi[0] = 0;

                i = new int[1];
                i[0] = 0;
            }
            SysParam.p = ρ;
        }

        public static void getIndexOfArray(double x, double[] array, out int x1, out int x2)
        {
            x1 = -1;
            x2 = -1;


            for (int i = 0; i < array.Length; i++)
            {
                if (Math.Abs(array[i] - Math.Round(x, 2)) <= 0.001)
                {
                    x1 = i;
                    x2 = i;
                    break;
                }
                if (array[i] > x)
                {
                    x1 = i;
                    x2 = i;
                    break;
                }

            }
        }

        public static int getIndexOfArray(double x, double[] array, double frac)
        {
            int k = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (Math.Abs(array[i] - x) <= frac)
                    return i;
            }
            if (x < array[0])
                k = 0;
            if (x > array[array.Length - 1])
                k = array.Length - 1;
            return k;

        }
        public static double getA1ρmb(double m, double b)
        {
            if ((b == 0) && (m < 0.24)) return 0;
            if (b > 0.5) return 0;
            int x = 0;
            int y = 0;
            x = getIndexOfArray(Math.Round(m, 2), A1_m, 0.001);
            y = getIndexOfArray(Math.Round(b, 2), A1_b, 0.001);

            return A1_ρ[x * A1_b.Length + y];

        }
        public static double[] A1_m ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30
        };

        public static double[] A1_b ={
             0.00,0.1,0.2,0.3,0.4,0.5
        };
        public static double[] A1_ρ ={
           -1,0.111 ,0.222 ,0.309 ,0.347 ,0.358,
             -1,0.119 ,0.237 ,0.322 ,0.358 ,0.368 ,
             -1,0.128 ,0.254 ,0.336 ,0.369 ,0.378 ,
             -1,0.139 ,0.273 ,0.350 ,0.380 ,0.388 ,
             -1,0.152 ,0.294 ,0.363 ,0.391 ,0.399 ,
             -1,0.168 ,0.316 ,0.377 ,0.402 ,0.410 ,
             -1,0.191 ,0.339 ,0.392 ,0.414 ,0.421 ,
             -1,0.223 ,0.361 ,0.406 ,0.426 ,0.432 ,
             -1,0.272 ,0.383 ,0.421 ,0.438 ,0.443 ,
             -1,0.333 ,0.405 ,0.436 ,0.451 ,0.455 ,
            0.173 ,0.379 ,0.425 ,0.451 ,0.464 ,0.468 ,
            0.388 ,0.415 ,0.446 ,0.466 ,0.477 ,0.480,
            0.430 ,0.444 ,0.465 ,0.481 ,0.490 ,0.493 ,
            0.461 ,0.469 ,0.484 ,0.496 ,0.504 ,0.506 ,
            0.487 ,0.492 ,0.503 ,0.512 ,0.518 ,0.520 ,
            0.510 ,0.514 ,0.521 ,0.528 ,0.533 ,0.534
        };

        public static double[] A2_m_12 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30

    };

        public static double[] A2_b_12 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_12 ={
              -1 ,0.113 ,0.236 ,0.331 ,0.368 ,0.378 ,
              -1,0.121 ,0.255 ,0.346 ,0.379 ,0.389 ,
              -1,0.131 ,0.277 ,0.361 ,0.391 ,0.400 ,
              -1,0.143 ,0.301 ,0.376 ,0.403 ,0.411 ,
              -1,0.158 ,0.328 ,0.392 ,0.416 ,0.423 ,
              -1,0.178 ,0.354 ,0.407 ,0.429 ,0.435 ,
              -1,0.209 ,0.380 ,0.424 ,0.442 ,0.448,
              -1,0.274 ,0.406 ,0.440 ,0.456 ,0.461 ,
              -1,0.367 ,0.430 ,0.457 ,0.470 ,0.474 ,
              -1,0.418 ,0.453 ,0.474 ,0.485 ,0.489 ,
            0.439 ,0.454 ,0.476 ,0.492 ,0.500 ,0.503 ,
            0.476 ,0.484 ,0.498 ,0.509 ,0.516 ,0.518 ,
            0.506 ,0.510 ,0.519 ,0.527 ,0.533 ,0.534,
            0.532 ,0.535 ,0.541 ,0.546 ,0.549 ,0.551,
            0.557 ,0.558 ,0.561 ,0.565 ,0.567 ,0.568 ,
            0.580 ,0.580 ,0.582 ,0.583 ,0.585 ,0.585,
            0.602 ,0.602 ,0.602 ,0.603 ,0.603 ,0.603,
            0.623 ,0.623 ,0.622 ,0.622 ,0.622 ,0.622 ,
            0.644 ,0.643 ,0.642 ,0.642 ,0.641 ,0.641 ,
            0.664 ,0.664 ,0.663 ,0.661 ,0.660 ,0.660
    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_12_next ={
            0.684 ,0.704 ,0.725 ,0.745 ,0.765,
            0.785 ,0.805 ,0.825 ,0.845 ,0.866 ,0.886 ,0.907 ,0.928 ,0.949 ,0.970 ,
            0.991 ,1.012 ,1.034 ,1.055 ,1.077 ,1.099 ,1.120 ,1.142 ,1.165 ,1.187 ,
            1.209 ,1.232 ,1.255 ,1.277 ,1.300 ,1.324 ,1.347 ,1.370 ,1.394 ,1.418 ,
            1.442 ,1.466 ,1.490 ,1.514 ,1.539 ,1.564 ,1.589 ,1.614 ,1.640 ,1.665 ,
            1.691 ,1.717 ,1.743 ,1.770 ,1.797 ,1.824 ,1.851 ,1.879 ,1.906 ,1.934 ,
            1.936 ,1.991 ,2.020 ,2.050 ,2.079 ,2.109 ,2.139 ,2.170 ,2.201 ,2.232 ,
            2.264 ,2.296 ,2.329 ,2.362 ,2.396 ,2.430 ,2.465 ,2.500 ,2.536 ,2.572 ,
            2.609 ,2.647 ,2.685 ,2.724 ,2.764 ,2.805 ,2.847 ,2.889 ,2.933 ,2.978 ,
            3.024 ,3.071 ,3.120 ,3.171 ,3.223 ,3.277 ,3.333 ,3.392 ,3.453 ,3.518,
            3.587 ,3.661 ,3.740 ,3.827 ,3.923 ,4.035 ,4.170 ,4.361
    };

        public static double getA2ρ_12(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_12, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_12, 0.001);
                return A2_ρ_12[x * A2_b_12.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_12_next表中的值
                if (m > 1.37) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_12_next[x];
            }
        }


        public static double[] A2_m_13 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_13 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_13 ={
              -1,0.113 ,0.235 ,0.329 ,0.366 ,0.376 ,
              -1,0.121 ,0.254 ,0.344 ,0.378 ,0.387 ,
              -1,0.131 ,0.275 ,0.359 ,0.389 ,0.398 ,
              -1,0.142 ,0.299 ,0.374 ,0.401 ,0.409 ,
              -1,0.157 ,0.325 ,0.389 ,0.414 ,0.421 ,
            -1,0.177 ,0.351 ,0.405 ,0.427 ,0.433 ,
            -1,0.207 ,0.377 ,0.421 ,0.440 ,0.445 ,
            -1,0.266 ,0.402 ,0.437 ,0.453 ,0.458 ,
            -1,0.359 ,0.426 ,0.454 ,0.468 ,0.472 ,
            -1,0.411 ,0.449 ,0.471 ,0.482 ,0.486,
            0.431 ,0.448 ,0.472 ,0.488 ,0.497 ,0.500 ,
            0.470 ,0.478 ,0.493 ,0.505 ,0.513 ,0.515 ,
            0.500 ,0.505 ,0.515 ,0.523 ,0.529 ,0.531 ,
            0.527 ,0.529 ,0.536 ,0.541 ,0.545 ,0.547 ,
            0.551 ,0.552 ,0.556 ,0.560 ,0.562 ,0.563 ,
            0.574 ,0.575 ,0.576 ,0.578 ,0.580 ,0.580 ,
            0.596 ,0.596 ,0.597 ,0.597 ,0.598 ,0.598 ,
            0.617 ,0.617 ,0.616 ,0.616 ,0.616 ,0.616 ,
            0.637 ,0.637 ,0.636 ,0.636 ,0.635 ,0.635 ,
            0.657 ,0.657 ,0.656 ,0.655 ,0.654 ,0.654
    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_13_next ={
            0.677 ,0.697 ,0.717 ,0.737 ,0.757,
            0.776 ,0.796 ,0.816 ,0.836 ,0.856 ,0.876 ,0.896 ,0.916 ,0.937 ,0.957,
            0.978 ,0.998 ,1.019 ,1.040 ,1.061 ,1.082 ,1.104 ,1.125 ,1.146 ,1.168,
            1.190 ,1.211 ,1.233 ,1.255 ,1.278 ,1.300 ,1.322 ,1.345 ,1.367 ,1.390,
            1.413 ,1.436 ,1.459 ,1.483 ,1.506 ,1.530 ,1.553 ,1.577 ,1.601 ,1.626,
            1.650 ,1.675 ,1.699 ,1.724 ,1.750 ,1.775 ,1.800 ,1.826 ,1.852 ,1.878,
            1.904 ,1.931 ,1.958 ,1.985 ,2.012 ,2.039 ,2.067 ,2.095 ,2.123 ,2.151,
            2.180 ,2.209 ,2.239 ,2.268 ,2.298 ,2.328 ,2.359 ,2.390 ,2.421 ,2.453,
            2.485 ,2.517 ,2.550 ,2.584 ,2.617 ,2.652 ,2.686 ,2.721 ,2.757 ,2.793,
            2.830 ,2.868 ,2.906 ,2.945 ,2.984 ,3.025 ,3.066 ,3.108 ,3.151 ,3.195,
            3.240 ,3.286 ,3.333 ,3.382 ,3.432 ,3.483 ,3.537 ,3.592 ,3.649 ,3.709 ,
            3.772 ,3.837 ,3.907 ,3.981 ,4.060 ,4.146 ,4.241 ,4.349 ,4.476 ,4.639 ,
            4.935
    };

        public static double getA2ρ_13(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_13, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_13, 0.001);
                return A2_ρ_13[x * A2_b_13.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_12_next表中的值
                if (m > 1.5) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_13_next[x];
            }
        }


        public static double[] A2_m_14 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_14 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_14 ={
              -1,0.113 ,0.234 ,0.328 ,0.365 ,0.375,
            -1,0.121 ,0.252 ,0.342 ,0.376 ,0.385,
            -1,0.130 ,0.273 ,0.357 ,0.388 ,0.396 ,
            -1,0.142 ,0.297 ,0.372 ,0.400 ,0.408,
            -1,0.157 ,0.322 ,0.387 ,0.412 ,0.419 ,
            -1,0.177 ,0.348 ,0.403 ,0.425 ,0.431 ,
            -1,0.206 ,0.374 ,0.419 ,0.438 ,0.444,
            -1,0.261 ,0.398 ,0.435 ,0.451 ,0.456,
            -1,0.351 ,0.422 ,0.451 ,0.465 ,0.469 ,
            -1,0.405 ,0.446 ,0.468 ,0.480 ,0.483 ,
            0.424 ,0.443 ,0.468 ,0.485 ,0.494 ,0.497 ,
            0.464 ,0.473 ,0.490 ,0.502 ,0.510 ,0.512 ,
            0.495 ,0.500 ,0.511 ,0.520 ,0.526 ,0.528 ,
            0.522 ,0.525 ,0.531 ,0.538 ,0.542 ,0.543 ,
            0.546 ,0.548 ,0.552 ,0.556 ,0.559 ,0.560 ,
            0.569 ,0.570 ,0.572 ,0.574 ,0.576 ,0.577 ,
            0.590 ,0.591 ,0.592 ,0.593 ,0.594 ,0.594,
            0.611 ,0.612 ,0.612 ,0.612 ,0.612 ,0.612 ,
            0.632 ,0.632 ,0.631 ,0.631 ,0.630 ,0.630,
            0.652 ,0.652 ,0.651 ,0.650 ,0.649 ,0.649
    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_14_next ={
          0.672 ,0.691 ,0.711 ,0.730 ,0.750,
            0.769 ,0.789 ,0.808 ,0.828 ,0.847 ,0.867 ,0.887 ,0.907 ,0.927 ,0.947 ,
            0.967 ,0.987 ,1.008 ,1.028 ,1.049 ,1.069 ,1.090 ,1.111 ,1.132 ,1.153 ,
            1.174 ,1.195 ,1.216 ,1.238 ,1.259 ,1.281 ,1.302 ,1.324 ,1.346 ,1.368 ,
            1.390 ,1.412 ,1.435 ,1.457 ,1.480 ,1.503 ,1.525 ,1.548 ,1.572 ,1.595,
            1.618 ,1.642 ,1.665 ,1.689 ,1.713 ,1.737 ,1.761 ,1.786 ,1.810 ,1.835 ,
            1.860 ,1.885 ,1.910 ,1.935 ,1.961 ,1.987 ,2.013 ,2.039 ,2.065 ,2.091 ,
            2.118 ,2.145 ,2.172 ,2.200 ,2.227 ,2.255 ,2.283 ,2.311 ,2.340 ,2.369,
            2.398 ,2.427 ,2.457 ,2.487 ,2.517 ,2.548 ,2.579 ,2.610 ,2.641 ,2.673 ,
            2.706 ,2.738 ,2.771 ,2.805 ,2.839 ,2.873 ,2.908 ,2.943 ,2.979 ,3.015,
            3.052 ,3.089 ,3.127 ,3.166 ,3.205 ,3.245 ,3.285 ,3.327 ,3.369 ,3.412,
            3.456 ,3.501 ,3.547 ,3.594 ,3.643 ,3.693 ,3.744 ,3.796 ,3.851 ,3.907,
            3.966 ,4.026 ,4.090 ,4.157 ,4.227 ,4.301 ,4.380 ,4.466 ,4.560 ,4.665,
            4.785 ,4.932 ,5.139
    };

        public static double getA2ρ_14(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_14, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_14, 0.001);
                return A2_ρ_14[x * A2_b_14.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_12_next表中的值
                if (m > 1.62) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_14_next[x];
            }
        }


        public static double[] A2_m_15 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_15 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_15 ={
            -1,0.112 ,0.233 ,0.326 ,0.363 ,0.374,
            -1,0.121 ,0.251 ,0.341 ,0.375 ,0.384 ,
            -1,0.130 ,0.272 ,0.355 ,0.386 ,0.395 ,
            -1,0.142 ,0.295 ,0.370 ,0.398 ,0.406 ,
            -1,0.157 ,0.320 ,0.385 ,0.411 ,0.418,
            -1,0.176 ,0.346 ,0.401 ,0.423 ,0.430 ,
            -1,0.205 ,0.371 ,0.417 ,0.436 ,0.442,
            -1,0.257 ,0.396 ,0.433 ,0.449 ,0.454 ,
            -1,0.345 ,0.420 ,0.449 ,0.463 ,0.468,
            -1,0.401 ,0.443 ,0.465 ,0.477 ,0.481,
            0.418 ,0.439 ,0.465 ,0.482 ,0.492 ,0.495 ,
            0.459 ,0.469 ,0.486 ,0.500 ,0.507 ,0.510 ,
            0.491 ,0.496 ,0.507 ,0.517 ,0.523 ,0.525 ,
            0.517 ,0.521 ,0.528 ,0.535 ,0.539 ,0.541 ,
            0.542 ,0.544 ,0.548 ,0.553 ,0.556 ,0.557,
            0.565 ,0.566 ,0.568 ,0.571 ,0.573 ,0.573 ,
            0.586 ,0.587 ,0.588 ,0.589 ,0.590 ,0.590 ,
            0.607 ,0.607 ,0.607 ,0.608 ,0.608 ,0.608,
            0.627 ,0.627 ,0.627 ,0.626 ,0.626 ,0.626 ,
            0.647 ,0.647 ,0.646 ,0.645 ,0.645 ,0.644
    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_15_next ={
          0.667 ,0.686 ,0.706 ,0.725 ,0.744,
            0.763 ,0.783 ,0.802 ,0.821 ,0.840 ,0.860 ,0.879 ,0.899 ,0.918 ,0.938,
            0.958 ,0.978 ,0.998 ,1.018 ,1.038 ,1.058 ,1.079 ,1.099 ,1.119 ,1.140,
            1.161 ,1.181 ,1.202 ,1.223 ,1.244 ,1.265 ,1.286 ,1.307 ,1.329 ,1.350,
            1.372 ,1.393 ,1.415 ,1.437 ,1.459 ,1.481 ,1.503 ,1.525 ,1.547 ,1.570 ,
            1.592 ,1.615 ,1.638 ,1.661 ,1.684 ,1.707 ,1.730 ,1.753 ,1.777 ,1.801 ,
            1.824 ,1.848 ,1.872 ,1.897 ,1.921 ,1.945 ,1.970 ,1.995 ,2.020 ,2.045,
            2.070 ,2.096 ,2.121 ,2.147 ,2.173 ,2.199 ,2.226 ,2.252 ,2.279 ,2.306,
            2.333 ,2.360 ,2.388 ,2.415 ,2.443 ,2.471 ,2.500 ,2.529 ,2.557 ,2.587,
            2.616 ,2.646 ,2.676 ,2.706 ,2.736 ,2.767 ,2.798 ,2.830 ,2.862 ,2.894,
            2.926 ,2.959 ,2.992 ,3.026 ,3.060 ,3.094 ,3.129 ,3.164 ,3.200 ,3.236,
            3.273 ,3.310 ,3.348 ,3.386 ,3.425 ,3.465 ,3.505 ,3.546 ,3.588 ,3.630,
            3.673 ,3.718 ,3.763 ,3.809 ,3.856 ,3.904 ,3.954 ,4.004 ,4.057 ,4.110 ,
            4.166 ,4.223 ,4.282 ,4.344 ,4.408 ,4.476 ,4.546 ,4.621 ,4.701 ,4.786 ,
            4.878 ,4.981 ,5.096 ,5.233 ,5.408 ,5.734

    };

        public static double getA2ρ_15(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_15, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_15, 0.001);
                return A2_ρ_15[x * A2_b_15.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_12_next表中的值
                if (m > 1.75) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_15_next[x];
            }
        }


        public static double[] A2_m_16 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_16 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_16 ={
           -1,0.112 ,0.232 ,0.325 ,0.362 ,0.372,
            -1,0.120 ,0.250 ,0.340 ,0.374 ,0.383 ,
            -1,0.130 ,0.270 ,0.354 ,0.385 ,0.394 ,
            -1,0.142 ,0.293 ,0.369 ,0.397 ,0.405 ,
            -1,0.156 ,0.318 ,0.384 ,0.409 ,0.416 ,
            -1,0.175 ,0.343 ,0.399 ,0.422 ,0.428 ,
            -1,0.203 ,0.369 ,0.415 ,0.435 ,0.440 ,
            -1,0.253 ,0.393 ,0.431 ,0.448 ,0.453 ,
            -1,0.339 ,0.417 ,0.447 ,0.461 ,0.466,
            -1,0.396 ,0.440 ,0.463 ,0.476 ,0.479 ,
            0.413 ,0.435 ,0.462 ,0.480 ,0.490 ,0.493 ,
            0.455 ,0.466 ,0.484 ,0.497 ,0.505 ,0.508,
            0.487 ,0.493 ,0.504 ,0.514 ,0.521 ,0.523 ,
            0.514 ,0.517 ,0.525 ,0.532 ,0.537 ,0.538 ,
            0.538 ,0.540 ,0.545 ,0.550 ,0.553 ,0.554 ,
            0.561 ,0.562 ,0.565 ,0.568 ,0.570 ,0.571 ,
            0.582 ,0.583 ,0.584 ,0.586 ,0.587 ,0.587 ,
            0.603 ,0.603 ,0.604 ,0.604 ,0.605 ,0.605 ,
            0.624 ,0.623 ,0.623 ,0.623 ,0.623 ,0.623 ,
            0.643 ,0.643 ,0.642 ,0.641 ,0.641 ,0.641

    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_16_next ={
                0.663 ,0.682 ,0.701 ,0.720 ,0.739 ,
                0.758 ,0.777 ,0.796 ,0.815 ,0.834 ,0.854 ,0.873 ,0.892 ,0.911 ,0.931,
                0.950 ,0.970 ,0.990 ,1.009 ,1.029 ,1.049 ,1.069 ,1.089 ,1.109 ,1.129 ,
                1.150 ,1.170 ,1.190 ,1.211 ,1.231 ,1.252 ,1.273 ,1.293 ,1.314 ,1.335 ,
                1.356 ,1.377 ,1.398 ,1.420 ,1.441 ,1.462 ,1.484 ,1.506 ,1.527 ,1.549 ,
                1.571 ,1.593 ,1.615 ,1.637 ,1.660 ,1.682 ,1.705 ,1.727 ,1.750 ,1.773 ,
                1.796 ,1.819 ,1.842 ,1.865 ,1.889 ,1.912 ,1.936 ,1.960 ,1.984 ,2.008 ,
                2.032 ,2.056 ,2.081 ,2.105 ,2.630 ,2.155 ,2.180 ,2.205 ,2.230 ,2.256 ,
                2.282 ,2.307 ,2.333 ,2.360 ,2.386 ,2.412 ,2.439 ,2.466 ,2.493 ,2.520 ,
                2.548 ,2.575 ,2.603 ,2.631 ,2.660 ,2.688 ,2.717 ,2.746 ,2.775 ,2.805,
                2.834 ,2.864 ,2.894 ,2.925 ,2.956 ,2.987 ,3.018 ,3.050 ,3.082 ,3.114 ,
                3.147 ,3.180 ,3.213 ,3.247 ,3.281 ,3.315 ,3.350 ,3.385 ,3.421 ,3.457 ,
                3.494 ,3.531 ,3.569 ,3.607 ,3.646 ,3.685 ,3.725 ,3.766 ,3.807 ,3.849,
                3.891 ,3.935 ,3.979 ,4.024 ,4.070 ,4.117 ,4.165 ,4.215 ,4.265 ,4.317,
                4.370 ,4.425 ,4.481 ,4.539 ,4.599 ,4.662 ,4.727 ,4.795 ,4.866 ,4.941,
                5.020 ,5.105 ,5.197 ,5.297 ,5.409 ,5.538 ,5.696 ,5.919


    };

        public static double getA2ρ_16(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_16, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_16, 0.001);
                return A2_ρ_16[x * A2_b_16.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_12_next表中的值
                if (m > 1.87) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_16_next[x];
            }
        }


        public static double[] A2_m_17 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_17 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_17 ={
                -1,0.112 ,0.231 ,0.324 ,0.361 ,0.372 ,
                -1,0.120 ,0.249 ,0.338 ,0.373 ,0.382 ,
                -1,0.130 ,0.269 ,0.353 ,0.384 ,0.393 ,
                -1,0.141 ,0.292 ,0.368 ,0.396 ,0.404 ,
                -1,0.156 ,0.316 ,0.383 ,0.408 ,0.415 ,
                -1,0.175 ,0.342 ,0.398 ,0.421 ,0.427 ,
                -1,0.203 ,0.367 ,0.413 ,0.433 ,0.439 ,
                -1,0.251 ,0.391 ,0.429 ,0.446 ,0.452 ,
                -1,0.334 ,0.415 ,0.445 ,0.460 ,0.464 ,
                -1,0.392 ,0.438 ,0.462 ,0.474 ,0.478 ,
                0.408 ,0.431 ,0.460 ,0.478 ,0.488 ,0.492 ,
                0.451 ,0.463 ,0.481 ,0.495 ,0.503 ,0.506 ,
                0.483 ,0.490 ,0.502 ,0.512 ,0.519 ,0.521 ,
                0.511 ,0.514 ,0.522 ,0.530 ,0.534 ,0.536 ,
                0.535 ,0.537 ,0.542 ,0.547 ,0.551 ,0.552 ,
                0.558 ,0.559 ,0.562 ,0.565 ,0.567 ,0.568 ,
                0.579 ,0.580 ,0.581 ,0.583 ,0.584 ,0.585 ,
                0.600 ,0.600 ,0.601 ,0.601 ,0.602 ,0.602 ,
                0.620 ,0.620 ,0.620 ,0.620 ,0.620 ,0.620 ,
                0.640 ,0.640 ,0.639 ,0.638 ,0.638 ,0.637


    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_17_next ={
                0.659 ,0.678 ,0.697 ,0.716 ,0.735 ,
                0.754 ,0.773 ,0.792 ,0.810 ,0.829 ,0.848 ,0.867 ,0.886 ,0.905 ,0.925,
                0.944 ,0.963 ,0.983 ,1.002 ,1.022 ,1.041 ,1.061 ,1.080 ,1.100 ,1.120 ,
                1.140 ,1.160 ,1.180 ,1.200 ,1.220 ,1.241 ,1.261 ,1.281 ,1.302 ,1.322,
                1.343 ,1.364 ,1.384 ,1.405 ,1.426 ,1.447 ,1.468 ,1.489 ,1.510 ,1.532 ,
                1.553 ,1.575 ,1.596 ,1.618 ,1.640 ,1.661 ,1.683 ,1.705 ,1.727 ,1.750,
                1.772 ,1.794 ,1.817 ,1.839 ,1.862 ,1.885 ,1.908 ,1.931 ,1.951 ,1.977 ,
                2.000 ,2.024 ,2.047 ,2.071 ,2.095 ,2.119 ,2.143 ,2.167 ,2.191 ,2.216,
                2.240 ,2.265 ,2.290 ,2.315 ,2.340 ,2.365 ,2.391 ,2.416 ,2.442 ,2.468,
                2.494 ,2.520 ,2.546 ,2.573 ,2.599 ,2.626 ,2.653 ,2.681 ,2.708 ,2.736,
                2.763 ,2.791 ,2.819 ,2.848 ,2.876 ,2.905 ,2.934 ,2.964 ,2.993 ,3.023 ,
                3.053 ,3.083 ,3.113 ,3.144 ,3.175 ,3.206 ,3.238 ,3.270 ,3.302 ,3.335,
                3.367 ,3.400 ,3.434 ,3.468 ,3.502 ,3.536 ,3.571 ,3.607 ,3.642 ,3.678 ,
                3.715 ,3.752 ,3.790 ,3.828 ,3.866 ,3.905 ,3.945 ,3.985 ,4.026 ,4.067 ,
                4.109 ,4.152 ,4.196 ,4.240 ,4.286 ,4.332 ,4.379 ,4.426 ,4.476 ,4.526 ,
                4.577 ,4.630 ,4.684 ,4.739 ,4.796 ,4.855 ,4.916 ,4.980 ,5.045 ,5.114,
                5.185 ,5.261 ,5.340 ,5.425 ,5.516 ,5.614 ,5.723 ,5.847
    };

        public static double getA2ρ_17(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_17, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_17, 0.001);
                return A2_ρ_17[x * A2_b_17.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_17_next表中的值
                if (m > 1.97) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_17_next[x];
            }
        }


        public static double[] A2_m_18 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_18 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_18 ={
                -1,0.112 ,0.231 ,0.323 ,0.361 ,0.371 ,
                -1,0.120 ,0.248 ,0.338 ,0.372 ,0.381 ,
                -1,0.130 ,0.268 ,0.352 ,0.383 ,0.392 ,
                -1,0.141 ,0.291 ,0.367 ,0.395 ,0.403 ,
                -1,0.156 ,0.315 ,0.382 ,0.407 ,0.414 ,
                -1,0.175 ,0.340 ,0.397 ,0.419 ,0.426 ,
                -1,0.202 ,0.365 ,0.412 ,0.432 ,0.438 ,
                -1,0.248 ,0.389 ,0.428 ,0.445 ,0.450 ,
                -1,0.330 ,0.413 ,0.444 ,0.459 ,0.463 ,
                -1,0.389 ,0.436 ,0.460 ,0.473 ,0.476 ,
                0.403 ,0.428 ,0.458 ,0.477 ,0.487 ,0.490 ,
                0.448 ,0.460 ,0.479 ,0.493 ,0.502 ,0.504 ,
                0.480 ,0.487 ,0.500 ,0.510 ,0.517 ,0.519 ,
                0.508 ,0.512 ,0.520 ,0.528 ,0.533 ,0.534 ,
                0.532 ,0.535 ,0.540 ,0.545 ,0.549 ,0.550 ,
                0.555 ,0.556 ,0.559 ,0.563 ,0.565 ,0.566 ,
                0.576 ,0.577 ,0.579 ,0.581 ,0.582 ,0.583 ,
                0.597 ,0.597 ,0.598 ,0.599 ,0.599 ,0.600 ,
                0.617 ,0.617 ,0.617 ,0.617 ,0.617 ,0.617 ,
                0.637 ,0.637 ,0.636 ,0.635 ,0.635 ,0.635
    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_18_next ={
               0.656 ,0.675 ,0.694 ,0.713 ,0.731 ,
                0.750 ,0.769 ,0.787 ,0.806 ,0.825 ,0.844 ,0.862 ,0.881 ,0.900 ,0.919,
                0.938 ,0.957 ,0.976 ,0.996 ,1.015 ,1.034 ,1.054 ,1.073 ,1.093 ,1.112 ,
                1.132 ,1.152 ,1.171 ,1.191 ,1.211 ,1.231 ,1.251 ,1.271 ,1.291 ,1.311 ,
                1.332 ,1.352 ,1.372 ,1.393 ,1.413 ,1.434 ,1.455 ,1.475 ,1.496 ,1.517,
                1.538 ,1.559 ,1.580 ,1.601 ,1.623 ,1.644 ,1.665 ,1.687 ,1.708 ,1.730,
                1.752 ,1.774 ,1.795 ,1.817 ,1.840 ,1.862 ,1.884 ,1.906 ,1.929 ,1.951,
                1.974 ,1.997 ,2.020 ,2.043 ,2.066 ,2.089 ,2.112 ,2.135 ,2.159 ,2.182 ,
                2.206 ,2.230 ,2.254 ,2.278 ,2.302 ,2.326 ,2.351 ,2.375 ,2.400 ,2.425 ,
                2.450 ,2.475 ,2.500 ,2.525 ,2.551 ,2.576 ,2.602 ,2.628 ,2.654 ,2.680,
                2.706 ,2.733 ,2.760 ,2.787 ,2.814 ,2.841 ,2.868 ,2.896 ,2.923 ,2.951,
                2.979 ,3.008 ,3.036 ,3.065 ,3.094 ,3.123 ,3.152 ,3.181 ,3.211 ,3.241,
                3.271 ,3.302 ,3.333 ,3.364 ,3.395 ,3.426 ,3.458 ,3.490 ,3.522 ,3.555,
                3.588 ,3.621 ,3.655 ,3.689 ,3.723 ,3.757 ,3.792 ,3.828 ,3.863 ,3.900 ,
                3.936 ,3.973 ,4.010 ,4.048 ,4.087 ,4.126 ,4.165 ,4.205 ,4.245 ,4.286 ,
                4.328 ,4.370 ,4.413 ,4.457 ,4.502 ,4.547 ,4.593 ,4.640 ,4.688 ,4.736 ,
                4.786 ,4.837 ,4.889 ,4.943 ,4.998 ,5.054 ,5.112 ,5.172 ,5.234 ,5.298
    };

        public static double getA2ρ_18(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_18, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_18, 0.001);
                return A2_ρ_18[x * A2_b_18.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_18_next表中的值
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_18_next[x];
            }
        }



        public static double[] A2_m_19 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_19 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_19 ={
                -1,0.112 ,0.230 ,0.322 ,0.360 ,0.370 ,
                -1,0.120 ,0.248 ,0.337 ,0.371 ,0.381 ,
                -1,0.130 ,0.268 ,0.351 ,0.383 ,0.391 ,
                -1,0.141 ,0.290 ,0.366 ,0.394 ,0.402 ,
                -1,0.156,0.314,0.381,0.406,0.414,
                -1,0.174,0.339,0.396,0.418,0.425,
                -1,0.201,0.364,0.411,0.431,0.437,
                -1,0.247,0.388,0.427,0.444,0.449,
                -1,0.326,0.411,0.442,0.458,0.462,
                -1,0.386,0.434,0.459,0.471,0.475,
                0.399,0.426,0.456,0.475,0.486,0.489,
                0.445,0.457,0.477,0.492,0.5,0.503,
                0.478,0.485,0.498,0.509,0.515,0.518,
                0.505,0.509,0.518,0.526,0.531,0.533,
                0.530,0.532,0.538,0.543,0.547,0.548,
                0.552,0.554,0.557,0.561,0.563,0.564,
                0.574,0.575,0.577,0.579,0.58,0.581,
                0.595,0.595,0.596,0.597,0.597,0.597,
                0.615,0.615,0.615,0.615,0.615,0.615,
                0.634,0.634,0.633,0.633,0.632,0.632

    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_19_next ={
                0.653,0.672,0.691,0.71,0.728,
                0.747,0.765,0.784,0.802,0.821,0.839,0.858,0.877,0.895,0.914,
                0.933,0.952,0.971,0.99,1.009,1.028,1.047,1.067,1.086,1.105,
                1.125,1.144,1.164,1.183,1.203,1.223,1.242,1.262,1.282,1.302,
                1.322,1.342,1.362,1.382,1.402,1.423,1.443,1.463,1.484,1.504,
                1.525,1.545,1.566,1.587,1.608,1.629,1.65,1.671,1.692,1.713,
                1.734,1.756,1.777,1.799,1.82,1.842,1.864,1.886,1.908,1.93,
                1.952,1.974,1.996,2.018,2.041,2.063,2.086,2.109,2.131,2.154,
                2.177,2.2,2.224,2.247,2.27,2.294,2.317,2.341,2.365,2.389,
                2.413,2.437,2.461,2.486,2.51,2.535,2.56,2.584,2.609,2.635,
                2.66,2.685,2.711,2.736,2.762,2.788,2.814,2.84,2.867,2.893,
                2.92,2.947,2.974,3.001,3.028,3.056,3.083,3.111,3.139,3.167,
                3.196,3.224,3.253,3.282,3.311,3.34,3.37,3.4,3.43,3.46,
                3.49,3.521,3.552,3.583,3.614,3.646,3.678,3.71,3.743,3.775,
                3.808,3.842,3.875,3.909,3.944,3.978,4.013,4.049,4.084,4.121,
                4.157,4.194,4.231,4.269,4.307,4.346,4.385,4.425,4.465,4.506,
                4.547,4.589,4.631,4.674,4.718,4.763,4.808,4.854,4.901,4.949

    };

        public static double getA2ρ_19(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_19, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_19, 0.001);
                return A2_ρ_19[x * A2_b_19.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_19next表中的值
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_19_next[x];
            }
        }
        /// <summary>
        /// GJB/Z 377A2-94  续表 A2（N=20，21）
        /// </summary>

        public static double[] A2_m_20 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_20 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_20 ={
                -1,0.112,0.230,0.322,0.359,0.369,
                -1,0.120,0.247,0.336,0.370,0.380,
                -1,0.130,0.267,0.350,0.382,0.391,
                -1,0.141,0.289,0.365,0.393,0.402,
                -1,0.155,0.313,0.380,0.405,0.413,
                -1,0.174,0.337,0.395,0.418,0.424,
                -1,0.200,0.362,0.410,0.430,0.436,
                -1,0.245,0.286,0.426,0.443,0.448,
                -1,0.323,0.410,0.441,0.456,0.461,
                -1,0.383,0.432,0.457,0.470,0.474,
                0.396,0.423,0.454,0.474,0.484,0.488,
                0.442,0.455,0.475,0.490,0.499,0.502,
                0.475,0.482,0.496,0.507,0.514,0.516,
                0.503,0.507,0.516,0.524,0.529,0.531,
                0.527,0.530,0.536,0.541,0.545,0.547,
                0.550,0.552,0.555,0.559,0.562,0.562,
                0.572,0.572,0.574,0.577,0.578,0.579,
                0.592,0.593,0.593,0.594,0.595,0.595,
                0.612,0.612,0.612,0.612,0.613,0.613,
                0.632,0.631,0.631,0.631,0.630,0.630

    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_20_next ={
                0.651,0.670,0.688,0.707,0.725,
                0.744,0.762,0.780,0.799,0.817,0.836,0.854,0.873,0.891,0.910,
                0.929,0.947,0.966,0.985,1.004,1.023,1.042,1.061,1.080,1.099,
                1.119,1.138,1.157,1.176,1.196,1.215,1.235,1.254,1.274,1.294,
                1.313,1.333,1.353,1.373,1.393,1.413,1.433,1.453,1.473,1.493,
                1.513,1.534,1.554,1.575,1.595,1.616,1.636,1.657,1.678,1.699,
                1.719,1.740,1.761,1.783,1.804,1.825,1.846,1.868,1.889,1.911,
                1.932,1.954,1.976,1.998,2.020,2.042,2.064,2.086,2.108,2.130,
                2.153,2.175,2.198,2.221,2.243,2.266,2.289,2.312,2.335,2.358,
                2.382,2.405,2.429,2.452,2.476,2.500,2.524,2.548,2.572,2.596,
                2.621,2.645,2.670,2.694,2.719,2.744,2.769,2.794,2.820,2.845,
                2.871,2.896,2.922,2.948,2.974,3.000,3.027,3.053,3.080,3.107,
                3.134,3.161,3.188,3.216,3.243,3.271,3.299,3.327,3.355,3.384,
                3.412,3.441,3.470,3.499,3.529,3.558,3.588,3.618,3.648,3.679,
                3.709,3.740,3.771,3.803,3.834,3.866,3.898,3.930,3.963,3.996,
                4.029,4.062,4.096,4.130,4.165,4.199,4.234,4.270,4.305,4.342,
                4.378,4.415,4.452,4.490,4.528,4.566,4.605,4.645,4.684,4.725
    };


        public static double[] A2_ρ_21_next ={
                0.649,0.667,0.686,0.704,0.723,
                0.741,0.759,0.778,0.796,0.814,0.833,0.851,0.869,0.888,0.906,
                0.925,0.943,0.962,0.981,0.999,1.018,1.037,1.056,1.075,1.094,
                1.113,1.132,1.151,1.170,1.189,1.209,1.228,1.247,1.267,1.286,
                1.306,1.325,1.345,1.364,1.384,1.404,1.424,1.443,1.463,1.483,
                1.503,1.524,1.544,1.565,1.585,1.606,1.626,1.647,1.667,1.689,
                1.706,1.727,1.748,1.768,1.789,1.810,1.831,1.852,1.873,1.894,
                1.915,1.937,1.958,1.980,2.001,2.023,2.044,2.066,2.088,2.109,
                2.131,2.153,2.175,2.198,2.220,2.242,2.265,2.287,2.310,2.332,
                2.355,2.378,2.401,2.424,2.447,2.470,2.493,2.517,2.540,2.564,
                2.587,2.611,2.635,2.659,2.683,2.707,2.731,2.755,2.780,2.804,
                2.829,2.854,2.879,2.904,2.929,2.954,2.979,3.005,3.031,3.056,
                3.082,3.108,3.134,3.160,3.187,3.213,3.240,3.267,3.294,3.321,
                3.348,3.376,3.403,3.431,3.459,3.487,3.515,3.543,3.572,3.600,
                3.629,3.658,3.687,3.717,3.746,3.776,3.806,3.836,3.867,3.897,
                3.928,3.959,3.991,4.022,4.054,4.086,4.118,4.151,4.183,4.216,
                4.250,4.283,4.317,4.351,4.386,4.420,4.455,4.491,4.526,4.562,
    };
        public static double getA2ρ_20(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);

            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_20, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_20, 0.001);
                return A2_ρ_20[x * A2_b_20.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_19next表中的值
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_20_next[x];
            }
        }

        /// <summary>
        /// 获取n=21的ρ 和n=20公用的一部分数据
        /// </summary>
        /// <param name="m"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double getA2ρ_21(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_20, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_20, 0.001);
                return A2_ρ_20[x * A2_b_20.Length + y];
            }
            else
            {
                //当m>0.35的时候直接取A2_ρ_19next表中的值
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_21_next[x];
            }
        }

        /// <summary>
        /// page55 续表 A2（N=22,23,24）
        /// </summary>

        public static double[] A2_m_22 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_22 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_22 ={
                -1,0.112,0.229,0.320,0.358,0.368,
                -1,0.120,0.246,0.334,0.369,0.378,
                -1,0.129,0.265,0.348,0.380,0.389,
                -1,0.141,0.287,0.363,0.392,0.400,
                -1,0.155,0.310,0.377,0.403,0.411,
                -1,0.173,0.334,0.392,0.416,0.422,
                -1,0.199,0.359,0.407,0.428,0.434,
                -1,0.241,0.383,0.423,0.441,0.446,
                -1,0.315,0.406,0.438,0.454,0.459,
                -1,0.376,0.429,0.454,0.467,0.472,
                0.386,0.418,0.450,0.470,0.481,0.485,
                0.435,0.450,0.471,0.487,0.496,0.499,
                0.469,0.477,0.492,0.503,0.511,0.513,
                0.497,0.502,0.512,0.520,0.526,0.528,
                0.522,0.525,0.531,0.537,0.541,0.543,
                0.545,0.546,0.550,0.555,0.557,0.558,
                0.566,0.567,0.569,0.572,0.574,0.574,
                0.587,0.587,0.588,0.590,0.590,0.591,
                0.606,0.607,0.607,0.607,0.608,0.608,
                0.626,0.626,0.625,0.625,0.625,0.625

    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_22_next ={
            0.647,0.665,0.684,0.702,0.720,
            0.739,0.757,0.775,0.793,0.811,0.830,0.848,0.866,0.884,0.903,
            0.921,0.940,0.958,0.977,0.995,1.014,1.033,1.052,1.070,1.089,
            1.108,1.127,1.146,1.165,1.184,1.203,1.222,1.241,1.260,1.280,
            1.299,1.318,1.338,1.357,1.377,1.396,1.416,1.435,1.455,1.475,
            1.494,1.514,1.534,1.554,1.574,1.594,1.614,1.634,1.654,1.674,
            1.695,1.715,1.735,1.756,1.776,1.797,1.818,1.838,1.859,1.880,
            1.901,1.922,1.943,1.964,1.985,2.006,2.027,2.048,2.070,2.091,
            2.113,2.134,2.156,2.178,2.199,2.221,2.243,2.265,2.287,2.310,
            2.332,2.354,2.376,2.399,2.421,2.444,2.467,2.490,2.512,2.535,
            2.558,2.581,2.605,2.628,2.651,2.675,2.698,2.722,2.746,2.770,
            2.794,2.818,2.842,2.866,2.890,2.915,2.939,2.964,2.989,3.013,
            3.038,3.063,3.089,3.114,3.139,3.165,3.190,3.216,3.242,3.268,
            3.294,3.321,3.347,3.373,3.400,3.427,3.454,3.481,3.508,3.535,
            3.563,3.591,3.618,3.646,3.674,3.703,3.731,3.760,3.788,3.817,
            3.846,3.876,3.905,3.935,3.965,3.994,4.025,4.055,4.086,4.116,
            4.148,4.179,4.210,4.242,4.274,4.306,4.338,4.371,4.404,4.437
    };


        public static double[] A2_ρ_23_next ={
            0.645,0.663,0.682,0.700,0.718,
            0.736,0.755,0.773,0.791,0.809,0.827,0.845,0.863,0.882,0.900,
            0.918,0.936,0.955,0.973,0.992,1.010,1.029,1.047,1.066,1.085,
            1.103,1.122,1.141,1.160,1.179,1.198,1.217,1.236,1.255,1.274,
            1.293,1.312,1.331,1.350,1.370,1.389,1.408,1.428,1.447,1.467,
            1.486,1.506,1.526,1.545,1.565,1.585,1.605,1.624,1.644,1.664,
            1.684,1.704,1.725,1.745,1.765,1.785,1.806,1.826,1.847,1.867,
            1.888,1.908,1.929,1.950,1.970,1.991,2.012,2.033,2.054,2.075,
            2.096,2.118,2.139,2.160,2.182,2.203,2.225,2.246,2.268,2.290,
            2.311,2.333,2.355,2.377,2.399,2.421,2.444,2.466,2.488,2.511,
            2.533,2.556,2.578,2.601,2.624,2.647,2.670,2.693,2.716,2.739,
            2.763,2.786,2.810,2.833,2.857,2.881,2.904,2.928,2.952,2.976,
            3.001,3.025,3.049,3.074,3.099,3.123,3.148,3.173,3.198,3.223,
            3.248,3.274,3.299,3.325,3.350,3.376,3.402,3.428,3.454,3.481,
            3.507,3.533,3.560,3.587,3.614,3.641,3.668,3.695,3.723,3.750,
            3.778,3.806,3.834,3.862,3.890,3.919,3.948,3.976,4.005,4.034,
            4.064,4.093,4.123,4.153,4.183,4.213,4.243,4.274,4.305,4.336
    };
        public static double[] A2_ρ_24_next ={
            0.643,0.662,0.680,0.698,0.716,
            0.735,0.753,0.771,0.789,0.807,0.825,0.843,0.861,0.879,0.897,
            0.915,0.933,0.952,0.970,0.988,1.007,1.025,1.044,1.062,1.081,
            1.099,1.118,1.137,1.155,1.174,1.193,1.212,1.231,1.250,1.268,
            1.287,1.306,1.325,1.345,1.364,1.383,1.402,1.421,1.441,1.460,
            1.479,1.499,1.518,1.538,1.557,1.577,1.596,1.616,1.636,1.655,
            1.675,1.695,1.715,1.735,1.755,1.775,1.795,1.815,1.835,1.856,
            1.876,1.896,1.917,1.937,1.958,1.978,1.999,2.019,2.040,2.061,
            2.082,2.103,2.124,2.145,2.166,2.187,2.208,2.229,2.250,2.272,
            2.293,2.315,2.336,2.358,2.380,2.401,2.423,2.445,2.467,2.489,
            2.511,2.533,2.555,2.578,2.600,2.622,2.645,2.667,2.690,2.713,
            2.736,2.758,2.781,2.804,2.828,2.851,2.874,2.897,2.921,2.944,
            2.968,2.992,3.015,3.039,3.063,3.087,3.111,3.135,3.160,3.184,
            3.209,3.233,3.258,3.283,3.308,3.332,3.358,3.383,3.408,3.433,
            3.459,3.485,3.510,3.536,3.562,3.588,3.614,3.640,3.667,3.693,
            3.720,3.747,3.774,3.801,3.828,3.855,3.883,3.910,3.938,3.966,
            3.994,4.022,4.050,4.078,4.107,4.136,4.164,4.193,4.223,4.252
    };
        public static double getA2ρ_22(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_22, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_22, 0.001);
                return A2_ρ_22[x * A2_b_22.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_22_next[x];
            }
        }


        public static double getA2ρ_23(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_22, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_22, 0.001);
                return A2_ρ_22[x * A2_b_22.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_23_next[x];
            }
        }

        public static double getA2ρ_24(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_22, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_22, 0.001);
                return A2_ρ_22[x * A2_b_22.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_24_next[x];
            }
        }


        /// <summary>
        /// page57 续表 A2（N=25,26,27）
        /// </summary>

        public static double[] A2_m_25 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_25 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_25 ={
                -1,0.112,0.228,0.319,0.356,0.367,
                -1,0.120,0.245,0.333,0.367,0.377,
                -1,0.129,0.264,0.347,0.379,0.388,
                -1,0.140,0.285,0.361,0.390,0.398,
                -1,0.155,0.308,0.376,0.402,0.409,
                -1,0.173,0.332,0.391,0.414,0.421,
                -1,0.198,0.356,0.406,0.426,0.432,
                -1,0.238,0.380,0.421,0.439,0.444,
                -1,0.309,0.403,0.436,0.452,0.457,
                -1,0.371,0.426,0.452,0.465,0.470,
                0.378,0.413,0.447,0.468,0.479,0.483,
                0.430,0.446,0.468,0.484,0.493,0.496,
                0.465,0.473,0.488,0.501,0.508,0.510,
                0.493,0.498,0.508,0.517,0.523,0.525,
                0.518,0.521,0.528,0.534,0.538,0.540,
                0.541,0.542,0.547,0.551,0.554,0.555,
                0.562,0.563,0.566,0.568,0.570,0.571,
                0.582,0.583,0.584,0.586,0.587,0.587,
                0.602,0.602,0.603,0.603,0.603,0.604,
                0.621,0.621,0.621,0.621,0.621,0.621

    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_25_next ={
        0.642,0.660,0.678,0.697,0.715,
        0.733,0.751,0.769,0.787,0.804,0.822,0.840,0.858,0.876,0.895,
        0.913,0.931,0.949,0.967,0.985,1.004,1.022,1.040,1.059,1.077,
        1.096,1.114,1.133,1.151,1.170,1.189,1.207,1.226,1.245,1.264,
        1.282,1.301,1.320,1.339,1.358,1.377,1.396,1.415,1.434,1.453,
        1.473,1.492,1.511,1.530,1.550,1.569,1.589,1.608,1.628,1.647,
        1.667,1.686,1.706,1.726,1.746,1.766,1.785,1.805,1.825,1.845,
        1.865,1.885,1.906,1.926,1.946,1.966,1.987,2.007,2.028,2.048,
        2.069,2.089,2.110,2.131,2.151,2.172,2.193,2.214,2.235,2.256,
        2.277,2.298,2.319,2.341,2.362,2.383,2.405,2.426,2.448,2.470,
        2.491,2.513,2.535,2.557,2.579,2.601,2.623,2.645,2.667,2.689,
        2.712,2.734,2.757,2.779,2.802,2.824,2.847,2.870,2.893,2.916,
        2.939,2.962,2.985,3.009,3.032,3.055,3.079,3.103,3.126,3.150,
        3.174,3.198,3.222,3.246,3.270,3.294,3.319,3.343,3.368,3.392,
        3.417,3.442,3.467,3.492,3.517,3.542,3.568,3.593,3.619,3.644,
        3.670,3.696,3.722,3.748,3.774,3.800,3.827,3.853,3.880,3.907,
        3.934,3.961,3.988,4.015,4.042,4.070,4.097,4.125,4.153,4.181


    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_26_next ={
        0.640,0.659,0.677,0.695,0.713,
        0.731,0.749,0.767,0.785,0.803,0.820,0.838,0.856,0.874,0.892,
        0.910,0.928,0.946,0.965,0.983,1.001,1.019,1.037,1.056,1.074,
        1.092,1.111,1.129,1.148,1.166,1.185,1.203,1.222,1.241,1.259,
        1.278,1.297,1.315,1.334,1.353,1.372,1.391,1.410,1.429,1.448,
        1.467,1.486,1.505,1.524,1.543,1.563,1.582,1.601,1.620,1.640,
        1.659,1.679,1.698,1.718,1.737,1.757,1.777,1.796,1.816,1.836,
        1.856,1.876,1.896,1.916,1.936,1.956,1.976,1.996,2.016,2.036,
        2.057,2.077,2.098,2.118,2.138,2.159,2.180,2.200,2.221,2.242,
        2.263,2.283,2.304,2.325,2.346,2.367,2.388,2.410,2.431,2.452,
        2.474,2.495,2.516,2.538,2.560,2.581,2.603,2.625,2.646,2.668,
        2.690,2.712,2.734,2.757,2.779,2.801,2.823,2.846,2.868,2.891,
        2.913,2.936,2.959,2.981,3.004,3.027,3.050,3.073,3.097,3.120,
        3.143,3.167,3.190,3.214,3.237,3.261,3.285,3.309,3.332,3.356,
        3.381,3.405,3.429,3.453,3.478,3.502,3.527,3.552,3.577,3.601,
        3.626,3.652,3.677,3.702,3.727,3.753,3.778,3.804,3.830,3.856,
        3.882,3.908,3.934,4.960,3.987,4.013,4.040,4.067,4.093,4.120

    };

        public static double[] A2_ρ_27_next ={
        0.639,0.657,0.676,0.694,0.712,
        0.730,0.747,0.765,0.783,0.801,0.819,0.836,0.854,0.872,0.890,
        0.908,0.926,0.944,0.962,0.980,0.998,1.016,1.035,1.053,1.071,
        1.089,1.108,1.126,1.144,1.163,1.181,1.200,1.218,1.237,1.255,
        1.274,1.292,1.311,1.330,1.348,1.367,1.386,1.405,1.424,1.443,
        1.461,1.480,1.499,1.518,1.537,1.556,1.576,1.595,1.614,1.633,
        1.652,1.672,1.691,1.710,1.730,1.749,1.769,1.788,1.808,1.828,
        1.847,1.867,1.887,1.906,1.926,1.946,1.966,1.986,2.006,2.026,
        2.046,2.066,2.086,2.107,2.127,2.147,2.168,2.188,2.208,2.229,
        2.249,2.270,2.291,2.311,2.332,2.353,2.374,2.395,2.416,2.437,
        2.458,2.479,2.500,2.521,2.542,2.564,2.585,2.607,2.628,2.650,
        2.671,2.693,2.714,2.736,2.758,2.780,2.802,2.824,2.846,2.868,
        2.890,2.913,2.935,2.957,2.980,3.002,3.025,3.048,3.070,3.093,
        3.116,3.139,3.162,3.185,3.208,3.231,3.254,3.278,3.301,3.325,
        3.348,3.372,3.396,3.419,3.443,3.467,3.491,3.515,3.539,3.564,
        3.588,3.612,3.637,3.662,3.686,3.711,3.736,3.761,3.786,3.811,
        3.836,3.862,3.887,3.913,3.938,3.964,3.990,4.016,4.042,4.068
    };
        public static double getA2ρ_25(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_25, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_25, 0.001);
                return A2_ρ_25[x * A2_b_25.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_25_next[x];
            }
        }

        public static double getA2ρ_26(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_25, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_25, 0.001);
                return A2_ρ_25[x * A2_b_25.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_26_next[x];
            }
        }

        public static double getA2ρ_27(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_25, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_25, 0.001);
                return A2_ρ_25[x * A2_b_25.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_27_next[x];
            }
        }

        /// <summary>
        /// page59 续表 A2（N=28,29）
        /// </summary>

        public static double[] A2_m_28 ={
             0.15,0.16,0.17, 0.18,0.19,0.20, 0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,0.30,
            0.31, 0.32,0.33,0.34 };

        public static double[] A2_b_28 ={
             0.00,0.1,0.2,0.3,0.4,0.5

    };

        public static double[] A2_ρ_28 ={
            -1,0.112,0.227,0.318,0.355,0.366,
            -1,0.120,0.244,0.331,0.366,0.376,
            -1,0.129,0.263,0.346,0.378,0.387,
            -1,0.140,0.284,0.360,0.389,0.397,

            -1,0.154,0.307,0.374,0.401,0.408,
            -1,0.172,0.330,0.389,0.413,0.420,
            -1,0.197,0.354,0.404,0.425,0.431,
            -1,0.237,0.378,0.419,0.438,0.443,
            -1,0.304,0.401,0.435,0.451,0.455,

            -1,0.367,0.423,0.450,0.464,0.468,
            0.372,0.410,0.445,0.466,0.478,0.481,
            0.426,0.442,0.466,0.482,0.492,0.495,
            0.461,0.470,0.486,0.499,0.506,0.509,
            0.490,0.495,0.506,0.515,0.521,0.523,

            0.514,0.518,0.525,0.532,0.536,0.538,
            0.537,0.539,0.544,0.549,0.552,0.553,
            0.559,0.560,0.563,0.566,0.568,0.569,
            0.579,0.580,0.581,0.583,0.584,0.585,
            0.599,0.599,0.600,0.600,0.601,0.601,
            0.618,0.618,0.618,0.618,0.618,0.618

    };

        /// <summary>
        /// 当m>0.35的时候直接取该表中的数值
        /// </summary>
        public static double[] A2_ρ_28_next ={
        0.638,0.656,0.674,0.692,0.710,
        0.728,0.746,0.764,0.781,0.799,0.817,0.835,0.852,0.870,0.888,
        0.906,0.924,0.942,0.960,0.978,0.996,1.014,1.032,1.050,1.068,
        1.087,1.105,1.123,1.141,1.160,1.178,1.196,1.215,1.233,1.252,
        1.270,1.289,1.307,1.326,1.344,1.363,1.382,1.400,1.419,1.438,
        1.456,1.475,1.494,1.513,1.532,1.551,1.570,1.589,1.608,1.627,
        1.646,1.665,1.684,1.704,1.723,1.742,1.762,1.781,1.800,1.820,
        1.839,1.859,1.878,1.898,1.918,1.937,1.957,1.977,1.997,2.016,
        2.036,2.056,2.076,2.096,2.116,2.136,2.157,2.177,2.197,2.217,
        2.238,2.258,2.278,2.299,2.319,2.340,2.360,2.381,2.402,2.422,
        2.443,2.464,2.485,2.506,2.527,2.548,2.569,2.590,2.611,2.633,
        2.654,2.675,2.697,2.718,2.740,2.761,2.783,2.804,2.826,2.848,
        2.870,2.892,2.914,2.936,2.958,2.980,3.002,3.024,3.047,3.069,
        3.091,3.114,3.136,3.159,3.182,3.204,3.227,3.250,3.273,3.296,
        3.319,3.342,3.366,3.389,3.412,3.436,3.459,3.483,3.506,3.530,
        3.554,3.578,3.602,3.626,3.650,3.674,3.698,3.723,3.747,3.772,
        3.796,3.821,3.846,3.870,3.895,3.920,3.945,3.971,3.996,4.021


    };

        public static double[] A2_ρ_29_next ={
            0.637,0.655,0.673,0.691,0.709,
            0.727,0.745,0.762,0.780,0.798,0.815,0.833,0.851,0.869,0.886,
            0.904,0.922,0.940,0.958,0.976,0.994,1.012,1.030,1.048,1.066,
            1.084,1.102,1.120,1.138,1.157,1.175,1.193,1.212,1.230,1.248,
            1.267,1.285,1.303,1.322,1.340,1.359,1.377,1.396,1.415,1.433,
            1.452,1.471,1.489,1.508,1.527,1.546,1.565,1.583,1.602,1.621,
            1.640,1.659,1.678,1.697,1.717,1.736,1.755,1.774,1.793,1.813,
            1.832,1.851,1.871,1.890,1.910,1.929,1.949,1.968,1.988,2.008,
            2.027,2.047,2.067,2.087,2.107,2.127,2.146,2.166,2.186,2.207,
            2.227,2.247,2.267,2.287,2.308,2.328,2.348,2.369,2.389,2.410,
            2.430,2.451,2.471,2.492,2.513,2.534,2.554,2.575,2.596,2.617,
            2.638,2.659,2.680,2.701,2.723,2.744,2.765,2.787,2.808,2.830,
            2.851,2.873,2.894,2.916,2.938,2.960,2.981,3.003,3.025,3.047,
            3.069,3.091,3.114,3.136,3.158,3.181,3.203,3.225,3.248,3.271,
            3.293,3.316,3.339,3.362,3.385,3.408,3.431,3.454,3.477,3.500,
            3.523,3.547,3.570,3.594,3.617,3.461,3.665,3.689,3.712,3.736,
            3.760,3.785,3.809,3.833,3.857,3.882,3.906,3.931,3.955,3.980
    };
        public static double getA2ρ_28(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_28, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_28, 0.001);
                return A2_ρ_28[x * A2_b_28.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_28_next[x];
            }
        }

        public static double getA2ρ_29(double m, double b)
        {
            int x = 0;
            int y = 0;
            m = Math.Round(m, 2);
            if (m < 0.35)
            {
                if (m < 0.15) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A2_m_28, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A2_b_28, 0.001);
                return A2_ρ_28[x * A2_b_28.Length + y];
            }
            else
            {
                if (m > 1.99) return -1;
                x = Convert.ToInt16((m - 0.35) * 100);
                return A2_ρ_29_next[x];
            }
        }


        public static double getA3_G(double ρ, double b)
        {
            int x = 0;
            int y = 0;
            double ρ1, ρ0, x0, x1;
            if (ρ == 1.19) return 0.983;
            if ((ρ > 1.19) && (ρ < 1.195)) return 0.983;
            if ((ρ >= 1.195) && (ρ < 1.2)) return 0.982;
            if (ρ < 0.2) return 0;
            if (ρ < 1.2)
            {
                if (b < 0.3)
                {
                    if (Math.Round(ρ, 2) > ρ)
                    {
                        ρ1 = Math.Round(ρ, 2);
                        ρ0 = ρ1 - 0.01;
                    }
                    else
                    {
                        ρ0 = Math.Round(ρ, 2);
                        ρ1 = ρ0 + 0.01;
                    }
                    x1 = A3_G_ρ_b_2[getIndexOfArray(ρ0, A3_G_ρ, 0.0001)];
                    x0 = A3_G_ρ_b_2[getIndexOfArray(ρ1, A3_G_ρ, 0.0001)];
                    return Math.Round(x1 - 100 * (x1 - x0) * (ρ - ρ0), 3);
                }
                else if (b >= 0.3)
                {
                    if (Math.Round(ρ, 2) > ρ)
                    {
                        ρ1 = Math.Round(ρ, 2);
                        ρ0 = ρ1 - 0.01;
                    }
                    else
                    {
                        ρ0 = Math.Round(ρ, 2);
                        ρ1 = ρ0 + 0.01;
                    }


                    x1 = A3_G_ρ_b_3[getIndexOfArray(ρ0, A3_G_ρ, 0.0001)];
                    x0 = A3_G_ρ_b_3[getIndexOfArray(ρ1, A3_G_ρ, 0.0001)];
                    return Math.Round(x1 - 100 * (x1 - x0) * (ρ - ρ0), 3);
                }
            }
            else
            {
                if (Math.Round(ρ, 1) > ρ)
                {
                    ρ1 = Math.Round(ρ, 1);
                    ρ0 = ρ1 - 0.1;
                }
                else
                {
                    ρ0 = Math.Round(ρ, 1);
                    ρ1 = ρ0 + 0.1;
                }
                if (ρ == 4.9) return 0.908;
                if (ρ > 4.9) return 0;
                getIndexOfArray(Math.Round(ρ0, 1), A3_ρ_next, out x, out y);
                if ((x >= 0) && (y >= 0))
                {
                    if (x == y)
                    {
                        x1 = A3_G_ρ_next[x];
                    }
                    else x1 = (A3_G_ρ_next[x] + A3_G_ρ_next[y]) * 0.5;
                }
                else
                    x1 = 0;

                getIndexOfArray(Math.Round(ρ1, 1), A3_ρ_next, out x, out y);
                if ((x >= 0) && (y >= 0))
                {
                    if (x == y)
                    {
                        x0 = A3_G_ρ_next[x];
                    }
                    else x0 = (A3_G_ρ_next[x] + A3_G_ρ_next[y]) * 0.5;
                }
                else
                    x0 = 0;
                if (x0 == 0)
                    return 0;
                else
                    return Math.Round(x1 - 10 * (x1 - x0) * (ρ - ρ0), 3);

            }
            return 0;
        }


        public static double getA4_H(double ρ, double b)
        {

            int weishu, xishu;
            double ρ1, ρ0, x0, x1, d;
            if (ρ == 1.99) return 1.749;
            if ((ρ > 1.99) && (ρ < 1.9916666666)) return 1.749;
            if ((ρ >= 1.9916666666) && (ρ < 1.995)) return 1.750;
            if ((ρ >= 1.995) && (ρ < 1.99833333333)) return 1.751;
            if ((ρ >= 1.99833333333) && (ρ < 2)) return 1.752;
            if (ρ == 2) return 1.752;
            if (ρ < 0.2) return 0;
            if (ρ > 4.9) return 0;
            if (ρ < 2)
            {
                d = 0.01;
                weishu = 2;
                xishu = 100;
            }
            else
            {
                d = 0.1;
                weishu = 1;
                xishu = 10;
            }
            if (b < 0.3)
            {

                if (Math.Round(ρ, weishu) > ρ)
                {
                    ρ1 = Math.Round(ρ, weishu);
                    ρ0 = ρ1 - d;
                }
                else
                {
                    ρ0 = Math.Round(ρ, weishu);
                    ρ1 = ρ0 + d;
                }
                x1 = A4_H_ρ_b_2[getIndexOfArray(ρ0, A4_H_ρ, 0.0001)];
                x0 = A4_H_ρ_b_2[getIndexOfArray(ρ1, A4_H_ρ, 0.0001)];
                return Math.Round(x1 - xishu * (x1 - x0) * (ρ - ρ0), 3);
            }
            else if (b >= 0.3)
            {
                if (Math.Round(ρ, weishu) > ρ)
                {
                    ρ1 = Math.Round(ρ, weishu);
                    ρ0 = ρ1 - d;
                }
                else
                {
                    ρ0 = Math.Round(ρ, weishu);
                    ρ1 = ρ0 + d;
                }
                x1 = A4_H_ρ_b_3[getIndexOfArray(ρ0, A4_H_ρ, 0.0001)];
                x0 = A4_H_ρ_b_3[getIndexOfArray(ρ1, A4_H_ρ, 0.0001)];
                return Math.Round(x1 - xishu * (x1 - x0) * (ρ - ρ0), 3);
            }
            return 0;
        }


        public static double[] A3_G_ρ_b_2 ={
        1.253,1.253,1.253,1.253,1.253,1.253,1.252,1.252,1.251,1.250,
        1.248,1.246,1.244,1.241,1.238,1.234,1.230,1.226,1.221,1.216,
        1.210,1.205,1.199,1.193,1.188,1.182,1.176,1.170,1.164,1.158,
        1.152,1.147,1.141,1.136,1.130,1.125,1.120,1.115,1.111,1.106,
        1.102,1.097,1.093,1.089,1.085,1.082,1.078,1.075,1.071,1.068,
        1.065,1.062,1.059,1.056,1.053,1.051,1.048,1.046,1.043,1.041,
        1.039,1.036,1.034,1.032,1.030,1.028,1.026,1.024,1.023,1.021,
        1.019,1.017,1.016,1.014,1.013,1.011,1.010,1.008,1.007,1.005,
        1.004,1.003,1.001,1.000,0.999,0.998,0.997,0.995,0.994,0.993,
        0.992,0.991,0.990,0.989,0.988,0.987,0.986,0.985,0.984,0.983
        };

        /// <summary>
        /// 当b=0.3，0.4，0.5
        /// </summary>
        public static double[] A3_G_ρ_b_3 ={
            3.208,2.839,2.517,2.295,2.118,1.975,1.858,1.761,1.680,1.611,
            1.552,1.502,1.458,1.420,1.386,1.357,1.330,1.307,1.286,1.268,
            1.251,1.236,1.222,1.210,1.198,1.188,1.176,1.169,1.161,1.154,
            1.146,1.140,1.134,1.128,1.122,1.117,1.112,1.108,1.103,1.099,
            1.095,1.091,1.087,1.084,1.080,1.077,1.074,1.071,1.068,1.065,
            1.062,1.059,1.056,1.054,1.051,1.049,1.047,1.044,1.042,1.040,
            1.038,1.036,1.033,1.031,1.030,1.028,1.026,1.024,1.022,1.021,
            1.019,1.017,1.016,1.014,1.013,1.011,1.010,1.008,1.007,1.005,
            1.004,1.003,1.001,1.000,0.999,0.998,0.997,0.995,0.994,0.993,
            0.992,0.991,0.990,0.989,0.988,0.987,0.986,0.985,0.984,0.983
        };

        /// <summary>
        /// 当ρ>1.1
        /// </summary>
        public static double[] A3_G_ρ_next ={
            0.982,0.974,0.967,0.961,0.956,0.952,0.948,0.944,
            0.941,0.938,0.936,0.934,0.931,0.930,0.928,0.926,0.925,0.923,
            0.922,0.921,0.920,0.919,0.918,0.917,0.916,0.915,0.914,0.913,
            0.913,0.912,0.911,0.911,0.910,0.910,0.909,0.909,0.908,0.908
        };
        /// <summary>
        /// ???此处的ρ的变化在大于2.0的时候，不知道该怎么取值
        /// </summary>
        public static double[] A4_H_ρ ={
            0.2,0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,
            0.3,0.31,0.32,0.33,0.34,0.35,0.36,0.37,0.38,0.39,
            0.4,0.41,0.42,0.43,0.44,0.45,0.46,0.47,0.48,0.49,
            0.5,0.51,0.52,0.53,0.54,0.55,0.56,0.57,0.58,0.59,
            0.6,0.61,0.62,0.63,0.64,0.65,0.66,0.67,0.68,0.69,
            0.7,0.71,0.72,0.73,0.74,0.75,0.76,0.77,0.78,0.79,
            0.8,0.81,0.82,0.83,0.84,0.85,0.86,0.87,0.88,0.89,
            0.9,0.91,0.92,0.93,0.94,0.95,0.96,0.97,0.98,0.99,
            1.0,1.01,1.02,1.03,1.04,1.05,1.06,1.07,1.08,1.09,
            1.1,1.11,1.12,1.13,1.14,1.15,1.16,1.17,1.18,1.19,
            1.2,1.21,1.22,1.23,1.24,1.25,1.26,1.27,1.28,1.29,
            1.3,1.31,1.32,1.33,1.34,1.35,1.36,1.37,1.38,1.39,
            1.4,1.41,1.42,1.43,1.44,1.45,1.46,1.47,1.48,1.49,
            1.5,1.51,1.52,1.53,1.54,1.55,1.56,1.57,1.58,1.59,
            1.6,1.61,1.62,1.63,1.64,1.65,1.66,1.67,1.68,1.69,
            1.7,1.71,1.72,1.73,1.74,1.75,1.76,1.77,1.78,1.79,
            1.8,1.81,1.82,1.83,1.84,1.85,1.86,1.87,1.88,1.89,
            1.9,1.91,1.92,1.93,1.94,1.95,1.96,1.97,1.98,1.99,
            2,2.1,2.2,2.3,2.4,2.5,2.6,2.7,2.8,2.9,
            3,3.1,3.2,3.3,3.4,3.5,3.6,3.7,3.8,3.9,
            4,4.1,4.2,4.3,4.4,4.5,4.6,4.7,4.8,4.9
        };

        /// <summary>
        /// b=0.0，0.1，0.2
        /// </summary>
        public static double[] A4_H_ρ_b_2 ={
       72.038,43.27,27.988,19.245,13.923,10.513,8.229,6.642,5.503,4.663,
        4.023,3.539,3.156,2.850,2.602,2.408,2.233,2.094,1.977,1.877,
        1.793,1.720,1.658,1.604,1.557,1.517,1.482,1.451,1.424,1.401,
        1.381,1.363,1.348,1.334,1.323,1.313,1.305,1.298,1.292,1.287,
        1.284,1.281,1.278,1.277,1.276,1.276,1.276,1.276,1.277,1.279,
        1.280,1.282,1.284,1.287,1.289,1.292,1.295,1.298,1.301,1.305,
        1.308,1.312,1.315,1.319,1.322,1.326,1.330,1.334,1.337,1.341,
        1.345,1.349,1.353,1.357,1.360,1.364,1.368,1.372,1.376,1.380,
        1.384,1.388,1.391,1.395,1.399,1.403,1.407,1.411,1.415,1.418,
        1.422,1.426,1.430,1.434,1.438,1.442,1.445,1.449,1.453,1.457,
        1.461,1.464,1.468,1.472,1.476,1.480,1.484,1.487,1.491,1.495,
        1.499,1.502,1.506,1.510,1.514,1.518,1.521,1.525,1.529,1.533,
        1.536,1.540,1.544,1.548,1.551,1.555,1.559,1.562,1.566,1.570,
        1.574,1.577,1.581,1.585,1.588,1.592,1.596,1.599,1.603,1.607,
        1.610,1.614,1.618,1.621,1.625,1.629,1.632,1.636,1.639,1.643,
        1.647,1.650,1.654,1.657,1.661,1.664,1.668,1.671,1.675,1.679,
        1.682,1.686,1.689,1.693,1.696,1.700,1.703,1.707,1.710,1.714,
        1.717,1.721,1.724,1.728,1.731,1.735,1.738,1.742,1.745,1.749,
        1.752,1.786,1.820,1.853,1.885,1.917,1.949,1.980,2.011,2.041,
        2.071,2.101,2.130,2.159,2.187,2.215,2.243,2.270,2.298,2.324,
        2.351,2.377,2.403,2.429,2.454,2.480,2.504,2.529,2.554,2.578
        };

        /// <summary>
        /// b=0.0，0.1，0.2
        /// </summary>
        public static double[] A4_H_ρ_b_3 ={
        1.283,1.192,1.100,1.055,1.016,0.987,0.966,0.951,0.941,0.934,
        0.931,0.931,0.933,0.937,0.942,0.949,0.958,0.967,0.977,0.988,
        0.999,1.011,1.023,1.036,1.048,1.061,1.074,1.087,1.099,1.111,
        1.123,1.135,1.146,1.157,1.167,1.177,1.186,1.195,1.204,1.212,
        1.219,1.226,1.233,1.239,1.246,1.251,1.257,1.262,1.267,1.271,
        1.276,1.280,1.284,1.289,1.292,1.296,1.300,1.304,1.307,1.311,
        1.315,1.318,1.322,1.325,1.329,1.332,1.336,1.339,1.343,1.346,
        1.350,1.353,1.357,1.360,1.364,1.367,1.371,1.375,1.378,1.382,
        1.386,1.389,1.393,1.397,1.400,1.404,1.408,1.412,1.415,1.419,
        1.422,1.426,1.430,1.434,1.438,1.442,1.445,1.449,1.453,1.457,
        1.461,1.464,1.468,1.472,1.476,1.480,1.484,1.487,1.491,1.495,
        1.499,1.502,1.506,1.510,1.514,1.518,1.521,1.525,1.529,1.533,
        1.536,1.540,1.544,1.548,1.551,1.555,1.559,1.562,1.566,1.570,
        1.574,1.577,1.581,1.585,1.588,1.592,1.596,1.599,1.603,1.607,
        1.610,1.614,1.618,1.621,1.625,1.629,1.632,1.636,1.639,1.643,
        1.647,1.650,1.654,1.657,1.661,1.664,1.668,1.671,1.675,1.679,
        1.682,1.686,1.689,1.693,1.696,1.700,1.703,1.707,1.710,1.714,
        1.717,1.721,1.724,1.728,1.731,1.735,1.738,1.742,1.745,1.749,
        1.752,1.786,1.820,1.853,1.885,1.917,1.949,1.980,2.011,2.041,
        2.071,2.101,2.130,2.159,2.187,2.215,2.243,2.270,2.298,2.324,
        2.351,2.377,2.403,2.429,2.454,2.480,2.504,2.529,2.554,2.578
        };
        public static double[] A3_G_ρ ={
            0.2,0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,
            0.3,0.31,0.32,0.33,0.34,0.35,0.36,0.37,0.38,0.39,
            0.4,0.41,0.42,0.43,0.44,0.45,0.46,0.47,0.48,0.49,
            0.5,0.51,0.52,0.53,0.54,0.55,0.56,0.57,0.58,0.59,
            0.6,0.61,0.62,0.63,0.64,0.65,0.66,0.67,0.68,0.69,
            0.7,0.71,0.72,0.73,0.74,0.75,0.76,0.77,0.78,0.79,
            0.8,0.81,0.82,0.83,0.84,0.85,0.86,0.87,0.88,0.89,
            0.9,0.91,0.92,0.93,0.94,0.95,0.96,0.97,0.98,0.99,
            1,1.01,1.02,1.03,1.04,1.05,1.06,1.07,1.08,1.09,
            1.1,1.11,1.12,1.13,1.14,1.15,1.16,1.17,1.18,1.19,
        };
        public static double[] A3_ρ_next ={
            1.2,1.3,1.4,1.5,1.6,1.7,1.8,1.9,
          2.0,2.1,2.2,2.3,2.4,2.5,2.6,2.7,2.8,2.9,
          3.0,3.1,3.2,3.3,3.4,3.5,3.6,3.7,3.8,3.9,
          4.0,4.1,4.2,4.3,4.4,4.5,4.6,4.7,4.8,4.9
        };


        /// <summary>
        /// 逻辑斯谛分布
        /// </summary>
        public static double[] A5_H_M_12 ={
               0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,
           0.3,0.31,0.32,0.33,0.34,0.35,0.36,0.37,0.38,0.39,
           0.4,0.41,0.42,0.43,0.44,0.45,0.46,0.47,0.48,0.49
        };

        public static double[] A5_H_b ={
           0.0,0.1,0.2,0.3,0.4,0.5
        };

        public static double[] A5_H_ρ_12 ={
            -1,-1,0.216,0.25,0.264,0.27,
            -1,-1,0.231,0.257,0.272,0.279,
            -1,-1,0.239,0.266,0.281,0.286,
            -1,0.215,0.252,0.276,0.29,0.292,
            -1,0.233,0.265,0.287,0.299,0.301,
            0.234,0.253,0.279,0.297,0.307,0.310,
            0.259,0.271,0.292,0.308,0.316,0.319,
            0.279,0.288,0.305,0.319,0.326,0.329,
            0.297,0.304,0.318,0.329,0.336,0.338,
            0.314,0.319,0.331,0.339,0.346,0.348,
            0.330,0.334,0.343,0.351,0.356,0.358,
            0.345,0.349,0.355,0.362,0.367,0.369,
            0.360,0.363,0.367,0.373,0.378,0.379,
            0.375,0.375,0.380,0.385,0.389,0.390,
            0.387,0.389,0.382,0.397,0.400,0.401,
            0.399,0.402,0.405,0.409,0.411,0.412,
            0.413,0.414,0.418,0.421,0.423,0.424,
            0.426,0.427,0.429,0.433,0.435,0.435,
            0.439,0.440,0.442,0.445,0.446,0.447,
            0.452,0.453,0.454,0.456,0.459,0.459,
            0.465,0.465,0.467,0.469,0.470,0.471,
            0.478,0.478,0.479,0.481,0.482,0.483,
            0.491,0.491,0.492,0.493,0.494,0.494,
            0.504,0.504,0.505,0.506,0.506,0.507,
            0.517,0.517,0.518,0.518,0.519,0.519,
            0.530,0.530,0.531,0.531,0.531,0.532,
            0.543,0.543,0.543,0.544,0.544,0.544,
            0.556,0.556,0.556,0.557,0.557,0.557,
            0.569,0.569,0.569,0.569,0.570,0.570
        };


        /// <summary>
        /// m从0.5到1.29
        /// </summary>
        public static double[] A5_H_ρ_12_Next ={
            0.582,0.595,0.608,0.621,0.635,0.648,0.662,0.675,0.689,0.703,
            0.717,0.731,0.745,0.759,0.773,0.787,0.802,0.816,0.831,0.846,
            0.860,0.875,0.890,0.906,0.921,0.936,0.952,0.967,0.983,0.999,
            1.015,1.031,1.048,1.064,1.081,1.097,1.114,1.132,1.149,1.166,
            1.184,1.202,1.220,1.238,1.256,1.275,1.294,1.313,1.332,1.352,
            1.372,1.392,1.412,1.433,1.453,1.475,1.496,1.518,1.540,1.563,
            1.586,1.610,1.634,1.658,1.683,1.708,1.734,1.761,1.788,1.816,
            1.845,1.874,1.905,1.936,1.969,2.002,2.037,2.074,2.112,2.153
        };


        public static double getA5_H_ρ_12(double m, double b)
        {
            int x = 0;
            int y = 0;

            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                x = getIndexOfArray(Math.Round(m, 2), A5_H_M_12, 0.001);
                y = getIndexOfArray(Math.Round(b, 2), A5_H_b, 0.001);
                return A5_H_ρ_12[x * A5_H_b.Length + y];
            }
            else
            {
                if (m > 1.29) return -1;
                x = Convert.ToInt16((m - 0.5) * 10);
                return A5_H_ρ_12_Next[x];
            }

        }


        public static double[] A5_H_ρ_13 ={
        -1,-1,0.214,0.248,0.263,0.269,
        -1,-1,0.229,0.256,0.27,0.277,
        -1,-1,0.238,0.264,0.279,0.284,
        -1,0.214,0.250,0.274,0.288,0.29,
        -1,0.230,0.263,0.284,0.297,0.299,
        0.230,0.250,0.276,0.295,0.305,0.308,
        0.255,0.268,0.289,0.306,0.314,0.317,
        0.276,0.285,0.302,0.317,0.323,0.326,
        0.294,0.301,0.315,0.326,0.333,0.336,
        0.310,0.316,0.328,0.336,0.343,0.345,
        0.326,0.331,0.340,0.347,0.353,0.355,
        0.341,0.345,0.351,0.359,0.364,0.365,
        0.356,0.359,0.363,0.370,0.374,0.376,
        0.370,0.371,0.376,0.381,0.385,0.386,
        0.383,0.384,0.388,0.393,0.396,0.397,
        0.395,0.397,0.401,0.404,0.407,0.408,
        0.408,0.409,0.413,0.416,0.418,0.419,
        0.421,0.422,0.425,0.428,0.430,0.431,
        0.434,0.435,0.437,0.440,0.442,0.442,
        0.447,0.447,0.449,0.451,0.453,0.454,
        0.459,0.460,0.461,0.463,0.465,0.466,
        0.472,0.472,0.474,0.475,0.477,0.478,
        0.485,0.485,0.486,0.487,0.489,0.489,
        0.497,0.498,0.499,0.500,0.500,0.501,
        0.510,0.510,0.511,0.512,0.512,0.513,
        0.523,0.523,0.524,0.524,0.525,0.525,
        0.536,0.536,0.536,0.537,0.537,0.537,
        0.548,0.548,0.549,0.549,0.550,0.550,
        0.561,0.561,0.561,0.562,0.562,0.562

        };


        /// <summary>
        /// m从0.5到1.49
        /// </summary>
        public static double[] A5_H_ρ_13_Next ={
            0.574,0.586,0.599,0.612,0.625,0.638,0.652,0.665,0.678,0.691,
            0.705,0.718,0.732,0.745,0.759,0.773,0.787,0.800,0.814,0.829,
            0.843,0.857,0.871,0.886,0.900,0.915,0.930,0.945,0.960,0.975,
            0.990,1.005,0.020,1.036,1.051,1.067,1.083,1.099,1.115,1.131,
            1.148,1.164,1.181,1.198,1.214,1.232,1.249,1.266,1.284,1.301,
            1.319,1.337,1.356,1.374,1.393,1.412,1.431,1.450,1.469,1.489,
            1.509,1.529,1.550,1.571,1.592,1.613,1.635,1.657,1.679,1.702,
            1.725,1.748,1.772,1.796,1.821,1.846,1.871,1.898,1.924,1.952,
            1.980,2.008,2.038,2.068,2.099,2.132,2.165,2.199,2.235,2.272,
            2.311,2.352,2.395,2.441,2.491,2.544,2.603,2.669,2.747,2.846

        };


        public static double getA5_H_ρ_13(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_13[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_13[x2 * A5_H_b.Length + y2] - A5_H_ρ_13[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_13[x2 * A5_H_b.Length + y2] - A5_H_ρ_13[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.49) return -1;
                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_13_Next[x1];
            }

        }


        public static double[] A5_H_ρ_14 ={
            -1,-1,0.213,0.246,0.262,0.267,
            -1,-1,0.227,0.255,0.269,0.276,
            -1,-1,0.237,0.262,0.277,0.283,
            -1,0.213,0.248,0.272,0.286,0.289,
            -1,0.227,0.261,0.282,0.295,0.297,
            0.227,0.247,0.274,0.293,0.304,0.306,
            0.252,0.265,0.287,0.303,0.312,0.315,
            0.273,0.282,0.299,0.314,0.321,0.324,
            0.291,0.298,0.312,0.324,0.331,0.333,
            0.307,0.313,0.325,0.334,0.341,0.343,
            0.323,0.328,0.338,0.345,0.351,0.353,
            0.338,0.342,0.348,0.356,0.361,0.363,
            0.353,0.356,0.360,0.367,0.371,0.373,
            0.367,0.368,0.373,0.378,0.382,0.383,
            0.381,0.381,0.385,0.389,0.393,0.394,
            0.391,0.394,0.397,0.401,0.404,0.405,
            0.404,0.406,0.409,0.412,0.415,0.416,
            0.417,0.418,0.421,0.424,0.426,0.427,
            0.430,0.430,0.433,0.436,0.438,0.438,
            0.442,0.443,0.445,0.447,0.449,0.450,
            0.455,0.455,0.457,0.459,0.461,0.461,
            0.467,0.468,0.469,0.471,0.472,0.473,
            0.480,0.480,0.481,0.483,0.484,0.484,
            0.492,0.493,0.493,0.495,0.495,0.496,
            0.505,0.505,0.506,0.507,0.507,0.507,
            0.517,0.517,0.518,0.519,0.519,0.519,
            0.530,0.530,0.530,0.531,0.531,0.532,
            0.542,0.542,0.543,0.543,0.543,0.544,
            0.555,0.555,0.555,0.555,0.556,0.556


        };


        /// <summary>
        /// m从0.5到1.59
        /// </summary>
        public static double[] A5_H_ρ_14_Next ={
            0.567,0.580,0.592,0.605,0.618,0.630,0.643,0.656,0.669,0.682,
            0.695,0.708,0.721,0.734,0.748,0.761,0.774,0.788,0.801,0.815,
            0.829,0.843,0.856,0.870,0.884,0.898,0.913,0.927,0.941,0.955,
            0.970,0.985,0.999,1.014,1.029,1.044,1.059,1.074,1.089,1.105,
            1.120,1.136,1.151,1.167,1.183,1.199,1.215,1.231,1.248,1.264,
            1.281,1.298,1.315,1.332,1.349,1.366,1.384,1.401,1.419,1.437,
            1.455,1.473,1.492,1.511,1.529,1.549,1.568,1.587,1.607,1.627,
            1.647,1.667,1.688,1.709,1.730,1.751,1.773,1.795,1.817,1.840,
            1.863,1.886,1.910,1.934,1.958,1.983,2.009,2.035,2.061,2.088,
            2.115,2.143,2.172,2.202,2.232,2.263,2.295,2.328,2.361,2.397,
            2.433,2.471,2.511,2.552,2.596,2.642,2.691,2.744,2.803,2.867


        };


        public static double getA5_H_ρ_14(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_14[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_14[x2 * A5_H_b.Length + y2] - A5_H_ρ_14[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_14[x2 * A5_H_b.Length + y2] - A5_H_ρ_14[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.59) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_14_Next[x1];
            }

        }


        public static double[] A5_H_ρ_15 ={
            -1,-1,0.211,0.245,0.261,0.266,
            -1,-1,0.226,0.254,0.267,0.274,
            -1,-1,0.236,0.261,0.275,0.282,
            -1,0.212,0.246,0.271,0.284,0.288,
            -1,0.225,0.259,0.281,0.293,0.296,
            0.224,0.245,0.272,0.291,0.302,0.304,
            0.250,0.263,0.285,0.301,0.310,0.313,
            0.270,0.280,0.297,0.312,0.319,0.322,
            0.288,0.296,0.310,0.322,0.329,0.331,
            0.305,0.311,0.322,0.332,0.339,0.341,
            0.320,0.325,0.335,0.342,0.348,0.351,
            0.335,0.339,0.346,0.353,0.359,0.360,
            0.350,0.353,0.358,0.364,0.369,0.371,
            0.364,0.365,0.370,0.375,0.379,0.381,
            0.378,0.378,0.382,0.387,0.390,0.391,
            0.388,0.391,0.394,0.398,0.401,0.402,
            0.401,0.403,0.406,0.409,0.412,0.413,
            0.414,0.415,0.418,0.421,0.423,0.424,
            0.426,0.427,0.429,0.432,0.434,0.435,
            0.439,0.439,0.441,0.444,0.446,0.446,
            0.451,0.452,0.453,0.455,0.457,0.457,
            0.463,0.464,0.465,0.467,0.468,0.469,
            0.476,0.476,0.477,0.478,0.480,0.481,
            0.488,0.488,0.489,0.490,0.491,0.491,
            0.500,0.500,0.501,0.502,0.503,0.503,
            0.512,0.513,0.513,0.514,0.515,0.515,
            0.525,0.525,0.526,0.526,0.526,0.527,
            0.537,0.537,0.538,0.538,0.538,0.539,
            0.549,0.550,0.550,0.550,0.550,0.551



        };


        /// <summary>
        /// m从0.5到1.59
        /// </summary>
        public static double[] A5_H_ρ_15_Next ={
            0.562,0.574,0.586,0.599,0.611,0.624,0.636,0.649,0.661,0.674,
            0.687,0.700,0.712,0.725,0.738,0.751,0.764,0.777,0.791,0.804,
            0.817,0.831,0.844,0.858,0.871,0.885,0.898,0.912,0.926,0.940,
            0.954,0.968,0.982,0.996,1.011,1.025,1.040,1.054,1.069,1.083,
            1.098,1.113,1.128,1.143,1.158,1.173,1.189,1.204,1.220,1.235,
            1.251,1.267,1.283,1.299,1.315,1.331,1.348,1.364,1.381,1.398,
            1.415,1.432,1.449,1.466,1.484,1.501,1.519,1.537,1.555,1.573,
            1.591,1.610,1.628,1.647,1.666,1.686,1.705,1.725,1.744,1.765,
            1.785,1.805,1.826,1.847,1.868,1.890,1.911,1.933,1.956,1.978,
            2.001,2.024,2.048,2.072,2.096,2.121,2.146,2.172,2.198,2.224,
            2.251,2.279,2.307,2.336,2.365,2.395,2.426,2.458,2.490,2.524,
            2.559,2.594,2.631,2.670,2.710,2.752,2.796,2.842,2.892,2.945
        };


        public static double getA5_H_ρ_15(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_15[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_15[x2 * A5_H_b.Length + y2] - A5_H_ρ_15[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_15[x2 * A5_H_b.Length + y2] - A5_H_ρ_15[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.69) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_15_Next[x1];
            }

        }



        public static double[] A5_H_ρ_16 ={
            -1,-1,0.210,0.244,0.259,0.264,
            -1,-1,0.224,0.253,0.266,0.273,
            -1,-1,0.235,0.260,0.274,0.281,
            -1,0.211,0.245,0.269,0.283,0.287,
            -1,0.223,0.258,0.279,0.292,0.294,
            0.222,0.243,0.270,0.289,0.301,0.303,
            0.247,0.261,0.283,0.300,0.309,0.312,
            0.268,0.278,0.296,0.310,0.318,0.321,
            0.286,0.293,0.308,0.321,0.327,0.330,
            0.303,0.308,0.320,0.330,0.337,0.339,
            0.318,0.323,0.333,0.341,0.347,0.349,
            0.333,0.337,0.344,0.351,0.357,0.358,
            0.347,0.350,0.355,0.362,0.367,0.368,
            0.361,0.363,0.367,0.373,0.377,0.379,
            0.375,0.375,0.379,0.384,0.388,0.389,
            0.386,0.388,0.391,0.395,0.398,0.399,
            0.398,0.400,0.403,0.407,0.409,0.410,
            0.411,0.412,0.415,0.418,0.420,0.421,
            0.423,0.424,0.427,0.429,0.431,0.432,
            0.435,0.436,0.438,0.441,0.442,0.443,
            0.437,0.448,0.450,0.452,0.454,0.454,
            0.460,0.460,0.462,0.463,0.465,0.466,
            0.472,0.472,0.474,0.475,0.476,0.477,
            0.484,0.484,0.485,0.487,0.488,0.488,
            0.496,0.497,0.497,0.498,0.499,0.499,
            0.508,0.509,0.509,0.510,0.511,0.511,
            0.520,0.521,0.521,0.522,0.522,0.523,
            0.533,0.533,0.533,0.534,0.534,0.534,
            0.545,0.545,0.545,0.546,0.546,0.546
        };


        /// <summary>
        /// m从0.5到1.79
        /// </summary>
        public static double[] A5_H_ρ_16_Next ={
        0.557,0.569,0.581,0.593,0.606,0.618,0.630,0.643,0.655,0.667,
        0.680,0.693,0.705,0.718,0.730,0.743,0.756,0.769,0.782,0.795,
        0.808,0.821,0.834,0.847,0.860,0.874,0.887,0.900,0.914,0.927,
        0.941,0.955,0.968,0.982,0.996,1.010,1.024,1.038,1.052,1.066,
        1.080,1.095,1.109,1.124,1.138,1.153,1.168,1.182,1.197,1.212,
        1.227,1.242,1.258,1.273,1.288,1.304,1.320,1.335,1.351,1.367,
        1.383,1.399,1.415,1.432,1.448,1.464,1.481,1.498,1.515,1.532,
        1.549,1.566,1.584,1.601,1.619,1.636,1.654,1.672,1.691,1.709,
        1.728,1.746,1.765,1.784,1.803,1.823,1.842,1.862,1.882,1.902,
        1.923,1.943,1.964,1.985,2.006,2.028,2.050,2.072,2.094,2.117,
        2.140,2.163,2.186,2.210,2.234,2.259,2.284,2.309,2.335,2.361,
        2.388,2.415,2.442,2.471,2.499,2.529,2.559,2.590,2.621,2.653,
        2.687,2.721,2.756,2.792,2.830,2.869,2.909,2.952,2.996,3.043

        };


        public static double getA5_H_ρ_16(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_16[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_16[x2 * A5_H_b.Length + y2] - A5_H_ρ_16[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_16[x2 * A5_H_b.Length + y2] - A5_H_ρ_16[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.79) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_16_Next[x1];
            }

        }

        public static double[] A5_H_ρ_17 ={
        -1,-1,0.209,0.243,0.258,0.263,
        -1,-1,0.223,0.252,0.266,0.272,
        -1,-1,0.234,0.259,0.273,0.28,
        -1,0.21,0.244,0.268,0.282,0.286,
        -1,0.222,0.256,0.278,0.291,0.293,
        0.220,0.242,0.269,0.288,0.300,0.302,
        0.245,0.260,0.282,0.298,0.308,0.310,
        0.266,0.276,0.294,0.309,0.316,0.319,
        0.284,0.292,0.306,0.320,0.326,0.328,
        0.301,0.306,0.319,0.329,0.335,0.338,
        0.316,0.321,0.331,0.339,0.345,0.347,
        0.331,0.335,0.343,0.349,0.355,0.357,
        0.345,0.348,0.353,0.360,0.365,0.367,
        0.359,0.361,0.365,0.371,0.275,0.377,
        0.373,0.373,0.377,0.382,0.386,0.387,
        0.384,0.385,0.389,0.393,0.396,0.397,
        0.396,0.398,0.401,0.404,0.407,0.408,
        0.408,0.409,0.413,0.416,0.418,0.419,
        0.420,0.421,0.424,0.427,0.429,0.429,
        0.433,0.433,0.435,0.438,0.440,0.440,
        0.445,0.445,0.447,0.449,0.451,0.452,
        0.457,0.457,0.459,0.460,0.462,0.463,
        0.469,0.469,0.471,0.472,0.473,0.474,
        0.481,0.481,0.482,0.484,0.485,0.485,
        0.493,0.493,0.494,0.495,0.496,0.496,
        0.505,0.505,0.506,0.507,0.507,0.507,
        0.517,0.517,0.518,0.518,0.519,0.519,
        0.529,0.529,0.530,0.530,0.530,0.531,
        0.541,0.541,0.541,0.542,0.542,0.542

        };


        /// <summary>
        /// m从0.5到1.89
        /// </summary>
        public static double[] A5_H_ρ_17_Next ={
        0.553,0.565,0.577,0.589,0.601,0.613,0.625,0.637,0.65,0.662,
        0.674,0.687,0.699,0.711,0.724,0.736,0.749,0.762,0.774,0.787,
        0.800,0.812,0.825,0.838,0.851,0.864,0.877,0.890,0.903,0.917,
        0.930,0.943,0.957,0.970,0.983,0.997,1.011,1.024,1.038,1.052,
        1.066,1.080,1.094,1.108,1.122,1.136,1.150,1.164,1.179,1.193,
        1.208,1.222,1.237,1.252,1.267,1.282,1.297,1.312,1.327,1.342,
        1.357,1.373,1.388,1.404,1.419,1.435,1.451,1.467,1.483,1.499,
        1.515,1.532,1.548,1.565,1.581,1.598,1.615,1.632,1.649,1.666,
        1.683,1.701,1.718,1.736,1.754,1.772,1.790,1.808,1.827,1.845,
        1.864,1.883,1.902,1.921,1.941,1.960,1.980,2.000,2.020,2.040,
        2.061,2.081,2.102,2.123,2.145,2.166,2.188,2.210,2.232,2.255,
        2.278,2.301,2.324,2.348,2.372,2.397,2.421,2.447,2.472,2.498,
        2.524,2.551,2.578,2.606,2.634,2.663,2.692,2.722,2.753,2.784,
        2.816,2.849,2.883,2.918,2.953,2.990,3.028,3.068,3.109,3.151
        };


        public static double getA5_H_ρ_17(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_17[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_17[x2 * A5_H_b.Length + y2] - A5_H_ρ_17[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_17[x2 * A5_H_b.Length + y2] - A5_H_ρ_17[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.89) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_17_Next[x1];
            }

        }



        public static double[] A5_H_ρ_18 ={
        -1,-1,0.208,0.242,0.258,0.262,
        -1,-1,0.222,0.251,0.265,0.271,
        -1,-1,0.234,0.258,0.272,0.279,
        -1,0.210,0.243,0.267,0.281,0.285,
        -1,0.221,0.255,0.277,0.290,0.292,
        0.218,0.240,0.268,0.287,0.299,0.300,
        0.244,0.258,0.280,0.297,0.307,0.309,
        0.264,0.275,0.293,0.308,0.315,0.318,
        0.282,0.290,0.305,0.318,0.324,0.327,
        0.299,0.305,0.317,0.327,0.334,0.336,
        0.314,0.319,0.329,0.337,0.344,0.346,
        0.329,0.333,0.341,0.348,0.353,0.355,
        0.343,0.346,0.352,0.359,0.363,0.365,
        0.357,0.360,0.364,0.369,0.374,0.375,
        0.370,0.371,0.375,0.380,0.384,0.385,
        0.383,0.383,0.387,0.391,0.394,0.395,
        0.394,0.396,0.399,0.402,0.405,0.406,
        0.406,0.407,0.411,0.413,0.416,0.416,
        0.418,0.419,0.422,0.425,0.427,0.427,
        0.430,0.431,0.433,0.436,0.438,0.438,
        0.442,0.443,0.445,0.447,0.449,0.449,
        0.454,0.455,0.456,0.458,0.460,0.460,
        0.466,0.467,0.468,0.469,0.471,0.472,
        0.478,0.478,0.480,0.481,0.482,0.483,
        0.490,0.490,0.491,0.492,0.493,0.493,
        0.502,0.502,0.503,0.504,0.504,0.505,
        0.514,0.514,0.515,0.515,0.516,0.516,
        0.526,0.526,0.526,0.527,0.527,0.527,
        0.538,0.538,0.538,0.538,0.539,0.539


        };


        /// <summary>
        /// m从0.5到1.89
        /// </summary>
        public static double[] A5_H_ρ_18_Next ={
        0.549,0.561,0.573,0.585,0.597,0.609,0.621,0.633,0.645,0.657,
        0.669,0.681,0.694,0.706,0.718,0.730,0.743,0.755,0.768,0.780,
        0.793,0.805,0.818,0.830,0.843,0.856,0.869,0.882,0.895,0.907,
        0.920,0.933,0.947,0.960,0.973,0.986,0.999,1.013,1.026,1.040,
        1.053,1.067,1.080,1.094,1.108,1.122,1.135,1.149,1.163,1.177,
        1.191,1.206,1.220,1.234,1.248,1.263,1.277,1.292,1.307,1.321,
        1.336,1.351,1.366,1.381,1.396,1.411,1.426,1.441,1.457,1.472,
        1.488,1.503,1.519,1.535,1.551,1.567,1.583,1.599,1.615,1.632,
        1.648,1.665,1.681,1.698,1.715,1.732,1.749,1.766,1.783,1.801,
        1.818,1.836,1.854,1.872,1.890,1.908,1.926,1.945,1.963,1.982,
        2.001,2.020,2.039,2.058,2.078,2.098,2.117,2.137,2.158,2.178,
        2.199,2.219,2.240,2.261,2.283,2.304,2.326,2.348,2.371,2.393,
        2.416,2.439,2.463,2.486,2.510,2.535,2.559,2.584,2.609,2.635,
        2.661,2.688,2.714,2.742,2.770,2.798,2.827,2.856,2.886,2.916,
        2.948,2.979,3.012,3.045,3.080,3.115,3.151,3.188,3.227,3.267

        };


        public static double getA5_H_ρ_18(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_18[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_18[x2 * A5_H_b.Length + y2] - A5_H_ρ_18[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_18[x2 * A5_H_b.Length + y2] - A5_H_ρ_18[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_18_Next[x1];
            }

        }


        public static double[] A5_H_ρ_19 ={
        -1,-1,0.207,0.241,0.257,0.262,
        -1,-1,0.221,0.251,0.264,0.27,
        -1,-1,0.233,0.258,0.271,0.278,
        -1,0.208,0.242,0.266,0.28,0.285,
        -1,0.22,0.254,0.276,0.289,0.291,
        0.217,0.239,0.367,0.286,0.298,0.299,
        0.242,0.257,0.279,0.296,0.306,0.308,
        0.263,0.273,0.291,0.307,0.314,0.317,
        0.281,0.289,0.304,0.317,0.323,0.326,
        0.297,0.303,0.316,0.326,0.333,0.335,
        0.313,0.318,0.328,0.336,0.342,0.344,
        0.327,0.331,0.340,0.347,0.352,0.354,
        0.341,0.345,0.350,0.357,0.362,0.364,
        0.355,0.358,0.362,0.368,0.372,0.374,
        0.369,0.369,0.374,0.379,0.382,0.384,
        0.381,0.382,0.385,0.390,0.393,0.394,
        0.392,0.394,0.397,0.400,0.403,0.404,
        0.404,0.406,0.409,0.412,0.414,0.415,
        0.416,0.417,0.420,0.423,0.425,0.425,
        0.428,0.429,0.431,0.434,0.436,0.436,
        0.440,0.441,0.442,0.445,0.447,0.447,
        0.452,0.453,0.454,0.456,0.458,0.458,
        0.464,0.464,0.466,0.467,0.468,0.469,
        0.476,0.476,0.477,0.478,0.479,0.480,
        0.487,0.488,0.489,0.490,0.491,0.491,
        0.499,0.500,0.500,0.501,0.502,0.502,
        0.511,0.511,0.512,0.513,0.513,0.513,
        0.523,0.523,0.524,0.524,0.524,0.525,
        0.535,0.535,0.535,0.535,0.536,0.536
        };


        /// <summary>
        /// m从0.5到1.89
        /// </summary>
        public static double[] A5_H_ρ_19_Next ={
            0.546,0.558,0.570,0.581,0.593,0.605,0.617,0.629,0.641,0.653,
            0.665,0.677,0.689,0.701,0.713,0.725,0.737,0.750,0.762,0.774,
            0.787,0.799,0.811,0.824,0.836,0.849,0.862,0.874,0.887,0.900,
            0.912,0.925,0.938,0.951,0.964,0.977,0.990,1.003,1.016,1.029,
            1.042,1.056,1.069,1.082,1.096,1.109,1.123,1.136,1.150,1.164,
            1.178,1.191,1.205,1.219,1.233,1.247,1.261,1.275,1.290,1.304,
            1.318,1.333,1.347,1.361,1.376,1.391,1.405,1.420,1.435,1.450,
            1.465,1.480,1.495,1.510,1.526,1.541,1.556,1.572,1.588,1.603,
            1.619,1.635,1.651,1.667,1.683,1.699,1.715,1.732,1.748,1.765,
            1.781,1.798,1.815,1.832,1.849,1.866,1.883,1.901,1.918,1.936,
            1.954,1.971,1.989,2.007,2.026,2.044,2.062,2.081,2.100,2.119,
            2.138,2.157,2.176,2.196,2.215,2.235,2.255,2.275,2.295,2.316,
            2.337,2.357,2.378,2.400,2.421,2.443,2.465,2.487,2.509,2.532,
            2.554,2.577,2.601,2.624,2.648,2.672,2.697,2.722,2.747,2.772,
            2.798,2.824,2.851,2.878,2.905,2.933,2.961,2.990,3.019,3.049
        };


        public static double getA5_H_ρ_19(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_19[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_19[x2 * A5_H_b.Length + y2] - A5_H_ρ_19[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_19[x2 * A5_H_b.Length + y2] - A5_H_ρ_19[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_19_Next[x1];
            }

        }


        public static double[] A5_H_ρ_20 ={
        -1,-1,0.206,0.24,0.256,0.261,
        -1,-1,0.220,0.25,0.264,0.269,
        -1,-1,0.233,0.257,0.27,0.278,
        -1,0.206,0.241,0.265,0.279,0.284,
        -1,0.219,0.253,0.275,0.288,0.290,
        0.215,0.238,0.266,0.285,0.297,0.299,
        0.241,0.256,0.278,0.295,0.305,0.307,
        0.262,0.272,0.290,0.305,0.313,0.316,
        0.280,0.288,0.303,0.316,0.322,0.325,
        0.296,0.302,0.315,0.325,0.332,0.334,
        0.311,0.316,0.327,0.335,0.341,0.343,
        0.326,0.330,0.339,0.345,0.351,0.353,
        0.340,0.343,0.349,0.356,0.361,0.362,
        0.354,0.356,0.361,0.366,0.371,0.372,
        0.367,0.368,0.372,0.377,0.381,0.382,
        0.380,0.380,0.384,0.388,0.391,0.392,
        0.390,0.392,0.395,0.399,0.402,0.403,
        0.402,0.404,0.407,0.410,0.412,0.413,
        0.414,0.415,0.419,0.421,0.423,0.424,
        0.426,0.427,0.429,0.432,0.434,0.434,
        0.438,0.439,0.441,0.443,0.445,0.445,
        0.450,0.451,0.452,0.454,0.456,0.456,
        0.462,0.462,0.463,0.465,0.466,0.467,
        0.473,0.474,0.475,0.476,0.477,0.478,
        0.485,0.486,0.486,0.488,0.489,0.489,
        0.497,0.497,0.498,0.499,0.499,0.500,
        0.509,0.509,0.509,0.510,0.511,0.511,
        0.520,0.520,0.521,0.521,0.522,0.522,
        0.532,0.532,0.533,0.533,0.533,0.533
        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_20_Next ={
                0.544,0.555,0.567,0.548,0.590,0.602,0.614,0.625,0.637,0.649,
                0.661,0.673,0.685,0.697,0.709,0.721,0.733,0.745,0.757,0.769,
                0.781,0.794,0.806,0.818,0.830,0.843,0.855,0.868,0.880,0.893,
                0.905,0.918,0.913,0.943,0.956,0.969,0.982,0.994,1.007,1.020,
                1.033,1.046,1.059,1.072,1.086,1.099,1.112,1.125,1.139,1.152,
                1.166,1.179,1.193,1.206,1.220,1.234,1.247,1.261,1.275,1.289,
                1.303,1.317,1.331,1.345,1.359,1.374,1.388,1.402,1.417,1.431,
                1.446,1.460,1.475,1.490,1.504,1.519,1.534,1.549,1.564,1.579,
                1.595,1.610,1.625,1.641,1.656,1.672,1.687,1.703,1.719,1.735,
                1.751,1.767,1.783,1.799,1.815,1.832,1.848,1.865,1.881,1.898,
                1.915,1.932,1.949,1.966,1.983,2.001,2.018,2.036,2.053,2.071,
                2.089,2.107,2.125,2.143,2.162,2.180,2.199,2.218,2.236,2.255,
                2.275,2.294,2.313,2.333,2.353,2.373,2.393,2.413,2.433,2.454,
                2.475,2.495,2.517,2.538,2.559,2.581,2.603,2.625,2.647,2.670,
                2.693,2.716,2.739,2.763,2.786,2.810,2.835,2.859,2.884,2.910

        };


        public static double getA5_H_ρ_20(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_20[x1 * A5_H_ρ_20.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_20[x2 * A5_H_b.Length + y2] - A5_H_ρ_20[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_20[x2 * A5_H_b.Length + y2] - A5_H_ρ_20[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_20_Next[x1];
            }

        }



        public static double[] A5_H_ρ_21 ={
        -1,-1,0.206,0.239,0.255,0.26,
        -1,-1,0.22,0.249,0.263,0.268,
        -1,-1,0.232,0.257,0.27,0.277,
        -1,0.205,0.24,0.265,0.278,0.284,
        -1,0.219,0.253,0.274,0.287,0.29,
        0.214,0.237,0.265,0.284,0.296,0.298,
        0.240,0.255,0.277,0.294,0.304,0.306,
        0.261,0.271,0.289,0.305,0.312,0.315,
        0.279,0.286,0.302,0.315,0.321,0.324,
        0.295,0.301,0.314,0.325,0.331,0.333,
        0.310,0.315,0.326,0.334,0.340,0.342,
        0.325,0.329,0.338,0.344,0.350,0.352,
        0.339,0.342,0.348,0.355,0.360,0.361,
        0.352,0.355,0.359,0.365,0.370,0.371,
        0.366,0.367,0.371,0.376,0.380,0.381,
        0.379,0.379,0.382,0.387,0.390,0.391,
        0.389,0.391,0.394,0.398,0.400,0.401,
        0.401,0.403,0.405,0.408,0.411,0.412,
        0.413,0.414,0.417,0.419,0.421,0.422,
        0.425,0.425,0.428,0.431,0.432,0.433,
        0.436,0.437,0.439,0.442,0.443,0.444,
        0.448,0.449,0.450,0.452,0.454,0.454,
        0.460,0.460,0.462,0.463,0.465,0.465,
        0.472,0.472,0.473,0.474,0.475,0.476,
        0.483,0.484,0.484,0.486,0.487,0.487,
        0.495,0.495,0.496,0.497,0.497,0.498,
        0.506,0.507,0.507,0.508,0.509,0.509,
        0.518,0.518,0.519,0.519,0.520,0.520,
        0.530,0.530,0.530,0.531,0.531,0.531
        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_21_Next ={
                0.541,0.553,0.564,0.576,0.587,0.599,0.611,0.622,0.634,0.646,
                0.658,0.669,0.681,0.693,0.705,0.717,0.729,0.741,0.753,0.765,
                0.777,0.789,0.801,0.813,0.825,0.837,0.850,0.862,0.874,0.887,
                0.899,0.911,0.924,0.936,0.949,0.962,0.974,0.987,1.000,1.012,
                1.025,1.038,1.051,1.064,1.077,1.090,1.108,1.116,1.129,1.142,
                1.155,1.168,1.182,1.195,1.208,1.222,1.235,1.249,1.262,1.276,
                1.290,1.303,1.317,1.331,1.345,1.359,1.373,1.387,1.401,1.415,
                1.429,1.443,1.457,1.472,1.486,1.501,1.515,1.530,1.544,1.559,
                1.574,1.589,1.603,1.618,1.633,1.648,1.664,1.679,1.694,1.709,
                1.725,1.740,1.756,1.771,1.787,1.803,1.819,1.835,1.851,1.867,
                1.883,1.899,1.915,1.932,1.948,1.865,1.981,1.998,2.015,2.032,
                2.049,2.066,2.083,2.101,2.118,2.136,2.153,2.171,2.189,2.207,
                2.225,2.243,2.261,2.279,2.298,2.317,2.335,2.354,2.374,2.392,
                2.412,2.431,2.451,2.470,2.490,2.510,2.530,2.551,2.571,2.592,
                2.613,2.634,2.655,2.676,2.698,2.719,2.741,2.763,2.786,2.808
        };


        public static double getA5_H_ρ_21(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_21[x1 * A5_H_ρ_21.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_21[x2 * A5_H_b.Length + y2] - A5_H_ρ_21[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_21[x2 * A5_H_b.Length + y2] - A5_H_ρ_21[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_21_Next[x1];
            }

        }



        public static double[] A5_H_ρ_22 ={
        -1,-1,0.205,0.239,0.255,0.26,
        -1,-1,0.219,0.249,0.263,0.268,
        -1,-1,0.232,0.256,0.269,0.276,
        -1,0.204,0.240,0.264,0.278,0.283,
        -1,0.218,0.252,0.274,0.286,0.289,
        0.213,0.236,0.264,0.284,0.295,0.297,
        0.239,0.254,0.276,0.294,0.304,0.306,
        0.260,0.270,0.289,0.304,0.311,0.314,
        0.277,0.285,0.301,0.314,0.320,0.323,
        0.294,0.300,0.313,0.324,0.330,0.332,
        0.309,0.314,0.325,0.333,0.339,0.341,
        0.323,0.328,0.337,0.343,0.349,0.351,
        0.337,0.341,0.347,0.354,0.359,0.360,
        0.351,0.354,0.358,0.364,0.368,0.370,
        0.364,0.366,0.370,0.375,0.378,0.380,
        0.377,0.377,0.381,0.385,0.389,0.390,
        0.388,0.389,0.393,0.396,0.399,0.400,
        0.399,0.401,0.404,0.407,0.409,0.410,
        0.411,0.412,0.416,0.418,0.420,0.421,
        0.423,0.424,0.426,0.429,0.431,0.431,
        0.435,0.436,0.437,0.440,0.441,0.442,
        0.447,0.447,0.449,0.450,0.452,0.453,
        0.458,0.459,0.460,0.462,0.463,0.464,
        0.470,0.470,0.471,0.473,0.474,0.475,
        0.481,0.482,0.483,0.484,0.485,0.485,
        0.493,0.493,0.494,0.495,0.496,0.496,
        0.504,0.505,0.505,0.506,0.507,0.507,
        0.516,0.516,0.517,0.517,0.518,0.518,
        0.527,0.520,0.528,0.529,0.529,0.529

        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_22_Next ={
            0.539,0.551,0.562,0.574,0.585,0.596,0.608,0.620,0.631,0.643,
            0.654,0.666,0.678,0.690,0.701,0.713,0.725,0.737,0.749,0.761,
            0.772,0.784,0.796,0.808,0.820,0.833,0.845,0.857,0.869,0.881,
            0.894,0.906,0.918,0.930,0.943,0.955,0.968,0.980,0.993,1.005,
            1.018,1.031,1.043,1.056,1.069,1.081,1.094,1.107,1.120,1.133,
            1.146,1.159,1.172,1.185,1.198,1.211,1.225,1.238,1.251,1.265,
            1.278,1.291,1.305,1.318,1.332,1.346,1.359,1.373,1.387,1.401,
            1.414,1.428,1.442,1.456,1.470,1.485,1.499,1.513,1.527,1.541,
            1.556,1.570,1.585,1.599,1.614,1.629,1.643,1.658,1.673,1.688,
            1.703,1.718,1.733,1.748,1.763,1.778,1.794,1.809,1.824,1.840,
            1.856,1.871,1.887,1.903,1.919,1.935,1.951,1.967,1.983,1.999,
            2.015,2.032,2.048,2.065,2.082,2.098,2.115,2.132,2.149,2.166,
            2.183,2.201,2.218,2.235,2.253,2.271,2.288,2.306,2.324,2.342,
            2.361,2.379,2.397,2.416,2.434,2.453,2.472,2.491,2.510,2.529,
            2.549,2.568,2.588,2.608,2.628,2.648,2.668,2.688,2.709,2.730

        };


        public static double getA5_H_ρ_22(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_22[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_22[x2 * A5_H_b.Length + y2] - A5_H_ρ_22[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_22[x2 * A5_H_b.Length + y2] - A5_H_ρ_22[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_22_Next[x1];
            }

        }


        public static double[] A5_H_ρ_23 ={
        -1,-1,0.205,0.238,0.254,0.259,
        -1,-1,0.218,0.248,0.262,0.267,
        -1,-1,0.231,0.256,0.269,0.276,
        -1,0.203,0.239,0.264,0.277,0.283,
        -1,0.218,0.251,0.273,0.286,0.289,
        0.212,0.235,0.264,0.283,0.295,0.297,
        0.238,0.253,0.276,0.293,0.303,0.305,
        0.259,0.269,0.288,0.303,0.311,0.314,
        0.276,0.285,0.300,0.313,0.320,0.322,
        0.293,0.299,0.312,0.323,0.329,0.331,
        0.308,0.313,0.324,0.332,0.338,0.340,
        0.322,0.327,0.336,0.342,0.348,0.350,
        0.336,0.340,0.346,0.353,0.358,0.359,
        0.350,0.353,0.357,0.363,0.367,0.369,
        0.363,0.365,0.369,0.374,0.377,0.379,
        0.376,0.376,0.380,0.384,0.388,0.389,
        0.387,0.388,0.391,0.395,0.398,0.399,
        0.398,0.400,0.403,0.406,0.408,0.409,
        0.410,0.411,0.414,0.417,0.419,0.420,
        0.422,0.423,0.425,0.428,0.429,0.430,
        0.433,0.434,0.436,0.439,0.440,0.441,
        0.445,0.446,0.447,0.449,0.451,0.451,
        0.457,0.457,0.458,0.460,0.462,0.462,
        0.468,0.469,0.470,0.471,0.472,0.473,
        0.480,0.480,0.481,0.482,0.483,0.484,
        0.491,0.492,0.492,0.493,0.494,0.494,
        0.503,0.503,0.504,0.505,0.505,0.505,
        0.514,0.514,0.515,0.516,0.516,0.516,
        0.526,0.526,0.526,0.527,0.527,0.527
        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_23_Next ={
            0.537,0.548,0.560,0.571,0.583,0.594,0.606,0.617,0.629,0.640,
            0.652,0.663,0.675,0.687,0.698,0.710,0.722,0.733,0.745,0.757,
            0.769,0.718,0.792,0.804,0.816,0.828,0.840,0.852,0.864,0.876,
            0.889,0.901,0.913,0.925,0.937,0.950,0.962,0.974,0.987,0.999,
            1.012,1.024,1.036,1.049,1.062,1.074,1.087,1.100,1.112,1.125,
            1.138,1.515,1.164,1.176,1.189,1.202,1.215,1.228,1.241,1.255,
            1.268,1.281,1.294,1.308,1.321,1.334,1.348,1.361,1.375,1.388,
            1.402,1.415,1.429,1.443,1.457,1.470,1.484,1.498,1.512,1.526,
            1.540,1.554,1.568,1.583,1.597,1.611,1.625,1.640,1.654,1.669,
            1.683,1.698,1.713,1.727,1.742,1.757,1.772,1.787,1.802,1.817,
            1.832,1.847,1.862,1.878,1.893,1.909,1.924,1.940,1.955,1.971,
            1.987,2.003,2.019,2.034,2.051,2.067,2.083,2.099,2.115,2.132,
            2.148,2.165,2.182,2.198,2.215,2.232,2.249,2.266,2.283,2.301,
            2.318,2.335,2.353,2.370,2.388,2.406,2.424,2.442,2.460,2.478,
            2.497,2.515,2.533,2.552,2.571,2.590,2.609,2.628,2.647,2.667
        };


        public static double getA5_H_ρ_23(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_23[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_23[x2 * A5_H_b.Length + y2] - A5_H_ρ_23[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_23[x2 * A5_H_b.Length + y2] - A5_H_ρ_23[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_23_Next[x1];
            }

        }


        public static double[] A5_H_ρ_24 ={
        -1,-1,0.204,0.238,0.254,0.258,
        -1,-1,0.218,0.248,0.262,0.267,
        -1,-1,0.231,0.255,0.268,0.275,
        -1,0.202,0.239,0.263,0.276,0.282,
        -1,0.218,0.251,0.273,0.285,0.288,
        0.211,0.235,0.263,0.282,0.294,0.296,
        0.237,0.252,0.275,0.292,0.302,0.304,
        0.258,0.269,0.287,0.302,0.310,0.313,
        0.276,0.284,0.299,0.313,0.319,0.322,
        0.292,0.298,0.311,0.322,0.328,0.331,
        0.307,0.312,0.323,0.331,0.338,0.340,
        0.322,0.326,0.325,0.342,0.347,0.349,
        0.335,0.339,0.346,0.352,0.357,0.359,
        0.349,0.352,0.356,0.362,0.367,0.368,
        0.362,0.364,0.368,0.373,0.377,0.378,
        0.375,0.375,0.379,0.383,0.387,0.388,
        0.386,0.387,0.390,0.394,0.397,0.398,
        0.397,0.399,0.402,0.405,0.407,0.408,
        0.409,0.410,0.413,0.416,0.418,0.418,
        0.421,0.421,0.424,0.427,0.428,0.429,
        0.432,0.433,0.435,0.438,0.439,0.439,
        0.444,0.444,0.446,0.448,0.450,0.450,
        0.455,0.456,0.457,0.459,0.461,0.461,
        0.467,0.467,0.468,0.470,0.471,0.472,
        0.478,0.479,0.480,0.481,0.482,0.482,
        0.490,0.490,0.491,0.492,0.493,0.493,
        0.501,0.501,0.502,0.503,0.503,0.504,
        0.512,0.513,0.513,0.514,0.514,0.515,
        0.524,0.524,0.525,0.525,0.525,0.526

        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_24_Next ={
            0.535,0.547,0.558,0.570,0.581,0.592,0.603,0.615,0.626,0.638,
            0.649,0.661,0.672,0.684,0.695,0.707,0.719,0.730,0.742,0.754,
            0.765,0.777,0.789,0.801,0.813,0.824,0.836,0.848,0.860,0.872,
            0.884,0.896,0.908,0.920,0.932,0.945,0.957,0.969,0.981,0.993,
            1.006,1.018,1.030,1.043,1.055,1.068,1.080,1.093,1.105,1.118,
            1.131,1.143,1.156,1.169,1.181,1.194,1.207,1.220,1.233,1.246,
            1.259,1.272,1.285,1.298,1.311,1.324,1.337,1.351,1.364,1.377,
            1.391,1.404,1.417,1.431,1.444,1.458,1.472,1.485,1.499,1.513,
            1.526,1.540,1.554,1.568,1.582,1.596,1.610,1.624,1.638,1.652,
            1.667,1.681,1.695,1.710,1.724,1.738,1.753,1.768,1.782,1.797,
            1.812,1.826,1.841,1.856,1.871,1.886,1.901,1.916,1.931,1.947,
            1.962,1.977,1.993,2.008,2.024,2.039,2.055,2.071,2.087,2.102,
            2.118,2.134,2.150,2.167,2.183,2.199,2.215,2.232,2.248,2.265,
            2.282,2.298,2.315,2.332,2.349,2.366,2.383,2.400,2.418,2.435,
            2.453,2.470,2.488,2.506,2.524,2.541,2.560,2.578,2.596,2.614
        };


        public static double getA5_H_ρ_24(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_24[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_24[x2 * A5_H_b.Length + y2] - A5_H_ρ_24[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_24[x2 * A5_H_b.Length + y2] - A5_H_ρ_24[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_24_Next[x1];
            }

        }


        public static double[] A5_H_ρ_25 ={
       -1,-1,0.204,0.237,0.253,0.258,
        -1,-1,0.217,0.247,0.261,0.266,
        -1,-1,0.231,0.255,0.268,0.274,
        -1,0.201,0.239,0.262,0.276,0.282,
        -1,0.217,0.25,0.272,0.285,0.288,
        0.210,0.234,0.262,0.282,0.293,0.295,
        0.236,0.252,0.274,0.292,0.302,0.304,
        0.257,0.268,0.286,0.302,0.309,0.312,
        0.275,0.283,0.298,0.312,0.318,0.321,
        0.291,0.297,0.310,0.322,0.328,0.330,
        0.306,0.311,0.322,0.331,0.337,0.339,
        0.321,0.325,0.334,0.341,0.346,0.348,
        0.334,0.338,0.345,0.351,0.356,0.358,
        0.348,0.351,0.355,0.361,0.366,0.367,
        0.361,0.363,0.367,0.372,0.376,0.377,
        0.374,0.374,0.378,0.382,0.386,0.387,
        0.385,0.386,0.389,0.393,0.396,0.397,
        0.396,0.398,0.401,0.404,0.406,0.407,
        0.408,0.409,0.412,0.415,0.417,0.417,
        0.419,0.420,0.423,0.425,0.427,0.428,
        0.431,0.432,0.434,0.436,0.438,0.438,
        0.443,0.443,0.445,0.447,0.448,0.449,
        0.454,0.455,0.456,0.457,0.459,0.460,
        0.465,0.466,0.467,0.468,0.470,0.470,
        0.477,0.477,0.478,0.479,0.490,0.481,
        0.488,0.489,0.489,0.490,0.491,0.491,
        0.500,0.500,0.501,0.501,0.502,0.502,
        0.511,0.511,0.512,0.513,0.513,0.513,
        0.522,0.522,0.523,0.523,0.524,0.524
        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_25_Next ={
            0.534,0.545,0.556,0.568,0.579,0.590,0.601,0.613,0.624,0.636,
            0.647,0.658,0.670,0.681,0.693,0.704,0.716,0.727,0.739,0.751,
            0.762,0.774,0.786,0.797,0.809,0.821,0.833,0.845,0.856,0.868,
            0.880,0.892,0.904,0.916,0.928,0.940,0.952,0.964,0.976,0.988,
            1.001,1.013,1.025,1.037,1.050,1.062,1.074,1.087,1.099,1.112,
            1.124,1.137,1.149,1.162,1.174,1.187,1.200,1.212,1.225,1.238,
            1.251,1.263,1.276,1.289,1.302,1.315,1.328,1.341,1.354,1.367,
            1.380,1.394,1.407,1.420,1.433,1.447,1.460,1.474,1.487,1.501,
            1.514,1.528,1.541,1.555,1.569,1.582,1.596,1.610,1.624,1.638,
            1.652,1.666,1.680,1.694,1.708,1.722,1.736,1.750,1.765,1.779,
            1.794,1.808,1.823,1.837,1.852,1.866,1.881,1.896,1.911,1.925,
            1.940,1.955,1.970,1.985,2.000,2.016,2.031,2.046,2.062,2.077,
            2.092,2.108,2.124,2.139,2.155,2.171,2.186,2.202,2.218,2.234,
            2.250,2.267,2.283,2.299,2.316,2.332,2.348,2.365,2.382,2.398,
            2.415,2.432,2.449,2.466,2.483,2.500,2.518,2.535,2.553,2.570

        };


        public static double getA5_H_ρ_25(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_25[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_25[x2 * A5_H_b.Length + y2] - A5_H_ρ_25[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_25[x2 * A5_H_b.Length + y2] - A5_H_ρ_25[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_25_Next[x1];
            }

        }



        public static double[] A5_H_ρ_26 ={
        -1,-1,0.203,0.237,0.253,0.258,
        -1,-1,0.217,0.247,0.261,0.266,
        -1,-1,0.23,0.255,0.268,0.274,
        -1,0.200,0.238,0.262,0.276,0.282,
        -1,0.217,0.250,0.272,0.284,0.288,
        0.209,0.233,0.262,0.281,0.293,0.295,
        0.235,0.251,0.274,0.291,0.302,0.303,
        0.256,0.267,0.286,0.301,0.309,0.312,
        0.274,0.282,0.298,0.311,0.318,0.320,
        0.290,0.297,0.310,0.321,0.327,0.329,
        0.305,0.311,0.321,0.330,0.336,0.338,
        0.320,0.324,0.333,0.340,0.346,0.348,
        0.334,0.337,0.344,0.350,0.355,0.357,
        0.347,0.350,0.355,0.361,0.365,0.367,
        0.360,0.362,0.366,0.371,0.375,0.376,
        0.373,0.373,0.377,0.382,0.385,0.386,
        0.384,0.385,0.388,0.392,0.395,0.396,
        0.395,0.397,0.400,0.403,0.405,0.406,
        0.407,0.408,0.411,0.414,0.416,0.416,
        0.418,0.419,0.422,0.424,0.426,0.427,
        0.430,0.431,0.433,0.435,0.437,0.437,
        0.441,0.442,0.444,0.446,0.447,0.448,
        0.453,0.453,0.455,0.456,0.458,0.459,
        0.464,0.465,0.466,0.467,0.468,0.469,
        0.476,0.476,0.477,0.478,0.479,0.480,
        0.487,0.487,0.488,0.489,0.490,0.490,
        0.498,0.499,0.499,0.500,0.501,0.501,
        0.510,0.510,0.510,0.511,0.512,0.512,
        0.521,0.521,0.522,0.522,0.522,0.523
        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_26_Next ={
            0.532,0.543,0.555,0.566,0.577,0.588,0.600,0.611,0.622,0.634,
            0.645,0.656,0.668,0.679,0.691,0.702,0.713,0.725,0.736,0.748,
            0.760,0.771,0.783,0.794,0.806,0.818,0.829,0.841,0.853,0.865,
            0.876,0.888,0.900,0.912,0.924,0.936,0.948,0.960,0.972,0.984,
            0.996,1.008,1.020,1.032,1.044,1.057,1.069,1.081,1.093,1.106,
            1.118,1.131,1.143,1.155,1.168,1.180,1.193,1.205,1.218,1.231,
            1.243,1.256,1.269,1.281,1.294,1.307,1.320,1.333,1.346,1.358,
            1.371,1.384,1.397,1.411,1.424,1.437,1.450,1.463,1.476,1.490,
            1.503,1.516,1.530,1.543,1.557,1.570,1.584,1.597,1.611,1.625,
            1.638,1.652,1.666,1.680,1.694,1.707,1.721,1.735,1.749,1.763,
            1.778,1.792,1.806,1.820,1.834,1.849,1.863,1.878,1.892,1.907,
            1.921,1.936,1.950,1.965,1.980,1.995,2.010,2.025,2.039,2.054,
            2.070,2.085,2.100,2.115,2.130,2.146,2.161,2.177,2.192,2.208,
            2.223,2.239,2.255,2.270,2.286,2.302,2.318,2.334,2.350,2.367,
            2.383,2.399,2.416,2.432,2.449,2.465,2.482,2.498,2.515,2.532
        };


        public static double getA5_H_ρ_26(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_26[x1 * A5_H_ρ_26.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_26[x2 * A5_H_b.Length + y2] - A5_H_ρ_26[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_26[x2 * A5_H_b.Length + y2] - A5_H_ρ_26[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_26_Next[x1];
            }

        }



        public static double[] A5_H_ρ_27 ={
            -1,-1,0.203,0.237,0.252,0.257,
            -1,-1,0.216,0.246,0.261,0.265,
            -1,-1,0.23,0.254,0.267,0.274,
            -1,0.200,0.238,0.262,0.275,0.281,
            -1,0.217,0.249,0.271,0.284,0.287,
            0.208,0.233,0.261,0.281,0.292,0.294,
            0.235,0.250,0.273,0.291,0.301,0.303,
            0.255,0.267,0.285,0.301,0.309,0.311,
            0.273,0.282,0.297,0.311,0.317,0.320,
            0.290,0.296,0.309,0.321,0.326,0.329,
            0.305,0.310,0.321,0.329,0.336,0.338,
            0.319,0.323,0.333,0.339,0.345,0.347,
            0.333,0.336,0.344,0.350,0.355,0.356,
            0.346,0.349,0.354,0.360,0.364,0.366,
            0.359,0.362,0.365,0.370,0.374,0.376,
            0.372,0.372,0.376,0.381,0.384,0.385,
            0.384,0.384,0.388,0.391,0.394,0.395,
            0.394,0.396,0.399,0.402,0.405,0.405,
            0.406,0.407,0.410,0.413,0.415,0.416,
            0.417,0.418,0.421,0.424,0.425,0.426,
            0.429,0.430,0.432,0.434,0.436,0.436,
            0.440,0.441,0.443,0.445,0.446,0.447,
            0.452,0.452,0.454,0.455,0.457,0.468,
            0.463,0.464,0.465,0.466,0.467,0.468,
            0.475,0.475,0.476,0.477,0.478,0.479,
            0.486,0.486,0.487,0.488,0.489,0.489,
            0.497,0.497,0.498,0.499,0.499,0.500,
            0.508,0.509,0.509,0.510,0.510,0.510,
            0.520,0.520,0.520,0.521,0.521,0.521

        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_27_Next ={
        0.531,0.542,0.553,0.565,0.576,0.587,0.598,0.609,0.620,0.632,
        0.643,0.654,0.666,0.677,0.688,0.700,0.711,0.723,0.734,0.746,
        0.757,0.769,0.780,0.792,0.803,0.815,0.826,0.838,0.850,0.861,
        0.873,0.885,0.897,0.908,0.920,0.932,0.944,0.956,0.968,0.980,
        0.992,1.004,1.016,1.028,1.040,1.052,1.064,1.076,1.088,1.101,
        1.113,1.125,1.137,1.150,1.162,1.174,1.187,1.199,1.212,1.224,
        1.237,1.249,1.262,1.274,1.287,1.300,1.312,1.325,1.338,1.350,
        1.363,1.376,1.389,1.402,1.415,1.428,1.441,1.454,1.467,1.480,
        1.493,1.506,1.520,1.533,1.546,1.559,1.573,1.586,1.599,1.613,
        1.626,1.640,1.653,1.667,1.681,1.694,1.708,1.722,1.736,1.749,
        1.763,1.777,1.791,1.805,1.819,1.833,1.847,1.861,1.876,1.890,
        1.904,1.918,1.933,1.947,1.692,1.976,1.991,2.005,2.020,2.035,
        2.049,2.064,2.079,2.094,2.109,2.124,2.139,2.154,2.169,2.184,
        2.199,2.215,2.230,2.245,2.261,2.276,2.292,2.307,2.323,2.339,
        2.354,2.370,2.386,2.402,2.418,2.434,2.450,2.467,2.483,2.499

        };


        public static double getA5_H_ρ_27(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_27[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_27[x2 * A5_H_b.Length + y2] - A5_H_ρ_27[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_27[x2 * A5_H_b.Length + y2] - A5_H_ρ_27[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_27_Next[x1];
            }
        }

        public static double[] A5_H_ρ_28 ={
        -1,-1,0.203,0.236,0.252,0.257,
        -1,-1,0.216,0.246,0.260,0.265,
        -1,-1,0.230,0.254,0.267,0.273,
        -1,0.200,0.238,0.261,0.275,0.281,
        -1,0.216,0.249,0.271,0.283,0.287,
        0.207,0.232,0.261,0.280,0.292,0.294,
        0.234,0.250,0.273,0.290,0.301,0.302,
        0.255,0.266,0.285,0.300,0.308,0.311,
        0.273,0.281,0.297,0.310,0.317,0.319,
        0.289,0.295,0.308,0.320,0.326,0.328,
        0.304,0.309,0.320,0.329,0.335,0.337,
        0.318,0.323,0.332,0.339,0.345,0.347,
        0.332,0.336,0.343,0.349,0.354,0.356,
        0.346,0.348,0.353,0.359,0.364,0.365,
        0.359,0.361,0.364,0.370,0.374,0.375,
        0.371,0.372,0.376,0.380,0.384,0.385,
        0.383,0.384,0.387,0.391,0.394,0.395,
        0.393,0.395,0.398,0.401,0.404,0.405,
        0.405,0.406,0.409,0.412,0.414,0.415,
        0.417,0.417,0.420,0.423,0.424,0.425,
        0.428,0.429,0.431,0.433,0.435,0.435,
        0.440,0.440,0.442,0.444,0.446,0.446,
        0.451,0.451,0.453,0.454,0.456,0.457,
        0.462,0.463,0.464,0.465,0.467,0.467,
        0.473,0.474,0.475,0.476,0.477,0.478,
        0.485,0.485,0.486,0.487,0.488,0.488,
        0.496,0.496,0.497,0.498,0.498,0.499,
        0.507,0.507,0.508,0.509,0.509,0.509,
        0.518,0.519,0.519,0.520,0.520,0.520


        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_28_Next ={
        0.530,0.541,0.552,0.563,0.574,0.585,0.596,0.608,0.619,0.630,
        0.641,0.653,0.664,0.675,0.686,0.698,0.709,0.720,0.732,0.743,
        0.755,0.766,0.778,0.789,0.801,0.812,0.824,0.835,0.847,0.858,
        0.870,0.882,0.893,0.905,0.917,0.929,0.940,0.952,0.964,0.976,
        0.988,1.000,1.012,1.024,1.036,1.048,1.060,1.072,1.084,1.096,
        1.108,1.120,1.132,1.144,1.157,1.169,1.181,1.193,1.206,1.218,
        1.231,1.243,1.255,1.268,1.280,1.293,1.305,1.318,1.331,1.343,
        1.356,1.369,1.381,1.394,1.407,1.420,1.433,1.445,1.458,1.471,
        1.484,1.497,1.510,1.523,1.536,1.550,1.563,1.576,1.589,1.602,
        1.616,1.629,1.642,1.656,1.669,1.683,1.696,1.710,1.723,1.737,
        1.750,1.764,1.778,1.792,1.805,1.819,1.833,1.847,1.861,1.875,
        1.889,1.903,1.917,1.931,1.945,1.960,1.974,1.988,2.002,2.017,
        2.031,2.046,2.060,2.075,2.089,2.104,2.119,2.133,2.148,2.163,
        2.178,2.193,2.208,2.223,2.238,2.253,2.268,2.283,2.299,2.314,
        2.329,2.345,2.360,2.376,2.391,2.407,2.423,2.439,2.454,2.470
        };


        public static double getA5_H_ρ_28(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_28[x1 * A5_H_ρ_28.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_28[x2 * A5_H_b.Length + y2] - A5_H_ρ_28[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_28[x2 * A5_H_b.Length + y2] - A5_H_ρ_28[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_28_Next[x1];
            }
        }




        public static double[] A5_H_ρ_29 ={
       -1,-1,0.202,0.236,0.252,0.257,
        -1,-1,0.216,0.246,0.26,0.265,
        -1,-1,0.229,0.254,0.267,0.273,
        -1,0.200,0.238,0.261,0.274,0.281,
        -1,0.216,0.248,0.270,0.283,0.287,
        0.207,0.232,0.260,0.280,0.292,0.294,
        0.233,0.249,0.272,0.290,0.300,0.302,
        0.254,0.265,0.284,0.300,0.308,0.310,
        0.272,0.281,0.296,0.310,0.316,0.319,
        0.288,0.295,0.308,0.320,0.325,0.328,
        0.304,0.309,0.320,0.329,0.335,0.337,
        0.318,0.322,0.331,0.338,0.344,0.346,
        0.332,0.335,0.343,0.349,0.354,0.355,
        0.345,0.348,0.353,0.359,0.363,0.365,
        0.358,0.360,0.364,0.369,0.373,0.374,
        0.371,0.371,0.375,0.380,0.383,0.384,
        0.382,0.383,0.386,0.390,0.393,0.394,
        0.392,0.395,0.397,0.401,0.403,0.404,
        0.404,0.406,0.409,0.411,0.413,0.414,
        0.416,0.417,0.420,0.422,0.424,0.424,
        0.427,0.428,0.430,0.433,0.434,0.435,
        0.439,0.439,0.441,0.443,0.445,0.445,
        0.450,0.451,0.452,0.454,0.455,0.456,
        0.461,0.462,0.463,0.464,0.466,0.466,
        0.473,0.473,0.474,0.475,0.476,0.477,
        0.484,0.484,0.485,0.486,0.487,0.487,
        0.495,0.495,0.496,0.497,0.497,0.498,
        0.506,0.506,0.507,0.508,0.508,0.508,
        0.517,0.517,0.518,0.518,0.519,0.519
        };


        /// <summary>
        /// m从0.5到1.99
        /// </summary>
        public static double[] A5_H_ρ_29_Next ={
        0.528,0.540,0.551,0.562,0.573,0.584,0.595,0.606,0.617,0.629,
        0.640,0.651,0.662,0.673,0.685,0.696,0.707,0.719,0.730,0.741,
        0.753,0.764,0.775,0.787,0.798,0.810,0.821,0.833,0.844,0.856,
        0.867,0.879,0.891,0.902,0.914,0.925,0.937,0.949,0.961,0.972,
        0.984,0.996,1.008,1.020,1.032,1.044,1.055,1.067,1.079,1.091,
        1.103,1.115,1.128,1.140,1.152,1.164,1.176,1.188,1.200,1.213,
        1.225,1.237,1.250,1.262,1.274,1.287,1.299,1.312,1.324,1.337,
        1.349,1.362,1.374,1.387,1.400,1.412,1.425,1.438,1.450,1.463,
        1.476,1.489,1.502,1.515,1.528,1.541,1.554,1.567,1.580,1.593,
        1.606,1.619,1.632,1.645,1.659,1.672,1.685,1.699,1.712,1.725,
        1.739,1.752,1.766,1.779,1.793,1.807,1.820,1.834,1.848,1.861,
        1.875,1.889,1.903,1.917,1.931,1.945,1.959,1.973,1.987,2.001,
        2.015,2.029,2.043,2.058,2.072,2.086,2.101,2.115,2.130,2.144,
        2.159,1.174,2.188,2.203,2.218,2.232,2.247,2.262,2.277,2.292,
        2.307,2.322,2.337,2.352,2.368,2.383,2.398,2.414,2.429,2.444

        };


        public static double getA5_H_ρ_29(double m, double b)
        {

            int x1;
            int y1;
            int x2;
            int y2;
            if (m < 0.5)
            {
                if (m < 0.21) return -1;
                getIndexOfArray(Math.Round(m, 2), A5_H_M_12, out x1, out x2);
                getIndexOfArray(Math.Round(b, 2), A5_H_b, out y1, out y2);
                if (x1 == x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {
                    return A5_H_ρ_29[x1 * A5_H_b.Length + y1];
                }

                //插值处理
                if (x1 != x2 && y1 == y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_29[x2 * A5_H_b.Length + y2] - A5_H_ρ_29[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                //插值处理
                if (x1 == x2 && y1 != y2 && x1 != -1 && y1 != -1)
                {

                    return Math.Round((A5_H_ρ_29[x2 * A5_H_b.Length + y2] - A5_H_ρ_29[x1 * A5_H_b.Length + y1]) / m, 8);
                }
                return -1;
            }
            else
            {
                if (m > 1.99) return -1;

                x1 = Convert.ToInt16((m - 0.5) * 100);
                return A5_H_ρ_29_Next[x1];
            }
        }


        public static double[] A6_G_ρ ={
            0.2,0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,
            0.3,0.31,0.32,0.33,0.34,0.35,0.36,0.37,0.38,0.39,
            0.4,0.41,0.42,0.43,0.44,0.45,0.46,0.47,0.48,0.49,
            0.5,0.51,0.52,0.53,0.54,0.55,0.56,0.57,0.58,0.59,
            0.6,0.61,0.62,0.63,0.64,0.65,0.66,0.67,0.68,0.69,
            0.7,0.71,0.72,0.73,0.74,0.75,0.76,0.77,0.78,0.79,
            0.8,0.81,0.82,0.83,0.84,0.85,0.86,0.87,0.88,0.89,
            0.9,0.91,0.92,0.93,0.94,0.95,0.96,0.97,0.98,0.99,
            1,1.01,1.02,1.03,1.04,1.05,1.06,1.07,1.08,1.09,
            1.1,1.11,1.12,1.13,1.14,1.15,1.16,1.17,1.18,1.19
        };
        /// <summary>
        /// b=0.0，0.1，0.2
        /// </summary>
        public static double[] A6_G_ρ_b_2 ={
            1.980,1.975,1.970,1.963,1.957,1.949,1.942,1.934,1.925,1.917,
            1.908,1.899,1.890,1.881,1.873,1.864,1.855,1.846,1.838,1.829,
            1.821,1.813,1.805,1.798,1.790,1.783,1.776,1.769,1.763,1.756,
            1.750,1.744,1.738,1.732,1.727,1.721,1.715,1.711,1.706,1.701,
            1.697,1.692,1.688,1.684,1.679,1.675,1.672,1.668,1.664,1.661,
            1.657,1.654,1.651,1.647,1.644,1.641,1.638,1.635,1.633,1.630,
            1.627,1.625,1.622,1.620,1.617,1.615,1.613,1.610,1.608,1.606,
            1.604,1.602,1.600,1.598,1.596,1.594,1.592,1.590,1.589,1.587,
            1.585,1.584,1.582,1.580,1.579,1.577,1.576,1.574,1.573,1.571,
            1.570,1.569,1.567,1.566,1.565,1.563,1.562,1.561,1.560,1.558

        };

        /// <summary>
        /// 当b=0.3，0.4，0.5
        /// </summary>
        public static double[] A6_G_ρ_b_3 ={
        2.769,2.645,2.549,2.450,2.373,2.306,2.248,2.197,2.152,2.112,
        2.076,2.044,2.015,1.989,1.966,1.944,1.924,1.906,1.890,1.874,
        1.860,1.847,1.834,1.823,1.812,1.802,1.792,1.783,1.775,1.767,
        1.759,1.752,1.745,1.738,1.732,1.726,1.720,1.714,1.709,1.704,
        1.699,1.694,1.689,1.685,1.681,1.676,1.672,1.665,1.665,1.661,
        1.658,1.654,1.651,1.648,1.645,1.641,1.638,1.636,1.633,1.630,
        1.627,1.625,1.622,1.620,1.617,1.615,1.613,1.610,1.608,1.606,
        1.604,1.602,1.600,1.598,1.896,1.594,1.592,1.590,1.589,1.587,
        1.585,1.584,1.582,1.580,1.579,1.577,1.576,1.574,1.573,1.571,
        1.570,1.569,1.567,1.566,1.565,1.563,1.562,1.561,1.560,1.558
        };

        /// <summary>
        /// 当ρ>=1.2 步长 0.1
        /// </summary>
        public static double[] A6_G_ρ_next ={
        1.557,1.546,1.537,1.529,1.522,1.516,1.51,1.505,
        1.501,1.497,1.493,1.49,1.486,1.484,1.481,1.479,1.476,1.474,
        1.472,1.47,1.469,1.467,1.465,1.464,1.463,1.461,1.46,1.459
        };


        public static double getA6_G(double ρ, double b)
        {

            double ρ1, ρ0, x0, x1;
            if (ρ == 1.19) return 1.558;
            if ((ρ > 1.19) && (ρ < 1.195)) return 1.558;
            if ((ρ >= 1.195) && (ρ < 1.2)) return 1.557;
            if (ρ < 0.2) return 0;
            if (ρ > 3.9) return 0;
            if (ρ == 1.2) return 1.557;
            if (ρ == 3.9) return 1.459;

            if (ρ < 1.2)
            {
                if (b < 0.3)
                {
                    if (Math.Round(ρ, 2) > ρ)
                    {
                        ρ1 = Math.Round(ρ, 2);
                        ρ0 = ρ1 - 0.01;
                    }
                    else
                    {
                        ρ0 = Math.Round(ρ, 2);
                        ρ1 = ρ0 + 0.01;
                    }


                    x1 = A6_G_ρ_b_2[getIndexOfArray(ρ0, A6_G_ρ, 0.0001)];
                    x0 = A6_G_ρ_b_2[getIndexOfArray(ρ1, A6_G_ρ, 0.0001)];

                    return Math.Round(x1 - 100 * (x1 - x0) * (ρ - ρ0), 3);


                }
                else if (b >= 0.3)
                {
                    if (Math.Round(ρ, 2) > ρ)
                    {
                        ρ1 = Math.Round(ρ, 2);
                        ρ0 = ρ1 - 0.01;
                    }
                    else
                    {
                        ρ0 = Math.Round(ρ, 2);
                        ρ1 = ρ0 + 0.01;
                    }


                    x1 = A6_G_ρ_b_3[getIndexOfArray(ρ0, A6_G_ρ, 0.0001)];
                    x0 = A6_G_ρ_b_3[getIndexOfArray(ρ1, A6_G_ρ, 0.0001)];

                    return Math.Round(x1 - 100 * (x1 - x0) * (ρ - ρ0), 3);
                }
            }
            else
            {
                if (Math.Round(ρ, 1) > ρ)
                {
                    ρ1 = Math.Round(ρ, 1);
                    ρ0 = ρ1 - 0.1;
                }
                else
                {
                    ρ0 = Math.Round(ρ, 1);
                    ρ1 = ρ0 + 0.1;
                }

                x1 = A6_G_ρ_next[Convert.ToInt16((ρ0 - 1.2) * 10)];
                x0 = A6_G_ρ_next[Convert.ToInt16((ρ1 - 1.2) * 10)];

                return Math.Round(x1 - 10 * (x1 - x0) * (ρ - ρ0), 3);

            }
            return 0;
        }



        public static double[] A7_H_ρ ={
            0.2,0.21,0.22,0.23,0.24,0.25,0.26,0.27,0.28,0.29,
            0.3,0.31,0.32,0.33,0.34,0.35,0.36,0.37,0.38,0.39,
            0.4,0.41,0.42,0.43,0.44,0.45,0.46,0.47,0.48,0.49,
            0.5,0.51,0.52,0.53,0.54,0.55,0.56,0.57,0.58,0.59,
            0.6,0.61,0.62,0.63,0.64,0.65,0.66,0.67,0.68,0.69,
            0.7,0.71,0.72,0.73,0.74,0.75,0.76,0.77,0.78,0.79,
            0.8,0.81,0.82,0.83,0.84,0.85,0.86,0.87,0.88,0.89,
            0.9,0.91,0.92,0.93,0.94,0.95,0.96,0.97,0.98,0.99,
            1,1.01,1.02,1.03,1.04,1.05,1.06,1.07,1.08,1.09,
            1.1,1.11,1.12,1.13,1.14,1.15,1.16,1.17,1.18,1.19
        };
        /// <summary>
        /// b=0.0，0.1，0.2
        /// </summary>
        public static double[] A7_H_ρ_b_2 ={
        2.453,2.290,2.158,2.048,1.956,1.880,1.815,1.761,1.741,1.675,
        1.641,1.612,1.587,1.567,1.549,1.534,1.521,1.511,1.502,1.495,
        1.490,1.485,1.482,1.480,1.479,1.478,1.479,1.479,0.481,1.483,
        1.485,1.488,1.491,1.494,1.498,1.502,1.506,1.510,1.515,1.519,
        1.524,1.529,1.534,1.539,1.544,1.550,1.555,1.560,1.566,1.571,
        1.577,1.582,1.588,1.593,1.599,1.605,1.610,1.616,1.622,1.627,
        1.633,1.639,1.644,1.650,1.656,1.661,1.667,1.673,1.678,1.684,
        1.689,1.659,1.701,1.706,1.712,1.717,1.723,1.729,1.734,1.740,
        1.745,1.751,1.756,1.762,1.762,1.773,1.778,1.784,1.789,1.794,
        1.800,1.805,1.811,1.816,1.821,1.827,1.832,1.837,1.843,1.848


        };

        /// <summary>
        /// 当b=0.3，0.4，0.5
        /// </summary>
        public static double[] A7_H_ρ_b_3 ={
        1.105,1.107,1.112,1.120,1.129,1.140,1.513,1.166,1.180,1.194,
        1.208,1.223,1.237,1.251,1.265,1.279,1.292,1.305,1.317,1.329,
        1.341,1.352,1.363,1.374,1.384,1.393,1.403,1.412,1.421,1.429,
        1.437,1.445,1.453,1.461,1.468,1.475,1.483,1.490,1.496,1.503,
        1.510,1.516,1.523,1.529,1.536,1.542,1.548,1.554,1.560,1.567,
        1.537,1.579,1.585,1.591,1.597,1.603,1.608,1.614,1.620,1.626,
        1.632,1.638,1.643,1.649,1.655,1.661,1.666,1.672,1.678,1.683,
        1.689,1.695,1.700,1.706,1.712,1.717,1.723,1.728,1.734,1.740,
        1.745,1.751,1.756,1.762,1.762,1.773,1.778,1.784,1.789,1.794,
        1.800,1.805,1.811,1.816,1.821,1.827,1.832,1.837,1.843,1.848

        };

        /// <summary>
        /// 当ρ>=1.2 步长 0.1
        /// </summary>
        public static double[] A7_H_ρ_next ={
        1.853,1.905,1.956,2.006,2.055,2.102,2.149,2.195,
        2.239,2.283,2.326,2.369,2.411,2.452,2.492,2.532,2.571,2.609,
        2.647,2.685,2.722,2.758,2.794,2.829,2.862,2.899,2.934,2.967
        };

        public static double getA7_H(double ρ, double b)
        {

            double ρ1, ρ0, x0, x1;
            if (ρ == 1.19) return 1.848;

            if (ρ < 0.2) return 0;
            if (ρ > 3.9) return 0;
            if (ρ == 1.2) return 1.853;
            if (ρ == 3.9) return 2.967;
            if ((ρ > 1.19) && (ρ < 1.2))
            {
                return Math.Round((100 * (ρ - 1.19) * (1.853 - 1.848) + 1.848), 3);
            }

            if (ρ < 1.2)
            {
                if (b < 0.3)
                {
                    if (Math.Round(ρ, 2) > ρ)
                    {
                        ρ1 = Math.Round(ρ, 2);
                        ρ0 = ρ1 - 0.01;
                    }
                    else
                    {
                        ρ0 = Math.Round(ρ, 2);
                        ρ1 = ρ0 + 0.01;
                    }


                    x1 = A7_H_ρ_b_2[getIndexOfArray(ρ0, A7_H_ρ, 0.0001)];
                    x0 = A7_H_ρ_b_2[getIndexOfArray(ρ1, A7_H_ρ, 0.0001)];

                    return Math.Round(x1 - 100 * (x1 - x0) * (ρ - ρ0), 3);


                }
                else if (b >= 0.3)
                {
                    if (Math.Round(ρ, 2) > ρ)
                    {
                        ρ1 = Math.Round(ρ, 2);
                        ρ0 = ρ1 - 0.01;
                    }
                    else
                    {
                        ρ0 = Math.Round(ρ, 2);
                        ρ1 = ρ0 + 0.01;
                    }


                    x1 = A7_H_ρ_b_3[getIndexOfArray(ρ0, A7_H_ρ, 0.0001)];
                    x0 = A7_H_ρ_b_3[getIndexOfArray(ρ1, A7_H_ρ, 0.0001)];

                    return Math.Round(x1 - 100 * (x1 - x0) * (ρ - ρ0), 3);
                }
            }
            else
            {
                if (Math.Round(ρ, 1) > ρ)
                {
                    ρ1 = Math.Round(ρ, 1);
                    ρ0 = ρ1 - 0.1;
                }
                else
                {
                    ρ0 = Math.Round(ρ, 1);
                    ρ1 = ρ0 + 0.1;
                }

                x1 = A7_H_ρ_next[Convert.ToInt16((ρ0 - 1.2) * 10)];
                x0 = A7_H_ρ_next[Convert.ToInt16((ρ1 - 1.2) * 10)];

                return Math.Round(x1 - 10 * (x1 - x0) * (ρ - ρ0), 3);

            }
            return 0;
        }


        //t分布双侧分位数，p为置信度，df为自由度,R软件中qt(0.995,seq(1,60,by=1))命令生成0.99对应的60个值
        //131220更改，增加了置信水平
        //R软件程序，生成双侧分位数数据
        //n=100
        //p=c(0.1,0.15,0.2,0.25,0.3,0.35,0.4,0.45,0.5,0.55,0.6,0.65,0.7,0.75,0.8,0.85,0.9,0.91,0.92,0.93,0.94,0.95,0.96,0.97,0.98,0.99)
        //p=(1+p)/2


        //for(pi in 1:length(p)) {

        //a=qt(p[pi],seq(1,n,by=1))
        //write('{',"a.txt",ncolumns=n,append = TRUE,sep="")
        //write(a,"a.txt",ncolumns=n,append = TRUE,sep=",")
        //write('},',"a.txt",ncolumns=n,append = TRUE,sep="")
        //}

        public static double get_t_shuangce(double p, int df)
        {
            double[,] t_shuangce ={

                                    {
                                    0.1583844,0.1421338,0.1365982,0.1338304,0.1321752,0.1310757,0.1302928,0.1297073,0.1292529,0.1288902,0.1285939,0.1283474,0.128139,0.1279607,0.1278063,0.1276713,0.1275522,0.1274465,0.127352,0.127267,0.1271901,0.1271202,0.1270565,0.126998,0.1269443,0.1268947,0.1268488,0.1268063,0.1267666,0.1267296,0.126695,0.1266626,0.1266321,0.1266035,0.1265765,0.126551,0.1265268,0.126504,0.1264823,0.1264617,0.1264421,0.1264235,0.1264057,0.1263887,0.1263725,0.126357,0.1263422,0.126328,0.1263143,0.1263012,0.1262887,0.1262766,0.1262649,0.1262537,0.1262429,0.1262325,0.1262225,0.1262128,0.1262034,0.1261944,0.1261856,0.1261771,0.1261689,0.126161,0.1261533,0.1261458,0.1261386,0.1261315,0.1261247,0.1261181,0.1261116,0.1261054,0.1260993,0.1260933,0.1260876,0.126082,0.1260765,0.1260712,0.126066,0.1260609,0.126056,0.1260511,0.1260464,0.1260418,0.1260374,0.126033,0.1260287,0.1260245,0.1260204,0.1260164,0.1260125,0.1260087,0.126005,0.1260013,0.1259977,0.1259942,0.1259908,0.1259874,0.1259841,0.1259809
                                    },
                                    {
                                    0.2400788,0.2145596,0.2059698,0.2016913,0.1991374,0.1974428,0.1962371,0.1953359,0.1946368,0.1940788,0.1936232,0.1932441,0.1929238,0.1926497,0.1924123,0.1922049,0.192022,0.1918595,0.1917143,0.1915837,0.1914656,0.1913583,0.1912603,0.1911706,0.1910881,0.1910119,0.1909415,0.190876,0.1908152,0.1907583,0.1907052,0.1906554,0.1906087,0.1905647,0.1905232,0.190484,0.190447,0.1904119,0.1903786,0.190347,0.1903169,0.1902883,0.190261,0.190235,0.1902101,0.1901863,0.1901635,0.1901416,0.1901207,0.1901006,0.1900813,0.1900627,0.1900449,0.1900277,0.1900111,0.1899951,0.1899797,0.1899648,0.1899504,0.1899365,0.1899231,0.1899101,0.1898975,0.1898853,0.1898735,0.189862,0.1898509,0.1898401,0.1898296,0.1898194,0.1898095,0.1897999,0.1897906,0.1897815,0.1897726,0.189764,0.1897556,0.1897474,0.1897394,0.1897316,0.1897241,0.1897167,0.1897094,0.1897024,0.1896955,0.1896888,0.1896822,0.1896758,0.1896695,0.1896634,0.1896574,0.1896515,0.1896458,0.1896402,0.1896347,0.1896293,0.189624,0.1896189,0.1896138,0.1896088
                                    },
                                    {
                                    0.3249197,0.2886751,0.2766707,0.2707223,0.2671809,0.2648345,0.2631669,0.2619211,0.2609553,0.2601848,0.2595559,0.2590327,0.2585909,0.2582127,0.2578853,0.2575992,0.257347,0.257123,0.2569228,0.2567428,0.2565799,0.256432,0.2562971,0.2561734,0.2560597,0.2559548,0.2558577,0.2557675,0.2556836,0.2556054,0.2555322,0.2554636,0.2553991,0.2553385,0.2552814,0.2552274,0.2551764,0.2551281,0.2550822,0.2550387,0.2549973,0.2549578,0.2549203,0.2548844,0.2548501,0.2548173,0.2547859,0.2547559,0.254727,0.2546993,0.2546727,0.2546472,0.2546226,0.2545989,0.2545761,0.2545541,0.2545328,0.2545123,0.2544925,0.2544734,0.2544549,0.254437,0.2544196,0.2544028,0.2543866,0.2543708,0.2543555,0.2543406,0.2543262,0.2543121,0.2542985,0.2542853,0.2542724,0.2542599,0.2542477,0.2542358,0.2542242,0.254213,0.254202,0.2541913,0.2541808,0.2541707,0.2541607,0.254151,0.2541415,0.2541323,0.2541232,0.2541144,0.2541058,0.2540973,0.2540891,0.254081,0.2540731,0.2540653,0.2540578,0.2540504,0.2540431,0.254036,0.254029,0.2540222
                                    },
                                    {
                                    0.4142136,0.3651484,0.3492181,0.3413756,0.3367215,0.3336438,0.3314592,0.3298287,0.3285655,0.3275583,0.3267364,0.3260531,0.325476,0.3249822,0.3245549,0.3241815,0.3238525,0.3235603,0.3232991,0.3230642,0.3228518,0.3226589,0.3224829,0.3223217,0.3221734,0.3220366,0.32191,0.3217925,0.3216832,0.3215811,0.3214857,0.3213963,0.3213123,0.3212333,0.3211589,0.3210885,0.321022,0.3209591,0.3208993,0.3208426,0.3207886,0.3207372,0.3206883,0.3206415,0.3205969,0.3205541,0.3205133,0.3204741,0.3204365,0.3204004,0.3203658,0.3203325,0.3203004,0.3202696,0.3202398,0.3202112,0.3201835,0.3201568,0.320131,0.3201061,0.320082,0.3200586,0.3200361,0.3200142,0.319993,0.3199724,0.3199525,0.3199331,0.3199143,0.319896,0.3198783,0.319861,0.3198443,0.319828,0.3198121,0.3197966,0.3197815,0.3197669,0.3197526,0.3197386,0.319725,0.3197117,0.3196988,0.3196862,0.3196738,0.3196618,0.31965,0.3196385,0.3196272,0.3196162,0.3196055,0.319595,0.3195847,0.3195746,0.3195647,0.3195551,0.3195456,0.3195364,0.3195273,0.3195184
                                    },
                                    {
                                    0.5095254,0.4447496,0.4242016,0.4141633,0.4082287,0.4043134,0.4015382,0.3994693,0.3978678,0.3965915,0.3955506,0.3946856,0.3939553,0.3933306,0.3927902,0.392318,0.3919019,0.3915326,0.3912024,0.3909056,0.3906373,0.3903936,0.3901712,0.3899675,0.3897802,0.3896074,0.3894475,0.3892992,0.3891611,0.3890322,0.3889118,0.3887989,0.3886928,0.3885931,0.3884991,0.3884103,0.3883264,0.3882469,0.3881715,0.3880998,0.3880317,0.3879669,0.3879051,0.3878461,0.3877897,0.3877358,0.3876842,0.3876348,0.3875873,0.3875418,0.3874981,0.3874561,0.3874156,0.3873767,0.3873392,0.387303,0.3872681,0.3872344,0.3872019,0.3871704,0.38714,0.3871105,0.387082,0.3870544,0.3870277,0.3870017,0.3869766,0.3869521,0.3869284,0.3869054,0.386883,0.3868612,0.3868401,0.3868195,0.3867995,0.3867799,0.3867609,0.3867424,0.3867244,0.3867068,0.3866896,0.3866729,0.3866566,0.3866406,0.3866251,0.3866098,0.386595,0.3865805,0.3865663,0.3865524,0.3865389,0.3865256,0.3865126,0.3864999,0.3864874,0.3864753,0.3864633,0.3864516,0.3864402,0.386429
                                    },
                                    {
                                    0.6128008,0.5283959,0.5023134,0.4896824,0.4822479,0.4773557,0.4738943,0.471317,0.4693238,0.4677365,0.4664428,0.4653681,0.4644612,0.4636857,0.4630149,0.462429,0.4619129,0.4614548,0.4610454,0.4606774,0.4603447,0.4600426,0.459767,0.4595146,0.4592825,0.4590684,0.4588703,0.4586865,0.4585154,0.4583558,0.4582066,0.4580668,0.4579355,0.4578119,0.4576955,0.4575856,0.4574816,0.4573832,0.4572898,0.4572012,0.4571168,0.4570365,0.45696,0.456887,0.4568172,0.4567505,0.4566866,0.4566254,0.4565667,0.4565103,0.4564562,0.4564042,0.4563541,0.456306,0.4562595,0.4562147,0.4561716,0.4561299,0.4560896,0.4560506,0.456013,0.4559766,0.4559413,0.4559071,0.455874,0.4558419,0.4558108,0.4557805,0.4557512,0.4557227,0.455695,0.4556681,0.4556419,0.4556164,0.4555916,0.4555675,0.4555439,0.455521,0.4554987,0.4554769,0.4554557,0.455435,0.4554148,0.4553951,0.4553758,0.455357,0.4553386,0.4553207,0.4553031,0.4552859,0.4552692,0.4552527,0.4552367,0.455221,0.4552056,0.4551905,0.4551757,0.4551613,0.4551471,0.4551332
                                    },
                                    {
                                    0.7265425,0.6172134,0.5843897,0.5686491,0.5594296,0.5533809,0.5491097,0.5459338,0.5434802,0.541528,0.5399379,0.5386177,0.5375041,0.5365522,0.5357291,0.5350105,0.5343775,0.5338158,0.5333139,0.5328628,0.5324551,0.532085,0.5317473,0.5314381,0.5311538,0.5308916,0.530649,0.5304239,0.5302144,0.530019,0.5298363,0.5296651,0.5295044,0.5293532,0.5292107,0.5290761,0.5289489,0.5288285,0.5287142,0.5286057,0.5285025,0.5284042,0.5283106,0.5282212,0.5281359,0.5280542,0.5279761,0.5279012,0.5278294,0.5277605,0.5276942,0.5276306,0.5275694,0.5275104,0.5274536,0.5273989,0.527346,0.527295,0.5272457,0.5271981,0.5271521,0.5271075,0.5270644,0.5270226,0.5269821,0.5269428,0.5269047,0.5268678,0.5268319,0.526797,0.5267632,0.5267302,0.5266982,0.5266671,0.5266367,0.5266072,0.5265785,0.5265504,0.5265231,0.5264965,0.5264706,0.5264453,0.5264205,0.5263964,0.5263729,0.5263499,0.5263274,0.5263054,0.526284,0.526263,0.5262425,0.5262224,0.5262027,0.5261835,0.5261647,0.5261463,0.5261282,0.5261106,0.5260932,0.5260763
                                    },
                                    {
                                    0.8540807,0.7126268,0.6714714,0.6519466,0.6405729,0.6331353,0.6278948,0.6240043,0.6210023,0.6186159,0.6166734,0.6150617,0.613703,0.6125419,0.6115385,0.6106625,0.6098912,0.6092069,0.6085956,0.6080463,0.60755,0.6070994,0.6066884,0.6063121,0.6059662,0.6056472,0.605352,0.6050782,0.6048234,0.6045857,0.6043635,0.6041554,0.6039599,0.6037761,0.6036028,0.6034392,0.6032846,0.6031381,0.6029992,0.6028673,0.6027419,0.6026225,0.6025087,0.6024,0.6022963,0.6021971,0.6021021,0.6020111,0.6019238,0.6018401,0.6017597,0.6016823,0.6016079,0.6015363,0.6014673,0.6014007,0.6013366,0.6012746,0.6012147,0.6011569,0.6011009,0.6010468,0.6009944,0.6009436,0.6008945,0.6008468,0.6008005,0.6007556,0.600712,0.6006697,0.6006285,0.6005885,0.6005496,0.6005118,0.600475,0.6004391,0.6004042,0.6003702,0.600337,0.6003047,0.6002732,0.6002424,0.6002124,0.6001831,0.6001545,0.6001266,0.6000993,0.6000726,0.6000465,0.6000211,0.5999961,0.5999718,0.5999479,0.5999246,0.5999017,0.5998793,0.5998574,0.599836,0.5998149,0.5997943
                                    },
                                    {
                                    1,0.8164966,0.7648923,0.7406971,0.7266868,0.7175582,0.7111418,0.7063866,0.7027221,0.6998121,0.6974453,0.6954829,0.6938293,0.6924171,0.6911969,0.6901323,0.6891951,0.6883638,0.6876215,0.6869545,0.686352,0.685805,0.6853063,0.6848496,0.68443,0.684043,0.683685,0.6833528,0.6830439,0.6827557,0.6824863,0.6822339,0.681997,0.6817741,0.6815641,0.6813658,0.6811784,0.6810009,0.6808326,0.6806727,0.6805207,0.680376,0.6802381,0.6801065,0.6799808,0.6798606,0.6797456,0.6796353,0.6795296,0.6794282,0.6793308,0.6792371,0.679147,0.6790602,0.6789766,0.678896,0.6788183,0.6787433,0.6786708,0.6786007,0.678533,0.6784674,0.678404,0.6783425,0.6782829,0.6782252,0.6781692,0.6781148,0.678062,0.6780107,0.6779609,0.6779125,0.6778654,0.6778196,0.677775,0.6777316,0.6776893,0.6776481,0.677608,0.6775688,0.6775307,0.6774935,0.6774571,0.6774217,0.677387,0.6773532,0.6773202,0.6772879,0.6772564,0.6772255,0.6771953,0.6771658,0.6771369,0.6771087,0.677081,0.6770539,0.6770274,0.6770014,0.676976,0.676951
                                    },
                                    {
                                    1.17085,0.9313343,0.8664184,0.8363711,0.8190861,0.8078686,0.800005,0.7941885,0.7897126,0.7861621,0.7832771,0.7808867,0.7788738,0.7771556,0.7756718,0.7743775,0.7732386,0.7722287,0.771327,0.7705172,0.7697857,0.7691218,0.7685166,0.7679625,0.7674533,0.7669839,0.7665497,0.7661469,0.7657723,0.7654229,0.7650963,0.7647903,0.7645031,0.764233,0.7639784,0.7637382,0.763511,0.7632959,0.763092,0.7628983,0.7627142,0.7625389,0.7623718,0.7622124,0.7620602,0.7619146,0.7617752,0.7616417,0.7615137,0.7613908,0.7612728,0.7611594,0.7610503,0.7609452,0.760844,0.7607464,0.7606523,0.7605614,0.7604737,0.7603888,0.7603068,0.7602275,0.7601506,0.7600762,0.7600041,0.7599342,0.7598664,0.7598006,0.7597367,0.7596746,0.7596143,0.7595557,0.7594987,0.7594432,0.7593893,0.7593367,0.7592855,0.7592357,0.7591871,0.7591397,0.7590936,0.7590485,0.7590045,0.7589616,0.7589197,0.7588788,0.7588388,0.7587997,0.7587615,0.7587242,0.7586877,0.758652,0.758617,0.7585829,0.7585494,0.7585166,0.7584845,0.7584531,0.7584223,0.7583921
                                    },
                                    {
                                    1.376382,1.06066,0.9784723,0.9409646,0.9195438,0.9057033,0.8960296,0.8888895,0.8834039,0.8790578,0.87553,0.8726093,0.8701515,0.8680548,0.866245,0.864667,0.863279,0.8620487,0.8609506,0.8599644,0.859074,0.8582661,0.8575296,0.8568555,0.8562362,0.8556652,0.8551372,0.8546475,0.854192,0.8537673,0.8533703,0.8529985,0.8526494,0.8523212,0.8520119,0.85172,0.851444,0.8511828,0.850935,0.8506998,0.8504762,0.8502633,0.8500604,0.8498668,0.8496819,0.8495051,0.8493359,0.8491738,0.8490184,0.8488692,0.848726,0.8485883,0.8484558,0.8483283,0.8482054,0.848087,0.8479727,0.8478625,0.8477559,0.847653,0.8475535,0.8474571,0.8473639,0.8472736,0.8471861,0.8471013,0.847019,0.8469391,0.8468616,0.8467863,0.8467131,0.846642,0.8465728,0.8465055,0.8464401,0.8463763,0.8463142,0.8462538,0.8461948,0.8461373,0.8460813,0.8460266,0.8459733,0.8459212,0.8458704,0.8458208,0.8457723,0.8457249,0.8456786,0.8456333,0.845589,0.8455457,0.8455033,0.8454618,0.8454212,0.8453814,0.8453425,0.8453044,0.845267,0.8452304
                                    },
                                    {
                                    1.631852,1.209629,1.10452,1.057299,1.030548,1.013349,1.001367,0.9925445,0.9857784,0.9804254,0.976085,0.9724949,0.9694762,0.9669026,0.9646824,0.9627475,0.9610464,0.959539,0.958194,0.9569866,0.9558967,0.9549079,0.9540068,0.9531822,0.9524248,0.9517266,0.9510811,0.9504824,0.9499257,0.9494066,0.9489215,0.9484672,0.9480407,0.9476397,0.947262,0.9469054,0.9465684,0.9462493,0.9459468,0.9456596,0.9453866,0.9451267,0.944879,0.9446427,0.944417,0.9442012,0.9439947,0.9437968,0.9436072,0.9434251,0.9432503,0.9430823,0.9429206,0.942765,0.9426151,0.9424706,0.9423312,0.9421967,0.9420668,0.9419412,0.9418197,0.9417022,0.9415885,0.9414784,0.9413716,0.9412681,0.9411678,0.9410704,0.9409758,0.940884,0.9407947,0.940708,0.9406237,0.9405416,0.9404617,0.940384,0.9403083,0.9402345,0.9401627,0.9400926,0.9400243,0.9399576,0.9398926,0.9398291,0.9397671,0.9397066,0.9396475,0.9395897,0.9395332,0.939478,0.939424,0.9393712,0.9393195,0.9392689,0.9392194,0.939171,0.9391235,0.939077,0.9390315,0.9389869
                                    },
                                    {
                                    1.962611,1.386207,1.249778,1.189567,1.155767,1.134157,1.119159,1.108145,1.099716,1.093058,1.087666,1.083211,1.079469,1.07628,1.073531,1.071137,1.069033,1.06717,1.065507,1.064016,1.06267,1.061449,1.060337,1.059319,1.058384,1.057523,1.056727,1.055989,1.055302,1.054662,1.054064,1.053504,1.052979,1.052485,1.052019,1.05158,1.051165,1.050772,1.050399,1.050046,1.04971,1.04939,1.049085,1.048794,1.048516,1.04825,1.047996,1.047752,1.047519,1.047295,1.04708,1.046873,1.046674,1.046483,1.046298,1.04612,1.045949,1.045783,1.045623,1.045469,1.04532,1.045175,1.045035,1.0449,1.044768,1.044641,1.044518,1.044398,1.044281,1.044169,1.044059,1.043952,1.043848,1.043747,1.043649,1.043554,1.043461,1.04337,1.043282,1.043195,1.043111,1.043029,1.042949,1.042871,1.042795,1.042721,1.042648,1.042577,1.042508,1.04244,1.042373,1.042308,1.042245,1.042183,1.042122,1.042062,1.042004,1.041947,1.041891,1.041836
                                    },
                                    {
                                    2.414214,1.603567,1.422625,1.344398,1.300949,1.273349,1.254279,1.240318,1.229659,1.221255,1.21446,1.208853,1.204146,1.20014,1.196689,1.193685,1.191047,1.188711,1.186629,1.184761,1.183076,1.181549,1.180157,1.178884,1.177716,1.176639,1.175644,1.174722,1.173864,1.173065,1.172318,1.171619,1.170963,1.170346,1.169765,1.169217,1.168699,1.168208,1.167743,1.167302,1.166883,1.166483,1.166103,1.16574,1.165394,1.165062,1.164745,1.164442,1.164151,1.163871,1.163603,1.163345,1.163097,1.162859,1.162629,1.162407,1.162194,1.161987,1.161788,1.161596,1.161409,1.161229,1.161055,1.160886,1.160723,1.160564,1.16041,1.160261,1.160116,1.159975,1.159839,1.159706,1.159577,1.159451,1.159329,1.15921,1.159094,1.158981,1.158871,1.158763,1.158659,1.158557,1.158457,1.15836,1.158265,1.158172,1.158082,1.157993,1.157907,1.157822,1.15774,1.157659,1.15758,1.157502,1.157426,1.157352,1.15728,1.157209,1.157139,1.157071
                                    },
                                    {
                                    3.077684,1.885618,1.637744,1.533206,1.475884,1.439756,1.414924,1.396815,1.383029,1.372184,1.36343,1.356217,1.350171,1.34503,1.340606,1.336757,1.333379,1.330391,1.327728,1.325341,1.323188,1.321237,1.31946,1.317836,1.316345,1.314972,1.313703,1.312527,1.311434,1.310415,1.309464,1.308573,1.307737,1.306952,1.306212,1.305514,1.304854,1.30423,1.303639,1.303077,1.302543,1.302035,1.301552,1.30109,1.300649,1.300228,1.299825,1.299439,1.299069,1.298714,1.298373,1.298045,1.29773,1.297426,1.297134,1.296853,1.296581,1.296319,1.296066,1.295821,1.295585,1.295356,1.295134,1.29492,1.294712,1.294511,1.294315,1.294126,1.293942,1.293763,1.293589,1.293421,1.293256,1.293097,1.292941,1.29279,1.292643,1.2925,1.29236,1.292224,1.292091,1.291961,1.291835,1.291711,1.291591,1.291473,1.291358,1.291246,1.291136,1.291029,1.290924,1.290821,1.290721,1.290623,1.290527,1.290432,1.29034,1.29025,1.290161,1.290075
                                    },
                                    {
                                    4.1653,2.281931,1.92432,1.778192,1.699363,1.650173,1.616592,1.592221,1.573736,1.559236,1.54756,1.537956,1.52992,1.523095,1.517228,1.51213,1.50766,1.503708,1.500189,1.497036,1.494194,1.49162,1.489277,1.487136,1.485171,1.483363,1.481692,1.480143,1.478705,1.477365,1.476113,1.474942,1.473843,1.47281,1.471838,1.470921,1.470055,1.469235,1.468458,1.46772,1.46702,1.466353,1.465718,1.465112,1.464534,1.463981,1.463452,1.462945,1.46246,1.461994,1.461547,1.461117,1.460704,1.460306,1.459923,1.459554,1.459197,1.458854,1.458522,1.458201,1.457891,1.457591,1.457301,1.45702,1.456748,1.456484,1.456228,1.455979,1.455738,1.455504,1.455277,1.455056,1.454841,1.454632,1.454428,1.45423,1.454037,1.45385,1.453666,1.453488,1.453314,1.453144,1.452979,1.452817,1.45266,1.452505,1.452355,1.452208,1.452064,1.451924,1.451786,1.451652,1.451521,1.451392,1.451266,1.451143,1.451022,1.450904,1.450788,1.450675
                                    },
                                    {
                                    6.313752,2.919986,2.353363,2.131847,2.015048,1.94318,1.894579,1.859548,1.833113,1.812461,1.795885,1.782288,1.770933,1.76131,1.75305,1.745884,1.739607,1.734064,1.729133,1.724718,1.720743,1.717144,1.713872,1.710882,1.708141,1.705618,1.703288,1.701131,1.699127,1.697261,1.695519,1.693889,1.69236,1.690924,1.689572,1.688298,1.687094,1.685954,1.684875,1.683851,1.682878,1.681952,1.681071,1.68023,1.679427,1.67866,1.677927,1.677224,1.676551,1.675905,1.675285,1.674689,1.674116,1.673565,1.673034,1.672522,1.672029,1.671553,1.671093,1.670649,1.670219,1.669804,1.669402,1.669013,1.668636,1.668271,1.667916,1.667572,1.667239,1.666914,1.6666,1.666294,1.665996,1.665707,1.665425,1.665151,1.664885,1.664625,1.664371,1.664125,1.663884,1.663649,1.66342,1.663197,1.662978,1.662765,1.662557,1.662354,1.662155,1.661961,1.661771,1.661585,1.661404,1.661226,1.661052,1.660881,1.660715,1.660551,1.660391,1.660234
                                    },
                                    {
                                    7.026366,3.103977,2.470807,2.2261,2.097837,2.019201,1.966153,1.927986,1.899222,1.876774,1.858772,1.844015,1.8317,1.821267,1.812316,1.804553,1.797755,1.791754,1.786417,1.78164,1.777339,1.773447,1.769907,1.766675,1.763711,1.760983,1.758466,1.756134,1.753968,1.751952,1.750069,1.748308,1.746657,1.745106,1.743645,1.742269,1.740968,1.739738,1.738572,1.737466,1.736416,1.735416,1.734464,1.733557,1.73269,1.731862,1.73107,1.730312,1.729585,1.728888,1.728219,1.727576,1.726957,1.726362,1.725789,1.725237,1.724705,1.724191,1.723695,1.723215,1.722752,1.722304,1.72187,1.72145,1.721043,1.720649,1.720267,1.719896,1.719536,1.719186,1.718846,1.718516,1.718195,1.717883,1.717579,1.717284,1.716996,1.716716,1.716442,1.716176,1.715916,1.715663,1.715416,1.715175,1.71494,1.71471,1.714485,1.714266,1.714052,1.713842,1.713637,1.713437,1.713241,1.713049,1.712862,1.712678,1.712498,1.712322,1.712149,1.71198
                                    },
                                    {
                                    7.915815,3.319764,2.605427,2.332873,2.190958,2.104306,2.046011,2.004152,1.972653,1.948099,1.928427,1.912313,1.898874,1.887496,1.877739,1.869279,1.861875,1.85534,1.84953,1.844331,1.839651,1.835417,1.831567,1.828051,1.824828,1.821863,1.819126,1.816592,1.814238,1.812047,1.810002,1.808089,1.806295,1.80461,1.803024,1.801528,1.800116,1.79878,1.797514,1.796314,1.795173,1.794088,1.793054,1.792069,1.791128,1.79023,1.78937,1.788547,1.787758,1.787001,1.786275,1.785577,1.784906,1.78426,1.783638,1.783039,1.782461,1.781904,1.781366,1.780846,1.780343,1.779857,1.779386,1.77893,1.778489,1.778061,1.777646,1.777244,1.776853,1.776474,1.776106,1.775748,1.7754,1.775061,1.774732,1.774411,1.774099,1.773795,1.773498,1.77321,1.772928,1.772653,1.772385,1.772124,1.771869,1.771619,1.771376,1.771138,1.770906,1.770679,1.770456,1.770239,1.770027,1.769819,1.769615,1.769416,1.769221,1.76903,1.768842,1.768659
                                    },
                                    {
                                    9.057887,3.578247,2.762599,2.455892,2.297392,2.201059,2.136453,2.090166,2.055395,2.028327,2.006663,1.988934,1.974158,1.961656,1.95094,1.941654,1.93353,1.926362,1.919992,1.914292,1.909164,1.904524,1.900307,1.896457,1.892928,1.889682,1.886686,1.883912,1.881336,1.878938,1.876701,1.874607,1.872645,1.870802,1.869068,1.867432,1.865888,1.864427,1.863043,1.861731,1.860483,1.859297,1.858168,1.857091,1.856063,1.85508,1.854141,1.853241,1.852379,1.851552,1.850759,1.849996,1.849263,1.848558,1.847878,1.847224,1.846592,1.845983,1.845395,1.844827,1.844278,1.843747,1.843233,1.842736,1.842253,1.841786,1.841333,1.840894,1.840467,1.840053,1.839651,1.83926,1.838879,1.83851,1.83815,1.8378,1.837459,1.837127,1.836803,1.836488,1.836181,1.835881,1.835588,1.835303,1.835024,1.834752,1.834486,1.834227,1.833973,1.833725,1.833482,1.833245,1.833013,1.832786,1.832564,1.832346,1.832133,1.831925,1.83172,1.83152
                                    },
                                    {
                                    10.57889,3.896425,2.95051,2.600762,2.421585,2.313263,2.240879,2.189155,2.150375,2.120234,2.096139,2.076441,2.060038,2.046169,2.034289,2.024,2.015002,2.007067,2.000017,1.993713,1.988041,1.982911,1.978249,1.973994,1.970095,1.966509,1.9632,1.960136,1.957293,1.954645,1.952175,1.949865,1.9477,1.945666,1.943752,1.941948,1.940244,1.938633,1.937106,1.935659,1.934283,1.932975,1.93173,1.930542,1.929409,1.928326,1.92729,1.926298,1.925348,1.924437,1.923562,1.922722,1.921914,1.921136,1.920388,1.919666,1.918971,1.9183,1.917652,1.917026,1.916421,1.915836,1.915269,1.914721,1.91419,1.913676,1.913176,1.912692,1.912222,1.911766,1.911323,1.910892,1.910474,1.910066,1.90967,1.909285,1.908909,1.908544,1.908187,1.90784,1.907501,1.907171,1.906849,1.906535,1.906228,1.905928,1.905636,1.90535,1.90507,1.904797,1.90453,1.904269,1.904013,1.903763,1.903519,1.903279,1.903045,1.902815,1.90259,1.90237
                                    },
                                    {
                                    12.7062,4.302653,3.182446,2.776445,2.570582,2.446912,2.364624,2.306004,2.262157,2.228139,2.200985,2.178813,2.160369,2.144787,2.13145,2.119905,2.109816,2.100922,2.093024,2.085963,2.079614,2.073873,2.068658,2.063899,2.059539,2.055529,2.051831,2.048407,2.04523,2.042272,2.039513,2.036933,2.034515,2.032245,2.030108,2.028094,2.026192,2.024394,2.022691,2.021075,2.019541,2.018082,2.016692,2.015368,2.014103,2.012896,2.011741,2.010635,2.009575,2.008559,2.007584,2.006647,2.005746,2.004879,2.004045,2.003241,2.002465,2.001717,2.000995,2.000298,1.999624,1.998972,1.998341,1.99773,1.997138,1.996564,1.996008,1.995469,1.994945,1.994437,1.993943,1.993464,1.992997,1.992543,1.992102,1.991673,1.991254,1.990847,1.99045,1.990063,1.989686,1.989319,1.98896,1.98861,1.988268,1.987934,1.987608,1.98729,1.986979,1.986675,1.986377,1.986086,1.985802,1.985523,1.985251,1.984984,1.984723,1.984467,1.984217,1.983972
                                    },
                                    {
                                    15.89454,4.848732,3.481909,2.998528,2.756509,2.612242,2.516752,2.448985,2.398441,2.359315,2.32814,2.302722,2.281604,2.263781,2.24854,2.235358,2.223845,2.213703,2.204701,2.196658,2.189427,2.182893,2.176958,2.171545,2.166587,2.162029,2.157825,2.153935,2.150325,2.146966,2.143833,2.140904,2.138159,2.135581,2.133157,2.130871,2.128714,2.126674,2.124742,2.12291,2.12117,2.119515,2.11794,2.116438,2.115005,2.113636,2.112327,2.111073,2.109873,2.108721,2.107616,2.106555,2.105534,2.104552,2.103607,2.102696,2.101818,2.100971,2.100153,2.099363,2.098599,2.097861,2.097146,2.096455,2.095785,2.095135,2.094506,2.093895,2.093302,2.092727,2.092168,2.091625,2.091097,2.090584,2.090084,2.089598,2.089124,2.088663,2.088214,2.087777,2.08735,2.086934,2.086528,2.086131,2.085745,2.085367,2.084998,2.084638,2.084286,2.083942,2.083605,2.083276,2.082954,2.08264,2.082331,2.08203,2.081734,2.081445,2.081162,2.080884
                                    },
                                    {
                                    21.20495,5.642778,3.896046,3.29763,3.002875,2.828928,2.714573,2.633814,2.573804,2.527484,2.490664,2.4607,2.435845,2.414898,2.397005,2.381545,2.368055,2.35618,2.345648,2.336242,2.327792,2.32016,2.313231,2.306913,2.30113,2.295815,2.290914,2.28638,2.282175,2.278262,2.274614,2.271203,2.268008,2.265009,2.262188,2.259529,2.25702,2.254648,2.252401,2.250271,2.248249,2.246326,2.244495,2.24275,2.241085,2.239494,2.237974,2.236518,2.235124,2.233787,2.232503,2.231271,2.230086,2.228946,2.227849,2.226792,2.225772,2.224789,2.22384,2.222923,2.222038,2.221181,2.220352,2.219549,2.218772,2.218019,2.217289,2.21658,2.215893,2.215226,2.214577,2.213948,2.213335,2.21274,2.212161,2.211597,2.211048,2.210514,2.209993,2.209485,2.208991,2.208508,2.208038,2.207578,2.20713,2.206692,2.206265,2.205847,2.205439,2.205041,2.204651,2.204269,2.203896,2.203531,2.203174,2.202824,2.202482,2.202147,2.201819,2.201497
                                    },
                                    {
                                    31.82052,6.964557,4.540703,3.746947,3.36493,3.142668,2.997952,2.896459,2.821438,2.763769,2.718079,2.680998,2.650309,2.624494,2.60248,2.583487,2.566934,2.55238,2.539483,2.527977,2.517648,2.508325,2.499867,2.492159,2.485107,2.47863,2.47266,2.46714,2.462021,2.457262,2.452824,2.448678,2.444794,2.44115,2.437723,2.434494,2.431447,2.428568,2.425841,2.423257,2.420803,2.41847,2.41625,2.414134,2.412116,2.410188,2.408345,2.406581,2.404892,2.403272,2.401718,2.400225,2.39879,2.39741,2.396081,2.394801,2.393568,2.392377,2.391229,2.390119,2.389047,2.388011,2.387008,2.386037,2.385097,2.384186,2.383302,2.382446,2.381615,2.380807,2.380024,2.379262,2.378522,2.377802,2.377102,2.37642,2.375757,2.375111,2.374482,2.373868,2.37327,2.372687,2.372119,2.371564,2.371022,2.370493,2.369977,2.369472,2.368979,2.368497,2.368026,2.367566,2.367115,2.366674,2.366243,2.365821,2.365407,2.365002,2.364606,2.364217
                                    },
                                    {
                                    63.65674,9.924843,5.840909,4.604095,4.032143,3.707428,3.499483,3.355387,3.249836,3.169273,3.105807,3.05454,3.012276,2.976843,2.946713,2.920782,2.898231,2.87844,2.860935,2.84534,2.83136,2.818756,2.807336,2.79694,2.787436,2.778715,2.770683,2.763262,2.756386,2.749996,2.744042,2.738481,2.733277,2.728394,2.723806,2.719485,2.715409,2.711558,2.707913,2.704459,2.701181,2.698066,2.695102,2.692278,2.689585,2.687013,2.684556,2.682204,2.679952,2.677793,2.675722,2.673734,2.671823,2.669985,2.668216,2.666512,2.66487,2.663287,2.661759,2.660283,2.658857,2.657479,2.656145,2.654854,2.653604,2.652394,2.65122,2.650081,2.648977,2.647905,2.646863,2.645852,2.644869,2.643913,2.642983,2.642078,2.641198,2.64034,2.639505,2.638691,2.637897,2.637123,2.636369,2.635632,2.634914,2.634212,2.633527,2.632858,2.632204,2.631565,2.63094,2.63033,2.629732,2.629148,2.628576,2.628016,2.627468,2.626931,2.626405,2.625891
                                    }
                                  };
            if ((df < 101) && (df > 0))
            {
                if (Math.Abs(p - 0.10) < 0.000001)
                    return t_shuangce[0, df - 1];
                if (Math.Abs(p - 0.15) < 0.000001)
                    return t_shuangce[1, df - 1];
                if (Math.Abs(p - 0.20) < 0.000001)
                    return t_shuangce[2, df - 1];
                if (Math.Abs(p - 0.25) < 0.000001)
                    return t_shuangce[3, df - 1];
                if (Math.Abs(p - 0.30) < 0.000001)
                    return t_shuangce[4, df - 1];
                if (Math.Abs(p - 0.35) < 0.000001)
                    return t_shuangce[5, df - 1];
                if (Math.Abs(p - 0.40) < 0.000001)
                    return t_shuangce[6, df - 1];
                if (Math.Abs(p - 0.45) < 0.000001)
                    return t_shuangce[7, df - 1];
                if (Math.Abs(p - 0.50) < 0.000001)
                    return t_shuangce[8, df - 1];
                if (Math.Abs(p - 0.55) < 0.000001)
                    return t_shuangce[9, df - 1];
                if (Math.Abs(p - 0.60) < 0.000001)
                    return t_shuangce[10, df - 1];
                if (Math.Abs(p - 0.65) < 0.000001)
                    return t_shuangce[11, df - 1];
                if (Math.Abs(p - 0.70) < 0.000001)
                    return t_shuangce[12, df - 1];
                if (Math.Abs(p - 0.75) < 0.000001)
                    return t_shuangce[13, df - 1];
                if (Math.Abs(p - 0.80) < 0.000001)
                    return t_shuangce[14, df - 1];
                if (Math.Abs(p - 0.85) < 0.000001)
                    return t_shuangce[15, df - 1];
                if (Math.Abs(p - 0.90) < 0.000001)
                    return t_shuangce[16, df - 1];
                if (Math.Abs(p - 0.91) < 0.000001)
                    return t_shuangce[17, df - 1];
                if (Math.Abs(p - 0.92) < 0.000001)
                    return t_shuangce[18, df - 1];
                if (Math.Abs(p - 0.93) < 0.000001)
                    return t_shuangce[19, df - 1];
                if (Math.Abs(p - 0.94) < 0.000001)
                    return t_shuangce[20, df - 1];
                if (Math.Abs(p - 0.95) < 0.000001)
                    return t_shuangce[21, df - 1];
                if (Math.Abs(p - 0.96) < 0.000001)
                    return t_shuangce[22, df - 1];
                if (Math.Abs(p - 0.97) < 0.000001)
                    return t_shuangce[23, df - 1];
                if (Math.Abs(p - 0.98) < 0.000001)
                    return t_shuangce[24, df - 1];
                if (Math.Abs(p - 0.99) < 0.000001)
                    return t_shuangce[25, df - 1];
                else return MLR_polar.tinv((p + 1) / 2, df);
            }
            else return 0;
        }
        //t分布单侧分位数，p为置信度，df为自由度
        //131220更改，增加了置信水平
        //R软件程序，生成单侧分位数数据
        //n=100
        //p=c(0.1,0.15,0.2,0.25,0.3,0.35,0.4,0.45,0.5,0.55,0.6,0.65,0.7,0.75,0.8,0.85,0.9,0.91,0.92,0.93,0.94,0.95,0.96,0.97,0.98,0.99)
        //

        //for(pi in 1:length(p)) {

        //a=qt(p[pi],seq(1,n,by=1))
        //write('{',"a.txt",ncolumns=n,append = TRUE,sep="")
        //write(a,"a.txt",ncolumns=n,append = TRUE,sep=",")
        //write('},',"a.txt",ncolumns=n,append = TRUE,sep="")
        //}
        public static double get_t_dance(double p, int df)
        {
            double[,] t_shuangce ={
                                    {
                                    -3.077684,-1.885618,-1.637744,-1.533206,-1.475884,-1.439756,-1.414924,-1.396815,-1.383029,-1.372184,-1.36343,-1.356217,-1.350171,-1.34503,-1.340606,-1.336757,-1.333379,-1.330391,-1.327728,-1.325341,-1.323188,-1.321237,-1.31946,-1.317836,-1.316345,-1.314972,-1.313703,-1.312527,-1.311434,-1.310415,-1.309464,-1.308573,-1.307737,-1.306952,-1.306212,-1.305514,-1.304854,-1.30423,-1.303639,-1.303077,-1.302543,-1.302035,-1.301552,-1.30109,-1.300649,-1.300228,-1.299825,-1.299439,-1.299069,-1.298714,-1.298373,-1.298045,-1.29773,-1.297426,-1.297134,-1.296853,-1.296581,-1.296319,-1.296066,-1.295821,-1.295585,-1.295356,-1.295134,-1.29492,-1.294712,-1.294511,-1.294315,-1.294126,-1.293942,-1.293763,-1.293589,-1.293421,-1.293256,-1.293097,-1.292941,-1.29279,-1.292643,-1.2925,-1.29236,-1.292224,-1.292091,-1.291961,-1.291835,-1.291711,-1.291591,-1.291473,-1.291358,-1.291246,-1.291136,-1.291029,-1.290924,-1.290821,-1.290721,-1.290623,-1.290527,-1.290432,-1.29034,-1.29025,-1.290161,-1.290075
                                    },
                                    {
                                    -1.962611,-1.386207,-1.249778,-1.189567,-1.155767,-1.134157,-1.119159,-1.108145,-1.099716,-1.093058,-1.087666,-1.083211,-1.079469,-1.07628,-1.073531,-1.071137,-1.069033,-1.06717,-1.065507,-1.064016,-1.06267,-1.061449,-1.060337,-1.059319,-1.058384,-1.057523,-1.056727,-1.055989,-1.055302,-1.054662,-1.054064,-1.053504,-1.052979,-1.052485,-1.052019,-1.05158,-1.051165,-1.050772,-1.050399,-1.050046,-1.04971,-1.04939,-1.049085,-1.048794,-1.048516,-1.04825,-1.047996,-1.047752,-1.047519,-1.047295,-1.04708,-1.046873,-1.046674,-1.046483,-1.046298,-1.04612,-1.045949,-1.045783,-1.045623,-1.045469,-1.04532,-1.045175,-1.045035,-1.0449,-1.044768,-1.044641,-1.044518,-1.044398,-1.044281,-1.044169,-1.044059,-1.043952,-1.043848,-1.043747,-1.043649,-1.043554,-1.043461,-1.04337,-1.043282,-1.043195,-1.043111,-1.043029,-1.042949,-1.042871,-1.042795,-1.042721,-1.042648,-1.042577,-1.042508,-1.04244,-1.042373,-1.042308,-1.042245,-1.042183,-1.042122,-1.042062,-1.042004,-1.041947,-1.041891,-1.041836
                                    },
                                    {
                                    -1.376382,-1.06066,-0.9784723,-0.9409646,-0.9195438,-0.9057033,-0.8960296,-0.8888895,-0.8834039,-0.8790578,-0.87553,-0.8726093,-0.8701515,-0.8680548,-0.866245,-0.864667,-0.863279,-0.8620487,-0.8609506,-0.8599644,-0.859074,-0.8582661,-0.8575296,-0.8568555,-0.8562362,-0.8556652,-0.8551372,-0.8546475,-0.854192,-0.8537673,-0.8533703,-0.8529985,-0.8526494,-0.8523212,-0.8520119,-0.85172,-0.851444,-0.8511828,-0.850935,-0.8506998,-0.8504762,-0.8502633,-0.8500604,-0.8498668,-0.8496819,-0.8495051,-0.8493359,-0.8491738,-0.8490184,-0.8488692,-0.848726,-0.8485883,-0.8484558,-0.8483283,-0.8482054,-0.848087,-0.8479727,-0.8478625,-0.8477559,-0.847653,-0.8475535,-0.8474571,-0.8473639,-0.8472736,-0.8471861,-0.8471013,-0.847019,-0.8469391,-0.8468616,-0.8467863,-0.8467131,-0.846642,-0.8465728,-0.8465055,-0.8464401,-0.8463763,-0.8463142,-0.8462538,-0.8461948,-0.8461373,-0.8460813,-0.8460266,-0.8459733,-0.8459212,-0.8458704,-0.8458208,-0.8457723,-0.8457249,-0.8456786,-0.8456333,-0.845589,-0.8455457,-0.8455033,-0.8454618,-0.8454212,-0.8453814,-0.8453425,-0.8453044,-0.845267,-0.8452304
                                    },
                                    {
                                    -1,-0.8164966,-0.7648923,-0.7406971,-0.7266868,-0.7175582,-0.7111418,-0.7063866,-0.7027221,-0.6998121,-0.6974453,-0.6954829,-0.6938293,-0.6924171,-0.6911969,-0.6901323,-0.6891951,-0.6883638,-0.6876215,-0.6869545,-0.686352,-0.685805,-0.6853063,-0.6848496,-0.68443,-0.684043,-0.683685,-0.6833528,-0.6830439,-0.6827557,-0.6824863,-0.6822339,-0.681997,-0.6817741,-0.6815641,-0.6813658,-0.6811784,-0.6810009,-0.6808326,-0.6806727,-0.6805207,-0.680376,-0.6802381,-0.6801065,-0.6799808,-0.6798606,-0.6797456,-0.6796353,-0.6795296,-0.6794282,-0.6793308,-0.6792371,-0.679147,-0.6790602,-0.6789766,-0.678896,-0.6788183,-0.6787433,-0.6786708,-0.6786007,-0.678533,-0.6784674,-0.678404,-0.6783425,-0.6782829,-0.6782252,-0.6781692,-0.6781148,-0.678062,-0.6780107,-0.6779609,-0.6779125,-0.6778654,-0.6778196,-0.677775,-0.6777316,-0.6776893,-0.6776481,-0.677608,-0.6775688,-0.6775307,-0.6774935,-0.6774571,-0.6774217,-0.677387,-0.6773532,-0.6773202,-0.6772879,-0.6772564,-0.6772255,-0.6771953,-0.6771658,-0.6771369,-0.6771087,-0.677081,-0.6770539,-0.6770274,-0.6770014,-0.676976,-0.676951
                                    },
                                    {
                                    -0.7265425,-0.6172134,-0.5843897,-0.5686491,-0.5594296,-0.5533809,-0.5491097,-0.5459338,-0.5434802,-0.541528,-0.5399379,-0.5386177,-0.5375041,-0.5365522,-0.5357291,-0.5350105,-0.5343775,-0.5338158,-0.5333139,-0.5328628,-0.5324551,-0.532085,-0.5317473,-0.5314381,-0.5311538,-0.5308916,-0.530649,-0.5304239,-0.5302144,-0.530019,-0.5298363,-0.5296651,-0.5295044,-0.5293532,-0.5292107,-0.5290761,-0.5289489,-0.5288285,-0.5287142,-0.5286057,-0.5285025,-0.5284042,-0.5283106,-0.5282212,-0.5281359,-0.5280542,-0.5279761,-0.5279012,-0.5278294,-0.5277605,-0.5276942,-0.5276306,-0.5275694,-0.5275104,-0.5274536,-0.5273989,-0.527346,-0.527295,-0.5272457,-0.5271981,-0.5271521,-0.5271075,-0.5270644,-0.5270226,-0.5269821,-0.5269428,-0.5269047,-0.5268678,-0.5268319,-0.526797,-0.5267632,-0.5267302,-0.5266982,-0.5266671,-0.5266367,-0.5266072,-0.5265785,-0.5265504,-0.5265231,-0.5264965,-0.5264706,-0.5264453,-0.5264205,-0.5263964,-0.5263729,-0.5263499,-0.5263274,-0.5263054,-0.526284,-0.526263,-0.5262425,-0.5262224,-0.5262027,-0.5261835,-0.5261647,-0.5261463,-0.5261282,-0.5261106,-0.5260932,-0.5260763
                                    },
                                    {
                                    -0.5095254,-0.4447496,-0.4242016,-0.4141633,-0.4082287,-0.4043134,-0.4015382,-0.3994693,-0.3978678,-0.3965915,-0.3955506,-0.3946856,-0.3939553,-0.3933306,-0.3927902,-0.392318,-0.3919019,-0.3915326,-0.3912024,-0.3909056,-0.3906373,-0.3903936,-0.3901712,-0.3899675,-0.3897802,-0.3896074,-0.3894475,-0.3892992,-0.3891611,-0.3890322,-0.3889118,-0.3887989,-0.3886928,-0.3885931,-0.3884991,-0.3884103,-0.3883264,-0.3882469,-0.3881715,-0.3880998,-0.3880317,-0.3879669,-0.3879051,-0.3878461,-0.3877897,-0.3877358,-0.3876842,-0.3876348,-0.3875873,-0.3875418,-0.3874981,-0.3874561,-0.3874156,-0.3873767,-0.3873392,-0.387303,-0.3872681,-0.3872344,-0.3872019,-0.3871704,-0.38714,-0.3871105,-0.387082,-0.3870544,-0.3870277,-0.3870017,-0.3869766,-0.3869521,-0.3869284,-0.3869054,-0.386883,-0.3868612,-0.3868401,-0.3868195,-0.3867995,-0.3867799,-0.3867609,-0.3867424,-0.3867244,-0.3867068,-0.3866896,-0.3866729,-0.3866566,-0.3866406,-0.3866251,-0.3866098,-0.386595,-0.3865805,-0.3865663,-0.3865524,-0.3865389,-0.3865256,-0.3865126,-0.3864999,-0.3864874,-0.3864753,-0.3864633,-0.3864516,-0.3864402,-0.386429
                                    },
                                    {
                                    -0.3249197,-0.2886751,-0.2766707,-0.2707223,-0.2671809,-0.2648345,-0.2631669,-0.2619211,-0.2609553,-0.2601848,-0.2595559,-0.2590327,-0.2585909,-0.2582127,-0.2578853,-0.2575992,-0.257347,-0.257123,-0.2569228,-0.2567428,-0.2565799,-0.256432,-0.2562971,-0.2561734,-0.2560597,-0.2559548,-0.2558577,-0.2557675,-0.2556836,-0.2556054,-0.2555322,-0.2554636,-0.2553991,-0.2553385,-0.2552814,-0.2552274,-0.2551764,-0.2551281,-0.2550822,-0.2550387,-0.2549973,-0.2549578,-0.2549203,-0.2548844,-0.2548501,-0.2548173,-0.2547859,-0.2547559,-0.254727,-0.2546993,-0.2546727,-0.2546472,-0.2546226,-0.2545989,-0.2545761,-0.2545541,-0.2545328,-0.2545123,-0.2544925,-0.2544734,-0.2544549,-0.254437,-0.2544196,-0.2544028,-0.2543866,-0.2543708,-0.2543555,-0.2543406,-0.2543262,-0.2543121,-0.2542985,-0.2542853,-0.2542724,-0.2542599,-0.2542477,-0.2542358,-0.2542242,-0.254213,-0.254202,-0.2541913,-0.2541808,-0.2541707,-0.2541607,-0.254151,-0.2541415,-0.2541323,-0.2541232,-0.2541144,-0.2541058,-0.2540973,-0.2540891,-0.254081,-0.2540731,-0.2540653,-0.2540578,-0.2540504,-0.2540431,-0.254036,-0.254029,-0.2540222
                                    },
                                    {
                                    -0.1583844,-0.1421338,-0.1365982,-0.1338304,-0.1321752,-0.1310757,-0.1302928,-0.1297073,-0.1292529,-0.1288902,-0.1285939,-0.1283474,-0.128139,-0.1279607,-0.1278063,-0.1276713,-0.1275522,-0.1274465,-0.127352,-0.127267,-0.1271901,-0.1271202,-0.1270565,-0.126998,-0.1269443,-0.1268947,-0.1268488,-0.1268063,-0.1267666,-0.1267296,-0.126695,-0.1266626,-0.1266321,-0.1266035,-0.1265765,-0.126551,-0.1265268,-0.126504,-0.1264823,-0.1264617,-0.1264421,-0.1264235,-0.1264057,-0.1263887,-0.1263725,-0.126357,-0.1263422,-0.126328,-0.1263143,-0.1263012,-0.1262887,-0.1262766,-0.1262649,-0.1262537,-0.1262429,-0.1262325,-0.1262225,-0.1262128,-0.1262034,-0.1261944,-0.1261856,-0.1261771,-0.1261689,-0.126161,-0.1261533,-0.1261458,-0.1261386,-0.1261315,-0.1261247,-0.1261181,-0.1261116,-0.1261054,-0.1260993,-0.1260933,-0.1260876,-0.126082,-0.1260765,-0.1260712,-0.126066,-0.1260609,-0.126056,-0.1260511,-0.1260464,-0.1260418,-0.1260374,-0.126033,-0.1260287,-0.1260245,-0.1260204,-0.1260164,-0.1260125,-0.1260087,-0.126005,-0.1260013,-0.1259977,-0.1259942,-0.1259908,-0.1259874,-0.1259841,-0.1259809
                                    },
                                    {
                                    6.123032e-17,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0
                                    },
                                    {
                                    0.1583844,0.1421338,0.1365982,0.1338304,0.1321752,0.1310757,0.1302928,0.1297073,0.1292529,0.1288902,0.1285939,0.1283474,0.128139,0.1279607,0.1278063,0.1276713,0.1275522,0.1274465,0.127352,0.127267,0.1271901,0.1271202,0.1270565,0.126998,0.1269443,0.1268947,0.1268488,0.1268063,0.1267666,0.1267296,0.126695,0.1266626,0.1266321,0.1266035,0.1265765,0.126551,0.1265268,0.126504,0.1264823,0.1264617,0.1264421,0.1264235,0.1264057,0.1263887,0.1263725,0.126357,0.1263422,0.126328,0.1263143,0.1263012,0.1262887,0.1262766,0.1262649,0.1262537,0.1262429,0.1262325,0.1262225,0.1262128,0.1262034,0.1261944,0.1261856,0.1261771,0.1261689,0.126161,0.1261533,0.1261458,0.1261386,0.1261315,0.1261247,0.1261181,0.1261116,0.1261054,0.1260993,0.1260933,0.1260876,0.126082,0.1260765,0.1260712,0.126066,0.1260609,0.126056,0.1260511,0.1260464,0.1260418,0.1260374,0.126033,0.1260287,0.1260245,0.1260204,0.1260164,0.1260125,0.1260087,0.126005,0.1260013,0.1259977,0.1259942,0.1259908,0.1259874,0.1259841,0.1259809
                                    },
                                    {
                                    0.3249197,0.2886751,0.2766707,0.2707223,0.2671809,0.2648345,0.2631669,0.2619211,0.2609553,0.2601848,0.2595559,0.2590327,0.2585909,0.2582127,0.2578853,0.2575992,0.257347,0.257123,0.2569228,0.2567428,0.2565799,0.256432,0.2562971,0.2561734,0.2560597,0.2559548,0.2558577,0.2557675,0.2556836,0.2556054,0.2555322,0.2554636,0.2553991,0.2553385,0.2552814,0.2552274,0.2551764,0.2551281,0.2550822,0.2550387,0.2549973,0.2549578,0.2549203,0.2548844,0.2548501,0.2548173,0.2547859,0.2547559,0.254727,0.2546993,0.2546727,0.2546472,0.2546226,0.2545989,0.2545761,0.2545541,0.2545328,0.2545123,0.2544925,0.2544734,0.2544549,0.254437,0.2544196,0.2544028,0.2543866,0.2543708,0.2543555,0.2543406,0.2543262,0.2543121,0.2542985,0.2542853,0.2542724,0.2542599,0.2542477,0.2542358,0.2542242,0.254213,0.254202,0.2541913,0.2541808,0.2541707,0.2541607,0.254151,0.2541415,0.2541323,0.2541232,0.2541144,0.2541058,0.2540973,0.2540891,0.254081,0.2540731,0.2540653,0.2540578,0.2540504,0.2540431,0.254036,0.254029,0.2540222
                                    },
                                    {
                                    0.5095254,0.4447496,0.4242016,0.4141633,0.4082287,0.4043134,0.4015382,0.3994693,0.3978678,0.3965915,0.3955506,0.3946856,0.3939553,0.3933306,0.3927902,0.392318,0.3919019,0.3915326,0.3912024,0.3909056,0.3906373,0.3903936,0.3901712,0.3899675,0.3897802,0.3896074,0.3894475,0.3892992,0.3891611,0.3890322,0.3889118,0.3887989,0.3886928,0.3885931,0.3884991,0.3884103,0.3883264,0.3882469,0.3881715,0.3880998,0.3880317,0.3879669,0.3879051,0.3878461,0.3877897,0.3877358,0.3876842,0.3876348,0.3875873,0.3875418,0.3874981,0.3874561,0.3874156,0.3873767,0.3873392,0.387303,0.3872681,0.3872344,0.3872019,0.3871704,0.38714,0.3871105,0.387082,0.3870544,0.3870277,0.3870017,0.3869766,0.3869521,0.3869284,0.3869054,0.386883,0.3868612,0.3868401,0.3868195,0.3867995,0.3867799,0.3867609,0.3867424,0.3867244,0.3867068,0.3866896,0.3866729,0.3866566,0.3866406,0.3866251,0.3866098,0.386595,0.3865805,0.3865663,0.3865524,0.3865389,0.3865256,0.3865126,0.3864999,0.3864874,0.3864753,0.3864633,0.3864516,0.3864402,0.386429
                                    },
                                    {
                                    0.7265425,0.6172134,0.5843897,0.5686491,0.5594296,0.5533809,0.5491097,0.5459338,0.5434802,0.541528,0.5399379,0.5386177,0.5375041,0.5365522,0.5357291,0.5350105,0.5343775,0.5338158,0.5333139,0.5328628,0.5324551,0.532085,0.5317473,0.5314381,0.5311538,0.5308916,0.530649,0.5304239,0.5302144,0.530019,0.5298363,0.5296651,0.5295044,0.5293532,0.5292107,0.5290761,0.5289489,0.5288285,0.5287142,0.5286057,0.5285025,0.5284042,0.5283106,0.5282212,0.5281359,0.5280542,0.5279761,0.5279012,0.5278294,0.5277605,0.5276942,0.5276306,0.5275694,0.5275104,0.5274536,0.5273989,0.527346,0.527295,0.5272457,0.5271981,0.5271521,0.5271075,0.5270644,0.5270226,0.5269821,0.5269428,0.5269047,0.5268678,0.5268319,0.526797,0.5267632,0.5267302,0.5266982,0.5266671,0.5266367,0.5266072,0.5265785,0.5265504,0.5265231,0.5264965,0.5264706,0.5264453,0.5264205,0.5263964,0.5263729,0.5263499,0.5263274,0.5263054,0.526284,0.526263,0.5262425,0.5262224,0.5262027,0.5261835,0.5261647,0.5261463,0.5261282,0.5261106,0.5260932,0.5260763
                                    },
                                    {
                                    1,0.8164966,0.7648923,0.7406971,0.7266868,0.7175582,0.7111418,0.7063866,0.7027221,0.6998121,0.6974453,0.6954829,0.6938293,0.6924171,0.6911969,0.6901323,0.6891951,0.6883638,0.6876215,0.6869545,0.686352,0.685805,0.6853063,0.6848496,0.68443,0.684043,0.683685,0.6833528,0.6830439,0.6827557,0.6824863,0.6822339,0.681997,0.6817741,0.6815641,0.6813658,0.6811784,0.6810009,0.6808326,0.6806727,0.6805207,0.680376,0.6802381,0.6801065,0.6799808,0.6798606,0.6797456,0.6796353,0.6795296,0.6794282,0.6793308,0.6792371,0.679147,0.6790602,0.6789766,0.678896,0.6788183,0.6787433,0.6786708,0.6786007,0.678533,0.6784674,0.678404,0.6783425,0.6782829,0.6782252,0.6781692,0.6781148,0.678062,0.6780107,0.6779609,0.6779125,0.6778654,0.6778196,0.677775,0.6777316,0.6776893,0.6776481,0.677608,0.6775688,0.6775307,0.6774935,0.6774571,0.6774217,0.677387,0.6773532,0.6773202,0.6772879,0.6772564,0.6772255,0.6771953,0.6771658,0.6771369,0.6771087,0.677081,0.6770539,0.6770274,0.6770014,0.676976,0.676951
                                    },
                                    {
                                    1.376382,1.06066,0.9784723,0.9409646,0.9195438,0.9057033,0.8960296,0.8888895,0.8834039,0.8790578,0.87553,0.8726093,0.8701515,0.8680548,0.866245,0.864667,0.863279,0.8620487,0.8609506,0.8599644,0.859074,0.8582661,0.8575296,0.8568555,0.8562362,0.8556652,0.8551372,0.8546475,0.854192,0.8537673,0.8533703,0.8529985,0.8526494,0.8523212,0.8520119,0.85172,0.851444,0.8511828,0.850935,0.8506998,0.8504762,0.8502633,0.8500604,0.8498668,0.8496819,0.8495051,0.8493359,0.8491738,0.8490184,0.8488692,0.848726,0.8485883,0.8484558,0.8483283,0.8482054,0.848087,0.8479727,0.8478625,0.8477559,0.847653,0.8475535,0.8474571,0.8473639,0.8472736,0.8471861,0.8471013,0.847019,0.8469391,0.8468616,0.8467863,0.8467131,0.846642,0.8465728,0.8465055,0.8464401,0.8463763,0.8463142,0.8462538,0.8461948,0.8461373,0.8460813,0.8460266,0.8459733,0.8459212,0.8458704,0.8458208,0.8457723,0.8457249,0.8456786,0.8456333,0.845589,0.8455457,0.8455033,0.8454618,0.8454212,0.8453814,0.8453425,0.8453044,0.845267,0.8452304
                                    },
                                    {
                                    1.962611,1.386207,1.249778,1.189567,1.155767,1.134157,1.119159,1.108145,1.099716,1.093058,1.087666,1.083211,1.079469,1.07628,1.073531,1.071137,1.069033,1.06717,1.065507,1.064016,1.06267,1.061449,1.060337,1.059319,1.058384,1.057523,1.056727,1.055989,1.055302,1.054662,1.054064,1.053504,1.052979,1.052485,1.052019,1.05158,1.051165,1.050772,1.050399,1.050046,1.04971,1.04939,1.049085,1.048794,1.048516,1.04825,1.047996,1.047752,1.047519,1.047295,1.04708,1.046873,1.046674,1.046483,1.046298,1.04612,1.045949,1.045783,1.045623,1.045469,1.04532,1.045175,1.045035,1.0449,1.044768,1.044641,1.044518,1.044398,1.044281,1.044169,1.044059,1.043952,1.043848,1.043747,1.043649,1.043554,1.043461,1.04337,1.043282,1.043195,1.043111,1.043029,1.042949,1.042871,1.042795,1.042721,1.042648,1.042577,1.042508,1.04244,1.042373,1.042308,1.042245,1.042183,1.042122,1.042062,1.042004,1.041947,1.041891,1.041836
                                    },
                                    {
                                    3.077684,1.885618,1.637744,1.533206,1.475884,1.439756,1.414924,1.396815,1.383029,1.372184,1.36343,1.356217,1.350171,1.34503,1.340606,1.336757,1.333379,1.330391,1.327728,1.325341,1.323188,1.321237,1.31946,1.317836,1.316345,1.314972,1.313703,1.312527,1.311434,1.310415,1.309464,1.308573,1.307737,1.306952,1.306212,1.305514,1.304854,1.30423,1.303639,1.303077,1.302543,1.302035,1.301552,1.30109,1.300649,1.300228,1.299825,1.299439,1.299069,1.298714,1.298373,1.298045,1.29773,1.297426,1.297134,1.296853,1.296581,1.296319,1.296066,1.295821,1.295585,1.295356,1.295134,1.29492,1.294712,1.294511,1.294315,1.294126,1.293942,1.293763,1.293589,1.293421,1.293256,1.293097,1.292941,1.29279,1.292643,1.2925,1.29236,1.292224,1.292091,1.291961,1.291835,1.291711,1.291591,1.291473,1.291358,1.291246,1.291136,1.291029,1.290924,1.290821,1.290721,1.290623,1.290527,1.290432,1.29034,1.29025,1.290161,1.290075
                                    },
                                    {
                                    3.442023,2.026081,1.741297,1.622578,1.557869,1.517235,1.489376,1.469095,1.453676,1.441559,1.431788,1.423742,1.417001,1.411273,1.406344,1.402059,1.3983,1.394974,1.392012,1.389357,1.386963,1.384793,1.382819,1.381013,1.379356,1.377831,1.376421,1.375114,1.3739,1.372769,1.371712,1.370723,1.369795,1.368923,1.368101,1.367327,1.366595,1.365902,1.365245,1.364622,1.364029,1.363466,1.362929,1.362417,1.361928,1.36146,1.361013,1.360585,1.360174,1.35978,1.359402,1.359038,1.358689,1.358352,1.358028,1.357716,1.357414,1.357123,1.356843,1.356571,1.356309,1.356055,1.35581,1.355572,1.355341,1.355118,1.354901,1.354691,1.354487,1.354289,1.354096,1.353909,1.353727,1.35355,1.353378,1.35321,1.353047,1.352888,1.352733,1.352582,1.352435,1.352291,1.352151,1.352014,1.35188,1.35175,1.351622,1.351498,1.351376,1.351257,1.351141,1.351027,1.350916,1.350807,1.3507,1.350596,1.350494,1.350394,1.350296,1.3502
                                    },
                                    {
                                    3.894743,2.189401,1.858928,1.722933,1.6493,1.603251,1.571766,1.548892,1.531527,1.517898,1.506917,1.497882,1.490317,1.483892,1.478367,1.473565,1.469354,1.46563,1.462314,1.459341,1.456663,1.454236,1.452027,1.450008,1.448155,1.446449,1.444873,1.443413,1.442056,1.440792,1.439611,1.438506,1.437469,1.436495,1.435578,1.434712,1.433895,1.433121,1.432388,1.431692,1.43103,1.430401,1.429801,1.42923,1.428684,1.428162,1.427663,1.427185,1.426726,1.426287,1.425864,1.425459,1.425068,1.424693,1.424331,1.423983,1.423646,1.423322,1.423009,1.422706,1.422413,1.42213,1.421856,1.421591,1.421334,1.421084,1.420843,1.420608,1.42038,1.420159,1.419945,1.419736,1.419533,1.419335,1.419143,1.418956,1.418774,1.418597,1.418424,1.418255,1.418091,1.417931,1.417775,1.417622,1.417473,1.417327,1.417185,1.417047,1.416911,1.416778,1.416648,1.416522,1.416397,1.416276,1.416157,1.416041,1.415927,1.415815,1.415706,1.415599
                                    },
                                    {
                                    4.473743,2.383378,1.995022,1.837524,1.75289,1.700205,1.664295,1.638266,1.618538,1.603075,1.590631,1.5804,1.571841,1.564575,1.558331,1.552907,1.548151,1.543947,1.540205,1.536852,1.533831,1.531094,1.528604,1.526329,1.524241,1.522319,1.520544,1.518899,1.51737,1.515947,1.514617,1.513373,1.512206,1.511109,1.510077,1.509103,1.508183,1.507312,1.506487,1.505704,1.50496,1.504252,1.503577,1.502934,1.50232,1.501733,1.501172,1.500634,1.500119,1.499624,1.499149,1.498693,1.498254,1.497832,1.497425,1.497033,1.496656,1.496291,1.495939,1.495598,1.495269,1.494951,1.494643,1.494345,1.494056,1.493775,1.493504,1.49324,1.492984,1.492736,1.492494,1.49226,1.492032,1.49181,1.491594,1.491384,1.491179,1.49098,1.490786,1.490596,1.490412,1.490232,1.490056,1.489884,1.489717,1.489554,1.489394,1.489238,1.489085,1.488936,1.488791,1.488648,1.488509,1.488372,1.488239,1.488108,1.48798,1.487854,1.487732,1.487611
                                    },
                                    {
                                    5.242184,2.620162,2.156239,1.971232,1.872678,1.811652,1.770207,1.740243,1.717579,1.699841,1.685583,1.673873,1.664085,1.655782,1.64865,1.642459,1.637033,1.632239,1.627972,1.624151,1.620709,1.617592,1.614757,1.612166,1.60979,1.607602,1.605582,1.603711,1.601972,1.600353,1.598841,1.597426,1.5961,1.594853,1.593679,1.592572,1.591527,1.590537,1.5896,1.58871,1.587865,1.58706,1.586294,1.585564,1.584866,1.5842,1.583562,1.582951,1.582366,1.581805,1.581265,1.580747,1.580249,1.57977,1.579308,1.578863,1.578434,1.57802,1.57762,1.577234,1.576861,1.576499,1.57615,1.575811,1.575483,1.575165,1.574857,1.574558,1.574268,1.573986,1.573712,1.573446,1.573187,1.572935,1.57269,1.572452,1.572219,1.571993,1.571773,1.571558,1.571349,1.571144,1.570945,1.570751,1.570561,1.570375,1.570194,1.570017,1.569844,1.569675,1.56951,1.569348,1.56919,1.569035,1.568884,1.568735,1.56859,1.568448,1.568309,1.568172
                                    },
                                    {
                                    6.313752,2.919986,2.353363,2.131847,2.015048,1.94318,1.894579,1.859548,1.833113,1.812461,1.795885,1.782288,1.770933,1.76131,1.75305,1.745884,1.739607,1.734064,1.729133,1.724718,1.720743,1.717144,1.713872,1.710882,1.708141,1.705618,1.703288,1.701131,1.699127,1.697261,1.695519,1.693889,1.69236,1.690924,1.689572,1.688298,1.687094,1.685954,1.684875,1.683851,1.682878,1.681952,1.681071,1.68023,1.679427,1.67866,1.677927,1.677224,1.676551,1.675905,1.675285,1.674689,1.674116,1.673565,1.673034,1.672522,1.672029,1.671553,1.671093,1.670649,1.670219,1.669804,1.669402,1.669013,1.668636,1.668271,1.667916,1.667572,1.667239,1.666914,1.6666,1.666294,1.665996,1.665707,1.665425,1.665151,1.664885,1.664625,1.664371,1.664125,1.663884,1.663649,1.66342,1.663197,1.662978,1.662765,1.662557,1.662354,1.662155,1.661961,1.661771,1.661585,1.661404,1.661226,1.661052,1.660881,1.660715,1.660551,1.660391,1.660234
                                    },
                                    {
                                    7.915815,3.319764,2.605427,2.332873,2.190958,2.104306,2.046011,2.004152,1.972653,1.948099,1.928427,1.912313,1.898874,1.887496,1.877739,1.869279,1.861875,1.85534,1.84953,1.844331,1.839651,1.835417,1.831567,1.828051,1.824828,1.821863,1.819126,1.816592,1.814238,1.812047,1.810002,1.808089,1.806295,1.80461,1.803024,1.801528,1.800116,1.79878,1.797514,1.796314,1.795173,1.794088,1.793054,1.792069,1.791128,1.79023,1.78937,1.788547,1.787758,1.787001,1.786275,1.785577,1.784906,1.78426,1.783638,1.783039,1.782461,1.781904,1.781366,1.780846,1.780343,1.779857,1.779386,1.77893,1.778489,1.778061,1.777646,1.777244,1.776853,1.776474,1.776106,1.775748,1.7754,1.775061,1.774732,1.774411,1.774099,1.773795,1.773498,1.77321,1.772928,1.772653,1.772385,1.772124,1.771869,1.771619,1.771376,1.771138,1.770906,1.770679,1.770456,1.770239,1.770027,1.769819,1.769615,1.769416,1.769221,1.76903,1.768842,1.768659
                                    },
                                    {
                                    10.57889,3.896425,2.95051,2.600762,2.421585,2.313263,2.240879,2.189155,2.150375,2.120234,2.096139,2.076441,2.060038,2.046169,2.034289,2.024,2.015002,2.007067,2.000017,1.993713,1.988041,1.982911,1.978249,1.973994,1.970095,1.966509,1.9632,1.960136,1.957293,1.954645,1.952175,1.949865,1.9477,1.945666,1.943752,1.941948,1.940244,1.938633,1.937106,1.935659,1.934283,1.932975,1.93173,1.930542,1.929409,1.928326,1.92729,1.926298,1.925348,1.924437,1.923562,1.922722,1.921914,1.921136,1.920388,1.919666,1.918971,1.9183,1.917652,1.917026,1.916421,1.915836,1.915269,1.914721,1.91419,1.913676,1.913176,1.912692,1.912222,1.911766,1.911323,1.910892,1.910474,1.910066,1.90967,1.909285,1.908909,1.908544,1.908187,1.90784,1.907501,1.907171,1.906849,1.906535,1.906228,1.905928,1.905636,1.90535,1.90507,1.904797,1.90453,1.904269,1.904013,1.903763,1.903519,1.903279,1.903045,1.902815,1.90259,1.90237
                                    },
                                    {
                                    15.89454,4.848732,3.481909,2.998528,2.756509,2.612242,2.516752,2.448985,2.398441,2.359315,2.32814,2.302722,2.281604,2.263781,2.24854,2.235358,2.223845,2.213703,2.204701,2.196658,2.189427,2.182893,2.176958,2.171545,2.166587,2.162029,2.157825,2.153935,2.150325,2.146966,2.143833,2.140904,2.138159,2.135581,2.133157,2.130871,2.128714,2.126674,2.124742,2.12291,2.12117,2.119515,2.11794,2.116438,2.115005,2.113636,2.112327,2.111073,2.109873,2.108721,2.107616,2.106555,2.105534,2.104552,2.103607,2.102696,2.101818,2.100971,2.100153,2.099363,2.098599,2.097861,2.097146,2.096455,2.095785,2.095135,2.094506,2.093895,2.093302,2.092727,2.092168,2.091625,2.091097,2.090584,2.090084,2.089598,2.089124,2.088663,2.088214,2.087777,2.08735,2.086934,2.086528,2.086131,2.085745,2.085367,2.084998,2.084638,2.084286,2.083942,2.083605,2.083276,2.082954,2.08264,2.082331,2.08203,2.081734,2.081445,2.081162,2.080884
                                    },
                                    {
                                    31.82052,6.964557,4.540703,3.746947,3.36493,3.142668,2.997952,2.896459,2.821438,2.763769,2.718079,2.680998,2.650309,2.624494,2.60248,2.583487,2.566934,2.55238,2.539483,2.527977,2.517648,2.508325,2.499867,2.492159,2.485107,2.47863,2.47266,2.46714,2.462021,2.457262,2.452824,2.448678,2.444794,2.44115,2.437723,2.434494,2.431447,2.428568,2.425841,2.423257,2.420803,2.41847,2.41625,2.414134,2.412116,2.410188,2.408345,2.406581,2.404892,2.403272,2.401718,2.400225,2.39879,2.39741,2.396081,2.394801,2.393568,2.392377,2.391229,2.390119,2.389047,2.388011,2.387008,2.386037,2.385097,2.384186,2.383302,2.382446,2.381615,2.380807,2.380024,2.379262,2.378522,2.377802,2.377102,2.37642,2.375757,2.375111,2.374482,2.373868,2.37327,2.372687,2.372119,2.371564,2.371022,2.370493,2.369977,2.369472,2.368979,2.368497,2.368026,2.367566,2.367115,2.366674,2.366243,2.365821,2.365407,2.365002,2.364606,2.364217
                                    }
                                 };
            if ((df < 101) && (df > 0))
            {
                if (Math.Abs(p - 0.10) < 0.000001)
                    return t_shuangce[0, df - 1];
                if (Math.Abs(p - 0.15) < 0.000001)
                    return t_shuangce[1, df - 1];
                if (Math.Abs(p - 0.20) < 0.000001)
                    return t_shuangce[2, df - 1];
                if (Math.Abs(p - 0.25) < 0.000001)
                    return t_shuangce[3, df - 1];
                if (Math.Abs(p - 0.30) < 0.000001)
                    return t_shuangce[4, df - 1];
                if (Math.Abs(p - 0.35) < 0.000001)
                    return t_shuangce[5, df - 1];
                if (Math.Abs(p - 0.40) < 0.000001)
                    return t_shuangce[6, df - 1];
                if (Math.Abs(p - 0.45) < 0.000001)
                    return t_shuangce[7, df - 1];
                if (Math.Abs(p - 0.50) < 0.000001)
                    return t_shuangce[8, df - 1];
                if (Math.Abs(p - 0.55) < 0.000001)
                    return t_shuangce[9, df - 1];
                if (Math.Abs(p - 0.60) < 0.000001)
                    return t_shuangce[10, df - 1];
                if (Math.Abs(p - 0.65) < 0.000001)
                    return t_shuangce[11, df - 1];
                if (Math.Abs(p - 0.70) < 0.000001)
                    return t_shuangce[12, df - 1];
                if (Math.Abs(p - 0.75) < 0.000001)
                    return t_shuangce[13, df - 1];
                if (Math.Abs(p - 0.80) < 0.000001)
                    return t_shuangce[14, df - 1];
                if (Math.Abs(p - 0.85) < 0.000001)
                    return t_shuangce[15, df - 1];
                if (Math.Abs(p - 0.90) < 0.000001)
                    return t_shuangce[16, df - 1];
                if (Math.Abs(p - 0.91) < 0.000001)
                    return t_shuangce[17, df - 1];
                if (Math.Abs(p - 0.92) < 0.000001)
                    return t_shuangce[18, df - 1];
                if (Math.Abs(p - 0.93) < 0.000001)
                    return t_shuangce[19, df - 1];
                if (Math.Abs(p - 0.94) < 0.000001)
                    return t_shuangce[20, df - 1];
                if (Math.Abs(p - 0.95) < 0.000001)
                    return t_shuangce[21, df - 1];
                if (Math.Abs(p - 0.96) < 0.000001)
                    return t_shuangce[22, df - 1];
                if (Math.Abs(p - 0.97) < 0.000001)
                    return t_shuangce[23, df - 1];
                if (Math.Abs(p - 0.98) < 0.000001)
                    return t_shuangce[24, df - 1];
                if (Math.Abs(p - 0.99) < 0.000001)
                    return t_shuangce[25, df - 1];
                else return MLR_polar.tinv(p, df);
            }
            else return 0;
        }

        //public static void getDataArray(ref double[] xArray, ref int[] vArray)
        //{
        //    DataSet myDataSet = DataBase.returnDS("select I,y,v from updown_detail where groupId=" + SysParam.GroupId.ToString() + " and id=" + SysParam.upDown_MastId.ToString() + " order by I");

        //    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
        //    {
        //        xArray[i] = (double)myDataSet.Tables[0].Rows[i]["y"];
        //        if (SysParam.store_upDown_Reverse == "是")
        //        {
        //            if ((int)myDataSet.Tables[0].Rows[i]["v"] == 0)
        //                vArray[i] = 1;
        //            else
        //                vArray[i] = 0;
        //        }
        //        else
        //        {
        //            vArray[i] = (int)myDataSet.Tables[0].Rows[i]["v"];
        //        }

        //    }

        //    myDataSet.Dispose();
        //}
        //public static void getDataArray(ref double[] xArray, ref int[] vArray, string GroupId, string MastId, out int xArrayLength)
        //{
        //    DataSet myDataSet = DataBase.returnDS("select I,y,v from updown_detail where groupId=" + GroupId + " and id=" + MastId + " order by I");
        //    xArrayLength = myDataSet.Tables[0].Rows.Count;
        //    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
        //    {
        //        xArray[i] = (double)myDataSet.Tables[0].Rows[i]["y"];
        //        if (SysParam.store_upDown_Reverse == "是")
        //        {
        //            if ((int)myDataSet.Tables[0].Rows[i]["v"] == 0)
        //                vArray[i] = 1;
        //            else
        //                vArray[i] = 0;
        //        }
        //        else
        //        {
        //            vArray[i] = (int)myDataSet.Tables[0].Rows[i]["v"];
        //        }

        //    }

        //    myDataSet.Dispose();
        //}

        //取变换前的值
        //public static void getDataArray(int xsti, ref double[] xArray, ref int[] vArray, string GroupId, string MastId, out int xArrayLength)
        //{

        //    DataSet myDataSet = DataBase.returnDS("select X,I,y,v from updown_detail where groupId=" + GroupId + " and id=" + MastId + " order by I");
        //    xArrayLength = myDataSet.Tables[0].Rows.Count;
        //    for (int i = 0; i < myDataSet.Tables[0].Rows.Count; i++)
        //    {
        //        xArray[i] = (double)myDataSet.Tables[0].Rows[i]["X"];
        //        if (SysParam.store_upDown_Reverse == "是")
        //        {
        //            if ((int)myDataSet.Tables[0].Rows[i]["v"] == 0)
        //                vArray[i] = 1;
        //            else
        //                vArray[i] = 0;
        //        }
        //        else
        //        {
        //            vArray[i] = (int)myDataSet.Tables[0].Rows[i]["v"];
        //        }

        //    }

        //    myDataSet.Dispose();
        //}


        //public static void getDataArrayLength(string GroupId, string MastId, out int xArrayLength)
        //{
        //    DataSet myDataSet = DataBase.returnDS("select I,y,v from updown_detail where groupId=" + GroupId + " and id=" + MastId + " order by I");
        //    xArrayLength = myDataSet.Tables[0].Rows.Count;

        //    myDataSet.Dispose();
        //}


        //public static void updateABM_db()
        //{

        //    DataBase.OperateDB("Delete from updown_ABM where id=" + SysParam.upDown_MastId.ToString() +
        //        " and groupId=" + SysParam.GroupId.ToString());

        //    for (int i = 0; i < SysParam.result_i.Length; i++)
        //    {
        //        DataBase.OperateDB("Insert into  updown_ABM(Id,groupId,i,i2,vi,mi,ivi,i2vi)values(" + SysParam.upDown_MastId.ToString() +
        //            "," + SysParam.GroupId.ToString() +
        //            "," + SysParam.result_i[i].ToString() + "," + (SysParam.result_i[i] * SysParam.result_i[i]).ToString() +
        //            "," + SysParam.vi[i].ToString() + "," + SysParam.mi[i].ToString() +
        //            "," + (SysParam.result_i[i] * SysParam.vi[i]).ToString() +
        //            "," + (SysParam.result_i[i] * SysParam.result_i[i] * SysParam.vi[i]).ToString() + ")");

        //    }
        //}

        //public static void getParamFromDb(int testId, int grpId)
        //{
        //    //  initdata();
        //    DataSet myDataSet = DataBase.returnDS("select * from " +
        //    " updown_master where id=" + testId.ToString());

        //    if (myDataSet.Tables[0].Rows.Count > 0)
        //    {
        //        //设置系统参数

        //        SysParam.store_upDown_product = myDataSet.Tables[0].Rows[0]["ProductName"].ToString();
        //        SysParam.store_upDown_Distribute = myDataSet.Tables[0].Rows[0]["Distribution"].ToString();
        //        SysParam.store_upDown_Reverse = myDataSet.Tables[0].Rows[0]["reverse"].ToString();
        //        SysParam.store_upDown_group = myDataSet.Tables[0].Rows[0]["multigroup"].ToString();
        //        SysParam.store_upDown_tradition = myDataSet.Tables[0].Rows[0]["tradition"].ToString();
        //        SysParam.mu_final = (double)myDataSet.Tables[0].Rows[0]["mu"];
        //        SysParam.sigma_final = (double)myDataSet.Tables[0].Rows[0]["sigma"];
        //        SysParam.sigmaMu = (double)myDataSet.Tables[0].Rows[0]["sigmamu"];
        //        SysParam.sigmaSigma = (double)myDataSet.Tables[0].Rows[0]["sigmasigma"];
        //        SysParam.updown_Precision = (double)myDataSet.Tables[0].Rows[0]["Precision"];
        //        SysParam.maxGroupId = (int)myDataSet.Tables[0].Rows[0]["testN"];
        //    }


        //    myDataSet = DataBase.returnDS("select * from " +
        //             " updown_group where id=" + testId.ToString() +
        //             " and groupId=" + grpId.ToString());

        //    //如果是多组的话，计算最后一组的结果
        //    //这里要注意区分多组和单组的情况
        //    if (myDataSet.Tables[0].Rows.Count > 0)
        //    {
        //        //设置系统参数
        //        try
        //        {
        //            SysParam.test_n = (int)myDataSet.Tables[0].Rows[0]["nj"];
        //            SysParam.store_upDown_x0 = (myDataSet.Tables[0].Rows[0]["x0"].ToString());
        //            //  y0 = (double)myDataSet.Tables[0].Rows[0]["y0"];
        //            SysParam.store_upDown_step = (myDataSet.Tables[0].Rows[0]["step"].ToString());
        //            // N = (int)myDataSet.Tables[0].Rows[0]["N"];


        //            SysParam.testCount = (int)myDataSet.Tables[0].Rows[0]["testCount"];


        //            SysParam.G = (double)myDataSet.Tables[0].Rows[0]["G"];
        //            SysParam.H = (double)myDataSet.Tables[0].Rows[0]["H"];
        //            SysParam.mu = (double)myDataSet.Tables[0].Rows[0]["mu"];
        //            SysParam.sigma = (double)myDataSet.Tables[0].Rows[0]["sigma"];
        //            SysParam.sigmaMu = (double)myDataSet.Tables[0].Rows[0]["sigmaMu"];
        //            SysParam.sigmaSigma = (double)myDataSet.Tables[0].Rows[0]["sigmaSigma"];
        //            //prec999 = mu + 3.090232 * sigma;
        //            //prec001 = mu - 3.090232 * sigma;
        //            SysParam.prec001 = (double)myDataSet.Tables[0].Rows[0]["prec001"];
        //            SysParam.prec999 = (double)myDataSet.Tables[0].Rows[0]["prec999"];
        //            // SysParam.maxGroupId = (int)myDataSet.Tables[0].Rows[0]["groupid"];
        //        }
        //        catch
        //        {
        //            // MessageBox.Show("函数getParamFromDb读取参数有误！");
        //        }
        //        SysParam.GroupId = grpId;
        //    }
        //    else
        //    {

        //    }

        //    myDataSet.Dispose();




        //}
        //public static void updateGH_db()
        //{
        //    DataBase.OperateDB("update updown_group set " +
        //               " nj=" + SysParam.test_n.ToString() + "," +
        //       " Gj=" + SysParam.G.ToString() + "," +
        //       " Hj=" + SysParam.H.ToString() + "," +
        //       " muj=" + SysParam.mu.ToString() + "," +
        //       " sigmaj=" + SysParam.sigma.ToString() + "," +
        //       " G=" + SysParam.G.ToString() + "," +
        //       " H=" + SysParam.H.ToString() + "," +
        //       " mu=" + SysParam.mu.ToString() + "," +
        //       " sigma=" + SysParam.sigma.ToString() + "," +
        //       " sigmaMu=" + SysParam.sigmaMu.ToString() + "," +
        //       " sigmaSigma=" + SysParam.sigmaSigma.ToString() + "," +
        //       " testCount=" + SysParam.testCount.ToString() + "," +
        //          " A=" + SysParam.A.ToString() + "," +
        //             " B=" + SysParam.B.ToString() + "," +
        //                " M=" + SysParam.M.ToString() + "," +
        //                   " bb=" + SysParam.b.ToString() + "," +
        //                      " rou=" + SysParam.p.ToString() + "," +
        //       " prec001=" + SysParam.prec001.ToString() + "," +
        //       " prec999=" + SysParam.prec999.ToString() +
        //       " where Id=" + SysParam.upDown_MastId.ToString() +
        //       " and groupId=" + SysParam.GroupId.ToString());

        //}

        //public static void getGHfromDb()
        //{


        //    DataSet MyDataSet = DataBase.returnDS("select groupId,testCount,nj,Gj,Hj,muj,sigmaj from updown_group where id=" +
        //       SysParam.upDown_MastId.ToString());

        //    //SysParam.sumcount = (int)MyDataSet.Tables[0].Rows[0]["sumcount"];

        //    if (MyDataSet.Tables[0].Rows.Count > 0 && MyDataSet != null)
        //    {

        //        try
        //        {
        //            for (int i = 0; i < MyDataSet.Tables[0].Rows.Count; i++)
        //            {
        //                SysParam.nj[i] = (int)MyDataSet.Tables[0].Rows[i]["nj"];
        //                SysParam.Gj[i] = (double)MyDataSet.Tables[0].Rows[i]["Gj"];
        //                SysParam.Hj[i] = (double)MyDataSet.Tables[0].Rows[i]["Hj"];
        //                SysParam.muj[i] = (double)MyDataSet.Tables[0].Rows[i]["muj"];
        //                SysParam.sigmaj[i] = (double)MyDataSet.Tables[0].Rows[i]["sigmaj"];
        //            }

        //        }
        //        catch
        //        {
        //            // MessageBox.Show("没有中间结果生成！");
        //            return;
        //        }

        //    }

        //    MyDataSet.Dispose();

        //}
        //分多组计算结果
        //public static void updatemaster()
        //{
        //    try
        //    {
        //        DataBase.OperateDB("update updown_master set " +
        //                   " testN=" + SysParam.maxGroupId.ToString() + "," +
        //           " mu=" + SysParam.mu_final.ToString() + "," +
        //           " sigma=" + SysParam.sigma_final.ToString() + "," +
        //           " sigmaMu=" + SysParam.sigmaMu_final.ToString() + "," +
        //           " prec001=" + SysParam.prec001.ToString() + "," +
        //           " prec999=" + SysParam.prec999.ToString() + "," +
        //           " sigmaSigma=" + SysParam.sigmaSigma_final.ToString() + " where Id=" + SysParam.upDown_MastId.ToString());

        //    }
        //    catch
        //    { }
        //}



    }
}