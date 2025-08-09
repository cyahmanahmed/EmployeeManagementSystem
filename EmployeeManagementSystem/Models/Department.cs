using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Category Name cannot exceed 50 characters.")]
        public string DepartmentName { get; set; }
        public virtual ICollection<Employee> Employee { get; set; } 
        public bool IsActive { get; set; }=true;
    }
}
