using System.ComponentModel;
using XPlat.Core;

namespace Gwen.Net.OpenTk
{
    public static class GwenGuiFactory
    {
        public static IGwenGui CreateFromGame(XPlat.Core.IPlatform platform, ISdlPlatformEvents sdlPlatform, GwenGuiSettings settings = default)
        {
            if (settings == null)
            {
                settings = GwenGuiSettings.Default;
            }

            return new GwenGui(platform, sdlPlatform, settings);
        }
    }
}
