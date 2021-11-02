namespace net6test
{
    public class RendererComponent : Component
    {
        public Mesh? Mesh { get; set; }
        //public Shader? shader { get; set; }
        public void Draw(){
            var shader = Shader.Current;
            if(Mesh != null && shader != null)
            {
                Shader.Use(shader);
                var mat = Node.Transform.GetMatrix();
                shader.SetUniform(StandardUniform.ModelMatrix, ref mat);
                Mesh.DrawUsingShader(shader);
            }
        }
    }
}

