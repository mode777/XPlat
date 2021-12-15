using GLES2;
using net6test.NanoGuiPort;
using NanoVGDotNet;
using System.Drawing;
using System.Numerics;
using Microsoft.Extensions.Logging;
using static SDL2.SDL;

namespace net6test.samples
{
    public class NanoGuiPortDemo : Screen
    {
        private readonly ILogger<NanoGuiPortDemo> logger;
        private readonly ISdlPlatformEvents events;

        public NanoGuiPortDemo(ILogger<NanoGuiPortDemo> logger, ISdlPlatformEvents events, IPlatform info) : base(info, events)
        {
            this.logger = logger;
            this.events = events;

            events.Subscribe(SDL_EventType.SDL_MOUSEMOTION, MouseMove);

            var window = new Window(this, "Button demo");
              window.Position = new Vector2(15,15);
              //window.Layout = new GroupLayout();

              //new Label(window, "Push buttons", "sans-bold");

            PerformLayout();
        }

        public void MouseMove(SDL_EventType type, ref SDL_Event ev){
            logger.LogInformation($"Mouse move dx: {ev.motion.xrel} dy: {ev.motion.yrel}");
        }

    }
}