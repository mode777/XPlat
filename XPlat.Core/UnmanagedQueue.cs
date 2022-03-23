using System.Collections.Concurrent;

namespace XPlat.Core {
    // This class will delete unmanaged resources. Pinvoke calls from finalizer thread will crash the program (at least on macos) as 
    // so we need to queue them and then call Purge from the main thread. 
    public static class UnmanagedQueue {
        public static readonly ConcurrentQueue<uint> DeleteBuffers = new ConcurrentQueue<uint>();
        public static readonly ConcurrentQueue<uint> DeleteTextures = new ConcurrentQueue<uint>();
        public static readonly ConcurrentQueue<uint> DeletePrograms = new ConcurrentQueue<uint>();
        
        public static void Purge(){
            while(DeleteBuffers.TryDequeue(out var res)) GlUtil.DeleteBuffer(res);
            while(DeleteTextures.TryDequeue(out var res)) GlUtil.DeleteTexture(res);
            while(DeletePrograms.TryDequeue(out var res)) GlUtil.DeleteProgram(res);
        }
    }
}