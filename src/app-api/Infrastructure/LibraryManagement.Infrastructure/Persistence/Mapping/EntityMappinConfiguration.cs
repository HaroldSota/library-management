using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Persistence.Mapping
{
    public partial class EntityMappingConfiguration<TEntity> : IMappingConfiguration, IEntityTypeConfiguration<TEntity>
        where TEntity : class, new()
    {
        public void ApplyConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(this);
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            var tblName = typeof(TEntity).Name;
            var customTableAttribute = typeof(TEntity).GetCustomAttributes(false);
            if (customTableAttribute.Length > 0)
            {
                var attribute = customTableAttribute.FirstOrDefault(a => a.GetType() == typeof(TableAttribute));

                if (attribute != null)
                    tblName = ((TableAttribute)attribute).Name;
            }

            builder.ToTable(tblName);
        }


    }
}
