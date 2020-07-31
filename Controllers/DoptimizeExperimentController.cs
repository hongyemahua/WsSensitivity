using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;

namespace WsSensitivity.Controllers
{
    public class DoptimizeExperimentController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: DoptimizeExperiment
        //D优化法实验
        public ActionResult DoptimizeExperiment(int dop_id)
        {
            DoptimizeExperimentModel doptimizeExperimentModel = new DoptimizeExperimentModel();
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            doptimizeExperimentModel.doptimizeNameSeting = DoptimizePublic.DistributionState(det);
            doptimizeExperimentModel.sq = ddt_list[ddt_list.Count - 1].ddt_StimulusQuantity;
            doptimizeExperimentModel.det = det;
            return View(doptimizeExperimentModel);
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
            doptimize.number = 2;
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
            doptimizes.Reverse();
            return Json(new { code = 0, msg = "", count = doptimizes.Count, data = doptimizes }, JsonRequestBehavior.AllowGet);
        }
        //响应操作
        [HttpPost]
        public JsonResult InsertData(int dop_id,string response,string sq)
        {
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            ddt_list[ddt_list.Count - 1].ddt_StimulusQuantity = sq != "" ? double.Parse(sq) : ddt_list[ddt_list.Count - 1].ddt_StimulusQuantity;
            ddt_list[ddt_list.Count - 1].ddt_Response = int.Parse(response);
            var der_list = DoptimizePublic.DoptimizeExperimentRecoedsList(ddt_list,det);
            var xAndV = DoptimizePublic.ReturnXarrayAndVarray(der_list);
            var outputParameter = DoptimizePublic.SelectState(det).GetResult(xAndV.xArray, xAndV.vArray, det.det_StimulusQuantityFloor, det.det_StimulusQuantityCeiling, det.det_PrecisionInstruments, out double z, ddt_list[ddt_list.Count - 1].ddt_SigmaGuess);
            DoptimizeDataTable ddt = ddt_list[ddt_list.Count - 1];
            DoptimizePublic.UpdateDoptimizeDataTable(ref ddt,outputParameter,response,sq);
            dbDrive.Update(ddt);
            bool isTurn = dbDrive.Insert(DoptimizePublic.DoptimizeDataTable(det.det_Id, dbDrive, double.Parse(z.ToString("f6")), outputParameter));
            string[] value = { isTurn.ToString(), ddt_list.Count.ToString(), z.ToString("f6")};
            return Json(value);
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
        //文件接收方法
        [HttpPost]
        public ActionResult InputIntervalCalculation()
        {
            return Json("1");
        }
    }
}