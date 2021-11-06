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
        public int Range(int a, int b, int threshold) => a + Size(b - a, threshold);
        public T Select<T>(List<T> l) => l[random.Next(l.Count)];
        public T Select<T>(IEnumerable<T> e)
        {
            var l = e.ToList();
            return l[random.Next(l.Count)];
        }

        public bool Roll(int percent) => random.Next(100) < percent;
    }


}