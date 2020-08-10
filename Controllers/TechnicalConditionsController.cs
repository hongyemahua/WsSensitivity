using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;

namespace WsSensitivity.Controllers
{
    public class TechnicalConditionsController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: TechnicalConditions
        public ActionResult TechnicalConditions(int id,string name)
        {
            if (name.Equals("D"))
            {
                var det = dbDrive.GetDoptimizeExperimentTable(id);
                ViewData["tc"] = det.det_TechnicalConditions;
                ViewData["id"] = id;
                ViewData["type"] = "D";
            }
            if (name.Equals("L"))
            {
                var let = dbDrive.GetLangleyExperimentTable(id);
                ViewData["tc"] = let.let_TechnicalConditions;
                ViewData["id"] = id;
                ViewData["type"] = "L";
            }
            return View();
        }

        [HttpPost]
        //编辑技术条件
        public ActionResult TechnicalConditions_Update()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var let = dbDrive.GetLangleyExperimentTable(js.Deserialize<LangleyExperimentTable>(stream).let_Id);
            let.let_TechnicalConditions = js.Deserialize<LangleyExperimentTable>(stream).let_TechnicalConditions;
            return Json(dbDrive.Update(let));
        }

        [HttpPost]
        public JsonResult TechnicalConditionsD_Update()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var det = dbDrive.GetDoptimizeExperimentTable(js.Deserialize<DoptimizeExperimentTable>(stream).det_Id);
            det.det_TechnicalConditions = js.Deserialize<DoptimizeExperimentTable>(stream).det_TechnicalConditions;
            return Json(dbDrive.Update(det));
        }
    }
}