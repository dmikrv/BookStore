using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store.Entities.Configurations
{
    internal class BookDiscountEntityTypeConfiguration : IEntityTypeConfiguration<BookDiscount>
    {
        public void Configure(EntityTypeBuilder<BookDiscount> builder)
        {
            builder.HasKey(e => new { e.BookId, e.DiscountId })
                .HasName("PK__BookDisc__43A334DED432C3C2");

            builder.ToTable("BookDiscount");

            builder.HasOne(d => d.Book)
                .WithMany(p => p.BookDiscounts)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("BookDiscount_BookId_Book_id");

            builder.HasOne(d => d.Discount)
                .WithMany(p => p.BookDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("BookDiscount_DiscountId_Discount_id");
        }
    }
}
