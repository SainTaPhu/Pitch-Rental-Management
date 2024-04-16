using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace UnityComplex.Models
{
    public partial class UnityteamContext : DbContext
    {
        public UnityteamContext()
        {
        }

        public UnityteamContext(DbContextOptions<UnityteamContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingService> BookingServices { get; set; } = null!;
        public virtual DbSet<Field> Fields { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Refund> Refunds { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Service> Services { get; set; } = null!;
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; } = null!;
        public virtual DbSet<Sport> Sports { get; set; } = null!;
        public virtual DbSet<Time> Times { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.\\;Initial Catalog=Unityteam;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.IdBooking)
                    .HasName("PK__Booking__473534427360905A");

                entity.ToTable("Booking");

                entity.Property(e => e.IdBooking).HasColumnName("ID_Booking");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.EndTime).HasColumnName("End_Time");

                entity.Property(e => e.IdField).HasColumnName("ID_Field");

                entity.Property(e => e.StartTime).HasColumnName("Start_Time");

                entity.Property(e => e.TotalPrice).HasColumnName("Total_Price");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.IdFieldNavigation)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.IdField)
                    .HasConstraintName("FK_Booking_Field");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Booking_User");
            });

            modelBuilder.Entity<BookingService>(entity =>
            {
                entity.HasKey(e => new { e.IdService, e.IdBooking })
                    .HasName("PK__BookingS__B84E19AB1F3E8179");

                entity.ToTable("BookingService");

                entity.Property(e => e.IdService).HasColumnName("ID_Service");

                entity.Property(e => e.IdBooking).HasColumnName("ID_Booking");

                entity.HasOne(d => d.IdBookingNavigation)
                    .WithMany(p => p.BookingServices)
                    .HasForeignKey(d => d.IdBooking)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderService_Booking");

                entity.HasOne(d => d.IdServiceNavigation)
                    .WithMany(p => p.BookingServices)
                    .HasForeignKey(d => d.IdService)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderService_Service");
            });

            modelBuilder.Entity<Field>(entity =>
            {
                entity.HasKey(e => e.IdField)
                    .HasName("PK__Field__6F43C2B5A7244477");

                entity.ToTable("Field");

                entity.Property(e => e.IdField).HasColumnName("ID_Field");

                entity.Property(e => e.FieldName)
                    .HasMaxLength(255)
                    .HasColumnName("Field_Name");

                entity.Property(e => e.IdSport).HasColumnName("ID_Sport");

                entity.HasOne(d => d.IdSportNavigation)
                    .WithMany(p => p.Fields)
                    .HasForeignKey(d => d.IdSport)
                    .HasConstraintName("FK_Field_Sport");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.IdImage)
                    .HasName("PK__Image__31E45A2A02A4970F");

                entity.ToTable("Image");

                entity.Property(e => e.IdImage).HasColumnName("ID_Image");

                entity.Property(e => e.IdField).HasColumnName("ID_Field");

                entity.Property(e => e.Image1)
                    .HasMaxLength(255)
                    .HasColumnName("Image");

                entity.HasOne(d => d.IdFieldNavigation)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.IdField)
                    .HasConstraintName("FK_Image_Field2");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.IdPayment)
                    .HasName("PK__Payment__C2118ADE0898B1E9");

                entity.ToTable("Payment");

                entity.Property(e => e.IdPayment).HasColumnName("ID_Payment");

                entity.Property(e => e.Deposit).HasColumnType("money");

                entity.Property(e => e.IdBooking).HasColumnName("ID_Booking");

                entity.Property(e => e.IdField).HasColumnName("ID_Field");

                entity.Property(e => e.IdUser).HasColumnName("ID_User");

                entity.Property(e => e.PaymentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("Payment_Time");

                entity.HasOne(d => d.IdBookingNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.IdBooking)
                    .HasConstraintName("FK_Payment_Booking");

                entity.HasOne(d => d.IdFieldNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.IdField)
                    .HasConstraintName("FK_Payment_Field");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Payment_User");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => e.IdRating)
                    .HasName("PK__Rating__232ADA48BF50A7EB");

                entity.ToTable("Rating");

                entity.HasIndex(e => e.IdBooking, "IX_Rating")
                    .IsUnique();

                entity.Property(e => e.IdRating).HasColumnName("ID_Rating");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.IdBooking).HasColumnName("ID_Booking");

                entity.Property(e => e.IdField).HasColumnName("ID_Field");

                entity.Property(e => e.RatingValue).HasColumnName("Rating_Value");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.HasOne(d => d.IdBookingNavigation)
                    .WithOne(p => p.Rating)
                    .HasForeignKey<Rating>(d => d.IdBooking)
                    .HasConstraintName("FK_Rating_Booking");
            });

            modelBuilder.Entity<Refund>(entity =>
            {
                entity.HasKey(e => e.IdRefund)
                    .HasName("PK__Refund__4A49DE206A9F4C03");

                entity.ToTable("Refund");

                entity.Property(e => e.IdRefund).HasColumnName("ID_Refund");

                entity.Property(e => e.IdPayment).HasColumnName("ID_Payment");

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.UserVnpay)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("User_VNPAY");

                entity.HasOne(d => d.IdPaymentNavigation)
                    .WithMany(p => p.Refunds)
                    .HasForeignKey(d => d.IdPayment)
                    .HasConstraintName("FK_Refund_Payment");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PK__Role__43DCD32DD7CCCE65");

                entity.ToTable("Role");

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.Role1)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.IdService)
                    .HasName("PK__Service__8C3D4AEF0E4B7FBC");

                entity.ToTable("Service");

                entity.Property(e => e.IdService).HasColumnName("ID_Service");

                entity.Property(e => e.Decription).HasMaxLength(255);

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.ServiceName)
                    .HasMaxLength(255)
                    .HasColumnName("Service_Name");

                entity.HasOne(d => d.IdCategoryNavigation)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.IdCategory)
                    .HasConstraintName("FK_Service_ServiceCategory");
            });

            modelBuilder.Entity<ServiceCategory>(entity =>
            {
                entity.HasKey(e => e.IdCategory)
                    .HasName("PK__ServiceC__6DB3A68AE7AEF709");

                entity.ToTable("ServiceCategory");

                entity.Property(e => e.IdCategory).HasColumnName("ID_Category");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(255)
                    .HasColumnName("Category_Name");
            });

            modelBuilder.Entity<Sport>(entity =>
            {
                entity.HasKey(e => e.IdSport)
                    .HasName("PK__Sport__74BD78DC71D110C0");

                entity.ToTable("Sport");

                entity.Property(e => e.IdSport).HasColumnName("ID_Sport");

                entity.Property(e => e.SportName).HasMaxLength(255);
            });

            modelBuilder.Entity<Time>(entity =>
            {
                entity.HasKey(e => e.IdTime);

                entity.ToTable("Time");

                entity.Property(e => e.IdTime).HasColumnName("id_Time");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.HasIndex(e => e.Email, "UQ__User__A9D1053414D6F9CE")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("User_ID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(255);

                entity.Property(e => e.IdRole).HasColumnName("ID_Role");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_User_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
