using System;
using System.Collections.Generic;
using System.Linq;

namespace net6test.MapGenerator
{
    public class ProdGen
    {
        private readonly Random random;

        public ProdGen(int seed)
        {
            this.random = new Random(seed);
        }

        public int Size(int s, int threshold) => random.Next(s - threshold * 2) + threshold;
        public int Range(int a, int b, int threshold = 0) => a + Size(b - a, threshold);
        public float RangeF(float a, float b) => a + (((float)random.NextDouble()) * (b-a));
        public T Select<T>(List<T> l)
        {
            if (l.Count == 0) return default(T);
            return l[random.Next(l.Count)];
        }

        public T Select<T>(IEnumerable<T> e)
        {
            var l = e.ToList();
            return l[random.Next(l.Count)];
        }

        public bool Roll(int percent) => random.Next(100) < percent;

        public void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }


}