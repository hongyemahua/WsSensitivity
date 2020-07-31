using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;

namespace WsSensitivity.Controllers
{
    public class DoptimizeQueryController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: DoptimizeQuery
        //D优化法查询界面
        public ActionResult DoptimizeQuery()
        {
            var lets = dbDrive.GetAllLangleyExperimentTables();
            List<string> productName = new List<string>();
            foreach (var let in lets)
            {
                if (!productName.Contains(let.let_ProductName))
                    productName.Add(let.let_ProductName);
            }
            DoptimizeQueryModel doptimizeQueryModel = new DoptimizeQueryModel();
            doptimizeQueryModel.productName = productName;
            return View(doptimizeQueryModel);
        }
        //获取全部D优化法实验数据
        public ActionResult GetAllDoptimizeExperiment(int page = 1, int limit = 20)
        {
            List<DoptimizeExperimentTable> det_list = dbDrive.GetAllDoptimizeExperimentTables();
            List<DoptimizeExperimentTable> PagesLet = new List<DoptimizeExperimentTable>();
            int last = det_list.Count - (page - 1) * limit;
            int first = 0;
            if (page * limit < det_list.Count)
            {
                first = det_list.Count - page * limit;
            }
            for (int i = first; i < last; i++)
            {
                PagesLet.Add(det_list[i]);
            }
            return Json(new { code = 0, msg = "", fenye = 5, count = det_list.Count, data = DoptimizePublic.Doptimization_list(dbDrive,PagesLet,first) }, JsonRequestBehavior.AllowGet);
        }
        //D优化法返回查询结果
        [HttpPost]
        public ActionResult doptimize_query(string productName, string startTime, string endTime)
        {
            List<DoptimizeExperimentTable> det_list = new List<DoptimizeExperimentTable>();
            if (startTime != "" && endTime != "")
            {
                DateTime st = Convert.ToDateTime(startTime);
                DateTime et = Convert.ToDateTime(endTime);
                det_list = dbDrive.QueryDoptimizeExperimentTable(productName, st, et.AddDays(1));
            }
            else
                det_list = dbDrive.QueryDoptimizeExperimentTable(productName);
            return Json(new { code = 0, msg = "", count = det_list.Count, data = DoptimizePublic.Doptimization_list(dbDrive,det_list) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Doptimize_delete(int dop_id)
        {
            var det = dbDrive.GetDoptimizeExperimentTable(dop_id);
            return Json(dbDrive.Delete(det));
        }
    }
}