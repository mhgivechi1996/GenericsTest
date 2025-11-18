using ReflectionPlayground.Attributes;

namespace ReflectionPlayground.Models;

public class UserDto
{
    [MyRequired]
    public Guid Id { get; set; }

    [MapTo("FirstName")]
    public string GivenName { get; set; } = string.Empty;

    [MapTo("LastName")]
    public string FamilyName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [MapTo("RegisteredAt")]
    public DateTime CreatedAt { get; set; }
}
