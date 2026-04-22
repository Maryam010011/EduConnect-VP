namespace Vp.Models;

public sealed class Admin : Person
{
    public override UserRole GetRole() => UserRole.Admin;
}
