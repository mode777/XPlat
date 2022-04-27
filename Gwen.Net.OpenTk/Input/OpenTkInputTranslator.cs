using System;
using System.Runtime.InteropServices;
using Gwen.Net.Control;
using Gwen.Net.Input;
using XPlat.Core;
using static SDL2.SDL;

namespace Gwen.Net.OpenTk.Input
{
    public class OpenTkInputTranslator
    {
        private readonly Canvas canvas;
        private Point lastMousePosition;

        bool controlPressed = false;

        public OpenTkInputTranslator(Canvas canvas)
        {
            this.canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        }

        private GwenMappedKey TranslateKeyCode(Key key)
        {
            switch (key)
            {
                case Key.BACKSPACE: return GwenMappedKey.Backspace;
                case Key.RETURN: return GwenMappedKey.Return;
                case Key.RETURN2: return GwenMappedKey.Return;
                case Key.ESCAPE: return GwenMappedKey.Escape;
                case Key.TAB: return GwenMappedKey.Tab;
                case Key.SPACE: return GwenMappedKey.Space;
                case Key.UP: return GwenMappedKey.Up;
                case Key.DOWN: return GwenMappedKey.Down;
                case Key.LEFT: return GwenMappedKey.Left;
                case Key.RIGHT: return GwenMappedKey.Right;
                case Key.HOME: return GwenMappedKey.Home;
                case Key.END: return GwenMappedKey.End;
                case Key.DELETE: return GwenMappedKey.Delete;
                case Key.LCTRL:
                    controlPressed = true;
                    return GwenMappedKey.Control;
                case Key.LALT: return GwenMappedKey.Alt;
                case Key.LSHIFT: return GwenMappedKey.Shift;
                case Key.RCTRL: return GwenMappedKey.Control;
                case Key.RALT:
                    if (controlPressed)
                    {
                        canvas.Input_Key(GwenMappedKey.Control, false);
                    }
                    return GwenMappedKey.Alt;
                case Key.RSHIFT: return GwenMappedKey.Shift;

            }
            return GwenMappedKey.Invalid;
        }

        private static char TranslateChar(Key key)
        {
            if (key >= Key.A && key <= Key.Z)
                return (char)('a' + ((int)key - (int)Key.A));
            return ' ';
        }

        public void ProcessMouseButton(ref SDL_Event ev)
        {
            if (canvas is null)
                return;

            if (ev.button.button == SDL_BUTTON_LEFT)
                canvas.Input_MouseButton(0, ev.button.state == SDL_PRESSED);
            else if (ev.button.button == SDL_BUTTON_RIGHT)
                canvas.Input_MouseButton(1, ev.button.state == SDL_PRESSED);
        }

        public void ProcessMouseMove(ref SDL_Event ev)
        {
            if (null == canvas)
                return;

            var deltaPosition = new Point(ev.motion.xrel, ev.motion.yrel);
            lastMousePosition = new Point(ev.motion.x, ev.motion.y);

            canvas.Input_MouseMoved((int)lastMousePosition.X, (int)lastMousePosition.Y, (int)deltaPosition.X, (int)deltaPosition.Y);
        }

        public void ProcessMouseWheel(ref SDL_Event ev)
        {
            if (null == canvas)
                return;

            canvas.Input_MouseWheel((int)(ev.wheel.y * 60));
        }

        public bool ProcessKeyDown(ref SDL_Event ev)
        {
            char ch = TranslateChar((Key)ev.key.keysym.sym);

            if (InputHandler.DoSpecialKeys(canvas, ch))
                return false;

            GwenMappedKey iKey = TranslateKeyCode((Key)ev.key.keysym.sym);
            if (iKey == GwenMappedKey.Invalid)
            {
                return false;
            }

            return canvas.Input_Key(iKey, true);
        }

        public void ProcessTextInput(ref SDL_Event obj)
        {
            unsafe
            {
                fixed(byte* p = obj.text.text)
                {
                    var str = Marshal.PtrToStringUTF8(new IntPtr(p));
                    foreach (char c in str)
                    {
                        canvas.Input_Character(c);
                    }
                }
            }
        }

        public bool ProcessKeyUp(ref SDL_Event ev)
        {
            GwenMappedKey key = TranslateKeyCode((Key)ev.key.keysym.sym);

            return canvas.Input_Key(key, false);
        }
    }
}