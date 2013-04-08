using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ResearchProgram
{
    class FileWriter
    {
        public static void saveTable(String fileName, DataTable dt)
        {
            XLWorkbook wb = new XLWorkbook();
            wb.Worksheets.Add(dt);
            wb.SaveAs(fileName);
        }

        public static DataTable getTable()
        {
            DataTable table = new DataTable("Data");
            table.Columns.Add("p list", typeof(string));
            table.Columns.Add("K", typeof(int));
            table.Columns.Add("{S, k} GCD", typeof(double));
            table.Columns.Add("{#Repeat, k} GCD", typeof(string));
            table.Columns.Add("Density", typeof(double));
            table.Columns.Add("Density * k", typeof(double));

            return table;
        }
    }
}
