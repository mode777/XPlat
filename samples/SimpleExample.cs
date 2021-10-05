using GLES2;

namespace net6test.samples
{
    public class SimpleExample : ISdlApp
    {
        string vsource = @"attribute vec4 a_Position;
        void main() {
            gl_Position = a_Position;
        }";

        string fsource = @"void main() {
            gl_FragColor = vec4(0.0, 1.0, 0.0, 1.0);
        }";

        ushort[] indices = new ushort[] { 3,2,1,3,1,0 };

        float[] positions = new float[] { 
        -1,  1, 
        -1, -1,
        1, -1,
        1,  1
        };
        private uint program;
        private uint vbuffer;
        private uint ibuffer;
        private int location;

        public void Init()
        {
            program = GlUtil.CreateProgram(vsource,fsource);
            vbuffer = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, positions);
            ibuffer = GlUtil.CreateBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, indices);
            location = GL.glGetAttribLocation(program, "a_Position");
        }

        public void Update()
        {
            GL.glClearColor(1,0,0,1);
            GL.glClear(GL.GL_COLOR_BUFFER_BIT);

            GL.glUseProgram(program);
            
            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, vbuffer);
            GL.glVertexAttribPointer((uint)location, 2, GL.GL_FLOAT, false, 0, IntPtr.Zero);
            GL.glEnableVertexAttribArray((uint)location);
            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, ibuffer);
            GL.glDrawElements(GL.GL_TRIANGLES, 6, GL.GL_UNSIGNED_SHORT, IntPtr.Zero);
        }
    }
}