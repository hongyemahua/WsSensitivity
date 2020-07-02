using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class LangleyDataTable
    {
        [Key]
        [DisplayName("实验数据表主键")]
        public int ldt_Id { get; set; }
        [DisplayName("实验Id")]
        public int ldt_ExperimentTableId { get; set; }
        [DisplayName("刺激量")]
        public double ldt_StimulusQuantity { get; set; }
        [DisplayName("编号")]
        public int ldt_Number { get; set; }
        [DisplayName("响应")]
        public int ldt_Response { get; set; }
        [DisplayName("均值")]
        public double ldt_Mean { get; set; }
        [DisplayName("标准差")]
        public double ldt_StandardDeviation { get; set; }
        [DisplayName("均值方差")]
        public double ldt_MeanVariance { get; set; }
        [DisplayName("标准差方差")]
        public double ldt_StandardDeviationVariance { get; set; }
        [DisplayName("备注")]
        public double ldt_Covmusigma { get; set; }
        [DisplayName("备注1")]
        public string ldt_Note1 { get; set; }
    }
}