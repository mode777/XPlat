namespace XPlat.Engine
{
    public interface IRenderPass
    {
        void StartFrame();
        void FinishFrame();
        void OnRender(Node n);
    }
}