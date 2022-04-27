using System;
using Gwen.Net.Control;
using Gwen.Net.OpenTk.Exceptions;
using Gwen.Net.OpenTk.Input;
using Gwen.Net.OpenTk.Platform;
using Gwen.Net.OpenTk.Renderers;
using Gwen.Net.Platform;
using Gwen.Net.Renderer;
using Gwen.Net.Skin;
using XPlat.Core;
using XPlat.NanoVg;
using static SDL2.SDL;

namespace Gwen.Net.OpenTk
{
    internal class GwenGui : IGwenGui
    {
        private RendererBase renderer;
        private SkinBase skin;
        private Canvas canvas;
        private OpenTkInputTranslator input;
        private IntPtr cursor;

        public GwenGuiSettings Settings { get; }

        public XPlat.Core.IPlatform Platform { get; }
        public ISdlPlatformEvents PlatformEvents { get; }

        public ControlBase Root => canvas;

        internal GwenGui(XPlat.Core.IPlatform platform, ISdlPlatformEvents events, GwenGuiSettings settings)
        {
            Platform = platform;
            PlatformEvents = events;
            Settings = settings;
        }

        public void Load()
        {
            GwenPlatform.Init(new NetCorePlatform(SetCursor));
            AttachToWindowEvents();
            renderer = ResolveRenderer(Settings.Renderer);
            skin = new TexturedBase(renderer, "assets/ui/DefaultSkin2.png")
            {
                DefaultFont = new Font(renderer, "sans", 11)
            };
            canvas = new Canvas(skin);
            input = new OpenTkInputTranslator(canvas);

            canvas.SetSize((int)Platform.WindowSize.X, (int)Platform.WindowSize.Y);
            canvas.ShouldDrawBackground = true;
            canvas.BackgroundColor = skin.Colors.ModalBackground;
        }

        public void Render()
        {
            canvas.RenderCanvas();
        }

        public void Resize(Size size)
        {
            canvas.SetSize(size.Width, size.Height);
        }

        public void Dispose()
        {
            DetachWindowEvents();
            canvas.Dispose();
            skin.Dispose();
            renderer.Dispose();
        }

        private void AttachToWindowEvents()
        {
            PlatformEvents.Subscribe(SDL_EventType.SDL_KEYUP, Parent_KeyUp);
            PlatformEvents.Subscribe(SDL_EventType.SDL_KEYDOWN, Parent_KeyDown);
            PlatformEvents.Subscribe(SDL_EventType.SDL_TEXTINPUT, Parent_TextInput);
            PlatformEvents.Subscribe(SDL_EventType.SDL_MOUSEBUTTONDOWN, Parent_MouseDown);
            PlatformEvents.Subscribe(SDL_EventType.SDL_MOUSEBUTTONUP, Parent_MouseUp);
            PlatformEvents.Subscribe(SDL_EventType.SDL_MOUSEMOTION, Parent_MouseMove);
            PlatformEvents.Subscribe(SDL_EventType.SDL_MOUSEWHEEL, Parent_MouseWheel);
        }

        private void DetachWindowEvents()
        {
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_KEYUP, Parent_KeyUp);
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_KEYDOWN, Parent_KeyDown);
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_TEXTINPUT, Parent_TextInput);
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_MOUSEBUTTONDOWN, Parent_MouseDown);
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_MOUSEBUTTONUP, Parent_MouseUp);
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_MOUSEMOTION, Parent_MouseMove);
            PlatformEvents.Unsubscribe(SDL_EventType.SDL_MOUSEWHEEL, Parent_MouseWheel);
        }

        private void Parent_KeyUp(SDL_EventType t, ref SDL_Event e)
            => input.ProcessKeyUp(ref e);

        private void Parent_KeyDown(SDL_EventType t, ref SDL_Event e)
            => input.ProcessKeyDown(ref e);

        private void Parent_TextInput(SDL_EventType t, ref SDL_Event e)
            => input.ProcessTextInput(ref e);

        private void Parent_MouseDown(SDL_EventType t, ref SDL_Event e)
            => input.ProcessMouseButton(ref e);

        private void Parent_MouseUp(SDL_EventType t, ref SDL_Event e)
            => input.ProcessMouseButton(ref e);

        private void Parent_MouseMove(SDL_EventType t, ref SDL_Event e)
            => input.ProcessMouseMove(ref e);

        private void Parent_MouseWheel(SDL_EventType t, ref SDL_Event e)
            => input.ProcessMouseWheel(ref e);

        private void SetCursor(SDL_SystemCursor mouseCursor)
        {
            //first destroy old cursor object from memory
            SDL_FreeCursor(cursor);

            //based on type of cursor value passed, create mouse cursor using SDL ID flag value 
            cursor = SDL_CreateSystemCursor(mouseCursor);

            //use cursor pointer to assign cursor to SDL
            SDL_SetCursor(cursor);
        }

        private RendererBase ResolveRenderer(GwenGuiRenderer gwenGuiRenderer)
        {
            switch (gwenGuiRenderer)
            {
                case GwenGuiRenderer.NanoVg:
                    return new NanoVGRenderer(NVGcontext.CreateGl(), Platform);
                //case GwenGuiRenderer.GL20:
                //    return new OpenTKGL20Renderer();
                //case GwenGuiRenderer.GL40:
                //    return new OpenTKGL40Renderer();
                default:
                    throw new RendererNotFoundException(gwenGuiRenderer);
            };
        }
    }
}
