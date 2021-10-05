using GLES2;

namespace net6test.samples
{
    public class ShadedCube : ISdlApp
    {
        // See: https://github.com/bonigarcia/webgl-examples/blob/master/lighting/relistic_shading.html
        public void Init()
        {
            programm = GlUtil.CreateProgram(vshader, fshader);
            
            pos = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, vertices);
            norm = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, normals);
            col = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, colors);
            
            posA = GL.glGetAttribLocation(programm, "a_Position");
            posN = GL.glGetAttribLocation(programm, "a_Normal");
            posC = GL.glGetAttribLocation(programm, "a_Color");
            
            idx = GlUtil.CreateBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, indices);
        }

        public void Update()
        {
            GL.glClearColor(0,0,0,1);
            GL.glClear(GL.GL_COLOR_BUFFER_BIT);

            GL.glUseProgram(programm);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, pos);
            GL.glEnableVertexAttribArray((uint)posA);
            GL.glVertexAttribPointer((uint)posA, 3, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, col);
            GL.glEnableVertexAttribArray((uint)posC);
            GL.glVertexAttribPointer((uint)posC, 3, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, norm);
            GL.glEnableVertexAttribArray((uint)posN);
            GL.glVertexAttribPointer((uint)posN, 3, GL.GL_FLOAT, false, 0, IntPtr.Zero);

            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, idx);

        }

        string vshader = @"attribute vec3 a_Color;
    attribute vec3 a_Normal;
    attribute vec4 a_Position;

    uniform mat4 u_pMatrix;
    uniform mat4 u_vMatrix;
    uniform mat4 u_mvMatrix;

    varying highp vec4 v_Color;
    varying highp vec3 v_Normal;

    void main(void) {
        gl_Position = u_pMatrix * u_vMatrix * u_mvMatrix * a_Position;
        v_Normal = normalize(a_Normal - vec3(gl_Position));
        v_Color = vec4(a_Color, 1.0);
    }";
        string fshader = @"precision mediump float;

    uniform vec3 u_LightColor;
    uniform vec3 u_LightDirection;

    varying highp vec4 v_Color;
    varying highp vec3 v_Normal;

    void main(void) {
        float nDotL = max(dot(u_LightDirection, v_Normal), 0.0);

        vec3 diffuse = u_LightColor * v_Color.rgb * nDotL;
        gl_FragColor = vec4(diffuse, v_Color.a);
    }";

        float[] vertices = new float[]{ // Coordinates
      0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f, // v0-v1-v2-v3 front
      0.5f, 0.5f, 0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, -0.5f, 0.5f, 0.5f, -0.5f, // v0-v3-v4-v5 right
      0.5f, 0.5f, 0.5f, 0.5f, 0.5f, -0.5f, -0.5f, 0.5f, -0.5f, -0.5f, 0.5f, 0.5f, // v0-v5-v6-v1 up
      -0.5f, 0.5f, 0.5f, -0.5f, 0.5f, -0.5f, -0.5f, -0.5f, -0.5f, -0.5f, -0.5f, 0.5f, // v1-v6-v7-v2 left
      -0.5f, -0.5f, -0.5f, 0.5f, -0.5f, -0.5f, 0.5f, -0.5f, 0.5f, -0.5f, -0.5f, 0.5f, // v7-v4-v3-v2 down
      0.5f, -0.5f, -0.5f, -0.5f, -0.5f, -0.5f, -0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f // v4-v7-v6-v5 back
    };

        float[] colors = new float[]{ // Colors
      1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, // v0-v1-v2-v3 front
      1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, // v0-v3-v4-v5 right
      1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, // v0-v5-v6-v1 up
      1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, // v1-v6-v7-v2 left
      1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, // v7-v4-v3-v2 down
      1, 0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0 // v4-v7-v6-v5 back
      };

        float[] normals = new float[]{ // Normal
      0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, // v0-v1-v2-v3 front
      1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, // v0-v3-v4-v5 right
      0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f, // v0-v5-v6-v1 up
      -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, // v1-v6-v7-v2 left
      0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, // v7-v4-v3-v2 down
      0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f, -1.0f // v4-v7-v6-v5 back
      };

        byte[] indices = new byte[]{ 0, 1, 2, 0, 2, 3, // front
      4, 5, 6, 4, 6, 7, // right
      8, 9, 10, 8, 10, 11, // up
      12, 13, 14, 12, 14, 15, // left
      16, 17, 18, 16, 18, 19, // down
      20, 21, 22, 20, 22, 23 // back
      };
        private uint programm;
        private uint pos;
        private uint norm;
        private uint col;
        private uint idx;
        private int posA;
        private int posN;
        private int posC;
    }
}