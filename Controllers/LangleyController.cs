using Newtonsoft.Json.Linq;
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
    struct Langley_list
    {
        public int number;
        public int Id;
        public double PrecisionInstruments;
        public double StimulusQuantityCeiling;
        public double StimulusQuantityFloor;
        public string Power;
        public string DistributionState;
        public int Correction;
        public int FlipTheResponse;
        public string ExperimentalDate;
        public int count;
    }
    public class LangleyController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        private static double pow = 0;
        private static int let_id = 1;
        private static int distributionState = 0;
        private static int standardState = 0;
        private static double[] xArray = { };
        private static int[] vArray = { };
        private static double stimulusQuantityCeiling = 0;
        private static double stimulusQuantityFloor = 0;
        private static double precisionInstruments = 0;
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
            var let = dbDrive.GetLangleyExperimentTable(let_id);
            string tc = let.let_TechnicalConditions;
            return View();
        }

        [HttpPost]
        //编辑技术条件
        public ActionResult TechnicalConditions_Update()
        {
            var let = dbDrive.GetLangleyExperimentTable(let_id);
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            //string jsonText = stream;
            JavaScriptSerializer js = new JavaScriptSerializer();
            let.let_TechnicalConditions = js.Deserialize<LangleyExperimentTable>(stream).let_TechnicalConditions;
            return Json(dbDrive.Update(let));
        }

        //增加数据
        [HttpPost]
        public JsonResult InsertData(int response)
        {
            List<LangleyDataTable> ldts = new List<LangleyDataTable>();
            LangleyDataTable ldt = new LangleyDataTable();
            ldts = dbDrive.GetAllLangleyDataTable(let_id);

            for (int i = 0; i<ldts.Count;i++)
            {
                xArray[i] = ldts[i].ldt_StimulusQuantity;
                vArray[i] = ldts[i].ldt_Response;
            }
            vArray[ldts.Count-1] = response;
            var lr = SelectState(distributionState, standardState);
            var pointCalculateValue = lr.GetResult(xArray, vArray);
            ldt = ldts[ldts.Count-1];
            ldt.ldt_Mean = pointCalculateValue.μ0_final;
            ldt.ldt_StandardDeviation = pointCalculateValue.σ0_final;
            ldt.ldt_MeanVariance = pointCalculateValue.varmu;
            ldt.ldt_StandardDeviationVariance = pointCalculateValue.varsigma;
            ldt.ldt_Covmusigma = pointCalculateValue.covmusigma;
            dbDrive.UpDate(ldt);
            //增加一条新数据
            double sq = SelectState(distributionState, standardState).CalculateStimulusQuantity(xArray, vArray, stimulusQuantityCeiling, stimulusQuantityFloor, precisionInstruments);
            return Json(dbDrive.Insert(LangleyDataTables(sq)));
        }

        //点计算
        [HttpPost]
        public JsonResult PointCalculate()
        {
            List<LangleyDataTable> ldts = new List<LangleyDataTable>();
            LangleyDataTable ldt = new LangleyDataTable();
            ldts = dbDrive.GetAllLangleyDataTable(let_id);
            for (int i = 0; i < ldts.Count; i++)
            {
                xArray[i] = ldts[i].ldt_StimulusQuantity;
                vArray[i] = ldts[i].ldt_Response;
            }
            var lr = SelectState(distributionState, standardState);
            var pointCalculateValue = lr.GetResult(xArray,vArray);
            ldt = ldts[ldts.Count-1];
            ldt.ldt_Mean = pointCalculateValue.μ0_final;
            ldt.ldt_StandardDeviation = pointCalculateValue.σ0_final;
            ldt.ldt_MeanVariance = pointCalculateValue.varmu;
            ldt.ldt_StandardDeviationVariance = pointCalculateValue.varsigma;
            ldt.ldt_Covmusigma = pointCalculateValue.covmusigma;
            double maxX = lr.MaxAndMinValue(pointCalculateValue.Maxf);
            double minX = lr.MaxAndMinValue(pointCalculateValue.Mins);
            double prec99 = lr.Prec99(pointCalculateValue.μ0_final,pointCalculateValue.σ0_final);
            double prec01 = lr.Prec01(pointCalculateValue.μ0_final, pointCalculateValue.σ0_final);
            return Json(dbDrive.UpDate(ldt));
        }

        [HttpPost]
        //响应点计算
        public ActionResult ResponsePointCalculate(double fq,double favg,double fsigma)
        {
            var lr = SelectState(distributionState, standardState);
            return Json(lr.ResponsePointCalculate(fq, favg, fsigma));
        }

        [HttpPost]
        //响应概率计算
        public ActionResult ResponseProbabilityCalculate(double fq, double favg, double fsigma)
        {
            var lr = SelectState(distributionState, standardState);
            return Json(lr.ResponseProbabilityCalculate(fq, favg, fsigma));
        }

        [HttpPost]
        //撤销
        public ActionResult RevocationData()
        {
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(let_id);
            return Json(dbDrive.Delete(ldts[ldts.Count]));
        }
          
        //修改分析参数
        public JsonResult UpdateParameter()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.Deserialize<LangleyExperimentTable>(stream);
            LangleyExperimentTable let = dbDrive.GetLangleyExperimentTable(let_id);
            let.let_DistributionState = js.Deserialize<LangleyExperimentTable>(stream).let_DistributionState;
            let.let_Correction = js.Deserialize<LangleyExperimentTable>(stream).let_Correction;
            return Json(dbDrive.Update(let));
        }

        //响应概率区间估计
        [HttpPost]
        public ActionResult ResponseProbabilityIntervalEstimate(double reponseProbability, double confidenceLevel)
        {
            var lr = SelectState(distributionState, standardState);
            var ies = lr.ResponseProbabilityIntervalEstimate(xArray,vArray, reponseProbability, confidenceLevel);
            var 单侧响应点下限 =  ies[0].Mu.Down;
            var 单侧响应点上限 = ies[0].Mu.Up;
            var 单侧均值下限 = ies[0].Sigma.Down;
            var 单侧均值上限 = ies[0].Sigma.Up;
            var 单侧标准差下限 = ies[0].Confidence.Down;
            var 单侧标准差上限 = ies[0].Confidence.Up;
            var 双侧响应点下限 = ies[1].Mu.Down;
            var 双侧响应点上限 = ies[1].Mu.Up;
            var 双侧均值下限 = ies[1].Sigma.Down;
            var 双侧均值上限 = ies[1].Sigma.Up;
            var 双侧标准差下限 = ies[1].Confidence.Down;
            var 双侧标准差上限 = ies[1].Confidence.Up;
            return Json(true);
        }

        //响应点区间估计
        [HttpPost]
        public ActionResult ResponsePointIntervalEstimate(double reponseProbability, double confidenceLevel, double cjl, double favg, double fsigma)
        {
            var lr = SelectState(distributionState, standardState);
            var ies = lr.ResponsePointIntervalEstimate(xArray,vArray, reponseProbability, confidenceLevel, cjl, favg, fsigma);
            var 单侧响应点下限 = ies[0].Mu.Down;
            var 单侧响应点上限 = ies[0].Mu.Up;
            var 单侧均值下限 = ies[0].Sigma.Down;
            var 单侧均值上限 = ies[0].Sigma.Up;
            var 单侧标准差下限 = ies[0].Confidence.Down;
            var 单侧标准差上限 = ies[0].Confidence.Up;
            var 双侧响应点下限 = ies[1].Mu.Down;
            var 双侧响应点上限 = ies[1].Mu.Up;
            var 双侧均值下限 = ies[1].Sigma.Down;
            var 双侧均值上限 = ies[1].Sigma.Up;
            var 双侧标准差下限 = ies[1].Confidence.Down;
            var 双侧标准差上限 = ies[1].Confidence.Up;
            return Json(true);
        }

        //批量区间估计
        [HttpPost]
        public ActionResult BatchIntervalCalculate(double Y_Ceiling, double Y_LowerLimit, int Y_PartitionNumber, double ConfidenceLevel, double favg, double fsigma, int intervalChoose)
        {
            var lr = SelectState(distributionState, standardState);
            var srd = lr.BatchIntervalCalculate(Y_Ceiling, Y_LowerLimit, Y_PartitionNumber, ConfidenceLevel, favg, fsigma,xArray,vArray, intervalChoose);
            return Json(true);
        }

        //兰利法查询
        public ActionResult LangleyQuery()
        {
            var lets = dbDrive.GetAllLangleyExperimentTables();
            List<string> productName = new List<string>();
            List<string> pn = new List<string>();
            foreach (var let in lets)
            {
                if (!productName.Contains(let.let_ProductName))
                    productName.Add(let.let_ProductName);
            }
            ViewData["pn"] = productName;
            return View();
        }

        //名称、时间查询
        public ActionResult Langley_query(string productName,string startTime,string endTime)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            if (startTime != "" && endTime != "")
            {
                DateTime st = Convert.ToDateTime(startTime);
                DateTime et = Convert.ToDateTime(endTime);
                lets = dbDrive.QueryLangleyExperimentTable(productName, st, et);
            }
            else
                lets = dbDrive.QueryLangleyExperimentTable(productName);
            return Json(new { code = 0, msg = "", count = lets.Count, data = lets }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        //获取全部let_id的兰利法表
        public ActionResult GetAllLangleys(int id)
        {
            List<LangleyDataTable> ldts = dbDrive.GetAllLangleyDataTable(id);
            return Json(new { code = 0, msg = "", count = ldts.Count, data = ldts }, JsonRequestBehavior.AllowGet);
        }

        //获取全部的兰利法实验
        public ActionResult GetAllLangleysExperiment()
        {
            List<LangleyExperimentTable> lets = dbDrive.GetAllLangleyExperimentTables();
            List<Langley_list> langletlists = new List<Langley_list>(); 
            for (int i = 0;i < lets.Count;i++)
            {
                Langley_list langley_List = new Langley_list();
                langley_List.number = i + 1;
                langley_List.Id = lets[i].let_Id;
                langley_List.PrecisionInstruments = lets[i].let_PrecisionInstruments;
                langley_List.StimulusQuantityFloor = lets[i].let_StimulusQuantityFloor;
                langley_List.StimulusQuantityCeiling = lets[i].let_StimulusQuantityCeiling;
                langley_List.Power = lets[i].let_Power;
                langley_List.DistributionState = DistributionState(lets[i].let_DistributionState, lets[i].let_StandardState);
                langley_List.Correction = lets[i].let_Correction;
                langley_List.count = dbDrive.GetAllLangleyDataTable(lets[i].let_Id).Count;
                langley_List.FlipTheResponse = lets[i].let_FlipTheResponse;
                langley_List.ExperimentalDate = lets[i].let_ExperimentalDate.ToString();
                langletlists.Add(langley_List);
            }
            return Json(new { code = 0, msg = "", count = lets.Count, data = langletlists }, JsonRequestBehavior.AllowGet);
        }

        private string DistributionState(int ds,int ss)
        {
            if (ds == 0 && ss == 0)
                return "正态分布标准";
            else if (ds == 0 && ss == 1)
                return "正态分布Ln";
            else if (ds == 0 && ss == 2)
                return "正态分布Log10";
            else if (ds == 0 && ss == 3)
                return "正态分布幂";
            else if (ds == 1 && ss == 0)
                return "逻辑斯谛分布标准";
            else if (ds == 1 && ss == 1)
                return "逻辑斯谛分布Ln";
            else if (ds == 1 && ss == 2)
                return "逻辑斯谛分布Log10";
            else if (ds == 1 && ss == 3)
                return "逻辑斯谛分布幂";
            throw new Exception("错误");
        }

        //删除兰利法&&全部清除
        [HttpPost]
        public ActionResult Langley_delete(int id)
        {
            var let = dbDrive.GetLangleyExperimentTable(id);
            return Json(dbDrive.Delete(let));
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
            let.let_ExperimentalDate = DateTime.Now;
            pow = Convert.ToDouble(let.let_Power);
            distributionState = let.let_DistributionState;
            standardState = let.let_StandardState;
            stimulusQuantityCeiling = let.let_StimulusQuantityCeiling;
            stimulusQuantityFloor = let.let_StimulusQuantityFloor;
            precisionInstruments = let.let_PrecisionInstruments;

            double sq = SelectState(distributionState, standardState).CalculateStimulusQuantity(xArray, vArray, stimulusQuantityCeiling, stimulusQuantityFloor, precisionInstruments);

            dbDrive.Insert(let);
            let_id = let.let_Id;
            dbDrive.Insert(LangleyDataTables(sq));
            return Json(true);
        }

        private LangleyDataTable LangleyDataTables(double sq = 0, int resp = 0, double mean = 0,double sd = 0,double mv = 0,double sdv = 0,double covmusigma= 0 ,string note = null)
        {
            LangleyDataTable ldt = new LangleyDataTable();
            ldt.ldt_ExperimentTableId = let_id;
            ldt.ldt_StimulusQuantity = sq;
            ldt.ldt_Number = dbDrive.GetAllLangleyDataTable(let_id).Count +1;
            ldt.ldt_Response = resp;
            ldt.ldt_Mean = mean;
            ldt.ldt_StandardDeviation = sd;
            ldt.ldt_MeanVariance = mv;
            ldt.ldt_StandardDeviationVariance = sdv;
            ldt.ldt_Covmusigma = covmusigma;
            ldt.ldt_Note1 = note;
            return ldt;
        }

        private LangleyAlgorithm SelectState(int let_DistributionState,int let_StandardState)
        {
            if (let_DistributionState == 0 && let_StandardState == 0)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Standard());
                return langley;
            }
            if (let_DistributionState == 0 && let_StandardState == 1)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Ln());
                return langley;
            }
            if (let_DistributionState == 0 && let_StandardState == 2)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Log());
                return langley;
            }
            if (let_DistributionState == 0 && let_StandardState == 3)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Normal(), new Pow(pow));
                return langley;
            }
            if (let_DistributionState == 1 && let_StandardState == 0)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Standard());
                return langley;
            }
            if (let_DistributionState == 1 && let_StandardState == 1)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Ln());
                return langley;
            }
            if (let_DistributionState == 1 && let_StandardState == 2)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Log());
                return langley;
            }
            if (let_DistributionState == 1 && let_StandardState == 3)
            {
                LangleyAlgorithm langley = new LangleyAlgorithm(new Logistic(), new Pow(pow));
                return langley;
            }
            throw new Exception("错误");
        }
    }
}