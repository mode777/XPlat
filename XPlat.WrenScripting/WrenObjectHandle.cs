//using IronWren;

namespace XPlat.WrenScripting;

public class WrenObjectHandle : IDisposable {
    internal readonly IntPtr handle;
    //private readonly int no;
    private readonly WrenVm vm;
    private bool disposedValue;
    //private static int ctr = 0;

    internal WrenObjectHandle(WrenVm vm, int slot)
    {
        this.vm = vm;
        this.handle = WrenNative.wrenGetSlotHandle(vm.handle,slot);
        //this.no = ctr++;
        //System.Console.WriteLine($"Allocate {no}");
        vm.RegisterHandle(this);
    }

    public void Call(string signature, params object[] parameters){
        CallInternal(signature, parameters);
    }

    public WrenObjectHandle CallForObject(string signature, params object[] parameters){
        CallInternal(signature, parameters);
        if(WrenNative.wrenGetSlotType(vm.handle,0) != WrenNative.WrenType.WREN_TYPE_UNKNOWN) throw new WrenScriptException(vm.handle, WrenNative.WrenErrorType.WREN_ERROR_RUNTIME, null, -1, $"Method call did not return object");
        return new WrenObjectHandle(vm, 0);
    }

    private void CallInternal(string signature, params object[] parameters){
        //var start = WrenNative.wrenGetSlotCount(vm.handle);
        WrenNative.wrenEnsureSlots(vm.handle, parameters.Length+1);
        WrenNative.wrenSetSlotHandle(vm.handle, 0, handle);
        for (int i = 0; i < parameters.Length; i++)
        {
            var p = parameters[i];
            var t = p.GetType();

            if(p is WrenObjectHandle obj){
                WrenNative.wrenSetSlotHandle(vm.handle, i+1, obj.handle);
            // todo: handle primitives
            } else {
                var m = vm.GetWrenObject(t, p);
                WrenNative.wrenSetSlotHandle(vm.handle, i+1, m.handle);
            }

        }
        var result = WrenNative.wrenCall(vm.handle, vm.GetCallHandle(signature));
        if(result != WrenNative.WrenInterpretResult.WREN_RESULT_SUCCESS){
            vm.ThrowExceptions();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue && !vm.IsDisposed)
        {
            WrenNative.wrenReleaseHandle(vm.handle, handle);
            disposedValue = true;
            //System.Console.WriteLine($"Dispose {no}");

        }
    }

    ~WrenObjectHandle()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
