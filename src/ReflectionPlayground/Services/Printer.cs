namespace ReflectionPlayground.Services;

public class Printer
{
    public string PrintWithPrefix(string message)
        => $"[Printer] {message}";

    public string PrintUpper(string message)
        => message.ToUpperInvariant();

    public void Repeat(string message, int times)
    {
        for (var i = 0; i < times; i++)
        {
            Console.WriteLine($"({i + 1}) {message}");
        }
    }
}
