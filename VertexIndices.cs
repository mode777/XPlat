using GLES2;

namespace net6test
{
    public class VertexIndices 
    {
        private readonly ushort[] _data;
        private readonly uint _glBuffer;

        public VertexIndices(ushort[] data)
        {
            this._data = data;
            this._glBuffer = GlUtil.CreateBuffer(GL.ELEMENT_ARRAY_BUFFER, data);
        }

        public void DrawWithShader(Shader shader){
            GL.BindBuffer(GL.ELEMENT_ARRAY_BUFFER, _glBuffer);
            GL.DrawElements(GL.TRIANGLES, (uint)_data.Length, GL.UNSIGNED_SHORT, IntPtr.Zero);
        }
    }
}