using SDL2;

namespace XPlat.Core
{
    public static class Input
    {
        public static bool IsKeyDown(SDL.SDL_Keycode key){
            var scancode = SDL.SDL_GetScancodeFromKey(key);
            unsafe {
                var arr = (byte*)SDL.SDL_GetKeyboardState(out var length);
                if((int)scancode >= length) return false;
                return arr[(int)scancode] > 0;
            }
        }
    }
}