using System.Numerics;
using Xunit;

namespace XPlat.Core.Tests;

public class TransformTests
{
    [Fact]
    public async void Test1()
    {
        var t = new Transform3d();
        t.RotateDeg(new Vector3(1,2,3));
        var vec = t.RotationDeg;
        Assert.Equal(new Vector3(1,2,3), vec);
    }

    [Fact]
    public async void TransformCreatesFromMat4x4(){
        var t = new Transform3d();
        t.Translation = new Vector3(1,2,3);
        t.Scale = new Vector3(1,2,3);
        t.RotationQuat = Quaternion.CreateFromYawPitchRoll(1,2,3);

        var mat = t.GetMatrix();

        var sut = new Transform3d(mat);

        Assert.Equal(t.Translation, sut.Translation);
        Assert.Equal(t.Scale, sut.Scale);
        Assert.Equal(t.RotationQuat, sut.RotationQuat);

    }
}
