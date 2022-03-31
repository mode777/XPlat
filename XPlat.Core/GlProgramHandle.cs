namespace XPlat.Core
{
    public class GlProgramHandle : NativeHandle<uint>
    {
        public GlProgramHandle(uint handle) : base(handle) {}

        protected override void Delete()
        {
            UnmanagedQueue.DeletePrograms.Enqueue(Handle);
        }
    }
}