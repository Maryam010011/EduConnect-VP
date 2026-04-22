using Vp.Exceptions;
using Vp.Interfaces;
using Vp.Models;

namespace Vp.Services;

public sealed class CourseService(DataStore store, INotificationService notificationService) : ICourseService
{
    public event Action? OnEnrollmentChanged;

    public IEnumerable<Course> GetAll() => store.Courses.OrderBy(c => c.Code);

    public Course? GetById(Guid id) => store.Courses.FirstOrDefault(c => c.Id == id);

    public void Add(Course entity) => store.Courses.Add(entity);

    public void Update(Course entity)
    {
        var existing = GetById(entity.Id) ?? throw new InvalidOperationException("Course not found.");
        existing.Code = entity.Code;
        existing.Title = entity.Title;
        existing.CreditHours = entity.CreditHours;
        existing.MaxCapacity = entity.MaxCapacity;
        existing.FacultyId = entity.FacultyId;
    }

    public void Delete(Guid id) => store.Courses.RemoveAll(c => c.Id == id);

    public IEnumerable<Course> GetCoursesForFaculty(Guid facultyId) => store.Courses.Where(c => c.FacultyId == facultyId);

    public IEnumerable<Course> GetAvailableCoursesForStudent(Guid studentId) =>
        store.Courses.Where(c => c.CapacityStatus != CourseEnrollmentStatus.Full && c.Enrollments.All(e => !(e.StudentId == studentId && e.Status == EnrollmentStatus.Active)));

    public IEnumerable<Course> GetCoursesForStudent(Guid studentId) =>
        store.Courses.Where(c => c.Enrollments.Any(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Active));

    public void EnrollStudent(Guid studentId, Guid courseId)
    {
        var course = GetById(courseId) ?? throw new InvalidOperationException("Course not found.");
        if (course.CapacityStatus == CourseEnrollmentStatus.Full) throw new CourseFullException("Course is full.");

        var droppedThisSemester = course.Enrollments.Any(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Dropped);
        if (droppedThisSemester) throw new InvalidOperationException("Dropped courses cannot be re-enrolled in same semester.");

        if (course.Enrollments.Any(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Active)) return;

        course.Enrollments.Add(new Enrollment { StudentId = studentId, CourseId = courseId, Status = EnrollmentStatus.Active });
        notificationService.Push(new Notification
        {
            RecipientId = studentId,
            Type = NotificationType.Enrollment,
            Message = $"You are enrolled in {course.Code} - {course.Title}."
        });

        OnEnrollmentChanged?.Invoke();
    }

    public void DropCourse(Guid studentId, Guid courseId)
    {
        var course = GetById(courseId) ?? throw new InvalidOperationException("Course not found.");
        var enrollment = course.Enrollments.FirstOrDefault(e => e.StudentId == studentId && e.Status == EnrollmentStatus.Active)
            ?? throw new InvalidOperationException("Active enrollment not found.");

        enrollment.Status = EnrollmentStatus.Dropped;
        OnEnrollmentChanged?.Invoke();
    }
}
