namespace XPlat.Engine
{
    public interface ISubSystem
    {
        void Init();
        void BeforeUpdate();
        void AfterUpdate();
        void OnUpdate(Node n);
    }
}