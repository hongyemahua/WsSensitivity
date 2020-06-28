using WsSensitivity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        #endregion

        #region 兰利数据表操作
        public abstract bool Insert(LangleyDataTable ldt);

        public abstract List<LangleyDataTable> GetAllLangleyDataTable(int id);

        #endregion
    }
}