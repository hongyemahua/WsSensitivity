using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WsSensitivity.Controllers
{
    public class DoptimizeQueryController : Controller
    {
        // GET: DoptimizeQuery
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
            Doptimize_list doptimize2 = new Doptimize_list();
            doptimize2.count = 1;
            doptimize2.DistributionState = "1";
            doptimize2.ExperimentalDate = "1";
            doptimize2.FlipTheResponse = 2;
            doptimize2.Id = 2;
            doptimize2.number = 2;
            doptimize2.Power = "2";
            doptimize2.PrecisionInstruments = 2;
            doptimize2.StandardDeviationEstimate = 2;
            doptimize2.StimulusQuantityCeiling = 100;
            doptimize2.StimulusQuantityFloor = 10;
            doptimizes.Add(doptimize2);
            return Json(new { code = 0, msg = "", count = doptimizes.Count, data = doptimizes }, JsonRequestBehavior.AllowGet);
        }
        //D优化法返回查询结果
        [HttpPost]
        public ActionResult doptimize_query(string productName, string startTime, string endTime)
        {
            return Json(true);
        }
        [HttpPost]
        public JsonResult Doptimize_delete(int dop_id)
        {
            return Json(true);
        }
    }
}