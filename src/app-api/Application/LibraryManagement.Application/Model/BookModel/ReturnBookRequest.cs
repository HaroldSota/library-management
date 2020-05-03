using System;
using MediatR;

namespace LibraryManagement.Application.Model.BookModel
{
    public sealed class ReturnBookRequest: IRequest<MessageResponse<ReturnBookResponse>>
    {
        public Guid BorrowId { get; set; }
        public Guid UserId { get; set; }
    }

    public sealed class ReturnBookResponse
    {
    }
}
