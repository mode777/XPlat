namespace XPlat.Core
{
    public class GlFramebufferHandle : NativeHandle<uint>
    {
        public GlFramebufferHandle(uint handle) : base(handle) { }

        protected override void Delete()
        {
            UnmanagedQueue.DeleteFramebuffers.Enqueue(Handle);
        }
    }
}