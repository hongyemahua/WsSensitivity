using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmReconstruct
{
    public abstract class LiftingMethodStandardSelection
    {
        public abstract double InverseProcessValue(double value);
        public abstract double GetAvgValue(double value);
        public abstract double ProcessValue(double value);
        public double[] InverseProcessArray(double[] value)
        {
            double[] ret = new double[value.Length];
            for (int i = 0; i < value.Length; i++)
            {
                ret[i] = InverseProcessValue(value[i]);
            }
            return ret;
        }
        public abstract string MethodStandard();
        public abstract bool IsStandard();
    }

    public class LiftingStandard : LiftingMethodStandardSelection
    {
        public override double GetAvgValue(double value) => value;

        public override double InverseProcessValue(double value) => value;

        public override string MethodStandard() => "标准";

        public override double ProcessValue(double value) => value;

        public override bool IsStandard() => true;
    }

    public class LiftingLn : LiftingMethodStandardSelection
    {
        public override double GetAvgValue(double value)
        {
            if (Math.Abs(value) < Math.Pow(10, -10))
                value = 0;
            else
                value = Math.Exp(value);
            return value;
        }

        public override double InverseProcessValue(double value) => Math.Log(value);

        public override string MethodStandard() => "Ln";

        public override double ProcessValue(double value) => Math.Exp(value);

        public override bool IsStandard() => false;
    }

    public class LiftingLog : LiftingMethodStandardSelection
    {
        public override double GetAvgValue(double value)
        {
            if (Math.Abs(value) < Math.Pow(10, -10))
                value = 0;
            else
                value = Math.Pow(10, value);
            return value;
        }

        public override double InverseProcessValue(double value) => Math.Log10(value);

        public override string MethodStandard() => "Log";

        public override double ProcessValue(double value) => Math.Pow(10, value);

        public override bool IsStandard() => false;
    }

    public class LiftingPow : LiftingMethodStandardSelection
    {
        public static double pow;
        public  LiftingPow(double power) => pow = power;

        public override double GetAvgValue(double value) => Math.Pow(value, 1 / pow);

        public override double InverseProcessValue(double value) => Math.Pow(value,pow);

        public override string MethodStandard() => "幂 = "+ pow +"";

        public override double ProcessValue(double value) => Math.Pow(value, 1 / pow);

        public override bool IsStandard() => false;
    }
}
