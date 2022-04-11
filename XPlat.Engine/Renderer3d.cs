using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Engine.Components;
using XPlat.Graphics;

namespace XPlat.Engine
{

    public class Renderer3d
    {
        private readonly Shader shader;
        private readonly IPlatform platform;
        private LightId lightId;

        public Renderer3d(IPlatform platform)
        {
            this.shader = new PhongShader();
            this.platform = platform;
        }

        public void Render(Scene scene){
            lightId = LightId.Light_0;

            GL.UseProgram(shader.GlProgram.Handle);

            GL.Enable(GL.DEPTH_TEST);
            GL.Enable(GL.CULL_FACE);

            GL.ClearColor(1, 0, 0, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            var model = Matrix4x4.Identity;
            Shader.Use(shader);

            Visit(scene.RootNode, ref model);
        }

        private void Visit(Node node, ref Matrix4x4 model){

            var transform = node.Transform;
            var currentModel = transform.GetMatrix() * model;

            foreach(var c in node.Components){
                switch(c){
                    case MeshComponent mesh:
                        if(mesh.Mesh != null) RenderMesh(ref currentModel, mesh.Mesh);
                        break;
                    case LightComponent light:
                        if(light.Light != null) light.Light.ApplyToShader(shader, lightId, ref currentModel);
                        lightId = lightId.Offset(1);
                        break;
                    case CameraComponent cam:
                        if(cam.Camera != null) ApplyCamera(cam.Camera, ref currentModel);
                        break; 
                }
            }

            foreach (var n in node.Children){
                Visit(n, ref currentModel);
            }
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