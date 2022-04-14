using System.Collections;
using System.Drawing;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine
{
    public class SpatialGrid<T> : IEnumerable<List<T>>
    {
        public SpatialGrid(int cellSize)
        {
            CellSize = cellSize;
        }
        private struct CellId { public CellId(int x, int y){ X=x; Y=y; } public int X; public int Y; }
        private Dictionary<CellId, List<T>> buckets = new();

        public int CellSize { get; }

        private List<T> GetBucket(CellId cell){
            if(buckets.TryGetValue(cell, out var b)){
                return b;
            } else {
                var q = new List<T>(16);
                buckets.Add(cell, q);
                return q;
            }
        }

        public void Insert(T item, c2AABB r){
            int minX = (int)r.min.X / CellSize;
            int minY = (int)r.min.Y / CellSize;
            int maxX = (int)r.max.X / CellSize;
            int maxY = (int)r.max.Y / CellSize;

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    var cell = new CellId(x,y);
                    var bucket = GetBucket(cell);
                    bucket.Add(item);
                }
            }
        }

        public IEnumerator<List<T>> GetEnumerator()
            => buckets.Values.GetEnumerator();
        

        IEnumerator IEnumerable.GetEnumerator()
            => buckets.Values.GetEnumerator();
    }
}