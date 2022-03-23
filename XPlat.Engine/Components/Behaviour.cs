namespace XPlat.Engine.Components
{
    public abstract class Behaviour : Component
    {

        public bool IsEnabled { get; set; } = true;
        public virtual void Init() {

        } 

        public virtual void Update(){

        }
    }
}

