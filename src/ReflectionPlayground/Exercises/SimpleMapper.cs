using System.Reflection;
using ReflectionPlayground.Attributes;

namespace ReflectionPlayground.Exercises;

internal static class SimpleMapper
{
    public static TDestination Map<TSource, TDestination>(TSource source)
        where TDestination : new()
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var destination = new TDestination();
        var destinationType = typeof(TDestination);

        foreach (var property in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var targetName = property.GetCustomAttribute<MapToAttribute>()?.TargetProperty ?? property.Name;
            var destinationProperty = destinationType.GetProperty(targetName, BindingFlags.Public | BindingFlags.Instance);
            if (destinationProperty is null || !destinationProperty.CanWrite)
            {
                continue;
            }

            var value = property.GetValue(source);
            destinationProperty.SetValue(destination, value);
        }

        return destination;
    }
}
