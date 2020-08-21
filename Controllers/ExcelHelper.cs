﻿using NPOI.HSSF.UserModel;
using NPOI.POIFS.Storage;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc.Routing.Constraints;

namespace WsSensitivity.Controllers
{
    public class Excel
    { 
        public double Probability { get; set; }
        public double Stimulus { get; set; }
        public double Lower { get; set; }
        public double Upper { get; set; }
        public double Confidence { get; set; }
    }
    public class ExcelHelper
    {
        public static List<Excel> ExcelToDataTable(string strFileName)
        {
            DataTable dt = new DataTable();
            List<Excel> excel = new List<Excel>();
            FileStream file = null;
            IWorkbook Workbook = null;
            try
            {

                using (file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))//C#文件流读取文件
                {
                    if (strFileName.IndexOf(".xlsx") > 0)
                        //把xlsx文件中的数据写入Workbook中
                        Workbook = new XSSFWorkbook(file);

                    else if (strFileName.IndexOf(".xls") > 0)
                        //把xls文件中的数据写入Workbook中
                        Workbook = new HSSFWorkbook(file);

                    if (Workbook != null)
                    {
                        ISheet sheet = Workbook.GetSheetAt(0);//读取第一个sheet
                        System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                        //得到Excel工作表的行 
                        IRow headerRow = sheet.GetRow(0);
                        //得到Excel工作表的总列数  
                        int cellCount = headerRow.LastCellNum;

                        for (int j = 0; j < cellCount; j++)
                        {
                            //得到Excel工作表指定行的单元格  
                            ICell cell = headerRow.GetCell(j);
                            dt.Columns.Add(cell.ToString());
                        }

                        for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                        {
                            IRow row = sheet.GetRow(i);
                            Excel excel1 = new Excel();
                            excel1.Probability = double.Parse(row.Cells[0].ToString());
                            excel1.Stimulus = double.Parse(row.Cells[1].ToString());
                            if(row.Cells[2].ToString() == "-◇" || row.Cells[2].ToString() == "-∞")
                                excel1.Lower = Double.NegativeInfinity;
                            else
                                excel1.Lower = double.Parse(row.Cells[2].ToString());
                            if (row.Cells[3].ToString() == "◇" || row.Cells[3].ToString() == "∞")
                                excel1.Upper = Double.PositiveInfinity;
                            else
                                excel1.Upper = double.Parse(row.Cells[3].ToString());
                            excel1.Confidence = double.Parse(row.Cells[4].ToString());
                            excel.Add(excel1);
                        }
                    }
                    return excel;
                }
            }

            catch (Exception ex)
            {
                if (file != null)
                {
                    file.Close();//关闭当前流并释放资源
                }
                return null;
            }

        }
    }
    //    /// <summary>   
    //    /// 从Excel中获取数据到DataTable   
    //    /// </summary>   
    //    /// <param name="strFileName">Excel文件全路径(服务器路径)</param>   
    //    /// <param name="SheetName">要获取数据的工作表名称</param>   
    //    /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>   
    //    /// <returns></returns>   
    //    public static DataTable RenderDataTableFromExcel(string strFileName, string SheetName, int HeaderRowIndex)
    //    {
    //        IWorkbook Workbook = null;

    //        using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
    //        {
    //            if (strFileName.IndexOf(".xlsx") > 0)

    //                Workbook = new XSSFWorkbook(file);

    //            else if (strFileName.IndexOf(".xls") > 0)

    //                Workbook = new HSSFWorkbook(file);
    //            ISheet sheet = Workbook.GetSheet(SheetName);
    //            return RenderDataTableFromExcel(Workbook, SheetName, HeaderRowIndex);
    //        }
    //    }

    //    /// <summary>   
    //    /// 从Excel中获取数据到DataTable   
    //    /// </summary>   
    //    /// <param name="workbook">要处理的工作薄</param>   
    //    /// <param name="SheetName">要获取数据的工作表名称</param>   
    //    /// <param name="HeaderRowIndex">工作表标题行所在行号(从0开始)</param>   
    //    /// <returns></returns>   
    //    public static DataTable RenderDataTableFromExcel(IWorkbook workbook, string SheetName, int HeaderRowIndex)
    //    {
    //        ISheet sheet = workbook.GetSheet(SheetName);
    //        DataTable table = new DataTable();
    //        try
    //        {
    //            IRow headerRow = sheet.GetRow(HeaderRowIndex);
    //            int cellCount = headerRow.LastCellNum;

    //            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
    //            {
    //                DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
    //                table.Columns.Add(column);
    //            }

    //            int rowCount = sheet.LastRowNum;

    //            #region 循环各行各列,写入数据到DataTable
    //            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
    //            {
    //                IRow row = sheet.GetRow(i);
    //                DataRow dataRow = table.NewRow();
    //                for (int j = row.FirstCellNum; j < cellCount; j++)
    //                {
    //                    ICell cell = row.GetCell(j);
    //                    if (cell == null)
    //                    {
    //                        dataRow[j] = null;
    //                    }
    //                    else
    //                    {
    //                        //dataRow[j] = cell.ToString();   
    //                        switch (cell.CellType)
    //                        {
    //                            case CellType.Blank:
    //                                dataRow[j] = null;
    //                                break;
    //                            case CellType.Boolean:
    //                                dataRow[j] = cell.BooleanCellValue;
    //                                break;
    //                            case CellType.Numeric:
    //                                dataRow[j] = cell.ToString();
    //                                break;
    //                            case CellType.String:
    //                                dataRow[j] = cell.StringCellValue;
    //                                break;
    //                            case CellType.Error:
    //                                dataRow[j] = cell.ErrorCellValue;
    //                                break;
    //                            case CellType.Formula:
    //                            default:
    //                                dataRow[j] = "=" + cell.CellFormula;
    //                                break;
    //                        }
    //                    }
    //                }
    //                table.Rows.Add(dataRow);
    //                //dataRow[j] = row.GetCell(j).ToString();   
    //            }
    //            #endregion
    //        }
    //        catch (System.Exception ex)
    //        {
    //            table.Clear();
    //            table.Columns.Clear();
    //            table.Columns.Add("出错了");
    //            DataRow dr = table.NewRow();
    //            dr[0] = ex.Message;
    //            table.Rows.Add(dr);
    //            return table;
    //        }
    //        finally
    //        {
    //            //sheet.Dispose();   
    //            workbook = null;
    //            sheet = null;
    //        }
    //        #region 清除最后的空行
    //        for (int i = table.Rows.Count - 1; i > 0; i--)
    //        {
    //            bool isnull = true;
    //            for (int j = 0; j < table.Columns.Count; j++)
    //            {
    //                if (table.Rows[i][j] != null)
    //                {
    //                    if (table.Rows[i][j].ToString() != "")
    //                    {
    //                        isnull = false;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (isnull)
    //            {
    //                table.Rows[i].Delete();
    //            }
    //        }
    //        #endregion
    //        return table;
    //    }

    //}
}