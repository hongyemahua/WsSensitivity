using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WsSensitivity.Controllers
{
    public class MySettingController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        public ActionResult MyInfo()
        {
            List<Role> roles = dbDrive.GetAllRoles();
            ViewData["roles"] = roles;
            return View();
        }

        public ActionResult PassWord()
        {
            return View();
        }

        //获得管理员信息
        public ActionResult GetMyInfo(string str_adminid)
        {
            int adminid = int.Parse(str_adminid);
            Admin admin = dbDrive.FindAdmin(adminid);
            int roleid = int.Parse(admin.role);
            Role role = dbDrive.FindRole(roleid);
            admin.rolename = role.rolename;
            return Json(new { admin }, JsonRequestBehavior.AllowGet);
        }

        //修改密码
        [HttpPost]
        public JsonResult AlterPassword(int id,string newpass)
        {
            Admin admin = dbDrive.FindAdmin(id);
            admin.pass = newpass;
            return Json(dbDrive.Udpdate(admin));
        }
    }
}