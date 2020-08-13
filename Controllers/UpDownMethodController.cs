using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models.IDbDrives;
using WsSensitivity.Models.UpDown;

namespace WsSensitivity.Controllers
{
    public class SingleunitListData
    {
        public int i;
        public int i2;
        public int vi;
        public int mi;
        public int ivi;
        public int i2vi;
        public int imi1;
        public int i2mi1;
    }
    public class ComprehensiveListData
    {
        public int Groupid;
        public double Udt_Initialstimulus;
        public double Udt_Stepd;
        public int Datacount;
        public double Avg;
        public double Standarddeviation;
        public double Standarderrormean;
        public double Standarddeviationerror;
        public double Analyzewithheuristics;
        public double G;
        public double H;
        public double A;
        public double B;
        public double M;
        public double b;
        public double p;
        public double Zeroone;
        public double Zeronine;
    }

    public class UpDownMethodController : Controller
    {
        private static int upd_Id;
        IDbDrive dbDrive = new LingImp();
        public static UpDownPageShows upDownMethod = new UpDownPageShows();
        public ActionResult UpDownMethod(int upd_id)
        {
            int udt_Groupingstate = 0;
            upd_Id = upd_id;
            upDownMethod.id = upd_Id;
            upDownMethod.ProductName = "测试1";
            upDownMethod.Standardstate = "标准";
            if (udt_Groupingstate == 1)
                upDownMethod.Groupingstate = "不分组";
            else upDownMethod.Groupingstate = "多组试验";
            upDownMethod.Distribution = "正态";
            upDownMethod.Methodstate = "传统法";
            return View(upDownMethod);
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
        public ActionResult Add(int upd_id)
        {
            UpDownExperiment upDown = new UpDownExperiment();
            upDown.udt_Technicalconditions = "升降法技术条件" + upd_id;
            return View(upDown);
        }
        //修改技术条件方法
        [HttpPost]
        public ActionResult UpDownParameter_add()
        {
            return Json(1);
        }
        //修改升降法分析参数页面
        public ActionResult AlterParameterSetting(int id)
        {
            return View();
        }
        //修改分析参数
        [HttpPost]
        public ActionResult UpdateUPparSeting()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownExperiment upDownExperiment = js.Deserialize<UpDownExperiment>(stream);
            string[] str = { "1", "试验数据/分布/标准/分组" };//[0]表示修改状态，[1]代表修改后的试验参数
            return Json(str);
        }

        [HttpPost]//计算当前组数据
        public ActionResult CalculateCurrentData(int upd_id)
        {
            return Json(1);
        }
        [HttpPost]//前一组实验
        public ActionResult FromerGroup(int upd_id)//2
        {
            upd_Id = upd_id - 1;
            List<string> str = new List<string> { "1", upd_Id + "" };
            return Json(str);
        }
        [HttpPost]//后一组实验
        public ActionResult AfterGroup(int upd_id)
        {
            //upd_Id = upd_id;
            upd_Id = upd_id + 1;
            List<string> str = new List<string>();
            str.Add(upd_Id + "");
            if (upd_Id > 2)
            {//当前组为最后一组
                str.Add("0");
                return Json(str);
            }
            else
            {
                str.Add("1");
                return Json(str);//当前组不为最后一组
            }
        }
        //新增多组实验页面
        public ActionResult Manyexperiments(int id)
        {


            return View();
        }
        //多组实验参数设置
        [HttpPost]
        public ActionResult ManyexperimentsSetings()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownGroup upDownGroup = js.Deserialize<UpDownGroup>(stream);
            UpDownDataTable upDownDataTable = js.Deserialize<UpDownDataTable>(stream);
            //以下为测试数据
            upDownGroup.Id = upd_Id;
            upDownDataTable.dtup_DataTableId = upDownGroup.Id;
            return Json(upd_Id);
        }
        //撤销最后一组实验数据
        [HttpPost]
        public ActionResult RevocationData(int upd_id, int id)
        {
            return Json(1);
        }
        //响应不响应
        [HttpPost]
        public ActionResult InsertData(string response, string sq, int upd_id)
        {
            return Json(true);
        }
        #endregion

        #region 单组数据

