using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WsSensitivity.Controllers
{
    public class LineChartController : Controller
    {
        // GET: LangleyLineChart
        public ActionResult LineChart()
        {
            ViewData["aArray"] = LangleyPublic.aArray;
            ViewData["bArray"] = LangleyPublic.bArray;
            ViewData["cArray"] = LangleyPublic.cArray;
            ViewData["incredibleIntervalType"] = LangleyPublic.incredibleIntervalType;
            ViewData["incredibleLevelName"] = LangleyPublic.incredibleLevelName;
            return View();
        }

        //下载数据文档
        public FileResult DownloadDocument()
        {
            var sbHtml = new StringBuilder();
            sbHtml.Append("<table border='1' cellspacing='0' cellpadding='0'>");
            sbHtml.Append("<tr>");
            var lstTitle = new List<string> { "Probability", "Stimulus", "Lower", "Upper", "Confidence" };
            foreach (var item in lstTitle)
            {
                sbHtml.AppendFormat("<td style='font-size: 14px;text-align:center;background-color: #DCE0E2; font-weight:bold;' height='25'>{0}</td>", item);
            }
            sbHtml.Append("</tr>");
            for (int i = 0; i < LangleyPublic.sideReturnData.responsePoints.Length; i++)
            {
                sbHtml.Append("<tr>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>" + LangleyPublic.sideReturnData.responseProbability[i] + "</td>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>" + LangleyPublic.sideReturnData.responsePoints[i] + "</td>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>" + LangleyPublic.sideReturnData.Y_LowerLimits[i] + "</td>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>" + LangleyPublic.sideReturnData.Y_Ceilings[i] + "</td>");
                sbHtml.AppendFormat("<td style='font-size: 12px;height:20px;'>" + LangleyPublic.incredibleLevelName + "</td>");
                sbHtml.Append("</tr>");
            }
            sbHtml.Append("</table>");

            string incredibleIntervalType = LangleyPublic.incredibleIntervalType;
            //第一种:使用FileContentResult
            byte[] fileContents = Encoding.Default.GetBytes(sbHtml.ToString());
            return File(fileContents, "application/ms-excel", "" + incredibleIntervalType + ".xls");
        }
    }
}