using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace ResearchProgram
{
    static class NumberCruncher
    {
        public static double[] densityOfUMultiD(string id, ulong inputSize, uint[] set, uint[] scale, uint[] dList)
        {
            ulong totalFactors = 0;
            ulong currNumFactors = 0;
            ulong[] numWasTrue = new ulong[dList.Length];

            uint betweenPrint = 100000000;
            uint tillPrint = betweenPrint;

            for(ulong number = 0; number < inputSize; number++)
            {
                for(uint index = 0; index < set.Length; index++)
                {
                    currNumFactors += w(number, set[index]) * scale[index];
                }
                totalFactors += currNumFactors;

                for(int index = 0; index < dList.Length; index++)
                {
                    if(totalFactors % dList[index] == 0)
                        numWasTrue[index] += 1;
                }

                tillPrint--;

                if(tillPrint <= 0)
                {
                    tillPrint = betweenPrint;
                    Console.Out.WriteLine(id + " is " + number / (double)inputSize * 100 + "% done");
                }
            }

            double[] density = new double[dList.Length];

            for(int index = 0; index < dList.Length; index++)
            {
                density[index] = numWasTrue[index] / (double)inputSize;
            }

            return density;
        }

        public static double[] densityOfUMultipleSets(string id, ulong inputSize, uint[][] setList, uint[][] scale, uint[] dList)
        {
            ulong[] totalFactors = new ulong[setList.Length];
            ulong[] currNumFactors = new ulong[setList.Length];
            ulong[] numWasTrue = new ulong[setList.Length + 1];

            uint betweenPrint = 100000000;
            uint tillPrint = betweenPrint;

            for(ulong number = 0; number < inputSize; number++)
            {
                for(uint setIndex = 0; setIndex < setList.Length; setIndex++)
                {
                    for(uint numberIndex = 0; numberIndex < setList[setIndex].Length; numberIndex++)
                    {
                        currNumFactors[setIndex] += w(number, setList[setIndex][numberIndex]) * scale[setIndex][numberIndex];
                    }

                    totalFactors[setIndex] += currNumFactors[setIndex];
                }

                Boolean allTrue = true;
                for(int index = 0; index < setList.Length; index++)
                {
                    if(totalFactors[index] % dList[index] == 0)
                    {
                        numWasTrue[index] += 1;
                    }
                    else
                    {
                        allTrue = false;
                    }
                }

                if(allTrue)
                {
                    numWasTrue[setList.Length] += 1;
                }

                tillPrint--;

                if(tillPrint <= 0)
                {
                    tillPrint = betweenPrint;
                    Console.Out.WriteLine(id + " is " + number / (double)inputSize * 100 + "% done");
                }
            }

            double[] density = new double[dList.Length+1];

            for(int index = 0; index < dList.Length+1; index++)
            {
                density[index] = numWasTrue[index] / (double)inputSize;
            }

            return density;
        }

        public static ulong w(ulong number, uint divisor)
        {
            if(number < 1)
                return 0;

            ulong count = 0;
            while(number % divisor == 0)
            {
                number /= divisor;
                count++;
            }
            return count;
        }

        public static uint GCD(params uint[] numbers)
        {
            Func<uint, uint, uint> gcd = null;
            gcd = (a, b) => (b == 0 ? a : gcd(b, a % b));
            return numbers.Aggregate(gcd);
        }
    }
}
