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

        public override Component Clone(){
            var c = base.Clone() as Behaviour;
            c.WasInitialized = false;
            return c;
        }
    }
}

