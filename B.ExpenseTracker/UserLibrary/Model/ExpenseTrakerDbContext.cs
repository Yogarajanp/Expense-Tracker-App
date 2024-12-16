using ExpenseTrakerLibrary.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    

 public class ExpenseTrakerDbContext : DbContext
{
    public ExpenseTrakerDbContext()
    {

    }
    public ExpenseTrakerDbContext(DbContextOptions<ExpenseTrakerDbContext> options) : base(options)
    {

    }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Category> Categories  { get; set; }
    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

   /*     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=(localdb)\MSSQLLocalDB; database=ExpenseTrackerDB; integrated security=true");
        }*/
    }
}