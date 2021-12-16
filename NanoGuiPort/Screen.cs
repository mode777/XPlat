using System.Numerics;
using GLES2;
using NanoVGDotNet;
using static SDL2.SDL;

namespace net6test.NanoGuiPort
{
    // TODO: Implement IDisposable
    public class Screen : Widget, ISdlApp
    {
        public IPlatform Platform { get; }
        public NVGcolor Background;
        private bool redraw = true;
        private float lastInteraction = Time.RunningTime;
        private List<Widget> focusPath = new List<Widget>();
        private readonly ISdlPlatformEvents events;
        private bool dragActive;
        private bool processEvents = true;
        private Widget? dragWidget;
        public event EventHandler<Vector2> OnResize;

        public Vector2 MousePos => new Vector2(Platform.MousePosition.X, Platform.MousePosition.Y);

        protected NVGcontext nvgContext;

        public Vector2 FramebufferSize => new Vector2(Platform.RendererSize.Width, Platform.RendererSize.Height);
        public float PixelRatio => Platform.RetinaScale;
        
        public Screen(IPlatform platform, ISdlPlatformEvents events) : base(null)
        {
            this.Platform = platform;
            this.events = events;
            Background = "#555555";
            Size = new Vector2(platform.WindowSize.Width, platform.WindowSize.Height);
            nvgContext = new NVGcontext();

            Initialize();
        }

        private void Initialize()
        {
            GlNanoVG.nvgCreateGL(ref nvgContext, (int)NVGcreateFlags.NVG_ANTIALIAS |
                        (int)NVGcreateFlags.NVG_STENCIL_STROKES);

            var vg = nvgContext;
            vg.CreateFont("sans", "assets/Roboto-Regular.ttf");
            vg.CreateFont("sans-bold", "assets/Roboto-Bold.ttf");
            vg.CreateFont("icons", "assets/entypo.ttf");

            Theme = new Theme(nvgContext);

            RegisterEventCallbacks();

            /// Fixes retina display-related font rendering issue (#185)
            vg.BeginFrame((int)Size.X, (int)Size.Y, PixelRatio);
            vg.EndFrame();

            Platform.AutoSwap = false;
        }

        // TODO: Implement dispose
        public virtual void Init()
        {
        }

        private void RegisterEventCallbacks()
        {
            events.Subscribe(SDL_EventType.SDL_MOUSEMOTION, OnMouseMoveHandler);
            events.Subscribe(SDL_EventType.SDL_MOUSEBUTTONUP, OnMouseHandler);
            events.Subscribe(SDL_EventType.SDL_MOUSEBUTTONDOWN, OnMouseHandler);
            events.Subscribe(SDL_EventType.SDL_KEYDOWN, OnKeyHandler);
            events.Subscribe(SDL_EventType.SDL_KEYUP, OnKeyHandler);
            events.Subscribe(SDL_EventType.SDL_TEXTINPUT, OnTextHandler);
            events.Subscribe(SDL_EventType.SDL_DROPFILE, OnDropHandler);
            events.Subscribe(SDL_EventType.SDL_MOUSEWHEEL, OnScrollHandler);
            events.Subscribe(SDL_EventType.SDL_WINDOWEVENT, OnWindowHandler);
        }

        // TODO: Unsubscribe!

        public virtual void Update()
        {
            DrawAll();
        }

        internal void UpdateFocus(Widget? widget)
        {
            foreach (var w in focusPath)
            {
                if(!w.Focused) continue;
                w.FocusEvent(false);
            }
            focusPath.Clear();
            Window? window = null;
            while(widget != null){
                focusPath.Add(widget);
                if(widget is Window win){
                    window = win;
                }
                widget = widget.Parent;
            }
            foreach (var it in focusPath)
            {
                it.FocusEvent(true);
            }

            if(window != null)
                MoveWindowToFront(window);
        }

        public void MoveWindowToFront(Window window)
        {
            Children.Remove(window);
            Children.Add(window);
            var changed = false;
            do {
                var baseIndex = Children.IndexOf(window);
                changed = false;
                for (int i = 0; i < Children.Count; i++)
                {
                    var pw = Children[i] as Popup;
                    if(pw != null && pw.ParentWindow == window && i < baseIndex){
                        MoveWindowToFront(pw);
                        changed = true;
                        break;
                    }
                }
            } while(changed);
        }

        public void DisposeWindow(Window window){
            if(focusPath.Contains(window)) focusPath.Clear();
            if(dragWidget == window){
                dragWidget = null;
                dragActive = false;
            }
            RemoveChild(window);
        }

