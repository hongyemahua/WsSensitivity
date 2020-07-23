using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WsSensitivity.Controllers
{
    public class DoptimizeExperimentController : Controller
    {
        // GET: DoptimizeExperiment
        //D优化法实验
        public ActionResult DoptimizeExperiment(int dop_id)
        {
            
            ViewData["DoptimizeNameSeting"] = "D优化法";
            ViewData["dop_id"] = dop_id;
            return View();
        }
        //批量区间计算成果图
        public ActionResult DoptimizeChart(int dop_id)
        {
            return View();
        }
        //导出数据
        public FileResult DownloadDocument()
        {
            return File("1","defef");
        }
        //点计算
        [HttpPost]
        public JsonResult PointCalculate(int dop_id)
        {
            return Json(true);
        }
        //根据Id获取该id的D优化法数据
        public ActionResult GetAllDoptimizes(int dop_id,int page = 1, int limit = 20)
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
            Doptimize doptimize1 = new Doptimize();
            doptimize1.ddt_Covmusigma = 1;
            doptimize1.ddt_Id = 2;
            doptimize1.ddt_Mean = 1;
            doptimize1.ddt_MeanVariance = 1;
            doptimize1.ddt_Number = 2;
            doptimize1.ddt_Response = 1;
            doptimize1.ddt_SigmaGuess = 1;
            doptimize1.ddt_StandardDeviation = 1;
            doptimize1.ddt_StandardDeviationVariance = 1;
            doptimize1.ddt_StimulusQuantity = 1;
            doptimize1.number = 2;
            doptimizes.Add(doptimize1);
            return Json(new { code = 0, msg = "", count = doptimizes.Count, data = doptimizes }, JsonRequestBehavior.AllowGet);
        }
        //响应操作
        [HttpPost]
        public JsonResult InsertData(int dop_id,string response,string sq)
        {
            string[] res = {"1","1","1","1"};
            return Json(res);
        }
        //撤销操作
        [HttpPost]
        public JsonResult RevocationData(int dop_id, string id)
        {
            string[] valve = {"1","1","1" };
            return Json(valve);
        }
        //响应点
        [HttpPost]
        public JsonResult ResponsePointCalculate(int dop_id,double fq, double favg, double fsigma)
        {
            
            return Json(1);
        }
        [HttpPost]
        //响应概率计算
        public ActionResult ResponseProbabilityCalculate(int dop_id,double fq, double favg, double fsigma)
        {
           
            return Json(1);
        }
        //响应概率区间估计
        [HttpPost]
        public ActionResult ResponseProbabilityIntervalEstimate(int dop_id,double reponseProbability2, double confidenceLevel)
        {
           return Json(1);
        }
        //响应点区间估计
        [HttpPost]
        public ActionResult ResponsePointIntervalEstimate(int dop_id,double reponseProbability2, double confidenceLevel2, double cjl, double favg, double fsigma)
        {
           return Json(1);
        }
        [HttpPost]
        //批量区间估计
        public ActionResult BatchIntervalCalculation(double BatchConfidenceLevel, double yMin, double yMax, int Y_Axis, int intervalTypeSelection, double favg, int dop_id, double fsigma)
        {
           
            return Json(true);
        }
    }
}