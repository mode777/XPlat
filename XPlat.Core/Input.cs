using SDL2;

namespace XPlat.Core
{
    public static class Input
    {
        public static bool IsKeyDown(Key key){
            var scancode = SDL.SDL_GetScancodeFromKey((SDL.SDL_Keycode)key);
            unsafe {
                var arr = (byte*)SDL.SDL_GetKeyboardState(out var length);
                if((int)scancode >= length) return false;
                return arr[(int)scancode] > 0;
            }
        }
        public static int MouseX {
            get {
                SDL.SDL_GetMouseState(out var x, out var y);
                return x;
            }
        }
        public static int MouseY {
            get {
                SDL.SDL_GetMouseState(out var x, out var y);
                return y;
            }
        }

        public static bool IsMouseDown(MouseButton button){
            return ((int)SDL.SDL_GetMouseState(out var x, out var y) & (int)button) > 0;
        }
    }
}