namespace WebApplication3.Models
{
    public class Dept
    {
        public int  DeptId { get; set; }
        public String Dname{ get; set; }
        public String Location { get; set; }
        // Navigation property (1 department → many employees)
        public ICollection<Employee> Employees { get; set; }
    }
}
