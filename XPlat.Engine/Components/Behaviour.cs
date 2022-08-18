namespace XPlat.Engine.Components
{
    public abstract class Behaviour : Component
    {
        public bool WasInitialized { get; internal set; }

        public virtual void Init() {

        } 

        public virtual void Update(){

        }

        public virtual void OnCollision(CollisionInfo info){

        }

        public virtual void OnMessage(object message){
            
        }

        public override Component Clone(Node n){
            var c = base.Clone(n) as Behaviour;
            c.WasInitialized = false;
            return c;
        }
    }
}

