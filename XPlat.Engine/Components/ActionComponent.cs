using System.Xml.Linq;
using XPlat.Engine.Serialization;

namespace XPlat.Engine.Components
{


    public class ActionComponent : Behaviour
    {
        public Action<ActionComponent>? InitAction { get; set; }
        public Action<ActionComponent>? UpdateAction { get; set; }

        public ActionComponent() { }
        public ActionComponent(Action<ActionComponent>? initAction, Action<ActionComponent>? updateAction)
        {
            InitAction = initAction;
            UpdateAction = updateAction;
        }

        public override void Init() => this.InitAction?.Invoke(this);
        public override void Update() => this.UpdateAction?.Invoke(this);

        public override void Parse(XElement el, SceneReader reader)
        {
            throw new NotSupportedException("Action Component does not support serialization");
            
        }
    }
}

