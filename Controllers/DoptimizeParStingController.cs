using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WsSensitivity.Controllers
{
    public class DoptimizeParStingController : Controller
    {
        // GET: DoptimizeParSting
        //D优化法参数设置
        public ActionResult DoptimizeParameterSettings()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DoptimizeParameterSettingsJson()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return Json("1");
        }
    }
}