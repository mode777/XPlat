namespace XPlat.Engine
{
    public class EngineConfiguration
    {
        public bool Debug { get; set; }
        public string InitialScene { get; set; }
        public bool ThrowExceptions { get; set; }
        public string[] Import { get; set; }
    }
}