namespace XPlat.Engine
{
    public class SceneTemplateAttribute : Attribute
    {
        public string Name { get; }
        public SceneTemplateAttribute(string name)
        {
            this.Name = name;

        }
    }
}