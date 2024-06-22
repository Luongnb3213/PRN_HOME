using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PRN221_Assignment.Models;
using System.Reflection.Metadata;
using Thread = PRN221_Assignment.Models.Thread;

namespace PRN221_Assignment.Respository
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Info> Info { get; set; }
        public DbSet<Block> Block { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<CommentImages> CommentImages { get; set; }

        public DbSet<Conversation> Conversation { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Thread> Thread { get; set; }

        public DbSet<ThreadComment> ThreadComment { get; set; }
        public DbSet<ThreadImages> ThreadImages { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasOne(e => e.Info)
                .WithOne(e => e.Account)
                .HasForeignKey<Info>(e => e.UserID)
            .IsRequired();


            modelBuilder.Entity<Account>()
          .HasMany(e => e.Follows)
          .WithOne(e => e.Account)
          .HasForeignKey(e => e.UserID)
          .IsRequired();

            modelBuilder.Entity<Account>()
          .HasMany(e => e.Mess)
          .WithOne(e => e.Author)
          .HasForeignKey(e => e.AuthorId)
          .IsRequired();


            modelBuilder.Entity<Account>()
          .HasMany(e => e.BLocks)
          .WithOne(e => e.Account)
          .HasForeignKey(e => e.UserID)
          .IsRequired();

            modelBuilder.Entity<Account>()
        .HasMany(e => e.Threads)
        .WithOne(e => e.Account)
        .HasForeignKey(e => e.AuthorId)
        .IsRequired();

            modelBuilder.Entity<Account>()
        .HasMany(e => e.Comments)
        .WithOne(e => e.Account)
        .HasForeignKey(e => e.AuthorId)
        .IsRequired();

            modelBuilder.Entity<Thread>()
      .HasMany(e => e.ThreadImages)
      .WithOne(e => e.Thread)
      .HasForeignKey(e => e.ThreadId)
      .IsRequired();



            modelBuilder.Entity<Comment>()
      .HasMany(e => e.CommentImages)
      .WithOne(e => e.Comment)
      .HasForeignKey(e => e.CommentId)
      .IsRequired();


            modelBuilder.Entity<Thread>()
       .HasMany(e => e.Comments)
       .WithMany(e => e.Threads)
       .UsingEntity<ThreadComment>();


            modelBuilder.Entity<Account>()
             .HasMany(e => e.Group)
             .WithMany(e => e.Accounts)
             .UsingEntity<GroupUser>();


            modelBuilder.Entity<ThreadComment>()
      .HasMany(e => e.Conversations)
      .WithOne(e => e.ThreadComment)
      .HasForeignKey(e => e.ThreadCommentId)
      .IsRequired();


            modelBuilder.Entity<Mess>()
                .HasOne(e => e.MessageReceive)
                .WithOne(e => e.Mess)
                .HasForeignKey<MessageReceive>(e => e.messID)
            .IsRequired();

            modelBuilder.Entity<Group>()
     .HasMany(e => e.MessageReceive)
     .WithOne(e => e.Group)
     .HasForeignKey(e => e.GroupID)
     .IsRequired();
        }
    }
}
