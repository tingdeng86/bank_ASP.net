using ASP_Assignment.Models;
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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
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
