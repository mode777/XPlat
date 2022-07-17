namespace XPlat.Engine
{
    public class SceneConfiguration
    {
        public IServiceProvider Services { get; }
        public SceneConfiguration(IServiceProvider services)
        {
            this.Services = services;

        }

        private List<IRenderPass> _renderPasses = new();
        private List<ISubSystem> _subsystems = new();

        public void Apply(Scene scene)
        {
            foreach (var u in _subsystems)
                scene.RegisterSubsystem(u);

            foreach (var r in _renderPasses)
                scene.RegisterRenderPass(r);
        }

        public void AddRenderPass(IRenderPass pass, int index = -1)
        {
            _renderPasses.Insert(index == -1 ? _renderPasses.Count : index, pass);
        }

        public void AddSubSystem(ISubSystem uss, int index = -1)
        {
            _subsystems.Insert(index == -1 ? _subsystems.Count : index, uss);
        }
    }
}