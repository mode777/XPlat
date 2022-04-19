namespace XPlat.Engine
{
    public interface IRenderPass
    {
        void OnAttach(Scene scene);
        void StartFrame();
        void FinishFrame();
        void OnRender(Node n);
    }
}