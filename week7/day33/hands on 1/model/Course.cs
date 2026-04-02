namespace WebApplication4.Models
{
    public class Course
    {
       
           public int CourseId { get; set; }   // Primary Key
            public string CourseName { get; set; }

            // Navigation Property (One Course → Many Students)
            public List<Student> Students { get; set; }
        }
}
