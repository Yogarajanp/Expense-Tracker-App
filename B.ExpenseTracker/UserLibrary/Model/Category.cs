using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTrakerLibrary.Model
{
    [Table("Category")]
   public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Category ID")]
        public int CategoryID{ get; set; }

        [Required(ErrorMessage = "Category Type is required")]
        [StringLength(20)]
        [Display(Name = "Category Name")]
        public  string CategoryType { get; set; }
        public int DeleteID { get; set; }   
     
    }
}
