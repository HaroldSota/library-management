using AutoMapper;
using LibraryManagement.Core.Domain.Model;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    /// <summary>
    ///     Provides a named configuration for maps for persistence entities.
    ///     Naming conventions become scoped per profile.
    /// </summary>
    public class DomainToDataMappingProfile : Profile
    {
        /// <summary>
        ///     DomainToDataMappingProfile ctor.
        /// </summary>
        public DomainToDataMappingProfile()
        {
            CreateMap<Book, BookData>();
            CreateMap<User, UserData>();
            CreateMap<UserBook, UserBookData>();
        }
    }
}
