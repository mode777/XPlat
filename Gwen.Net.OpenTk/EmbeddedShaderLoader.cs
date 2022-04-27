using System;
using System.IO;

namespace Gwen.Net.OpenTk
{
    internal static class EmbeddedShaderLoader
    {
        public static string GetShader<T>(string type)
        {
            var programType = typeof(T);
            string shaderName = $"{programType.FullName}.{type}";

            var stream = programType.Assembly.GetManifestResourceStream(shaderName);
            if (stream == null)
            {
                throw new Exception($"Resource '{shaderName}' not found");
            }

            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }

        /// <summary>
        /// Loads the shader source from an assembly. Where the <typeparamref name="TRoot"/> provides the root namespace to resolve the shader resource
        /// </summary>
        /// <typeparam name="TRoot"></typeparam>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetShader<TRoot>(string name, string type)
        {
            var programType = typeof(TRoot);
            string shaderName = $"{programType.Namespace}.{name}.{type}";

            var stream = programType.Assembly.GetManifestResourceStream(shaderName);
            if (stream == null)
            {
                throw new Exception($"Resource '{shaderName}' not found");
            }

            using StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}