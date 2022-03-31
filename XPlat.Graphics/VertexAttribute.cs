using System;
using GLES2;
using XPlat.Core;

namespace XPlat.Graphics
{
    public class VertexAttribute
    {
        private readonly Attribute _type;
        private readonly GlBufferHandle _glBuffer;
        private readonly VertexAttributeDescriptor _descriptor;
        private bool disposedValue;

        public VertexAttribute(Attribute type, GlBufferHandle glBuffer, VertexAttributeDescriptor descriptor){
            this._type = type;
            this._glBuffer = glBuffer;
            this._descriptor = descriptor;
        }

        public void EnableOnShader(Shader shader){
            if(!shader.HasAttribute(_type)) return;
            GL.BindBuffer(GL.ARRAY_BUFFER, _glBuffer.Handle);
            shader.EnableAttribute(_type, _descriptor);
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