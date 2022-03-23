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

            this.stack = new Stack
            {
                Direction = Direction.Vertical
            };

            new Label(stack)
            {
                Text = "My Label"
            };

            new Label(stack)
            {
                Text = "My Larger Label"
            };
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

    public class Stack : Widget
    {
        public Stack(Widget? parent = null) : base(parent)
        {
        }

        public Direction Direction { get; set; }

        public override Vector2 PreferredSize(NVGcontext ctx)
        {
            var expand = Direction == Direction.Horizontal ? 0 : 1;
            var stack = Direction == Direction.Horizontal ? 1 : 0;

            var size = Vector2.Zero;
            foreach (var c in Children)
            {
                var csize = c.PreferredSize(ctx);
                size.Component(stack, Math.Max(size.Component(stack), csize.Component(stack)));
                size.Component(expand, size.Component(expand) + csize.Component(expand));
            }

            return size;
        }

        public override void PerformLayout(NVGcontext ctx)
        {
            var pref = PreferredSize(ctx);
            var fix = FixedSize;
            Size = new Vector2(
                fix.X != 0 ? fix.X : pref.X,
                fix.Y != 0 ? fix.Y : pref.Y);

            var expand = Direction == Direction.Horizontal ? 0 : 1;

            var pos = Vector2.Zero;
            foreach (var c in Children)
            {
                var csize = c.PreferredSize(ctx);
                c.Position = pos;
                pos.Component(expand, pos.Component(expand) + csize.Component(expand));
                c.PerformLayout(ctx);
            }
        }


    }

    public enum Direction
    {
        Vertical,
        Horizontal,
    }

    public class Label : Widget
    {
        public Label(Widget? parent = null) : base(parent)
        {
        }

        public string Text { get; set; }

        public override Vector2 PreferredSize(NVGcontext vg)
        {
            if (string.IsNullOrEmpty(Text)) return Vector2.Zero;

            vg.FontFace("sans");
            vg.FontSize(18);
            var bounds = new float[4];
            if (FixedSize != Vector2.Zero)
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                vg.TextBoxBounds(Position.X, Position.Y, FixedSize.X, Text, bounds);
                return new Vector2(FixedSize.X, bounds[3] - bounds[1]);
            }
            else
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.TextBounds(0, 0, Text, bounds);
                return new Vector2(bounds[2] - bounds[0] + 2, bounds[3] - bounds[1]);
            }
        }

        public override void Draw(NVGcontext vg)
        {
            base.Draw(vg);
            vg.FontFace("sans");
            vg.FontSize(18);
            vg.FillColor("#000");
            if (FixedSize != Vector2.Zero)
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                vg.TextBox(Position.X, Position.Y, FixedSize.X, Text);
            }
            else
            {
                vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_MIDDLE);
                vg.Text(Position.X, Position.Y + Size.Y * 0.5f, Text);
            }
        }


    }
}
