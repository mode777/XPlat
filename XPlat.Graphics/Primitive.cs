namespace XPlat.Graphics
{
    public class Primitive 
    {
        private readonly VertexAttribute[] attributes;
        private readonly VertexIndices indices;

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
    }
}