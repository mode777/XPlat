using System.Numerics;
using System.Text;
using GLES2;

namespace net6test
{
    public enum ShaderAttribute {
        Position = 1,
        Normal = 2,
        Color = 3
    }

    public class Shader
    {
        private class ShaderProperty {
            public int Size { get; set; }
            public uint Type { get; set; }
            public string Name { get; set; }
            public int Id { get; set; }
        }

        public class ShaderOptions {
            public string NormalAttribute { get; set; }
            public string PositionAttribute { get; set; }
            public string ColorAttribute { get; set; }
        }

        public uint Handle { get; }
        private readonly Dictionary<string, ShaderProperty> _uniforms = new();
        private readonly Dictionary<string, ShaderProperty> _attributes = new();
        private readonly Dictionary<ShaderAttribute, ShaderProperty> _standardAttributes = new();


        public Shader(string vertex_source, string fragment_source, ShaderOptions options = null)
        {
            Handle = GlUtil.CreateProgram(vertex_source, fragment_source);
            CollectInfo();
            if(options != null) {
                if(options.NormalAttribute != null) _standardAttributes.Add(ShaderAttribute.Normal, _attributes[options.NormalAttribute]); 
                if(options.ColorAttribute != null) _standardAttributes.Add(ShaderAttribute.Color, _attributes[options.ColorAttribute]); 
                if(options.PositionAttribute != null) _standardAttributes.Add(ShaderAttribute.Position, _attributes[options.PositionAttribute]); 
            }
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

        public void EnableAttribute(ShaderAttribute type, VertexAttribute attr){
             uint id = (uint)_standardAttributes[type].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(ShaderAttribute type){
            var attr = _standardAttributes[type];
            EnableAttribute((uint)attr.Id, ResolveVertexAttribByType(attr.Type));
        }

        public void EnableAttribute(string name){
            var attr = _attributes[name];           
            EnableAttribute((uint)attr.Id, ResolveVertexAttribByType(attr.Type));
        }

        private VertexAttribute ResolveVertexAttribByType(uint type){
            switch (type)
            {
                case GL.GL_FLOAT:
                    return VertexAttribute.Float;
                case GL.GL_FLOAT_VEC2:
                    return VertexAttribute.Vec2f;
                case GL.GL_FLOAT_VEC3:
                    return VertexAttribute.Vec3f;
                case GL.GL_FLOAT_VEC4:
                    return VertexAttribute.Vec4f;
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