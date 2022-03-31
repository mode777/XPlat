namespace XPlat.Core
{
    public class GlTextureHandle : NativeHandle<uint>
    {
        public GlTextureHandle(uint handle) : base(handle) {}

        protected override void Delete()
        {
            UnmanagedQueue.DeleteTextures.Enqueue(Handle);
        }
    }
}