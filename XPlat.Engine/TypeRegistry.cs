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
        var pairs = asm.ExportedTypes
            .Where(x => typeof(ISceneElement).IsAssignableFrom(x))
            .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneElementAttribute>()?.Name ?? x.Name, x));

        foreach (var kv in pairs)
        {
            SceneElements.Add(kv.Key, kv.Value);
        }

        var templates = asm.ExportedTypes
            .Where(x => typeof(SceneConfiguration).IsAssignableFrom(x))
            .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneTemplateAttribute>()?.Name ?? x.Name, x));

        foreach (var kv in templates)
        {
            SceneTemplates.Add(kv.Key, kv.Value);
            
        }

        var resources = asm.ExportedTypes
            .Where(x => typeof(IResource).IsAssignableFrom(x))
            .Select(x => new KeyValuePair<string, Type>(x.GetCustomAttribute<SceneElementAttribute>()?.Name ?? x.Name, x));

        foreach (var kv in resources)
        {
            Resources.Add(kv.Key, kv.Value);
            
        }
    }

    public void LoadElementsFromAssembly(string name){
        var asm = LoadAssembly(name);
        LoadElementsFromAssembly(asm);
    }

    private Assembly LoadAssembly(string name){
        var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == name);
        if(asm != null) return asm;
        return Assembly.Load(name) ?? throw new InvalidOperationException($"Could not load file or assembly '{name}'");
    }

}
