using Vp.Models;

namespace Vp.Interfaces;

public interface IStudentService : IRepository<Student>
{
    event Action? OnStudentUpdated;
    IEnumerable<Enrollment> GetEnrollments(Guid studentId);
    IEnumerable<GradeRecord> GetGrades(Guid studentId);
    double CalculateCgpa(Guid studentId);
}
