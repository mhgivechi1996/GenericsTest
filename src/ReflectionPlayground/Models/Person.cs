namespace ReflectionPlayground.Models;

public class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }

    public string GetFullName() => $"{FirstName} {LastName}";

    public string Greet(string greeting) => $"{greeting}, my name is {GetFullName()} and I am {Age} years old.";
}
