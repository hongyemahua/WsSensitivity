using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models;

namespace WsSensitivity.Controllers
{
    struct Doptimize
    {
        public int ddt_Id;
        public int ddt_Number;
        public double ddt_StimulusQuantity;
        public int ddt_Response;
        public double ddt_Mean;
        public double ddt_StandardDeviation;
        public double ddt_SigmaGuess;
        public double ddt_MeanVariance;
        public double ddt_StandardDeviationVariance;
        public double ddt_Covmusigma;
        public double number;
    }
    struct Doptimize_list
    {
        public int number;
        public int Id;
        public double PrecisionInstruments;
        public double StimulusQuantityCeiling;
        public double StimulusQuantityFloor;
        public double StandardDeviationEstimate;
        public string Power;
        public string DistributionState;
        public int FlipTheResponse;
        public string ExperimentalDate;
        public int count;
    }
    public class DoptimizeController : Controller
    {
        // GET: DParam
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
            return Json(true);
        }
        //D优化法实验
        public ActionResult DoptimizeExperiment(int dop_id = -1)
        {
            if (dop_id == -1)
                ViewData["DoptimizeNameSeting"] = "D优化法";
            else ViewData["DoptimizeNameSeting"] = "D优化法详情";
            return View();
        }
        //技术条件
        public ActionResult TechnicalConditions()
        {
            ViewData["tc"] = "技术条件";
            return View();
        }
        //修改技术条件
        [HttpPost]
        public JsonResult TechnicalConditions_Update()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return Json(true);
        }
        //点计算
        [HttpPost]
        public JsonResult PointCalculate()
        {
            return Json(true);
        }
        //根据Id获取该id的D优化法数据
        public ActionResult GetAllDoptimizes(int page = 1, int limit = 20)
        {
            List<Doptimize> doptimizes = new List<Doptimize>();
            Doptimize doptimize = new Doptimize();
            doptimize.ddt_Covmusigma = 1;
            doptimize.ddt_Id = 1;
            doptimize.ddt_Mean = 1;
            doptimize.ddt_MeanVariance = 1;
            doptimize.ddt_Number = 1;
            doptimize.ddt_Response = 1;
            doptimize.ddt_SigmaGuess = 1;
            doptimize.ddt_StandardDeviation = 1;
            doptimize.ddt_StandardDeviationVariance = 1;
            doptimize.ddt_StimulusQuantity = 1;
            doptimize.number = 1;
            doptimizes.Add(doptimize);
            return Json(new { code = 0, msg = "", count = 1, data = doptimizes }, JsonRequestBehavior.AllowGet);
        }
        //D优化法查询界面
        public ActionResult DoptimizeQuery()
        {
            List<string> strList = new List<string>();
            string a = "D优化法1";
            string a1 = "D优化法2";
            string a2 = "D优化法3";
            strList.Add(a);
            strList.Add(a1);
            strList.Add(a2);
            ViewData["pn"] = strList;
            return View();
        }
        [HttpPost]
        public ActionResult doptimize_query(string productName, string startTime, string endTime)
        {
            return Json(true);
        }
        //获取全部D优化法实验数据
        public ActionResult GetAllDoptimizeExperiment(int page = 1, int limit = 20)
        {
            List<Doptimize_list> doptimizes = new List<Doptimize_list>();
            Doptimize_list doptimize = new Doptimize_list();
            doptimize.count = 1;
            doptimize.DistributionState = "1";
            doptimize.ExperimentalDate = "1";
            doptimize.FlipTheResponse = 1;
            doptimize.Id = 1;
            doptimize.number = 1;
            doptimize.Power = "2";
            doptimize.PrecisionInstruments = 1;
            doptimize.StandardDeviationEstimate = 1;
            doptimize.StimulusQuantityCeiling = 100;
            doptimize.StimulusQuantityFloor = 10;
            doptimizes.Add(doptimize);
            return Json(new { code = 0, msg = "", count = 1, data = doptimizes }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Doptimize_delete(int id)
        {
            return Json(true);
        }

    }
}