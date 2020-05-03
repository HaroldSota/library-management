using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryManagement.Application.Model.BookModel;
using LibraryManagement.Core.Domain.Model;
using MediatR;

namespace LibraryManagement.Application.Handlers.BookHandler
{
    public sealed class BorrowHandler: IRequestHandler<BorrowRequest, MessageResponse<BorrowResponse>>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUserBookRepository _userBookRepository;
        private List<string> Errors { get; } = new List<string>();


        public BorrowHandler(IBookRepository bookRepository, IUserBookRepository userBookRepository)
        {
            _bookRepository = bookRepository;
            _userBookRepository = userBookRepository;
        }


        public async Task<MessageResponse<BorrowResponse>> Handle(BorrowRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var book = await _bookRepository.GetById(request.BookId);
                if (!await  Validate(request, book))
                    return new MessageResponse<BorrowResponse>(MessageType.Validation, new MessageResponseError("Validate", Errors.First()));
              
                book.AvailableCopies--;
                await _bookRepository.Update(book);
                await _userBookRepository.AddAsync(new UserBook(request.UserId, request.BookId));

                return new MessageResponse<BorrowResponse>(new BorrowResponse());
            }
            catch (Exception e)
            {
                return new MessageResponse<BorrowResponse>(MessageType.Error, e);
            }
        }

        private async Task<bool> Validate(BorrowRequest request, Book book)
        {
            if(book == null)
                Errors.Add($"The requested book with id '{request.BookId}' not found!");
            else if(book.AvailableCopies == 0)
                Errors.Add($"There are no copies available '{book.Title}', please try latter!");
            else
            {
                var borrowedBooks = await _userBookRepository.GetByUserIdAsync(request.UserId);
                if (borrowedBooks.Count >= 2)
                {
                    Errors.Add($"Not allowed to have more than two books borrowed!");
                }
            }
            return Errors.Count == 0;
        }
    }
}