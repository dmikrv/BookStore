using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

using Book_Store.Models;

#nullable disable

namespace Book_Store
{
    public partial class BookStoreContext : DbContext
    {
        public BookStoreContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }


        public static readonly LoggerFactory MyLoggerFactory = new LoggerFactory(new ILoggerProvider[] { new LogEntryLoggerProvider(), new NLogLoggerProvider() });
        private readonly string _connectionString;

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookDiscount> BookDiscounts { get; set; }
        public virtual DbSet<ContinuationBook> ContinuationBooks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DecommissionedBook> DecommissionedBooks { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(MyLoggerFactory) // Warning: Do not create a new ILoggerFactory instance each time
                    .UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Book");

                entity.Property(e => e.CostPrice).HasColumnType("money");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.Property(e => e.YearPublishing).HasColumnType("date");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("Book_AuthorId_Author_Id");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Book_GenreId_Genre_id");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Book_PublisherId_Publisher_id");
            });

            modelBuilder.Entity<BookDiscount>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.DiscountId })
                    .HasName("PK__BookDisc__43A334DE8C53E623");

                entity.ToTable("BookDiscount");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.BookDiscounts)
                    .HasForeignKey(d => d.BookId)
                    .HasConstraintName("BookDiscount_BookId_Book_id");

                entity.HasOne(d => d.Discount)
                    .WithMany(p => p.BookDiscounts)
                    .HasForeignKey(d => d.DiscountId)
                    .HasConstraintName("BookDiscount_DiscountId_Discount_id");
            });

            modelBuilder.Entity<ContinuationBook>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__Continua__3DE0C2077201045F");

                entity.ToTable("ContinuationBook");

                entity.Property(e => e.BookId).ValueGeneratedNever();

                entity.HasOne(d => d.Book)
                    .WithOne(p => p.ContinuationBookBook)
                    .HasForeignKey<ContinuationBook>(d => d.BookId)
                    .HasConstraintName("ContinuationBook_BookId_Book_id");

                entity.HasOne(d => d.Predecessor)
                    .WithMany(p => p.ContinuationBookPredecessors)
                    .HasForeignKey(d => d.PredecessorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ContinuationBook_PredecessorId_Book_id");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Patronymic)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DecommissionedBook>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__Decommis__3DE0C207F14DBEBB");

                entity.ToTable("DecommissionedBook");

                entity.Property(e => e.BookId).ValueGeneratedNever();

                entity.HasOne(d => d.Book)
                    .WithOne(p => p.DecommissionedBook)
                    .HasForeignKey<DecommissionedBook>(d => d.BookId)
                    .HasConstraintName("DecommissionedBook_BookId_Book_id");
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("Genre");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.CustomerId })
                    .HasName("PK__Order__A7AA244AECCE47CE");

                entity.ToTable("Order");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.HasOne(d => d.Book)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BookId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Order_BookId_Book_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Order_CustomerId_Customer_Id");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("Publisher");

                entity.HasIndex(e => e.Name, "UQ__Publishe__737584F6AE7398BD")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
