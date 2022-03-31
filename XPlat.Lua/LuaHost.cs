using NLua;

namespace XPlat.LuaScripting
{
    public class LuaHost
    {
        private Lua state;

        public LuaHost()
        {
            this.state = new Lua();
            state.LoadCLRPackage();
            //state.DoString("import ('XPlat.Core')");
            //var mod = state.DoString("return { myfunc = function() return 42 end }").First() as LuaTable;
            //var func = mod["myfunc"] as LuaFunction;
            //var res = (long)func.Call().First();
            
        }

        public LuaScript CreateScript(string script = null)
        {
            return new LuaScript(state, script);
        }

        public void SetGlobal(string name, object obj){
            state[name] = obj;
        }

        public void ImportNamespace(string ns)
        {
            state.DoString($"import ('{ns}')");
        }
    }
}

