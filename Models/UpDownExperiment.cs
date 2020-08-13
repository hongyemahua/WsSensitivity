using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WsSensitivity.Models
{
    public class UpDownExperiment
    {
        //升降法实验表
        [Key]
        [DisplayName("实验Id")]
        public int id { get; set; }
        [DisplayName("产品名称")]
        public string udt_ProdectName { get; set; }

        [DisplayName("初始刺激量")]
        public double udt_Initialstimulus { get; set; }

        [DisplayName("步长d")]
        public double udt_Stepd { get; set; }

        [DisplayName("仪器分辨率")]
        public double udt_Instrumentresolution { get; set; }

        [DisplayName("分布状态")]
        public int udt_Distribution { get; set; }

        [DisplayName("标准状态")]
        public int udt_Standardstate { get; set; }

        [DisplayName("幂值")]
        public double udt_Power { get; set; }

        [DisplayName("分组状态")]
        public int udt_Groupingstate { get; set; }

        [DisplayName("翻转响应")]
        public int udt_Flipresponse { get; set; }

        [DisplayName("技术条件")]
        public string udt_Technicalconditions { get; set; }

        [DisplayName("创建时间")]
        public DateTime udt_Creationtime { get; set; }
        
    }
}