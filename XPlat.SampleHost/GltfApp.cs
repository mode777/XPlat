using Microsoft.Extensions.Logging;
using XPlat.Core;
using XPlat.Gltf;

namespace XPlat.SampleHost
{
    public class GltfApp : ISdlApp
    {
        private readonly ILogger<GltfApp> logger;
        public GltfApp(ILogger<GltfApp> logger)
        {
            this.logger = logger;

        }

        public void Init()
        {
            var scene = GltfReader.Load("assets/test_scene.glb");
            scene.Dump((s) => Console.WriteLine(s));
            var l = scene.FindNode("/Light/Light_Orientation");
        }

        public void Update()
        {
        }
    }
}