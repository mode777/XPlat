//using IronWren;

namespace XPlat.WrenScripting;

internal class WrenScriptException : Exception{
    public WrenScriptException(IntPtr vm, WrenNative.WrenErrorType type, string module, int line, string message) 
        : base($"{type.ToString()} in module '{module}' line {line}: {message}")
    {
        Vm = vm;
        Type = type;
        Module = module;
        Line = line;
        OriginalMessage = message;
    }

    public IntPtr Vm { get; }
    public WrenNative.WrenErrorType Type { get; }
    public string Module { get; }
    public int Line { get; }
    public string OriginalMessage { get; }
}
