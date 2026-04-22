using Vp.Models;

namespace Vp.Interfaces;

public interface IGradeService
{
    event Action? OnGradesSubmitted;
    void SubmitGrade(GradeRecord record);
    IEnumerable<GradeRecord> GetStudentGrades(Guid studentId);
    IEnumerable<GradeRecord> GetCourseGrades(Guid courseId);
}
