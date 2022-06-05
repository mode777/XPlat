using System.Drawing;
using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

public struct Triangle {
    Vector3 A;
    Vector3 B;
    Vector3 C;
    Vector2 UvA;
    Vector2 UvB;
    Vector2 UvC;
    Vector3 Normal => Vector3.Normalize(Vector3.Cross(B - A, C - A));
    public void Write(MeshBuilder builder){
        builder.SetNormal(Normal);
        builder.SetPosition(A);
        builder.SetUv(UvA);
        var a = builder.AddVertex();
        builder.SetPosition(B);
        builder.SetUv(UvB);
        var b = builder.AddVertex();
        builder.SetPosition(C);
        builder.SetUv(UvC);
        var c = builder.AddVertex();
        builder.AddTriangle(a,b,c);
    }
}

public class MeshBuilderApp : ISdlApp
{
    private readonly IPlatform platform;

    private Shader shader;
    private MeshBuilder builder;
    private Mesh mesh;
    private PhongMaterial material;
    private Camera3d camera;
    private Transform3d transform;
    private PointLight light;

    public MeshBuilderApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.shader = new PhongShader();

        this.builder = new MeshBuilder();

        BuildCube();

        this.mesh = builder.Build();

        this.material = new PhongMaterial(new Texture("assets/textures/bricks.jpeg"), Uniform.AlbedoTexture);
        material.Roughness = 0;
        material.Metallic = 0;

        this.mesh.Primitives.First().Material = this.material;
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

    private void BuildCube()
    {
        BuildTriangle(
            new Vector3(-1,-1,1), 
            new Vector3(1,-1,1),
            new Vector3(-1,1,1));
    }

    private void BuildTriangle(Vector3 a, Vector3 b, Vector3 c, RectangleF textureRect = default(RectangleF)){
        textureRect = textureRect == default(RectangleF) ? new RectangleF(0,0,1,1) : textureRect;
        
        
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
        mesh.DrawUsingShader(shader);
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
