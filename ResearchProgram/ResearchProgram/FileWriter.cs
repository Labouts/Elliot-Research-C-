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
            table.Columns.Add("Set", typeof(string));
            table.Columns.Add("d", typeof(int));
            table.Columns.Add("Little g", typeof(double));
            table.Columns.Add("Big G", typeof(double));
            table.Columns.Add("Density", typeof(double));
            table.Columns.Add("Formula Result", typeof(double));
            table.Columns.Add("Error", typeof(double));
            table.Columns.Add("Density * d * gcd(d, 2*S(2), 3*S(3) ... n*S(n))", typeof(double));

            return table;
        }
    }
}
