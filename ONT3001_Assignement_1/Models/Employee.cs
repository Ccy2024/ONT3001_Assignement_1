using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ONT3001_Assignement_1.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "FirstName cannot execeed more that 60 characters.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(60, ErrorMessage = "LastName cannot execeed more that 60 characters.")]
        public string LastName { get; set; }    

        public string FullName  => FirstName + " " + LastName;
        [Range(1, 200, ErrorMessage = "Age must be between 1 and 120.")]
        public int Age { get; set; }
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
        [Required]

        //Employee is active upon creation
        public bool IsActive { get; set; } = true;
    }
}
