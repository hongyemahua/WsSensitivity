using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using AlgorithmReconstruct;
using Spire.Xls;
using WsSensitivity.Models;
using static WsSensitivity.Models.AlgorithmReconstruct;

namespace WsSensitivity.Controllers
{
    public class FreeSpire
    {
        public static string LangleyFreeSpireExcel(LangleyExperimentTable langlryExpTable, List<LangleyDataTable> ldts)
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
            sheet.Range["F3:H3"].Text = langlryExpTable.let_ExperimentalDate.ToString("yyyy-MM-dd HH:mm");
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
            var strFullName = @"C:\兰利法\" + "兰利法" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            book.SaveToFile(strFullName, ExcelVersion.Version2010);
            return strFullName;
        }

        public static string DoptimizeFreeSpireExcel(DoptimizeExperimentTable doptimizeExperimentTable, List<DoptimizeDataTable> ddts)
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
            sheet.Range["F3:H3"].Text = doptimizeExperimentTable.det_ExperimentalDate.ToString("yyyy-MM-dd HH:mm");
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
            var strFullName = @"C:\D-优化法\" + "D-优化法" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
            book.SaveToFile(strFullName, ExcelVersion.Version2010);
            return strFullName;
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

        private static string UpDownIgnition(double StandardDeviation,double Mean, LiftingAlgorithm lr, double probability)
        {
            return StandardDeviation == 0 ? "0" : "" + lr.ResponsePointCalculate(probability, Mean, StandardDeviation) + "";
        }

