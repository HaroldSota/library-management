using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Model.BookModel;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Persistence.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Application.Handlers.BookHandler
{
    public sealed class GetBorrowedBooksByUserHandler : IRequestHandler<GetBorrowedBooksByUserRequest, MessageResponse<GetBorrowedBooksByUserResponse[]>>
    {
        private readonly IDbContext _dbContext;
        private List<string> Errors { get; } = new List<string>();

        /// <summary>
        ///     GetAllHandler ctor.
        /// </summary>
        /// <param name="dbContext">Book repository</param>
        public GetBorrowedBooksByUserHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MessageResponse<GetBorrowedBooksByUserResponse[]>> Handle(GetBorrowedBooksByUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
               var result = await _dbContext.Set<UserBookData>()
                    .Where(w => w.UserId.Equals(request.UserId))
                    .Select(s => new GetBorrowedBooksByUserResponse
                    {
                        Id = s.Id,
                        Title = s.Book.Title,
                        Author = s.Book.Author
                    })
                    .ToArrayAsync(cancellationToken);
                
               return new MessageResponse<GetBorrowedBooksByUserResponse[]>(result);
            }
            catch (Exception e)
            {
                return new MessageResponse<GetBorrowedBooksByUserResponse[]>(MessageType.Error, e);
            }
        }
    }
}