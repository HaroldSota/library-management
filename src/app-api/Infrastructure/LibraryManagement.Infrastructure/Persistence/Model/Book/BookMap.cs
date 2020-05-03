using LibraryManagement.Infrastructure.Persistence.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    internal class BookMap : EntityMappingConfiguration<BookData>
    {
        public override void Configure(EntityTypeBuilder<BookData> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);


            entityTypeBuilder.Property(t => t.Title)
                .HasMaxLength(150)
                .IsRequired();

            entityTypeBuilder.Property(t => t.Author)
                .HasMaxLength(150)
                .IsRequired();

            entityTypeBuilder
                .HasMany(b => b.UserBooks)
                .WithOne(ub => ub.Book);

            base.Configure(entityTypeBuilder);
        }
    }
}
