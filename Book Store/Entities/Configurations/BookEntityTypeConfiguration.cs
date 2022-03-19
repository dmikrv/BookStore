using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store.Entities.Configurations
{
    internal class BookEntityTypeConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Book");

            builder.Property(e => e.CostPrice).HasColumnType("money");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            builder.Property(e => e.Price).HasColumnType("money");

            builder.Property(e => e.YearPublishing).HasColumnType("date");

            builder.HasOne(d => d.Author)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("Book_AuthorId_Author_Id");

            builder.HasOne(d => d.Genre)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Book_GenreId_Genre_id");

            builder.HasOne(d => d.Publisher)
                .WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Book_PublisherId_Publisher_id");

            builder.HasOne(d => d.Image)
                .WithOne(p => p.Book)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("Book_ImageId_Image_id");
        }
    }
}
