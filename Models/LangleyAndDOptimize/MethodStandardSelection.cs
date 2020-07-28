using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public abstract class LangleyMethodStandardSelection
    {
        public abstract double ProcessValue(double value);
        public abstract double InverseProcessValue(double value);
        public double[] InverseProcessArray(double[] values)
        {
            var ret = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                ret[i] = InverseProcessValue(values[i]);
            }

            return ret;
        }

        public abstract  double GetAvgValue(double value);

        public abstract string StandardSelection();
    }

    public class Standard : LangleyMethodStandardSelection
    {
        public override string StandardSelection() => "标准";

        public override double GetAvgValue(double value) => value;

        public override double InverseProcessValue(double value) => value;

        public override double ProcessValue(double value) => value;

    }

    public class Ln : LangleyMethodStandardSelection
    {
        public override string StandardSelection() => "Ln";

        public override double GetAvgValue(double value)
        {
            if (Math.Abs(value) < Math.Pow(10, -10))
                value = 0;
            else
                value = Math.Exp(value);
            return value;
        }

        public override double InverseProcessValue(double value) => Math.Log(value);

        public override double ProcessValue(double value) => Math.Exp(value);
    }

    public class Log : LangleyMethodStandardSelection
    {
        public override string StandardSelection() => "Log10";

        public override double GetAvgValue(double value)
        {
            if (Math.Abs(value) < Math.Pow(10, -10))
                value = 0;
            else
                value = Math.Pow(10, value);
            return value;
        }

        public override double InverseProcessValue(double value) => Math.Log10(value);

        public override double ProcessValue(double value) => Math.Pow(10, value);

    }

    public class Pow : LangleyMethodStandardSelection
    {
        public static double pow;
        public Pow(double power)
        {
            pow = power;
        }

        public override string StandardSelection() => "幂 = "+pow+"";

        public override double GetAvgValue(double value)
        {
            return Math.Pow(value, 1 / pow);
        }

        public override double InverseProcessValue(double value) => Math.Pow(value, pow);

        public override double ProcessValue(double value) => Math.Pow(value, 1 / pow);

    }
}