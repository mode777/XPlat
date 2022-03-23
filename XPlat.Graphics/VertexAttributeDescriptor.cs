namespace XPlat.Graphics
{
    public class VertexAttributeDescriptor
    {
        public static readonly VertexAttributeDescriptor Float = new VertexAttributeDescriptor(1, GLES2.GL.FLOAT);
        public static readonly VertexAttributeDescriptor Vec2f = new VertexAttributeDescriptor(2, GLES2.GL.FLOAT);
        public static readonly VertexAttributeDescriptor Vec3f = new VertexAttributeDescriptor(3, GLES2.GL.FLOAT);
        public static readonly VertexAttributeDescriptor Vec4f = new VertexAttributeDescriptor(4, GLES2.GL.FLOAT);
        public static readonly VertexAttributeDescriptor Color = new VertexAttributeDescriptor(4, GLES2.GL.UNSIGNED_BYTE);
        public uint VertexSize { get; }
        public int Offset { get; }
        public bool Normalized { get; }
        public int Components { get; set; }
        public uint Type { get; set; }
        public VertexAttributeDescriptor(int components, uint type, uint vertexSize = 0, int offset = 0, bool normalized = false)
        {
            this.Normalized = normalized;
            this.Offset = offset;
            this.VertexSize = vertexSize;
            this.Type = type;
            this.Components = components;
        }
    }
}