using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WsSensitivity.Models
{
    public class DoptimizeDataTable
    {
        [Key]
        [DisplayName("实验数据表主键")]
        public int ddt_Id { get; set; }
        [DisplayName("实验Id")]
        public int ddt_ExperimentTableId { get; set; }
        [DisplayName("刺激量")]
        public double ddt_StimulusQuantity { get; set; }
        [DisplayName("编号")]
        public int ddt_Number { get; set; }
        [DisplayName("响应")]
        public int ddt_Response { get; set; }
        [DisplayName("均值")]
        public double ddt_Mean { get; set; }
        [DisplayName("标准差")]
        public double ddt_StandardDeviation { get; set; }
        [DisplayName("SigmaGuess")]
        public double ddt_SigmaGuess { get; set; }
        [DisplayName("均值方差")]
        public double ddt_MeanVariance { get; set; }
        [DisplayName("标准差方差")]
        public double ddt_StandardDeviationVariance { get; set; }
        [DisplayName("备注")]
        public double ddt_Covmusigma { get; set; }
        [DisplayName("备注1")]
        public string ddt_Note1 { get; set; }
    }
}