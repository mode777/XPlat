using XPlat.Engine.Components;
using XPlat.LuaScripting;

namespace XPlat.Engine
{

    public class BehaviourSubsystem : ISubSystem
    {
        public void AfterUpdate() { }

        public void BeforeUpdate() { }

        public void Init()
        {
        }

        public void OnUpdate(Node n)
        {
            var msgs = n.GetMessages();
            foreach (var comp in n.Components)
            {
                if (comp is Behaviour b)
                {
                    if (!b.WasInitialized)
                    {
                        b.Init();
                        b.WasInitialized = true;
                    }
                    foreach (var msg in msgs)
                    {
                        b.OnMessage(msg);
                    }
                    foreach (var c in n.Collisions)
                    {
                        b.OnCollision(c);
                    }
                    if (b.IsEnabled) b.Update();
                }
            }
            msgs.Clear();
        }
    }
}