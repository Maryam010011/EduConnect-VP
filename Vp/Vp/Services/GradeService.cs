using Vp.Interfaces;
using Vp.Models;

namespace Vp.Services;

public sealed class GradeService(DataStore store, INotificationService notificationService) : IGradeService
{
    public event Action? OnGradesSubmitted;

    public IEnumerable<GradeRecord> GetStudentGrades(Guid studentId) => store.Grades.Where(g => g.StudentId == studentId);

    public IEnumerable<GradeRecord> GetCourseGrades(Guid courseId) => store.Grades.Where(g => g.CourseId == courseId);

    public void SubmitGrade(GradeRecord record)
    {
        var errors = record.Validate();
        if (errors.Count > 0) throw new InvalidOperationException(string.Join("; ", errors.Values));

        var existing = store.Grades.FirstOrDefault(g => g.StudentId == record.StudentId && g.CourseId == record.CourseId);
        if (existing is null) store.Grades.Add(record);
        else existing.Marks = record.Marks;

        notificationService.Push(new Notification
        {
            RecipientId = record.StudentId,
            Type = NotificationType.GradePosted,
            Message = "New grade has been posted."
        });

        OnGradesSubmitted?.Invoke();
    }
}
