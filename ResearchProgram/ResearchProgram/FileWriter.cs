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
            table.Columns.Add("d", typeof(long));
            table.Columns.Add("H = GCD( d, 2*S'(2), 3*S'(3), ... n*S'(n))", typeof(double));
            table.Columns.Add("Density", typeof(double));
            table.Columns.Add("density*d*H", typeof(double));
            

            return table;
        }
    }
}
