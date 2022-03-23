namespace net6test
{
    public class Mesh 
    {
        private readonly Primitive[] _primitives;

        public Mesh(Primitive[] primitives)
        {
            this._primitives = primitives;
        }

        public void DrawUsingShader(Shader shader)
        {
            foreach (var prim in _primitives)
            {
                prim.DrawWithShader(shader);
            }
        }
    }
}

