using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Gltf;
using XPlat.Graphics;
using Attribute = XPlat.Graphics.Attribute;

public class LightsApp : ISdlApp
{
    // blinn: https://learnopengl.com/code_viewer_gh.php?code=src/5.advanced_lighting/1.advanced_lighting/1.advanced_lighting.fs
    // lights: https://learnopengl.com/code_viewer_gh.php?code=src/2.lighting/6.multiple_lights/multiple_lights.cpp
    // range: https://gamedev.stackexchange.com/questions/56897/glsl-light-attenuation-color-and-intensity-formula
    // gltf: https://github.com/KhronosGroup/glTF/blob/main/extensions/2.0/Khronos/KHR_lights_punctual/README.md
    private readonly IPlatform platform;

    private Shader shader;
    private Primitive primitive;
    private PhongMaterial texture;
    private Camera3d camera;
    private Transform3d transform;
    private PointLight light;

    public LightsApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.shader = new PhongShader();

        var gltf = GltfReader.Load("assets/models/test_scene.glb");
        var node = gltf.FindNode("Suzanne");
        primitive = node.ReadMesh().Primitives.First();

        this.camera = new Camera3d {
            Positon = new Vector3(0,2,-5),
            Target = new Vector3(0,0,0)
        };
        this.transform = new Transform3d();
        this.light = new PointLight {
            Position = new Vector3(0,2,-2),
            //Range = 10,
            Intensity = 1
        };
    }

    public void Update()
    {
        GL.Enable(GL.DEPTH_TEST);
        GL.Enable(GL.CULL_FACE);

        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

        transform.RotationQuat = Quaternion.CreateFromYawPitchRoll(Time.RunningTime, 0, 0);

        Matrix4x4 model = transform.GetMatrix();
        Matrix4x4 normal = transform.GetNormalMatrix();
        Shader.Use(shader);
        shader.SetUniform(Uniform.ModelMatrix, ref model);
        shader.SetUniform(Uniform.NormalMatrix, ref normal);

        camera.Ratio = platform.WindowSize.X / platform.WindowSize.Y;
        camera.ApplyToShader(shader);
        light.ApplyToShader(shader, LightId.Light_0);
        primitive.DrawWithShader(shader);
    }
}
