using System.Xml.Linq;
using XPlat.Core;
using XPlat.Engine.Serialization;
using XPlat.LuaScripting;

namespace XPlat.Engine
{
    [SceneElement("scene")]
    public class Scene : ISceneElement, IDisposable
    {
        private bool disposedValue;

        public Scene()
        {
            RootNode = new Node();
            RootNode.Scene = this;
            SetupLua();
        }

        // private class LuaTime {
            
        // }

        private void SetupLua(){
            LuaHost = new LuaHost();
            LuaHost.ImportNamespace(nameof(XPlat)+ "." + nameof(Core));

            //LuaHost.SetGlobal("Time", Time.);
        }

        public Node FindNode(string name) => RootNode.Find(name);

        public Scene(Node rootNode) 
        {
            this.RootNode = rootNode;
        }

        public Node RootNode { get; private set; }
        public LuaHost LuaHost { get; private set; }

        public void Init() => RootNode.Init();
        public void Update() => RootNode.Update();

        public void Parse(XElement el, SceneReader reader)
        {
            foreach(var i in el.Elements("import")){
                reader.ReadElement(i);
            }
            var rootEl = el.Element("node") ?? throw new InvalidDataException("A scene needs a root note element");
            RootNode.Parse(rootEl, reader);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    RootNode.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}