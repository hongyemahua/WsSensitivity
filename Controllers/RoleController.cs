using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WsSensitivity.Controllers
{
    public class RoleController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // 角色列表页面
        public ActionResult RoleList()
        {
            return View();
        }
        //角色编辑页面
        public ActionResult RoleEditorForm()
        {
            return View();
        }
        //角色添加页面
        public ActionResult RoleAddForm()
        {
            return View();
        }

        //获得全部角色
        public ActionResult GetAllRoles()
        {
            List<Role> roles = dbDrive.GetAllRoles();
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = roles.Count, data = roles }, JsonRequestBehavior.AllowGet);
        }

        //角色名字查重
        [HttpPost]
        public JsonResult Role_hasname(string strid, string newrolename)
        {
            string oldname = "";
            if (strid != "")
            {
                int id = int.Parse(strid);
                oldname = dbDrive.FindRole(id).rolename;
            }
            if (oldname.Equals(newrolename)) return Json(true);
            List<Role> roleList = dbDrive.AccurateQueryRoles(newrolename);
            if (roleList != null && roleList.Count > 0) return Json(false);
            return Json(true);
        }

        //添加角色
        public ActionResult Role_add()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            string jsonText = stream;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Role role = new Role();
            role = js.Deserialize<Role>(stream);
            role.limit = role.admin + "-" + role.langley + "-" + role.liftingMethod + "-" + role.Doptimize + "-" + role.mysetting + "-" + role.about;
            return Json(dbDrive.Insert(role));
        }

        //删除角色
        [HttpPost]
        public ActionResult Role_delete(int id)
        {
            List<Admin> admins = dbDrive.GetAllAdmins();
            foreach (Admin admin in admins)
            {
                int roleid = int.Parse(admin.role);
                if (roleid==id) {
                    return Json(false);
                }
            }
            Role role = new Role();
            role.id = id;
            return Json(dbDrive.Delete(role));
        }

        //编辑角色
        [HttpPost]
        public JsonResult Role_editor()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            string jsonText = stream;
            JavaScriptSerializer js = new JavaScriptSerializer();
            Role role = js.Deserialize<Role>(stream);
            role.limit = role.admin + "-" + role.langley + "-" + role.liftingMethod + "-" + role.Doptimize + "-" + role.mysetting  + "-" + role.about;
            return Json(dbDrive.Udpdate(role));
        }

        //查询角色
        public ActionResult Role_query(string rolename)
        {
            List<Role> roles = dbDrive.QueryRoles(rolename);
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = roles.Count, data = roles }, JsonRequestBehavior.AllowGet);
        }
    }
}