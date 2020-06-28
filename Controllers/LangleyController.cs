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
    public class LangleyController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        private static double pow = 0;
        private static int let_id = 1;
        //兰利法主页面
        public ActionResult LangleyExperiment()
        {
            return View();
        }

        //兰利法分析参数设置
        public ActionResult ParameterSetting()
        {
            return View();
        }

        //技术条件
        public ActionResult TechnicalConditions()
        {
            return View();
        }

        //兰利法查询
        public ActionResult LangleyQuery()
        {
            return View();
        }

        //获取全部的兰利法表
        public ActionResult GetAllLangleys()
        {
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(let_id);
            return Json(new { code = 0, msg = "", count = ldts.Count, data = ldts }, JsonRequestBehavior.AllowGet);
        }

        //删除兰利法
        [HttpPost]
        public ActionResult Langley_delete(int id)
        {
            return Json(true);
        }
        public ActionResult LanglieParameterSettings() 
        {
            return View();
        }

        [HttpPost]
        public JsonResult LanglieParameterSettingsJson() 
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            //string jsonText = stream;
            JavaScriptSerializer js = new JavaScriptSerializer();
            LangleyExperimentTable let = js.Deserialize<LangleyExperimentTable>(stream);
            let.let_NumberOfData = 1;
            let.let_ExperimentalDate = DateTime.Now;
            pow = Convert.ToDouble(let.let_Power);
            double[] xArray = { };
            int[] vArray = { };
            double sq = SelectState(let).CalculateStimulusQuantity(xArray, vArray, let.let_StimulusQuantityCeiling, let.let_StimulusQuantityFloor, let.let_PrecisionInstruments);
            dbDrive.Insert(let);
            LangleyDataTable ldt = new LangleyDataTable();
            ldt.ldt_ExperimentTableId = let.let_Id;
            ldt.ldt_StimulusQuantity = sq;
            ldt.ldt_Number = 1;
            ldt.ldt_Response = 0;
            ldt.ldt_Mean = 0;
            ldt.ldt_StandardDeviation = 0;
            ldt.ldt_MeanVariance = 0;
            ldt.ldt_StandardDeviationVariance = 0;
            dbDrive.Insert(ldt);
            let_id = let.let_Id;
            GetAllLangleys();
            return Json(true);
        }

        private LangleyAlgorithm SelectState(LangleyExperimentTable let)
        {
            if (let.let_DistributionState == 0 && let.let_StandardState == 0)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Standard());
                return langley;
            }
            if (let.let_DistributionState == 0 && let.let_StandardState == 1)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Ln());
                return langley;
            }
            if (let.let_DistributionState == 0 && let.let_StandardState == 2)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Log());
                return langley;
            }
            if (let.let_DistributionState == 0 && let.let_StandardState == 3)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Pow(pow));
                return langley;
            }
            if (let.let_DistributionState == 1 && let.let_StandardState == 0)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Standard());
                return langley;
            }
            if (let.let_DistributionState == 1 && let.let_StandardState == 1)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Ln());
                return langley;
            }
            if (let.let_DistributionState == 1 && let.let_StandardState == 2)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Log());
                return langley;
            }
            if (let.let_DistributionState == 1 && let.let_StandardState == 3)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Pow(pow));
                return langley;
            }
            throw new Exception("错误");
        }
    }
}