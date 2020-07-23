﻿using System;
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
        public ActionResult TechnicalConditions(int langlryExpTableId)
        {
            var let = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            ViewData["tc"] = let.let_TechnicalConditions;
            ViewData["langlryExpTableId"] = langlryExpTableId;
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
    }
}