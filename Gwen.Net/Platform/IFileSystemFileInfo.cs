namespace Gwen.Net.Platform
{
    public interface IFileSystemFileInfo : IFileSystemItemInfo
    {
        string FormattedFileLength { get; }
    }
}