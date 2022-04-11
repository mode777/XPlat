using XPlat.Core;
using XPlat.Engine.Serialization;

namespace XPlat.Engine
{
    public class SceneResource : FileResource
    {
        public Scene Scene => Value as Scene;
        private readonly IServiceProvider services;

        public SceneResource(IServiceProvider services, string id, string file) : base(id, file)
        {
            this.services = services;
        }
        protected override object LoadFile()
        {
            if (Scene != null) Scene.Dispose();
            var scene = new SceneReader(services).Read(Filename);
            return scene;
        }
    }
}