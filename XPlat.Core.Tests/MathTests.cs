using Xunit;

namespace XPlat.Core.Tests;

public class MathTests
{
    [Fact]
    public void Qua()
    {
        var str = Resource.LoadResourceString<ResourceTests>("HelloWorld.txt");
        Assert.Equal("Hello World", str);
    }
}
