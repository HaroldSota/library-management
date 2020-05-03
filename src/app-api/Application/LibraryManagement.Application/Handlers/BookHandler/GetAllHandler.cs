using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Model.BookModel;
using LibraryManagement.Core.Domain.Model;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Persistence.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace LibraryManagement.Application.Handlers.BookHandler
{
    public sealed class GetAllHandler : IRequestHandler<GetAllRequest, MessageResponse<GetAllResponse[]>>
    {
        private readonly IDbContext _dbContext;
        private List<string> Errors { get; } = new List<string>();

        /// <summary>
        ///     GetAllHandler ctor.
        /// </summary>
        /// <param name="dbContext">Book repository</param>
        public GetAllHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<MessageResponse<GetAllResponse[]>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _dbContext.Set<BookData>()
                    .SelectMany(sm => sm.UserBooks
                                                .Where(ub => ub.UserId.Equals(request.UserId))
                                                .DefaultIfEmpty(),
                        (b, ub) =>
                        new GetAllResponse
                        {
                            Id = b.Id,
                            Title = b.Title,
                            Author = b.Author,
                            IsAvailable = b.AvailableCopies > 0 && ub == null
                        }
                    )
                    .AsNoTracking()
                    .ToArrayAsync(cancellationToken: cancellationToken);


                return new MessageResponse<GetAllResponse[]>(result);
            }
            catch (Exception e)
            {
                return new MessageResponse<GetAllResponse[]>(MessageType.Error, e);
            }

        }
    }
}