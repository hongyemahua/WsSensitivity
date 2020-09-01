using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WsSensitivity.Controllers
{
    public class AdminController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        //管理员页
        public ActionResult AdminList()
        {
            return View();
        }

        //管理员详情页面
        public ActionResult AdminDetailForm()
        {
            return View();
        }

        //管理员添加页面
        public ActionResult AdminAddForm()
        {
            List<Role> roles = dbDrive.GetAllRoles();
            ViewData["roles"] = roles;
            return View();
        }

        //管理员编辑页面
        public ActionResult AdminEditorForm()
        {
            List<Role> roles = dbDrive.GetAllRoles();
            ViewData["roles"] = roles;
            return View();
        }

        //获得全部管理员
        public ActionResult GetAllAdmins()
        {
            List<Admin> admins = dbDrive.GetAllAdmins();
            List<Role> roles = dbDrive.GetAllRoles();
            foreach (Admin admin in admins)
            {
                int roleid = admin.role;
                foreach (Role role in roles)
                {
                    if (roleid == role.id) {
                        admin.rolename = role.rolename;
                    }
                }
            }
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = admins.Count, data = admins }, JsonRequestBehavior.AllowGet);
        }

        //添加管理员
        public ActionResult Admin_add()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            string jsonText = stream;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Admin admin = js.Deserialize<Admin>(stream);
            return Json(dbDrive.Insert(admin));
        }

        //编辑管理员
        [HttpPost]
        public JsonResult Admin_editor()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            string jsonText = stream;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Admin admin = js.Deserialize<Admin>(stream);
            admin.pass = dbDrive.FindAdmin(admin.id).pass;
            return Json(dbDrive.Udpdate(admin));
        }

        //删除管理员
        [HttpPost]
        public ActionResult Admin_delete(int id)
        {
            Admin admin = new Admin();
            admin.id = id;
            return Json(dbDrive.Delete(admin));
        }

        //管理员名字查重
        [HttpPost]
        public JsonResult Admin_hasname(string strid, string newname)
        {
            string oldname="";
            if (strid != "") {
                int id = int.Parse(strid);
                oldname = dbDrive.FindAdmin(id).name;
            }
            if (oldname.Equals(newname)) return Json(true);
            List<Admin> adminList = dbDrive.AccurateQueryAdmins(newname);
            if (adminList != null && adminList.Count > 0) return Json(false);
            return Json(true);
        }

        //查询管理员
        public ActionResult Admin_query(string name)
        {
            List<Admin> admins = dbDrive.QueryAdmins(name);
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = admins.Count, data = admins }, JsonRequestBehavior.AllowGet);
        }

        //重置管理员密码
        [HttpPost]
        public JsonResult Admin_resetPassword(int id)
        {
            Admin admin = dbDrive.FindAdmin(id);
            admin.pass = "admin";
            return Json(dbDrive.Udpdate(admin));
        }
    }
}