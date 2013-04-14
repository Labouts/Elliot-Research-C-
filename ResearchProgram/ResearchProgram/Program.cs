using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace ResearchProgram
{
    class Program
    {
        private static int numDone = 0;
        private static int totalToRun = 0;
        private static Dictionary<ulong, Dictionary<ulong, double>> densityCache = new Dictionary<ulong, Dictionary<ulong, double>>();
        private static readonly object excelWriteLocder = new object();
        private static readonly object updateProgressLocder = new object();

        static void Main(string[] args)
        {
            uint[][] setList1 = new uint[3][];
            uint[][] scaleList1 = new uint[3][];
            uint[][] dList1 = new uint[3][];

            setList1[0] = new uint[] { 3 * 5 };
            scaleList1[0] = new uint[] { 1 };
            dList1[0] = new uint[] { (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6), (uint)Math.Pow(2, 7),(uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6), (uint)Math.Pow(5, 7),(uint)Math.Pow(5, 8),
                                          (uint)Math.Pow(3, 2)*(uint)Math.Pow(5, 2),(uint)Math.Pow(3, 3)*(uint)Math.Pow(5, 3),(uint)Math.Pow(3, 4) * (uint)Math.Pow(5, 4),(uint)Math.Pow(3, 2) * (uint)Math.Pow(5, 5), (uint)Math.Pow(3, 2) * (uint)Math.Pow(5, 6), (uint)Math.Pow(3, 3) * (uint)Math.Pow(5, 7), 3 * (uint)Math.Pow(5, 8)};


            setList1[1] = new uint[] { 2 * 7 };
            scaleList1[1] = new uint[] { 1 };
            dList1[1] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6), (uint)Math.Pow(2, 7),(uint)Math.Pow(2, 8),
                                          (uint)Math.Pow(7, 2),(uint)Math.Pow(7, 3),(uint)Math.Pow(7, 4),(uint)Math.Pow(7, 5),(uint)Math.Pow(7, 6), (uint)Math.Pow(7, 7),(uint)Math.Pow(7, 8),
                                          (uint)Math.Pow(2, 2)*(uint)Math.Pow(7, 2),(uint)Math.Pow(2, 3)*(uint)Math.Pow(7, 3),(uint)Math.Pow(2, 4) * (uint)Math.Pow(7, 4),(uint)Math.Pow(2, 2) * (uint)Math.Pow(7, 5), (uint)Math.Pow(2, 2) * (uint)Math.Pow(7, 6), (uint)Math.Pow(2, 3) * (uint)Math.Pow(7, 7), 3 * (uint)Math.Pow(7, 8)};

            setList1[2] = new uint[] { 3 * 7 };
            scaleList1[2] = new uint[] { 1 };
            dList1[2] = new uint[] { (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6), (uint)Math.Pow(3, 7),(uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(7, 2),(uint)Math.Pow(7, 3),(uint)Math.Pow(7, 4),(uint)Math.Pow(7, 5),(uint)Math.Pow(7, 6), (uint)Math.Pow(7, 7),(uint)Math.Pow(7, 8),
                                          (uint)Math.Pow(3, 2)*(uint)Math.Pow(7, 2),(uint)Math.Pow(3, 3)*(uint)Math.Pow(7, 3),(uint)Math.Pow(3, 4) * (uint)Math.Pow(7, 4),(uint)Math.Pow(3, 2) * (uint)Math.Pow(7, 5), (uint)Math.Pow(3, 2) * (uint)Math.Pow(7, 6), (uint)Math.Pow(3, 3) * (uint)Math.Pow(7, 7), 3 * (uint)Math.Pow(7, 8)};


            uint[][] setList2 = new uint[3][];
            uint[][] scaleList2 = new uint[3][];
            uint[][] dList2 = new uint[3][];

            setList2[0] = new uint[] { 2 * 5 * 7 };
            scaleList2[0] = new uint[] { 1 };
            dList2[0] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6), (uint)Math.Pow(2, 7),(uint)Math.Pow(2, 8),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6), (uint)Math.Pow(5, 7),(uint)Math.Pow(5, 8),
                                          (uint)Math.Pow(7, 2),(uint)Math.Pow(7, 3),(uint)Math.Pow(7, 4),(uint)Math.Pow(7, 5),(uint)Math.Pow(7, 6), (uint)Math.Pow(7, 7),(uint)Math.Pow(7, 8),
                                          (uint)Math.Pow(5, 2)*(uint)Math.Pow(7, 2),(uint)Math.Pow(5, 3)*(uint)Math.Pow(7, 3),(uint)Math.Pow(5, 4) * (uint)Math.Pow(7, 4),(uint)Math.Pow(5, 2) * (uint)Math.Pow(7, 5), (uint)Math.Pow(5, 2) * (uint)Math.Pow(7, 6), (uint)Math.Pow(5, 3) * (uint)Math.Pow(7, 7), 3 * (uint)Math.Pow(7, 8),
                                          (uint)Math.Pow(2, 2)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(7, 2),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 3)*(uint)Math.Pow(7, 3),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(7, 2), (uint)Math.Pow(2, 4)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(7, 2)};

            setList2[1] = new uint[] { 5 * 7 };
            scaleList2[1] = new uint[] { 1 };
            dList2[1] = new uint[] { (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6), (uint)Math.Pow(5, 7),(uint)Math.Pow(5, 8),
                                          (uint)Math.Pow(7, 2),(uint)Math.Pow(7, 3),(uint)Math.Pow(7, 4),(uint)Math.Pow(7, 5),(uint)Math.Pow(7, 6), (uint)Math.Pow(7, 7),(uint)Math.Pow(7, 8),
                                          (uint)Math.Pow(5, 2)*(uint)Math.Pow(7, 2),(uint)Math.Pow(5, 3)*(uint)Math.Pow(7, 3),(uint)Math.Pow(5, 4) * (uint)Math.Pow(7, 4),(uint)Math.Pow(5, 2) * (uint)Math.Pow(7, 5), (uint)Math.Pow(5, 2) * (uint)Math.Pow(7, 6), (uint)Math.Pow(5, 3) * (uint)Math.Pow(7, 7), 3 * (uint)Math.Pow(7, 8)};


            setList2[2] = new uint[] { 2, 7 };
            scaleList2[2] = new uint[] { 5, 3 };
            dList2[2] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6),
                                          (uint)Math.Pow(7, 2),(uint)Math.Pow(7, 3),(uint)Math.Pow(7, 4),(uint)Math.Pow(7, 5),(uint)Math.Pow(7, 6)};


            uint[][] setList3 = new uint[3][];
            uint[][] scaleList3 = new uint[3][];
            uint[][] dList3 = new uint[3][];

            setList3[0] = new uint[] { 12, 14, 16 };
            scaleList3[0] = new uint[] { 1, 1, 1 };
            dList3[0] = new uint[] { (uint)Math.Pow(2, 2), (uint)Math.Pow(2, 3), (uint)Math.Pow(2, 4), (uint)Math.Pow(2, 5), (uint)Math.Pow(2, 6), (uint)Math.Pow(2, 7), (uint)Math.Pow(2, 8), (uint)Math.Pow(2, 9), (uint)Math.Pow(2, 10), (uint)Math.Pow(2, 11), (uint)Math.Pow(2, 12), };

            setList3[1] = new uint[] { 2, 3, 5 };
            scaleList3[1] = new uint[] { 1, 1, 1 };
            dList3[1] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6), (uint)Math.Pow(2, 7),(uint)Math.Pow(2, 8),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6), (uint)Math.Pow(5, 7),(uint)Math.Pow(5, 8),
                                          (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6), (uint)Math.Pow(3, 7),(uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2),(uint)Math.Pow(5, 3)*(uint)Math.Pow(3, 3),(uint)Math.Pow(5, 4) * (uint)Math.Pow(3, 4),(uint)Math.Pow(5, 2) * (uint)Math.Pow(3, 5), (uint)Math.Pow(5, 2) * (uint)Math.Pow(3, 6), (uint)Math.Pow(5, 3) * (uint)Math.Pow(3, 7), 3 * (uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(2, 2)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 3)*(uint)Math.Pow(3, 3),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2), (uint)Math.Pow(2, 4)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2)};

            setList3[2] = new uint[] { 2, 3, 5 };
            scaleList3[2] = new uint[] { 7, 11, 7 };
            dList3[2] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6), (uint)Math.Pow(2, 7),(uint)Math.Pow(2, 8),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6), (uint)Math.Pow(5, 7),(uint)Math.Pow(5, 8),
                                          (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6), (uint)Math.Pow(3, 7),(uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2),(uint)Math.Pow(5, 3)*(uint)Math.Pow(3, 3),(uint)Math.Pow(5, 4) * (uint)Math.Pow(3, 4),(uint)Math.Pow(5, 2) * (uint)Math.Pow(3, 5), (uint)Math.Pow(5, 2) * (uint)Math.Pow(3, 6), (uint)Math.Pow(5, 3) * (uint)Math.Pow(3, 7), 3 * (uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(2, 2)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 3)*(uint)Math.Pow(3, 3),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2), (uint)Math.Pow(2, 4)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2)}; ;


            uint[][] setList4 = new uint[3][];
            uint[][] scaleList4 = new uint[3][];
            uint[][] dList4 = new uint[3][];

            setList4[0] = new uint[] { 4, 8, 12 };
            scaleList4[0] = new uint[] { 9, 3, 3 };
            dList4[0] = new uint[] { (uint)Math.Pow(2, 2), (uint)Math.Pow(2, 3), (uint)Math.Pow(2, 4), (uint)Math.Pow(2, 5), (uint)Math.Pow(2, 6) };

            setList4[1] = new uint[] { 2, 3, 5, 7 };
            scaleList4[1] = new uint[] { 1, 1, 1, 1 };
            dList4[1] = new uint[] { (uint)Math.Pow(2, 2) * (uint)Math.Pow(3, 2) * (uint)Math.Pow(5, 2) * (uint)Math.Pow(7, 2),(uint)Math.Pow(2, 2), (uint)Math.Pow(3, 2), (uint)Math.Pow(5, 2), (uint)Math.Pow(7, 2),
                                          (uint)Math.Pow(2, 2) * (uint)Math.Pow(3, 2), (uint)Math.Pow(2, 2) * (uint)Math.Pow(5, 2), (uint)Math.Pow(2, 2) * (uint)Math.Pow(7, 2), (uint)Math.Pow(3, 2) * (uint)Math.Pow(5, 2),
                                          (uint)Math.Pow(3, 2) * (uint)Math.Pow(7, 2), (uint)Math.Pow(5, 2) * (uint)Math.Pow(7, 2)};

            setList4[2] = new uint[] { 2, 3, 7 };
            scaleList4[2] = new uint[] { 3, 5, 3 };
            dList4[2] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6), (uint)Math.Pow(2, 7),(uint)Math.Pow(2, 8),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6), (uint)Math.Pow(5, 7),(uint)Math.Pow(5, 8),
                                          (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6), (uint)Math.Pow(3, 7),(uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2),(uint)Math.Pow(5, 3)*(uint)Math.Pow(3, 3),(uint)Math.Pow(5, 4) * (uint)Math.Pow(3, 4),(uint)Math.Pow(5, 2) * (uint)Math.Pow(3, 5), (uint)Math.Pow(5, 2) * (uint)Math.Pow(3, 6), (uint)Math.Pow(5, 3) * (uint)Math.Pow(3, 7), 3 * (uint)Math.Pow(3, 8),
                                          (uint)Math.Pow(2, 2)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 3)*(uint)Math.Pow(3, 3),(uint)Math.Pow(2, 3)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2), (uint)Math.Pow(2, 4)*(uint)Math.Pow(5, 2)*(uint)Math.Pow(3, 2)}; ;

            totalToRun = setList1.Length + setList2.Length + setList3.Length + setList4.Length;

            new Thread(delegate()
            {
                multiTest(setList1, scaleList1, dList1, "_2ThreadOne");
            }).Start();

            new Thread(delegate()
            {
                multiTest(setList2, scaleList2, dList2, "_2ThreadTwo");
            }).Start();

            new Thread(delegate()
            {
                multiTest(setList3, scaleList3, dList3, "_2ThreadThree");
            }).Start();

            new Thread(delegate()
            {
                multiTest(setList4, scaleList4, dList4, "_2ThreadFour");
            }).Start();

        }

        static void multiTest(uint[][] setList, uint[][] scaleList, uint[][] dList, string toAppendToFile)
        {
            ulong inputSize = 100000;
            DataTable dtClose = FileWriter.getTable();
            DataTable dtNotClose = FileWriter.getTable();

            for(int setListIndex = 0; setListIndex < setList.Length; setListIndex++)
            {
                double[] density = NumberCruncher.densityOfUMultiD(inputSize, setList[setListIndex], scaleList[setListIndex], dList[setListIndex]);

                for(int dListIndex = 0; dListIndex < density.Length; dListIndex++)
                {
                    uint currentD = dList[setListIndex][dListIndex];
                    uint littleGFromFormula = getLittleG(scaleList[setListIndex], currentD);
                    uint bigGFromFormula = getBigG(setList[setListIndex], scaleList[setListIndex], currentD, littleGFromFormula);
                    double expectedDensity = getExpectedDensity(bigGFromFormula, littleGFromFormula, currentD, inputSize);

                    DataTable table = dtClose;
                    double error = Math.Abs((expectedDensity - density[dListIndex]) / density[dListIndex]);
                    if(error > 0.06)
                    {
                        table = dtNotClose;
                    }
                    double mulDivisor = density[dListIndex] * littleGFromFormula * currentD;

                    table.Rows.Add(getArrayNumberString(setList[setListIndex], scaleList[setListIndex]),
                                    currentD,
                                    littleGFromFormula, bigGFromFormula,
                                    density[dListIndex], expectedDensity,
                                    error, mulDivisor);
                }

                updateProgress(setList[setListIndex]);
            }

            lock(excelWriteLocder)
            {
                FileWriter.saveTable("CommonFactor_April_13_Close" + toAppendToFile + ".xlsx", dtClose);
                FileWriter.saveTable("CommonFactor_April_13_NotClose" + toAppendToFile + ".xlsx", dtNotClose);
            }
        }

        static uint getLittleG(uint[] scales, uint d)
        {
            uint[] gcdTwoArray = new uint[scales.Length + 1];
            for(int gcdindex = 0; gcdindex < scales.Length; gcdindex++)
            {
                gcdTwoArray[gcdindex] = scales[gcdindex];
            }
            gcdTwoArray[scales.Length] = d;

            return NumberCruncher.GCD(gcdTwoArray);
        }

        static uint getBigG(uint[] set, uint[] scales, uint d, uint littleG)
        {
            uint[] gcdSet = new uint[scales.Length + 1];
            for(int gcdIndex = 0; gcdIndex < scales.Length; gcdIndex++)
            {
                gcdSet[gcdIndex] = set[gcdIndex] * (scales[gcdIndex] % d);
            }

            gcdSet[scales.Length] = d;

            return NumberCruncher.GCD(gcdSet) / littleG;
        }

        private static double getExpectedDensity(uint bigG, uint littleG, uint d, ulong inputSize)
        {
            uint[] formDivisor = { bigG };
            uint[] formScale = { 1 };
            uint[] formFactor = { d / littleG };

            double expectedDensity;
            if(formFactor[0] == 1 || formDivisor[0] == 1)
            {
                expectedDensity = 1;
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
                        densityCache[formDivisor[0]].Add(formFactor[0], NumberCruncher.densityOfUMultiD(inputSize, formDivisor, formScale, formFactor)[0]);
                    }
                    expectedDensity = densityCache[formDivisor[0]][formFactor[0]];
                }
            }

            return expectedDensity;
        }

        private static void updateProgress(uint[] setList)
        {
            lock(updateProgressLocder)
            {
                numDone++;
            }

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
