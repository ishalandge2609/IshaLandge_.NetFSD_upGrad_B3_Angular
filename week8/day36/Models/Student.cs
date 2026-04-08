using System.ComponentModel.DataAnnotations.Schema;

namespace StudentCourseRelationshipUsingDapper.Models
{
    public class Student
    {
        
            public int StudentId { get; set; }   // Primary Key
            public string StudentName { get; set; }

        // Foreign Key
            [ForeignKey("Course")]
            public int CourseId { get; set; }

            // Navigation Property (Each Student → One Course)
            public Course Course { get; set; }
        }
    }

