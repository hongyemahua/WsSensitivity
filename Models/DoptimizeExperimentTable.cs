using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class DoptimizeExperimentTable
    {
        [Key]
        [DisplayName("D优化法实验表主键")]
        public int det_Id { get; set; }
        [DisplayName("产品名称")]
        public string det_ProductName { get; set; }
        [DisplayName("仪器精度")]
        public double det_PrecisionInstruments { get; set; }
        [DisplayName("刺激量上限")]
        public double det_StimulusQuantityCeiling { get; set; }
        [DisplayName("刺激量下限")]
        public double det_StimulusQuantityFloor { get; set; }
        [DisplayName("标准差预估值")]
        public double det_StandardDeviationEstimate { get; set; }
        [DisplayName("幂")]
        public string let_Power { get; set; }
        [DisplayName("分布状态")]
        public int let_DistributionState { get; set; }
        [DisplayName("标准状态")]
        public int let_StandardState { get; set; }
        [DisplayName("翻转响应")]
        public int let_FlipTheResponse { get; set; }
        [DisplayName("技术条件")]
        public string let_TechnicalConditions { get; set; }
        [DisplayName("日期")]
        public DateTime let_ExperimentalDate { get; set; }
    }
}