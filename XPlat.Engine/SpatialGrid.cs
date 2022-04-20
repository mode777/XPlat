using System.Collections;
using System.Drawing;
using static TinyC2.TinyC2Api;

namespace XPlat.Engine
{
    public class Bucket<T> : IEnumerable<T> where T : class
    {
        private List<T> _items;
        private int _count = 0;
        public int Count => _count;

        public Bucket(int capacity)
        {
            this._items = new(capacity);
        }

        public void Clear()
        {
            for (int i = 0; i < _count; i++)
            {
                _items[i] = null;
            }
            _count = 0;
        }

        public T this[int i] { get => _items[i]; }

        public void Add(T item)
        {
            if(_count < _items.Count)
            {
                _items[_count] = item;
            } else
            {
                _items.Add(item);
            }
            _count++;
        }

        public IEnumerator<T> GetEnumerator() => _items.Take(_count).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _items.Take(_count).GetEnumerator();
    }

    public class SpatialGrid<T> : IEnumerable<Bucket<T>> where T : class
    {
        public SpatialGrid(int cellSize)
        {
            CellSize = cellSize;
        }
        private struct CellId { public CellId(int x, int y){ X=x; Y=y; } public int X; public int Y; }
        private Dictionary<CellId, Bucket<T>> buckets = new();

        public int CellSize { get; }

        private Bucket<T> GetBucket(CellId cell){
            if(buckets.TryGetValue(cell, out var b)){
                return b;
            } else {
                var q = new Bucket<T>(16);
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

        public IEnumerator<Bucket<T>> GetEnumerator()
            => buckets.Values.GetEnumerator();
        

        IEnumerator IEnumerable.GetEnumerator()
            => buckets.Values.GetEnumerator();
    }
}