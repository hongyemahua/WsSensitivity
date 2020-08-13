using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WsSensitivity.Models
{
    public class UpDownGroup
    {
        //升降组表
        [Key]
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("实验id")]
        public int dudt_ExperimentId { get; set; }
        [DisplayName("步长")]
        public double dudt_Stepd { get;set; }
    }
}