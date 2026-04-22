using Vp.Interfaces;

namespace Vp.Models;

public sealed class GradeRecord : IValidatable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Guid CourseId { get; set; }
    public double Marks { get; set; }

    public string LetterGrade => Marks switch
    {
        >= 85 => "A", >= 70 => "B", >= 55 => "C", >= 45 => "D", _ => "F"
    };

    public double GradePoint => LetterGrade switch
    {
        "A" => 4.0, "B" => 3.0, "C" => 2.0, "D" => 1.0, _ => 0.0
    };

    public Dictionary<string, string> Validate()
    {
        var errors = new Dictionary<string, string>();
        if (Marks < 0 || Marks > 100) errors[nameof(Marks)] = "Marks must be between 0 and 100.";
        return errors;
    }
}
