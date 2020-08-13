using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models.UpDown
{
    public class NewGroupUpDownTable
    {
        public int dudt_ExperimentId { get; set; }//实验表Id

        public double dudt_Stepd { get; set; }//组步长

        public double dtup_Initialstimulus { get; set; }//分组的初始刺激量
    }
}