        public void CenterWindow(Window window){
            if(window.Size == Vector2.Zero){
                window.Size = window.PreferredSize(nvgContext);
                window.PerformLayout(nvgContext);
            }
            window.Position = Size - window.Size / 2f;
        }

        void Redraw()
        {
            if (!redraw)
            {
                redraw = true;
            }
        }

        public virtual void DrawAll()
        {
            //if (this.redraw)
            //{
                redraw = false;

                DrawSetup();
                DrawContents();
                DrawWidgets();
                DrawTeardown();
            //}
        }

        public virtual void Clear()
        {
            GL.ClearColor(Background.r, Background.g, Background.b, 1);
            GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
        }

        public virtual void DrawSetup()
        {
            Size.X = Platform.WindowSize.Width;
            Size.Y = Platform.WindowSize.Height;
        }

        public virtual void DrawContents()
        {
            Clear();
        }

        public virtual void DrawTeardown()
        {
            Platform.SwapBuffers();
        }

        internal void DrawWidgets()
        {
            var vg = nvgContext;

            vg.BeginFrame((int)Size.X, (int)Size.Y, PixelRatio);

            Draw(vg);

            DrawTooltip(vg);

            vg.EndFrame();
        }

        private void DrawTooltip(NVGcontext vg)
        {
            float elapsed = Time.RunningTime - this.lastInteraction;

            if (elapsed > 0.5f)
            {
                var widget = FindWidget(new Vector2(Platform.MousePosition.X, Platform.MousePosition.Y));
                if (widget != null && !string.IsNullOrEmpty(widget.Tooltip))
                {
                    float tooltipWidth = 150;

                    var bounds = new float[4];
                    vg.FontFace("sans");
                    vg.FontSize(15);
                    vg.TextAlign((int)NVGalign.NVG_ALIGN_LEFT | (int)NVGalign.NVG_ALIGN_TOP);
                    vg.TextLineHeight(1.1f);
                    var pos = widget.AbsolutePosition + new Vector2(widget.Width / 2.0f, widget.Height + 10);

                    vg.TextBounds(pos.X, pos.Y, widget.Tooltip, bounds);

                    float h = (bounds[2] - bounds[0]) / 2;
                    if (h > tooltipWidth / 2)
                    {
                        vg.TextAlign((int)NVGalign.NVG_ALIGN_CENTER | (int)NVGalign.NVG_ALIGN_TOP);
                        vg.TextBoxBounds(pos.X, pos.Y, tooltipWidth, widget.Tooltip, bounds);

                        h = (bounds[2] - bounds[0]) / 2;
                    }
                    float shift = 0;

                    if (pos.X - h - 8 < 0)
                    {
                        shift = pos.X - h - 8;
                        pos.X -= shift;
                        bounds[0] -= shift;
                        bounds[2] -= shift;
                    }

                    vg.GlobalAlpha(MathF.Min(1f, 2 * (elapsed - 0.5f)) * 0.8f);

                    vg.BeginPath();
                    vg.FillColor("#000000");
                    vg.RoundedRect(bounds[0] - 4 - h, bounds[1] - 4,
                           (bounds[2] - bounds[0]) + 8,
                           (bounds[3] - bounds[1]) + 8, 3);

                    float px = ((bounds[2] + bounds[0]) / 2) - h + shift;
                    vg.MoveTo(px, bounds[1] - 10);
                    vg.LineTo(px + 7, bounds[1] + 1);
                    vg.LineTo(px - 7, bounds[1] + 1);
                    vg.Fill();

                    vg.FillColor("#ffffff");
                    vg.FontBlur(0.0f);
                    vg.TextBox(pos.X - h, pos.Y, tooltipWidth,
                            widget.Tooltip);
                }
            }
        }

