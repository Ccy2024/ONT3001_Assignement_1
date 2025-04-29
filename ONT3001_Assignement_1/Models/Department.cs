using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ONT3001_Assignement_1.Models
{
    public class Department
    {
        [Key] 
        public int DepartmentID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters")]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
        
    }
}
