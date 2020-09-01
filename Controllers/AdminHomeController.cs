using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WsSensitivity.Controllers;

namespace Navigation.Controllers
{
    public class AdminHomeController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult HomePage()
        {
            return View();
        }

        #region 药品列表
        public ActionResult PharmacyList()
        {
            return View();
        }
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LinkManagement()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            
            return View();
        }

        //管理员登录页面
        public ActionResult AdminLogin(string name,string pass)
        {
            Admin admin = new Admin();
            admin.name = name;
            admin.pass = pass;
            Admin loginadmin = dbDrive.AdminLogin(admin);
            if (loginadmin != null)
            {
                if (loginadmin.state.Equals("禁用")) return Json(false);
                int roleid = loginadmin.role;
                Role role = dbDrive.FindRole(roleid);
                Session["limit"] = role.limit;
                Session["Admin"] = loginadmin;
                LangleyPublic.adminId = loginadmin.id;
                return Json(true);
            }
            return Json(false);
        }

        //退出
        public ActionResult AdminLoginoff()
        {
            Session.Clear();
            return RedirectToAction("Login", "AdminHome");
        }
    }
}