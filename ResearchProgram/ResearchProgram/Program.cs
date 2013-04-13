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
        private static readonly object locker = new object();
        private static readonly object updateNum = new object();
        private static int numDone = 0;
        static void Main(string[] args)
        {
            uint[][] divisorList1 = new uint[3][];
            uint[][] scaleList1 = new uint[3][];
            uint[][] factorList1 = new uint[3][];

            divisorList1[0] = new uint[] { 4 };
            scaleList1[0] = new uint[] { 1 };
            factorList1[0] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6) };


            divisorList1[1] = new uint[] { 6 };
            scaleList1[1] = new uint[] { 1 };
            factorList1[1] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6),
                                          (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6)};

            divisorList1[2] = new uint[] { 9 };
            scaleList1[2] = new uint[] { 1 };
            factorList1[2] = new uint[] { (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6)};

            new Thread(delegate()
            {
                multiTest(divisorList1, scaleList1, factorList1, "_2ThreadOne");
            }).Start();


            uint[][] divisorList2 = new uint[3][];
            uint[][] scaleList2 = new uint[3][];
            uint[][] factorList2 = new uint[3][];

            divisorList2[0] = new uint[] { 10 };
            scaleList2[0] = new uint[] { 1 };
            factorList2[0] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6) };

            divisorList2[1] = new uint[] { 12 };
            scaleList2[1] = new uint[] { 1 };
            factorList2[1] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6),
                                          (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6)};

            divisorList2[2] = new uint[] { 14 };
            scaleList2[2] = new uint[] { 1 };
            factorList2[2] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6),
                                          (uint)Math.Pow(7, 2),(uint)Math.Pow(7, 3),(uint)Math.Pow(7, 4),(uint)Math.Pow(7, 5),(uint)Math.Pow(7, 6)};

            new Thread(delegate()
            {
                multiTest(divisorList2, scaleList2, factorList2, "_2ThreadTwo");
            }).Start();

            uint[][] divisorList3 = new uint[3][];
            uint[][] scaleList3 = new uint[3][];
            uint[][] factorList3 = new uint[3][];

            divisorList3[0] = new uint[] { 15 };
            scaleList3[0] = new uint[] { 1 };
            factorList3[0] = new uint[] { (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6),
                                          (uint)Math.Pow(5, 2),(uint)Math.Pow(5, 3),(uint)Math.Pow(5, 4),(uint)Math.Pow(5, 5),(uint)Math.Pow(5, 6) };

            divisorList3[1] = new uint[] { 16 };
            scaleList3[1] = new uint[] { 1 };
            factorList3[1] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6) };

            divisorList3[2] = new uint[] { 18 };
            scaleList3[2] = new uint[] { 1 };
            factorList3[2] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6),
                                          (uint)Math.Pow(3, 2),(uint)Math.Pow(3, 3),(uint)Math.Pow(3, 4),(uint)Math.Pow(3, 5),(uint)Math.Pow(3, 6)};


            new Thread(delegate()
            {
                multiTest(divisorList3, scaleList3, factorList3, "_2ThreadThree");
            }).Start();

            uint[][] divisorList4 = new uint[3][];
            uint[][] scaleList4 = new uint[3][];
            uint[][] factorList4 = new uint[3][];

            divisorList4[0] = new uint[] { 4, 8, 12 };
            scaleList4[0] = new uint[] { 4, 4, 4 };
            factorList4[0] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6) };

            divisorList4[1] = new uint[] { 2, 6, 8, 12 };
            scaleList4[1] = new uint[] { 3, 2, 3, 2 };
            factorList4[1] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6) };

            divisorList4[2] = new uint[] { 2, 3, 4 };
            scaleList4[2] = new uint[] { 3, 4, 3 };
            factorList4[2] = new uint[] { (uint)Math.Pow(2, 2),(uint)Math.Pow(2, 3),(uint)Math.Pow(2, 4),(uint)Math.Pow(2, 5),(uint)Math.Pow(2, 6) };


            new Thread(delegate()
            {
                multiTest(divisorList4, scaleList4, factorList4, "_2ThreadFour");
            }).Start();

        }

        static void multiTest(uint[][] divisorList, uint[][] scaleList, uint[][] factorList, string atEnd)
        {
            ulong inputSize = 2000000000;
            DataTable dtInteresting = FileWriter.getTable();
            DataTable dtBoring = FileWriter.getTable();
            for(int index = 0; index < divisorList.Length; index++)
            {
                double[] density = NumberCruncher.densityOfUMultiK(inputSize, divisorList[index], scaleList[index], factorList[index]);

                for(int densityIndex = 0; densityIndex < density.Length; densityIndex++)
                {
                    DataTable table;

                    uint[] gcdOneArray = new uint[divisorList[index].Length + 1];
                    uint hasAllFactors = 1;
                    for(int gcdindex = 0; gcdindex < divisorList[index].Length; gcdindex++)
                    {
                        gcdOneArray[gcdindex] = divisorList[index][gcdindex];
                        hasAllFactors *= divisorList[index][gcdindex];
                    }

                    gcdOneArray[divisorList[index].Length] = hasAllFactors;
                    uint gcdOfSet = NumberCruncher.GCD(gcdOneArray);
                    gcdOneArray[divisorList[index].Length] = factorList[index][densityIndex];
                    uint gcdOne = NumberCruncher.GCD(gcdOneArray);

                    uint[] gcdTwoArray = new uint[scaleList[index].Length + 1];
                    for(int gcdindex = 0; gcdindex < scaleList[index].Length; gcdindex++)
                    {
                        gcdTwoArray[gcdindex] = scaleList[index][gcdindex];
                    }
                    gcdTwoArray[divisorList[index].Length] = factorList[index][densityIndex];
                    uint gcdFromFormula = NumberCruncher.GCD(gcdTwoArray);

                    uint[] gcdSet = new uint[scaleList[index].Length + 1];
                    for(int gcdIndex = 0; gcdIndex < scaleList[index].Length; gcdIndex++)
                    {
                        gcdSet[gcdIndex] = divisorList[index][gcdIndex] * scaleList[index][gcdIndex];
                    }

                    gcdSet[scaleList[index].Length] = factorList[index][densityIndex];

                    uint bigGFromFomula = NumberCruncher.GCD(gcdSet) / gcdFromFormula;

                    uint formulaSum = 0;

                    for(uint sumIndex = 1; sumIndex < bigGFromFomula + 1; sumIndex++)
                    {
                        uint[] tempArray = { sumIndex, bigGFromFomula };
                        formulaSum += NumberCruncher.GCD(tempArray);
                    }

                    double expectedDensity = gcdFromFormula / (double)(factorList[index][densityIndex] * bigGFromFomula) * formulaSum;

                    table = dtInteresting;
                    if(Math.Abs((expectedDensity - density[densityIndex]) / density[densityIndex]) > 0.07)
                    {
                        table = dtBoring;
                    }


                    table.Rows.Add(getArrayNumberString(divisorList[index], scaleList[index]), factorList[index][densityIndex], gcdOne, density[densityIndex], expectedDensity);                    
                }

                lock(updateNum) {
                    numDone++;
                }

                Console.Out.WriteLine( 100 * numDone / ((double)divisorList.Length*4) + "% done\n");
            }



            lock(locker)
            {
                FileWriter.saveTable("CommonFactor_April_7_Interesting" + atEnd + ".xlsx", dtInteresting);
                FileWriter.saveTable("CommonFactor_April_7_Boring" + atEnd + ".xlsx", dtBoring);
            }

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
        static void multiK(ulong inputSize, uint divisor, uint power, uint numPowers)
        {
            uint[] divisorArray = { divisor };
            uint[] scale = { 1 };
            string fileName = getFileName(divisorArray, scale, "Single") + " Power = " + power + ".txt";
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            fileInfo.Directory.Create();

            uint[] factors = new uint[numPowers];
            uint newK = (uint)Math.Pow(divisor, power);
            uint offset = 0;
            for(uint index = 0; index < factors.Length; index++)
            {
                if((index + 1 + offset) % divisor == 0)
                {
                    offset++;
                    index--;
                    continue;
                }
                factors[index] = newK * (index + 1 + offset);
            }

            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, false))
            {

                double[] density = NumberCruncher.densityOfUMultiK(inputSize * Math.Max(1, (power / 2)), divisorArray, scale, factors);

                for(uint index = 0; index < factors.Length; index++)
                {
                    uint toMul = divisor * factors[index];
                    uint toSub = (power + 1) * divisor;

                    String toWrite = " The density for k = " + factors[index].ToString() + " is " + density[index].ToString() + "\n this number times " + toMul.ToString() + " minus " + toSub.ToString() + " equals " + (density[index] * toMul - toSub).ToString();
                    file.WriteLine(toWrite);
                }
            }
            Console.WriteLine("Finished " + divisor + ", pow = " + power);

        }
        static void writeResults(ulong inputSize, uint[] divisors, uint[] scale, uint[] factors)
        {
            string fileName = getFileName(divisors, scale, "Multi");
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileName);
            fileInfo.Directory.Create();
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, false))
            {
                foreach(uint factor in factors)
                {
                    double density = NumberCruncher.densityOfU(inputSize, divisors, scale, factor);
                    String toWrite = " The density for k = " + factor.ToString() + " is " + density.ToString();
                    file.WriteLine(toWrite);
                }
            }
        }
        static string getFileName(uint[] divisors, uint[] scale, string subDirectory)
        {
            string fileName = "data/" + subDirectory + "/{";
            bool isFirst = true;
            for(int index = 0; index < divisors.Length; index++)
            {
                for(int k = 0; k < scale[index]; k++)
                {
                    if(isFirst)
                    {
                        fileName += divisors[index];
                        isFirst = false;
                    }
                    else
                    {
                        fileName += ", " + divisors[index];
                    }
                }
            }
            fileName += "}/results.txt";
            return fileName;
        }
    }
}
