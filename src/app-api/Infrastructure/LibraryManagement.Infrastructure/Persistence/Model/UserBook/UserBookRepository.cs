using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Domain.Model;
using LibraryManagement.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    public sealed class UserBookRepository : IUserBookRepository
    {
        private readonly IBaseRepository<UserBook, UserBookData> _baseRepository;


        /// <summary>
        ///     UserBookRepository ctor.
        /// </summary>
        /// <param name="baseRepository">The base repository containing all the CRUD actions</param>
        public UserBookRepository(IBaseRepository<UserBook, UserBookData> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <inheritdoc />
        public async  Task AddAsync(UserBook userBook)
        {
           await  _baseRepository.Add(userBook);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(UserBook userBook)
        {
            await _baseRepository.Delete(userBook.Id);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(UserBook userBook)
        {
            await _baseRepository.Update(userBook);
        }

        /// <inheritdoc />
        public async Task<UserBook> GetByIdAsync(Guid id)
        {
            var userBook = await _baseRepository.GetById(id);

            if (userBook == null)
            {
                throw new Exception("Entity does not exist!");
            }

            return new UserBook(userBook.Id, userBook.UserId, userBook.BookId);
        }

        /// <inheritdoc />
        public async Task<IList<UserBook>> GetByUserIdAsync(Guid userId)
        {
            return await _baseRepository.TableNoTracking
                .Where(w => w.UserId.Equals(userId))
                .Select(s => new UserBook(s.Id, s.UserId, s.BookId))
                .ToListAsync();
        }

        public Task<bool> IdExists(Guid id)
        {
            return _baseRepository.IdExists(id);
        }
    }
}
