using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Persistence;

namespace LibraryManagement.Core.Domain.Model
{
    public interface IUserBookRepository : IRepository<UserBook>
    {
        Task AddAsync(UserBook userBook);

        Task DeleteAsync(UserBook userBook);

        Task UpdateAsync(UserBook userBook);

        Task<bool> IdExists(Guid id);
        Task<UserBook> GetByIdAsync(Guid id);
        Task<IList<UserBook>> GetByUserIdAsync(Guid userId);
    }
}
