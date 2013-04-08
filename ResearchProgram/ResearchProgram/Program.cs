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
        static void Main(string[] args)
        {
            uint[][] divisorList1 = new uint[2][];
            uint[][] scaleList1 = new uint[2][];
            uint[][] factorList1 = new uint[2][];

            divisorList1[0] = new uint[] { 11, 41 };
            scaleList1[0] = new uint[] { 5, 2 };
            factorList1[0] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            divisorList1[1] = new uint[] { 2, 3, 4 };
            scaleList1[1] = new uint[] { 2, 4, 6 };
            factorList1[1] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            new Thread(delegate()
            {
                multiTest(divisorList1, scaleList1, factorList1, "_2ThreadOne");
            }).Start();

            uint[][] divisorList2 = new uint[2][];
            uint[][] scaleList2 = new uint[2][];
            uint[][] factorList2 = new uint[2][];

            divisorList2[0] = new uint[] { 2, 4, 6 };
            scaleList2[0] = new uint[] { 3, 3, 3 };
            factorList2[0] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            divisorList2[1] = new uint[] { 9, 18 };
            scaleList2[1] = new uint[] { 2 , 4 };
            factorList2[1] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            new Thread(delegate()
            {
                multiTest(divisorList2, scaleList2, factorList2, "_2ThreadTwo");
            }).Start();

            uint[][] divisorList3 = new uint[2][];
            uint[][] scaleList3 = new uint[2][];
            uint[][] factorList3 = new uint[2][];

            divisorList3[0] = new uint[] { 5, 3 };
            scaleList3[0] = new uint[] { 3, 5 };
            factorList3[0] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            divisorList3[1] = new uint[] { 3, 5, 7 };
            scaleList3[1] = new uint[] { 1, 3, 3 };
            factorList3[1] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };


            new Thread(delegate()
            {
                multiTest(divisorList3, scaleList3, factorList3, "_2ThreadThree");
            }).Start();

            uint[][] divisorList4 = new uint[2][];
            uint[][] scaleList4 = new uint[2][];
            uint[][] factorList4 = new uint[2][];

            divisorList4[0] = new uint[] { 2, 3, 5, 8, 13 };
            scaleList4[0] = new uint[] { 2, 1, 1, 1, 1, 1 };
            factorList4[0] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

            divisorList4[1] = new uint[] { 5, 7, 11 };
            scaleList4[1] = new uint[] { 6, 3, 9 };
            factorList4[1] = new uint[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };


            new Thread(delegate()
            {
                multiTest(divisorList4, scaleList4, factorList4, "_2ThreadFour");
            }).Start();
        }
        static string getGCDS(uint[] scaleList, uint factor)
        {
            HashSet<uint> alreadyCounted = new HashSet<uint>( );
            String toShow = "";
            foreach(uint scale in scaleList)
            {
                uint gcd = NumberCruncher.GCD(scale, factor);



                if(gcd > 1 && !alreadyCounted.Contains(gcd)) {
                    if(toShow != "")
                    {
                        toShow += ", ";
                    }

                    toShow += gcd.ToString( );
                    alreadyCounted.Add(gcd);
                }
            }

            if(toShow == "") { 
                toShow += "1";
            }

            return toShow;
        }

        static void multiTest(uint[][] divisorList, uint[][] scaleList, uint[][] factorList, string atEnd)
        {
            ulong inputSize = 1000000000;
            DataTable dtInteresting = FileWriter.getTable();
            DataTable dtBoring = FileWriter.getTable();
            for(int index = 0; index < divisorList.Length; index++)
            {
                double[] density = NumberCruncher.densityOfUMultiK(inputSize, divisorList[index], scaleList[index], factorList[index]);

                for(int densityIndex = 0; densityIndex < density.Length; densityIndex++)
                {
                    DataTable table;

                    if(Math.Abs(density[densityIndex] * factorList[index][densityIndex] - 1) > 0.1)
                        table = dtInteresting;
                    else
                        table = dtBoring;

                    uint[] gcdOneArray = new uint[divisorList[index].Length+1];

                    for( int gcdindex = 0; gcdindex < divisorList[index].Length; gcdindex++ ) {
                        gcdOneArray[gcdindex] = divisorList[index][gcdindex];
                    }

                    gcdOneArray[divisorList[index].Length] = factorList[index][densityIndex];

                    table.Rows.Add(getArrayNumberString(divisorList[index], scaleList[index]), factorList[index][densityIndex], NumberCruncher.GCD(gcdOneArray),getGCDS(scaleList[index], factorList[index][densityIndex]), density[densityIndex], density[densityIndex] * factorList[index][densityIndex]);
                }

                Console.Out.WriteLine((index+1) / (double)divisorList.Length + "% done with entire program \n");
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
