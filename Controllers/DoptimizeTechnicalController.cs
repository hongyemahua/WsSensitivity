﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WsSensitivity.Models;

namespace WsSensitivity.Controllers
{
    struct Doptimize
    {
        public int ddt_Id;
        public int ddt_Number;
        public double ddt_StimulusQuantity;
        public int ddt_Response;
        public double ddt_Mean;
        public double ddt_StandardDeviation;
        public double ddt_SigmaGuess;
        public double ddt_MeanVariance;
        public double ddt_StandardDeviationVariance;
        public double ddt_Covmusigma;
        public double number;
    }
    struct Doptimize_list
    {
        public int number;
        public int Id;
        public double PrecisionInstruments;
        public double StimulusQuantityCeiling;
        public double StimulusQuantityFloor;
        public double StandardDeviationEstimate;
        public string Power;
        public string DistributionState;
        public int FlipTheResponse;
        public string ExperimentalDate;
        public int count;
    }
    public class DoptimizeTechnicalController : Controller
    {
        // GET: DoptimizeTechnical
        //技术条件
        public ActionResult TechnicalConditions(int dop_id)
        {
            ViewData["tc"] = "技术条件"+dop_id;
            ViewData["dop_id"] = dop_id;
            return View();
        }
        //修改技术条件
        [HttpPost]
        public JsonResult TechnicalConditions_Update()
        {
            var str = new StreamReader(Request.InputStream);
            var stream = str.ReadToEnd();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return Json(true);
        }
       
        
       

    }
}