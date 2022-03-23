using System.Numerics;
using GLES2;
using XPlat.Core;
using XPlat.Graphics;

public class GraphicsApp : ISdlApp
{
    private readonly IPlatform platform;

    private Shader shader;
    private Primitive primitive;
    private TextureMaterial texture;

    public GraphicsApp(IPlatform platform)
    {
        this.platform = platform;
    }

    public void Init()
    {
        this.shader = new Shader(File.ReadAllText("shader/fbo.vertex.glsl"), File.ReadAllText("shader/fbo.fragment.glsl"),
        new Dictionary<XPlat.Graphics.Attribute, string>
        {
            [XPlat.Graphics.Attribute.Position] = "aPos",
            [XPlat.Graphics.Attribute.Uv_0] = "aUv",
        },
        new Dictionary<Uniform, string>
        {
            [Uniform.AlbedoTexture] = "uTex",
            [Uniform.TextureSize] = "uTexRes",
            [Uniform.ViewportSize] = "uScreenRes",
        });

        this.primitive = new Primitive(new[]
        {
            new VertexAttribute<float>(XPlat.Graphics.Attribute.Position, VertexPositions, VertexAttributeDescriptor.Vec2f),
            new VertexAttribute<float>(XPlat.Graphics.Attribute.Uv_0, VertexUvs, VertexAttributeDescriptor.Vec2f),
        }, new VertexIndices(Indices));

        this.texture = new TextureMaterial(new Texture("assets/desktop.jpeg"), Uniform.AlbedoTexture);
        
        this.primitive.Material = this.texture;
    }

    public void Update()
    {
        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT);

        //this.shader.SetUniform(StandardUniform.TextureSize, texture.Size);
        this.shader.SetUniform(Uniform.ViewportSize, platform.RendererSize);

        this.primitive.DrawWithShader(shader);
    }

    private static readonly float[] VertexPositions = new float[] { -1,1, -1,-1, 1,-1, 1,1 };
    private static readonly float[] VertexUvs = new float[] { 0,0, 0,1, 1,1, 1,0 };
    private static readonly ushort[] Indices = new ushort[] { 0,1,2, 0,2,3 };
}
