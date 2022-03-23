using Xunit;

namespace XPlat.Core.Tests;

public class ResourceTests
{
    [Fact]
    public void Test1()
    {
        var str = Resource.LoadResourceString<ResourceTests>("HelloWorld.txt");
        Assert.Equal("Hello World", str);
    }
}
