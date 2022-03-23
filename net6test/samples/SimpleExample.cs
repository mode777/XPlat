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
            vbuffer = GlUtil.CreateBuffer(GL.ARRAY_BUFFER, positions);
            ibuffer = GlUtil.CreateBuffer(GL.ELEMENT_ARRAY_BUFFER, indices);
            location = GL.GetAttribLocation(program, "a_Position");
        }

        public void Update()
        {
            GL.ClearColor(1,0,0,1);
            GL.Clear(GL.COLOR_BUFFER_BIT);

            GL.UseProgram(program);
            
            GL.BindBuffer(GL.ARRAY_BUFFER, vbuffer);
            GL.VertexAttribPointer((uint)location, 2, GL.FLOAT, false, 0, IntPtr.Zero);
            GL.EnableVertexAttribArray((uint)location);
            GL.BindBuffer(GL.ELEMENT_ARRAY_BUFFER, ibuffer);
            GL.DrawElements(GL.TRIANGLES, 6, GL.UNSIGNED_SHORT, IntPtr.Zero);
        }
    }
}