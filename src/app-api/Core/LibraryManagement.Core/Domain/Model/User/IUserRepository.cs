using System;
using System.Threading.Tasks;
using LibraryManagement.Core.Persistence;

namespace LibraryManagement.Core.Domain.Model
{
    public interface IUserRepository : IRepository<User>
    {
        void Add(User user);

        Task<User> GetById(Guid id);
        Task<User> GetByUsername(string username);
    }
}
