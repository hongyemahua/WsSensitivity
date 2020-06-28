using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class Role
    {
        [Key]
        [DisplayName("角色Id")]
        public int id { get; set; }
        [DisplayName("角色名称")]
        public string rolename { get; set; }

        [DisplayName("权限")]
        public string limit { get; set; }

        [DisplayName("备注说明")]
        public string descr { get; set; }

        [NotMapped]
        public string admin = "close";
        [NotMapped]
        public string langley = "close";
        [NotMapped]
        public string liftingMethod = "close";
        [NotMapped]
        public string Doptimize = "close";
        [NotMapped]
        public string mysetting = "close";
        [NotMapped]
        public string about = "close";

    }
}