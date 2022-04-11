namespace XPlat.Engine
{
    public interface IInitSubSystem {
        void BeforeInit();
        void AfterInit();
        void OnInit(Node n);
    }
}