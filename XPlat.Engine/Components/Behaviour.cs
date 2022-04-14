namespace XPlat.Engine.Components
{
    public abstract class Behaviour : Component
    {

        public virtual void Init() {

        } 

        public virtual void Update(){

        }

        public virtual void OnCollision(CollisionInfo info){

        }
    }
}

