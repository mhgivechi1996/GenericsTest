using ReflectionPlayground.Attributes;

namespace ReflectionPlayground.Services;

public class StartupTasks
{
    [RunOnStart("Preload configuration", order: 1)]
    public void LoadConfiguration()
    {
        Console.WriteLine("Loading configuration...");
    }

    [RunOnStart("Warm up caches", order: 2)]
    public void WarmUpCaches()
    {
        Console.WriteLine("Warming up caches...");
    }
}

public static class StartupStaticTasks
{
    [RunOnStart("Static initialization", order: 0)]
    public static void PrepareStaticContext()
    {
        Console.WriteLine("Preparing static context...");
    }
}
