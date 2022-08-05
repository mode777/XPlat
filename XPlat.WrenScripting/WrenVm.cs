//using IronWren;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace XPlat.WrenScripting;

public class WrenVm : IDisposable
{
    static void WriteStatic(IntPtr vm, string str) => System.Console.Write(str);
    static void ErrorStatic(IntPtr vm, WrenNative.WrenErrorType type, string module, int line, string message) => throw new WrenScriptException(vm, type, module, line, message);


    static WrenNative.WrenForeignClassMethods BindForeignClassStatic(IntPtr vm, string module, string className) => GetVm(vm).BindForeignClass(module, className);
    static WrenNative.WrenForeignMethodFn BindForeignMethodStatic(IntPtr vm, string module, string className, bool isStatic, string signature) => GetVm(vm).BindForeignMethod(module, className, isStatic, signature);
    static WrenNative.WrenLoadModuleResult LoadModuleStatic(IntPtr vm, string name) {
        var lmr = new WrenNative.WrenLoadModuleResult();
        
        return lmr;
    }

    static string ResolveModuleStatic(IntPtr vm, string importer, string name){
        return name;
    }
    
    private static Dictionary<IntPtr, WrenVm> Lookup = new();
     public static WrenVm GetVm(IntPtr ptr){
        return Lookup[ptr];
     }
    private readonly ConditionalWeakTable<object, WrenObjectHandle> foreignObjectsLookup = new();
    private readonly HashSet<WeakReference> wrenObjectHandles = new();
    internal void RegisterHandle(WrenObjectHandle wrenObjectHandle)
    {
        var h = new WeakReference(wrenObjectHandle);
        wrenObjectHandles.Add(h);
    }

    internal WrenObjectHandle GetWrenObject(Type t, object obj){
        if(foreignObjectsLookup.TryGetValue(obj, out var handle)) return handle;
        else {
            var c = GetForeignClass(t);
            var m = c.WrapManagedObject(obj);
            foreignObjectsLookup.Add(obj, m);
            return m;
        }
    }

    internal WrenNative.WrenType[] Stack => GetStack();

    internal WrenNative.WrenType[] GetStack(){
        var stack = new WrenNative.WrenType[WrenNative.wrenGetSlotCount(handle)];
        for (int i = 0; i < stack.Length; i++)
        {
            stack[i] = WrenNative.wrenGetSlotType(handle, i);
        }
        return stack;
    }

    internal WrenForeignClass GetForeignClass(Type t)
    {
        return GetClass(t);
    }

    public WrenVm()
    {
        var v = WrenNative.wrenGetVersionNumber();
        if(v != 4000) throw new BadImageFormatException($"Expected Wren version 4000 but got {v}");
        WrenNative.wrenInitConfiguration(out var config);
        config.writeFn = WriteStatic;
        // Todo: Pin delegates
        //GCHandle.Alloc(config.writeFn);
        config.errorFn = ErrorStatic;
        config.bindForeignClassFn = BindForeignClassStatic;
        config.bindForeignMethodFn = BindForeignMethodStatic;
        config.loadModuleFn = LoadModuleStatic;
        config.resolveModuleFn = ResolveModuleStatic;
        handle = WrenNative.wrenNewVM(ref config); 
        Lookup[handle] = this;
        this.config = config;
    }

    // This prevents delegate GC
    private readonly WrenNative.WrenConfiguration config;

    // public void RegisterType(Type type, Func<object> factory){
    //     var fc = new WrenForeignClass(type, factory);
    //     classRegistry.Add($"{type.Namespace}.{type.Name}", fc);
    // }

    public WrenNative.WrenForeignClassMethods BindForeignClass(string module, string className){
        var fcm = new WrenNative.WrenForeignClassMethods();

        var type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.ExportedTypes).FirstOrDefault(x => x.Namespace == module && x.Name == className);
        if(type == null) throw new InvalidOperationException($"Did not find .NET type '{module}.{className}'");
        
        var c = new WrenForeignClass(this, module, className, type, () => null);

        fcm.allocate = c.AllocateFn;
        fcm.finalize = c.FinalizeFn;
        
        classRegistry.Add($"{module}.{className}",c);
        return fcm;
    }

    public WrenNative.WrenForeignMethodFn BindForeignMethod(string module, string className, bool isStatic, string signature){
        var c = GetClass(module, className);
        return c.GetMethodFn(signature, isStatic);
    }

    private WrenForeignClass GetClass(string module, string className){
        if(classRegistry.TryGetValue($"{module}.{className}", out var c)) return c;
        else throw new KeyNotFoundException($"Unable to find foreign class {module}.{className}");
    }

    private WrenForeignClass GetClass(Type t){
        if(classRegistry.TryGetValue(t.FullName, out var c)) return c;
        else throw new KeyNotFoundException($"Unable to find foreign class {t}");
    }



    private Dictionary<string, WrenForeignClass> classRegistry = new();

    private bool disposedValue;
    public bool IsDisposed => disposedValue;
    internal IntPtr handle;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                foreach (var item in callHandleCache)
                {
                    WrenNative.wrenReleaseHandle(handle, item.Value);
                }
                foreach (var item in classRegistry)
                {
                    item.Value.Dispose();
                }
                foreach(var item in foreignObjectsLookup){
                    item.Value.Dispose();
                }
                foreach(var item in wrenObjectHandles){
                    if(item.IsAlive) (item.Target as WrenObjectHandle)?.Dispose();
                }
                Lookup.Remove(handle);
            }
            WrenNative.wrenFreeVM(handle);

            disposedValue = true;
        }
    }
    ~WrenVm()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public void Interpret(string module, string code){
        var result = WrenNative.wrenInterpret(handle, module, code);
    }

    public WrenObjectHandle GetObject(string module, string className){
        WrenNative.wrenEnsureSlots(handle, 1);
        WrenNative.wrenGetVariable(handle, module, className, 0);
        if(WrenNative.wrenGetSlotType(handle,0) != WrenNative.WrenType.WREN_TYPE_UNKNOWN) throw new WrenScriptException(handle, WrenNative.WrenErrorType.WREN_ERROR_RUNTIME, module, -1, $"Variable {className} not found");
        return new WrenObjectHandle(this, 0);
    }

    internal IntPtr GetCallHandle(string signature){
        var v = IntPtr.Zero;
        if(callHandleCache.TryGetValue(signature, out var h)) v = h;
        else 
            v = callHandleCache[signature] = WrenNative.wrenMakeCallHandle(handle, signature);
        return v;
    }

    private Dictionary<string, IntPtr> callHandleCache = new();
}
