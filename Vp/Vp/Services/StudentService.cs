using Vp.Exceptions;
using Vp.Interfaces;
using Vp.Models;

namespace Vp.Services;

public sealed class StudentService(DataStore store, IGradeService gradeService) : IStudentService
{
    public event Action? OnStudentUpdated;

    public IEnumerable<Student> GetAll() => store.Students.OrderBy(s => s.FullName);

    public Student? GetById(Guid id) => store.Students.FirstOrDefault(s => s.Id == id);

    public void Add(Student entity)
    {
        var errors = entity.Validate();
        if (errors.Count > 0) throw new InvalidOperationException(string.Join("; ", errors.Values));
        store.Students.Add(entity);
        store.Users.Add(entity);
        OnStudentUpdated?.Invoke();
    }

    public void Update(Student entity)
    {
        var existing = GetById(entity.Id) ?? throw new InvalidOperationException("Student not found.");
        var errors = entity.Validate();
        if (errors.Count > 0) throw new InvalidOperationException(string.Join("; ", errors.Values));

        existing.FullName = entity.FullName;
        existing.Email = entity.Email;
        existing.Semester = entity.Semester;
        existing.CGPA = entity.CGPA;

        OnStudentUpdated?.Invoke();
    }

    public void Delete(Guid id)
    {
        var hasActive = store.Courses.Any(c => c.Enrollments.Any(e => e.StudentId == id && e.Status == EnrollmentStatus.Active));
        if (hasActive) throw new StudentHasActiveEnrollmentsException("Student has active enrollments and cannot be deleted.");

        var student = GetById(id);
        if (student is null) return;

        store.Students.Remove(student);
        store.Users.RemoveAll(u => u.Id == id);
        OnStudentUpdated?.Invoke();
    }

    public IEnumerable<Enrollment> GetEnrollments(Guid studentId) => store.Courses.SelectMany(c => c.Enrollments).Where(e => e.StudentId == studentId);

    public IEnumerable<GradeRecord> GetGrades(Guid studentId) => gradeService.GetStudentGrades(studentId);

    public double CalculateCgpa(Guid studentId)
    {
        var grades = gradeService.GetStudentGrades(studentId).ToList();
        if (grades.Count == 0) return 0;

        var weighted = (from g in grades
                        join c in store.Courses on g.CourseId equals c.Id
                        select new { g.GradePoint, c.CreditHours }).ToList();

        var creditSum = weighted.Sum(x => x.CreditHours);
        if (creditSum == 0) return 0;

        var points = weighted.Sum(x => x.GradePoint * x.CreditHours);
        return Math.Round(points / creditSum, 2);
    }
}
