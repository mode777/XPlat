// See https://aka.ms/new-console-template for more information
using GLES2;
using XPlat.Core;

public class MinimalApp : ISdlApp
{
    public void Init()
    {
    }

    public void Update()
    {
        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT);
    }
}
