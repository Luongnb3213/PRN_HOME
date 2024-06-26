﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRN221_Assignment.Respository;

#nullable disable

namespace PRN221_Assignment.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PRN221_Assignment.Models.Account", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<bool?>("isActive")
                        .HasColumnType("bit");

                    b.HasKey("UserID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Block", b =>
                {
                    b.Property<int>("BlockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlockId"), 1L, 1);

                    b.Property<int>("BlockUserID")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("BlockId");

                    b.HasIndex("UserID");

                    b.ToTable("Block");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CommentId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("React")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.CommentImages", b =>
                {
                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<string>("Media")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CommentId");

                    b.ToTable("CommentImages");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Conversation", b =>
                {
                    b.Property<int>("ConversationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ConversationId"), 1L, 1);

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("ThreadCommentId")
                        .HasColumnType("int");

                    b.HasKey("ConversationId");

                    b.HasIndex("ThreadCommentId");

                    b.ToTable("Conversation");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Follow", b =>
                {
                    b.Property<int>("FollowerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FollowerId"), 1L, 1);

                    b.Property<int>("UserFollowErId")
                        .HasColumnType("int");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FollowerId");

                    b.HasIndex("UserID");

                    b.ToTable("Follow");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupId"), 1L, 1);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdBy")
                        .HasColumnType("datetime2");

                    b.HasKey("GroupId");

                    b.ToTable("Group");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.GroupUser", b =>
                {
                    b.Property<int>("GroupUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GroupUserId"), 1L, 1);

                    b.Property<int>("GroupId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("GroupUserId");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Info", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Dob")
                        .HasColumnType("datetime2");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Story")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("userName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Info");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Mess", b =>
                {
                    b.Property<int>("messId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("messId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("createdBy")
                        .HasColumnType("datetime2");

                    b.Property<bool>("type")
                        .HasColumnType("bit");

                    b.HasKey("messId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Mess");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.MessageReceive", b =>
                {
                    b.Property<int>("MessageReceiveId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageReceiveId"), 1L, 1);

                    b.Property<int?>("GroupID")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("messID")
                        .HasColumnType("int");

                    b.Property<bool>("seen")
                        .HasColumnType("bit");

                    b.Property<bool>("type")
                        .HasColumnType("bit");

                    b.HasKey("MessageReceiveId");

                    b.HasIndex("GroupID");

                    b.HasIndex("UserId");

                    b.HasIndex("messID")
                        .IsUnique();

                    b.ToTable("MessageReceive");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Thread", b =>
                {
                    b.Property<int>("ThreadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThreadId"), 1L, 1);

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("React")
                        .HasColumnType("int");

                    b.Property<int>("Share")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ThreadId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Thread");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadComment", b =>
                {
                    b.Property<int>("ThreadCommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThreadCommentId"), 1L, 1);

                    b.Property<int>("CommentId")
                        .HasColumnType("int");

                    b.Property<int>("ThreadId")
                        .HasColumnType("int");

                    b.HasKey("ThreadCommentId");

                    b.HasIndex("CommentId");

                    b.HasIndex("ThreadId");

                    b.ToTable("ThreadComment");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadImages", b =>
                {
                    b.Property<int>("ThreadImageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThreadImageId"), 1L, 1);

                    b.Property<string>("Media")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ThreadId")
                        .HasColumnType("int");

                    b.HasKey("ThreadImageId");

                    b.HasIndex("ThreadId");

                    b.ToTable("ThreadImages");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadReact", b =>
                {
                    b.Property<int>("ThreadReactId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ThreadReactId"), 1L, 1);

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("threadId")
                        .HasColumnType("int");

                    b.HasKey("ThreadReactId");

                    b.HasIndex("threadId");

                    b.ToTable("ThreadReact");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Block", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Account", "Account")
                        .WithMany("BLocks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Comment", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.CommentImages", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Comment", "Comment")
                        .WithMany("CommentImages")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Conversation", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.ThreadComment", "ThreadComment")
                        .WithMany("Conversations")
                        .HasForeignKey("ThreadCommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ThreadComment");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Follow", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Account", "Account")
                        .WithMany("Follows")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.GroupUser", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Group", "Group")
                        .WithMany("GroupUser")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN221_Assignment.Models.Account", "User")
                        .WithMany("GroupUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Info", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Account", "Account")
                        .WithOne("Info")
                        .HasForeignKey("PRN221_Assignment.Models.Info", "UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Mess", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Account", "Author")
                        .WithMany("Mess")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.MessageReceive", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Group", "Group")
                        .WithMany("MessageReceive")
                        .HasForeignKey("GroupID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN221_Assignment.Models.Account", "Author")
                        .WithMany("MessageReceives")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN221_Assignment.Models.Mess", "Mess")
                        .WithOne("MessageReceive")
                        .HasForeignKey("PRN221_Assignment.Models.MessageReceive", "messID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Group");

                    b.Navigation("Mess");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Thread", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Account", "Account")
                        .WithMany("Threads")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadComment", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Comment", "Comment")
                        .WithMany("ThreadComments")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRN221_Assignment.Models.Thread", "Thread")
                        .WithMany("ThreadComments")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("Thread");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadImages", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Thread", "Thread")
                        .WithMany("ThreadImages")
                        .HasForeignKey("ThreadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Thread");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadReact", b =>
                {
                    b.HasOne("PRN221_Assignment.Models.Thread", "Thread")
                        .WithMany("ThreadReacts")
                        .HasForeignKey("threadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Thread");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Account", b =>
                {
                    b.Navigation("BLocks");

                    b.Navigation("Comments");

                    b.Navigation("Follows");

                    b.Navigation("GroupUser");

                    b.Navigation("Info");

                    b.Navigation("Mess");

                    b.Navigation("MessageReceives");

                    b.Navigation("Threads");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Comment", b =>
                {
                    b.Navigation("CommentImages");

                    b.Navigation("ThreadComments");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Group", b =>
                {
                    b.Navigation("GroupUser");

                    b.Navigation("MessageReceive");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Mess", b =>
                {
                    b.Navigation("MessageReceive");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.Thread", b =>
                {
                    b.Navigation("ThreadComments");

                    b.Navigation("ThreadImages");

                    b.Navigation("ThreadReacts");
                });

            modelBuilder.Entity("PRN221_Assignment.Models.ThreadComment", b =>
                {
                    b.Navigation("Conversations");
                });
#pragma warning restore 612, 618
        }
    }
}
