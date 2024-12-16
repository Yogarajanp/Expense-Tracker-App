using ExpenseTracker.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrakerLibrary.Model
{
    [Table("UserRole")]

    public class UserRole
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "User Role ID")]
        public int UserRoleID { get; set; }

        [ForeignKey("Users")]
        [Display(Name = "User ID")]
        public int UserId { get; set; }

        [ForeignKey("Role")]
        [Display(Name = "Role ID")]
        public int RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual Users? Users { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role? Role { get; set; }
    }
}
