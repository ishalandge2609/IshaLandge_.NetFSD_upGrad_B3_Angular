using Dapper;
using Microsoft.Data.SqlClient;
using StudentCourseRelationshipUsingDapper.Models;

namespace StudentCourseRelationshipUsingDapper.Repository
{
    public class StudentRepository: IStudentRepository
    {
        private readonly string _connStr; 

        public StudentRepository(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connStr);
        }
        public IEnumerable<Student> GetStudentsWithCourse()
        {
            using (var db = GetConnection())
            {
                string sql = @"SELECT 
                               s.StudentId, s.StudentName, s.CourseId,
                               c.CourseId, c.CourseName
                               FROM Students s
                               INNER JOIN Courses c
                               ON s.CourseId = c.CourseId";

                return db.Query<Student, Course, Student>(
                    sql,
                    (student, course) =>
                    {
                        student.Course = course;   // mapping
                        return student;
                    },
                    splitOn: "CourseId"
                );
            }
        }


        public IEnumerable<Course> GetCoursesWithStudents()
        {
            using (var db = GetConnection())
            {
                string sql = @"SELECT 
                               c.CourseId, c.CourseName,
                               s.StudentId, s.StudentName, s.CourseId
                               FROM Courses c
                               LEFT JOIN Students s
                               ON c.CourseId = s.CourseId";

                var dict = new Dictionary<int, Course>();

                var list = db.Query<Course, Student, Course>(
                    sql,
                    (course, student) =>
                    {
                        if (!dict.TryGetValue(course.CourseId, out var currentCourse))
                        {
                            currentCourse = course;
                            currentCourse.Students = new List<Student>();
                            dict.Add(currentCourse.CourseId, currentCourse);
                        }

                        if (student != null)
                        {
                            currentCourse.Students.Add(student);
                        }

                        return currentCourse;
                    },
                    splitOn: "StudentId"
                );

                return dict.Values;
            }
        }
    }
}
    
