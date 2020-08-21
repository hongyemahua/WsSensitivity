using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using Spire.Xls;
using WsSensitivity.Models;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    public class FreeSpire
    {
        public static void LangleyFreeSpireExcel(LangleyExperimentTable langlryExpTable, List<LangleyDataTable> ldts)
        {
            var lr = LangleyPublic.SelectState(langlryExpTable);
            ldts.RemoveRange(ldts.Count - 1, 1);
            var xOrVArray = LangleyPublic.XAndVArrays(ldts);
            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            var iCellcount = 1;
            //1.设置表头
            sheet.Range[1, iCellcount++].Text = "兰利法感度试验数据记录及处理结果";
            sheet.Range["A1:H1"].Merge();
            sheet.Range["A1:H1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
            sheet.Range["E2"].Text = "打印时间";
            sheet.Range["F2:H2"].Text = DateTime.Now.ToString("yyyy-MM-dd");
            sheet.Range["F2:H2"].Merge();
            sheet.Range["A3"].Text = "样本名称";
            sheet.Range["B3:D3"].Text = langlryExpTable.let_ProductName;
            sheet.Range["B3:D3"].Merge();
            sheet.Range["E3"].Text = "试验时间";
            sheet.Range["F3:H3"].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            sheet.Range["F3:H3"].Merge();
            sheet.Range["A4"].Text = "实验数量";
            sheet.Range["B4"].Text = ldts.Count.ToString();
            sheet.Range["C4"].Text = "分辨率";
            sheet.Range["D4"].Text = langlryExpTable.let_PrecisionInstruments.ToString();
            sheet.Range["E4"].Text = "发布选择";
            sheet.Range["F4:H4"].Text = LangleyPublic.DistributionState(langlryExpTable);
            sheet.Range["F4:H4"].Merge();
            sheet.Range["A5"].Text = "刺激量上限";
            sheet.Range["B5"].Text = langlryExpTable.let_StimulusQuantityCeiling.ToString();
            sheet.Range["C5"].Text = "刺激量下限";
            sheet.Range["D5"].Text = langlryExpTable.let_StimulusQuantityFloor.ToString();
            sheet.Range["E5"].Text = "标准差修正";
            sheet.Range["F5"].Text = langlryExpTable.let_Correction == 0 ? "是" : "否";
            sheet.Range["G5"].Text = "翻转响应";
            sheet.Range["H5"].Text = langlryExpTable.let_FlipTheResponse == 1 ? "是" : "否";
            sheet.Range["A6"].Text = "技术条件";
            sheet.Range["B6:H6"].Text = langlryExpTable.let_TechnicalConditions;
            sheet.Range["B6:H6"].Merge();
            if (langlryExpTable.let_FlipTheResponse == 0)
                if (langlryExpTable.let_Correction == 1)
                    sheet.Range["A7:H7"].Text = "标记：发火：“1”，不发火：“0” 点估计标准差计算结果为最大拟然估计结果";
                else
                    sheet.Range["A7:H7"].Text = "标记：发火：“1”，不发火：“0” 点估计标准差计算结果为按照GJB377修正结果";
            else
                if (langlryExpTable.let_Correction == 1)
                sheet.Range["A7:H7"].Text = "标记：发火：“0”，不发火：“1” 点估计标准差计算结果为最大拟然估计结果";
            else
                sheet.Range["A7:H7"].Text = "标记：发火：“0”，不发火：“1” 点估计标准差计算结果为按照GJB377修正结果";
            TableHead(sheet);
            int count = 9;
            for (int i = 0; i < ldts.Count; i++)
            {
                sheet.Range["A" + count + ""].Text = (i + 1).ToString();
                sheet.Range["B" + count + ":C" + count + ""].Text = ldts[i].ldt_StimulusQuantity.ToString();
                sheet.Range["B" + count + ":C" + count + ""].Merge();
                sheet.Range["D" + count + ""].Text = ldts[i].ldt_Response.ToString();
                sheet.Range["E" + count + ":F" + count + ""].Text = ldts[i].ldt_Mean.ToString();
                sheet.Range["E" + count + ":F" + count + ""].Merge();
                sheet.Range["G" + count + ":H" + count + ""].Text = ldts[i].ldt_StandardDeviation.ToString();
                sheet.Range["G" + count + ":H" + count + ""].Merge();
                count++;
            }
            sheet.Range["A" + count + ""].Text = "点估计：";
            sheet.Range["B" + count + ":D" + count + ""].Text = "均值：" + ldts[ldts.Count - 1].ldt_Mean + "";
            sheet.Range["B" + count + ":D" + count + ""].Merge();
            sheet.Range["E" + count + ":H" + count + ""].Text = "标准差：" + ldts[ldts.Count - 1].ldt_StandardDeviation + "";
            sheet.Range["E" + count + ":H" + count + ""].Merge();
            count++;
            var ignition99 = LangleyIgnition(ldts,lr,0.99);
            var ignition1 = LangleyIgnition(ldts, lr, 0.01);
            var ignition999 = LangleyIgnition(ldts, lr, 0.999);
            var ignition01 = LangleyIgnition(ldts, lr, 0.001);
            var ignition9999 = LangleyIgnition(ldts, lr, 0.9999);
            var ignition001 = LangleyIgnition(ldts, lr, 0.0001);
            count = PointCalculation(count, ignition99, ignition1, ignition999, ignition01, ignition9999, ignition001, sheet);
            var ie = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.999, 0.8));
            var ie01 = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.001, 0.8));
            count = IntervalEstimation(count, sheet, ie, ie01, 0.8);

            ie = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.999, 0.95));
            ie01 = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.001, 0.95));
            count = IntervalEstimation(count, sheet, ie, ie01, 0.95);

            ie = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.999, 0.99));
            ie01 = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.001, 0.99));
            count = IntervalEstimation(count, sheet, ie, ie01, 0.99);

            sheet.Range["A3:H" + count + ""].BorderInside(LineStyleType.Thin, Color.Black);
            sheet.Range["A3:H" + count + ""].BorderAround(LineStyleType.Medium, Color.Black);
            count++;
            sheet.Range["B" + count + ""].Text = "复查人";
            sheet.Range["F" + count + ""].Text = "试验人";
            //设置行宽
            sheet.Range["A1:H1"].RowHeight = 20;
            var strFullName = @"D:\Data\Upload\" + "兰利法" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            book.SaveToFile(strFullName, ExcelVersion.Version2010);
        }

        public static void DoptimizeFreeSpireExcel(DoptimizeExperimentTable doptimizeExperimentTable, List<DoptimizeDataTable> ddts)
        {
            var lr = DoptimizePublic.SelectState(doptimizeExperimentTable);
            ddts.RemoveRange(ddts.Count - 1, 1);
            var der_list = DoptimizePublic.DoptimizeExperimentRecoedsList(ddts, doptimizeExperimentTable);
            var xOrVArray = DoptimizePublic.ReturnXarrayAndVarray(der_list);
            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            var iCellcount = 1;
            //1.设置表头
            sheet.Range[1, iCellcount++].Text = "D-优化法感度试验数据记录及处理结果";
            sheet.Range["A1:H1"].Merge();
            sheet.Range["A1:H1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
            sheet.Range["E2"].Text = "打印时间";
            sheet.Range["F2:H2"].Text = DateTime.Now.ToString("yyyy-MM-dd");
            sheet.Range["F2:H2"].Merge();
            sheet.Range["A3"].Text = "样本名称";
            sheet.Range["B3:D3"].Text = doptimizeExperimentTable.det_ProductName;
            sheet.Range["B3:D3"].Merge();
            sheet.Range["E3"].Text = "试验时间";
            sheet.Range["F3:H3"].Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            sheet.Range["F3:H3"].Merge();
            sheet.Range["A4"].Text = "实验数量";
            sheet.Range["B4"].Text = ddts.Count.ToString();
            sheet.Range["C4"].Text = "分辨率";
            sheet.Range["D4"].Text = doptimizeExperimentTable.det_PrecisionInstruments.ToString();
            sheet.Range["E4"].Text = "发布选择";
            sheet.Range["F4:H4"].Text = DoptimizePublic.DistributionState(doptimizeExperimentTable);
            sheet.Range["F4:H4"].Merge();
            sheet.Range["A5"].Text = "刺激量上限";
            sheet.Range["B5"].Text = doptimizeExperimentTable.det_StimulusQuantityCeiling.ToString();
            sheet.Range["C5"].Text = "刺激量下限";
            sheet.Range["D5"].Text = doptimizeExperimentTable.det_StimulusQuantityFloor.ToString();
            sheet.Range["E5"].Text = "预估值";
            sheet.Range["F5"].Text = doptimizeExperimentTable.det_StandardDeviationEstimate.ToString();
            sheet.Range["G5"].Text = "翻转响应";
            sheet.Range["H5"].Text = doptimizeExperimentTable.det_FlipTheResponse == 1 ? "是" : "否";
            sheet.Range["A6"].Text = "技术条件";
            sheet.Range["B6:H6"].Text = doptimizeExperimentTable.det_TechnicalConditions;
            sheet.Range["B6:H6"].Merge();
            if (doptimizeExperimentTable.det_FlipTheResponse == 0)
                sheet.Range["A7:H7"].Text = "标记：发火：“1”，不发火：“0”;";
            else
                sheet.Range["A7:H7"].Text = "标记：发火：“0”，不发火：“1”;";
            TableHead(sheet);
            int count = 9;
            for (int i = 0; i < ddts.Count; i++)
            {
                sheet.Range["A" + count + ""].Text = (i + 1).ToString();
                sheet.Range["B" + count + ":C" + count + ""].Text = ddts[i].ddt_StimulusQuantity.ToString();
                sheet.Range["B" + count + ":C" + count + ""].Merge();
                sheet.Range["D" + count + ""].Text = ddts[i].ddt_Response.ToString();
                sheet.Range["E" + count + ":F" + count + ""].Text = ddts[i].ddt_Mean.ToString();
                sheet.Range["E" + count + ":F" + count + ""].Merge();
                sheet.Range["G" + count + ":H" + count + ""].Text = ddts[i].ddt_StandardDeviation.ToString();
                sheet.Range["G" + count + ":H" + count + ""].Merge();
                count++;
            }
            sheet.Range["A" + count + ""].Text = "点估计：";
            sheet.Range["B" + count + ":D" + count + ""].Text = "均值：" + ddts[ddts.Count - 1].ddt_Mean + "";
            sheet.Range["B" + count + ":D" + count + ""].Merge();
            sheet.Range["E" + count + ":H" + count + ""].Text = "标准差：" + ddts[ddts.Count - 1].ddt_StandardDeviation + "";
            sheet.Range["E" + count + ":H" + count + ""].Merge();
            count++;
            var ignition99 = DoptimizeIgnition(ddts,lr,0.99);
            var ignition1 = DoptimizeIgnition(ddts, lr, 0.01);
            var ignition999 = DoptimizeIgnition(ddts, lr, 0.999);
            var ignition01 = DoptimizeIgnition(ddts, lr, 0.001);
            var ignition9999 = DoptimizeIgnition(ddts, lr, 0.9999);
            var ignition001 = DoptimizeIgnition(ddts, lr, 0.0001);
            count = PointCalculation(count, ignition99, ignition1, ignition999, ignition01, ignition9999, ignition001,sheet);
            var ie = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.999, 0.8));
            var ie01 = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.001, 0.8));
            sheet.Range["A" + count + ""].Text = "区间估计:";
            count = IntervalEstimation(count,sheet,ie,ie01,0.8);

            ie = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.999, 0.95));
            ie01 = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.001, 0.95));
            count = IntervalEstimation(count, sheet, ie, ie01, 0.95);

            ie = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.999, 0.99));
            ie01 = lr.GetIntervalEstimationValue(lr.DoubleSideEstimation(xOrVArray.xArray, xOrVArray.vArray, 0.001, 0.99));
            count = IntervalEstimation(count, sheet, ie, ie01, 0.99);

            sheet.Range["A3:H" + count + ""].BorderInside(LineStyleType.Thin, Color.Black);
            sheet.Range["A3:H" + count + ""].BorderAround(LineStyleType.Medium, Color.Black);
            count++;
            sheet.Range["B" + count + ""].Text = "复查人";
            sheet.Range["F" + count + ""].Text = "试验人";
            //设置行宽
            sheet.Range["A1:H1"].RowHeight = 20;
            var strFullName = @"D:\Data\Upload\" + "D-优化法" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            book.SaveToFile(strFullName, ExcelVersion.Version2010);
        }

        private static void TableHead(Worksheet sheet)
        {
            sheet.Range["A7:H7"].Merge();
            sheet.Range["A8"].Text = "编号";
            sheet.Range["B8:C8"].Text = "刺激量";
            sheet.Range["B8:C8"].Merge();
            sheet.Range["D8"].Text = "响应";
            sheet.Range["E8:F8"].Text = "均值中间值";
            sheet.Range["E8:F8"].Merge();
            sheet.Range["G8:H8"].Text = "标准差中间值";
            sheet.Range["G8:H8"].Merge();
        }

        private static int IntervalEstimation(int count, Worksheet sheet,IntervalEstimation ie,IntervalEstimation ie01,double confidenceLevel)
        {
            sheet.Range["B" + count + ":H" + count + ""].Text = "置信水平"+confidenceLevel+"";
            sheet.Range["B" + count + ":H" + count + ""].Merge();
            count++;
            sheet.Range["B" + count + ":H" + count + ""].Text = "均值区间估计：(" + ie.Mu.Down + "," + ie.Mu.Up + ")";
            sheet.Range["B" + count + ":H" + count + ""].Merge();
            count++;
            sheet.Range["B" + count + ":H" + count + ""].Text = "标准差区间估计：(" + ie.Sigma.Down + "," + ie.Sigma.Up + ")";
            sheet.Range["B" + count + ":H" + count + ""].Merge();
            count++;
            sheet.Range["B" + count + ":H" + count + ""].Text = "响应概率为99.9%区间估计：(" + ie.Confidence.Down + "," + ie.Confidence.Up + ")";
            sheet.Range["B" + count + ":H" + count + ""].Merge();
            count++;
            sheet.Range["B" + count + ":H" + count + ""].Text = "响应概率为0.1%区间估计：(" + ie01.Confidence.Down + "," + ie01.Confidence.Up + ")";
            sheet.Range["B" + count + ":H" + count + ""].Merge();
            count++;
            return count;
        }

        private static int PointCalculation(int count, string ignition99,string ignition1,string ignition999,string ignition01,string ignition9999,string ignition001, Worksheet sheet)
        {
            sheet.Range["B" + count + ":D" + count + ""].Text = "99%发火点：" + ignition99 + "";
            sheet.Range["B" + count + ":D" + count + ""].Merge();
            sheet.Range["E" + count + ":H" + count + ""].Text = "1%发火点：" + ignition1 + "";
            sheet.Range["E" + count + ":H" + count + ""].Merge();
            count++;
            sheet.Range["B" + count + ":D" + count + ""].Text = "99.9%发火点：" + ignition999 + "";
            sheet.Range["B" + count + ":D" + count + ""].Merge();
            sheet.Range["E" + count + ":H" + count + ""].Text = "0.1%发火点：" + ignition01 + "";
            sheet.Range["E" + count + ":H" + count + ""].Merge();
            count++;
            sheet.Range["B" + count + ":D" + count + ""].Text = "99.99%发火点：" + ignition9999 + "";
            sheet.Range["B" + count + ":D" + count + ""].Merge();
            sheet.Range["E" + count + ":H" + count + ""].Text = "0.01%发火点：" + ignition001 + "";
            sheet.Range["E" + count + ":H" + count + ""].Merge();
            count++;
            return count;
        }

        private static string LangleyIgnition(List<LangleyDataTable> ldts, LangleyAlgorithm lr,double probability)
        {
            return ldts[ldts.Count - 1].ldt_StandardDeviation == 0 ? "0" : "" + lr.ResponsePointCalculate(probability, ldts[ldts.Count - 1].ldt_Mean, ldts[ldts.Count - 1].ldt_StandardDeviation) + "";
        }

        private static string DoptimizeIgnition(List<DoptimizeDataTable> ddts, LangleyAlgorithm lr, double probability)
        {
            return ddts[ddts.Count - 1].ddt_StandardDeviation == 0 ? "0" : "" + lr.ResponsePointCalculate(probability, ddts[ddts.Count - 1].ddt_Mean, ddts[ddts.Count - 1].ddt_StandardDeviation) + "";
        }
    }
}