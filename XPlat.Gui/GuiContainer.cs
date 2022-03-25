using System.Numerics;
using XPlat.Core;
using XPlat.NanoVg;
using static SDL2.SDL;

namespace XPlat.Gui
{
        public class GuiContainer
    {
        public IPlatform Platform { get; }
        private bool redraw = true;
        private float lastInteraction = Time.RunningTime;
        private List<Widget> focusPath = new List<Widget>();
        private readonly ISdlPlatformEvents events;
        private bool dragActive;
        private bool processEvents = true;
        private Widget? dragWidget;
        public event EventHandler<Vector2> OnResize;

        public Vector2 MousePos => Platform.MousePosition;

        protected NVGcontext nvgContext;

        public Vector2 FramebufferSize => Platform.RendererSize;
        public float PixelRatio => Platform.RetinaScale;

        public Vector2 Size => Platform.WindowSize;

        public Widget Root { get; set; }
        
        public GuiContainer(IPlatform platform, ISdlPlatformEvents events, NVGcontext vg)
        {
            this.Platform = platform;
            this.events = events;
            nvgContext = vg;

            Initialize();
        }

        private void Initialize()
        {
            RegisterEventCallbacks();

            /// Fixes retina display-related font rendering issue (#185)
            nvgContext.BeginFrame((int)Platform.WindowSize.X, (int)Platform.WindowSize.Y, PixelRatio);
            nvgContext.EndFrame();
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

        public virtual void PerformLayout(){
            Root.PerformLayout(nvgContext);
        }

        void Redraw()
        {
            if (!redraw)
            {
                redraw = true;
            }
        }

        public virtual void Draw()
        {
            //if (this.redraw)
            //{
                redraw = false;

                DrawWidgets();
            //}
        }

        internal void DrawWidgets()
        {
            var vg = nvgContext;

            vg.BeginFrame((int)Size.X, (int)Size.Y, PixelRatio);

            Root.Draw(vg);

            //DrawTooltip(vg);

            vg.EndFrame();
        }

        private bool KeyboardEvent(int keycode, int scancode, int action, bool repeat, int modifiers)
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

        private bool KeyboardCharacterEvent(uint codepoint)
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

        private bool ResizeEvent(Vector2 size)
        {
            OnResize?.Invoke(this, size);
            redraw = true;
            return true;
        }

        private void OnMouseMoveHandler(SDL_EventType type, ref SDL_Event ev)
        {
            var p = new Vector2(ev.motion.x, ev.motion.y);

            lastInteraction = Time.RunningTime;

            p -= new Vector2(1, 2);

            var ret = false;
            if (!dragActive)
            {
                var widget = Root.FindWidget(p);
                // if (widget != null && widget.Cursor != this.Cursor)
                // {
                //     this.Cursor = widget.Cursor;
                //     // TODO: Platform set cursor
                // }
            }
            else
            {
               ret = dragWidget.MouseDragEvent(p - dragWidget.Parent?.AbsolutePosition ?? Vector2.Zero, new Vector2(ev.motion.xrel, ev.motion.yrel), ev.motion.state, 0); 
            }

            if (!ret) ret = Root.MouseMotionEvent(p, new Vector2(ev.motion.xrel, ev.motion.yrel), ev.motion.state, 0);

            redraw = redraw ? true : ret;
        }

        private void OnMouseHandler(SDL_EventType type, ref SDL_Event ev)
        {
            lastInteraction = Time.RunningTime;

            // if(focusPath.Count > 1)
            // {
            //     var window = focusPath[focusPath.Count - 2] as Window;
            //     if (window != null && window.Modal)
            //     {
            //         if (!window.Contains(MousePos)) return;
            //     }
            // }

            var dropWidget = Root.FindWidget(MousePos);
            if(dragActive && type == SDL_EventType.SDL_MOUSEBUTTONUP && dropWidget != dragWidget)
            {
                var retWidget = dragWidget?.MouseButtonEvent(MousePos - dragWidget.Parent?.AbsolutePosition ?? Vector2.Zero, (int)ev.button.button, false, 0) ?? false;
                redraw = redraw ? true : retWidget;
            }

            // if(dropWidget != null && dropWidget.Cursor != Cursor)
            // {
            //     Cursor = dropWidget.Cursor;
            //     // TODO: Platform set cursor
            // }

            var btn12 = ev.button.button == SDL_BUTTON_LEFT || ev.button.button == SDL_BUTTON_RIGHT;

            if(!dragActive && type == SDL_EventType.SDL_MOUSEBUTTONDOWN && btn12)
            {
                dragWidget = Root.FindWidget(MousePos);
                //if(dragWidget == this) dragWidget = null;
                dragActive = dragWidget != null;
                //if (!dragActive) UpdateFocus(null);
            } else if (dragActive && type == SDL_EventType.SDL_MOUSEBUTTONUP && btn12)
            {
                dragActive = false;
                dragWidget = null;
            }

            var ret = Root.MouseButtonEvent(MousePos, ev.button.button, type == SDL_EventType.SDL_MOUSEBUTTONDOWN, 0);
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

            // if (focusPath.Count > 1)
            // {
            //     var window = focusPath[focusPath.Count - 2] as Window;
            //     if (window != null && window.Modal)
            //     {
            //         if (!window.Contains(MousePos)) return;
            //     }
            // }
            var ret = Root.ScrollEvent(MousePos, new Vector2(ev.wheel.x, ev.wheel.y));
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