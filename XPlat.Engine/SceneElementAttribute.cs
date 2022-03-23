namespace XPlat.Engine
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SceneElementAttribute : Attribute
    {
        public string Name { get; }
        public SceneElementAttribute(string name)
        {
            this.Name = name;
        }
    }
}

