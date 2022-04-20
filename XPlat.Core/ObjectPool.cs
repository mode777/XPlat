using System;
using System.Collections.Generic;
using System.Text;

namespace XPlat.Core
{
    public class ObjectPool<T> where T : class, new()
    {
        Queue<T> free = new Queue<T>();
        Queue<T> used = new Queue<T>();


        public T Get()
        {
            T t = null;
            if(free.TryDequeue(out var ft))
            {
                t = ft;
            }
            else t = new T();
            used.Enqueue(t);
            return t;
        }

        public void FreeAll()
        {
            var t = free;
            free = used;
            used = t;
        }
    }
}
