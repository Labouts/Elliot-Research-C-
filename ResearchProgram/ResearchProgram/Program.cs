using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace ResearchProgram
{
    class Program
    {
        private static int numDone = 0;
        private static int totalToRun = 0;
        private static Dictionary<ulong, Dictionary<ulong, double>> densityCache = new Dictionary<ulong, Dictionary<ulong, double>>();
        private static readonly object excelWriteLocder = new object();
        private static readonly object updateProgressLocker = new object();

        static void Main(string[] args)
        {
            readFileAndRun("ThreadOneInput.txt", "ThreadOne");
            readFileAndRun("ThreadTwoInput.txt", "ThreadTwo");
            readFileAndRun("ThreadThreeInput.txt", "ThreadThree");
            readFileAndRun("ThreadFourInput.txt", "ThreadFour");
        }

        static void readFileAndRun(String fileName, String toAppend)
        {
            List<uint[]> setList = new List<uint[]>();
            List<uint[]> scaleList = new List<uint[]>();
            List<uint[]> dList = new List<uint[]>();

            Regex reg = new Regex(" +");

            ulong inputSize = 0;
            using(TextReader reader = File.OpenText(fileName))
            {
                string line = getNextNonEmptyLine(reader);
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
            lock(updateProgressLocker)
            {
                totalToRun += setList.Count;
            }
            new Thread(delegate()
            {
                multiTest(inputSize, setList, scaleList, dList, toAppend);
            }).Start();
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

        private static uint[] lineToArray(string[] numbers)
        {
            uint[] numArray = new uint[numbers.Length - 1];

            for(int index = 0; index < numArray.Length; index++)
            {
                numArray[index] = uint.Parse(numbers[index + 1]);
            }

            return numArray;
        }

        static void multiTest(ulong inputSize, List<uint[]> setList, List<uint[]> scaleList, List<uint[]> dList, string toAppendToFile)
        {
            Console.Out.WriteLine("Starting " + toAppendToFile);
            DataTable table = FileWriter.getTable();

            for(int setListIndex = 0; setListIndex < setList.Count; setListIndex++)
            {
                double[] density = NumberCruncher.densityOfUMultiD(toAppendToFile + " set #" + (setListIndex+1), inputSize, setList[setListIndex], scaleList[setListIndex], dList[setListIndex]);

                for(int dListIndex = 0; dListIndex < density.Length; dListIndex++)
                {
                    uint currentD = dList[setListIndex][dListIndex];
                    uint littleGFromFormula = getLittleG(scaleList[setListIndex], currentD);
                    uint bigGFromFormula = getBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);
                    uint oldBigGFromFormula = getOldBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);
                    double formulaOneSum = getFormulaOneSum(scaleList[setListIndex], setList[setListIndex], dList[setListIndex][dListIndex]);
                    double formulaOne = getFormulaOne(scaleList[setListIndex], setList[setListIndex], dList[setListIndex][dListIndex]);
                    double formulaTwo = getFormulaThree(toAppendToFile + " Formula Two Calculation set #" + (setListIndex+1) + " d#" + (dListIndex+1), oldBigGFromFormula, littleGFromFormula, currentD, inputSize);
                    double formulaThree = getFormulaThree(toAppendToFile + " Formula Three Calculation set #" + (setListIndex+1) + " d#" + (dListIndex+1), bigGFromFormula, littleGFromFormula, currentD, inputSize);

                    double errorOne = Math.Abs(formulaOne - density[dListIndex]) / density[dListIndex];
                    double errorTwo = Math.Abs(formulaTwo - density[dListIndex]) / density[dListIndex];
                    double errorThree = Math.Abs(formulaThree - density[dListIndex]) / density[dListIndex];

                    string winner;

                    if(errorOne < errorTwo && errorOne < errorThree)
                    {
                        winner = "Formula One";
                    }
                    else if(errorTwo < errorThree)
                    {
                        winner = "Formula Two";
                    }
                    else
                    {
                        winner = "Formula Three";
                    }

                    table.Rows.Add(getArrayNumberString(setList[setListIndex], scaleList[setListIndex]),
                                    currentD,
                                    littleGFromFormula, oldBigGFromFormula, bigGFromFormula, formulaOneSum,
                                    density[dListIndex], formulaOne, formulaTwo, formulaThree,
                                    errorOne, errorTwo, errorThree, winner);
                }

                updateProgress(setList[setListIndex]);
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

        static uint getLittleG(uint[] scales, uint d)
        {
            uint[] gcdTwoArray = new uint[scales.Length + 1];
            for(int gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdTwoArray[gcdIndex] = scales[gcdIndex];
            }
            gcdTwoArray[scales.Length] = d;

            return NumberCruncher.GCD(gcdTwoArray);
        }

        static uint getBigG(uint[] set, uint[] scales, uint d, uint littleG)
        {
            uint[] gcdSet = new uint[scales.Length];
            for(int gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdSet[gcdIndex] = set[gcdIndex] * (scales[gcdIndex] % d);
            }

            return NumberCruncher.GCD(gcdSet) / littleG;
        }

        static uint getOldBigG(uint[] set, uint[] scales, uint d, uint littleG)
        {
            uint[] gcdSet = new uint[scales.Length + 1];
            for(int gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdSet[gcdIndex] = set[gcdIndex] * (scales[gcdIndex] % d);
            }

            gcdSet[scales.Length] = d;

            return NumberCruncher.GCD(gcdSet) / littleG;
        }
        public static double getFormulaOneSum(uint[] setList, uint[] scaleList, uint d)
        {
            uint littleGFromFormula = getLittleG(scaleList, d);
            uint bigGFromFomula = getOldBigG(setList, scaleList, d, littleGFromFormula);
            uint formulaSum = 0;

            for(uint sumIndex = 1; sumIndex < bigGFromFomula + 1; sumIndex++)
            {
                uint[] tempArray = { sumIndex, bigGFromFomula };
                formulaSum += NumberCruncher.GCD(tempArray);
            }

            return formulaSum;
        }

        public static double getFormulaOne(uint[] setList, uint[] scaleList, uint d)
        {
            uint littleGFromFormula = getLittleG(scaleList, d);
            uint bigGFromFomula = getOldBigG(setList, scaleList, d, littleGFromFormula);
            uint formulaSum = 0;

            for(uint sumIndex = 1; sumIndex < bigGFromFomula + 1; sumIndex++)
            {
                uint[] tempArray = { sumIndex, bigGFromFomula };
                formulaSum += NumberCruncher.GCD(tempArray);
            }

            return littleGFromFormula / (double)(d * bigGFromFomula) * formulaSum;
        }
        private static double getFormulaThree(string id, uint bigG, uint littleG, uint d, ulong inputSize)
        {
            uint[] formDivisor = { bigG };
            uint[] formScale = { 1 };
            uint[] formFactor = { d / littleG };

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

        private static void updateProgress(uint[] setList)
        {
            lock(updateProgressLocker)
            {
                numDone++;
            }
            Console.Out.WriteLine(numDone + " of " + totalToRun + "Completed");
            Console.Out.WriteLine(100 * numDone / totalToRun + "% done\n");
        }

        static string getArrayNumberString(uint[] numbers, uint[] scales)
        {
            string numString = "";
            Boolean firstNum = true;
            for(int index = 0; index < numbers.Length; index++)
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

                for(int timesWritten = 1; timesWritten < scales[index]; timesWritten++)
                {
                    numString += ", ";
                    numString += numbers[index];
                }
            }

            return numString;
        }

        static string getFileName(uint[] set, uint[] scale, string subDirectory)
        {
            string fileName = "data/" + subDirectory + "/{";
            bool isFirst = true;
            for(int index = 0; index < set.Length; index++)
            {
                for(int d = 0; d < scale[index]; d++)
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

