// See https://aka.ms/new-console-template for more information
using System.Numerics;
using Microsoft.Extensions.DependencyInjection;
using XPlat.Core;
using XPlat.Engine;
using XPlat.Engine.Serialization;
using XPlat.SampleHost;

//XPlatApp.RunSdl<SpriteBatchApp>(args);
//XPlatApp.RunSdl<Tiny2cApp>(args);
//XPlatApp.RunSdl<EngineXmlApp>(args);
//XPlatApp.RunSdl<LightsApp>(args);
//XPlatApp.RunSdl<MinimalApp>(args);
//XPlatApp.RunSdl<GltfApp>(args);
//XPlatApp.RunSdl<GuiApp>(args);
//XPlatApp.RunSdl<LuaApp>(args);
//XPlatApp.RunSdl<SpriteAtlasApp>(args);
XPlatApp.RunSdl<EngineHost>(args, services => {
    services.AddTransient<SceneReader>();
});