using System.Reflection;
using System.Runtime.InteropServices;
//using IronWren;

namespace XPlat.WrenScripting;

internal abstract class WrenForeignInvokeable {
    private readonly WrenVm vm;

    public WrenForeignInvokeable(WrenVm vm, WrenForeignClass owner, bool isStatic)
    {
        this.vm = vm;
        Owner = owner;
        IsStatic = isStatic;
        ForeignDelegate = Invoke;
    }

    public readonly WrenNative.WrenForeignMethodFn ForeignDelegate;

    internal WrenForeignClass Owner { get; }
    public bool IsStatic { get; }

    public abstract void Invoke(IntPtr vm);

    protected void SetWrenSlot(IntPtr vmHandle, Type type, object value, int slot){
        if(value == null) WrenNative.wrenSetSlotNull(vmHandle, slot); 
        else if(type == typeof(string)) WrenNative.wrenSetSlotString(vmHandle, slot, value as string);
        else if(type == typeof(double)) WrenNative.wrenSetSlotDouble(vmHandle, slot, (double)value);
        else if(type == typeof(int)) WrenNative.wrenSetSlotDouble(vmHandle, slot, (double)value);
        else if(type == typeof(float)) WrenNative.wrenSetSlotDouble(vmHandle, slot, (double)value);
        else if(type == typeof(bool)) 
            WrenNative.wrenSetSlotBool(vmHandle, slot, (bool)value);
        else {
            var wrenObj = vm.GetWrenObject(type, value);
            WrenNative.wrenSetSlotHandle(vmHandle, slot, wrenObj.handle);
        }
        //else throw new InvalidOperationException($"Unconvertable Type: {type.Name}");
    }

    protected object GetWrenSlot(IntPtr vm, Type type, int slot){
        if(type == typeof(string)) return Marshal.PtrToStringAnsi(WrenNative.wrenGetSlotString(vm, slot));
        else if(type == typeof(double)) return WrenNative.wrenGetSlotDouble(vm, slot);
        else if(type == typeof(int)) return (int)WrenNative.wrenGetSlotDouble(vm, slot);
        else if(type == typeof(float)) return (float)WrenNative.wrenGetSlotDouble(vm, slot);
        else if(type.IsEnum) return (int)WrenNative.wrenGetSlotDouble(vm, slot);
        else throw new InvalidOperationException($"Unconvertable Type: {type.Name}");
    }

    protected object GetTarget(IntPtr vmHandle){
        if(IsStatic){
            return null;
        }
        IntPtr ptr = WrenNative.wrenGetSlotForeign(vmHandle, 0);
        var handle = (GCHandle)Marshal.ReadIntPtr(ptr);
        return handle.Target;
    }
}

internal class WrenForeignIndexer : WrenForeignProperty
{
    private readonly ParameterInfo[] indexParamsInfo;
    private readonly object[] indexParams;

    public WrenForeignIndexer(WrenVm vm, WrenForeignClass owner, bool isStatic, PropertyInfo info, bool isSetter) : base(vm, owner, isStatic, info, isSetter)
    {
        this.indexParamsInfo = info.GetIndexParameters();
        this.indexParams = new object[indexParamsInfo.Length];
    }

    protected override void Get(IntPtr vm, object target)
    {
        var indexType = indexParamsInfo[0];
        indexParams[0] = GetWrenSlot(vm, indexType.ParameterType, 1);
        var val = info.GetValue(target, indexParams);
        SetWrenSlot(vm, propertyType, val, 0);
    }
}

internal class WrenForeignProperty : WrenForeignInvokeable {
    protected readonly PropertyInfo info;
    private readonly bool isSetter;
    protected readonly Type propertyType;

    public WrenForeignProperty(WrenVm vm, WrenForeignClass owner, bool isStatic, PropertyInfo info, bool isSetter) : base(vm, owner, isStatic)
    {
        this.info = info ?? throw new ArgumentNullException("PropertyInfo");
        this.isSetter = isSetter;
        this.propertyType = info.PropertyType;
    }

    protected virtual void Get(IntPtr vm, object target){
        var val = info.GetValue(target);
        SetWrenSlot(vm, propertyType, val, 0);
    }

    protected virtual void Set(IntPtr vm, object target){
        throw new NotImplementedException();
    }

    public override void Invoke(IntPtr vm){
        var target = GetTarget(vm);
        if(isSetter){
            Set(vm, target);
        } else {
            Get(vm, target);
        }
    }
}

internal class WrenForeignMethod : WrenForeignInvokeable {

    public WrenForeignMethod(WrenVm vm, WrenForeignClass owner, bool isStatic, MethodInfo info) : base(vm, owner, isStatic)
    {
        this.info = info;
        this.parameterInfo = this.info.GetParameters();
        this.returnType = this.info.ReturnType;
        this.parameters = new object[parameterInfo.Length];
    }
    public override void Invoke(IntPtr vm){
        var target = GetTarget(vm);
        for (int i = 0; i < parameters.Length; i++)
        {
            var type = parameterInfo[i].ParameterType;
            parameters[i] = GetWrenSlot(vm, type, i+1);
        }
        var ret = info.Invoke(target, parameters);
        if(ret != null){
            SetWrenSlot(vm, returnType, ret, 0);
        }
    }

    private readonly MethodInfo info;
    private readonly ParameterInfo[] parameterInfo;
    private readonly Type returnType;
    private readonly object[] parameters;
}
