using System;
using System.Collections.Generic;

namespace XPlat.Graphics
{
    public class Mesh : IDisposable 
    {
        private readonly Primitive[] _primitives;
        private bool disposedValue;

        public IEnumerable<Primitive> Primitives => _primitives;

        public Mesh(params Primitive[] primitives)
        {
            this._primitives = primitives;
        }

        public void DrawUsingShader(Shader shader)
        {
            foreach (var prim in _primitives)
            {
                prim.DrawWithShader(shader);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var p in Primitives)
                    {
                        p.Dispose();
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