        public static string UpDownFreeSpireExcel(UpDownExperiment upDownExperiment, List<UpDownView> upDownViews, int grop, List<UpDownView> upDownViews1, int[] nj, double[] Gj, double[] Hj,double[] muj,double[] sigmaj,LiftingAlgorithm lr,List<UpDownGroup> upDownGroups)
        {
            var up = LiftingPublic.Upanddown(upDownViews, upDownExperiment, lr);
            var mtr = lr.MultigroupTestResult(nj, Gj, Hj, muj, sigmaj);
            Workbook book = new Workbook();
            Worksheet sheet = book.Worksheets[0];
            var iCellcount = 1;
            //1.设置表头
            if(grop == 0)
                sheet.Range[1, iCellcount++].Text = "单组升降法试验记录及数据处理结果";
            else
                sheet.Range[1, iCellcount++].Text = "多组升降法试验数据处理结果";
            sheet.Range["A1:AO1"].Merge();
            sheet.Range["A1:AO1"].Style.HorizontalAlignment = HorizontalAlignType.Center;
            sheet.Range["A2:C2"].Text = "产品名称";
            sheet.Range["A2:C2"].Merge();
            sheet.Range["D2:L2"].Text = upDownExperiment.udt_ProdectName;
            sheet.Range["D2:L2"].Merge();
            sheet.Range["M2:O2"].Text = "产品数量";
            sheet.Range["M2:O2"].Merge();
            if (grop == 0)
                sheet.Range["P2:U2"].Text = upDownViews.Count.ToString();
            else
                sheet.Range["P2:U2"].Text = upDownViews1.Count.ToString();
            sheet.Range["P2:U2"].Merge();
            sheet.Range["V2:Z2"].Text = "技术条件";
            sheet.Range["V2:Z2"].Merge();
            sheet.Range["AA2:AO2"].Text = upDownExperiment.udt_Technicalconditions;
            sheet.Range["AA2:AO2"].Merge();
            sheet.Range["A3:C3"].Text = "试验时间";
            sheet.Range["A3:C3"].Merge();
            sheet.Range["D3:L3"].Text = upDownExperiment.udt_Creationtime.ToString("yyyy-MM-dd HH:mm");
            sheet.Range["D3:L3"].Merge();
            int count = 7;
            string[] s = { "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO" };
            if (grop == 0)
            {
                sheet.Range["M3:Q3"].Text = "初始刺激量";
                sheet.Range["M3:Q3"].Merge();
                sheet.Range["R3:U3"].Text = upDownViews[0].dtup_Initialstimulus.ToString();
                sheet.Range["R3:U3"].Merge();
                sheet.Range["V3:Y3"].Text = "步长";
                sheet.Range["V3:Y3"].Merge();
                sheet.Range["Z3:AG3"].Text = upDownViews[0].dudt_Stepd.ToString();
                sheet.Range["Z3:AG3"].Merge();
                sheet.Range["AH3:AK3"].Text = "台阶数";
                sheet.Range["AH3:AK3"].Merge();
                sheet.Range["AL3:AO3"].Text = up.n.ToString();
                sheet.Range["AL3:AO3"].Merge();
                sheet.Range["A4:C4"].Text = "假设分布/分析方法";
                sheet.Range["A4:C4"].Merge();
                sheet.Range["D4:L4"].Text = LiftingPublic.DistributionState(lr);
                sheet.Range["D4:L4"].Merge();
                sheet.Range["M4:Q4"].Text = "翻转响应";
                sheet.Range["M4:Q4"].Merge();
                sheet.Range["R4:U4"].Text = upDownExperiment.udt_Flipresponse == 0 ? "否" : "是";
                sheet.Range["R4:U4"].Merge();
                sheet.Range["V4:Y4"].Text = "分辨率";
                sheet.Range["V4:Y4"].Merge();
                sheet.Range["Z4:AO4"].Text = upDownExperiment.udt_Instrumentresolution.ToString();
                sheet.Range["Z4:AO4"].Merge();
                
                sheet.Range["A5:AO5"].Merge();
                sheet.Range["A6"].Text = "i";
                sheet.Range["B6"].Text = "X";
                if (upDownExperiment.udt_Standardstate == 1)
                    sheet.Range["C6"].Text = "Ln";
                else if (upDownExperiment.udt_Standardstate == 2)
                    sheet.Range["C6"].Text = "Log";
                else if (upDownExperiment.udt_Standardstate == 3)
                    sheet.Range["C6"].Text = "幂";
                for (int i = 0; i < s.Length; i++)
                {
                    sheet.Range["" + s[i] + "6"].Text = (i + 1).ToString();
                }
                int zero = 0;
                for (int i = 0; i < up.result_i.Length; i++)
                {
                    sheet.Range["A" + count + ""].Text = up.result_i[i].ToString();
                    if (up.result_i[i] == 0)
                        zero = count;
                    count++;
                }
                for (int j = 0; j < upDownViews.Count; j++)
                {
                    sheet.Range["B" + zero + ""].Text = upDownViews[j].dtup_Initialstimulus.ToString();
                    if (upDownExperiment.udt_Standardstate != 0)
                        sheet.Range["C" + zero + ""].Text = upDownViews[j].dtup_Standardstimulus.ToString();
                    sheet.Range["" + s[j] + "" + zero + ""].Text = upDownViews[j].dtup_response.ToString();
                    if (upDownViews[j].dtup_response == 1)
                        zero++;
                    else
                        zero--;
                }
            }
            else
            {
                sheet.Range["M3:Q3"].Text = "总组数";
                sheet.Range["M3:Q3"].Merge();
                sheet.Range["R3:U3"].Text = upDownGroups.Count.ToString();
                sheet.Range["R3:U3"].Merge();
                sheet.Range["V3:Y3"].Text = "仪器分辨率";
                sheet.Range["V3:Y3"].Merge();
                sheet.Range["Z3:AG3"].Text = upDownExperiment.udt_Instrumentresolution.ToString();
                sheet.Range["Z3:AG3"].Merge();
                sheet.Range["A4:C4"].Text = "假设分布/分析方法";
                sheet.Range["A4:C4"].Merge();
                sheet.Range["D4:L4"].Text = LiftingPublic.DistributionState(lr);
                sheet.Range["D4:L4"].Merge();
                sheet.Range["M4:Q4"].Text = "翻转响应";
                sheet.Range["M4:Q4"].Merge();
                sheet.Range["R4:U4"].Text = upDownExperiment.udt_Flipresponse == 0 ? "否" : "是";
                sheet.Range["R4:U4"].Merge();
            }
            if (grop == 0)
            {
                count = UpDown(up.μ0_final, up.σ0_final, count, sheet, up.Sigma_mu, up.Sigma_sigma, lr, up.n);
                count++;
                sheet.Range["A"+count+":AO"+count+""].Text = "升降法中间数据：试探次数："+up.n+",A:"+up.A+",B:"+up.B+",M:"+up.M+",b:"+up.b+",G:"+up.G+"";
                sheet.Range["A" + count + ":AO" + count + ""].Merge();
            }
            else
            {
                int n = 0;
                foreach(var i in nj)
                {
                    n += i;
                }
                count = UpDown(mtr.μ0_final, mtr.σ0_final, count, sheet, mtr.Sigma_mu, mtr.Sigma_sigma, lr, n);
            }
            for (int w = 0;w<s.Length;w++)
            {
                sheet.Range["" + s[w] + "1:" + s[w] + "" + count + ""].ColumnWidth = 4;
            }
            sheet.Range["A2:AO" + count + ""].BorderInside(LineStyleType.Thin, Color.Black);
            sheet.Range["A2:AO" + count + ""].BorderAround(LineStyleType.Medium, Color.Black);
            if (grop == 0)
            {
                var strFullName = @"C:\升降法\" + "单组升降法" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                book.SaveToFile(strFullName, ExcelVersion.Version2010);
                return strFullName;

            }
            else
            {
                var strFullName = @"C:\升降法\" + "多组升降法" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".xlsx";
                book.SaveToFile(strFullName, ExcelVersion.Version2010);
                return strFullName;
            }
        }

