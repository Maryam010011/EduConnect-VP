using Vp.Models;

namespace Vp.Services;

public sealed class DataStore
{
    public List<Person> Users { get; } = [];
    public List<Student> Students { get; } = [];
    public List<Faculty> FacultyMembers { get; } = [];
    public List<Admin> Admins { get; } = [];
    public List<Course> Courses { get; } = [];
    public List<GradeRecord> Grades { get; } = [];
    public List<Notification> Notifications { get; } = [];

    public DataStore()
    {
        Seed();
    }

    private void Seed()
    {
        var admin = new Admin { FullName = "Admin User", Email = "admin@edu.com", Password = "admin123" };
        var f1 = new Faculty { FullName = "Sir Obaid Ullah", Email = "obaid@edu.com", Password = "obaid345" };
        var f2 = new Faculty { FullName = "Sir Bilal", Email = "bilal@edu.com", Password = "bilal123" };

        var s1 = new Student { FullName = "Maryam Jahangir", Email = "mehru@edu.com", Password = "mehru241949", Semester = 4, CGPA = 3.8 };
        var s2 = new Student { FullName = "Maryam Yaqoob", Email = "maryamqb@edu.com", Password = "maryam241880", Semester = 5, CGPA = 3.6 };
        var s3 = new Student { FullName = "Shazain Nadeem", Email = "shazain@edu.com", Password = "shazain241567", Semester = 6, CGPA = 3.7};

        Admins.Add(admin);
        FacultyMembers.AddRange([f1, f2]);
        Students.AddRange([s1, s2, s3]);

        Users.AddRange([admin, f1, f2, s1, s2, s3 ]);

        var c1 = new Course { Code = "CS201", Title = "OOP", CreditHours = 3, MaxCapacity = 30, FacultyId = f1.Id };
        var c2 = new Course { Code = "CS284", Title = "Visual Programming", CreditHours = 3, MaxCapacity = 25, FacultyId = f2.Id };
        var c3 = new Course { Code = "CS305", Title = "DBMS", CreditHours = 3, MaxCapacity = 20, FacultyId = f1.Id };

        Courses.AddRange([c1, c2, c3]);
        f1.AssignedCourseIds.AddRange([c1.Id, c3.Id]);
        f2.AssignedCourseIds.Add(c2.Id);

        c1.Enrollments.Add(new Enrollment { StudentId = s1.Id, CourseId = c1.Id, Status = EnrollmentStatus.Active });
        c2.Enrollments.Add(new Enrollment { StudentId = s2.Id, CourseId = c2.Id, Status = EnrollmentStatus.Active });
        c3.Enrollments.Add(new Enrollment { StudentId = s3.Id, CourseId = c3.Id, Status = EnrollmentStatus.Active });
    }
}
