using System.Numerics;
using System.Text;
using GLES2;

namespace net6test
{
    public class Shader
    {
        public class ShaderProperty {
            public int Size { get; set; }
            public uint Type { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public uint Handle { get; }
        private readonly Dictionary<string, ShaderProperty> _uniforms = new();
        private readonly Dictionary<string, ShaderProperty> _attributes = new();
        

        public Shader(string vertex_source, string fragment_source)
        {
            Handle = GlUtil.CreateProgram(vertex_source, fragment_source);
            CollectInfo();
        }

        public void Use(){
            GL.glUseProgram(Handle);
        }

        public void SetUniform(string name, ref Matrix4x4 mat){
            GlUtil.SendUniform(_uniforms[name].Id, ref mat);
        }

        public void SetUniform(string name, Vector3 v){
            GlUtil.SendUniform(_uniforms[name].Id, v);
        }

        public void EnableAttribute(uint id, VertexAttribute attr){
            GL.glEnableVertexAttribArray(id);
            GL.glVertexAttribPointer(id, attr.Components, attr.Type, attr.Normalized, attr.VertexSize, (IntPtr)attr.Offset);
        }
        public void EnableAttribute(string name, VertexAttribute attr){
            uint id = (uint)_attributes[name].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(string name){
            var attr = _attributes[name];
            VertexAttribute vertexAttribute;
            switch (attr.Type)
            {
                case GL.GL_FLOAT:
                    vertexAttribute = VertexAttribute.Float;
                    break;
                case GL.GL_FLOAT_VEC2:
                    vertexAttribute = VertexAttribute.Vec2f;
                    break;
                case GL.GL_FLOAT_VEC3:
                    vertexAttribute = VertexAttribute.Vec3f;
                    break;
                case GL.GL_FLOAT_VEC4:
                    vertexAttribute = VertexAttribute.Vec4f;
                    break;
                default:
                    throw new Exception("Unable to derive attribute type automatically");
            }
            EnableAttribute((uint)attr.Id, vertexAttribute);
        }

        private void CollectInfo(){
            var sb = new StringBuilder(128);
            uint length;

            int numAttributes;
            GL.glGetProgramiv(Handle, GL.GL_ACTIVE_ATTRIBUTES, out numAttributes);

            for (int i = 0; i < numAttributes; i++)
            {
                uint size, type;
                GL.glGetActiveAttrib(Handle, (uint)i, (uint)sb.Capacity, out length, out size, out type, sb);
                _attributes.Add(sb.ToString(), new ShaderProperty{
                    Id = i,
                    Size = (int)size,
                    Type = type,
                    Name = sb.ToString()
                });
                sb.Clear();
            }

            int numUniforms;
            GL.glGetProgramiv(Handle, GL.GL_ACTIVE_UNIFORMS, out numUniforms);

            for (int i = 0; i < numUniforms; i++)
            {
                uint size, type;
                GL.glGetActiveUniform(Handle, (uint)i, (uint)sb.Capacity, out length, out size, out type, sb);
                _uniforms.Add(sb.ToString(), new ShaderProperty{
                    Id = i,
                    Size = (int)size,
                    Type = type,
                    Name = sb.ToString()
                });
                sb.Clear();
            }
        }
    }
}