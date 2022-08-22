using Ink.Runtime;
using Ink;
using XPlat.Engine;
using System.Xml.Linq;
using XPlat.Engine.Serialization;

namespace XPlat.Ink
{
    [SceneElement("ink")]
    public class InkResource : FileResource, ISerializableResource
    {
        public Story? Story => Value as Story;
        protected override object LoadFile()
        {
            var inkSource = File.ReadAllText(Filename);

            var compiler =  new Compiler(inkSource, new Compiler.Options
            {   
                sourceFilename = Filename,
                errorHandler = OnError 
            });

            var story = compiler.Compile();
            story.onError += OnError;

            return story;
        }

        public void Parse(XElement el, SceneReader reader)
        {
            if(el.TryGetAttribute("src", out var src)) { Filename = reader.ResolvePath(src); Load(); }
            if(el.TryGetAttribute("watch", out var value) && bool.TryParse(value, out var watch) && watch) { Watch(); }
        }

        private void OnError(string message, ErrorType type)
        {
            throw new Exception(message);
        }
    }
}