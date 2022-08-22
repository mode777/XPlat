using System.Reflection;

namespace XPlat.Engine;

public static class AssemblyScanner {
    public static IEnumerable<Type> FindTypes<T>(Assembly asm){
        return asm.ExportedTypes
            .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);
    }

    public static Assembly LoadAssembly(string name){
        var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.GetName().Name == name);
        if(asm != null) return asm;
        return Assembly.Load(name) ?? throw new InvalidOperationException($"Could not load file or assembly '{name}'");
    }
}
