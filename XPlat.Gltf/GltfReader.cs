using System.Numerics;
using GLES2;
using SharpGLTF.Schema2;
using XPlat.Core;
using XPlat.Graphics;
using Material = SharpGLTF.Schema2.Material;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace XPlat.Gltf
{
    public class GltfReader
    {
        public static GltfScene Load(string path){
            var root = ModelRoot.Load(path).DefaultScene;
            return new GltfScene(root);
        }
    }

    public class GltfScene {

        public string Name { get; private set; }
        public IEnumerable<GltfNode> Nodes => EnumerateNodes();


        internal Dictionary<int, XPlat.Graphics.Material> MaterialLookup { get; } = new Dictionary<int, Graphics.Material>();
        public IEnumerable<XPlat.Graphics.Material> Materials => MaterialLookup.Values;
        private readonly Scene scene;

        public GltfNode RootNode { get; private set; }

        internal GltfScene(Scene scene){
            this.scene = scene;
            Name = scene.Name;
        }
        private IEnumerable<GltfNode> EnumerateNodes()
        {
            foreach (var c in scene.VisualChildren)
            {
                var n = new GltfNode(c);
                n.Scene = this;
                n.Read();
                yield return n;
            }
            yield break;
        }

        public GltfNode FindNode(string path){
            var p = path.Split("/");
            var idx = 0;
            var c = Nodes;
            while(p[idx] == "") idx++;
            while(c != null){
                var iter = c;
                foreach (var n in iter)
                {
                    if(n.Name == p[idx]){
                        if(idx == p.Length-1) return n;
                        c = n.Children;
                        idx++;
                        break;
                    }
                    c = null;
                }
            }
            return null;

        }

        public void Dump(Action<string> writer){
            writer($"Scene: {Name}");
            foreach (var n in Nodes)
            {
                DumpRecur(writer, n, "");
            }
        }

        private void DumpRecur(Action<string> writer, GltfNode node, string prefix){
            prefix += $"/{node.Name}";
            writer($"Node: {prefix}");
            if(node.HasLight) writer($"Light: {prefix}");
            if(node.HasMesh) writer($"Mesh: {prefix}");
            //if(node.HasCamera) writer($"Camera: {prefix}");
            foreach (var n in node.Children)
            {
                DumpRecur(writer, n, prefix);
            }
        }
    }

    public class GltfNode
    {
        public string Name { get; private set; }
        public Transform3d Transform { get; private set; }
        // public Camera3d Camera { get; private set; }
        // public XPlat.Graphics.Mesh Mesh { get; private set; }
        // public PointLight Light { get; private set; }
        public GltfScene Scene { get; internal set; }
        public GltfNode Parent { get; internal set; }

        public IEnumerable<GltfNode> Children => EnumerateChildren();

        internal GltfNode(Node node){
            this.node = node;
        }

        private IEnumerable<GltfNode> EnumerateChildren(){
            foreach (var c in node.VisualChildren)
            {
                var n = new GltfNode(c);
                n.Scene = this.Scene;
                n.Parent = this;
                n.Read();
                yield return n;
            }
            yield break;
        }

        internal void Read(){
            Name = node.Name;
            ReadTransform(node);
        }

        public bool HasMesh => node.Mesh != null;
        public XPlat.Graphics.Mesh ReadMesh()
        {
            if(!HasMesh) return null;
            var prims = node.Mesh.Primitives.Select(x => CreatePrimitive(x)).ToArray();
            var mesh = new XPlat.Graphics.Mesh(prims);
            return mesh;
        }

        private Primitive CreatePrimitive(MeshPrimitive prim){
            var indices = prim.GetIndexAccessor().AsIndicesArray().Select(x => (ushort)x).ToArray();
            var vis = new VertexIndices(indices);

            var p = new Primitive(CreateAttributes(prim).ToArray(), vis); 
            
            if(!Scene.MaterialLookup.ContainsKey(prim.Material.LogicalIndex)){
                Scene.MaterialLookup[prim.Material.LogicalIndex] = CreateMaterial(prim.Material);
            }
            p.Material = Scene.MaterialLookup[prim.Material.LogicalIndex];

            return p;
        }

        private static readonly Dictionary<string, Graphics.Attribute> mappings = new()
        {
            ["POSITION"] = Graphics.Attribute.Position,
            ["NORMAL"] = Graphics.Attribute.Normal,
            ["TANGENT"] = Graphics.Attribute.Tangent,
            ["TEXCOORD_0"] = Graphics.Attribute.Uv_0,
            ["TEXCOORD_1"] = Graphics.Attribute.Uv_1,
        };
        private readonly Node node;

        private IEnumerable<VertexAttribute> CreateAttributes(MeshPrimitive prim){
            foreach (var kv in prim.VertexAccessors)
            {
                if (mappings.ContainsKey(kv.Key))
                {
                    var acc = kv.Value;
                    var type = mappings[kv.Key];
                    var dimension = 0;

                    switch (acc.Dimensions)
                    {
                        case SharpGLTF.Schema2.DimensionType.VEC2:
                            dimension = 2;
                            break;
                        case SharpGLTF.Schema2.DimensionType.VEC3:
                            dimension = 3;
                            break;
                        default:
                            break;
                    }
                    
                    if (dimension == 3 && acc.Encoding == SharpGLTF.Schema2.EncodingType.FLOAT)
                    {
                        var desc = new VertexAttributeDescriptor(dimension, GL.FLOAT, 0, 0, acc.Normalized);
                        yield return new VertexAttribute<Vector3>(type, acc.AsVector3Array().ToArray(), desc);
                    }
                    else if(dimension == 2 && acc.Encoding == SharpGLTF.Schema2.EncodingType.FLOAT)
                    {
                        var desc = new VertexAttributeDescriptor(dimension, GL.FLOAT, 0, 0, acc.Normalized);
                        yield return new VertexAttribute<Vector2>(type, acc.AsVector2Array().ToArray(), desc);
                    }
                }
            }
            yield break;
        }

        private XPlat.Graphics.Material CreateMaterial(Material mat){
            Image<Rgba32> pixels = null;
            var baseColor = mat.FindChannel("BaseColor").Value;
            var metallicRoughness = mat.FindChannel("MetallicRoughness").Value;
            var img = baseColor.Texture?.PrimaryImage.Content;
            if(img != null){
                pixels = Image.Load<Rgba32>(img.Value.Content.Span);
            } else {
                var p = baseColor.Parameter;
                pixels = new Image<Rgba32>(1,1,new Rgba32(p.X, p.Y, p.Z, p.W));
            }
            return new PhongMaterial(new Graphics.Texture(pixels)){
                Metallic = metallicRoughness.Parameter.X,
                Roughness = metallicRoughness.Parameter.Y
            };
        }

        public bool HasLight => node.PunctualLight != null;
        public PointLight ReadLight()
        {
            if(!HasLight) return null;
            return new PointLight {
                Color = node.PunctualLight.Color,
                Intensity = node.PunctualLight.Intensity * 0.005f,
                Range = node.PunctualLight.Range
            };
        }

        public bool HasCamera => node.Camera != null;
        public Camera3d ReadCamera(){
            if(!HasCamera) return null;
            if(node.Camera.Settings is CameraPerspective cam){
                return new Camera3d {
                    Fov = cam.VerticalFOV,
                    Ratio = cam.AspectRatio ?? 1,
                    NearPlane = cam.ZNear,
                    FarPlane = cam.ZFar
                };
            } else {
                throw new NotImplementedException("Orthographic camereas not yet supported");
            }
        }
        

        private void ReadTransform(Node node){
            Transform = new Transform3d {
                TranslationVector = node.LocalTransform.Translation,
                ScaleVector = node.LocalTransform.Scale,
                RotationQuat = node.LocalTransform.Rotation
            };
        }


    }
}
