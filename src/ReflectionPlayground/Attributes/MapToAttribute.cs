namespace ReflectionPlayground.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MapToAttribute : Attribute
{
    public string TargetProperty { get; }

    public MapToAttribute(string targetProperty)
    {
        TargetProperty = targetProperty;
    }
}
