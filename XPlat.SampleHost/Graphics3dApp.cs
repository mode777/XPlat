using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

public class Graphics3dApp : ISdlApp
{
    private readonly IPlatform platform;

    private Shader shader;
    private Primitive primitive;
    private PhongMaterial material;
    private Camera3d camera;
    private Transform3d transform;
    private PointLight light;

    public Graphics3dApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.shader = new PhongShader();

        this.primitive = new Primitive(new[]
        {
            new VertexAttribute<float>(XPlat.Graphics.Attribute.Position, positions, VertexAttributeDescriptor.Vec3f),
            new VertexAttribute<float>(XPlat.Graphics.Attribute.Normal, vertexNormals, VertexAttributeDescriptor.Vec3f),
            new VertexAttribute<float>(XPlat.Graphics.Attribute.Uv_0, uvs, VertexAttributeDescriptor.Vec2f),
        }, new VertexIndices(indices));

        this.material = new PhongMaterial(new Texture("assets/textures/bricks.jpeg"), Uniform.AlbedoTexture);
        material.Roughness = 0;
        material.Metallic = 0;

        this.primitive.Material = this.material;
        this.camera = new Camera3d {
            Positon = new Vector3(0,2,-5),
            Target = new Vector3(0,0,0)
        };
        this.transform = new Transform3d();
        this.light = new PointLight {
            Position = new Vector3(3,2,-2),
            Intensity = 5
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

    private readonly float[] uvs = {

            // Front
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Back
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Top
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Bottom
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Right
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,

            // Left
            0.0f,  0.0f,
            1.0f,  0.0f,
            1.0f,  1.0f,
            0.0f,  1.0f,
         };

    private readonly float[] vertexNormals = {
            // Front
            0.0f,  0.0f,  1.0f,
            0.0f,  0.0f,  1.0f,
            0.0f,  0.0f,  1.0f,
            0.0f,  0.0f,  1.0f,

            // Back
            0.0f,  0.0f, -1.0f,
            0.0f,  0.0f, -1.0f,
            0.0f,  0.0f, -1.0f,
            0.0f,  0.0f, -1.0f,

            // Top
            0.0f,  1.0f,  0.0f,
            0.0f,  1.0f,  0.0f,
            0.0f,  1.0f,  0.0f,
            0.0f,  1.0f,  0.0f,

            // Bottom
            0.0f, -1.0f,  0.0f,
            0.0f, -1.0f,  0.0f,
            0.0f, -1.0f,  0.0f,
            0.0f, -1.0f,  0.0f,

            // Right
            1.0f,  0.0f,  0.0f,
            1.0f,  0.0f,  0.0f,
            1.0f,  0.0f,  0.0f,
            1.0f,  0.0f,  0.0f,

            // Left
            -1.0f,  0.0f,  0.0f,
            -1.0f,  0.0f,  0.0f,
            -1.0f,  0.0f,  0.0f,
            -1.0f,  0.0f,  0.0f
         };

    private readonly float[] positions = {
            // Front face
            -1.0f, -1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,

            // Back face
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,

            // Top face
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f,  1.0f, -1.0f,

            // Bottom face
            -1.0f, -1.0f, -1.0f,
            1.0f, -1.0f, -1.0f,
            1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            // Right face
            1.0f, -1.0f, -1.0f,
            1.0f,  1.0f, -1.0f,
            1.0f,  1.0f,  1.0f,
            1.0f, -1.0f,  1.0f,

            // Left face
            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,
        };

    private readonly ushort[] indices = {
            0,  1,  2,      0,  2,  3,    // front
            4,  5,  6,      4,  6,  7,    // back
            8,  9,  10,     8,  10, 11,   // top
            12, 13, 14,     12, 14, 15,   // bottom
            16, 17, 18,     16, 18, 19,   // right
            20, 21, 22,     20, 22, 23,   // left
        };
}
