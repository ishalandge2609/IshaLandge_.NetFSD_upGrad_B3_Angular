using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public String EName { get; set; }
        public String Job { get; set; }
        public int Salary { get; set; }

        [ForeignKey("DeptId")] // to add foreign key constraint
        public int DeptId { get; set; }
        public Dept Dept  { get; set; }// one department → many employees
    }
}
