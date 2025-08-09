using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
        public string LastName { get; set; }
        [Required]
        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65")]
        public int Age { get; set; }
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public bool IsActive { get; set; } = true;
        public Department Department { get; set; }
    }
}
