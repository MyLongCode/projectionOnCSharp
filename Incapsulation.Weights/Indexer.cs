using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.Weights
{
    public class Indexer
    {
        private double[] container;
        private int begin;
        private int end;

        public Indexer(double[] array, int start, int length)
        {
            if (length > array.Length 
                || start < 0 || length < 0 
                || start + length > array.Length) throw new ArgumentException();
            container = array;
            this.begin = start;
            this.end = length;
        }

        public int Length
        {
            get
            {
                return end - begin + 1;
            }
        }

        public double this[int num]
        {
            get
            {
                if (num < 0 || num >= Length) throw new IndexOutOfRangeException();
                else return container[num + begin];
            }
            set
            {
                if (num < 0 || num >= Length) throw new IndexOutOfRangeException();
                container[num + begin] = value;
            }

        }
    }
}