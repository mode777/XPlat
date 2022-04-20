using System.Numerics;
using Xunit;

namespace XPlat.Core.Tests;

public class TransformTests
{
    [Fact]
    public async void Test1()
    {
        var t = new Transform3d();
        t.SetRotationDeg(new Vector3(1,2,3));
        var vec = t.RotationDeg;
        Assert.Equal(new Vector3(1,2,3), vec);
    }

    [Fact]
    public async void TransformCreatesFromMat4x4(){
        var t = new Transform3d();
        t.TranslationVector = new Vector3(1,2,3);
        t.ScaleVector = new Vector3(1,2,3);
        t.RotationQuat = Quaternion.CreateFromYawPitchRoll(1,2,3);

        var mat = t.GetMatrix();

        var sut = new Transform3d(mat);

        Assert.Equal(t.TranslationVector, sut.TranslationVector);
        Assert.Equal(t.ScaleVector, sut.ScaleVector);
        Assert.Equal(t.RotationQuat, sut.RotationQuat);

    }
}
