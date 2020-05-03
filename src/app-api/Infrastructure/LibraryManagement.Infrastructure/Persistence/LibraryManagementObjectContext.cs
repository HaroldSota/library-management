using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LibraryManagement.Infrastructure.Persistence.Mapping;
using Microsoft.EntityFrameworkCore;


namespace LibraryManagement.Infrastructure.Persistence
{
    public class LibraryManagementObjectContext : DbContext, IDbContext
    {

        public LibraryManagementObjectContext(DbContextOptions<LibraryManagementObjectContext> options)
            : base(options)
        {

        }

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : class, new()
        {
            return base.Set<TEntity>();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var typeConfigurations = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => (type.BaseType?.IsGenericType ?? false)
                               && type.BaseType.GetGenericTypeDefinition() == typeof(EntityMappingConfiguration<>)
                );

            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration?.ApplyConfiguration(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Seed();
        }
    }
}
