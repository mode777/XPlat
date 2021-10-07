using GLES2;
using System.Numerics;
using glTFLoader;

namespace net6test.samples
{
    public class ShadedCube : ISdlApp
    {
        float r = 0;

        // See: https://github.com/bonigarcia/webgl-examples/blob/master/lighting/relistic_shading.html
        public void Init()
        {
            shader = new Shader(vshader, fshader);
            shader.Use();
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_CULL_FACE);
            
            var model = Interface.LoadModel("assets/cube.glb");
            
            pos = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, vertices);
            norm = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, normals);
            col = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, colors);
             
            idx = GlUtil.CreateBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, indices);
        }

        public void Update()
        {
            GL.glClearColor(0,0,0,1);
            GL.glClear(GL.GL_COLOR_BUFFER_BIT | GL.GL_DEPTH_BUFFER_BIT);

            shader.Use();

            matP = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 2, 1, 0.1f, 100);
            shader.SetUniform("u_pMatrix", ref matP);

            matV = Matrix4x4.CreateLookAt(new Vector3(0,0,-3),new Vector3(0,0,0), new Vector3(0,1,0));
            shader.SetUniform("u_vMatrix", ref matV);

            matM = Matrix4x4.CreateRotationY(r+=0.1f);
            shader.SetUniform("u_mvMatrix", ref matM);

            var lightDirection = new Vector3(8, -8, -8);
            lightDirection = Vector3.Normalize(lightDirection);
            shader.SetUniform("u_LightDirection", lightDirection);
 
            var lightColor = new Vector3(1,1,1);
            shader.SetUniform("u_LightColor", lightColor);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, pos);
            shader.EnableAttribute("a_Position", VertexAttribute.Vec3f);

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, col);
            shader.EnableAttribute("a_Color");

            GL.glBindBuffer(GL.GL_ARRAY_BUFFER, norm);
            shader.EnableAttribute("a_Normal");

            GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, idx);

            GL.glDrawElements(GL.GL_TRIANGLES, 6*2*3, GL.GL_UNSIGNED_BYTE, IntPtr.Zero);
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
      1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, // v0-v1-v2-v3 front
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
        private Shader shader;
        private uint pos;
        private uint norm;
        private uint col;
        private uint idx;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
    }
}