
using ExpenseTracker.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTrakerLibrary.Model
{
    [Table("Expense")]
    public class Expense
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ExpenseID { get; set; }
        public int DeleteID { get; set; }

        [Required(ErrorMessage = "Category ID is required")]
        [Display(Name = "Category ID")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Display(Name = "User ID")]
        [Required(ErrorMessage = "User ID is required")]
        [ForeignKey("Users")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }

        [Display(Name = "Date Of Transaction")]
        [Required(ErrorMessage = "Date is required")]
        public DateTime DateofTransaction { get; set; }

        [Display(Name = "Mode Of Transaction")]
        [Required(ErrorMessage = "Mode of Transaction is Required")]
        public string ModeOfTransaction { get; set; }


        [Required(ErrorMessage = "Description is Required")]
        [StringLength(250)]
        public string Description { get; set; }


        [StringLength(150)]
        public string? ReceiptPath { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [ForeignKey("UserId")]
        public virtual Users? Users { get; set; }



    }
}
