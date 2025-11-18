namespace ReflectionPlayground.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public sealed class DeveloperAttribute : Attribute
{
    public string Name { get; }
    public string Level { get; }

    public DeveloperAttribute(string name, string level)
    {
        Name = name;
        Level = level;
    }
}
