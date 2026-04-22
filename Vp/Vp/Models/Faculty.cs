namespace Vp.Models;

public sealed class Faculty : Person
{
    public List<Guid> AssignedCourseIds { get; set; } = [];

    public override UserRole GetRole() => UserRole.Faculty;
}
