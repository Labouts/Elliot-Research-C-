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

        public static DataTable getSimpleTable()
        {
            DataTable table = new DataTable("Data");
            table.Columns.Add("Set", typeof(string));
            table.Columns.Add("d", typeof(long));
            table.Columns.Add("H = GCD( d, 2*S'(2), 3*S'(3), ... n*S'(n))", typeof(double));
            table.Columns.Add("Density", typeof(double));
            table.Columns.Add("density*d*H", typeof(double));


            return table;
        }

        public static DataTable getSingleTable()
        {
            DataTable table = new DataTable("Data");
            table.Columns.Add("Set", typeof(string));
            table.Columns.Add("d", typeof(int));
            table.Columns.Add("g = GCD( d, S(2), S(3), ... S(n))", typeof(double));
            table.Columns.Add("H = GCD( d, 2*S'(2), 3*S'(3), ... n*S'(n))", typeof(double));
            table.Columns.Add("G = GCD( 2*S'(2), 3*S'(3), ... n*S'(n))", typeof(double));
            table.Columns.Add("Sum[i=1 to i=H] GCD(i, H)", typeof(double));
            table.Columns.Add("Density", typeof(double));
            table.Columns.Add("Formula 1", typeof(double));
            table.Columns.Add("Formula 2", typeof(double));
            table.Columns.Add("Formula 3", typeof(double));
            table.Columns.Add("Formula 1 Error", typeof(double));
            table.Columns.Add("Formula 2 Error", typeof(double));
            table.Columns.Add("Formula 3 Error", typeof(double));
            table.Columns.Add("Best Match", typeof(string));

            return table;
        }

        public static DataTable getMultiTable()
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
