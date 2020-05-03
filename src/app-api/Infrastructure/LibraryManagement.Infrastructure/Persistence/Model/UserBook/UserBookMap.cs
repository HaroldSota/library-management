using LibraryManagement.Infrastructure.Persistence.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    public sealed class UserBookMap : EntityMappingConfiguration<UserBookData>
    {
        public override void Configure(EntityTypeBuilder<UserBookData> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);

            entityTypeBuilder.Property(t => t.UserId)
                .IsRequired();

            entityTypeBuilder.Property(t => t.BookId)
                .IsRequired();

            entityTypeBuilder
                .HasOne(ub=> ub.User)
                .WithMany(u => u.UserBooks);


            entityTypeBuilder
                .HasOne(ub => ub.Book)
                .WithMany(u => u.UserBooks);

            base.Configure(entityTypeBuilder);

        }
    }
}
