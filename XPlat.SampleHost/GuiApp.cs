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
        private readonly IPlatform platform;
        private NVGcontext vg;
        private Stack stack;

        public GuiApp(IPlatform platform)
        {
            this.platform = platform;
        }

        public void Init()
        {
            this.vg = NVGcontext.CreateGl(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES);

            this.stack = new Stack{Direction = Direction.Vertical};
            
            var stack2 = new Stack(stack){Direction = Direction.Horizontal};
            new Label(stack2){Icon = Icon.FA_STAR};
            var stack3 = new Stack(stack2){Direction = Direction.Vertical};
            new Label(stack3){Text = "My Label"};
            var stack4 = new Stack(stack3){Direction = Direction.Horizontal};

            new Label(stack4){Icon = Icon.FA_CUBE};
            new Label(stack4){Text = "My Larger Label"};
        }

        public void Update()
        {
            stack.PerformLayout(vg);

            Render();
        }

        public void Render()
        {
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);

            vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);

            stack.Draw(vg);

            vg.EndFrame();
        }
    }
}
