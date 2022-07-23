using System.Xml.Linq;
using CsharpVoxReader;
using XPlat.Engine;
using XPlat.Engine.Serialization;
using XPlat.Graphics;

namespace XPlat.Voxels;

[SceneElement("vox")]
public class VoxResource : FileResource, ISerializableResource
{
    public Mesh Mesh => Value as Mesh;
    private readonly IServiceProvider services;

    public VoxResource() : base()
    {
    }
    protected override object LoadFile()
    {
        //if (Primitive != null) Primitive.Dispose();
        return LoadVox(Filename);
    }

    private Mesh LoadVox(string filename){
        //var img = Image.Load<Rgba32>(paletteFilename);
        var loader = new VoxLoader();
        //loader.SetPalette(img.GetPixelRowSpan(0).ToArray());
        var r = new VoxReader(filename, loader);
        r.Read();
        
        var prim = loader.GetPrimitive();
        var mesh = new Mesh(prim);
        //prim.Material = loader.GetMaterial();
        return mesh;
    }

    public void Parse(XElement el, SceneReader reader)
    {
        if(el.TryGetAttribute("src", out var src)) { Filename = reader.ResolvePath(src); Load(); }
        if(el.TryGetAttribute("watch", out var value) && bool.TryParse(value, out var watch) && watch) { Watch(); }
    }
}
