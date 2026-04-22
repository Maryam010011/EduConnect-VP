using Vp.Models;

namespace Vp.Interfaces;

public interface ICourseService : IRepository<Course>
{
    event Action? OnEnrollmentChanged;
    IEnumerable<Course> GetCoursesForFaculty(Guid facultyId);
    IEnumerable<Course> GetAvailableCoursesForStudent(Guid studentId);
    IEnumerable<Course> GetCoursesForStudent(Guid studentId);
    void EnrollStudent(Guid studentId, Guid courseId);
    void DropCourse(Guid studentId, Guid courseId);
}
