using System;
using SDL2;
using GLES2;
using net6test;
using net6test.samples;

namespace net_gles2
{
    class Program
    {
        static void Main(string[] args)
        {
            new SdlHost(new Nvg()).Run();

        }
    }
}
