using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class Langlie
    {
        public static double[] langlie_sigma_norm_correct_xArrayLength ={
            10,13,14,15,16,17,18,19,20,25,28,30,
            32,34,35,38,40,43,46,50,53,58,63,67,75,85
        };
        public static double[] langlie_sigma_norm_correct_value ={
           1.36,1.35,1.33,1.31,1.30,1.29,1.28,1.27,1.26,1.25,1.24,1.23,2.22,1.21,1.20,1.19,1.18,
            1.17,1.16,1.15,1.14,1.13,1.12,1.11,1.10,1.09
        };
        public static double[] langlie_sigma_logis_correct_xArrayLength ={
            10,11,12,13,14,15,16,17,18,19,20,21,22,
            23,24,26,27,29,30,32,34,37,40,45,50,56
        };
        public static double[] langlie_sigma_logis_correct_value ={
           1.41,1.38,1.36,1.35,1.34,1.32,1.31,1.29,1.28,1.26,1.25,1.24,1.23,
           1.22,1.21,1.20,1.19,1.18,1.17,1.16,1.15,1.14,1.13,1.12,1.11,1.10
        };

        public static int getIndexOfArray(int x, double[] array, double frac)
        {
            int k = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (Math.Abs(array[i] - x) <= frac)
                    return i;
            }
            if (x < array[0])
                k = -1;
            if (x > array[array.Length - 1])
                k = array.Length - 1;
            return k;

        }

        public static double get_langlie_sigma_norm_correct(int xArrayLength)
        {
            if (xArrayLength < 10) return 1.4;
            if (xArrayLength > 85) return 1.05;
            if (xArrayLength == 10) return 1.36;
            if (xArrayLength == 85) return 1.09;

            //int x = 0;
            //int y = 0;
            int n, n1, n0;
            double x1 = 0, x0 = 0;

            n = getIndexOfArray(xArrayLength, langlie_sigma_norm_correct_xArrayLength, 0.0001);
            if (n == -1)
            {
                n1 = xArrayLength;
                n0 = xArrayLength;
                while (getIndexOfArray(n0, langlie_sigma_norm_correct_xArrayLength, 0.0001) == -1)
                {
                    n0 = n0 - 1;
                }
                while (getIndexOfArray(n1, langlie_sigma_norm_correct_xArrayLength, 0.0001) == -1)
                {
                    n1 = n1 + 1;
                }
                x1 = langlie_sigma_norm_correct_value[getIndexOfArray(n0, langlie_sigma_norm_correct_xArrayLength, 0.0001)];
                x0 = langlie_sigma_norm_correct_value[getIndexOfArray(n1, langlie_sigma_norm_correct_xArrayLength, 0.0001)];

            }
            else
            {
                x1 = langlie_sigma_norm_correct_value[getIndexOfArray(xArrayLength, langlie_sigma_norm_correct_xArrayLength, 0.0001)];
                x0 = langlie_sigma_norm_correct_value[getIndexOfArray(xArrayLength, langlie_sigma_norm_correct_xArrayLength, 0.0001)];
                n1 = 1;
                n0 = 0;
            }

            return Math.Round(x1 - (x1 - x0) * (xArrayLength - n0) / (n1 - n0), 3);

        }

        public static double get_langlie_sigma_logis_correct(int xArrayLength)
        {
            if (xArrayLength < 10) return 1.41;
            if (xArrayLength > 56) return 1.10;
            if (xArrayLength == 10) return 1.41;
            if (xArrayLength == 56) return 1.10;

            //int x = 0;
            //int y = 0;
            int n, n1, n0;
            double x1 = 0, x0 = 0;

            n = getIndexOfArray(xArrayLength, langlie_sigma_logis_correct_xArrayLength, 0.0001);
            if (n == -1)
            {
                n1 = xArrayLength;
                n0 = xArrayLength;
                while (getIndexOfArray(n0, langlie_sigma_logis_correct_xArrayLength, 0.0001) == -1)
                {
                    n0 = n0 - 1;
                }
                while (getIndexOfArray(n1, langlie_sigma_logis_correct_xArrayLength, 0.0001) == -1)
                {
                    n1 = n1 + 1;
                }
                x1 = langlie_sigma_logis_correct_value[getIndexOfArray(n0, langlie_sigma_logis_correct_xArrayLength, 0.0001)];
                x0 = langlie_sigma_logis_correct_value[getIndexOfArray(n1, langlie_sigma_logis_correct_xArrayLength, 0.0001)];

            }
            else
            {
                x1 = langlie_sigma_logis_correct_value[getIndexOfArray(xArrayLength, langlie_sigma_logis_correct_xArrayLength, 0.0001)];
                x0 = langlie_sigma_logis_correct_value[getIndexOfArray(xArrayLength, langlie_sigma_logis_correct_xArrayLength, 0.0001)];
                n1 = 1;
                n0 = 0;
            }

            return Math.Round(x1 - (x1 - x0) * (xArrayLength - n0) / (n1 - n0), 3);

        }
    }
}