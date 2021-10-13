using GLES2;

namespace net6test
{
    public class VertexAttribute 
    {
        private readonly StandardAttribute _type;
        private readonly uint _glBuffer;
        private readonly VertexAttributeDescriptor _descriptor;

        public VertexAttribute(StandardAttribute type, uint glBuffer, VertexAttributeDescriptor descriptor){
            this._type = type;
            this._glBuffer = glBuffer;
            this._descriptor = descriptor;
        }

        public void EnableOnShader(Shader shader){
            if(!shader.HasAttribute(_type)) return;
            GL.BindBuffer(GL.ARRAY_BUFFER, _glBuffer);
            shader.EnableAttribute(_type, _descriptor);
        }

    }

    public class VertexAttribute<T> : VertexAttribute where T : unmanaged
    {
        private readonly T[] _data;

        public VertexAttribute(StandardAttribute type, T[] data, VertexAttributeDescriptor descriptor) 
            : base(type, GlUtil.CreateBuffer(GL.ARRAY_BUFFER, data), descriptor)
        {
            this._data = data;
        }
    }
}