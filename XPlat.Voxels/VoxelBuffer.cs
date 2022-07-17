using System.Diagnostics;
using GLES2;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Core;
using XPlat.Graphics;

namespace XPlat.Voxels
{
    public class VoxelBuffer {
        private List<VoxelFace> faces = new();
        private List<ushort> indices = new();
        private RectanglePacker packer = new RectanglePacker(256,256);
        private Image<Rgba32> image = new Image<Rgba32>(256,256, new Rgba32(255,0,255));
        private int face = 0;
        public void Face(int x, int y, int z, FaceDirection dir, Rgba32 color, int w = 1, int h = 1, int tx = 0, int ty = 0){
            faces.Add(new VoxelFace(x,y,z,dir,w,h,tx,ty));
            var o = face*4;
            indices.Add((ushort)(o+0));
            indices.Add((ushort)(o+1));
            indices.Add((ushort)(o+2));
            indices.Add((ushort)(o+0));
            indices.Add((ushort)(o+2));
            indices.Add((ushort)(o+3));
            face++;
        }

        public Point ReserveRectangle(int w, int h){
            packer.AddRect(w,h,out var x, out var y);
            return new Point(x,y);
        }

        public void SetPixel(int x, int y, Rgba32 pixel){
            image.GetPixelRowSpan(y)[x] = pixel;
        }

        public Primitive ToPrimitive(){
            var desc = new VertexAttributeDescriptor(2, GL.UNSIGNED_SHORT);
            var buffer = GlUtil.CreateBuffer<VoxelFace>(GL.ARRAY_BUFFER, faces.ToArray());
            Debug.WriteLine("Faces: " + faces.Count);
            var attributes = Vertex.CreateAttributes(buffer);
            var idx = new VertexIndices(indices.ToArray());
            var p = new Primitive(attributes, idx);
            //image.SaveAsPng(Guid.NewGuid() + ".png");
            p.Material = new PhongMaterial(new Texture(image, TextureUsage.GraphicsPixel));
            return p;
        }
    }
}