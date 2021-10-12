namespace net6test
{
    public class ActionComponent : Component
    {
        public Action<ActionComponent>? InitAction { get; set; }
        public Action<ActionComponent>? UpdateAction { get; set; }

        public ActionComponent(){}
        public ActionComponent(Action<ActionComponent>? initAction, Action<ActionComponent>? updateAction)
        {
            InitAction = initAction;
            UpdateAction = updateAction;
        }

        public override void Init() => this.InitAction?.Invoke(this);
        public override void Update() => this.UpdateAction?.Invoke(this);
    }
}

