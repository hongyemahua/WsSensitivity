using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class Interval
    {
        public double Up { get; set; }
        public double Down { get; set; }
    }

    public class IntervalEstimation
    {
        public Interval Mu { get; set; } = new Interval();
        public Interval Sigma { get; set; } = new Interval();
        public Interval Confidence { get; set; } = new Interval();
        public static IntervalEstimation Parse(double[] finalResult)
        {
            IntervalEstimation ret = new IntervalEstimation();
            ret.Confidence.Down = finalResult[5];
            ret.Confidence.Up = finalResult[4];
            ret.Mu.Down = finalResult[1];
            ret.Mu.Up = finalResult[0];
            ret.Sigma.Down = finalResult[3];
            ret.Sigma.Up = finalResult[2];
            return ret;
        }
    }


}