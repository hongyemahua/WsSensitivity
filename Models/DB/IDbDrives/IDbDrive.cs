using System;
using System.Collections.Generic;

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

        public abstract List<LangleyExperimentTable> GetAllLangleyExperimentTables(int admin_id);

        public abstract LangleyExperimentTable GetLangleyExperimentTable(int id);

        public abstract List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName, DateTime startTime, DateTime endTime,int admin_id);

        public abstract List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName, int admin_id);

        public abstract bool Delete(LangleyExperimentTable let);

        public abstract bool Update(LangleyExperimentTable let);

        #endregion

        #region 兰利数据表操作
        public abstract bool Insert(LangleyDataTable ldt);

        public abstract List<LangleyDataTable> GetAllLangleyDataTable(int id);

        public abstract bool Update(LangleyDataTable ldt);

        public abstract bool Delete(LangleyDataTable ldt);

        #endregion

        #region D优化法实验表操作
        public abstract bool Insert(DoptimizeExperimentTable det);
        public abstract DoptimizeExperimentTable GetDoptimizeExperimentTable(int id);
        public abstract bool Update(DoptimizeExperimentTable det);
        public abstract List<DoptimizeExperimentTable> GetAllDoptimizeExperimentTables(int admin_id);
        public abstract List<DoptimizeExperimentTable> QueryDoptimizeExperimentTable(string productName, DateTime startTime, DateTime endTime,int admin_id);
        public abstract List<DoptimizeExperimentTable> QueryDoptimizeExperimentTable(string productName,int admin_id);
        public abstract bool Delete(DoptimizeExperimentTable det);
        #endregion

        #region D优化数据表操作
        public abstract bool Insert(DoptimizeDataTable ddt);
        public abstract List<DoptimizeDataTable> GetDoptimizeDataTables(int id);
        public abstract bool Update(DoptimizeDataTable ddt);
        public abstract DoptimizeDataTable GetDoptimizeDataTable(int id);
        public abstract bool Delete(DoptimizeDataTable ddt);
        #endregion

        #region 升降法表数据操作
        public abstract bool Insert(UpDownExperiment experiment);
        public abstract bool Insert(UpDownGroup group);
        public abstract bool Insert(UpDownDataTable upDownDataTable);
        public abstract bool Update(UpDownExperiment upDownExperiment);
        public abstract bool Delete(UpDownDataTable upDownDataTable);
        public abstract bool Delete(UpDownGroup upDownGroup);
        public abstract bool Delete(UpDownExperiment upDownExperiment);
        public abstract UpDownGroup GetDownGroup(int id);
        public abstract UpDownExperiment GetUpDownExperiment(int id);
        public abstract List<UpDownDataTable> GetUpDownDataTables(int id);
        public abstract List<UpDownView> GetUpDownViews(int id);
        public abstract List<UpDownView> GetUpDownViews_UDEID(int id);
        public abstract List<UpDownGroup> GetUpDownGroups(int id);
        public abstract List<UpDownExperiment> GetUpDownExperiments(int admin_id);
        public abstract List<UpDownExperiment> QueryExperimentTable(string productName, DateTime startTime, DateTime endTime,int admin_id);
        public abstract List<UpDownExperiment> QueryExperimentTable(string productName,int admin_id);
        #endregion
    }
}