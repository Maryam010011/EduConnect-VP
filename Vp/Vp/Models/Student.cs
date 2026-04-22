using Vp.Interfaces;

namespace Vp.Models;

public sealed class Student : Person, IValidatable
{
    public int Semester { get; set; }
    public double CGPA { get; set; }

    public override UserRole GetRole() => UserRole.Student;

    public Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(FullName)) errors[nameof(FullName)] = "Name is required.";
        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@')) errors[nameof(Email)] = "Valid email is required.";
        if (Semester < 1 || Semester > 8) errors[nameof(Semester)] = "Semester must be between 1 and 8.";
        if (CGPA < 0 || CGPA > 4) errors[nameof(CGPA)] = "CGPA must be between 0 and 4.";

        return errors;
    }
}
