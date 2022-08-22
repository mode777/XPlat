using System.Reflection;
using System.Runtime.InteropServices;
//using IronWren;

namespace XPlat.WrenScripting;


public class WrenForeignClass : IDisposable
{
    private readonly Lazy<IntPtr> classHandle;
    internal WrenForeignClass(WrenVm vm, string module, string className, Type type, Func<object> factory)
    {
        this.vm = vm;
        this.module = module;
        this.className = className;
        this.type = type;
        this.factory = factory;
        this.FinalizeFn = Finalize;
        this.AllocateFn = Allocate;
        classHandle = new(() =>
        {
            var start = WrenNative.wrenGetSlotCount(vm.handle);
            WrenNative.wrenEnsureSlots(vm.handle, start+1);
            WrenNative.wrenGetVariable(vm.handle, module, className, start);
            //System.Console.WriteLine($"Allocate {module}.{className}");
            return WrenNative.wrenGetSlotHandle(vm.handle, start);
        });
    }

    internal readonly WrenNative.WrenForeignMethodFn AllocateFn;
    private void Allocate(IntPtr vm){
        var ctor = GetConstructor(vm);
        ctor.Invoke(vm);
    }

    private readonly Dictionary<int, WrenForeignConstructor> ctorCache = new();

    private WrenForeignConstructor GetConstructor(IntPtr vmHandle){
        var numArgs = WrenNative.wrenGetSlotCount(vmHandle) - 1;
        if(ctorCache.TryGetValue(numArgs, out var ctor)){
            return ctor;
        } else {
            var ctorInfo = type.GetConstructors().FirstOrDefault(x =>
            {
                var para = x.GetParameters();
                return para.Length == numArgs || para.Where(y => !y.IsOptional).Count() == numArgs;
            });
            ctor = new WrenForeignConstructor(vm, this, ctorInfo, false);
            ctorCache.Add(numArgs, ctor);
            return ctor;
        }

    }

    internal readonly WrenNative.WrenFinalizerFn FinalizeFn;
    private void Finalize(IntPtr data){
        GCHandle handle = (GCHandle)Marshal.ReadIntPtr(data);
        handle.Free();
    }

    public WrenObjectHandle WrapManagedObject(object obj){
        var handle = classHandle.Value;
        var start = WrenNative.wrenGetSlotCount(vm.handle);
        WrenNative.wrenEnsureSlots(vm.handle, start+1);
        WrenNative.wrenSetSlotHandle(vm.handle, start, handle);
        IntPtr foreign = WrenNative.wrenSetSlotNewForeign(vm.handle, start,start, (IntPtr)IntPtr.Size);   
        Marshal.WriteIntPtr(foreign, (IntPtr)GCHandle.Alloc(obj));
        return new WrenObjectHandle(vm, start);
    }

    private readonly Dictionary<string, WrenForeignInvokeable> methodRegistry = new();

    WrenForeignInvokeable GetMethod(string signature, bool isStatic){
        WrenForeignInvokeable method = null;

        BindingFlags flags = isStatic ? BindingFlags.Static : BindingFlags.Instance;
        flags |= (BindingFlags.Public | BindingFlags.IgnoreCase);
        
        if(signature.Contains("=(")){
            var spl = signature.Split("=");
            var p = type.GetProperty(spl[0], flags);
            method = new WrenForeignProperty(vm, this, isStatic, p, true);
        }
        else if(signature.Contains('(')){
            var spl = signature.Split('(');
            int count = spl[1].Count(f => f == '_');
            var m = type.GetMethods(flags)
                .FirstOrDefault(x => 
                    x.Name.Equals(spl[0], StringComparison.OrdinalIgnoreCase) && 
                    x.GetParameters().Length == count);
            method = new WrenForeignMethod(vm, this, isStatic, m);
        } else if(signature.Contains('[')) {
            var p = type.GetProperties(flags).First(x => x.GetIndexParameters().Length > 0);
            method = new WrenForeignIndexer(vm, this, isStatic, p, false);
        } else {
            var p = type.GetProperty(signature, flags);
            method = new WrenForeignProperty(vm, this, isStatic, p, false);
        }

        methodRegistry.Add(signature, method);
        return method;
    }

    public void RegisterCustomBinding(string signature, WrenForeignInvokeable method){
        methodRegistry[signature] = method;
    }

    public WrenNative.WrenForeignMethodFn GetMethodFn(string signature, bool isStatic){
        var m = GetMethod(signature, isStatic);
        return m.ForeignDelegate;
    }

    private bool disposedValue;
    private readonly Func<object> factory;
    private readonly WrenVm vm;
    private readonly string module;
    private readonly string className;
    private readonly Type type;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            if(classHandle.IsValueCreated) { 
                WrenNative.wrenReleaseHandle(vm.handle, classHandle.Value);
                //System.Console.WriteLine($"Dispose {module}.{className}");
            }
            disposedValue = true;
        }
    }

    // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~WrenForeignClass()
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
}
