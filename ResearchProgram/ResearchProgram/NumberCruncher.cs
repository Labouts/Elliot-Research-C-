using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace ResearchProgram
{
    static class NumberCruncher
    {
        public static double[] densityOfUMultiD(ulong inputSize, uint[] set, uint[] scale, uint[] dList)
        {
            ulong totalFactors = 0;
            ulong currNumFactors = 0;
            ulong[] numWasTrue = new ulong[dList.Length];

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
            }

            double[] density = new double[dList.Length];

            for(int index = 0; index < dList.Length; index++)
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
