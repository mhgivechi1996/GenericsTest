using ReflectionPlayground.Attributes;

namespace ReflectionPlayground.Models;

public class UserInputModel
{
    [MyRequired("First name is required.")]
    public string? FirstName { get; set; }

    [MyRequired("Email cannot be empty.")]
    public string? Email { get; set; }

    public string? Biography { get; set; }

    [MyRequired]
    public DateTime? BirthDate { get; set; }
}
