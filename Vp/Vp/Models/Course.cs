namespace Vp.Models;

public sealed class Course
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; }
    public int MaxCapacity { get; set; } = 30;
    public Guid FacultyId { get; set; }
    public List<Enrollment> Enrollments { get; set; } = [];

    public int EnrolledCount => Enrollments.Count(e => e.Status == EnrollmentStatus.Active);

    public CourseEnrollmentStatus CapacityStatus => EnrolledCount >= MaxCapacity ? CourseEnrollmentStatus.Full : EnrolledCount >= (int)Math.Ceiling(MaxCapacity * 0.8)
            ? CourseEnrollmentStatus.AlmostFull
            : CourseEnrollmentStatus.Open;
}
