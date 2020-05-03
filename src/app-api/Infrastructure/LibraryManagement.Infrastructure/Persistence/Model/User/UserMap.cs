using LibraryManagement.Infrastructure.Persistence.Mapping;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    public class UserMap : EntityMappingConfiguration<UserData>
    {
        public override void Configure(EntityTypeBuilder<UserData> entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(t => t.Id);

            entityTypeBuilder.Property(t => t.UserName)
                .HasMaxLength(35)
                .IsRequired();

            entityTypeBuilder.Property(t => t.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            entityTypeBuilder.Property(t => t.LastName)
                .HasMaxLength(50)
                .IsRequired();


            entityTypeBuilder
                .HasMany(u => u.UserBooks)
                .WithOne(ub => ub.User);

            base.Configure(entityTypeBuilder);

        }
    }
}