using System;

namespace XPlat.Graphics
{
    public class Primitive : IDisposable 
    {
        private readonly VertexAttribute[] attributes;
        private readonly VertexIndices indices;
        private bool disposedValue;

        public Material? Material { get; set; }

        public Primitive(VertexAttribute[] attributes, VertexIndices indices)
        {
            this.attributes = attributes;
            this.indices = indices;
        }

        public void DrawWithShader(Shader shader, int count = -1, int offset = -1){
            if (Shader.Current != shader) Shader.Use(shader);
            Material?.ApplyToShader(shader);
            foreach (var attr in attributes)
            {
                attr.EnableOnShader(shader);
            }
            indices.DrawWithShader(shader, count, offset);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}