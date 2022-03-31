namespace XPlat.Core
{
    public class GlBufferHandle : NativeHandle<uint>
    {
        public GlBufferHandle(uint handle) : base(handle) {}

        protected override void Delete()
        {
            UnmanagedQueue.DeleteBuffers.Enqueue(Handle);
        }
    }
}