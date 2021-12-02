using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ASP_Assignment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public class MyRegisteredUser
        {
            [Display(Name = "First Name")]
            [Required]
            [RegularExpression(@"[a-zA-Z]{1,40}",
                        ErrorMessage = "This is not a valid first name.")]
            public string FirstName { get; set; }


            [Display(Name = "Last Name")]
            [Required]
            [RegularExpression(@"[a-zA-Z]{1,40}",
                        ErrorMessage = "This is not a valid last name.")]
            public string LastName { get; set; }

            [Key]
            [Required]
            [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                        ErrorMessage = "This is not a valid email.")]
            public string Email { get; internal set; }

            [Display(Name = "Account Type")]
            [Required]
            public AccountType AccountType { get; set; }

            [Display(Name = "Balance")]
            [Required]
            [RegularExpression(@"^[1-9]\d*(\.\d{2})",
                        ErrorMessage = "Balance should be a number which must include two digits to the right of the decimal.")]
            public decimal Balance { get; set; }
        }

        public enum AccountType
        {

            Chequing,
            Savings
        }
        public class Client
        {
            [Key]
            public int clientID { get; set; }
            public string lastName { get; set; }
            public string firstName { get; set; }
            public string email { get; set; }
            public virtual ICollection<ClientAccount> ClientAccounts { get; set; }

        }
        public class BankAccount
        {
            [Key]
            public int accountNum { get; set; }
            public string accountType { get; set; }
            public decimal balance { get; set; }
            public virtual ICollection<ClientAccount> ClientAccounts { get; set; }

        }
        public class ClientAccount
        {
            [Key, Column(Order = 0)]
            public int clientID { get; set; }
            [Key, Column(Order = 1)]
            public int accountNum { get; set; }
            public virtual Client Client { get; set; }
            public virtual BankAccount BankAccount { get; set; }
        }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ClientAccount> ClientAccounts { get; set; }
        public DbSet<MyRegisteredUser> MyRegisteredUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientAccount>()
                .HasKey(ca => new { ca.clientID, ca.accountNum });
            modelBuilder.Entity<ClientAccount>()
                .HasOne(c => c.Client)
                .WithMany(c => c.ClientAccounts)
                .HasForeignKey(fk => new { fk.clientID })
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClientAccount>()
                .HasOne(c => c.BankAccount)
                .WithMany(c => c.ClientAccounts)
                .HasForeignKey(fk => new { fk.accountNum })
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }


    }


}
