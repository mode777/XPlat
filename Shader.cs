using System.Numerics;
using System.Text;
using GLES2;

namespace net6test
{

    public class Shader
    {
        public static void Use(Shader shader) 
        {
            if(Current != shader)
            {
                GL.glUseProgram(shader.Handle);
                Current = shader;
            }
        }

        public static Shader Current { get; private set; }
    
        private class ShaderProperty {
            public int Size { get; set; }
            public uint Type { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public uint Handle { get; }
        private readonly Dictionary<string, ShaderProperty> _uniforms = new();
        private readonly Dictionary<string, ShaderProperty> _attributes = new();
        private readonly Dictionary<VertexAttributeType, ShaderProperty> _standardAttributes = new();


        public Shader(string vertex_source, string fragment_source, Dictionary<VertexAttributeType, string> attributes = null)
        {
            Handle = GlUtil.CreateProgram(vertex_source, fragment_source);
            CollectInfo();
            if(attributes != null) {
                foreach (var kv in attributes)
                {
                    _standardAttributes.Add(kv.Key, _attributes[kv.Value]);
                }
            }
        }

        public void SetUniform(string name, ref Matrix4x4 mat){
            GlUtil.SendUniform(_uniforms[name].Id, ref mat);
        }

        public void SetUniform(string name, Vector3 v){
            GlUtil.SendUniform(_uniforms[name].Id, v);
        }

        public bool HasAttribute(VertexAttributeType type){
            return _standardAttributes.ContainsKey(type);
        }

        public void EnableAttribute(uint id, VertexAttributeDescriptor attr){
            GL.glEnableVertexAttribArray(id);
            GL.glVertexAttribPointer(id, attr.Components, attr.Type, attr.Normalized, attr.VertexSize, (IntPtr)attr.Offset);
        }
        public void EnableAttribute(string name, VertexAttributeDescriptor attr){
            uint id = (uint)_attributes[name].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(VertexAttributeType type, VertexAttributeDescriptor attr){
             uint id = (uint)_standardAttributes[type].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(VertexAttributeType type){
            var attr = _standardAttributes[type];
            EnableAttribute((uint)attr.Id, ResolveVertexAttribByType(attr.Type));
        }

        public void EnableAttribute(string name){
            var attr = _attributes[name];           
            EnableAttribute((uint)attr.Id, ResolveVertexAttribByType(attr.Type));
        }

        private VertexAttributeDescriptor ResolveVertexAttribByType(uint type){
            switch (type)
            {
                case GL.GL_FLOAT:
                    return VertexAttributeDescriptor.Float;
                case GL.GL_FLOAT_VEC2:
                    return VertexAttributeDescriptor.Vec2f;
                case GL.GL_FLOAT_VEC3:
                    return VertexAttributeDescriptor.Vec3f;
                case GL.GL_FLOAT_VEC4:
                    return VertexAttributeDescriptor.Vec4f;
                default:
                    throw new Exception("Unable to derive attribute type automatically");
            }
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