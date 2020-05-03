using Autofac;
using LibraryManagement.Core.Configuration;
using LibraryManagement.Core.Persistence;
using LibraryManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Api.Framework.Extensions
{
    /// <summary>
    ///     ContainerBuilderExtensions
    /// </summary>
    public static class ContainerBuilderExtensions
    {
        public static void RegisterApiDependencies(this ContainerBuilder builder, ILibraryManagementConfig appConfig)
        {
            builder.RegisterInstance(appConfig).As<ILibraryManagementConfig>().SingleInstance();
        }

        public static void RegisterPersistenceDependencies(this ContainerBuilder builder)
        {
            builder
                .Register(context => new LibraryManagementObjectContext(context.Resolve<DbContextOptions<LibraryManagementObjectContext>>()))
                .As<IDbContext>()
                .InstancePerLifetimeScope();

            builder
                .RegisterGeneric(typeof(BaseRepository<,>))
                .As(typeof(IBaseRepository<,>));

            builder
                .RegisterAssemblyTypes(typeof(BaseRepository<,>).Assembly)
                .AsClosedTypesOf(typeof(IRepository<>));


  

        }
    }
}
