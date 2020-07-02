using WsSensitivity.Models;
using WsSensitivity.Models.DB;
using WsSensitivity.Models.IDbDrives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models.IDbDrives
{
    public class LingImp : IDbDrive
    {
        WsSensitivityDB db = new WsSensitivityDB();
        #region 管理员操作
        public override bool Delete(Admin admin)
        {
            Admin modle = db.Admin.FirstOrDefault(m => m.id == admin.id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.Admin.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Insert(Admin admin)
        {
            try
            {
                db.Admin.Add(admin);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Udpdate(Admin admin)
        {
            try
            {
                var entry = db.Set<Admin>().Find(admin.id);
                if (entry != null)
                {
                    db.Entry<Admin>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.Admin.Attach(admin);
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override List<Admin> GetAllAdmins()
        {
            List<Admin> admins = new List<Admin>();
            admins = db.Admin.ToList();
            return admins;
        }

        public override Admin FindAdmin(int id)
        {
            Admin admin = db.Admin.Find(id);
            return admin;
        }

        public override Admin AdminLogin(Admin admin)
        {
            Admin loginuser = db.Admin.FirstOrDefault(u => u.name == admin.name && u.pass == admin.pass);
            return loginuser;
        }

        public override List<Admin> QueryAdmins(string name)
        {
            List<Admin> users = new List<Admin>();
            users = db.Admin.Where(m => m.name.Contains(name)).ToList();
            return users;
        }

        public override List<Admin> AccurateQueryAdmins(string name)
        {
            List<Admin> users = new List<Admin>();
            users = db.Admin.Where(m => m.name == name).ToList();
            return users;
        }
        #endregion

        #region 角色管理
        public override bool Delete(Role role)
        {
            Role modle = db.Role.FirstOrDefault(m => m.id == role.id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.Role.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Insert(Role role)
        {
            try
            {
                db.Role.Add(role);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Udpdate(Role role)
        {
            try
            {
                var entry = db.Set<Role>().Find(role.id);
                if (entry != null)
                {
                    db.Entry<Role>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.Role.Attach(role);
                db.Entry(role).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();
            roles = db.Role.ToList();
            return roles;
        }

        public override Role FindRole(int id)
        {
            Role role = db.Role.Find(id);
            return role;
        }

        public override List<Role> QueryRoles(string rolename)
        {
            List<Role> roles = new List<Role>();
            roles = db.Role.Where(m => m.rolename.Contains(rolename)).ToList();
            return roles;
        }

        public override List<Role> AccurateQueryRoles(string rolename)
        {
            List<Role> roles = new List<Role>();
            roles = db.Role.Where(m => m.rolename == rolename).ToList();
            return roles;
        }


        #endregion

        #region 兰利实验表操作
        public override bool Insert(LangleyExperimentTable let)
        {
            try
            {
                db.LangleyExperimentTable.Add(let);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override List<LangleyExperimentTable> GetAllLangleyExperimentTables()
        {
            List<LangleyExperimentTable> lets = db.LangleyExperimentTable.ToList();
            return lets;
        }

        public override bool Delete(LangleyExperimentTable let)
        {
            LangleyExperimentTable modle = db.LangleyExperimentTable.FirstOrDefault(m => m.let_Id == let.let_Id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.LangleyExperimentTable.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public override bool Update(LangleyExperimentTable let)
        {
            try
            {
                var entry = db.Set<LangleyExperimentTable>().Find(let.let_Id);
                if (entry != null)
                {
                    db.Entry<LangleyExperimentTable>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.LangleyExperimentTable.Attach(let);
                db.Entry(let).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override LangleyExperimentTable GetLangleyExperimentTable(int let_id)
        {
            LangleyExperimentTable let = db.LangleyExperimentTable.Where(m => m.let_Id == let_id).First();
            return let;
        }


        public override List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName, DateTime startTime, DateTime endTime)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            lets = db.LangleyExperimentTable.Where(m => m.let_ProductName.Contains(productName) && DateTime.Compare(startTime,m.let_ExperimentalDate)<= 0 && DateTime.Compare(endTime,m.let_ExperimentalDate) >= 0  ).ToList();
            return lets;
        }

        public override List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            lets = db.LangleyExperimentTable.Where(m => m.let_ProductName.Contains(productName)).ToList();
            return lets;
        }
        #endregion

        #region 兰利数据表操作
        public override bool Insert(LangleyDataTable ldt)
        {
            try
            {
                db.LangleyDataTable.Add(ldt);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override List<LangleyDataTable> GetAllLangleyDataTable(int id)
        {
            List<LangleyDataTable> ldts = new List<LangleyDataTable>();
            ldts = db.LangleyDataTable.Where(m => m.ldt_ExperimentTableId == id).ToList();
            return ldts;
        }

        public override bool UpDate(LangleyDataTable ldt)
        {
            try
            {
                var entry = db.Set<LangleyDataTable>().Find(ldt.ldt_Id);
                if (entry != null)
                {
                    db.Entry<LangleyDataTable>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.LangleyDataTable.Attach(ldt);
                db.Entry(ldt).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Delete(LangleyDataTable ldt)
        {
            LangleyDataTable modle = db.LangleyDataTable.FirstOrDefault(m => m.ldt_Id == ldt.ldt_Id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.LangleyDataTable.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

      








        #endregion

    }
}