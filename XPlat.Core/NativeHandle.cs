using System;

namespace XPlat.Core
{

    public abstract class NativeHandle<T> : IDisposable
    {
        private readonly T _handle;
        public T Handle => !disposedValue ? _handle : throw new ObjectDisposedException("Native handle has been disposed");

        public NativeHandle(T handle)
        {
            _handle = handle;
        }
        private bool disposedValue;

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Delete();
                disposedValue = true;
            }
        }

        protected abstract void Delete();

        ~NativeHandle()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}