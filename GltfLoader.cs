using GLES2;
using System.Numerics;

namespace net6test
{
    public static class GltfLoader
    {
        public static Scene LoadScene(SharpGLTF.Schema2.ModelRoot root)
        {
            var scene = new Scene();
            foreach (var n in root.DefaultScene.VisualChildren)
            {
                scene.RootNode.AddChild(LoadNode(n));
            }
            return scene;
        }

        public static Node LoadNode(SharpGLTF.Schema2.Node node)
        {
            var mynode = new Node(); 
            mynode.Transform = node.LocalTransform;
            if(node.Mesh != null)
            {
                var prims = node.Mesh.Primitives.Select(x => LoadPrimitive(x)).ToArray();
                var mesh = new Mesh(prims);
                var renderer = new RendererComponent();
                renderer.Mesh = mesh;
                mynode.AddComponent(renderer);
            }
            return mynode;
        }

        public static Primitive LoadPrimitive(SharpGLTF.Schema2.MeshPrimitive prim)
        {
            var indices = prim.GetIndexAccessor().AsIndicesArray().Select(x => (ushort)x).ToArray();
            var vis = new VertexIndices(indices);

            return new Primitive(LoadAttributes(prim).ToArray(), vis);
        }

        private static readonly Dictionary<string, StandardAttribute> mappings = new()
        {
            ["POSITION"] = StandardAttribute.Position,
            ["NORMAL"] = StandardAttribute.Normal,
            ["TANGENT"] = StandardAttribute.Tangent,
            ["TEXCOORD_0"] = StandardAttribute.Uv_0,
            ["TEXCOORD_1"] = StandardAttribute.Uv_1,
        };

        private static IEnumerable<VertexAttribute> LoadAttributes(SharpGLTF.Schema2.MeshPrimitive prim)
        {
            foreach (var kv in prim.VertexAccessors)
            {
                if (mappings.ContainsKey(kv.Key))
                {
                    var acc = kv.Value;
                    var type = mappings[kv.Key];

                    if (acc.Dimensions == SharpGLTF.Schema2.DimensionType.VEC3 && acc.Encoding == SharpGLTF.Schema2.EncodingType.FLOAT)
                    {
                        var desc = new VertexAttributeDescriptor(3, GL.GL_FLOAT, 0, 0, acc.Normalized);
                        yield return new VertexAttribute<Vector3>(type, acc.AsVector3Array().ToArray(), desc);
                    }
                }
            }
            yield break;
        }

    }
}