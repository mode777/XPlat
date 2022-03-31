using GLES2;
using Microsoft.Extensions.Logging;
using SDL2;
using XPlat.Core;
using XPlat.LuaScripting;
using XPlat.NanoVg;

public class LuaApp : ISdlApp
{
    private LuaHost lua;
    private LuaScript script;
    private FileSystemWatcher watcher;
    private NVGcontext vg;
    private readonly IPlatform platform;
    private readonly ILogger<LuaApp> logger;

    public LuaApp(ISdlPlatformEvents events, IPlatform platform, ILogger<LuaApp> logger)
    {
        this.platform = platform;
        this.logger = logger;
        //events.Subscribe(SDL.SDL_EventType.SDL_KEYUP, OnKeyUp);
    }

    private void LoadScript(){
        script.Load(File.ReadAllText("assets/myscript.lua"), vg);
    }


    public void Init()
    {
        this.lua = new LuaHost();
        this.script = lua.CreateScript();
        this.script.OnError += (s,e) => logger.LogError(e.Message);
        this.watcher = new FileSystemWatcher("assets");
        watcher.Filter = "myscript.lua";
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Changed += (s, args) =>
        {
            LoadScript();
        };

        this.vg = NVGcontext.CreateGl();
        LoadScript();
        watcher.EnableRaisingEvents = true;
        
    }

    public void Update()
    {
        GL.ClearColor(1, 0, 0, 1);
        GL.Clear(GL.COLOR_BUFFER_BIT | GL.STENCIL_BUFFER_BIT);
        
        vg.BeginFrame((int)platform.WindowSize.X, (int)platform.WindowSize.Y, platform.RetinaScale);
        this.script.Update();
        vg.EndFrame();
    }
}
