using System;

namespace Gwen.Net.OpenTk.Shaders
{
    public interface IShader : IDisposable
    {
        int Program { get; }
        int VertexShader { get; }
        int FragmentShader { get; }

        UniformDictionary Uniforms { get; }
    }
}