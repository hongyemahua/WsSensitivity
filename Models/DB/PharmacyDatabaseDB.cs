using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models.DB
{
    public class WsSensitivityDB : DbContext
    {
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Role> Role { get; set; }

        public DbSet<LangleyExperimentTable> LangleyExperimentTable { get; set; }
        public DbSet<LangleyDataTable> LangleyDataTable { get; set; }

    }
}