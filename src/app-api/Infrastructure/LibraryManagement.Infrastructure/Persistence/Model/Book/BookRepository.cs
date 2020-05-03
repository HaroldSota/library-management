using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Domain.Model;
using LibraryManagement.Core.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    public sealed class BookRepository : IBookRepository
    {
        private readonly IBaseRepository<Book, BookData> _baseRepository;


        /// <summary>
        ///     BookRepository ctor.
        /// </summary>
        /// <param name="dbContext">Database context</param>
        /// <param name="baseRepository">The base repository containing all the CRUD actions</param>
        public BookRepository(IBaseRepository<Book, BookData> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        /// <inheritdoc />
        public async Task<Book> GetById(Guid id)
        {
            return await _baseRepository.TableNoTracking
                .Where(w => w.Id.Equals(id))
                .Select(s => new Book(s.Id, s.Title, s.Author, s.TotalCopies, s.AvailableCopies))
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IList<Book>> GetAllAsync()
        {
            return await _baseRepository.TableNoTracking
                .Select(s=> new Book( s.Id, s.Title, s.Author, s.TotalCopies,s.AvailableCopies ))
                .OrderBy(o=> o.Title)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task Add(Book book)
        {
           await _baseRepository.Add(book);
        }
        /// <inheritdoc />
        public async Task Update(Book book)
        {
            await _baseRepository.Update(book);
        }

    }

}