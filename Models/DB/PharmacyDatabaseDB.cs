using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WsSensitivity.Models.UpDown;

namespace WsSensitivity.Models.DB
{
    public class WsSensitivityDB : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<LangleyExperimentTable> LangleyExperimentTable { get; set; }
        public DbSet<LangleyDataTable> LangleyDataTable { get; set; }

        public DbSet<DoptimizeDataTable> DoptimizeDataTable { get; set; }
        public DbSet<DoptimizeExperimentTable> DoptimizeExperimentTable { get; set; }

        //public DbSet<UpDownExperiment> UpDownExperiment { get; set; }
        //public DbSet<UpDownGroup> UpDownGroup { get; set; }
        //public DbSet<UpDownDataTable> UpDownDataTable { get; set; }

    }
}