        private static int UpDown(double μ0_final,double σ0_final,int count, Worksheet sheet,double Sigma_mu,double Sigma_sigma,LiftingAlgorithm lr,int n)
        {
            sheet.Range["A" + count + ":C" + count + ""].Text = "点估计：";
            sheet.Range["A" + count + ":C" + count + ""].Merge();
            sheet.Range["D" + count + ":AO" + count + ""].Text = "均值：" + μ0_final + "      标准差：" + σ0_final + "       均值标准误差：" + Sigma_mu + "        标准差标准误差：" + Sigma_sigma + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            sheet.Range["D" + count + ":AO" + count + ""].Text = "99%发火点：" + UpDownIgnition(σ0_final, μ0_final, lr, 0.99) + "      99.9%发火点：" + UpDownIgnition(σ0_final, μ0_final, lr, 0.999) + "       99.9999%发火点：" + UpDownIgnition(σ0_final, μ0_final, lr, 0.999999) + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            sheet.Range["D" + count + ":AO" + count + ""].Text = "1%发火点：" + UpDownIgnition(σ0_final, μ0_final, lr, 0.01) + "      0.1%发火点：" + UpDownIgnition(σ0_final, μ0_final, lr, 0.001) + "       0.0001%发火点：" + UpDownIgnition(σ0_final, μ0_final, lr, 0.000001) + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            sheet.Range["A" + count + ":C" + count + ""].Text = "区间估计：";
            sheet.Range["A" + count + ":C" + count + ""].Merge();
            sheet.Range["D" + count + ":AO" + count + ""].Text = "置信度：0.95";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            var vfr = lr.VarianceFunctionResponseProbabilityIntervalEstimated(0.999, 0.95, n, μ0_final, σ0_final, Sigma_mu, Sigma_sigma);
            sheet.Range["D" + count + ":AO" + count + ""].Text = "均值区间估计：（" + vfr[5] + "," + vfr[4] + "）  标准差区间估计：（" + vfr[7] + "," + vfr[6] + "）";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            sheet.Range["D" + count + ":AO" + count + ""].Text = "响应概率为99.9%区间估计：（" + vfr[1] + "," + vfr[0] + "）  单侧置信下限：" + vfr[3] + "        单侧置信上限：" + vfr[2] + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            vfr = lr.VarianceFunctionResponseProbabilityIntervalEstimated(0.001, 0.95, n, μ0_final, σ0_final, Sigma_mu, Sigma_sigma);
            sheet.Range["D" + count + ":AO" + count + ""].Text = "响应概率为0.1%区间估计：（" + vfr[1] + "," + vfr[0] + "）  单侧置信下限：" + vfr[3] + "        单侧置信上限：" + vfr[2] + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            vfr = lr.VarianceFunctionResponseProbabilityIntervalEstimated(0.999999, 0.95, n, μ0_final, σ0_final, Sigma_mu, Sigma_sigma);
            sheet.Range["D" + count + ":AO" + count + ""].Text = "响应概率为99.9999%区间估计：（" + vfr[1] + "," + vfr[0] + "）  单侧置信下限：" + vfr[3] + "        单侧置信上限：" + vfr[2] + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            count++;
            vfr = lr.VarianceFunctionResponseProbabilityIntervalEstimated(0.000001, 0.95, n, μ0_final, σ0_final, Sigma_mu, Sigma_sigma);
            sheet.Range["D" + count + ":AO" + count + ""].Text = "响应概率为0.0001%区间估计：（" + vfr[1] + "," + vfr[0] + "）  单侧置信下限：" + vfr[3] + "        单侧置信上限：" + vfr[2] + "";
            sheet.Range["D" + count + ":AO" + count + ""].Merge();
            return count;
        }
    }
}