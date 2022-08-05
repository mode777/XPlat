using System.Runtime.InteropServices;
//using IronWren;

namespace XPlat.WrenScripting;

public unsafe static class WrenNative {

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr WrenReallocateFn(IntPtr memory, uint newSize, IntPtr userData);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WrenForeignMethodFn(IntPtr vm);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WrenFinalizerFn(IntPtr data);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr WrenResolveModuleFn(IntPtr vm, string importer, IntPtr name);
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WrenLoadModuleCompleteFn(IntPtr vm, string name, WrenLoadModuleResult result);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate WrenLoadModuleResult WrenLoadModuleFn(IntPtr vm, string name);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate WrenForeignMethodFn WrenBindForeignMethodFn(IntPtr vm, string module, string className, bool isStatic, string signature);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WrenWriteFn(IntPtr vm, string text);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void WrenErrorFn(IntPtr vm, WrenErrorType type, string module, int line, string message);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate WrenForeignClassMethods WrenBindForeignClassFn(IntPtr vm, string module, string classname);

    [StructLayout(LayoutKind.Sequential)]
    public struct WrenConfiguration {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenReallocateFn reallocateFn;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenResolveModuleFn resolveModuleFn;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenLoadModuleFn loadModuleFn;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenBindForeignMethodFn bindForeignMethodFn;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenBindForeignClassFn bindForeignClassFn;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenWriteFn writeFn;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenErrorFn errorFn;
        IntPtr InitialHeapSize;
        IntPtr minHeapSize;
        int heapGrowthPercent;
        IntPtr userData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WrenLoadModuleResult 
    {
        public string source;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenLoadModuleCompleteFn onComplete;
        public IntPtr userData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WrenForeignClassMethods
    {
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenForeignMethodFn allocate;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WrenFinalizerFn finalize;
    }

    public enum WrenErrorType
    {
        // A syntax or resolution error detected at compile time.
        WREN_ERROR_COMPILE,

        // The error message for a runtime error.
        WREN_ERROR_RUNTIME,

        // One entry of a runtime error's stack trace.
        WREN_ERROR_STACK_TRACE
    }

    public enum WrenInterpretResult
    {
        WREN_RESULT_SUCCESS,
        WREN_RESULT_COMPILE_ERROR,
        WREN_RESULT_RUNTIME_ERROR
    }

    public enum WrenType
    {
        WREN_TYPE_BOOL,
        WREN_TYPE_NUM,
        WREN_TYPE_FOREIGN,
        WREN_TYPE_LIST,
        WREN_TYPE_MAP,
        WREN_TYPE_NULL,
        WREN_TYPE_STRING,
        // The object is of a type that isn't accessible by the C API.
        WREN_TYPE_UNKNOWN
    }

    private const string libname = "libwren";
    
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern int wrenGetVersionNumber();
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenInitConfiguration(out WrenConfiguration configuration);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenNewVM(ref WrenConfiguration configuration);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenFreeVM(IntPtr vm);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenCollectGarbage(IntPtr vm);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern WrenInterpretResult wrenInterpret(IntPtr vm, string module, string source);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenMakeCallHandle(IntPtr vm, string signature);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern WrenInterpretResult wrenCall(IntPtr vm, IntPtr method);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenReleaseHandle(IntPtr vm, IntPtr handle);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern int wrenGetSlotCount(IntPtr vm);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenEnsureSlots(IntPtr vm, int numSlots);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern WrenType wrenGetSlotType(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern bool wrenGetSlotBool(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern unsafe byte* wrenGetSlotBytes(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern double wrenGetSlotDouble(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenGetSlotForeign(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenGetSlotString(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenGetSlotHandle(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotBool(IntPtr vm, int slot, bool value);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern unsafe void wrenSetSlotBytes(IntPtr vm, int slot, byte* bytes, IntPtr length);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenSetSlotNewForeign(IntPtr vm, int slot, int classSlot, IntPtr size);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotNewList(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotNewMap(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotNull(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotString(IntPtr vm, int slot, string text);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotDouble(IntPtr vm, int slot, double value);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetSlotHandle(IntPtr vm, int slot, IntPtr handle);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern int wrenGetListCount(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenGetListElement(IntPtr vm, int listSlot, int index, int elementSlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetListElement(IntPtr vm, int listSlot, int index, int elementSlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenInsertInList(IntPtr vm, int listSlot, int index, int elementSlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern int wrenGetMapCount(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern bool wrenGetMapContainsKey(IntPtr vm, int mapSlot, int keySlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenGetMapValue(IntPtr vm, int mapSlot, int keySlot, int valueSlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetMapValue(IntPtr vm, int mapSlot, int keySlot, int valueSlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenRemoveMapValue(IntPtr vm, int mapSlot, int keySlot, int valueSlot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenGetVariable(IntPtr vm, string module, string name, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern bool wrenHasVariable(IntPtr vm, string module, string name);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern bool wrenHasModule(IntPtr vm, string module);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern bool wrenAbortFiber(IntPtr vm, int slot);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern IntPtr wrenGetUserData(IntPtr vm);
    [DllImport(libname, CallingConvention = CallingConvention.Cdecl)] public static extern void wrenSetUserData(IntPtr vm, IntPtr userData);
}
