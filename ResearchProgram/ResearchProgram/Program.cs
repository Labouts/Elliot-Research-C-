using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace ResearchProgram
{
    class Program
    {
        private static long numDone = 0;
        private static long totalToRun = 0;
        private static Dictionary<ulong, Dictionary<ulong, double>> densityCache = new Dictionary<ulong, Dictionary<ulong, double>>();
        private static readonly object excelWriteLocder = new object();
        private static readonly object updateProgressLocker = new object();

        [STAThread]
        static void Main(string[] args)
        {
            int choice = 0;
            while(choice < 1 || choice > 3)
            {
                Console.Out.WriteLine("Which version do you want to run?\n1. Simple Single \n2. Complex Single \n3. Multiple Sets");
                choice = Console.In.ReadLine()[0] - '0';
                if(choice < 1 || choice > 3)
                {
                    Console.Out.WriteLine("Invalid Choice");
                }
            }

            Console.Out.WriteLine("How many threads do you want to run?");
            int numThreads = Console.In.ReadLine()[0] - '0';
            switch(choice)
            {
                case 1:
                    for(int count = 0; count < numThreads; count++)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.ShowDialog();
                        if(openFileDialog.SafeFileNames.Length > 0)
                            simpleReadFileAndRun(openFileDialog.FileName, "Thread" + (count + 1));
                    }
                    break;

                case 2:
                    for(int count = 0; count < numThreads; count++)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.ShowDialog();
                        if(openFileDialog.SafeFileNames.Length > 0)
                            singleReadFileAndRun(openFileDialog.FileName, "Thread" + (count + 1));
                    }
                    break;

                case 3:
                    for(int count = 0; count < numThreads; count++)
                    {
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        openFileDialog.ShowDialog();
                        //if(openFileDialog.SafeFileNames.Length > 0)
                            //multiReadFileAndRun(openFileDialog.FileName, "Thread" + (count + 1));
                    }
                    break;
            }

        }

        static void simpleReadFileAndRun(String fileName, String toAppend)
        {
            List<ulong[]> setList = new List<ulong[]>();
            List<ulong[]> scaleList = new List<ulong[]>();
            List<ulong[]> dList = new List<ulong[]>();

            Regex reg = new Regex(" +");

            bool run;
            ulong inputSize = 0;
            using(TextReader reader = File.OpenText(fileName))
            {
                string line = getNextNonEmptyLine(reader);
                run = line[0] == 'y';
                if(run)
                {
                    line = getNextNonEmptyLine(reader);
                    inputSize = ulong.Parse(line.Split()[1]);
                    line = getNextNonEmptyLine(reader);
                    while(line != null)
                    {
                        string[] numbers = reg.Split(line);
                        setList.Add(lineToArray(numbers));

                        line = getNextNonEmptyLine(reader);
                        numbers = reg.Split(line);
                        scaleList.Add(lineToArray(numbers));

                        line = getNextNonEmptyLine(reader);
                        numbers = reg.Split(line);
                        dList.Add(lineToArray(numbers));

                        line = getNextNonEmptyLine(reader);
                    }
                }
            }
            if(run)
            {
                lock(updateProgressLocker)
                {
                    totalToRun += setList.Count;
                }
                new Thread(delegate()
                {
                    simpleMultiTest(inputSize, setList, scaleList, dList, toAppend);
                }).Start();
            }
        }

        static void singleReadFileAndRun(String fileName, String toAppend)
        {
            List<ulong[]> setList = new List<ulong[]>();
            List<ulong[]> scaleList = new List<ulong[]>();
            List<ulong[]> dList = new List<ulong[]>();

            Regex reg = new Regex(" +");

            bool run;
            ulong inputSize = 0;
            using(TextReader reader = File.OpenText(fileName))
            {
                string line = getNextNonEmptyLine(reader);
                run = line[0] == 'y';
                if(run)
                {
                    line = getNextNonEmptyLine(reader);
                    inputSize = ulong.Parse(line.Split()[1]);
                    line = getNextNonEmptyLine(reader);
                    while(line != null)
                    {
                        string[] numbers = reg.Split(line);
                        setList.Add(lineToArray(numbers));

                        line = getNextNonEmptyLine(reader);
                        numbers = reg.Split(line);
                        scaleList.Add(lineToArray(numbers));

                        line = getNextNonEmptyLine(reader);
                        numbers = reg.Split(line);
                        dList.Add(lineToArray(numbers));

                        line = getNextNonEmptyLine(reader);
                    }
                }
            }
            if(run)
            {
                lock(updateProgressLocker)
                {
                    totalToRun += setList.Count;
                }
                new Thread(delegate()
                {
                    singleMultiTest(inputSize, setList, scaleList, dList, toAppend);
                }).Start();
            }
        }

        private static string getNextNonEmptyLine(TextReader reader)
        {
            string line = "";
            while(line != null && line == "")
            {
                line = reader.ReadLine();
            }

            if(line != null)
            {
                line = line.TrimEnd(' ');
                line = line.TrimStart(' ');
                line = line.Replace(",", " ");
            }

            return line;
        }

        private static ulong[] lineToArray(string[] numbers)
        {
            ulong[] numArray = new ulong[numbers.Length - 1];

            for(long index = 0; index < numArray.Length; index++)
            {
                numArray[index] = ulong.Parse(numbers[index + 1]);
            }

            return numArray;
        }

        static void simpleMultiTest(ulong inputSize, List<ulong[]> setList, List<ulong[]> scaleList, List<ulong[]> dList, string toAppendToFile)
        {
            Console.Out.WriteLine("Starting " + toAppendToFile);
            DataTable table = FileWriter.getSimpleTable();

            for(int setListIndex = 0; setListIndex < setList.Count; setListIndex++)
            {
                double[] density = NumberCruncher.densityOfUMultiD(toAppendToFile + " set #" + (setListIndex + 1), inputSize, setList[setListIndex], scaleList[setListIndex], dList[setListIndex]);

                for(long dListIndex = 0; dListIndex < density.Length; dListIndex++)
                {
                    ulong currentD = dList[setListIndex][dListIndex];
                    ulong littleGFromFormula = getLittleG(scaleList[setListIndex], currentD);
                    ulong bigGFromFormula = getBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);
                    ulong oldBigGFromFormula = getOldBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);



                    table.Rows.Add(getArrayNumberString(setList[setListIndex], scaleList[setListIndex]),
                                    currentD,
                                    oldBigGFromFormula,
                                    density[dListIndex],
                                    density[dListIndex] * currentD * oldBigGFromFormula);
                }

                updateProgress(setList[setListIndex]);
            }

            lock(excelWriteLocder)
            {
                string fileName = DateTime.Now.ToString() + toAppendToFile;
                fileName = fileName.Replace('/', '-');
                fileName = fileName.Replace(':', '_');
                fileName = fileName.Trim();
                fileName = "data/" + fileName + "_inputSize=" + inputSize.ToString() + ".xlsx";
                FileWriter.saveTable(fileName, table);
            }

            Console.Out.WriteLine(toAppendToFile + " Completed");
        }

        static void singleMultiTest(ulong inputSize, List<ulong[]> setList, List<ulong[]> scaleList, List<ulong[]> dList, string toAppendToFile)
        {
            Console.Out.WriteLine("Starting " + toAppendToFile);
            DataTable table = FileWriter.getSimpleTable();

            for(int setListIndex = 0; setListIndex < setList.Count; setListIndex++)
            {
                double[] density = NumberCruncher.densityOfUMultiD(toAppendToFile + " set #" + (setListIndex + 1), inputSize, setList[setListIndex], scaleList[setListIndex], dList[setListIndex]);

                for(long dListIndex = 0; dListIndex < density.Length; dListIndex++)
                {
                    ulong currentD = dList[setListIndex][dListIndex];
                    ulong littleGFromFormula = getLittleG(scaleList[setListIndex], currentD);
                    ulong bigGFromFormula = getBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);
                    ulong oldBigGFromFormula = getOldBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);



                    table.Rows.Add(getArrayNumberString(setList[setListIndex], scaleList[setListIndex]),
                                    currentD,
                                    oldBigGFromFormula,
                                    density[dListIndex],
                                    density[dListIndex] * currentD * oldBigGFromFormula);
                }

                updateProgress(setList[setListIndex]);
            }

            lock(excelWriteLocder)
            {
                string fileName = DateTime.Now.ToString() + toAppendToFile;
                fileName = fileName.Replace('/', '-');
                fileName = fileName.Replace(':', '_');
                fileName = fileName.Trim();
                fileName = "data/" + fileName + "_inputSize=" + inputSize.ToString() + ".xlsx";
                FileWriter.saveTable(fileName, table);
            }

            Console.Out.WriteLine(toAppendToFile + " Completed");
        }

          static void multiSetTest(ulong inputSize, ulong[][][] setList, ulong[][][] scaleList, ulong[][] dList, string toAppendToFile)
        {
            Console.Out.WriteLine("Starting " + toAppendToFile);

            DataTable table = new DataTable("Data");

            table.Columns.Add("Set #1", typeof(string));
            table.Columns.Add("d #1", typeof(double));
            table.Columns.Add("Density #1", typeof(double));
            table.Columns.Add("Set #2", typeof(string));
            table.Columns.Add("d #2", typeof(double));
            table.Columns.Add("Density #2", typeof(double));
            table.Columns.Add("Multiple Multiset Density", typeof(double));
            table.Columns.Add("Product of Density #1 and Density #2", typeof(double));
            table.Columns.Add("Error", typeof(double));

            for(int setListIndex = 0; setListIndex < setList.Length; setListIndex++)
            {
                double[] density = NumberCruncher.densityOfUMultipleSets(toAppendToFile + " set #" + (setListIndex + 1), inputSize, setList[setListIndex], scaleList[setListIndex], dList[setListIndex]);



                table.Rows.Add(getArrayNumberString(setList[setListIndex][0], scaleList[setListIndex][0]), dList[setListIndex][0], density[0],
                               getArrayNumberString(setList[setListIndex][1], scaleList[setListIndex][1]), dList[setListIndex][0], density[1],
                               density[2], density[0]*density[1], Math.Abs(density[0]*density[1] - density[2])/density[0]*density[1]);

                updateProgress(setList[setListIndex][0]);
            }

            lock(excelWriteLocder)
            {
                string fileName = DateTime.Now.ToString() + toAppendToFile + ".xlsx";
                fileName = fileName.Replace('/', '-');
                fileName = fileName.Replace(':', '_');
                fileName = fileName.Trim();
                fileName = "data/" + fileName;
                FileWriter.saveTable(fileName, table);
            }

            Console.Out.WriteLine(toAppendToFile + " Completed");
        }

        static ulong getLittleG(ulong[] scales, ulong d)
        {
            ulong[] gcdTwoArray = new ulong[scales.Length + 1];
            for(long gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdTwoArray[gcdIndex] = scales[gcdIndex];
            }
            gcdTwoArray[scales.Length] = d;

            return NumberCruncher.GCD(gcdTwoArray);
        }

        static ulong getBigG(ulong[] set, ulong[] scales, ulong d, ulong littleG)
        {
            ulong[] gcdSet = new ulong[scales.Length];
            for(long gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdSet[gcdIndex] = set[gcdIndex] * (scales[gcdIndex] % d);
            }

            return NumberCruncher.GCD(gcdSet) / littleG;
        }

        static ulong getOldBigG(ulong[] set, ulong[] scales, ulong d, ulong littleG)
        {
            ulong[] gcdSet = new ulong[scales.Length + 1];
            for(long gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdSet[gcdIndex] = set[gcdIndex] * (scales[gcdIndex] % d);
            }

            gcdSet[scales.Length] = d;

            return NumberCruncher.GCD(gcdSet) / littleG;
        }
        public static double getFormulaOneSum(ulong[] setList, ulong[] scaleList, ulong d)
        {
            ulong littleGFromFormula = getLittleG(scaleList, d);
            ulong bigGFromFomula = getOldBigG(setList, scaleList, d, littleGFromFormula);
            ulong formulaSum = 0;

            for(ulong sumIndex = 1; sumIndex < bigGFromFomula + 1; sumIndex++)
            {
                ulong[] tempArray = { sumIndex, bigGFromFomula };
                formulaSum += NumberCruncher.GCD(tempArray);
            }

            return formulaSum;
        }

        public static double getFormulaOne(ulong[] setList, ulong[] scaleList, ulong d)
        {
            ulong littleGFromFormula = getLittleG(scaleList, d);
            ulong bigGFromFomula = getOldBigG(setList, scaleList, d, littleGFromFormula);
            ulong formulaSum = 0;

            for(ulong sumIndex = 1; sumIndex < bigGFromFomula + 1; sumIndex++)
            {
                ulong[] tempArray = { sumIndex, bigGFromFomula };
                formulaSum += NumberCruncher.GCD(tempArray);
            }

            return littleGFromFormula / (double)(d * bigGFromFomula) * formulaSum;
        }
        private static double getFormulaThree(string id, ulong bigG, ulong littleG, ulong d, ulong inputSize)
        {
            ulong[] formDivisor = { bigG };
            ulong[] formScale = { 1 };
            ulong[] formFactor = { d / littleG };

            double expectedDensity;
            if(formFactor[0] == 1 || formDivisor[0] == 1)
            {
                expectedDensity = 1 / (double)d;
            }
            else
            {
                lock(densityCache)
                {
                    if(!densityCache.ContainsKey(formDivisor[0]))
                    {
                        densityCache.Add(formDivisor[0], new Dictionary<ulong, double>());
                    }
                    if(!densityCache[formDivisor[0]].ContainsKey(formFactor[0]))
                    {
                        densityCache[formDivisor[0]].Add(formFactor[0], NumberCruncher.densityOfUMultiD(id, inputSize, formDivisor, formScale, formFactor)[0]);
                    }
                    expectedDensity = densityCache[formDivisor[0]][formFactor[0]];
                }
            }

            return expectedDensity;
        }

        private static void updateProgress(ulong[] setList)
        {
            lock(updateProgressLocker)
            {
                numDone++;
            }
            Console.Out.WriteLine(numDone + " of " + totalToRun + "Completed");
            Console.Out.WriteLine(100 * numDone / totalToRun + "% done\n");
        }

        static string getArrayNumberString(ulong[] numbers, ulong[] scales)
        {
            string numString = "";
            Boolean firstNum = true;
            for(long index = 0; index < numbers.Length; index++)
            {
                if(!firstNum)
                {
                    numString += ", ";
                }
                else
                {
                    firstNum = false;
                }

                numString += numbers[index];

                for(uint timesWritten = 1; timesWritten < scales[index]; timesWritten++)
                {
                    numString += ", ";
                    numString += numbers[index];
                }
            }

            return numString;
        }

        static string getFileName(ulong[] set, ulong[] scale, string subDirectory)
        {
            string fileName = "data/" + subDirectory + "/{";
            bool isFirst = true;
            for(uint index = 0; index < set.Length; index++)
            {
                for(uint d = 0; d < scale[index]; d++)
                {
                    if(isFirst)
                    {
                        fileName += set[index];
                        isFirst = false;
                    }
                    else
                    {
                        fileName += ", " + set[index];
                    }
                }
            }
            fileName += "}/results.txt";
            return fileName;
        }
    }
}

