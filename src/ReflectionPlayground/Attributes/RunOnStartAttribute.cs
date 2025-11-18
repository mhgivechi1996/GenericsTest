namespace ReflectionPlayground.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class RunOnStartAttribute : Attribute
{
    public string? Description { get; }
    public int Order { get; }

    public RunOnStartAttribute(string? description = null, int order = 0)
    {
        Description = description;
        Order = order;
    }
}
