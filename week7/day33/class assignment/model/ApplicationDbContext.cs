using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                 :
         base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }//table name  is employees and not employee employee is the class
        public DbSet<Dept> Depts { get; set; }// table name is depts and not dept dept is the class

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()// Employee is the classname on which we want foreign key constraint  
                .HasOne(e => e.Dept)//Dept is the property name and not the table name 
                .WithMany(d => d.Employees)//Employees is the property name and not the table name 
                .HasForeignKey(e => e.DeptId);//foreign key constraint on DeptId column of Employee table

        }
    }
}
