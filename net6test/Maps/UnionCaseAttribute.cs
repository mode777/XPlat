[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class UnionCaseAttribute : Attribute
{
    public Type CaseType { get; }

    public String TagPropertyValue { get; }

    public UnionCaseAttribute(Type caseType, String tagPropertyValue) =>
        (this.CaseType, this.TagPropertyValue) = (caseType, tagPropertyValue);
}
