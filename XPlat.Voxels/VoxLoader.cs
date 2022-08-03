using CsharpVoxReader;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Graphics;

namespace XPlat.Voxels
{
    public class VoxelMesh {
        private readonly byte[,,] data;

        // TODO: load default palette
        public Rgba32[] Palette { get; set; }
        private readonly FaceAdjacency[,,] adj;

        public int SizeX => data.GetLength(0);
        public int SizeY => data.GetLength(1);
        public int SizeZ => data.GetLength(2);

        public VoxelMesh(byte[,,] data){
            this.data = data;
            this.adj = new FaceAdjacency[data.GetLength(0),data.GetLength(1),data.GetLength(2)];
        }

        private void Iterate(Action<int,int,int> action){
            for (int z = 0; z < data.GetLength(2); z++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    for (int x = 0; x < data.GetLength(0); x++)
                    {
                        action(x,y,z);
                    }
                }
            }
        }

        private void CalculateAdjacency(){
            for (int z = 0; z < data.GetLength(2); z++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    for (int x = 0; x < data.GetLength(0); x++)
                    {
                        var value = data[x,y,z];
                        var top = y < SizeY-1 ? data[x,y+1,z] : 0;
                        var bottom = y > 0 ? data[x,y-1,z] : 0;
                        var front = z < SizeZ-1 ? data[x,y,z+1] : 0;
                        var back = z > 0 ? data[x,y,z-1] : 0;
                        var right = x < SizeX-1 ? data[x+1,y,z] : 0;
                        var left = x > 0 ? data[x-1,y,z] : 0;

                        adj[x,y,z] = FaceAdjacency.None;

                        if(value == 0){
                            continue;
                        }

                        // y-axis is front-back
                        // front
                        if(top == 0) {
                            adj[x,y,z] |= FaceAdjacency.Top;
                            //buffer.Face(x,y,z,FaceDirection.Top, value-1);
                        }
                        
                        // back
                        if(bottom == 0) {
                            adj[x,y,z] |= FaceAdjacency.Bottom;
                            //buffer.Face(x,y,z,FaceDirection.Bottom, value-1);
                        }

                        // z-axis is top-bottom
                        // top
                        if(front == 0){
                            adj[x,y,z] |= FaceAdjacency.Front;
                            //buffer.Face(x,y,z,FaceDirection.Front, value-1);
                        } 

                        // bottom
                        if(back == 0) {
                            adj[x,y,z] |= FaceAdjacency.Back;
                            //buffer.Face(x,y,z,FaceDirection.Back, value-1);
                        }

                        // x-axis is left-right
                        // right
                        if(right == 0) {
                            adj[x,y,z] |= FaceAdjacency.Right;
                            //buffer.Face(x,y,z,FaceDirection.Right, value-1);
                        }

                        // left
                        if(left == 0) {
                            adj[x,y,z] |= FaceAdjacency.Left;
                            //buffer.Face(x,y,z,FaceDirection.Left, value-1);
                        }
                    }
                }
            }

        }

        bool ReadFlag(FaceAdjacency f, int a, int b, int depth){
            switch (f)
            {
                case FaceAdjacency.Front:
                case FaceAdjacency.Back:
                    return adj[a,b,depth].HasFlag(f);
                case FaceAdjacency.Top:
                case FaceAdjacency.Bottom:
                    return adj[a,depth,b].HasFlag(f);
                case FaceAdjacency.Right:
                case FaceAdjacency.Left:
                    return adj[depth,a,b].HasFlag(f);
                default:
                throw new InvalidOperationException();
            }
        }

        void ClearFlag(FaceAdjacency f, int a, int b, int depth){
            switch (f)
            {
                case FaceAdjacency.Front:
                case FaceAdjacency.Back:
                    adj[a,b,depth] &= (~f);
                    break;
                case FaceAdjacency.Top:
                case FaceAdjacency.Bottom:
                    adj[a,depth,b]  &= (~f);
                    break;
                case FaceAdjacency.Right:
                case FaceAdjacency.Left:
                    adj[depth,a,b] &= (~f);
                    break;
                default:
                throw new InvalidOperationException();
            }
        }

        Rgba32 GetPixel(FaceAdjacency f, int a, int b, int d){
            int value = 0;
            switch (f)
            {
                case FaceAdjacency.Front:
                case FaceAdjacency.Back:
                    value = data[a,b,d];
                    break;
                case FaceAdjacency.Top:
                case FaceAdjacency.Bottom:
                    value = data[a,d,b];
                    break;
                case FaceAdjacency.Right:
                case FaceAdjacency.Left:
                    value = data[d,a,b];
                    break;
                default:
                throw new InvalidOperationException();
            }
            return Palette[value];
        }

        Rectangle MergeCells(FaceAdjacency flag_, int a, int b, int depth_, int size_a_, int size_b_, VoxelBuffer buffer){
            
            int w = 1;
            int h = 1;

            for(int ca = a+1; ca < size_a_; ca++){
                if(ReadFlag(flag_, ca, b, depth_)) w++;
                else break;
            }
            for(int cb = b+1; cb < size_b_; cb++){
                var stop = false;
                for(int ca = a; ca < a+w; ca++){
                    if(!ReadFlag(flag_, ca, cb, depth_)){
                        stop = true;
                        break;
                    }
                }
                if(!stop) h++;
                else break;
            }

            var p = buffer.ReserveRectangle(w,h);

            for(int cb = b; cb < b+h; cb++){
                for(int ca = a; ca < a+w; ca++){
                    ClearFlag(flag_, ca, cb, depth_);
                    var color = GetPixel(flag_, ca, cb, depth_);
                    color.A = EncodeAo(2,2,3,3);
                    buffer.SetPixel(p.X + ca - a, p.Y + cb - b, color);
                }
            }

            return new Rectangle(p.X,p.Y,w,h);
        }

        private byte EncodeAo(int ul, int ur, int dr, int dl) {
            return (byte)(ul | (ur<<2) | (dr<<4) | (dl<<6));
        }

        public void WriteTo(VoxelBuffer buffer){
            CalculateAdjacency();
            for (int z = 0; z < data.GetLength(2); z++)
            {
                for (int y = 0; y < data.GetLength(1); y++)
                {
                    for (int x = 0; x < data.GetLength(0); x++)
                    {
                        var a = adj[x,y,z];         
                        var v = data[x,y,z];      
                        
                        if(a == FaceAdjacency.None) continue;
                        var c = Palette[v];

                        if(a.HasFlag(FaceAdjacency.Front)){
                            var r = MergeCells(FaceAdjacency.Front, x, y, z, SizeX, SizeY, buffer);
                            buffer.Face(x,y,z,FaceDirection.Front, c, r.Width, r.Height, r.X, r.Y);
                        }

                        if(a.HasFlag(FaceAdjacency.Back)){
                            var r = MergeCells(FaceAdjacency.Back, x, y, z, SizeX, SizeY, buffer);
                            buffer.Face(x,y,z,FaceDirection.Back, c, r.Width, r.Height, r.X, r.Y);
                        }

                        if(a.HasFlag(FaceAdjacency.Top)){
                            var r = MergeCells(FaceAdjacency.Top, x, z, y, SizeX, SizeZ, buffer);
                            buffer.Face(x,y,z,FaceDirection.Top, c, r.Width, r.Height, r.X, r.Y);
                        }

                        if(a.HasFlag(FaceAdjacency.Bottom)){
                            var r = MergeCells(FaceAdjacency.Bottom, x, z, y, SizeX, SizeZ, buffer);
                            buffer.Face(x,y,z,FaceDirection.Bottom, c, r.Width, r.Height, r.X, r.Y);
                        }

                        if(a.HasFlag(FaceAdjacency.Left)){
                            var r = MergeCells(FaceAdjacency.Left, y, z, x, SizeY, SizeZ, buffer);
                            buffer.Face(x,y,z,FaceDirection.Left, c, r.Width, r.Height, r.X, r.Y);
                        }

                        if(a.HasFlag(FaceAdjacency.Right)){
                            var r = MergeCells(FaceAdjacency.Right, y, z, x, SizeY, SizeZ, buffer);
                            buffer.Face(x,y,z,FaceDirection.Right, c, r.Width, r.Height, r.X, r.Y);
                        }
                    }
                }
            }
        }
    }

    public class VoxLoader : IVoxLoader
    {
        private VoxelBuffer buffer;
        private Rgba32[] palette;
        private VoxelMesh voxel;

        public VoxLoader()
        {
            this.buffer = new VoxelBuffer();
        }

        public void LoadModel(int sizeX, int sizeY, int sizeZ, byte[,,] data)
        {
            this.voxel = new VoxelMesh(data);
            
        }

        public void LoadPalette(uint[] palette)
        {
            voxel.Palette = palette.Select(x =>
            {
                var b = BitConverter.GetBytes(x);
                var c = new Rgba32(b[2],b[1],b[0],b[3]);
                return c;
            }).ToArray();
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

        // public Material GetMaterial(){
        //     //throw new NotImplementedException();
        //     var img = new Image<Rgba32>(2,2,new Rgba32(255,0,0));
        //     img.TryGetSinglePixelSpan(out var span);
        //     span[0] = new Rgba32(0,255,0);
        //     span[3] = new Rgba32(0,255,0);
        //     var t = new Texture(img, TextureUsage.GraphicsPixel);
        //     t.SetRepeat();
        //     // todo
        //     return new PhongMaterial(t);
        // }

        public Primitive GetPrimitive(){
            voxel.WriteTo(buffer);
            return buffer.ToPrimitive();
        }
    }
}