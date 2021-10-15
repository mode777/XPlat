using System;
using SDL2;
using GLES2;
using net6test;
using net6test.samples;

namespace net_gles2
{
    class Program
    {
        public static MauiApp CreateMauiApp() =>
            MauiApp
                .CreateBuilder()
                .UseMauiApp<App>()
                .Build();

        public static void Main(string[] args)
        {
            CreateMauiApp().Run();
        }
    }
}



