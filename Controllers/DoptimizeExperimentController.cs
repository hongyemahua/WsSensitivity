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
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            DoptimizeDataTable ddt = ddt_list[ddt_list.Count - 2];
            var lr = DoptimizePublic.SelectState(dbDrive.GetDoptimizeExperimentTable(dop_id));
            double mean = ddt.ddt_Mean;
            double standardDeviation = ddt.ddt_StandardDeviation;
            double[] precs = lr.Precs(mean, standardDeviation);
            string[] value = { mean.ToString("f6"), standardDeviation.ToString("f6"), precs[0].ToString("f6"), precs[1].ToString("f6") }; 
            return Json(value);
        }
        //根据Id获取该id的D优化法数据
        public ActionResult GetAllDoptimizes(int dop_id,int page = 1, int limit = 20)
        {
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            List<DoptimizeDataTable> PagesLdt = new List<DoptimizeDataTable>();
            int last = ddt_list.Count - (page - 1) * limit;
            int first = 0;
            if (page * limit < ddt_list.Count)
            {
                first = ddt_list.Count - page * limit;
            }
            for (int i = first; i < last; i++)
            {
                PagesLdt.Add(ddt_list[i]);
            }
            if (page == 1)
                PagesLdt.RemoveRange(PagesLdt.Count - 1, 1);
            return Json(new { code = 0, msg = "", count = ddt_list.Count - 1, data = DoptimizePublic.GetDoptimizes(PagesLdt,ddt_list.Count) }, JsonRequestBehavior.AllowGet) ;
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
        public JsonResult RevocationData(int dop_id)
        {
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            var isTurn = dbDrive.Update(ddt_list[ddt_list.Count - 1]);
            int updateShowNumber = ddt_list.Count - 2;
            double nextStimulusQuantity = ddt_list[ddt_list.Count - 2].ddt_StimulusQuantity;
            string[] valve = { isTurn.ToString(), updateShowNumber.ToString(), nextStimulusQuantity.ToString()};
            return Json(valve);
        }
        //响应点
        [HttpPost]
        public JsonResult ResponsePointCalculate(int dop_id,double fq, double favg, double fsigma)
        {
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            var lr = DoptimizePublic.SelectState(det);
            return Json(lr.ResponsePointCalculate(fq, favg, fsigma).ToString("f6"));
        }
        [HttpPost]
        //响应概率计算
        public ActionResult ResponseProbabilityCalculate(int dop_id,double fq, double favg, double fsigma)
        {
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            var lr = DoptimizePublic.SelectState(det);
            return Json(lr.ResponseProbabilityCalculate(fq, favg, fsigma).ToString("f6"));
        }
        //响应概率区间估计
        [HttpPost]
        public ActionResult ResponseProbabilityIntervalEstimate(int dop_id,double reponseProbability2, double confidenceLevel)
        {
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            ddt_list.RemoveRange(ddt_list.Count - 1, 1);
            var der_list = DoptimizePublic.DoptimizeExperimentRecoedsList(ddt_list, det);
            var xAndV = DoptimizePublic.ReturnXarrayAndVarray(der_list);
            var lr = DoptimizePublic.SelectState(det);
            var ies = lr.ResponseProbabilityIntervalEstimate(xAndV.xArray, xAndV.vArray, reponseProbability2, confidenceLevel);
            return Json(DoptimizePublic.GetIntervalEstimateValue(ies));
        }
        //响应点区间估计
        [HttpPost]
        public ActionResult ResponsePointIntervalEstimate(int dop_id,double reponseProbability2, double confidenceLevel2, double cjl, double favg, double fsigma)
        {
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            ddt_list.RemoveRange(ddt_list.Count - 1, 1);
            var der_list = DoptimizePublic.DoptimizeExperimentRecoedsList(ddt_list, det);
            var xAndV = DoptimizePublic.ReturnXarrayAndVarray(der_list);
            var lr = DoptimizePublic.SelectState(det);
            var ies = lr.ResponsePointIntervalEstimate(xAndV.xArray, xAndV.vArray, reponseProbability2, confidenceLevel2, cjl, favg, fsigma);
            return Json(DoptimizePublic.GetIntervalEstimateValue(ies));
        }
        [HttpPost]
        //批量区间估计
        public ActionResult BatchIntervalCalculation(double BatchConfidenceLevel, double yMin, double yMax, int Y_Axis, int intervalTypeSelection, double favg, int dop_id, double fsigma)
        {
            DoptimizeExperimentTable det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            List<DoptimizeDataTable> ddt_list = dbDrive.GetDoptimizeDataTables(dop_id);
            ddt_list.RemoveRange(ddt_list.Count - 1, 1);
            var der_list = DoptimizePublic.DoptimizeExperimentRecoedsList(ddt_list, det);
            var xAndV = DoptimizePublic.ReturnXarrayAndVarray(der_list);
            var lr = DoptimizePublic.SelectState(det);
            var srd = lr.BatchIntervalCalculate(yMax, yMin, Y_Axis, BatchConfidenceLevel, favg, fsigma, xAndV.xArray, xAndV.vArray, intervalTypeSelection);
            LangleyPublic.sideReturnData = srd;
            LangleyPublic.aArray.Clear();
            LangleyPublic.bArray.Clear();
            LangleyPublic.cArray.Clear();
            double ceiling = srd.responsePoints.Min();
            double lower = srd.responsePoints.Max();
            for (int i = 0; i < srd.responseProbability.Length; i++)
            {
                LangleyPublic.aArray.Add("[" + srd.responsePoints[i] + "," + srd.responseProbability[i] + "]");
                if (double.IsInfinity(srd.Y_Ceilings[i]))
                    LangleyPublic.bArray.Add("[" + lower + "," + srd.responseProbability[i] + "]");
                else
                    LangleyPublic.bArray.Add("[" + srd.Y_Ceilings[i] + "," + srd.responseProbability[i] + "]");
                if (double.IsInfinity(srd.Y_LowerLimits[i]))
                    LangleyPublic.cArray.Add("[" + ceiling + "," + srd.responseProbability[i] + "]");
                else
                    LangleyPublic.cArray.Add("[" + srd.Y_LowerLimits[i] + "," + srd.responseProbability[i] + "]");
            }
            if (intervalTypeSelection == 0)
                LangleyPublic.incredibleIntervalType = "拟然比区间计算-单侧置信区间";
            else
                LangleyPublic.incredibleIntervalType = "拟然比区间计算-双侧置信区间";
            LangleyPublic.incredibleLevelName = BatchConfidenceLevel.ToString();
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