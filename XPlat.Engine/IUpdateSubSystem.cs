namespace XPlat.Engine
{
    public interface IUpdateSubSystem
    {
        void BeforeUpdate();
        void AfterUpdate();
        void OnUpdate(Node n);
    }
}