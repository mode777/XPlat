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
                GL.UseProgram(shader.Handle);
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
        private readonly Dictionary<StandardAttribute, ShaderProperty> _standardAttributes = new();
        private readonly Dictionary<StandardUniform, ShaderProperty> _standardUniforms = new();


        public Shader(string vertex_source, string fragment_source, Dictionary<StandardAttribute, string> attributes = null, Dictionary<StandardUniform, string> uniforms = null)
        {
            Handle = GlUtil.CreateProgram(vertex_source, fragment_source);
            CollectInfo();
            if(attributes != null) {
                foreach (var kv in attributes)
                {
                    _standardAttributes.Add(kv.Key, _attributes[kv.Value]);
                }
            }
            if(uniforms != null) {
                foreach (var kv in uniforms)
                {
                    _standardUniforms.Add(kv.Key, _uniforms[kv.Value]);
                }
            }
        }

        public void SetUniform(int id, ref Matrix4x4 mat) => GlUtil.SendUniform(id, ref mat);
        public void SetUniform(string name, ref Matrix4x4 mat) => SetUniform(_uniforms[name].Id, ref mat);
        public void SetUniform(StandardUniform uniform, ref Matrix4x4 mat) => SetUniform(_standardUniforms[uniform].Id, ref mat);
        
        public void SetUniform(int id, Vector3 v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, Vector3 v) => SetUniform(_uniforms[name].Id, v);
        public void SetUniform(StandardUniform uniform, Vector3 v) => SetUniform(_standardUniforms[uniform].Id, v);

        public void SetUniform(int id, float v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, float v) => SetUniform(_uniforms[name].Id, v);
        public void SetUniform(StandardUniform uniform, float v) => SetUniform(_standardUniforms[uniform].Id, v);

        public bool HasAttribute(StandardAttribute type) => _standardAttributes.ContainsKey(type);
        public bool HasUniform(StandardUniform type) => _standardUniforms.ContainsKey(type);

        public void EnableAttribute(uint id, VertexAttributeDescriptor attr){
            GL.EnableVertexAttribArray(id);
            GL.VertexAttribPointer(id, attr.Components, attr.Type, attr.Normalized, attr.VertexSize, (IntPtr)attr.Offset);
        }
        public void EnableAttribute(string name, VertexAttributeDescriptor attr){
            uint id = (uint)_attributes[name].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(StandardAttribute type, VertexAttributeDescriptor attr){
             uint id = (uint)_standardAttributes[type].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(StandardAttribute type){
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
                case GL.FLOAT:
                    return VertexAttributeDescriptor.Float;
                case GL.FLOAT_VEC2:
                    return VertexAttributeDescriptor.Vec2f;
                case GL.FLOAT_VEC3:
                    return VertexAttributeDescriptor.Vec3f;
                case GL.FLOAT_VEC4:
                    return VertexAttributeDescriptor.Vec4f;
                default:
                    throw new Exception("Unable to derive attribute type automatically");
            }
        }

        private void CollectInfo(){
            var sb = new StringBuilder(128);
            uint length;

            int numAttributes;
            GL.GetProgramiv(Handle, GL.ACTIVE_ATTRIBUTES, out numAttributes);

            for (int i = 0; i < numAttributes; i++)
            {
                uint size, type;
                GL.GetActiveAttrib(Handle, (uint)i, (uint)sb.Capacity, out length, out size, out type, sb);
                _attributes.Add(sb.ToString(), new ShaderProperty{
                    Id = i,
                    Size = (int)size,
                    Type = type,
                    Name = sb.ToString()
                });
                sb.Clear();
            }

            int numUniforms;
            GL.GetProgramiv(Handle, GL.ACTIVE_UNIFORMS, out numUniforms);

            for (int i = 0; i < numUniforms; i++)
            {
                uint size, type;
                GL.GetActiveUniform(Handle, (uint)i, (uint)sb.Capacity, out length, out size, out type, sb);
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