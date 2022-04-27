namespace Gwen.Net.Platform
{
    public interface ISpecialFolder
    {
        string Name { get; }
        string Category { get; }
        string Path { get; }
    }
}