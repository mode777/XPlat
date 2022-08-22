using System.Text;

namespace XPlat.WrenScripting;

public class CodeGenerator
{
    private readonly StringBuilder sb = new();

    public string ToWrenClass<T>(){
        sb.Clear();
        var t = typeof(T);
        if(t.IsEnum){
            WriteEnumClass(t);
        } else {
            throw new NotImplementedException();
        }
        return sb.ToString();
    }

    private void WriteEnumClass(Type t){
        sb.AppendLine($"class {t.Name} {{");
        var names = System.Enum.GetNames(t);
        var values = System.Enum.GetValues(t).Cast<int>().ToArray();
        for (int i = 0; i < names.Length; i++)
        {
            sb.AppendLine($"    static {names[i]} {{ {values[i]} }}");
        };
        sb.AppendLine("}");
    }
}
