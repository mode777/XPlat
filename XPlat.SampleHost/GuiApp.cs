using GLES2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using XPlat.Core;
using XPlat.Gui;
using XPlat.NanoVg;

namespace XPlat.SampleHost
{
    internal class GuiApp : ISdlApp
    {
        private GuiContainer container;

        public GuiApp(IPlatform platform, ISdlPlatformEvents events)
        {
            var vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES);

            this.container = new GuiContainer(platform, events, vg);
        }

        public void Init()
        {
            var stack = new Stack{Direction = Direction.Vertical};
            
            var stack2 = new Stack(stack){Direction = Direction.Horizontal};
            new Label(stack2){Icon = Icon.FA_STAR};
            var stack3 = new Stack(stack2){Direction = Direction.Vertical};
            new Label(stack3){Text = "My Label"};
            var stack4 = new Stack(stack3){Direction = Direction.Horizontal};

            new Label(stack4){Icon = Icon.FA_CUBE};
            new Label(stack4){Text = "My Larger Label"};

            container.Root = stack;
        }

        public void Update()
        {
            container.PerformLayout();
            Render();
        }

        public void Render()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            container.Draw();
        }
    }
}
