namespace Gwen.Net.Platform
{
    public interface IFileSystemItemInfo
    {
        string Name { get; }
        string FullName { get; }
        string FormattedLastWriteTime { get; }
    }
}