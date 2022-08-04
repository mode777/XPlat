using System.Runtime.InteropServices;
//using IronWren;

namespace XPlat.WrenScripting;


internal class WrenForeignClass : IDisposable
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
            return WrenNative.wrenGetSlotHandle(vm.handle, start);
        });
    }

    internal readonly WrenNative.WrenForeignMethodFn AllocateFn;
    private void Allocate(IntPtr vm){
        IntPtr foreign = WrenNative.wrenSetSlotNewForeign(vm, 0,0, (IntPtr)IntPtr.Size);   
        Marshal.WriteIntPtr(foreign, (IntPtr)GCHandle.Alloc(factory()));
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
        if(signature.Contains('(')){
            var spl = signature.Split('(');
            //int count = spl[1].Count(f => f == '_');
            var m = type.GetMethod(spl[0]);
            method = new WrenForeignMethod(vm, m);
        } else {
            var p = type.GetProperty(signature);
            method = new WrenForeignProperty(vm, p, false);
        }
        methodRegistry.Add(signature, method);
        return method;
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

            if(classHandle.IsValueCreated) WrenNative.wrenReleaseHandle(vm.handle, classHandle.Value);
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
