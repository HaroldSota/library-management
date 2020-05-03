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
    public sealed class ReturnBookHandler : IRequestHandler<ReturnBookRequest, MessageResponse<ReturnBookResponse>>
    {

        private readonly IBookRepository _bookRepository;
        private readonly IUserBookRepository _userBookRepository;
        private List<string> Errors { get; } = new List<string>();


        public ReturnBookHandler(IBookRepository bookRepository, IUserBookRepository userBookRepository)
        {
            _bookRepository = bookRepository;
            _userBookRepository = userBookRepository;
        }

        public async Task<MessageResponse<ReturnBookResponse>> Handle(ReturnBookRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var borrowData= await _userBookRepository.IdExists(request.BorrowId)
                                      ? await _userBookRepository.GetByIdAsync(request.BorrowId)
                                      : null;

               
                if (!Validate(request, borrowData))
                    return new MessageResponse<ReturnBookResponse>(MessageType.Validation, new MessageResponseError("Validate", Errors.First()));

                var book = await _bookRepository.GetById(borrowData.BookId);
                book.AvailableCopies++;
                await _bookRepository.Update(book);
                await _userBookRepository.DeleteAsync(borrowData);

                return new MessageResponse<ReturnBookResponse>(new ReturnBookResponse());
            }
            catch (Exception e)
            {
                return new MessageResponse<ReturnBookResponse>(MessageType.Error, e);
            }
        }

        private bool Validate(ReturnBookRequest request, UserBook borrowData)
        {
            if (borrowData == null)
                Errors.Add($"The requested borrow with id '{request.BorrowId}' not found!");
            else if(!request.UserId.Equals(borrowData.UserId))
                Errors.Add($"The book with id '{borrowData.BookId}'is not borrowed by the user!");


            return Errors.Count == 0;
        }
    }
}
