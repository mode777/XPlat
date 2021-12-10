namespace net6test
{
    public static class Time
    {
        public static float RunningTime => SDL2.SDL.SDL_GetTicks() / 1000f;
    }
}