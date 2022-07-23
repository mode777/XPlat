using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Engine.Serialization;

namespace XPlat.Engine
{
    public class SceneResource : FileResource
    {
        public Scene Scene => Value as Scene;
        private readonly SceneReader reader;

        public SceneResource(SceneReader reader) : base()
        {
            this.reader = reader;
        }
        protected override object LoadFile()
        {
            if (Scene != null) Scene.Dispose();
            var scene = reader.Read(Filename);
            return scene;
        }

        public void Unload()
        {
            Scene?.Dispose();
            Value = null;
        }
    }
}