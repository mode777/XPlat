using System.Diagnostics;
using System.Numerics;
using CsharpVoxReader;
using GLES2;
using SDL2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace net6test.samples
{
    public class VoxLoader : IVoxLoader
    {
        private VoxelBuffer buffer;

        public VoxLoader()
        {
            this.buffer = new VoxelBuffer();
        }

        public void LoadModel(int sizeX, int sizeY, int sizeZ, byte[,,] data)
        {
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    for (int z = 0; z < sizeZ; z++)
                    {
                        var value = data[x,y,z];
                        if(value == 0) continue;

                        // y-axis is front-back
                        // front
                        if(y < sizeY-1 && data[x,y+1,z] == 0) buffer.Face(x,y,z,FaceDirection.Top, value-1);
                        
                        // back
                        if(y > 0 && data[x,y-1,z] == 0) buffer.Face(x,y,z,FaceDirection.Bottom, value-1);

                        // z-axis is top-bottom
                        // top
                        if(z < sizeZ-1 && data[x,y,z+1] == 0) buffer.Face(x,y,z,FaceDirection.Front, value-1);

                        // bottom
                        if(z > 0 && data[x,y,z-1] == 0) buffer.Face(x,y,z,FaceDirection.Back, value-1);

                        // x-axis is left-right
                        // right
                        if(x < sizeX-1 && data[x+1,y,z] == 0) buffer.Face(x,y,z,FaceDirection.Right, value-1);

                        // left
                        if(x > 0 && data[x-1,y,z] == 0) buffer.Face(x,y,z,FaceDirection.Left, value-1);
                    }
                }
            }
        }

        public void LoadPalette(uint[] palette)
        {
            //throw new NotImplementedException();
        }

        public void NewGroupNode(int id, Dictionary<string, byte[]> attributes, int[] childrenIds)
        {
            //throw new NotImplementedException();
        }

        public void NewLayer(int id, string name, Dictionary<string, byte[]> attributes)
        {
            //throw new NotImplementedException();
        }

        public void NewMaterial(int id, Dictionary<string, byte[]> attributes)
        {
            //throw new NotImplementedException();
        }

        public void NewShapeNode(int id, Dictionary<string, byte[]> attributes, int[] modelIds, Dictionary<string, byte[]>[] modelsAttributes)
        {
            //throw new NotImplementedException();
        }

        public void NewTransformNode(int id, int childNodeId, int layerId, string name, Dictionary<string, byte[]>[] framesAttributes)
        {
            //throw new NotImplementedException();
        }

        public void SetMaterialOld(int paletteId, CsharpVoxReader.Chunks.MaterialOld.MaterialTypes type, float weight, CsharpVoxReader.Chunks.MaterialOld.PropertyBits property, float normalized)
        {
            //throw new NotImplementedException();
        }

        public void SetModelCount(int count)
        {
            //throw new NotImplementedException();
        }

        public Material GetMaterial(){
            //throw new NotImplementedException();
            // todo
            return new TextureMaterial("assets/dbpal.png", StandardUniform.AlbedoTexture);
        }

        public Primitive GetPrimitive(){
            return buffer.ToPrimitive();
        }
    }

    public class VoxelBuffer {
        private VoxelFace[] faces = new VoxelFace[1];
        private ushort[] indices = new ushort[6];
        private int face = 0;

        private void Grow(){
            Array.Resize(ref faces, faces.Length * 2);
            Array.Resize(ref indices, faces.Length * 6);
        }

        public void Face(int x, int y, int z, FaceDirection dir, int color){
            if(face >= faces.Length) Grow();
            faces[face] = new VoxelFace(x,y,z,dir,color);
            var i = face*6;
            var o = face*4;
            indices[i+0] = (ushort)(o+0);
            indices[i+1] = (ushort)(o+1);
            indices[i+2] = (ushort)(o+2);
            indices[i+3] = (ushort)(o+0);
            indices[i+4] = (ushort)(o+2);
            indices[i+5] = (ushort)(o+3);
            face++;
        }

        public Primitive ToPrimitive(){
            var desc = new VertexAttributeDescriptor(2, GL.UNSIGNED_SHORT);
            var buffer = GlUtil.CreateBuffer<VoxelFace>(GL.ARRAY_BUFFER, faces);
            Debug.WriteLine("Triangles: " + (face-1) * 2);
            var position = new VertexAttribute(StandardAttribute.Position, buffer, desc);
            var idx = new VertexIndices(indices);
            idx.ElementsCount = (uint)face*6-1;
            return new Primitive(new[] {position}, idx);
        }
    }


    // 4-bit: sign (0=-1, 1=1 -> x*2-1)|z|y|x
    public enum FaceDirection : int {
        Front = 0b1100,
        Back = 0b0100,
        Top = 0b1010,
        Bottom = 0b0010,
        Right = 0b1001,
        Left = 0b0001
    }

    public struct VoxelVertex {
        ushort packed1 = 0;
        ushort packed2 = 0;

        public VoxelVertex(int x, int y, int z, FaceDirection dir, int color){
            packed1 |= (ushort)((31 & x) | (31 & y) << 5 | (31 & z) << 10);
            packed2 |= (ushort)((15 & (int)dir));
            packed2 |= (ushort)((255 & color) << 8);
        }

        public Vector3i Position => new Vector3i(packed1 & 31, (packed1 >> 5) & 31, (packed1 >> 10) & 31);
        public Vector3i Normal
        {
            get
            {
                var sign = (packed2 >> 3) * 2 - 1;
                return new Vector3i(
                    ((packed2 >> 0) & 1) * sign,
                    ((packed2 >> 1) & 1) * sign,
                    ((packed2 >> 2) & 1) * sign);
            }
        }
        public int Color => (packed2 >> 8) & 255;
    }

    public struct VoxelFace {
        VoxelVertex A;
        VoxelVertex B;
        VoxelVertex C;
        VoxelVertex D;

        public VoxelFace(int x, int y, int z, FaceDirection dir, int color, int w=1, int h=1){
            switch (dir)
            {
                case FaceDirection.Front:
                A = new VoxelVertex(x,   y,   z+1, dir, color);
                B = new VoxelVertex(x+1, y,   z+1, dir, color);
                C = new VoxelVertex(x+1, y+1, z+1, dir, color);
                D = new VoxelVertex(x,   y+1, z+1, dir, color);
                break;
                case FaceDirection.Back:
                A = new VoxelVertex(x,   y,   z  , dir, color);
                B = new VoxelVertex(x  , y+1, z  , dir, color);
                C = new VoxelVertex(x+1, y+1, z  , dir, color);
                D = new VoxelVertex(x+1, y,   z  , dir, color);
                break;
                case FaceDirection.Top:
                A = new VoxelVertex(x,   y+1, z  , dir, color);
                B = new VoxelVertex(x  , y+1, z+1, dir, color);
                C = new VoxelVertex(x+1, y+1, z+1, dir, color);
                D = new VoxelVertex(x+1, y+1, z  , dir, color);
                break;
                case FaceDirection.Bottom:
                A = new VoxelVertex(x,   y,   z  , dir, color);
                B = new VoxelVertex(x+1, y,   z  , dir, color);
                C = new VoxelVertex(x+1, y,   z+1, dir, color);
                D = new VoxelVertex(x,   y,   z+1, dir, color);
                break;
                case FaceDirection.Right:
                A = new VoxelVertex(x+1, y,   z  , dir, color);
                B = new VoxelVertex(x+1, y+1, z  , dir, color);
                C = new VoxelVertex(x+1, y+1, z+1, dir, color);
                D = new VoxelVertex(x+1, y,   z+1, dir, color);
                break;
                case FaceDirection.Left:
                A = new VoxelVertex(x,   y,   z  , dir, color);
                B = new VoxelVertex(x,   y,   z+1, dir, color);
                C = new VoxelVertex(x,   y+1, z+1, dir, color);
                D = new VoxelVertex(x,   y+1, z,   dir, color);
                break;
                default:
                throw new InvalidOperationException();
            }

        }
    }

    public class Voxels : ISdlApp
    {
        private readonly IPlatform platform;

        public Voxels(IPlatform platform)
        {
            this.platform = platform;

        }
        float r = 0;

        public void Init()
        {
            shader = new Shader(File.ReadAllText("shaders/voxel.vertex.glsl"), File.ReadAllText("shaders/voxel.fragment.glsl"), new()
            {
                [StandardAttribute.Position] = "vertexPacked",
                //[StandardAttribute.Normal] = "aNormal"
            }, new()
            {
                [StandardUniform.ModelMatrix] = "model",
                [StandardUniform.ViewMatrix] = "view",
                [StandardUniform.ProjectionMatrix] = "projection",
                [StandardUniform.AlbedoTexture] = "texture"
            });

            scene = new Scene();
            scene.RootNode.AddChild(LoadVoxObject());
            model = scene.FindNode("cube");
        }

        private Node LoadVoxObject()
        {
            var loader = new VoxLoader();
            var r = new VoxReader("assets/stan.vox", loader);
            r.Read();
            
            var node = new Node { Name = "cube" };   
            var prim = loader.GetPrimitive();
            prim.Material = loader.GetMaterial();
            var mesh = new Mesh(new[] { prim });

            var component = new RendererComponent();
            component.Mesh = mesh;

            node.AddComponent(component);
            return node;
        }

        private Node CreateCubeNode(){
            var node = new Node { Name = "cube" };

            var buffer = new VoxelBuffer();

            for (int i = 0; i < 16; i++)
            {                
                buffer.Face(i,i,i,FaceDirection.Front, i);
                buffer.Face(i,i,i,FaceDirection.Back, i);
                buffer.Face(i,i,i,FaceDirection.Top, i);
                buffer.Face(i,i,i,FaceDirection.Bottom, i);
                buffer.Face(i,i,i,FaceDirection.Right, i);
                buffer.Face(i,i,i,FaceDirection.Left, i);
            }

            var prim = buffer.ToPrimitive();
            prim.Material = new TextureMaterial("assets/dbpal.png", StandardUniform.AlbedoTexture);

            var mesh = new Mesh(new[] { prim });

            var component = new RendererComponent();
            component.Mesh = mesh;

            node.AddComponent(component);

            return node;
        }   

        public void Update(){
            DrawScene();
        }

        private void DrawScene()
        {
            GL.UseProgram(shader.Handle);
            Shader.Use(shader);

            GL.Enable(GL.DEPTH_TEST);
            GL.Enable(GL.CULL_FACE);

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.DEPTH_BUFFER_BIT);

            var screenSize = platform.RendererSize;
            matP = Matrix4x4.CreatePerspectiveFieldOfView((float)Math.PI / 4, screenSize.Width / (float)screenSize.Height, 0.1f, 100);
            shader.SetUniform(StandardUniform.ProjectionMatrix, ref matP);

            var cameraPos = new Vector3(0.001f, 7, 0);
            var cameraTarget = new Vector3(0, 0, 0);
            matV = Matrix4x4.CreateLookAt(cameraPos, cameraTarget, new Vector3(0, 1, 0));
            shader.SetUniform(StandardUniform.ViewMatrix, ref matV);
            shader.SetUniform("viewPos", cameraPos);
            var m = platform.MousePosition;
            var mx = (m.X / (float)platform.RendererSize.Width) * 10.0f - 5.0f;
            var my = (m.Y / (float)platform.RendererSize.Height) * 10.0f - 5.0f;
            shader.SetUniform("lightPos", new Vector3(my,4,-mx));
            shader.SetUniform("lightColor", "#ffffff");
            shader.SetUniform("objectColor", "#ffffff");

            float time = SDL.SDL_GetTicks() / 1000f;
            var scale = (float)Math.Sin(time) / 4 + 1;
            //model.Transform.Translation = 
            model.Transform.Scale = new Vector3(scale*0.1f, scale*0.1f, scale*0.1f);
            model.Transform.Rotation = Quaternion.CreateFromYawPitchRoll(0, r += 0.01f, MathF.PI/2);
            matM = model.Transform.GetMatrix();
            //shader.SetUniform(StandardUniform.ModelMatrix, ref matM);


            Matrix4x4 matMI;
            var succ = Matrix4x4.Invert(matM, out matMI);
            Matrix4x4 matMIT = Matrix4x4.Transpose(matMI);
            shader.SetUniform("modelTransInv", ref matMIT);

            scene.RootNode.Draw();
        }
        private Shader shader;
        private Matrix4x4 matP;
        private Matrix4x4 matV;
        private Matrix4x4 matM;
        private Scene scene;
        private Node model;
    }
}