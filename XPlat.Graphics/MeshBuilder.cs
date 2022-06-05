using GLES2;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using XPlat.Core;

namespace XPlat.Graphics
{
    public class MeshBuilder
    {
        internal struct Vertex
        {
            public Vector3 Position;
            public Vector3 Normal;
            public Vector2 Uv;
        }

        private List<Vertex> _vertices = new();
        private List<ushort> _indices = new();

        private Vector3 _position;
        private Vector3 _normal;
        private Vector2 _uv;

        public int AddVertex()
        {
            _vertices.Add(new Vertex
            {
                Position = _position,
                Normal = _normal,
                Uv = _uv
            });
            return _vertices.Count - 1;
        }

        public void AddTriangle(int a, int b, int c){
            _indices.Add((ushort)a);
            _indices.Add((ushort)b);
            _indices.Add((ushort)c);
        }

        public void SetPosition(float x, float y, float z) => _position = new Vector3(x, y, z);
        public void SetPosition(Vector3 pos) => _position = pos;
        public void SetNormal(float x, float y, float z) => _normal = new Vector3(x, y, z);
        public void SetNormal(Vector3 norm) => _normal = norm;
        public void SetUv(float x, float y) => _uv = new Vector2(x, y);
        public void SetUv(Vector2 uv) => _uv = uv;
        
        public Mesh Build(uint usage = GL.STATIC_DRAW)
        {
            unsafe
            {
                var buffer = GlUtil.CreateBuffer(GL.ARRAY_BUFFER, _vertices.ToArray(), usage);
                return new Mesh(new Primitive(new[]{
                    new VertexAttribute(Attribute.Position, buffer,
                        new VertexAttributeDescriptor(3, GL.FLOAT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>("Position"))),
                    new VertexAttribute(Attribute.Normal, buffer,
                        new VertexAttributeDescriptor(3, GL.FLOAT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>("Normal"))),
                    new VertexAttribute(Attribute.Uv_0, buffer,
                        new VertexAttributeDescriptor(2, GL.FLOAT, (uint)sizeof(Vertex), (int)Marshal.OffsetOf<Vertex>("Uv")))
                }, new VertexIndices(_indices.ToArray())));
            }
        }
    }
}
