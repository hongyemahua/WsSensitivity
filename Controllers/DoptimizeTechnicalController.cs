using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models;
using WsSensitivity.Models.IDbDrives;

namespace WsSensitivity.Controllers
{


    public class DoptimizeTechnicalController : Controller
    {
        IDbDrive dbDrive = new LingImp();
        // GET: DoptimizeTechnical
        //修改技术条件
        [HttpPost]
        public JsonResult TechnicalConditions_Update()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var det = dbDrive.GetDoptimizeExperimentTable(js.Deserialize<DoptimizeExperimentTable>(stream).det_Id);
            det.det_TechnicalConditions = js.Deserialize<DoptimizeExperimentTable>(stream).det_TechnicalConditions;
            return Json(dbDrive.Update(det));
        }
       
        
       

    }
}