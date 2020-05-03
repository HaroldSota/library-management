using LibraryManagement.Core.Domain;

namespace LibraryManagement.Core.Persistence
{
    public interface IRepository<T> where T : BaseEntity
    {
    }
}
