using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Models
        
{
 
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                :
        base(options)
        {
        }
        public DbSet<Course> Courses{ get; set; }
            
        public DbSet<Student> Students{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                        .HasOne(s => s.Course)
                        .WithMany(c => c.Students)
                        .HasForeignKey(s=>s.CourseId);
        }

    }
}
