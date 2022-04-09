using System;
using NLua;

namespace XPlat.LuaScripting
{
    public class LuaScriptInstance 
    {
        private bool hasError;
        private LuaFunction init;
        private LuaFunction update;

        internal LuaTable Table { get; }

        public event EventHandler<Exception> OnError;

        public LuaScriptInstance(LuaTable table)
        {
            this.init = table["init"] as LuaFunction;
            this.update = table["update"] as LuaFunction;
            Table = table;
        }

        public void Init()
        {
            try {
                if(!hasError) init?.Call(Table);
            } catch(Exception e) {
                hasError = true;
                OnError?.Invoke(this, e);
            }
        }

        public void Update()
        {
            try {
                if(!hasError) update?.Call(Table);
            } catch(Exception e){
                hasError = true;
                OnError?.Invoke(this, e);
            }
        }
    }
}

