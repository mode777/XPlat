using System.Numerics;
using System.Xml.Linq;

namespace XPlat.Engine.Serialization {
    public static class XElementExtensions {
        public static bool TryGetAttribute(this XElement el, string name, out string value){
            value = el.Attribute(name)?.Value;
            if(value == null) return false;
            return true;
        }

        public static Vector2 Vector2(this string attr){
            var str = attr;
            try {
                var v3 = str.Split(',').Select(x => float.Parse(x)).ToArray();
                return new Vector2(v3[0],v3[1]);
            } catch {
                throw new InvalidDataException($"'{str}' is not a valid Vector2");
            }
        }
        
        public static Vector3 Vector3(this string attr){
            var str = attr;
            try {
                var v3 = str.Split(',').Select(x => float.Parse(x)).ToArray();
                if(v3.Length == 2)
                    return new Vector3(v3[0],v3[1],0);
                else
                    return new Vector3(v3[0],v3[1],v3[2]);
            } catch {
                throw new InvalidDataException($"'{str}' is not a valid Vector3");
            }
        }


        public static Vector4 Vector4(this string attr){
            var str = attr;
            try {
                var v4 = str.Split(',').Select(x => float.Parse(x)).ToArray();
                return new Vector4(v4[0],v4[1],v4[2], v4[3]);
            } catch {
                throw new InvalidDataException($"'{str}' is not a valid Vector3");
            }
        }
    }
}