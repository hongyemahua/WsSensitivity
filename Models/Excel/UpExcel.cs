using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WsSensitivity.Models.Excel
{
    public class UpExcel
    {
        public static int upExcel(string SheetName,HttpPostedFileBase excel)
        {
            //string filename = Request.MapPath("~/Excel/") + Path.GetFileName(excel.FileName);
            //excel.SaveAs(filename); //这两句是把要上传的excel先保存到本地路径 然后获取
            return 1;
        }
    }
}