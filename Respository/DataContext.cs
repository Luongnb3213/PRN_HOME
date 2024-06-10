using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PRN221_Assignment.Models;
using System.Reflection.Metadata;

namespace PRN221_Assignment.Respository
{
    public class DataContext :DbContext
    { 
        public DataContext(DbContextOptions<DataContext> options) : base(options) {
           
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Info> Info { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(e => e.Info)
                .WithOne(e => e.Account)
                .HasForeignKey<Info>(e => e.UserID)
                .IsRequired();
        }
    }
}
