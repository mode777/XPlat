using System;
using System.Linq;
using NLua;

namespace XPlat.LuaScripting
{
    public class LuaScript
    {
        private readonly Lua state;
        private LuaFunction init;
        private LuaFunction update;
        private bool hasError;
        public event EventHandler<Exception> OnError;
        internal LuaScript(Lua state, string script)
        {
            this.state = state;
            if(script != null) Load(script);
        }

        public void Load(string script, params object[] args)
        {
            hasError = true;
            try {                
                var ctor = state.DoString(script).First() as LuaFunction;
                var mod = ctor.Call(args).First() as LuaTable;
                this.init = mod["init"] as LuaFunction;
                this.update = mod["update"] as LuaFunction;
                hasError = false;
            } catch(Exception e) {
                OnError?.Invoke(this, e);
            }
        }

        public void Init()
        {
            try {
                if(!hasError) init?.Call();
            } catch(Exception e) {
                hasError = true;
                OnError?.Invoke(this, e);
            }
        }

        public void Update()
        {
            try {
                if(!hasError) update?.Call();
            } catch(Exception e){
                hasError = true;
                OnError?.Invoke(this, e);
            }


        }
    }
}

