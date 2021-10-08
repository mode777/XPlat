using GLES2;
using SharpGLTF.Schema2;
using System.Numerics;

namespace net6test.samples
{
    public class ShadedCube : ISdlApp
    {
        
        float r = 0;

        public Action LoadModel(){
            var model = ModelRoot.Load("assets/cube.glb");
            var prim = model.LogicalMeshes.First().Primitives.First();
            
            var indices = prim.GetIndexAccessor().AsIndicesArray().Select(x => (ushort)x).ToArray();
            var idxBuf = GlUtil.CreateBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, indices);
            
            var posAccessor = prim.GetVertexAccessor("POSITION");
            var posData = posAccessor.AsVector3Array().ToArray();
            var posBuf = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, posData);
            
            var normAccessor = prim.GetVertexAccessor("NORMAL");
            var normData = normAccessor.AsVector3Array().ToArray();
            var normBuf = GlUtil.CreateBuffer(GL.GL_ARRAY_BUFFER, normData);

            return () => {
                GL.glBindBuffer(GL.GL_ARRAY_BUFFER, posBuf);
                shader.EnableAttribute(VertexAttributeType.Position, VertexAttributeDescriptor.Vec3f);

                // GL.glBindBuffer(GL.GL_ARRAY_BUFFER, col);
                // shader.EnableAttribute(VertexAttributeType.Color);

                GL.glBindBuffer(GL.GL_ARRAY_BUFFER, normBuf);
                shader.EnableAttribute(VertexAttributeType.Normal);

                GL.glBindBuffer(GL.GL_ELEMENT_ARRAY_BUFFER, idxBuf);

                GL.glDrawElements(GL.GL_TRIANGLES, (uint)indices.Length, GL.GL_UNSIGNED_SHORT, IntPtr.Zero);
            };
        }

        // See: https://github.com/bonigarcia/webgl-examples/blob/master/lighting/relistic_shading.html
        public void Init()
        {
            shader = new Shader(vshader, fshader, new() { 
                [VertexAttributeType.Position] = "a_Position",
                [VertexAttributeType.Normal] = "a_Normal"
            });
            shader.Use();
            GL.glEnable(GL.GL_DEPTH_TEST);
            GL.glEnable(GL.GL_CULL_FACE);                                    
          
            drawAction = LoadModel();
        }

        public void Update()
        {
            GL.glClearColor(0.5f,0.5f,0.5f,1);
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

            drawAction();
        }

        string vshader = @"attribute vec3 a_Color;
    attribute vec3 a_Normal;
    attribute vec4 a_Position;

    uniform mat4 u_pMatrix;
    uniform mat4 u_vMatrix;
    uniform mat4 u_mvMatrix;

    //varying highp vec4 v_Color;
    varying highp vec3 v_Normal;

    void main(void) {
        gl_Position = u_pMatrix * u_vMatrix * u_mvMatrix * a_Position;
        v_Normal = normalize(a_Normal - vec3(gl_Position));
        //v_Color = vec4(a_Color, 1.0);
    }";
        string fshader = @"precision mediump float;

    uniform vec3 u_LightColor;
    uniform vec3 u_LightDirection;

    //varying highp vec4 v_Color;
    varying highp vec3 v_Normal;

    void main(void) {
        float nDotL = max(dot(u_LightDirection, v_Normal), 0.0);

        vec3 diffuse = u_LightColor/* * v_Color.rgb*/ * nDotL;
        gl_FragColor = vec4(diffuse, 1.0);
    }";
        private Shader shader;
        private Action drawAction;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
    }
}