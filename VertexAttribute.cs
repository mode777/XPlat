namespace net6test
{
    public class VertexAttribute
    {
        public static readonly VertexAttribute Float = new VertexAttribute(1, GLES2.GL.GL_FLOAT);
        public static readonly VertexAttribute Vec2f = new VertexAttribute(2, GLES2.GL.GL_FLOAT);
        public static readonly VertexAttribute Vec3f = new VertexAttribute(3, GLES2.GL.GL_FLOAT);
        public static readonly VertexAttribute Vec4f = new VertexAttribute(4, GLES2.GL.GL_FLOAT);
        public static readonly VertexAttribute Color = new VertexAttribute(4, GLES2.GL.GL_UNSIGNED_BYTE);
        public uint VertexSize { get; }
        public int Offset { get; }
        public bool Normalized { get; }

        public int Components { get; set; }
        public uint Type { get; set; }
        public VertexAttribute(int components, uint type, uint vertexSize = 0, int offset = 0, bool normalized = false)
        {
            this.Normalized = normalized;
            this.Offset = offset;
            this.VertexSize = vertexSize;
            this.Type = type;
            this.Components = components;

        }
    }
}