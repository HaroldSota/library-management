using System;
using MediatR;

namespace LibraryManagement.Application.Model.BookModel
{
    public sealed class BorrowRequest: IRequest<MessageResponse<BorrowResponse>>
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
    public sealed class BorrowResponse
    {
        
    }
}