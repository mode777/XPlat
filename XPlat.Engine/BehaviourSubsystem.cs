using XPlat.Engine.Components;

namespace XPlat.Engine
{
    public class BehaviourSubsystem : IInitSubSystem, IUpdateSubSystem
    {
        public void AfterInit(){}

        public void AfterUpdate(){}

        public void BeforeInit(){}

        public void BeforeUpdate(){}

        public void OnInit(Node n)
        {
            foreach (var comp in n.Components)
            {
                if (comp is Behaviour b && b.IsEnabled) b.Init();
            }
        }

        public void OnUpdate(Node n)
        {
            foreach (var comp in n.Components)
            {
                if (comp is Behaviour b && b.IsEnabled) b.Update();
            }
        }
    }
}