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
            this._glBuffer = GlUtil.CreateBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, data);
        }

        public void DrawWithShader(Shader shader){
            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, _glBuffer);
            GL.glDrawElements(GL.GL_TRIANGLES, (uint)_data.Length, GL.GL_UNSIGNED_SHORT, IntPtr.Zero);
        }
    }
}