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
        private bool reload;

        public override void Init()
        {
            Script = Node.Scene.LuaHost.CreateScript();
            Script.OnError += (s, e) => Console.WriteLine(e.Message);
            if(Source != null) 
            {
                LoadScript();
                if(Watch){
                    SetupWatcher();
                }
            }
        }

        private void SetupWatcher(){
            var path = Path.GetDirectoryName(Source);
            var file = Path.GetFileName(Source);
            watcher = new FileSystemWatcher(path);
            watcher.Filter = file;
            watcher.NotifyFilter = NotifyFilters.LastWrite
                | NotifyFilters.LastAccess
                | NotifyFilters.Attributes
                | NotifyFilters.Size
                | NotifyFilters.CreationTime
                | NotifyFilters.DirectoryName
                | NotifyFilters.FileName
                | NotifyFilters.Security;
            watcher.Changed += (s, args) =>
            {
                reload = true;
            };
            watcher.EnableRaisingEvents = true;
        }

        private void LoadScript(){
            try
            {
                using (var fs = new FileStream(Source, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                    Script.Load(sr.ReadToEnd(), this.Node);

                reload = false;
                Script.Init();
               
            } catch(IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) Source = reader.ResolvePath(src);
            if(el.TryGetAttribute("watch", out var watch)) Watch = bool.Parse(watch);
            base.Parse(el, reader);
        }

        public override void Update()
        {
            if (reload) LoadScript();
            Script.Update();
        }
    }
}