        public override bool KeyboardEvent(int keycode, int scancode, int action, bool repeat, int modifiers)
        {
            if (focusPath.Count > 0)
            {
                foreach (var it in focusPath)
                {
                    if (it.Focused && it.KeyboardEvent(keycode, scancode, action, repeat, modifiers))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public override bool KeyboardCharacterEvent(uint codepoint)
        {
            if (focusPath.Count > 0)
            {
                foreach (var it in focusPath)
                {
                    if (it.Focused && it.KeyboardCharacterEvent(codepoint))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual bool ResizeEvent(Vector2 size)
        {
            OnResize?.Invoke(this, size);
            redraw = true;
            return true;
        }

        public bool TooltipFadeInProgress()
        {
            var elapsed = Time.RunningTime - lastInteraction;
            if(elapsed < 0.25f || elapsed > 1.25f) return false;

            var widget = FindWidget(MousePos);
            return string.IsNullOrEmpty(widget?.Tooltip ?? string.Empty); 
        }

        public void PerformLayout()
        {
            this.PerformLayout(nvgContext);
        }

        private void OnMouseMoveHandler(SDL_EventType type, ref SDL_Event ev)
        {
            var p = new Vector2(ev.motion.x, ev.motion.y);

            lastInteraction = Time.RunningTime;

            p -= new Vector2(1, 2);

            var ret = false;
            if (!dragActive)
            {
                var widget = FindWidget(p);
                if (widget != null && widget.Cursor != this.Cursor)
                {
                    this.Cursor = widget.Cursor;
                    // TODO: Platform set cursor
                }
            }
            else
            {
               ret = dragWidget.MouseDragEvent(p - dragWidget.Parent?.AbsolutePosition ?? Vector2.Zero, new Vector2(ev.motion.xrel, ev.motion.yrel), ev.motion.state, 0); 
            }

            if (!ret) ret = MouseMotionEvent(p, p - MousePos, ev.motion.state, 0);

            redraw = redraw ? true : ret;
        }

        private void OnMouseHandler(SDL_EventType type, ref SDL_Event ev)
        {
            lastInteraction = Time.RunningTime;

            if(focusPath.Count > 1)
            {
                var window = focusPath[focusPath.Count - 2] as Window;
                if (window != null && window.Modal)
                {
                    if (!window.Contains(MousePos)) return;
                }
            }

            var dropWidget = FindWidget(MousePos);
            if(dragActive && type == SDL_EventType.SDL_MOUSEBUTTONUP && dropWidget != dragWidget)
            {
                var retWidget = dragWidget?.MouseButtonEvent(MousePos - dragWidget.Parent?.AbsolutePosition ?? Vector2.Zero, (int)ev.button.button, false, 0) ?? false;
                redraw = redraw ? true : retWidget;
            }

            if(dropWidget != null && dropWidget.Cursor != Cursor)
            {
                Cursor = dropWidget.Cursor;
                // TODO: Platform set cursor
            }

            var btn12 = ev.button.button == SDL_BUTTON_LEFT || ev.button.button == SDL_BUTTON_RIGHT;

            if(!dragActive && type == SDL_EventType.SDL_MOUSEBUTTONDOWN && btn12)
            {
                dragWidget = FindWidget(MousePos);
                if(dragWidget == this) dragWidget = null;
                dragActive = dragWidget != null;
                if (!dragActive) UpdateFocus(null);
            } else if (dragActive && type == SDL_EventType.SDL_MOUSEBUTTONUP && btn12)
            {
                dragActive = false;
                dragWidget = null;
            }

            var ret = MouseButtonEvent(MousePos, ev.button.button, type == SDL_EventType.SDL_MOUSEBUTTONDOWN, 0);
            redraw = redraw ? true : ret;
        }

        private void OnKeyHandler(SDL_EventType type, ref SDL_Event ev)
        {
            lastInteraction = Time.RunningTime;

            var ret = KeyboardEvent((int)ev.key.keysym.sym, (int)ev.key.keysym.scancode, ev.key.state, ev.key.repeat > 0, 0);
            redraw = redraw ? true : ret;
        }

        private void OnTextHandler(SDL_EventType type, ref SDL_Event ev)
        {
            lastInteraction = Time.RunningTime;

            // TODO: Implement Text Input https://wiki.libsdl.org/Tutorials-TextInput
            //var ret = KeyboardCharacterEvent(ev.text.text)
        }

        private void OnDropHandler(SDL_EventType type, ref SDL_Event ev)
        {
            // TODO: Implement drop
        }
        private void OnScrollHandler(SDL_EventType type, ref SDL_Event ev)
        {
            lastInteraction = Time.RunningTime;

            if (focusPath.Count > 1)
            {
                var window = focusPath[focusPath.Count - 2] as Window;
                if (window != null && window.Modal)
                {
                    if (!window.Contains(MousePos)) return;
                }
            }
            var ret = ScrollEvent(MousePos, new Vector2(ev.wheel.x, ev.wheel.y));
            redraw = redraw ? true : ret;
        }

        private void OnWindowHandler(SDL_EventType type, ref SDL_Event ev)
        {

            if (ev.window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_SIZE_CHANGED)
            {
                lastInteraction = Time.RunningTime;

                ResizeEvent(Size);

            }
            // else if (ev.window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_GAINED || 
            //     ev.window.windowEvent == SDL_WindowEventID.SDL_WINDOWEVENT_FOCUS_LOST)
            // {

            // }

        }
    }
}