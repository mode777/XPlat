namespace XPlat.Voxels
{
    // 4-bit: sign (0=-1, 1=1 -> x*2-1)|z|y|x
    public enum FaceDirection : int {
        Front = 0b1100,
        Back = 0b0100,
        Top = 0b1010,
        Bottom = 0b0010,
        Right = 0b1001,
        Left = 0b0001
    }

    [Flags]
    public enum FaceAdjacency : byte {
        None = 0,
        Front = 1,
        Back = 1 << 1, 
        Top = 1 << 2, 
        Bottom = 1 << 3, 
        Right = 1 << 4, 
        Left = 1 << 5, 
    }
}