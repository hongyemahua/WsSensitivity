using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Controllers.UpDowunMethod.Functions
{
    public class 对数log : Function
    {
        public override double FunctionAlgorithm(double x0)
        {
            return Math.Log10(x0);
        }
    }
}