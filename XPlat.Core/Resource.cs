using System;
using System.IO;
using System.Reflection;

namespace XPlat.Core
{
	public static class Resource
	{
		public static Stream LoadResource(Assembly asm, string name)
        {
			return asm.GetManifestResourceStream(name);
        }

		public static Stream LoadResource<T>(string name)
        {
			return LoadResource(typeof(T).Assembly, name);
        }

		public static string LoadResourceString(Assembly asm, string name)
        {
			using (var sr = new StreamReader(LoadResource(asm, name)))
				return sr.ReadToEnd();
        }

		public static string LoadResourceString<T>(string name)
        {
			return LoadResourceString(typeof(T).Assembly, name);
        }
	}
}

