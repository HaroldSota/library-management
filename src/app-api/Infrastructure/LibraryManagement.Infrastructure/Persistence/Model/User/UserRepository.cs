using System;
using System.Linq;
using LibraryManagement.Core.Domain.Model;
using LibraryManagement.Core.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    public sealed class UserRepository : IUserRepository
    {
        private readonly IBaseRepository<User, UserData> _baseRepository;

        /// <summary>
        ///     UserRepository ctor.
        /// </summary>
        /// <param name="baseRepository">The base repository containing all the CRUD actions</param>
        public UserRepository(IBaseRepository<User, UserData> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <inheritdoc />
        public void Add(User user)
        {
            _baseRepository.Add(user);
        }

        /// <inheritdoc />
        public async Task<User> GetById(Guid id)
        {
            return await _baseRepository.TableNoTracking
                .Where(w => w.Id.Equals(id))
                .Select(s => new User(s.Id, s.UserName, s.FirstName, s.LastName, s.PasswordHash, s.PasswordSalt))
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<User> GetByUsername(string username)
        {
            return await _baseRepository.TableNoTracking
                .Where(w=> w.UserName.Equals(username))
                .Select(s => new User(s.Id, s.UserName, s.FirstName, s.LastName, s.PasswordHash, s.PasswordSalt))
                .FirstOrDefaultAsync();
        }
    }
}
