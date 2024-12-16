using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrakerLibrary.Model
{
    [Table("Role")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Role ID")]
        public int RoleID { get; set; }

        [Required(ErrorMessage = "RoleName is required")]
        [StringLength(20)]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
}
