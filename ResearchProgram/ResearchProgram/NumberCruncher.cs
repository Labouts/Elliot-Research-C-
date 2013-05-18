using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace ResearchProgram
{
    static class NumberCruncher
    {
        public static double[] densityOfUMultiD(string id, ulong inputSize, ulong[] set, ulong[] scale, ulong[] dList)
        {
            ulong[] currNumFactors = new ulong[dList.Length];
            ulong[] totalFactors = new ulong[dList.Length];
            ulong[] numWasTrue = new ulong[dList.Length];

            ulong betweenPrlong = 100000000;
            ulong tillPrlong = betweenPrlong;

            for(ulong number = 0; number < inputSize; number++)
            {                
                ulong toAdd = 0;
                for(uint index = 0; index < set.Length; index++)
                {
                    toAdd += w(number, set[index]) * scale[index];
                }

                for(uint index = 0; index < dList.Length; index++)
                {
                    currNumFactors[index] += toAdd;
                    currNumFactors[index] %= dList[index];
                    totalFactors[index] += currNumFactors[index];
                    totalFactors[index] %= dList[index];

                    if(totalFactors[index] == 0)
                        numWasTrue[index] += 1;
                }

                tillPrlong--;

                if(tillPrlong <= 0)
                {
                    tillPrlong = betweenPrlong;
                    for(uint index = 0; index < dList.Length; index++) { 
                        Console.Out.WriteLine(id + " is " + number / (double)inputSize * 100 + "% done");
                        double dense = numWasTrue[0]/(double)number;
                        Console.Out.WriteLine("numTrue is " + numWasTrue[0]);
                        Console.Out.WriteLine("First is " + dense);                 
                }
                }
            }

            double[] density = new double[dList.Length];

            for(long index = 0; index < dList.Length; index++)
            {
                density[index] = numWasTrue[index] / (double)inputSize;
            }

            return density;
        }

        public static ulong w(ulong number, ulong divisor)
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

        public static ulong GCD(params ulong[] numbers)
        {
            Func<ulong, ulong, ulong> gcd = null;
            gcd = (a, b) => (b == 0 ? a : gcd(b, a % b));
            return numbers.Aggregate(gcd);
        }
    }
}
