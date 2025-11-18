namespace ReflectionPlayground.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class LogExecutionAttribute : Attribute
{
    public string? Tag { get; }

    public LogExecutionAttribute(string? tag = null)
    {
        Tag = tag;
    }
}
