using GLES2;
using Gwen.Net;
using Gwen.Net.Control;
using Gwen.Net.Control.Internal;
using Gwen.Net.Control.Layout;
using Gwen.Net.OpenTk;
using Gwen.Net.Platform;
using Gwen.Net.Skin;
using Gwen.Net.Tests.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using XPlat.Core;
using XPlat.NanoVg;
using static SDL2.SDL;

namespace XPlat.SampleHost
{
    internal class GwenApp : ISdlApp
    {
        private const int MaxFrameSampleSize = 10000;

        private UnitTestHarnessControls unitTestControls;

        private readonly IGwenGui gui;
        private readonly Core.IPlatform platform;
        private readonly ISdlPlatformEvents events;

        public GwenApp(XPlat.Core.IPlatform platform, ISdlPlatformEvents events)
        {
            this.platform = platform;
            this.events = events;
            gui = GwenGuiFactory.CreateFromGame(platform, events, GwenGuiSettings.Default.From((settings) =>
            {
                //Have the skin come from somewhere else.
                //settings.SkinFile = new System.IO.FileInfo("assets/ui/DefaultSkin2.png");
            }));
            this.events.Subscribe(SDL2.SDL.SDL_EventType.SDL_WINDOWEVENT, (SDL_EventType t, ref SDL_Event e) => OnResize());
        }

        //protected override void Dispose(bool disposing)
        //{
        //    gui.Dispose();
        //    base.Dispose(disposing);
        //}

        protected void OnResize()
        {
            gui.Resize(new Size((int)platform.WindowSize.X, (int)platform.WindowSize.Y));
        }


        public void Init()
        {
            gui.Load();
            OnResize();
            unitTestControls = new UnitTestHarnessControls(gui.Root);
            //gui.Root.DrawDebugOutlines = true;
        }

        public void Update()
        {
            Render();
        }

        public void Render()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
            gui.Render();
        }
    }
}
