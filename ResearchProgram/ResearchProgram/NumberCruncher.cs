using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;

namespace ResearchProgram
{
    static class NumberCruncher
    {
        public static double densityOfU(ulong inputSize, uint[] divisors,uint[] scale, uint factor)
        {
            ulong totalFactors = 0;
            ulong currNumFactors = 0;
            ulong numWasTrue = 0;
            uint toInc = GCD(divisors);

            for(ulong number = 0; number < inputSize; number++)
            {
                for(uint index = 0; index < divisors.Length; index++)
                {
                    currNumFactors += w(number, divisors[index]) * scale[index];
                }
                totalFactors += currNumFactors;

                if(totalFactors % factor == 0)
                    numWasTrue += 1;
            }

            return numWasTrue / ((double)inputSize);
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

        public static double[] densityOfUMultiK(ulong inputSize, uint[] divisors, uint[] scale, uint[] factors)
        {
            ulong totalFactors = 0;
            ulong currNumFactors = 0;
            ulong[] numWasTrue = new ulong[factors.Length];

            for(ulong number = 0; number < inputSize; number++)
            {
                for(uint index = 0; index < divisors.Length; index++)
                {
                    currNumFactors += w(number, divisors[index]) * scale[index];
                }
                totalFactors += currNumFactors;

                for(int index = 0; index < factors.Length; index++)
                {
                    if(totalFactors % factors[index] == 0)
                        numWasTrue[index] += 1;
                }                
            }

            double[] density = new double[factors.Length];

            for(int index = 0; index < factors.Length; index++)
            {
                density[index] = numWasTrue[index] / (double)inputSize;
            }

            return density;
        }
    }
}
