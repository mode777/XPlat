using System;
using GLES2;
using XPlat.Core;

namespace XPlat.Graphics
{
    public class VertexIndices : IDisposable
    {
        private readonly ushort[] _data;
        private readonly uint _glBuffer;
        private bool disposedValue;

        public uint ElementsCount { get; set; }

        public VertexIndices(ushort[] data)
        {
            this._data = data;
            this._glBuffer = GlUtil.CreateBuffer(GL.ELEMENT_ARRAY_BUFFER, data);
            ElementsCount = (uint)_data.Length;
        }

        public void DrawWithShader(Shader shader, int count = -1, int offset = -1){
            GL.BindBuffer(GL.ELEMENT_ARRAY_BUFFER, _glBuffer);
            GL.DrawElements(GL.TRIANGLES, count == -1 ? ElementsCount : (uint)count, GL.UNSIGNED_SHORT, offset == -1 ? IntPtr.Zero : (IntPtr)offset);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                UnmanagedQueue.DeleteBuffers.Enqueue(_glBuffer);
                //GlUtil.DeleteBuffer(_glBuffer);
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~VertexIndices()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}