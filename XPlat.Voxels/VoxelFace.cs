using System.Drawing;

namespace XPlat.Voxels
{
    public struct VoxelFace {
        VoxelVertex A;
        VoxelVertex B;
        VoxelVertex C;
        VoxelVertex D;

        public VoxelFace(int x, int y, int z, FaceDirection dir, int w, int h, int tx, int ty){
            switch (dir)
            {
                case FaceDirection.Front:
                A = new VoxelVertex(x,   y,   z+1, dir, tx, ty);
                B = new VoxelVertex(x+w, y,   z+1, dir, tx+w, ty);
                C = new VoxelVertex(x+w, y+h, z+1, dir, tx+w, ty+h);
                D = new VoxelVertex(x,   y+h, z+1, dir, tx, ty+h);
                break;
                case FaceDirection.Back:
                A = new VoxelVertex(x,   y,   z  , dir, tx, ty);
                B = new VoxelVertex(x  , y+h, z  , dir, tx, ty+h);
                C = new VoxelVertex(x+w, y+h, z  , dir, tx+w, ty+h);
                D = new VoxelVertex(x+w, y,   z  , dir, tx+w, ty);
                break;
                case FaceDirection.Top:
                A = new VoxelVertex(x,   y+1, z  , dir, tx, ty);
                B = new VoxelVertex(x  , y+1, z+h, dir, tx, ty+h);
                C = new VoxelVertex(x+w, y+1, z+h, dir, tx+w, ty+h);
                D = new VoxelVertex(x+w, y+1, z  , dir, tx+w, ty);
                break;
                case FaceDirection.Bottom:
                A = new VoxelVertex(x,   y,   z  , dir, tx, ty);
                B = new VoxelVertex(x+w, y,   z  , dir, tx+w, ty);
                C = new VoxelVertex(x+w, y,   z+h, dir, tx+w, ty+h);
                D = new VoxelVertex(x,   y,   z+h, dir, tx, ty+h);
                break;
                case FaceDirection.Right:
                A = new VoxelVertex(x+1, y,   z  , dir, tx, ty);
                B = new VoxelVertex(x+1, y+w, z  , dir, tx+w, ty);
                C = new VoxelVertex(x+1, y+w, z+h, dir, tx+w, ty+h);
                D = new VoxelVertex(x+1, y,   z+h, dir, tx, ty+h);
                break;
                case FaceDirection.Left:
                A = new VoxelVertex(x,   y,   z  , dir, tx, ty);
                B = new VoxelVertex(x,   y,   z+h, dir, tx, ty+h);
                C = new VoxelVertex(x,   y+w, z+h, dir, tx+w, ty+h);
                D = new VoxelVertex(x,   y+w, z,   dir, tx+w, ty);
                break;
                default:
                throw new InvalidOperationException();
            }

        }
    }
}