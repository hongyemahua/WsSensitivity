using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;

namespace WsSensitivity.Controllers
{
    public class LangleyQueryController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: LangleyQuery
        public ActionResult LangleyQuery()
        {
            var lets = dbDrive.GetAllLangleyExperimentTables(LangleyPublic.adminId);
            List<string> productName = new List<string>();
            foreach (var let in lets)
            {
                if (!productName.Contains(let.let_ProductName))
                    productName.Add(let.let_ProductName);
            }
            ViewData["pn"] = productName;
            return View();
        }
        //名称、时间查询
        public ActionResult Langley_query(string productName, string startTime, string endTime)
        {
            List<LangleyExperimentTable> lets = new List<LangleyExperimentTable>();
            if (startTime != "" && endTime != "")
            {
                DateTime st = Convert.ToDateTime(startTime);
                DateTime et = Convert.ToDateTime(endTime);
                lets = dbDrive.QueryLangleyExperimentTable(productName, st, et.AddDays(1),LangleyPublic.adminId);
            }
            else
                lets = dbDrive.QueryLangleyExperimentTable(productName, LangleyPublic.adminId);
            return Json(new { code = 0, msg = "", count = lets.Count, data = LangleyPublic.Langley_lists(dbDrive, lets) }, JsonRequestBehavior.AllowGet);
        }
        //获取全部的兰利法实验并分页显示(前台带参访问)
        public ActionResult GetAllLangleysExperiment(int page = 1, int limit = 20)
        {
            List<LangleyExperimentTable> lets = dbDrive.GetAllLangleyExperimentTables(LangleyPublic.adminId);
            List<LangleyExperimentTable> PagesLet = new List<LangleyExperimentTable>();
            int last = lets.Count - (page - 1) * limit;
            int first = 0;
            if (page * limit < lets.Count)
            {
                first = lets.Count - page * limit;
            }
            for (int i = first; i < last; i++)
            {
                PagesLet.Add(lets[i]);
            }
            return Json(new { code = 0, msg = "", fenye = 5, count = lets.Count, data = LangleyPublic.Langley_lists(dbDrive, PagesLet, first) }, JsonRequestBehavior.AllowGet);
        }

        //删除兰利法&&全部清除
        [HttpPost]
        public ActionResult Langley_delete(int id)
        {
            var let = dbDrive.GetLangleyExperimentTable(id);
            return Json(dbDrive.Delete(let));
        }
    }
}