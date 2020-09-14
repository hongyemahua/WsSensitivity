using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models
{
    public class Admin
    {
        [Key]
        [DisplayName("用户Id")]
        public int id { get; set; }
        [DisplayName("昵称")]
        public string name { get; set; }

        [DisplayName("性别")]
        public string sex { get; set; }

        [DisplayName("电话")]
        public long phone { get; set; }

        [DisplayName("邮箱")]
        public string email { get; set; }

        [DisplayName("角色")]
        public int role { get; set; }

        [DisplayName("密码")]
        public string pass { get; set; }

        [DisplayName("状态")]
        public string state { get; set; }

        [NotMapped]
        public string rolename;
        [NotMapped]
        public int number;
    }
}