﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryWPF.Model.DBModels;

public partial class MvvmloginDbContext : DbContext
{
    public MvvmloginDbContext()
    {
    }

    public MvvmloginDbContext(DbContextOptions<MvvmloginDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autor> Autors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BookArchive> BookArchives { get; set; }

    public virtual DbSet<ListBookRequest> ListBookRequests { get; set; }

    public virtual DbSet<LoginUser> LoginUsers { get; set; }

    public virtual DbSet<Rack> Racks { get; set; }

    public virtual DbSet<ReadPlace> ReadPlaces { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestArchive> RequestArchives { get; set; }

    public virtual DbSet<RequestListBookRequest> RequestListBookRequests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<TempListBook> TempListBooks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-TMRQF3QP\\LANDROMMSQLSERV; Database=MVVMLoginDb; Integrated Security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>(entity =>
        {
            entity.ToTable("Autor");

            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(e => e.AutorId).HasColumnName("Autor_Id");
            entity.Property(e => e.CheckAvailability).HasComment("True - если книга в налии, false - если книга на руках");
            entity.Property(e => e.Publisher).HasMaxLength(25);
            entity.Property(e => e.Serias).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Autor).WithMany(p => p.Books)
                .HasForeignKey(d => d.AutorId)
                .HasConstraintName("FK_Books_Autor");

            entity.HasOne(d => d.ReadPlaceNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.ReadPlace)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_ReadPlace");

            entity.HasOne(d => d.StackNumberNavigation).WithMany(p => p.Books)
                .HasForeignKey(d => d.StackNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Books_Rack");
        });

        modelBuilder.Entity<BookArchive>(entity =>
        {
            entity.HasKey(e => e.IdBook);

            entity.ToTable("BookArchive");

            entity.Property(e => e.IdBook).HasColumnName("idBook");
            entity.Property(e => e.AutorLastName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.AutorName)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Publisher)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.ReadPlace)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Serias)
                .HasMaxLength(50)
                .IsFixedLength();
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsFixedLength();

            entity.HasOne(d => d.NumberRequestNavigation).WithMany(p => p.BookArchives)
                .HasForeignKey(d => d.NumberRequest)
                .HasConstraintName("FK_BookArchive_RequestArchive");
        });

        modelBuilder.Entity<ListBookRequest>(entity =>
        {
            entity.ToTable("ListBookRequest");

            entity.HasOne(d => d.Book).WithMany(p => p.ListBookRequests)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ListBookRequest_Books");
        });

        modelBuilder.Entity<LoginUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoginUse__3214EC07C62221CE");

            entity.ToTable("LoginUser");

            entity.HasIndex(e => e.Login, "UQ__LoginUse__5E55825BBECD97B9").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Login).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasDefaultValue(2);

            entity.HasOne(d => d.Role).WithMany(p => p.LoginUsers)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_LoginUser_Role");
        });

        modelBuilder.Entity<Rack>(entity =>
        {
            entity.HasKey(e => e.StackNumber);

            entity.ToTable("Rack");

            entity.HasIndex(e => e.StackNumber, "UI_StackNumber");

            entity.Property(e => e.StackNumber).ValueGeneratedNever();
        });

        modelBuilder.Entity<ReadPlace>(entity =>
        {
            entity.ToTable("ReadPlace");

            entity.HasIndex(e => e.ReadPlace1, "UI_ReadPlace");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ReadPlace1)
                .HasMaxLength(50)
                .HasColumnName("ReadPlace");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.Number);

            entity.ToTable("Request");

            entity.Property(e => e.DateRegistrRequest).HasColumnType("datetime");
            entity.Property(e => e.StatusRequest).HasComment("True - Заявка одобрена, false - заявка на рассмотрении.");

            entity.HasOne(d => d.UserCardNumberNavigation).WithMany(p => p.Requests)
                .HasForeignKey(d => d.UserCardNumber)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Request_User");
        });

        modelBuilder.Entity<RequestArchive>(entity =>
        {
            entity.HasKey(e => e.NumberRequest).HasName("PK_RequestArchive_1");

            entity.ToTable("RequestArchive");

            entity.HasIndex(e => e.NumberRequest, "IX_RequestArchive");

            entity.Property(e => e.NumberRequest).ValueGeneratedNever();
            entity.Property(e => e.DateRegistrRequest).HasColumnType("datetime");
        });

        modelBuilder.Entity<RequestListBookRequest>(entity =>
        {
            entity.ToTable("Request_ListBookRequest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdListBook).HasColumnName("idListBook");

            entity.HasOne(d => d.IdListBookNavigation).WithMany(p => p.RequestListBookRequests)
                .HasForeignKey(d => d.IdListBook)
                .HasConstraintName("FK_Request_ListBookRequest_ListBookRequest");

            entity.HasOne(d => d.NumberNavigation).WithMany(p => p.RequestListBookRequests)
                .HasForeignKey(d => d.Number)
                .HasConstraintName("FK_Request_ListBookRequest_Request");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.RoleUsers)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TempListBook>(entity =>
        {
            entity.ToTable("TempListBook");

            entity.HasIndex(e => e.IdBook, "IX_TempListBook").IsUnique();

            entity.HasOne(d => d.CardNumberUserNavigation).WithMany(p => p.TempListBooks)
                .HasForeignKey(d => d.CardNumberUser)
                .HasConstraintName("FK_TempListBook_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.CardNumber);

            entity.ToTable("User");

            entity.HasIndex(e => e.LoginUser, "UQ_LoginUser").IsUnique();

            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LoginUser).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(50);

            entity.HasOne(d => d.LoginUserNavigation).WithOne(p => p.User)
                .HasPrincipalKey<LoginUser>(p => p.Login)
                .HasForeignKey<User>(d => d.LoginUser)
                .HasConstraintName("FK_User_LoginUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
