using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Graphics;

namespace XPlat.Engine
{
    public class RenderPass3d : IRenderPass
    {
        private readonly Shader shader;
        private readonly IPlatform platform;
        private LightId lightId;

        public RenderPass3d(IPlatform platform)
        {
            this.shader = new PhongShader();
            this.platform = platform;
        }

        public void FinishFrame()
        {
        }

        public void OnAttach(Scene scene)
        {
        }

        public void OnRender(Node n)
        {
            foreach(var c in n.Components){
                switch(c){
                    case MeshComponent mesh:
                        if(mesh.Mesh != null) RenderMesh(ref n._globalMatrix, mesh.Mesh);
                        break;
                    case LightComponent light:
                        if(light.Light != null) light.Light.ApplyToShader(shader, lightId, ref n._globalMatrix);
                        lightId = lightId.Offset(1);
                        break;
                    case CameraComponent cam:
                        if(cam.Camera != null) ApplyCamera(cam.Camera, ref n._globalMatrix);
                        break; 
                }
            }
        }

        public void StartFrame()
        {
            lightId = LightId.Light_0;

            GL.UseProgram(shader.GlProgram.Handle);
            Shader.Use(shader);

            GL.Enable(GL.DEPTH_TEST);
            GL.Enable(GL.CULL_FACE);
        }

        private void ApplyCamera(Camera3d cam, ref Matrix4x4 model)
        {
            var transform = new Transform3d(model);
            cam.Ratio = platform.WindowSize.X / platform.WindowSize.Y;

            cam.ApplyToShader(shader, ref model);
        }

        private void RenderMesh(ref Matrix4x4 model, Mesh mesh)
        {
            // Make a copy and remove translation components
            // before creating the normal matrix
            Matrix4x4 copy = model;
            copy.M14 = 0;
            copy.M24 = 0;
            copy.M34 = 0;
            copy.M41 = 0;
            copy.M42 = 0;
            copy.M43 = 0;
            Matrix4x4 inv;
            Matrix4x4.Invert(copy, out inv);
            copy = Matrix4x4.Transpose(inv);

            shader.SetUniform(Uniform.ModelMatrix, ref model);
            shader.SetUniform(Uniform.NormalMatrix, ref copy);
            mesh.DrawUsingShader(shader);
        }
    }
}