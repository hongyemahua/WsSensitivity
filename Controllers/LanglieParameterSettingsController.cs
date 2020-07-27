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
    public class LanglieParameterSettingsController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: LanglieParameterSettings
        public ActionResult LanglieParameterSettings()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LanglieParameterSettingsJson()
        {
            double[] xArray = new double[] { };
            int[] vArray = new int[] { };
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            LangleyExperimentTable let = js.Deserialize<LangleyExperimentTable>(stream);
            let.let_ExperimentalDate = DateTime.Now;
            dbDrive.Insert(let);
            double sq = LangleyPublic.SelectState(let).CalculateStimulusQuantity(xArray, vArray, let.let_StimulusQuantityCeiling, let.let_StimulusQuantityFloor, let.let_PrecisionInstruments);
            bool isTure = dbDrive.Insert(LangleyPublic.LangleyDataTables(let.let_Id, dbDrive, sq));
            string name = let.let_ProductName;
            string[] value = { isTure.ToString(), let.let_Id.ToString(),name};
        
            return Json(value);
        }
    }
}