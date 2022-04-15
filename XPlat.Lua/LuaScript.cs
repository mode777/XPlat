using System;
using System.Linq;
using NLua;

namespace XPlat.LuaScripting
{

    public class LuaScript
    {
        private readonly Lua state;

        private bool hasError;
        private LuaFunction? ctor;

        public event EventHandler<Exception> OnError;
        internal LuaScript(Lua state, string script)
        {
            this.state = state;
            if(script != null) Load(script);
        }

        public bool Load(string script)
        {
            hasError = true;
            try {                
                ctor = state.DoString(script).First() as LuaFunction;
                hasError = false;
                return true;
            } catch(Exception e) {
                OnError?.Invoke(this, e);
                return false;
            }
        }

        public LuaScriptInstance Instantiate(params object[] args){
            if(!hasError){
                hasError = true;
                try {
                    var mod = ctor.Call(args).First() as LuaTable;
                    hasError = false;
                    return new LuaScriptInstance(mod);
                } catch(Exception e) {
                    OnError?.Invoke(this, e);
                }
            }
            return null;
        }
    }
}

