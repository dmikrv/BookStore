using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using Book_Store.Models;

#nullable disable

namespace Book_Store
{
    public partial class BookStoreContext : DbContext
    {
        public BookStoreContext()
        {
        }

        public BookStoreContext(DbContextOptions<BookStoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookDiscount> BookDiscounts { get; set; }
        public virtual DbSet<ContinuationBook> ContinuationBooks { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<DecommissionedBook> DecommissionedBooks { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Human> Humans { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BookStore;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.HumanId)
                    .HasName("PK__Author__119BA7BC0319B869");

                entity.ToTable("Author");

                entity.Property(e => e.HumanId).ValueGeneratedNever();

                entity.HasOne(d => d.Human)
                    .WithOne(p => p.Author)
                    .HasForeignKey<Author>(d => d.HumanId)
                    .HasConstraintName("Author_HumanId_Human_id");
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
                    .HasConstraintName("Book_AuthorId_Author_HumanId");

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
                    .HasName("PK__BookDisc__43A334DEE384E0A7");

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
                    .HasName("PK__Continua__3DE0C2075A53D891");

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
                entity.HasKey(e => e.HumanId)
                    .HasName("PK__Customer__119BA7BC064950AE");

                entity.ToTable("Customer");

                entity.Property(e => e.HumanId).ValueGeneratedNever();

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.Human)
                    .WithOne(p => p.Customer)
                    .HasForeignKey<Customer>(d => d.HumanId)
                    .HasConstraintName("Customer_HumanId_Human_id");
            });

            modelBuilder.Entity<DecommissionedBook>(entity =>
            {
                entity.HasKey(e => e.BookId)
                    .HasName("PK__Decommis__3DE0C20791B105EC");

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

            modelBuilder.Entity<Human>(entity =>
            {
                entity.ToTable("Human");

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

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => new { e.BookId, e.CustomerId })
                    .HasName("PK__Order__A7AA244AFD9A4AF1");

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
                    .HasConstraintName("Order_CustomerId_Customer_HumanId");
            });

            modelBuilder.Entity<Publisher>(entity =>
            {
                entity.ToTable("Publisher");

                entity.HasIndex(e => e.Name, "UQ__Publishe__737584F615164489")
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
