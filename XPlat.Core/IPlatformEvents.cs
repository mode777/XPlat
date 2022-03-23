using SDL2;

namespace XPlat.Core
{
    public delegate void PlatformEventHandler<TEvent, TData>(TEvent ev, ref TData data);
    public interface IPlatformEvents<TEvent,TData>
    {
        event PlatformEventHandler<TEvent,TData> OnEvent;
        void Subscribe(TEvent ev, PlatformEventHandler<TEvent,TData> handler);
        void Unsubscribe(TEvent ev, PlatformEventHandler<TEvent,TData> handler);
    }

    public interface ISdlPlatformEvents : IPlatformEvents<SDL.SDL_EventType, SDL.SDL_Event> {

    }
}