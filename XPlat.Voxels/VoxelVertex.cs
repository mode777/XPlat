using System.Numerics;
using XPlat.Graphics;

namespace XPlat.Voxels
{
    public struct VoxelVertex {
        public VoxelVertex(int x, int y, int z, FaceDirection dir, int u = 0, int v = 0){
            Position = new Vector3(x,y,z);
            switch (dir)
            {
                case FaceDirection.Front:
                    Normal = new Vector3(0,0,1);
                    break;
                case FaceDirection.Back:
                    Normal = new Vector3(0,0,-1);
                    break;
                case FaceDirection.Top:
                    Normal = new Vector3(0,1,0);
                    break;
                case FaceDirection.Bottom:
                    Normal = new Vector3(0,-1,0);
                    break;
                case FaceDirection.Right:
                    Normal = new Vector3(1,0,0);
                    break;
                case FaceDirection.Left:
                    Normal = new Vector3(-1,0,0);
                    break;
                default:
                    Normal = Vector3.Zero;
                    break;
            }
            Uv = new Vector2(u,v);
        }

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 Uv;

    
    }
}