﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models.IDbDrives;
using WsSensitivity.Models;
using AlgorithmReconstruct;
using System.Web.Security;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web.WebSockets;
using System.Net.Sockets;
using NPOI.OpenXmlFormats.Dml;

namespace WsSensitivity.Controllers
{
    public class UpDownMethodController : Controller
    {
        public static UpDownExperiment ude;
        public static List<UpDownView> udv;
        IDbDrive dbDrive = new LingImp();
        public ActionResult UpDownMethod(int udg_id)
        {
            UpDownMethodModel upDM = new UpDownMethodModel();
            UpDownGroup upDownGroup = dbDrive.GetDownGroup(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(upDownGroup.dudt_ExperimentId);
            List<UpDownGroup> upDownGroups = dbDrive.GetUpDownGroups(upDownExperiment.id);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            upDM.id = udg_id;
            upDM.ExperimentalId = upDownGroup.dudt_ExperimentId;
            upDM.ExperimentalLabelName = lr.DistributionNameAndMethodStandardName();
            upDM.ProductName = upDownExperiment.udt_ProdectName;
            upDM.Groupingstate = upDownExperiment.udt_Groupingstate == 0 ? "不分组" : "多组试验";
            upDM.IsLastGroup = upDownGroups[0].Id == udg_id ? true : false;
            int count = 0;
            for (int i = 0; i < upDownGroups.Count; i++)
            {
                count++;
                if (upDownGroups[i].Id == udg_id)
                    break;
            }
            upDM.GroupNumber = count;
            return View(upDM);
        }
        public ActionResult Query()
        {
            var udes = dbDrive.GetUpDownExperiments(LangleyPublic.adminId);
            List<string> productName = new List<string>();
            foreach (var ude in udes)
            {
                if (!productName.Contains(ude.udt_ProdectName))
                    productName.Add(ude.udt_ProdectName);
            }
            ViewData["pn"] = productName;
            return View();
        }
        public ActionResult ParameterSetting()
        {
            return View();
        }


        #region 实验数据table页面事件


        //修改技术条件页面
        public ActionResult Add(int udg_id)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(list_udv[0].dudt_ExperimentId);
            ude = upDownExperiment;
            udv = list_udv;
            return View(upDownExperiment);
        }
        //修改技术条件方法
        [HttpPost]
        public ActionResult UpDownParameter_add()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownExperiment upDownExperiment = ude;
            upDownExperiment.udt_Technicalconditions = js.Deserialize<UpDownExperiment>(stream).udt_Technicalconditions;
            return Json(dbDrive.Update(upDownExperiment));
        }
        //修改升降法分析参数页面
        public ActionResult AlterParameterSetting(int udg_id)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(list_udv[0].dudt_ExperimentId);
            ude = upDownExperiment;
            udv = list_udv;
            return View(upDownExperiment);
        }
        //修改分析参数
        [HttpPost]
        public ActionResult UpdateUPparSeting()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownExperiment upDownExperiment = ude;
            upDownExperiment.udt_Distribution = js.Deserialize<UpDownExperiment>(stream).udt_Distribution;
            var isTure = dbDrive.Update(upDownExperiment);
            double[] xArray = new double[udv.Count];
            int[] vArray = new int[udv.Count];
            for (int i = 0; i < udv.Count; i++)
            {
                xArray[i] = udv[i].dtup_Standardstimulus;
                vArray[i] = LiftingPublic.Filp(udv[i].dtup_response, upDownExperiment.udt_Flipresponse);
            }
            var lr = LiftingPublic.SelectState(upDownExperiment);
            var up = lr.GetReturn(xArray, vArray, upDownExperiment.udt_Initialstimulus, upDownExperiment.udt_Stepd, out double z, upDownExperiment.udt_Instrumentresolution, out double z1);
            double[] prec = lr.GetPrec(up.μ0_final, up.σ0_final);
            var group = upDownExperiment.udt_Groupingstate == 0 ? "不分组" : "多组试验";
            double[] rpse = lr.ResponsePointStandardError(up.Sigma_mu, up.Sigma_sigma);
            string[] str = { isTure.ToString(), up.μ0_final.ToString(), up.σ0_final.ToString(), up.Sigma_mu.ToString(), up.Sigma_sigma.ToString(), up.A.ToString(), up.M.ToString(), up.B.ToString(), up.b.ToString(), prec[1].ToString(), prec[0].ToString(), rpse[0].ToString(), rpse[1].ToString(), up.p.ToString(), up.G.ToString(), up.n.ToString(), up.H.ToString(), "升降法：" + lr.DistributionNameAndMethodStandardName() + "/" + group };//[0]表示修改状态，[1]代表修改后的试验参数
            return Json(str);
        }

        [HttpPost]//计算当前组数据
        public ActionResult CalculateCurrentData(int udg_id)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(list_udv[0].dudt_ExperimentId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            var up = LiftingPublic.Upanddown(list_udv, upDownExperiment, lr);

            double[] prec = lr.GetPrec(up.μ0_final, up.σ0_final);
            double[] rpse = lr.ResponsePointStandardError(up.Sigma_mu, up.Sigma_sigma);
            string[] value = { up.μ0_final.ToString(), up.σ0_final.ToString(), up.Sigma_mu.ToString(), up.Sigma_sigma.ToString(), up.A.ToString(), up.M.ToString(), up.B.ToString(), up.b.ToString(), prec[0].ToString(), prec[1].ToString(), rpse[0].ToString(), rpse[1].ToString(), up.p.ToString(), up.G.ToString(), up.n.ToString(), up.H.ToString() };
            return Json(value);
        }
        [HttpPost]//前一组实验
        public ActionResult FromerGroup(int udg_id, int ExperimentalId, string setTime)
        {
            List<UpDownGroup> upDownGroups = dbDrive.GetUpDownGroups(ExperimentalId);
            //判断当前组是否为第一组
            bool isTure = false;
            int st = int.Parse(setTime);
            if (upDownGroups[0].Id != udg_id)
            {
                isTure = true;
                --st;
                for (int i = 0; i < upDownGroups.Count; i++)
                {
                    if (upDownGroups[i].Id == udg_id) {
                        udg_id = upDownGroups[i - 1].Id;
                        break;
                    }
                }
            }
            string[] str = { isTure.ToString(), st.ToString(), udg_id.ToString() };
            return Json(str);
        }
        [HttpPost]//后一组实验
        public ActionResult AfterGroup(int udg_id, int ExperimentalId, string setTime)
        {
            List<UpDownGroup> upDownGroups = dbDrive.GetUpDownGroups(ExperimentalId);
            //判断当前组是否为最后一组
            bool isTure = true;
            int st = int.Parse(setTime);
            if (upDownGroups[upDownGroups.Count - 1].Id != udg_id)
            {
                isTure = false;
                ++st;
                for (int i = 0; i < upDownGroups.Count; i++)
                {
                    if (upDownGroups[i].Id == udg_id) {
                        udg_id = upDownGroups[i + 1].Id;
                        break;
                    }
                }
            }
            string[] str = { isTure.ToString(), ExperimentalId.ToString(), udg_id.ToString() };
            return Json(str);
        }
        //新增多组实验页面
        public ActionResult Manyexperiments(string ExperimentalId, string average, string standardDeviation)
        {
            int Experimentalid = Int32.Parse(ExperimentalId);
            ManyexperimentsModel manyexperimentsModel = new ManyexperimentsModel();
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(Experimentalid);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(Experimentalid);
            manyexperimentsModel.previousSetNumber = list_udg[list_udg.Count - 1].dudt_Stepd;
            manyexperimentsModel.currentSetNumber = list_udg.Count + 1;
            manyexperimentsModel.ProductName = upDownExperiment.udt_ProdectName;
            manyexperimentsModel.stimulusQuantity = average;
            manyexperimentsModel.stepLength = standardDeviation;
            manyexperimentsModel.distribution = LiftingPublic.SelectState(upDownExperiment).DistributionNameAndMethodStandardName();
            return View(manyexperimentsModel);
        }
        //多组实验参数设置
        [HttpPost]
        public ActionResult ManyexperimentsSetings(int ExperimentalId)
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            ManyexperimentsModel mm = js.Deserialize<ManyexperimentsModel>(stream);
            UpDownGroup upDownGroup = new UpDownGroup();
            UpDownDataTable upDownDataTable = new UpDownDataTable();
            upDownGroup.dudt_ExperimentId = ExperimentalId;
            upDownGroup.dudt_Stepd = double.Parse(mm.stepLength);
            dbDrive.Insert(upDownGroup);
            upDownDataTable.dtup_DataTableId = upDownGroup.Id;
            upDownDataTable.dtup_Initialstimulus = double.Parse(mm.stimulusQuantity);
            upDownDataTable.dtup_Standardstimulus = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).GetStandardStimulus(upDownDataTable.dtup_Initialstimulus);
            upDownDataTable.dtup_response = 0;
            bool isTure = dbDrive.Insert(upDownDataTable);
            string[] value = { isTure.ToString(), upDownGroup.Id.ToString() };
            return Json(value);
        }
        //撤销最后一组实验数据
        [HttpPost]
        public ActionResult RevocationData(int udg_id, int ExperimentalId)
        {
            List<UpDownDataTable> list_udt = dbDrive.GetUpDownDataTables(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            var xAndV = LiftingPublic.GetXArrayAndVArray(list_udt, upDownExperiment);
            if (list_udt.Count == 1)
            {
                string[] value = { "false", list_udt.Count.ToString(), list_udt[list_udt.Count - 1].dtup_Initialstimulus.ToString(), lr.StepsNumber(xAndV.xArray, xAndV.vArray).ToString() };
                return Json(value);
            }

            UpDownDataTable upDownDataTable = list_udt[list_udt.Count - 1];
            var isTure = dbDrive.Delete(upDownDataTable);
            List<UpDownDataTable> list_udtDelete = dbDrive.GetUpDownDataTables(udg_id);
            var xAndVDelete = LiftingPublic.GetXArrayAndVArray(list_udtDelete, upDownExperiment);
            string[] valueDelete = { isTure.ToString(), (list_udt.Count - 1).ToString(), list_udtDelete[list_udtDelete.Count - 1].dtup_Initialstimulus.ToString(), lr.StepsNumber(xAndVDelete.xArray, xAndVDelete.vArray).ToString() };
            return Json(valueDelete);
        }
        //响应不响应
        [HttpPost]
        public ActionResult InsertData(string response, int udg_id, int ExperimentalId)
        {
            List<UpDownDataTable> list_udt = dbDrive.GetUpDownDataTables(udg_id);
            UpDownGroup upDownGroup = dbDrive.GetDownGroup(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            list_udt[list_udt.Count - 1].dtup_response = int.Parse(response);
            var xAndV = LiftingPublic.GetXArrayAndVArray(list_udt, upDownExperiment);
            var up = lr.GetReturn(xAndV.xArray, xAndV.vArray, upDownExperiment.udt_Initialstimulus, upDownGroup.dudt_Stepd, out double z, upDownExperiment.udt_Instrumentresolution, out double z1);
            UpDownDataTable upDownDataTable = new UpDownDataTable();
            upDownDataTable.dtup_DataTableId = udg_id;
            upDownDataTable.dtup_Initialstimulus = z1;
            upDownDataTable.dtup_response = 0;
            upDownDataTable.dtup_Standardstimulus = z;
            bool isTure = dbDrive.Insert(upDownDataTable);
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(upDownExperiment.id);
            List<UpDownDataTable> list_udtInsert = dbDrive.GetUpDownDataTables(udg_id);
            var xAndVInsert = LiftingPublic.GetXArrayAndVArray(list_udtInsert, upDownExperiment);
            string[] value = { isTure.ToString(), LiftingPublic.CurrentSetNumber(list_udg, udg_id).ToString(), (list_udtInsert.Count).ToString(), lr.StepsNumber(xAndVInsert.xArray, xAndVInsert.vArray).ToString(), z1.ToString() };
            return Json(value);
        }
        #endregion

        #region 单组数据

        //单组数据区间估计
        public ActionResult LntervalEstimation(int udg_id, int ExperimentalId, string res, string avg, string sigma, string sigmaavg, string sigmasigma, int text)
        {
            ViewData["res"] = res;
            ViewData["udg_id"] = udg_id;
            ViewData["ExperimentalId"] = ExperimentalId;
            ViewData["avg"] = double.Parse(avg.Replace(" ",""));
            ViewData["sigma"] = double.Parse(sigma.Replace(" ", ""));
            ViewData["sigmaavg"] = double.Parse(sigmaavg.Replace(" ", ""));
            ViewData["sigmasigma"] = double.Parse(sigmasigma.Replace(" ", ""));
            ViewData["text"] = text;
            return View();
        }
        //单组试验结果&&综合计算结果响应点计算
        [HttpPost]
        public ActionResult ResponsePoint(int ExperimentalId, string average, string standardDeviation, double ResponsePointValue)
        {
            var rpc = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).ResponsePointCalculation(ResponsePointValue, double.Parse(standardDeviation), double.Parse(average));
            return Json(rpc);
        }
        //单组试验结果&&综合计算结果响应概率计算
        [HttpPost]
        public ActionResult StimulusQuantity(int ExperimentalId, string average, string standardDeviation, double StimulusQuantityValue)
        {
            var rpc = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).ResponseProbabilityCalculation(StimulusQuantityValue, double.Parse(average), double.Parse(standardDeviation));
            return Json(rpc);
        }

        #endregion
        //综合结果计算多组试验结果
        [HttpPost]
        public ActionResult MultipleExperimentsCalculated(int ExperimentalId)
        {
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(ExperimentalId);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            int[] nj = new int[list_udg.Count];
            double[] Gj = new double[list_udg.Count];
            double[] Hj = new double[list_udg.Count];
            double[] muj = new double[list_udg.Count];
            double[] sigmaj = new double[list_udg.Count];
            int n = 0;
            for (int i = 0; i < list_udg.Count; i++)
            {
                List<UpDownDataTable> upDownDataTables = dbDrive.GetUpDownDataTables(list_udg[i].Id);
                var xAndV = LiftingPublic.GetXArrayAndVArray(upDownDataTables, upDownExperiment);
                var up = lr.GetReturn(xAndV.xArray, xAndV.vArray, upDownDataTables[0].dtup_Initialstimulus, list_udg[i].dudt_Stepd, out double z, upDownExperiment.udt_Instrumentresolution, out double z1);
                nj[i] = up.n;
                Gj[i] = up.G;
                Hj[i] = up.H;
                muj[i] = up.μ0_final;
                sigmaj[i] = up.σ0_final;
                n += up.n;
            }
            var mtr = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).MultigroupTestResult(nj, Gj, Hj, muj, sigmaj);
            string[] value = { mtr.μ0_final.ToString(), mtr.σ0_final.ToString(), mtr.Sigma_mu.ToString(), mtr.Sigma_sigma.ToString(), mtr.prec01.ToString(), mtr.prec999.ToString(), mtr.rpse01.ToString(), mtr.rpse999.ToString(), n.ToString() };
            return Json(value);
        }

        //设置升降法初始参数
        public ActionResult ParameterSettingData()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownExperiment experiment = js.Deserialize<UpDownExperiment>(stream);
            experiment.udt_Creationtime = DateTime.Now;
            experiment.udt_RecordEmployeeId = LangleyPublic.adminId;
            dbDrive.Insert(experiment);
            UpDownGroup upDownGroup = new UpDownGroup();
            upDownGroup.dudt_ExperimentId = experiment.id;
            upDownGroup.dudt_Stepd = experiment.udt_Stepd;
            dbDrive.Insert(upDownGroup);
            UpDownDataTable upDownDataTable = new UpDownDataTable();
            upDownDataTable.dtup_DataTableId = upDownGroup.Id;
            upDownDataTable.dtup_Standardstimulus = LiftingPublic.SelectState(experiment).GetStandardStimulus(experiment.udt_Initialstimulus);
            upDownDataTable.dtup_Initialstimulus = experiment.udt_Initialstimulus;
            upDownDataTable.dtup_response = 0;
            var isTure = dbDrive.Insert(upDownDataTable);
            string[] str = { isTure.ToString(), upDownGroup.Id.ToString(), experiment.udt_ProdectName };
            return Json(str);//需要传输本次实验的ID
        }

        //获取全部试验数据
        public ActionResult GetAllExperiments(int page = 1, int limit = 20)
        {
            List<UpDownExperiment> udes = dbDrive.GetUpDownExperiments(LangleyPublic.adminId);
            List<UpDownExperiment> PagesLet = new List<UpDownExperiment>();
            int last = udes.Count - (page - 1) * limit;
            int first = 0;
            if (page * limit < udes.Count)
            {
                first = udes.Count - page * limit;
            }
            for (int i = first; i < last; i++)
            {
                PagesLet.Add(udes[i]);
            }
            return Json(new { code = 0, msg = "", fenye = 5, count = udes.Count, data = LiftingPublic.GetQueryModels(dbDrive, PagesLet, first) }, JsonRequestBehavior.AllowGet);
        }

        //删除当前组试验数据
        [HttpPost]
        public ActionResult Group_delete(int udg_id)
        {
            UpDownGroup upDownGroup = dbDrive.GetDownGroup(udg_id);
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(upDownGroup.dudt_ExperimentId);
            bool isTure = dbDrive.Delete(upDownGroup);
            int upDownGroupId = -1;
            if (list_udg[0].Id != udg_id)
            {
                upDownGroupId = list_udg[list_udg.Count - 2].Id;
            }
            string[] value = { isTure.ToString(), upDownGroupId.ToString() };
            return Json(value);
        }

        //删除总数据
        [HttpPost]
        public ActionResult Experiment_delete(int ude_id)
        {
            var ude = dbDrive.GetUpDownExperiment(ude_id);
            return Json(dbDrive.Delete(ude));
        }

        ////查询
        [HttpPost]
        public ActionResult Experiment_query(string cpmc, string startdate, string stopdate)
        {
            List<UpDownExperiment> udes = dbDrive.GetUpDownExperiments(LangleyPublic.adminId);
            if (startdate != "" && stopdate != "")
            {
                DateTime st = Convert.ToDateTime(startdate);
                DateTime et = Convert.ToDateTime(stopdate);
                udes = dbDrive.QueryExperimentTable(cpmc, st, et.AddDays(1),LangleyPublic.adminId);
            }
            else
                udes = dbDrive.QueryExperimentTable(cpmc,LangleyPublic.adminId);
            return Json(new { code = 0, msg = "", fenye = 5, count = udes.Count, data = LiftingPublic.GetQueryModels(dbDrive, udes) }, JsonRequestBehavior.AllowGet);
        }
        //查询当前ID下的所有当组实验数据
        public ActionResult GetAllUpDownMethods(int udg_id, int page = 1, int limit = 20)
        {
            //数据的当前组ID为upd_Id
            List<UpDownDataTable> list_udt = dbDrive.GetUpDownDataTables(udg_id);
            List<GetAllUpDownMethodsModel> list_udmm = new List<GetAllUpDownMethodsModel>(); 
            for (int i = 0;i< list_udt.Count;i++)
            {
                GetAllUpDownMethodsModel all_udm = new GetAllUpDownMethodsModel();
                all_udm.number = i + 1;
                all_udm.stimulusQuantity = list_udt[i].dtup_Initialstimulus;
                all_udm.response = list_udt[i].dtup_response;
                all_udm.standardStimulus = list_udt[i].dtup_Standardstimulus;
                all_udm.count = list_udt.Count;
                list_udmm.Add(all_udm);
            }
            list_udmm.Reverse();
            
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = list_udt.Count, data = list_udmm }, JsonRequestBehavior.AllowGet);
        }

        //单组试验结果数据
        public ActionResult GetAllsingleunitList(int udg_id)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(list_udv[0].dudt_ExperimentId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            var up = LiftingPublic.Upanddown(list_udv, upDownExperiment, lr);
            List<SingleExperimentTable> singleExperimentTables = new List<SingleExperimentTable>();
            for (int w = 0;w< up.result_i.Length;w++)
            {
                SingleExperimentTable singleExperimentTable = new SingleExperimentTable();
                singleExperimentTable.i = up.result_i[w];
                singleExperimentTable.i2 = up.result_i[w] * up.result_i[w];
                singleExperimentTable.i2mi1 = up.result_i[w] * up.result_i[w] * up.mi[w];
                singleExperimentTable.i2vi = up.result_i[w] * up.result_i[w] * up.vi[w];
                singleExperimentTable.imi1 = up.result_i[w] * up.mi[w];
                singleExperimentTable.ivi = up.result_i[w] * up.vi[w];
                singleExperimentTable.mi = up.mi[w];
                singleExperimentTable.vi = up.vi[w];
                singleExperimentTables.Add(singleExperimentTable);
            }
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code="",msg="",count= up.result_i.Length,data= singleExperimentTables},JsonRequestBehavior.AllowGet);
        }
        public ActionResult ComprehensiveList(int ude_id)
        {
            List<ComprehensiveResultsModel> list_crm = new List<ComprehensiveResultsModel>();
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(ude_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ude_id);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            for (int i=0;i< list_udg.Count;i++)
            {
                ComprehensiveResultsModel comprehensiveResultsModel = new ComprehensiveResultsModel();
                List<UpDownView> list_udv = dbDrive.GetUpDownViews(list_udg[i].Id);
                var up = LiftingPublic.Upanddown(list_udv, upDownExperiment,lr);
                double[] prec = lr.GetPrec(up.μ0_final, up.σ0_final);
                comprehensiveResultsModel.setNumber = i + 1;
                comprehensiveResultsModel.stimulusQuantity = list_udv[0].dtup_Initialstimulus;
                comprehensiveResultsModel.stepLength = list_udv[0].dudt_Stepd;
                comprehensiveResultsModel.sampleNumber = list_udv.Count();
                comprehensiveResultsModel.average = up.μ0_final;
                comprehensiveResultsModel.standardDeviation = up.σ0_final;
                comprehensiveResultsModel.msde = up.Sigma_mu;
                comprehensiveResultsModel.sdsde = up.Sigma_sigma;
                comprehensiveResultsModel.temptationNumber = up.n;
                comprehensiveResultsModel.G = up.G;
                comprehensiveResultsModel.H = up.H;
                comprehensiveResultsModel.A = up.A;
                comprehensiveResultsModel.B = up.B;
                comprehensiveResultsModel.M = up.M;
                comprehensiveResultsModel.b = up.b;
                comprehensiveResultsModel.p = up.p;
                comprehensiveResultsModel.percentage01 = prec[1];
                comprehensiveResultsModel.percentage999 = prec[0];
                list_crm.Add(comprehensiveResultsModel);
            }
            list_crm.Reverse();
            return Json(new { code="",msg="",count= list_crm.Count(),data= list_crm},JsonRequestBehavior.AllowGet);
        }
        #region 区间估计
        //响应概率区间估计
        [HttpPost]
        public ActionResult IntervalEstimationResponseProbability(int ExperimentalId, int udg_id,double Srb_ProbabilityResponse,double Srb_Confidencelevel, double favg, double fsigma)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            double[] xArray = new double[list_udv.Count];
            int[] vArray = new int[list_udv.Count];
            for (int i = 0; i < list_udv.Count; i++)
            {
                xArray[i] = list_udv[i].dtup_Initialstimulus;
                vArray[i] = LiftingPublic.Filp(list_udv[i].dtup_response, upDownExperiment.udt_Flipresponse);
            }
            var ies = LiftingPublic.SelectState(upDownExperiment).ResponseProbabilityIntervalEstimated(Srb_ProbabilityResponse, Srb_Confidencelevel,xArray,vArray, favg, fsigma);
            string[] str = { "("+ies[0].Confidence.Down.ToString("f6") + "," + ies[0].Confidence.Up.ToString("f6") + ")", "(" + ies[0].Mu.Down.ToString("f6") + "," + ies[0].Mu.Up.ToString("f6") + ")", "(" + ies[0].Sigma.Down.ToString("f6") + "," + ies[0].Sigma.Up.ToString("f6") + ")", "(" + ies[1].Confidence.Down.ToString("f6") + "," + ies[1].Confidence.Up.ToString("f6") + ")", "(" + ies[1].Mu.Down.ToString("f6") + "," + ies[1].Mu.Up.ToString("f6") + ")", "(" + ies[1].Sigma.Down.ToString("f6") + "," + ies[1].Sigma.Up.ToString("f6") +")"};
            return Json(str);
        }
        //响应点区间估计
        [HttpPost]
        public ActionResult ResponsePointIntervalEstimation(int ExperimentalId, int udg_id, double Srb_ProbabilityResponse, double Srb_Confidencelevel,double fq, double favg, double fsigma)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            double[] xArray = new double[list_udv.Count];
            int[] vArray = new int[list_udv.Count];
            for (int i = 0; i < list_udv.Count; i++)
            {
                xArray[i] = list_udv[i].dtup_Initialstimulus;
                vArray[i] = LiftingPublic.Filp(list_udv[i].dtup_response, upDownExperiment.udt_Flipresponse);
            }
            var ies = LiftingPublic.SelectState(upDownExperiment).ResponsePointIntervalEstimated(Srb_ProbabilityResponse, Srb_Confidencelevel, xArray, vArray,fq, favg, fsigma);
            string[] str = { "(" + ies[0].Confidence.Down.ToString("f6") + "," + ies[0].Confidence.Up.ToString("f6") + ")", "(" + ies[0].Mu.Down.ToString("f6") + "," + ies[0].Mu.Up.ToString("f6") + ")", "(" + ies[0].Sigma.Down.ToString("f6") + "," + ies[0].Sigma.Up.ToString("f6") + ")", "(" + ies[1].Confidence.Down.ToString("f6") + "," + ies[1].Confidence.Up.ToString("f6") + ")", "(" + ies[1].Mu.Down.ToString("f6") + "," + ies[1].Mu.Up.ToString("f6") + ")", "(" + ies[1].Sigma.Down.ToString("f6") + "," + ies[1].Sigma.Up.ToString("f6") + ")" };
            return Json(str);
        }
        //方差函数响应概率区间估计
        [HttpPost]
        public ActionResult Fchs_IntervalEstimationResponseProbability(int ExperimentalId,double Fchs_ProbabilityResponse,double xygl_zxsp,int textNumber, double favg, double fsigma, double fsigmaavg, double fsigmasigma)
        {
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            var vfr = LiftingPublic.SelectState(upDownExperiment).VarianceFunctionResponseProbabilityIntervalEstimated(Fchs_ProbabilityResponse, xygl_zxsp, textNumber, favg, fsigma, fsigmaavg, fsigmasigma);
            string[] value = { "(" + vfr[1].ToString("f6") + "," + vfr[0].ToString("f6") + ")", "(" + vfr[3].ToString("f6") + "," + vfr[2].ToString("f6") + ")", "(" + vfr[5].ToString("f6") + "," + vfr[4].ToString("f6") + ")", "(" + vfr[7].ToString("f6") + "," + vfr[6].ToString("f6") + ")" };
            return Json(value);
        }
        //方差函数响应点区间估计
        [HttpPost]
        public ActionResult Fchs_ResponsePointIntervalEstimation(int ExperimentalId, double Fchs_StimulusQuantity,double cjl_zxsp, int textNumber, double favg, double fsigma, double fsigmaavg, double fsigmasigma)
        {
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
            var vfr = LiftingPublic.SelectState(upDownExperiment).VarianceFunctionResponsePointIntervalEstimated(cjl_zxsp, textNumber, Fchs_StimulusQuantity, favg, fsigma, fsigmaavg, fsigmasigma);
            string[] value = { "(" + vfr[1].ToString("f6") + "," + vfr[0].ToString("f6") + ")", "(" + vfr[3].ToString("f6") + "," + vfr[2].ToString("f6") + ")", "(" + vfr[5].ToString("f6") + "," + vfr[4].ToString("f6") + ")", "(" + vfr[7].ToString("f6") + "," + vfr[6].ToString("f6") + ")" };
            return Json(value);
        }
        //似然比绘图
        [HttpPost]
        public ActionResult Likelihood(double BatchConfidenceLevel,double yMin,double yMax,int Y_Axis,int intervalTypeSelection,double favg,double fsigma,int ExperimentalId,int udg_id)
        {
            UpDownExperiment ude = dbDrive.GetUpDownExperiment(ExperimentalId);
            List<UpDownDataTable> list_udt = dbDrive.GetUpDownDataTables(udg_id);
            var lr = LiftingPublic.SelectState(ude);
            double[] xArray = new double[list_udt.Count];
            int[] vArray = new int[list_udt.Count];
            for (int i = 0; i < list_udt.Count; i++)
            {
                xArray[i] = list_udt[i].dtup_Initialstimulus;
                vArray[i] = LiftingPublic.Filp(list_udt[i].dtup_response, ude.udt_Flipresponse);
            }
            var srd = lr.QuasiLikelihoodRatioMethod(yMax, yMin, Y_Axis, BatchConfidenceLevel, favg, fsigma, xArray, vArray, intervalTypeSelection);
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
        //方差函数绘图
        [HttpPost]
        public ActionResult Variancefunction(double BatchConfidenceLevel, double yMin, double yMax, int Y_Axis, int intervalTypeSelection, double favg, double fsigma, int ExperimentalId,int textNumber,double fsigmaavg,double fsigmasigma)
        {
            UpDownExperiment ude = dbDrive.GetUpDownExperiment(ExperimentalId);
            var lr = LiftingPublic.SelectState(ude);
            var srd = lr.VarianceFunctionMethod(yMax, yMin, Y_Axis, BatchConfidenceLevel, favg, fsigma, intervalTypeSelection, textNumber, fsigmaavg, fsigmasigma);
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
                LangleyPublic.incredibleIntervalType = "GJB377单侧区间估计方法";
            else
                LangleyPublic.incredibleIntervalType = "GJB377双侧区间估计方法";
            LangleyPublic.incredibleLevelName = BatchConfidenceLevel.ToString();
            return Json(true);
        }
        #endregion

        [HttpPost]
        //导出excel
        public ActionResult GropingExcel(int udg_id,int ExperimentalId,int grop)
        {
            var strFullName = "";
            try
            {
                UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ExperimentalId);
                List<UpDownView> upDownViews = dbDrive.GetUpDownViews(udg_id);
                List<UpDownView> upDownViews1 = dbDrive.GetUpDownViews_UDEID(ExperimentalId);
                List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(ExperimentalId);
                var lr = LiftingPublic.SelectState(upDownExperiment);
                int[] nj = new int[list_udg.Count];
                double[] Gj = new double[list_udg.Count];
                double[] Hj = new double[list_udg.Count];
                double[] muj = new double[list_udg.Count];
                double[] sigmaj = new double[list_udg.Count];
                for (int i = 0; i < list_udg.Count; i++)
                {
                    List<UpDownDataTable> upDownDataTables = dbDrive.GetUpDownDataTables(list_udg[i].Id);
                    var xAndV = LiftingPublic.GetXArrayAndVArray(upDownDataTables, upDownExperiment);
                    var up = lr.GetReturn(xAndV.xArray, xAndV.vArray, upDownDataTables[0].dtup_Standardstimulus, list_udg[i].dudt_Stepd, out double z, upDownExperiment.udt_Instrumentresolution, out double z1);
                    nj[i] = up.n;
                    Gj[i] = up.G;
                    Hj[i] = up.H;
                    muj[i] = up.μ0_final;
                    sigmaj[i] = up.σ0_final;
                }
                strFullName =  FreeSpire.UpDownFreeSpireExcel(upDownExperiment, upDownViews,grop,upDownViews1,nj,Gj,Hj,muj,sigmaj,lr, list_udg);
            }
            catch (Exception ex) { }
            return Json(strFullName, JsonRequestBehavior.AllowGet);
        }
    }
}