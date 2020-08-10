using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Controllers.UpDowunMethod.Functions
{
    public class 幂 : Function
    {
        private double pow;
        public 幂(double pow) 
        {
            this.pow = pow;  
        }
        public override double FunctionAlgorithm(double x0)
        {
            throw new NotImplementedException();
        }
    }
}