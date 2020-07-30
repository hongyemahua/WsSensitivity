using WsSensitivity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WsSensitivity.Models.IDbDrives
{
    public abstract class IDbDrive
    {
        #region 管理员操作
        public abstract bool Insert(Admin admin);

        public abstract bool Delete(Admin admin);

        public abstract bool Udpdate(Admin admin);

        public abstract List<Admin> GetAllAdmins();

        public abstract Admin FindAdmin(int id);

        public abstract List<Admin> QueryAdmins(string name);

        public abstract List<Admin> AccurateQueryAdmins(string name);

        public abstract Admin AdminLogin(Admin admin);
        #endregion

        #region 角色操作
        public abstract bool Insert(Role role);

        public abstract bool Delete(Role role);

        public abstract bool Udpdate(Role role);

        public abstract List<Role> GetAllRoles();

        public abstract Role FindRole(int id);

        public abstract List<Role> QueryRoles(string rolename);

        public abstract List<Role> AccurateQueryRoles(string rolename);
        #endregion

        #region 兰利实验表操作
        public abstract bool Insert(LangleyExperimentTable let);

        public abstract List<LangleyExperimentTable> GetAllLangleyExperimentTables();

        public abstract LangleyExperimentTable GetLangleyExperimentTable(int id);

        public abstract List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName, DateTime startTime, DateTime endTime);

        public abstract List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName);

        public abstract bool Delete(LangleyExperimentTable let);

        public abstract bool Update(LangleyExperimentTable let);

        #endregion

        #region 兰利数据表操作
        public abstract bool Insert(LangleyDataTable ldt);

        public abstract List<LangleyDataTable> GetAllLangleyDataTable(int id);

        public abstract bool UpDate(LangleyDataTable ldt);

        public abstract bool Delete(LangleyDataTable ldt);

        #endregion

        #region D优化法实验表操作
        public abstract bool Insert(DoptimizeExperimentTable det);
        public abstract DoptimizeExperimentTable GetDoptimizeExperimentTable(int id);
        #endregion

        #region D优化数据表操作
        public abstract bool Insert(DoptimizeDataTable ddt);
        #endregion
    }
}