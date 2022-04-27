namespace Gwen.Net.OpenTk.Shaders
{
    public interface IShaderLoader
    {
        IShader Load(string shaderName);

        IShader Load(string vertexShaderName, string fragmentShaderName);
    }
}