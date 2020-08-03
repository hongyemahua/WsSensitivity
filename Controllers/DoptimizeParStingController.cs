using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    public class DoptimizeParStingController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: DoptimizeParSting
        //D优化法参数设置
        public ActionResult DoptimizeParameterSettings()
        {
            return View();
        }
        [HttpPost]
        public JsonResult DoptimizeParameterSettingsJson()
        {
            double[] xArray = { };
            int[] vArray = { };
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            DoptimizeExperimentTable det = js.Deserialize<DoptimizeExperimentTable>(stream);
            det.det_ExperimentalDate = DateTime.Now;
            dbDrive.Insert(det);
            var outputParameter = DoptimizePublic.SelectState(det).GetResult(xArray,vArray,det.det_StimulusQuantityFloor,det.det_StimulusQuantityCeiling,det.det_PrecisionInstruments,out double z,det.det_StandardDeviationEstimate);
            bool isTure = dbDrive.Insert(DoptimizePublic.DoptimizeDataTable(det.det_Id, dbDrive, z,outputParameter));
            string name = det.det_ProductName;
            string[] value = { isTure.ToString(),det.det_Id.ToString(),name};
            return Json(value);
        }
    }
}