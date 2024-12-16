using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Model
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "User ID")]
        public int UserID { get; set; }
        public int DeleteID { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [StringLength(20)]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Mail Address")]
        [Required(ErrorMessage = "Email is Required")]
        [StringLength(25)]
        [Display(Name = "E-Mail ID")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(200)]
        [Display(Name = "Password")]
        public string Password { get; set; }


    }
}
