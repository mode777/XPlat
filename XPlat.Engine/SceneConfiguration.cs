namespace XPlat.Engine
{
    public class SceneConfiguration {

        private List<IRenderPass> _renderPasses = new();
        private List<IInitSubSystem> _initSubsystems = new();
        private List<IUpdateSubSystem> _updateSubsystems = new();

        public void Apply(Scene scene){
            foreach (var i in _initSubsystems)
                scene.RegisterInitSubsystem(i);
            
            foreach (var u in _updateSubsystems)
                scene.RegisterUpdateSubsystem(u);

            foreach (var r in _renderPasses)
                scene.RegisterRenderPass(r);
        }

        public void AddRenderPass(IRenderPass pass, int index = -1){
            _renderPasses.Insert(index == -1 ? _renderPasses.Count : index, pass);
        }

        public void AddInitSystem(IInitSubSystem iss, int index = -1){
            _initSubsystems.Insert(index == -1 ? _initSubsystems.Count : index, iss);
        }

        public void AddUpdateSystem(IUpdateSubSystem uss, int index = -1){
            _updateSubsystems.Insert(index == -1 ? _updateSubsystems.Count : index, uss);
        }
    }
}