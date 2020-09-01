using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using WsSensitivity.Models.DB;

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

        public override List<LangleyExperimentTable> GetAllLangleyExperimentTables(int admin_id)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            if (admin.role == 1)
                lets = db.LangleyExperimentTable.ToList();
            else
                lets = db.LangleyExperimentTable.Where(m => m.let_RecordEmployeeId == admin_id).ToList();
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


        public override List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName, DateTime startTime, DateTime endTime, int admin_id)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            if (admin.role == 1)
                lets = db.LangleyExperimentTable.Where(m => m.let_ProductName.Contains(productName) && DateTime.Compare(startTime, m.let_ExperimentalDate) <= 0 && DateTime.Compare(endTime, m.let_ExperimentalDate) >= 0).ToList();
            else
                lets = db.LangleyExperimentTable.Where(m => m.let_ProductName.Contains(productName) && DateTime.Compare(startTime, m.let_ExperimentalDate) <= 0 && DateTime.Compare(endTime, m.let_ExperimentalDate) >= 0 && m.let_RecordEmployeeId == admin_id).ToList();
            return lets;
        }

        public override List<LangleyExperimentTable> QueryLangleyExperimentTable(string productName, int admin_id)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            if (admin.role ==1)
                lets = db.LangleyExperimentTable.Where(m => m.let_ProductName.Contains(productName)).ToList();
            else
                lets = db.LangleyExperimentTable.Where(m => m.let_ProductName.Contains(productName) && m.let_RecordEmployeeId == admin_id).ToList();
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

        public override bool Update(LangleyDataTable ldt)
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

        public override bool Update(DoptimizeExperimentTable det)
        {
            try
            {
                var entry = db.Set<DoptimizeExperimentTable>().Find(det.det_Id);
                if (entry != null)
                {
                    db.Entry<DoptimizeExperimentTable>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.DoptimizeExperimentTable.Attach(det);
                db.Entry(det).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override List<DoptimizeExperimentTable> GetAllDoptimizeExperimentTables(int admin_id)
        {
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            List<DoptimizeExperimentTable> det = new List<DoptimizeExperimentTable>();
            if (admin.role == 1)
                det = db.DoptimizeExperimentTable.ToList();
            else
                det = db.DoptimizeExperimentTable.Where(m => m.det_RecordEmployeeId == admin_id).ToList();
            return det;
        }

        public override List<DoptimizeExperimentTable> QueryDoptimizeExperimentTable(string productName, DateTime startTime, DateTime endTime,int admin_id)
        {
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            List<DoptimizeExperimentTable> det_list = new List<DoptimizeExperimentTable>();
            if(admin.role == 1)
                det_list = db.DoptimizeExperimentTable.Where(m => m.det_ProductName.Contains(productName) && DateTime.Compare(startTime, m.det_ExperimentalDate) <= 0 && DateTime.Compare(endTime, m.det_ExperimentalDate) >= 0).ToList();
            else
                det_list = db.DoptimizeExperimentTable.Where(m => m.det_ProductName.Contains(productName) && DateTime.Compare(startTime, m.det_ExperimentalDate) <= 0 && DateTime.Compare(endTime, m.det_ExperimentalDate) >= 0 && m.det_RecordEmployeeId == admin_id).ToList();
            return det_list;
        }
        public override List<DoptimizeExperimentTable> QueryDoptimizeExperimentTable(string productName,int admin_id)
        {
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            List<DoptimizeExperimentTable> det_list = new List<DoptimizeExperimentTable>();
            if(admin.role == 1)
                det_list = db.DoptimizeExperimentTable.Where(m => m.det_ProductName.Contains(productName)).ToList();
            else
                det_list = db.DoptimizeExperimentTable.Where(m => m.det_ProductName.Contains(productName) && m.det_RecordEmployeeId == admin_id).ToList();
            return det_list;
        }
        public override bool Delete(DoptimizeExperimentTable det)
        {
            DoptimizeExperimentTable modle = db.DoptimizeExperimentTable.FirstOrDefault(m => m.det_Id == det.det_Id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.DoptimizeExperimentTable.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


        #endregion

        #region D优化法实验表操作
        public override bool Insert(DoptimizeExperimentTable det)
        {
            try
            {
                db.DoptimizeExperimentTable.Add(det);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public override DoptimizeExperimentTable GetDoptimizeExperimentTable(int id)
        {
            DoptimizeExperimentTable det = db.DoptimizeExperimentTable.Where(m => m.det_Id == id).First();
            return det;
        }
        #endregion

        #region D优化法数据表操作
        public override bool Insert(DoptimizeDataTable ddt)
        {
            try
            {
                db.DoptimizeDataTable.Add(ddt);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        public override List<DoptimizeDataTable> GetDoptimizeDataTables(int id)
        {
            List<DoptimizeDataTable> ldts = new List<DoptimizeDataTable>();
            ldts = db.DoptimizeDataTable.Where(m => m.ddt_ExperimentTableId == id).ToList();
            return ldts;
        }

        public override bool Update(DoptimizeDataTable ddt)
        {
            try
            {
                var entry = db.Set<DoptimizeDataTable>().Find(ddt.ddt_Id);
                if (entry != null)
                {
                    db.Entry<DoptimizeDataTable>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.DoptimizeDataTable.Attach(ddt);
                db.Entry(ddt).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override DoptimizeDataTable GetDoptimizeDataTable(int id)
        {
            DoptimizeDataTable ddt = new DoptimizeDataTable();
            ddt = db.DoptimizeDataTable.Where(m => m.ddt_Id == id).ToList()[0];
            return ddt;
        }

        
        public override bool Delete(DoptimizeDataTable ddt)
        {
            DoptimizeDataTable modle = db.DoptimizeDataTable.FirstOrDefault(m => m.ddt_Id == ddt.ddt_Id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.DoptimizeDataTable.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 升降法数据操作
        public override bool Insert(UpDownExperiment experiment)
        {
            try
            {
                db.UpDownExperiment.Add(experiment);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Insert(UpDownGroup group)
        {
            try
            {
                db.UpDownGroup.Add(group);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Insert(UpDownDataTable upDownDataTable)
        {
            try
            {
                db.UpDownDataTable.Add(upDownDataTable);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override UpDownGroup GetDownGroup(int id)
        {
            UpDownGroup udg = new UpDownGroup();
            udg = db.UpDownGroup.Where(m => m.Id == id).First();
            return udg;
        }

        public override UpDownExperiment GetUpDownExperiment(int id)
        {
            UpDownExperiment ude = new UpDownExperiment();
            ude = db.UpDownExperiment.Where(m => m.id == id).ToList()[0];
            return ude;
        }

        public override List<UpDownDataTable> GetUpDownDataTables(int id)
        {
            List<UpDownDataTable> udts = new List<UpDownDataTable>();
            udts = db.UpDownDataTable.Where(m => m.dtup_DataTableId == id).ToList();
            return udts;
        }

        public override List<UpDownView> GetUpDownViews(int id)
        {
            List<UpDownView> udvs = new List<UpDownView>();
            udvs = db.UpDownView.Where(m => m.udg_Id == id).ToList();
            return udvs;
        }

        public override List<UpDownGroup> GetUpDownGroups(int id)
        {
            List<UpDownGroup> udgs = new List<UpDownGroup>();
            udgs = db.UpDownGroup.Where(m => m.dudt_ExperimentId == id).ToList();
            return udgs;
        }

        public override bool Update(UpDownExperiment upDownExperiment)
        {
            try
            {
                var entry = db.Set<UpDownExperiment>().Find(upDownExperiment.id);
                if (entry != null)
                {
                    db.Entry<UpDownExperiment>(entry).State = EntityState.Detached; //这个是在同一个上下文能修改的关键
                }
                db.UpDownExperiment.Attach(upDownExperiment);
                db.Entry(upDownExperiment).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Delete(UpDownDataTable upDownDataTable)
        {
            UpDownDataTable modle = db.UpDownDataTable.FirstOrDefault(m => m.Id == upDownDataTable.Id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.UpDownDataTable.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override bool Delete(UpDownGroup upDownGroup)
        {
            UpDownGroup modle = db.UpDownGroup.FirstOrDefault(m => m.Id == upDownGroup.Id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.UpDownGroup.Remove(modle);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public override List<UpDownView> GetUpDownViews_UDEID(int id)
        {
            List<UpDownView> udvs = new List<UpDownView>();
            udvs = db.UpDownView.Where(m => m.dudt_ExperimentId == id).ToList();
            return udvs;
        }

        public override List<UpDownExperiment> GetUpDownExperiments(int admin_id)
        {
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            List<UpDownExperiment> udes = new List<UpDownExperiment>();
            if(admin.role == 1)
                udes = db.UpDownExperiment.ToList();
            else
                udes = db.UpDownExperiment.Where(m => m.udt_RecordEmployeeId == admin_id).ToList();
            return udes;
        }

        public override List<UpDownExperiment> QueryExperimentTable(string productName, DateTime startTime, DateTime endTime,int admin_id)
        {
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            List<UpDownExperiment> udes = new List<UpDownExperiment>();
            if(admin.role == 1)
                udes = db.UpDownExperiment.Where(m => m.udt_ProdectName.Contains(productName) && DateTime.Compare(startTime, m.udt_Creationtime) <= 0 && DateTime.Compare(endTime, m.udt_Creationtime) >= 0).ToList();
            else
                udes = db.UpDownExperiment.Where(m => m.udt_ProdectName.Contains(productName) && DateTime.Compare(startTime, m.udt_Creationtime) <= 0 && DateTime.Compare(endTime, m.udt_Creationtime) >= 0 && m.udt_RecordEmployeeId == admin_id).ToList();
            return udes;
        }

        public override List<UpDownExperiment> QueryExperimentTable(string productName,int admin_id)
        {
            Admin admin = db.Admin.Where(m => m.id == admin_id).First();
            List<UpDownExperiment> udes = new List<UpDownExperiment>();
            if(admin.role == 1)
                udes = db.UpDownExperiment.Where(m => m.udt_ProdectName.Contains(productName)).ToList();
            else
                udes = db.UpDownExperiment.Where(m => m.udt_ProdectName.Contains(productName) && m.udt_RecordEmployeeId == admin_id).ToList();
            return udes;
        }

        public override bool Delete(UpDownExperiment upDownExperiment)
        {
            UpDownExperiment modle = db.UpDownExperiment.FirstOrDefault(m => m.id == upDownExperiment.id);
            if (modle == null)
            {
                return false;
            }
            try
            {
                db.UpDownExperiment.Remove(modle);
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