using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.PairsAnalysis
{
    public static class Analysis
    {
        public static int FindMaxPeriodIndex(params DateTime[] data)
        {
            return data.Pairs().MaxIndex();
        }

        public static double FindAverageRelativeDifference(params double[] data)
        {
            return data.Aggregate();
        }

        public static int MaxIndex<T>(this IEnumerable<T> data)
            where T : IComparable
        {
            var maxValue = default(T);
            var maxIndex = -1;
            var index = 0;
            foreach (var el in data)
            {
                if (el.CompareTo(maxValue) == 1)
                {
                    maxValue = el;
                    maxIndex = index;
                }
                index++;
            }
            if (index == 0) throw new InvalidOperationException();
            return maxIndex;
        }

        public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> data)
            where T : struct
        {
            var last_el = default(T);
            foreach (var el in data)
            {
                if (!last_el.Equals(null)) yield return new Tuple<T, T>(last_el, el);
                last_el = el;
            }
        }

        public static double Aggregate(this IEnumerable<double> temp)
        {
            var sum = 0.0;
            for (int i = 0; i < temp.Count(); i++)
                sum += temp.ElementAt(i + 1);
            return sum / temp.Count();
        }
    }
}