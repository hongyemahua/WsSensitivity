using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Controllers.UpDowunMethod.Functions
{
    public class 对数ln : Function
    {
        public override double FunctionAlgorithm(double x0)
        {
            return Math.Log(x0);
        }
    }
}