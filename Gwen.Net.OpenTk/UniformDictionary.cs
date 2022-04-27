using System;
using System.Collections.Generic;

namespace Gwen.Net.OpenTk
{
    public class UniformDictionary
    {
        private readonly Func<int, string, int> uniformLocationResolver;
        private readonly Dictionary<string, int> data;
        private readonly int program;

        public UniformDictionary(int program, Func<int, string, int> uniformLocationResolver)
        {
            data = new Dictionary<string, int>();
            this.program = program;
            this.uniformLocationResolver = uniformLocationResolver;
        }

        public int this[string key]
        {
            get
            {
                if (data.TryGetValue(key, out int loc))
                {
                    return loc;
                }
                else
                {
                    int uniformLocation = uniformLocationResolver.Invoke(program, key);
                    data.Add(key, uniformLocation);
                    return uniformLocation;
                }
            }
        }
    }
}