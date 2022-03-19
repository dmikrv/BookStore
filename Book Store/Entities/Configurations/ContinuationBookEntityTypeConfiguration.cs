using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Book_Store.Entities.Configurations
{
    internal class ContinuationBookEntityTypeConfiguration : IEntityTypeConfiguration<ContinuationBook>
    {
        public void Configure(EntityTypeBuilder<ContinuationBook> builder)
        {
            builder.HasKey(e => new { e.BookId, e.PredecessorBookId })
                .HasName("PK__Order__A7AA244A5BBE3634");

            builder.ToTable("ContinuationBook");

            builder.HasOne(d => d.Book)
                .WithMany()
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ContinuationBook_BookId_Book_id");

            builder.HasOne(d => d.PredecessorBook)
                .WithMany()
                .HasForeignKey(d => d.PredecessorBookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("ContinuationBook_PredecessorBookId_Book_id");
        }
    }
}
