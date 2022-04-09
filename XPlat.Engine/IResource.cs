namespace XPlat.Engine
{
    public interface IResource
    {
        string Id { get; }
        event EventHandler Changed;
        object Value { get; }
        T GetValue<T>();
    }
}