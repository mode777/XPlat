using XPlat.Engine.Serialization;

namespace XPlat.Engine
{
    public interface IResource
    {
        string Id { get; set; }
        event EventHandler Changed;
        object Value { get; }
        T GetValue<T>();
    }

    public interface ISerializableResource : IResource, ISceneElement {}
}