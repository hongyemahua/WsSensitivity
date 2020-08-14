using System;
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
            string[] str = { isTure.ToString(), up.μ0_final.ToString(), up.σ0_final.ToString(), up.Sigma_mu.ToString(), up.Sigma_sigma.ToString(), up.A.ToString(), up.M.ToString(), up.B.ToString(), up.b.ToString(), prec[0].ToString(), prec[1].ToString(), rpse[0].ToString(), rpse[1].ToString(), up.p.ToString(), up.G.ToString(), up.n.ToString(), up.H.ToString(),"升降法："+lr.DistributionNameAndMethodStandardName()+"/"+group};//[0]表示修改状态，[1]代表修改后的试验参数
            return Json(str);
        }

        [HttpPost]//计算当前组数据
        public ActionResult CalculateCurrentData(int udg_id)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(list_udv[0].dudt_ExperimentId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            double[] xArray = new double[list_udv.Count];
            int[] vArray = new int[list_udv.Count];
            for (int i = 0; i < list_udv.Count; i++)
            {
                xArray[i] = list_udv[i].dtup_Standardstimulus;
                vArray[i] = LiftingPublic.Filp(list_udv[i].dtup_response, upDownExperiment.udt_Flipresponse);
            }
            var up = lr.GetReturn(xArray, vArray, upDownExperiment.udt_Initialstimulus, upDownExperiment.udt_Stepd, out double z, upDownExperiment.udt_Instrumentresolution, out double z1);
            double[] prec = lr.GetPrec(up.μ0_final,up.σ0_final);
            double[] rpse = lr.ResponsePointStandardError(up.Sigma_mu,up.Sigma_sigma);
            string[] value = { up.μ0_final.ToString(), up.σ0_final.ToString(), up.Sigma_mu.ToString(), up.Sigma_sigma.ToString(), up.A.ToString(), up.M.ToString(), up.B.ToString(),up.b.ToString(),prec[0].ToString(),prec[1].ToString(),rpse[0].ToString(),rpse[1].ToString(),up.p.ToString(),up.G.ToString(),up.n.ToString(),up.H.ToString() };
            return Json(value);
        }
        [HttpPost]//前一组实验
        public ActionResult FromerGroup(int udg_id,int ExperimentalId,string setTime)
        {
            List<UpDownGroup> upDownGroups = dbDrive.GetUpDownGroups(ExperimentalId);
            //判断当前组是否为第一组
            bool isTure = false;
            int st = int.Parse(setTime);
            if (upDownGroups[0].Id != udg_id)
            {
                isTure = true;
                --st;
                for (int i = 0;i< upDownGroups.Count;i++)
                {
                    if (upDownGroups[i].Id == udg_id) {
                        udg_id = upDownGroups[i - 1].Id;
                        break;
                    }
                }
            }
            string[] str = { isTure.ToString(), st.ToString(),udg_id.ToString()};
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
            string[] str = { isTure.ToString(),ExperimentalId.ToString(), udg_id.ToString() };
            return Json(str);
        }
        //新增多组实验页面
        public ActionResult Manyexperiments(string ExperimentalId,string average,string standardDeviation)
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
            upDownDataTable.dtup_Standardstimulus = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).GetStandardStimulus(upDownDataTable.dtup_Initialstimulus);
            upDownDataTable.dtup_Initialstimulus = double.Parse(mm.stimulusQuantity); 
            upDownDataTable.dtup_response = 0;
            bool isTrue= dbDrive.Insert(upDownDataTable);
            string[] str = { isTrue.ToString(),upDownGroup.Id.ToString() };
            return Json(str);
        }
        //撤销最后一组实验数据
        [HttpPost]
        public ActionResult RevocationData(int udg_id)
        {
            List<UpDownDataTable> list_udt = dbDrive.GetUpDownDataTables(udg_id);
            UpDownDataTable upDownDataTable = list_udt[list_udt.Count - 1];
            return Json(dbDrive.Delete(upDownDataTable));
        }
        //响应不响应
        [HttpPost]
        public ActionResult InsertData(string response, int udg_id)
        {
            List<UpDownView> list_udv = dbDrive.GetUpDownViews(udg_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(list_udv[0].dudt_ExperimentId);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            list_udv[list_udv.Count - 1].dtup_response = int.Parse(response);
            double[] xArray = new double[list_udv.Count];
            int[] vArray = new int[list_udv.Count];
            for (int i = 0; i<list_udv.Count;i++)
            {
                xArray[i] = list_udv[i].dtup_Standardstimulus;
                vArray[i] = LiftingPublic.Filp(list_udv[i].dtup_response, upDownExperiment.udt_Flipresponse);
            }
            var up = lr.GetReturn(xArray,vArray,upDownExperiment.udt_Initialstimulus,upDownExperiment.udt_Stepd,out double z,upDownExperiment.udt_Instrumentresolution,out double z1);  
            UpDownDataTable upDownDataTable = new UpDownDataTable();
            upDownDataTable.dtup_DataTableId = udg_id;
            upDownDataTable.dtup_Initialstimulus = z;
            upDownDataTable.dtup_response = 0;
            upDownDataTable.dtup_Standardstimulus = z1;
            bool isTure = dbDrive.Insert(upDownDataTable);
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(upDownExperiment.id);
            string[] value = { isTure.ToString(),LiftingPublic.CurrentSetNumber(list_udg,udg_id).ToString(),(list_udv.Count + 1).ToString(),lr.StepsNumber(xArray,vArray).ToString(),z.ToString()};
            return Json(value);
        }
        #endregion

        #region 单组数据

        //单组数据区间估计
        public ActionResult LntervalEstimation(int id,string res)
        {
            ViewData["res"] = res;
            return View();
        }
        //单组试验结果&&综合计算结果响应点计算
        [HttpPost]
        public ActionResult ResponsePoint(int ExperimentalId, string average, string standardDeviation, double ResponsePointValue)
        {
            var rpc = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).ResponsePointCalculation(ResponsePointValue,double.Parse(standardDeviation),double.Parse(average));
            return Json(rpc);
        }
        //单组试验结果&&综合计算结果响应概率计算
        [HttpPost]
        public ActionResult StimulusQuantity(int ExperimentalId,string average, string standardDeviation, double StimulusQuantityValue)
        {
            var rpc = LiftingPublic.SelectState(dbDrive.GetUpDownExperiment(ExperimentalId)).ResponseProbabilityCalculation(StimulusQuantityValue, double.Parse(standardDeviation), double.Parse(average));
            return Json(rpc);
        }


        #endregion
        //综合结果计算多组试验结果
        [HttpPost]
        public ActionResult MultipleExperimentsCalculated(int id)
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return Json(1);
        }
        ////综合试验结果响应点计算
        //[HttpPost]
        //public ActionResult ResponsePointD(int ExperimentalId, string average, string standardDeviation, double ResponsePointValue)
        //{
        //    return Json("885.23");
        //}
        ////综合试验结果响应概率计算
        //[HttpPost]
        //public ActionResult StimulusQuantityD(int ExperimentalId, string average, string standardDeviation, double StimulusQuantityValue)
        //{
        //    return Json(23516.154);
        //}
        //设置升降法初始参数
        public ActionResult ParameterSettingData()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownExperiment experiment = js.Deserialize<UpDownExperiment>(stream);
            experiment.udt_Creationtime = DateTime.Now.Date;
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
            string[] str = { isTure.ToString(), upDownGroup.Id.ToString() ,experiment.udt_ProdectName };
            return Json(str);//需要传输本次实验的ID
        }

        //获取全部试验数据
        public ActionResult GetAllExperiments()
        {
            //    List<UpDownExperiment> experiments = dbDrive.GetAllExperiments();
            //    //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(1/*new { code = 0, msg = "", count = experiments.Count, data = experiments }, JsonRequestBehavior.AllowGet*/);
        }

        //删除当前组试验数据
        [HttpPost]
        public ActionResult Experiment_delete(int udg_id)
        {
            UpDownGroup upDownGroup = dbDrive.GetDownGroup(udg_id);
            return Json(dbDrive.Delete(upDownGroup));
        }

        ////查询
        [HttpPost]
        public ActionResult Experiment_query(string cpmc, DateTime startdate, DateTime stopdate)
        {
            //    List<UpDownExperiment> experiments = dbDrive.QueryExperiments(cpmc,startdate,stopdate);
            return Json(1/*new { code = 0, msg = "", count = experiments.Count, data = experiments }, JsonRequestBehavior.AllowGet*/);
        }
        //查询当前ID下的所有当组实验数据
        public ActionResult GetAllUpDownMethods(int udg_id, int page = 1, int limit = 20)
        {
            //数据的当前组ID为upd_Id
            List<UpDownDataTable> list_udt = dbDrive.GetUpDownDataTables(udg_id);
            List<GetAllUpDownMethodsModel> all_udms = new List<GetAllUpDownMethodsModel>();
            for (int i = 0;i< list_udt.Count;i++)
            {
                GetAllUpDownMethodsModel all_udm = new GetAllUpDownMethodsModel();
                all_udm.number = i + 1;
                all_udm.stimulusQuantity = list_udt[i].dtup_Initialstimulus;
                all_udm.response = list_udt[i].dtup_response;
                all_udm.standardStimulus = list_udt[i].dtup_Standardstimulus;
                all_udms.Add(all_udm);
            }
            all_udms.Reverse();
            List<GetAllUpDownMethodsModel> PagesUdt = new List<GetAllUpDownMethodsModel>();
            int last = all_udms.Count - (page - 1) * limit;
            int first = 0;
            if (page * limit < all_udms.Count)
            {
                first = all_udms.Count - page * limit;
            }
            for (int i = first; i < last; i++)
            {
                PagesUdt.Add(all_udms[i]);
            }
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = all_udms.Count, data = PagesUdt }, JsonRequestBehavior.AllowGet);
        }

        //单组试验结果数据
        public ActionResult GetAllsingleunitList(int[] i,int[] vi,int[] mi)
        {
            List<SingleExperimentTable> singleExperimentTables = new List<SingleExperimentTable>();
            for (int w = 0;w<i.Length;w++)
            {
                SingleExperimentTable singleExperimentTable = new SingleExperimentTable();
                singleExperimentTable.i = i[w];
                singleExperimentTable.i2 = i[w] * i[w];
                singleExperimentTable.i2mi1 = i[w] * i[w] * mi[w];
                singleExperimentTable.i2vi = i[w] * i[w] * vi[w];
                singleExperimentTable.imi1 = i[w] * mi[w];
                singleExperimentTable.ivi = i[w] * vi[w];
                singleExperimentTable.mi = mi[w];
                singleExperimentTable.vi = vi[w];
                singleExperimentTables.Add(singleExperimentTable);
            }
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code="",msg="",count=i.Length,data= singleExperimentTables},JsonRequestBehavior.AllowGet);
        }
        public ActionResult ComprehensiveList(int ude_id)
        {
            List<ComprehensiveResultsModel> list_crm = new List<ComprehensiveResultsModel>();
            List<UpDownView> list_udv = dbDrive.GetUpDownViews_UDEID(ude_id);
            List<UpDownGroup> list_udg = dbDrive.GetUpDownGroups(ude_id);
            UpDownExperiment upDownExperiment = dbDrive.GetUpDownExperiment(ude_id);
            var lr = LiftingPublic.SelectState(upDownExperiment);
            for (int i=0;i< list_udg.Count;i++)
            {
                ComprehensiveResultsModel comprehensiveResultsModel = new ComprehensiveResultsModel();
                List<UpDownView> upDownViews = new List<UpDownView>();
                foreach (var j in list_udv)
                {
                    if (list_udg[i].Id == j.udg_Id)
                        upDownViews.Add(j);
                }
                double[] xArray = new double[list_udv.Count];
                int[] vArray = new int[list_udv.Count];
                for (int w = 0; w < list_udv.Count; w++)
                {
                    xArray[w] = list_udv[w].dtup_Standardstimulus;
                    vArray[w] = LiftingPublic.Filp(list_udv[w].dtup_response, upDownExperiment.udt_Flipresponse);
                }
                var up = lr.GetReturn(xArray, vArray, upDownExperiment.udt_Initialstimulus, upDownExperiment.udt_Stepd, out double z, upDownExperiment.udt_Instrumentresolution, out double z1);
                double[] prec = lr.GetPrec(up.μ0_final, up.σ0_final);
                comprehensiveResultsModel.setNumber = i + 1;
                comprehensiveResultsModel.stimulusQuantity = upDownViews[0].dtup_Initialstimulus;
                comprehensiveResultsModel.stepLength = upDownViews[0].dudt_Stepd;
                comprehensiveResultsModel.sampleNumber = upDownViews.Count();
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
                comprehensiveResultsModel.percentage01 = prec[0];
                comprehensiveResultsModel.percentage999 = prec[1];
                list_crm.Add(comprehensiveResultsModel);
            }
            return Json(new { code="",msg="",count= list_crm.Count(),data= list_crm},JsonRequestBehavior.AllowGet);
        }
        #region 区间估计
        //响应概率区间估计
        [HttpPost]
        public ActionResult IntervalEstimationResponseProbability(double Srb_ProbabilityResponse,double Srb_Confidencelevel)
        {
            string[] str = {"1","2","3","4" };
            return Json(str);
        }
        //响应点区间估计
        [HttpPost]
        public ActionResult ResponsePointIntervalEstimation(double Srb_StimulusQuantity,double Srb_ConfidencelevelS)
        {
            return Json(1);
        }
        //方差函数响应概率区间估计
        [HttpPost]
        public ActionResult Fchs_IntervalEstimationResponseProbability(double Fchs_ProbabilityResponse,double xygl_zxsp)
        {
            return Json(1);
        }
        //方差函数响应点区间估计
        [HttpPost]
        public ActionResult Fchs_ResponsePointIntervalEstimation(double Fchs_StimulusQuantity,double cjl_zxsp)
        {
            return Json("1");
        }
        //似然比绘图
        [HttpPost]
        public ActionResult Likelihood()
        {
            return Json(1);
        }
        //方差函数绘图
        [HttpPost]
        public ActionResult Variancefunction()
        {
            return Json(1);
        }
        #endregion
    }
}