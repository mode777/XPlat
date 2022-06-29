using CsharpVoxReader;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using XPlat.Graphics;

namespace XPlat.Voxels
{
    public class VoxelMesh {
        private readonly byte[,,] data;
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

        void MergeCells(FaceAdjacency flag_, int a, int b, int depth_, int size_a_, int size_b_, out int w, out int h){
            
            w = 1;
            h = 1;

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
            for(int cb = b; cb < b+h; cb++){
                for(int ca = a; ca < a+w; ca++){
                    ClearFlag(flag_, ca, cb, depth_);
                }
            }
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
                        if(a == FaceAdjacency.None) continue;

                        if(a.HasFlag(FaceAdjacency.Front)){
                            MergeCells(FaceAdjacency.Front, x, y, z, SizeX, SizeY, out var w, out var h);
                            buffer.Face(x,y,z,FaceDirection.Front, 1, w, h);
                        }

                        if(a.HasFlag(FaceAdjacency.Back)){
                            MergeCells(FaceAdjacency.Back, x, y, z, SizeX, SizeY, out var w, out var h);
                            buffer.Face(x,y,z,FaceDirection.Back, 1, w, h);
                        }

                        if(a.HasFlag(FaceAdjacency.Top)){
                            MergeCells(FaceAdjacency.Top, x, z, y, SizeX, SizeZ, out var w, out var h);
                            buffer.Face(x,y,z,FaceDirection.Top, 1, w, h);
                        }

                        if(a.HasFlag(FaceAdjacency.Bottom)){
                            MergeCells(FaceAdjacency.Bottom, x, z, y, SizeX, SizeZ, out var w, out var h);
                            buffer.Face(x,y,z,FaceDirection.Bottom, 1, w, h);
                        }

                        if(a.HasFlag(FaceAdjacency.Left)){
                            MergeCells(FaceAdjacency.Left, y, z, x, SizeY, SizeZ, out var w, out var h);
                            buffer.Face(x,y,z,FaceDirection.Left, 1, w, h);
                        }

                        if(a.HasFlag(FaceAdjacency.Right)){
                            MergeCells(FaceAdjacency.Right, y, z, x, SizeY, SizeZ, out var w, out var h);
                            buffer.Face(x,y,z,FaceDirection.Right, 1, w, h);
                        }
                    }
                }
            }
        }
    }

    public class VoxLoader : IVoxLoader
    {
        private VoxelBuffer buffer;

        public VoxLoader()
        {
            this.buffer = new VoxelBuffer();
        }

        private void Iterate(int sx, int sy, int sz, Action<int,int,int> action){
            for (int z = 0; z < sz; z++)
            {
                for (int y = 0; y < sy; y++)
                {
                    for (int x = 0; x < sx; x++)
                    {
                        action(x,y,z);
                    }
                }
            }
        }

        

        public void LoadModel(int sizeX, int sizeY, int sizeZ, byte[,,] data)
        {
            var m = new VoxelMesh(data);
            m.WriteTo(buffer);
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
            return buffer.ToPrimitive();
        }
    }
}