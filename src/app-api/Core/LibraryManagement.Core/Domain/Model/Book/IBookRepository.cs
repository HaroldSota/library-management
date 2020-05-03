using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Core.Persistence;

namespace LibraryManagement.Core.Domain.Model
{
    public interface IBookRepository : IRepository<Book>
    {
        Task Add(Book book);

        Task Update(Book book);


        Task<Book> GetById(Guid id);

        Task<IList<Book>> GetAllAsync();
    }
}