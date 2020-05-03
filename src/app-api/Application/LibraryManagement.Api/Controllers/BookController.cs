using System;
using System.Threading.Tasks;
using LibraryManagement.Api.Framework.Controllers;
using LibraryManagement.Application.Model.BookModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    /// <summary>
    ///     BookController
    /// </summary>
    public class BookController : BaseApiController
    {
        /// <summary>
        ///  BookController ctor.
        /// </summary>
        /// <param name="bus"></param>
        public BookController(IMediator bus) : base(bus)
        {
        }


        /// <summary>
        ///    Get all books
        /// </summary>
        /// <returns>Return a list of suggestions</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return ToResult(await Bus.Send(new GetAllRequest(){ UserId =  UserId}));
        }

        /// <summary>
        ///    Borrow a book
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Borrow(Guid bookId)
        {
            return ToResult(await Bus.Send(new BorrowRequest() { BookId = bookId, UserId = UserId }));
        }

        /// <summary>
        ///    Borrow a book
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBorrowedBooksByUser()
        {
            return ToResult(await Bus.Send(new GetBorrowedBooksByUserRequest() { UserId = UserId }));
        }

        /// <summary>
        ///    Borrow a book
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ReturnBook(Guid borrowId)
        {
            return ToResult(await Bus.Send(new ReturnBookRequest() { BorrowId = borrowId, UserId = UserId }));
        }
    }
}