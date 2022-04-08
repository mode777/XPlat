namespace XPlat.Core
{
    public class GlRenderbufferHandle : NativeHandle<uint>
    {
        public GlRenderbufferHandle(uint handle) : base(handle) { }

        protected override void Delete()
        {
            UnmanagedQueue.DeleteRenderbuffers.Enqueue(Handle);
        }
    }
}