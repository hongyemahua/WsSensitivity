using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WsSensitivity.Models
{
    public class UpDownDataTable
    {
        //数据表格
        [Key]
        [DisplayName("数据表Id")]
        public int Id { get; set; }

        [DisplayName("实验组Id")]
        public int dtup_DataTableId { get; set; }

        [DisplayName("刺激量")]
        public double dtup_Initialstimulus { get; set; }

        [DisplayName("响应")]
        public int dtup_response { get; set; }

        [DisplayName("标准刺激量")]
        public double dtup_Standardstimulus { get; set; }
    }
}