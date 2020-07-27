using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    public class LangleyExperimentController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: LangleyExperiment
        public ActionResult LangleyExperiment(int let_id)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(let_id);
            ViewData["langlryExpTableId"] = langlryExpTable.let_Id;
            ViewData["langLeyNameString"] = "产品名称：" + langlryExpTable.let_ProductName + "" + "/" + LangleyPublic.DistributionState(langlryExpTable) + "/" + LangleyPublic.Correction(langlryExpTable.let_Correction);
            return View();
        }

        //增加数据
        [HttpPost]
        public ActionResult InsertData(string response, string sq,int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            List<LangleyDataTable> langleyDataTables = dbDrive.GetAllLangleyDataTable(langlryExpTable.let_Id);
            var xOrVArray = LangleyPublic.XAndVArrays(langleyDataTables);
            if (sq != null && sq != "")
                xOrVArray.xArray[langleyDataTables.Count - 1] = double.Parse(sq);
            if (response != null && response != "")
                xOrVArray.vArray[langleyDataTables.Count - 1] = int.Parse(response);
            else
                xOrVArray.vArray[langleyDataTables.Count - 1] = 0;
            var lr = LangleyPublic.SelectState(langlryExpTable);

            dbDrive.UpDate(LangleyPublic.UpdateLangleyDataTable(langlryExpTable, lr, xOrVArray.xArray, xOrVArray.vArray, langleyDataTables[langleyDataTables.Count - 1]));

            //增加一条新数据
            double StimulusQuantity = lr.CalculateStimulusQuantity(xOrVArray.xArray, xOrVArray.vArray, langlryExpTable.let_StimulusQuantityCeiling, langlryExpTable.let_StimulusQuantityFloor, langlryExpTable.let_PrecisionInstruments);
            var isTrue = dbDrive.Insert(LangleyPublic.LangleyDataTables(langlryExpTableId, dbDrive, double.Parse(StimulusQuantity.ToString("f6"))));
            var xOrVArray2 = LangleyPublic.XAndVArrays(dbDrive.GetAllLangleyDataTable(langlryExpTable.let_Id));
            xOrVArray2.vArray = LangleyPublic.IsFlipTheResponse(langlryExpTable, xOrVArray2.vArray);
            if (langlryExpTable.let_FlipTheResponse == 1)
            {
                for (int i = 0; i < xOrVArray2.vArray.Length - 1; i++)
                {
                    if (xOrVArray2.vArray[i] == 0)
                        xOrVArray2.vArray[i] = 1;
                    else
                        xOrVArray2.vArray[i] = 0;
                }
            }
            string[] value = { isTrue.ToString(), (xOrVArray2.xArray.Length - 1).ToString(), lr.GetNM(xOrVArray2.xArray, xOrVArray2.vArray), StimulusQuantity.ToString() };
            return Json(value);
        }
        //点计算
        [HttpPost]
        public ActionResult PointCalculate(int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(langlryExpTable.let_Id);
            ldts.RemoveRange(ldts.Count - 1, 1);
            var xOrVArray = LangleyPublic.XAndVArrays(ldts);
            var lr = LangleyPublic.SelectState(langlryExpTable);
            LangleyDataTable langleyDataTable = LangleyPublic.UpdateLangleyDataTable(langlryExpTable, lr, xOrVArray.xArray, xOrVArray.vArray, ldts[ldts.Count - 1]);
            langleyDataTable.ldt_Mean = double.Parse(langleyDataTable.ldt_Mean.ToString("f13"));
            string[] value = { lr.Precs(langleyDataTable.ldt_Mean, langleyDataTable.ldt_StandardDeviation)[0].ToString("f6"), lr.Precs(langleyDataTable.ldt_Mean, langleyDataTable.ldt_StandardDeviation)[1].ToString("f6"), langleyDataTable.ldt_Mean.ToString("f6"), langleyDataTable.ldt_StandardDeviation.ToString("f6"), lr.GetConversionNumber(xOrVArray.vArray), (xOrVArray.xArray.Length).ToString(), lr.GetNM(xOrVArray.xArray, xOrVArray.vArray) };
            dbDrive.UpDate(langleyDataTable);
            return Json(value);
        }
        [HttpPost]
        //响应点计算
        public ActionResult ResponsePointCalculate(double fq, double favg, double fsigma, int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            var lr = LangleyPublic.SelectState(langlryExpTable);
            return Json(lr.ResponsePointCalculate(fq, favg, fsigma).ToString("f6"));
        }

        [HttpPost]
        //响应概率计算
        public ActionResult ResponseProbabilityCalculate(double fq, double favg, double fsigma, int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            var lr = LangleyPublic.SelectState(langlryExpTable);
            return Json(lr.ResponseProbabilityCalculate(fq, favg, fsigma).ToString("f6"));
        }

        [HttpPost]
        //撤销
        public ActionResult RevocationData(int id,int langlryExpTableId)
        {
            LangleyDataTable langleyDataTable = new LangleyDataTable();
            List<LangleyDataTable> langleyDataTables = dbDrive.GetAllLangleyDataTable(langlryExpTableId);
            langleyDataTable.ldt_Id = langleyDataTables[langleyDataTables.Count - 1].ldt_Id;
            bool isTure = dbDrive.Delete(langleyDataTable);
            var xOrVArray = LangleyPublic.XAndVArrays(langleyDataTables);
            string[] value = { isTure.ToString(), (xOrVArray.xArray.Length - 1).ToString(), xOrVArray.xArray[xOrVArray.xArray.Length - 2].ToString() };
            return Json(value);
        }
        //响应概率区间估计
        [HttpPost]
        public ActionResult ResponseProbabilityIntervalEstimate(double reponseProbability2, double confidenceLevel, int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(langlryExpTable.let_Id);
            ldts.RemoveRange(ldts.Count - 1, 1);
            var xOrVArray = LangleyPublic.XAndVArrays(ldts);
            xOrVArray.vArray = LangleyPublic.IsFlipTheResponse(langlryExpTable, xOrVArray.vArray);
            var lr = LangleyPublic.SelectState(langlryExpTable);
            var ies = lr.ResponseProbabilityIntervalEstimate(xOrVArray.xArray, xOrVArray.vArray, reponseProbability2, confidenceLevel);
            string[] value = { "(" + ies[0].Confidence.Down.ToString("f6") + "," + ies[0].Confidence.Up.ToString("f6") + ")", "(" + ies[0].Mu.Down.ToString("f6") + "," + ies[0].Mu.Up.ToString("f6") + ")", "(" + ies[0].Sigma.Down.ToString("f6") + "," + ies[0].Sigma.Up.ToString("f6") + ")", "(" + ies[1].Confidence.Down.ToString("f6") + "," + ies[1].Confidence.Up.ToString("f6") + ")", "(" + ies[1].Mu.Down.ToString("f6") + "," + ies[1].Mu.Up.ToString("f6") + ")", "(" + ies[1].Sigma.Down.ToString("f6") + "," + ies[1].Sigma.Up.ToString("f6") + ")" };
            return Json(value);
        }

        //响应点区间估计
        [HttpPost]
        public ActionResult ResponsePointIntervalEstimate(double reponseProbability2, double confidenceLevel2, double cjl, double favg, double fsigma, int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(langlryExpTable.let_Id);
            ldts.RemoveRange(ldts.Count - 1, 1);
            var xOrVArray = LangleyPublic.XAndVArrays(ldts);
            xOrVArray.vArray = LangleyPublic.IsFlipTheResponse(langlryExpTable, xOrVArray.vArray);
            var lr = LangleyPublic.SelectState(langlryExpTable);
            var ies = lr.ResponsePointIntervalEstimate(xOrVArray.xArray, xOrVArray.vArray, reponseProbability2, confidenceLevel2, cjl, favg, fsigma);
            string[] value = { "(" + ies[0].Confidence.Down.ToString("f6") + "," + ies[0].Confidence.Up.ToString("f6") + ")", "(" + ies[0].Mu.Down.ToString("f6") + "," + ies[0].Mu.Up.ToString("f6") + ")", "(" + ies[0].Sigma.Down.ToString("f6") + "," + ies[0].Sigma.Up.ToString("f6") + ")", "(" + ies[1].Confidence.Down.ToString("f6") + "," + ies[1].Confidence.Up.ToString("f6") + ")", "(" + ies[1].Mu.Down.ToString("f6") + "," + ies[1].Mu.Up.ToString("f6") + ")", "(" + ies[1].Sigma.Down.ToString("f6") + "," + ies[1].Sigma.Up.ToString("f6") + ")" };
            return Json(value);
        }
        //获取let_id全部兰利法表并分页显示
        public ActionResult GetAllLangleys(int id,int page = 1, int limit = 20)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(id);
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(id);
            List<LangleyDataTable> PagesLdt = new List<LangleyDataTable>();
            int last = ldts.Count - (page - 1) * limit;
            int first = 0;
            if (page * limit < ldts.Count)
            {
                first = ldts.Count - page * limit;
            }
            for (int i = first; i < last; i++)
            {
                PagesLdt.Add(ldts[i]);
            }
            if (page == 1)
                PagesLdt.RemoveRange(PagesLdt.Count - 1, 1);
            return Json(new { code = 0, msg = "", count = ldts.Count - 1, data = LangleyPublic.Langleys(langlryExpTable, PagesLdt, first, ldts.Count) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        //批量区间估计
        public ActionResult BatchIntervalCalculation(double BatchConfidenceLevel, double yMin, double yMax, int Y_Axis, int intervalTypeSelection, double favg, double fsigma, int langlryExpTableId)
        {
            LangleyExperimentTable langlryExpTable = dbDrive.GetLangleyExperimentTable(langlryExpTableId);
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(langlryExpTable.let_Id);
            ldts.RemoveRange(ldts.Count - 1, 1);
            var xOrVArray = LangleyPublic.XAndVArrays(ldts);
            xOrVArray.vArray = LangleyPublic.IsFlipTheResponse(langlryExpTable, xOrVArray.vArray);
            var lr = LangleyPublic.SelectState(langlryExpTable);
            var srd = lr.BatchIntervalCalculate(yMax, yMin, Y_Axis, BatchConfidenceLevel, favg, fsigma, xOrVArray.xArray, xOrVArray.vArray, intervalTypeSelection);
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
    }
}