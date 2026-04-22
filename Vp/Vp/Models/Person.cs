namespace Vp.Models;

public abstract class Person
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = "pass123";

    public abstract UserRole GetRole();
}
