namespace ReflectionPlayground.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class MyRequiredAttribute : Attribute
{
    public string? ErrorMessage { get; }

    public MyRequiredAttribute(string? errorMessage = null)
    {
        ErrorMessage = errorMessage;
    }
}
