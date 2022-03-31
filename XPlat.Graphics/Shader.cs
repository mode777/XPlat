using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GLES2;
using XPlat.Core;

namespace XPlat.Graphics
{

    public class Shader : IDisposable
    {
        public static void Use(Shader shader) 
        {
            if(Current != shader)
            {
                GL.UseProgram(shader.GlProgram.Handle);
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

        public GlProgramHandle GlProgram { get; }
        private readonly Dictionary<string, ShaderProperty> _uniforms = new();
        private readonly Dictionary<string, ShaderProperty> _attributes = new();
        private readonly Dictionary<Attribute, ShaderProperty> _standardAttributes = new();
        private readonly Dictionary<Uniform, ShaderProperty> _standardUniforms = new();
        private bool disposedValue;

        public Shader(string vertex_source, string fragment_source, Dictionary<Attribute, string> attributes = null, Dictionary<Uniform, string> uniforms = null)
        {
            GlProgram = GlUtil.CreateProgram(vertex_source, fragment_source);
            CollectInfo();
            if(attributes != null) {
                foreach (var kv in attributes)
                {
                    if(_attributes.ContainsKey(kv.Value))
                        _standardAttributes.Add(kv.Key, _attributes[kv.Value]);
                }
            }
            if(uniforms != null) {
                foreach (var kv in uniforms)
                {
                    if(_uniforms.ContainsKey(kv.Value))
                            _standardUniforms.Add(kv.Key, _uniforms[kv.Value]);
                }
            }
        }

        public void SetUniform(int id, ref Matrix4x4 mat) => GlUtil.SendUniform(id, ref mat);
        public void SetUniform(string name, ref Matrix4x4 mat) => SetUniform(GetUniformByName(name), ref mat);
        public void SetUniform(Uniform uniform, ref Matrix4x4 mat) {
            if(_standardUniforms.TryGetValue(uniform, out var u)) SetUniform(u.Id, ref mat);
        }

        public void SetUniform(int id, Vector2 v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, Vector2 v) => SetUniform(GetUniformByName(name), v);
        public void SetUniform(Uniform uniform, Vector2 v) {
            if(_standardUniforms.TryGetValue(uniform, out var u)) SetUniform(u.Id, v);
        }

        public void SetUniform(int id, Vector3 v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, Vector3 v) => SetUniform(GetUniformByName(name), v);
        public void SetUniform(Uniform uniform, Vector3 v) {
            if(_standardUniforms.TryGetValue(uniform, out var u)) SetUniform(u.Id, v);
        }

        public void SetUniform(int id, Vector4 v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, Vector4 v) => SetUniform(GetUniformByName(name), v);
        public void SetUniform(Uniform uniform, Vector4 v) {
            if(_standardUniforms.TryGetValue(uniform, out var u)) SetUniform(u.Id, v);
        }

        //public void SetUniform(int id, NVGcolor v) => GlUtil.SendUniform(id, v);
        //public void SetUniform(string name, NVGcolor v) => SetUniform(GetUniformByName(name), v);
        //public void SetUniform(StandardUniform uniform, NVGcolor v) => SetUniform(_standardUniforms[uniform].Id, v);

        public void SetUniform(int id, int v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, int v) => SetUniform(GetUniformByName(name), v);
        public void SetUniform(Uniform uniform, int v) {
            if(_standardUniforms.TryGetValue(uniform, out var u)) SetUniform(u.Id, v);
        }

        public void SetUniform(int id, float v) => GlUtil.SendUniform(id, v);
        public void SetUniform(string name, float v) => SetUniform(GetUniformByName(name), v);
        public void SetUniform(Uniform uniform, float v) {
            if(_standardUniforms.TryGetValue(uniform, out var u)) SetUniform(u.Id, v);
        }

        public bool HasAttribute(Attribute type) => _standardAttributes.ContainsKey(type);
        public bool HasUniform(Uniform type) => _standardUniforms.ContainsKey(type);

        public void EnableAttribute(uint id, VertexAttributeDescriptor attr){
            GL.EnableVertexAttribArray(id);
            GL.VertexAttribPointer(id, attr.Components, attr.Type, attr.Normalized, attr.VertexSize, (IntPtr)attr.Offset);
        }
        
        public void EnableAttribute(string name, VertexAttributeDescriptor attr){
            uint id = (uint)_attributes[name].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(Attribute type, VertexAttributeDescriptor attr){
             uint id = (uint)_standardAttributes[type].Id;
            EnableAttribute(id, attr);
        }

        public void EnableAttribute(Attribute type){
            var attr = _standardAttributes[type];
            EnableAttribute((uint)attr.Id, ResolveVertexAttribByType(attr.Type));
        }

        public void EnableAttribute(string name){
            var attr = _attributes[name];           
            EnableAttribute((uint)attr.Id, ResolveVertexAttribByType(attr.Type));
        }

        private int GetUniformByName(string name) => _uniforms.GetValueOrDefault(name)?.Id ?? -1;
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
            GL.GetProgramiv(GlProgram.Handle, GL.ACTIVE_ATTRIBUTES, out numAttributes);

            for (int i = 0; i < numAttributes; i++)
            {
                uint size, type;
                GL.GetActiveAttrib(GlProgram.Handle, (uint)i, (uint)sb.Capacity, out length, out size, out type, sb);
                _attributes.Add(sb.ToString(), new ShaderProperty{
                    Id = i,
                    Size = (int)size,
                    Type = type,
                    Name = sb.ToString()
                });
                sb.Clear();
            }

            int numUniforms;
            GL.GetProgramiv(GlProgram.Handle, GL.ACTIVE_UNIFORMS, out numUniforms);

            for (int i = 0; i < numUniforms; i++)
            {
                uint size, type;
                GL.GetActiveUniform(GlProgram.Handle, (uint)i, (uint)sb.Capacity, out length, out size, out type, sb);
                _uniforms.Add(sb.ToString(), new ShaderProperty{
                    Id = i,
                    Size = (int)size,
                    Type = type,
                    Name = sb.ToString()
                });
                sb.Clear();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GlProgram.Dispose();
                }
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Shader()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}