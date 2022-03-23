using System.Numerics;
using Xunit;

namespace XPlat.Core.Tests;

public class TransformTests
{
    [Fact]
    public async void Test1()
    {
        var t = new Transform3d();
        t.RotationDeg = new Vector3(1,2,3);
        var vec = t.RotationDeg;
        Assert.Equal(new Vector3(1,2,3), vec);
    }
}
