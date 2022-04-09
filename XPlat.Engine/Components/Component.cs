using System.Xml.Linq;
using XPlat.Engine.Serialization;

namespace XPlat.Engine.Components
{
    public abstract class Component : ISceneElement, IDisposable
    {
        private bool disposedValue;

        public Node? Node { get; set; }
        public Scene Scene => Node.Scene;

        public virtual void Parse(XElement el, SceneReader reader) {
            // TODO: Implement default parser
        }

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    DisposeResources();
                }
                disposedValue = true;
            }
        }

        protected virtual void DisposeResources(){
            // Override to dispose resources
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

