using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class UpDownView
    {
        [Key]
        public int uddt_Id { get; set; }
        public int udg_Id { get; set; }
        public int dudt_ExperimentId { get; set; }
        public double dudt_Stepd { get; set; }
        public double dtup_Initialstimulus { get; set; }
        public int dtup_response { get; set; }
        public double dtup_Standardstimulus { get; set; }
    }
}