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

    }
}