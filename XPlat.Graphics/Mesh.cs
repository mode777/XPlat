using System.Collections.Generic;

namespace XPlat.Graphics
{
    public class Mesh 
    {
        private readonly Primitive[] _primitives;
        public IEnumerable<Primitive> Primitives => _primitives;

        public Mesh(params Primitive[] primitives)
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

