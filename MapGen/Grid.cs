using System.Collections;
using System.Drawing;

namespace net6test.MapGenerator
{
    public class Grid<T> : IEnumerable {
        private readonly int w;
        private readonly int h;
        private readonly T @default;
        private readonly T seed;
        private T[] data;

        public int W => w;
        public int H => h;

        public Grid(int w, int h, T @default, T seed)
        {
            this.w = w;
            this.h = h;
            this.@default = @default;
            this.seed = seed;
            this.data = new T[w*h];
            Clear();
        }

        public void Clear()
        {
            for (int i = 0; i < this.data.Length; i++)
            {
                data[i] = seed;
            }
        }

        public T this[int i] {
            get => data[i];
            set {
                if(IsOutOfBounds(i)) return;
                data[i] = value;
            }
        }

        public T this[int x, int y] {
            get => IsOutOfBounds(x,y) ? @default : data[y*w+x];
            set {
                if(IsOutOfBounds(x,y)) return;
                data[y*w+x] = value;
            }
        }

        public bool IsOutOfBounds(int id) => id < 0 || id >= data.Length; 

        public bool IsOutOfBounds(int x, int y) => y < 0 || y >= h || x < 0 || x >= w;

        public T[] Neighbours(int x, int y){
            var neighbours = new T[4];
            neighbours[0] = this[x+1,y]; 
            neighbours[1] = this[x-1,y];
            neighbours[2] = this[x,y-1]; 
            neighbours[3] = this[x,y+1]; 
            return neighbours; 
        }

        public void Fill(Func<int,int, T> func){
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    this[x,y] = func.Invoke(x,y);
                }
            }
        }

        public void Fill(int x, int y, int w, int h, Func<int,int,T> fn){
            for (int oy = y; oy < y+h; oy++)
            {
                for (int ox = x; ox < x+w; ox++)
                {
                    this[ox,oy] = fn.Invoke(ox,oy);
                }
            }
        }

        public Point Coords(int id) => new Point(id%w, id/ w);

        public void ForEachXY(Action<int, int, T> func){
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    func.Invoke(x,y,this[x,y]);
                }
            }
        }

        public Grid<T> SubGrid(int x, int y, int w, int h){
            var grid = new Grid<T>(w,h,@default,seed);
            for (int oy = y; oy < y+h; oy++)
            {
                for (int ox = x; ox < x+w; ox++)
                {
                    grid[ox-x, oy-y] = this[x,y];
                }
            }
            return grid;
        }

        public void PutGrid(Grid<T> grid, int x, int y){
            grid.ForEachXY((ox,oy,v) => this[x+ox,y+oy] = v);
        }

        public int Id(int x, int y){
            return y * w + x;
        }

        //public IEnumerator<T> GetEnumerator() => data.GetEnumerator<T>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }


}