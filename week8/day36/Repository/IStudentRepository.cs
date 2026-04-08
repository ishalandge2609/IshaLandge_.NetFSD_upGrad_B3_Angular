
using StudentCourseRelationshipUsingDapper.Models;

namespace StudentCourseRelationshipUsingDapper.Repository
{
    public interface IStudentRepository
    {
        IEnumerable<Student>GetStudentsWithCourse();
        IEnumerable<Course> GetCoursesWithStudents();
    }
}
