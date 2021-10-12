namespace net6test
{
    public class RendererComponent : Component
    {
        public Mesh? Mesh { get; set; }
        public Shader? Shader { get; set; } = Shader.Current;
        public void Draw(){
            if(Mesh != null && Shader != null)
            {
                Shader.Use(Shader);
                var mat = Node.Transform.GetMatrix();
                Shader.SetUniform(StandardUniform.ModelMatrix, ref mat);
                Mesh.DrawUsingShader(Shader);
            }
        }
    }
}

