using System.Xml.Linq;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine.Components
{
    [SceneElement("lua")]
    public class LuaScriptComponent : Behaviour
    {
        public LuaScript Script { get; private set; }
        public bool Watch { get; set; } = false;
        public string Source { get; set; }
        private FileSystemWatcher watcher;

        public override void Init()
        {
            Script = Node.Scene.LuaHost.CreateScript();
            if(Source != null) 
            {
                LoadScript();
                if(Watch){
                    SetupWatcher();
                }
            }
        }

        private void SetupWatcher(){
            this.watcher = new FileSystemWatcher(Path.GetDirectoryName(Source));
            watcher.Filter = Path.GetFileName(Source);
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Changed += (s, args) => LoadScript();
            watcher.EnableRaisingEvents = true;
        }

        private void LoadScript(){
            Script.Load(File.ReadAllText(Source), this.Node);
            Script.Init();
        }

        public override void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) Source = reader.ResolvePath(src);
            if(el.TryGetAttribute("watch", out var watch)) Watch = bool.Parse(watch);
            base.Parse(el, reader);
        }

        public override void Update()
        {
            Script.Update();
        }
    }
}

