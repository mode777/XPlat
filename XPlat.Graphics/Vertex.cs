using System.Numerics;
using System.Runtime.InteropServices;
using GLES2;
using XPlat.Core;

namespace XPlat.Graphics
{
    public struct Vertex
    {
        public static VertexAttribute[] CreateAttributes(GlBufferHandle buffer){
            unsafe {
                return new[] { 
                    new VertexAttribute(Graphics.Attribute.Position, buffer, new VertexAttributeDescriptor(3, GL.FLOAT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>(nameof(Position)))),
                    new VertexAttribute(Graphics.Attribute.Normal, buffer, new VertexAttributeDescriptor(3, GL.FLOAT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>(nameof(Normal)))),
                    new VertexAttribute(Graphics.Attribute.Uv_0, buffer, new VertexAttributeDescriptor(2, GL.FLOAT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>(nameof(Uv)))),
                    
                };
            }
        }
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Uv;
    }
}
