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
    public class ParameterSettingController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: ParameterSetting
        public ActionResult ParameterSetting(int langlryExpTableId)
        {
            var let = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            ViewData["ds"] = let.let_DistributionState;
            ViewData["correction"] = let.let_Correction;
            ViewData["langlryExpTableId"] = langlryExpTableId;
            return View();
        }

        [HttpPost]
        //修改分析参数
        public JsonResult UpdateParameter()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            LangleyExperimentTable let = dbDrive.GetLangleyExperimentTable(js.Deserialize<LangleyExperimentTable>(stream).let_Id);
            let.let_DistributionState = js.Deserialize<LangleyExperimentTable>(stream).let_DistributionState;
            let.let_Correction = js.Deserialize<LangleyExperimentTable>(stream).let_Correction;
            dbDrive.Update(let);
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(let.let_Id);
            ldts.RemoveRange(ldts.Count - 1, 1);
            var xOrVArray = LangleyPublic.XAndVArrays(ldts);
            var lr = LangleyPublic.SelectState(let);
            LangleyDataTable langleyDataTable = new LangleyDataTable();
            bool isTure = false;
            for (int i = 1; i <= ldts.Count; i++)
            {
                double[] xArray = new double[i];
                int[] vArray = new int[i];
                for (int j = 0; j < i; j++)
                {
                    xArray[j] = xOrVArray.xArray[j];
                    vArray[j] = xOrVArray.vArray[j];
                }
                langleyDataTable = LangleyPublic.UpdateLangleyDataTable(let, lr, xArray, vArray, ldts[i - 1]);
                isTure = dbDrive.UpDate(langleyDataTable);
                if (isTure == false)
                    break;
            }
            string[] value = { isTure.ToString(), lr.Precs(langleyDataTable.ldt_Mean, langleyDataTable.ldt_StandardDeviation)[0].ToString("f6"), lr.Precs(langleyDataTable.ldt_Mean, langleyDataTable.ldt_StandardDeviation)[1].ToString("f6"), langleyDataTable.ldt_Mean.ToString("f6"), langleyDataTable.ldt_StandardDeviation.ToString("f6") };
            return Json(value);
        }
    }
}