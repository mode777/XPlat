using System.Reflection;
using XPlat.Engine.Serialization;

namespace XPlat.Engine;

public class TypeRegistry 
{
    public Dictionary<string, Type> SceneElements { get; } = new Dictionary<string, Type>();
    public Dictionary<string, Type> SceneTemplates { get; } = new Dictionary<string, Type>();
    public Dictionary<string, Type> Resources { get; } = new Dictionary<string, Type>();

    public void LoadElementsFromAssembly(Assembly asm)
    {
        var pairs = AssemblyScanner.FindTypes<ISceneElement>(asm)
            .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneElementAttribute>()?.Name ?? x.Name, x));

        foreach (var kv in pairs)
        {
            SceneElements.Add(kv.Key, kv.Value);
        }

        var templates = AssemblyScanner.FindTypes<SceneConfiguration>(asm)
            .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneTemplateAttribute>()?.Name ?? x.Name, x));

        foreach (var kv in templates)
        {
            SceneTemplates.Add(kv.Key, kv.Value);
            
        }

        var resources = AssemblyScanner.FindTypes<IResource>(asm)
            .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneElementAttribute>()?.Name ?? x.Name, x));

        foreach (var kv in resources)
        {
            Resources.Add(kv.Key, kv.Value);
            
        }
    }

    public void LoadElementsFromAssembly(string name){
        var asm = AssemblyScanner.LoadAssembly(name);
        LoadElementsFromAssembly(asm);
    }



}
