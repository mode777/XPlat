using System;
using GLES2;
using XPlat.Core;

namespace XPlat.Graphics
{
    public class VertexAttribute : IDisposable
    {
        private readonly Attribute _type;
        private readonly uint _glBuffer;
        private readonly VertexAttributeDescriptor _descriptor;
        private bool disposedValue;

        public VertexAttribute(Attribute type, uint glBuffer, VertexAttributeDescriptor descriptor){
            this._type = type;
            this._glBuffer = glBuffer;
            this._descriptor = descriptor;
        }

        public void EnableOnShader(Shader shader){
            if(!shader.HasAttribute(_type)) return;
            GL.BindBuffer(GL.ARRAY_BUFFER, _glBuffer);
            shader.EnableAttribute(_type, _descriptor);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                UnmanagedQueue.DeleteBuffers.Enqueue(_glBuffer);
                //GlUtil.DeleteBuffer(_glBuffer);
                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        ~VertexAttribute()
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

    public class VertexAttribute<T> : VertexAttribute where T : unmanaged
    {
        private readonly T[] _data;

        public VertexAttribute(Attribute type, T[] data, VertexAttributeDescriptor descriptor) 
            : base(type, GlUtil.CreateBuffer(GL.ARRAY_BUFFER, data), descriptor)
        {
            this._data = data;
        }
    }
}