        //单组数据区间估计
        public ActionResult LntervalEstimation(int id,string res)
        {
            ViewData["res"] = res;
            return View();
        }
        //前一组实验
        [HttpPost]
        public ActionResult FromerGroupSingle(int upd_id)//2
        {
            upd_Id = upd_id - 1;
            string name = "单组数据前一组";
            List<string> str = new List<string> { "1",upd_Id+"",name };
            return Json(str);
        }
        [HttpPost]//后一组实验
        public ActionResult AfterGroupSingle(int upd_id)
        {
            //upd_Id = upd_id;
            upd_Id = upd_id + 1;
            List<string> str = new List<string>();
            str.Add(upd_Id + "");
            if (upd_Id > 2)
            {//当前组为最后一组
                str.Add("0");
                return Json(str);
            }
            else
            {
                string name = "单组数据后一组";
                str.Add(name);
                str.Add("1");
                return Json(str);//当前组不为最后一组
            }
        }
        //单组试验结果响应点计算
        [HttpPost]
        public ActionResult ResponsePoint(int upd_id,double ResponsePointValue)
        {
            return Json("55.024");
        }
        //单组试验结果响应概率计算
        [HttpPost]
        public ActionResult StimulusQuantity(int upd_id,double StimulusQuantityValue)
        {
            return Json(250.25);
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
        //综合试验结果响应点计算
        [HttpPost]
        public ActionResult ResponsePointD(int upd_id, double ResponsePointValue)
        {
            return Json("885.23");
        }
        //综合试验结果响应概率计算
        [HttpPost]
        public ActionResult StimulusQuantityD(int upd_id, double StimulusQuantityValue)
        {
            return Json(23516.154);
        }
        //设置升降法初始参数
        public ActionResult ParameterSettingData()
        {
            var sr = new StreamReader(Request.InputStream);
            var stream = sr.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            UpDownExperiment experiment = js.Deserialize<UpDownExperiment>(stream);
            experiment.udt_Creationtime = DateTime.Now.Date;
            UpDownGroup group = new UpDownGroup();
            group.Id = 1;
            group.dudt_ExperimentId = experiment.id;
            group.dudt_Stepd = experiment.udt_Stepd;

            //updownMethod updownMethod = new updownMethod();
            //UpDownData upDownData = updownMethod.get_norm_tradition(new double[] { 1200, 1250, 1200 }, new int[] { 0, 1, 1 }, 1200, 50.0);
            string[] str = { "1", group.Id + "", "测试1" };
            return Json(str/*dbDrive.Insert(experiment)*/);//需要传输本次实验的ID
        }

        //获取全部试验数据
        public ActionResult GetAllExperiments()
        {
            //    List<UpDownExperiment> experiments = dbDrive.GetAllExperiments();
            //    //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(1/*new { code = 0, msg = "", count = experiments.Count, data = experiments }, JsonRequestBehavior.AllowGet*/);
        }

        //删除试验数据
        public ActionResult Experiment_delete(int id)
        {

            //UpDownExperiment experiment = new UpDownExperiment();
            //experiment.id = id;
            string[] str = { "0"};
            return Json(/*dbDrive.Delete(experiment)*/str);
        }

        ////查询
        [HttpPost]
        public ActionResult Experiment_query(string cpmc, DateTime startdate, DateTime stopdate)
        {
            //    List<UpDownExperiment> experiments = dbDrive.QueryExperiments(cpmc,startdate,stopdate);
            return Json(1/*new { code = 0, msg = "", count = experiments.Count, data = experiments }, JsonRequestBehavior.AllowGet*/);
        }
        //查询当前ID下的所有当组实验数据
        public ActionResult GetAllUpDownMethods()
        {
            //数据的当前组ID为upd_Id
            int id = upd_Id;
            UpDownDataTable upDownData = new UpDownDataTable();
            upDownData.dtup_DataTableId = upd_Id;
            upDownData.Id = 1;
            upDownData.dtup_Initialstimulus = 1;
            upDownData.dtup_response = 1;
            upDownData.dtup_Standardstimulus = 1;
            UpDownDataTable langley2 = new UpDownDataTable();
            langley2.Id = 2;
            langley2.dtup_DataTableId = upd_Id;
            langley2.dtup_Initialstimulus = 2;
            langley2.dtup_response = 1;
            langley2.dtup_Standardstimulus = 2;
            UpDownDataTable langley3 = new UpDownDataTable();
            langley3.dtup_DataTableId = upd_Id;
            langley3.Id = 3;
            langley3.dtup_Initialstimulus = 1400;
            langley3.dtup_response = 0;
            langley3.dtup_Standardstimulus = 1500;
            UpDownDataTable langley4 = new UpDownDataTable();
            langley4.Id = 4;
            langley4.dtup_DataTableId = upd_Id;
            langley4.dtup_Initialstimulus = 250;
            langley4.dtup_response = 1;
            langley4.dtup_Standardstimulus = 1000;
            List<UpDownDataTable> langleys = new List<UpDownDataTable>();
            langleys.Add(upDownData);
            langleys.Add(langley2);
            langleys.Add(langley3);
            langleys.Add(langley4);
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code = 0, msg = "", count = langleys.Count, data = langleys }, JsonRequestBehavior.AllowGet);
        }

        //单组试验结果数据
        public ActionResult GetAllsingleunitList(int page,int limit ,int a,int b)
        {
            //当前数据的组Id为upd_Id
            int id = upd_Id;
            SingleunitListData listData1 = new SingleunitListData();
            listData1.i = 1;
            listData1.i2 = 1;
            listData1.vi = 2;
            listData1.mi = 0;
            listData1.ivi = 2;
            listData1.i2vi = 2;
            listData1.imi1 = 0;
            listData1.i2mi1 = 0;
            //SingleunitListData listData2 = new SingleunitListData();
            //listData2.i = 2;
            //listData2.i2 = 3;
            //listData2.vi = 2;
            //listData2.mi = 0;
            //listData2.ivi = 2;
            //listData2.i2vi = 2;
            //listData2.imi1 = 0;
            //listData2.i2mi1 = 0;
            //SingleunitListData listData3 = new SingleunitListData();
            //listData3.i = 1;
            //listData3.i2 = 1;
            //listData3.vi = 2;
            //listData3.mi = 0;
            //listData3.ivi = 2;
            //listData3.i2vi = 2;
            //listData3.imi1 = 0;
            //listData3.i2mi1 = 0;
            //SingleunitListData listData4 = new SingleunitListData();
            //listData4.i = 1;
            //listData4.i2 = 1;
            //listData4.vi = 2;
            //listData4.mi = 0;
            //listData4.ivi = 2;
            //listData4.i2vi = 2;
            //listData4.imi1 = 0;
            //listData4.i2mi1 = 0;
            List<SingleunitListData> listDatas = new List<SingleunitListData>();
            listDatas.Add(listData1);
            //listDatas.Add(listData2);
            //listDatas.Add(listData3);
            //listDatas.Add(listData4);
            //code--信息状态，默认为0，msg--返回信息，count--数据条数，data--数据
            return Json(new { code="",msg="",count=listDatas.Count(),data=listDatas},JsonRequestBehavior.AllowGet);
        }
        public ActionResult ComprehensiveList()
        {
            ComprehensiveListData listData = new ComprehensiveListData();
            listData.Groupid = 1;
            listData.Udt_Initialstimulus = 4500.45;
            listData.Udt_Stepd = 56.45;
            listData.Datacount = 3;
            listData.Avg = 236.012;
            listData.Standarddeviation = 34.5;
            listData.Standarddeviationerror = 67.45087567;
            listData.Standarderrormean = 4658.9809;
            listData.Analyzewithheuristics = 45546;
            listData.G = 34.545;
            listData.H = 45;
            listData.A = 89;
            listData.B = 456;
            listData.M = 253.021654622;
            listData.b = 4124;
            listData.p = 4575;
            listData.Zeroone = 12.25644;
            listData.Zeronine = 1256.15;
            List<ComprehensiveListData> listDatas = new List<ComprehensiveListData>();
            listDatas.Add(listData);
            return Json(new { code="",msg="",count=listDatas.Count(),data=listDatas},JsonRequestBehavior.AllowGet);
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