using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using LibraryManagement.Application;
using LibraryManagement.Application.Model.BookModel;
using LibraryManagement.Infrastructure.Persistence.Model;
using LibraryManagement.Tests.Infrastructure;
using Xunit;

namespace LibraryManagement.Tests.Integration
{
    [CollectionDefinition("BookControllerTests", DisableParallelization = true)]
    public sealed class BookControllerTests : ControllerTestBase
    {
        private string _authenticationToken;
        private string AuthenticationToken
        {
            get
            {
                if (string.IsNullOrEmpty(_authenticationToken))
                    _authenticationToken = GetAuthenticationToken("admin@gmail.com", "1234567890").Result;

                return _authenticationToken;
            }
        }

        public BookControllerTests()
            : base(new TestServerFixture(), "api/book/")
        {

            TestDbContext.Seed();
        }

        #region [ Get All ]

        [Fact]
        public async Task Get_All_Provides_The_List_Of_Books()
        {
            //Act
            var response = await GetAsync("GetAll", AuthenticationToken);

            //Assert
            var result = response.GetObject<GetAllResponse[]>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);


            result.Should().NotBeNull();
            result.Length.Should().Be(11);
        }

        [Fact]
        public async Task Get_All_Negative_Invalid_Authentication_Token()
        {
            //Act
            var response = await GetAsync("GetAll", "Invalid_Authentication_Token");

            //Assert

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
        #endregion

        #region [ Borrow ]

        [Fact]
        public async Task Borrow_Provides_a_Successful_Action()
        {
            await ClearBorrowedBooks();

            var bookToBorrow = TestDbContext.DbContext.Set<BookData>().First(w => w.AvailableCopies > 0);
            //Act
            var response = await PostFromQueryStringAsync($"Borrow?bookId={bookToBorrow.Id.ToString()}", AuthenticationToken);

            //Assert
            var result = response.GetObject<BorrowResponse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Borrow_Negative_Invalid_Authentication_Token()
        {
            await ClearBorrowedBooks();

            var bookToBorrow = TestDbContext.DbContext.Set<BookData>().First(w => w.AvailableCopies > 0);
            //Act
            var response = await PostFromQueryStringAsync($"Borrow?bookId={bookToBorrow.Id.ToString()}",  "Invalid_Authentication_Token");

            //Assert

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Borrow_Two_Books_Provides_a_Successful_Action()
        {
            await ClearBorrowedBooks();

            var booksToBorrow = TestDbContext.DbContext
                                                .Set<BookData>()
                                                .Where(w => w.AvailableCopies > 0)
                                                .Take(2)
                                                .ToList();
            //Act
            var response1 = await PostFromQueryStringAsync($"Borrow?bookId={booksToBorrow[0].Id.ToString()}", AuthenticationToken);
            var response2 = await PostFromQueryStringAsync($"Borrow?bookId={booksToBorrow[1].Id.ToString()}", AuthenticationToken);

            //Assert
            var result1 = response1.GetObject<BorrowResponse>();
            response1.StatusCode.Should().Be(HttpStatusCode.OK);
            result1.Should().NotBeNull();

            var result2 = response1.GetObject<BorrowResponse>();
            response2.StatusCode.Should().Be(HttpStatusCode.OK);
            result2.Should().NotBeNull();
        }

        [Fact]
        public async Task Borrow_Negative_Book_Does_Not_Exists()
        {
            //Act
            var response = await PostFromQueryStringAsync($"Borrow?bookId={Guid.Empty.ToString()}", AuthenticationToken);

            //Assert
            var result = response.GetObject<MessageResponseError>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be($"The requested book with id '{Guid.Empty.ToString()}' not found!");
        }

        [Fact]
        public async Task Borrow_Negative_Borrow_Three_Books()
        {
            await ClearBorrowedBooks();
            var booksToBorrow = TestDbContext.DbContext
                .Set<BookData>()
                .Where(w => w.AvailableCopies > 0)
                .Take(3)
                .ToList();
            //Act
            await PostFromQueryStringAsync($"Borrow?bookId={booksToBorrow[0].Id}", AuthenticationToken);

            await PostFromQueryStringAsync($"Borrow?bookId={booksToBorrow[1].Id}", AuthenticationToken);

            var response = await PostFromQueryStringAsync($"Borrow?bookId={booksToBorrow[2].Id}", AuthenticationToken);



            //Assert
            var result = response.GetObject<MessageResponseError>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be("Not allowed to have more than two books borrowed!");
        }

        [Fact]
        public async Task Borrow_Negative_No_Available_Copy()
        {
            await ClearBorrowedBooks();
            var bookToBorrow = TestDbContext.DbContext
                .Set<BookData>()
                .First(f => f.AvailableCopies == 0);
            //Act
            var response = await PostFromQueryStringAsync($"Borrow?bookId={bookToBorrow.Id}", AuthenticationToken);


            //Assert
            var result = response.GetObject<MessageResponseError>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be($"There are no copies available '{bookToBorrow.Title}', please try latter!");
        }

        #endregion

        #region [ GetBorrowedBooksByUser ]
        [Fact]
        public async Task GetBorrowedBooksByUser_Provides_The_List_Of_Books()
        {
            await ClearBorrowedBooks();

            await Borrow_Two_Books_Provides_a_Successful_Action();
            //Act
            var response = await GetAsync("GetBorrowedBooksByUser", AuthenticationToken);

            //Assert
            var result = response.GetObject<GetAllResponse[]>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);


            result.Should().NotBeNull();
            result.Length.Should().Be(2);
        }

        [Fact]
        public async Task GetBorrowedBooksByUser_Negative_Invalid_Authentication_Token()
        {
            await ClearBorrowedBooks();

            //Act
            var response = await GetAsync("GetBorrowedBooksByUser", "Invalid_Authentication_Token");

            //Assert

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }


        #endregion

        #region [ ReturnBook ]
        [Fact]
        public async Task ReturnBook_Provides_a_Successful_Action()
        {
            await ClearBorrowedBooks();
            await Borrow_Provides_a_Successful_Action();

            var borrowedBook = TestDbContext.DbContext.Set<UserBookData>().First();
            //Act
            var response = await PostFromQueryStringAsync($"ReturnBook?borrowId={borrowedBook.Id.ToString()}", AuthenticationToken);

            //Assert
            var result = response.GetObject<BorrowResponse>();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task ReturnBook_Negative_Invalid_Authentication_Token()
        {
            await ClearBorrowedBooks();
            await Borrow_Provides_a_Successful_Action();

            var borrowedBook = TestDbContext.DbContext.Set<UserBookData>().First();
            //Act
            var response = await PostFromQueryStringAsync($"ReturnBook?borrowId={borrowedBook.Id.ToString()}", "Invalid_Authentication_Token");

            //Assert

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task ReturnBook_Negative_Book_Not_Borrowed_By_Current_User()
        {
            await ClearBorrowedBooks();
            //Borrow the book from another account
            _authenticationToken = GetAuthenticationToken("immanuel.fuchs@gmail.com", "my-super-secret-password").Result;
            await Borrow_Provides_a_Successful_Action();



            _authenticationToken = GetAuthenticationToken("admin@gmail.com", "1234567890").Result;
            var borrowedBook = TestDbContext.DbContext.Set<UserBookData>().First();
            //Act
            var response = await PostFromQueryStringAsync($"ReturnBook?borrowId={borrowedBook.Id.ToString()}", AuthenticationToken);

            //Assert
            var result = response.GetObject<MessageResponseError>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be($"The book with id '{borrowedBook.BookId}'is not borrowed by the user!");

        }

        [Fact]
        public async Task ReturnBook_Negative_Borrow_Id_Not_Found()
        {
            await ClearBorrowedBooks();
            //Act
            var response = await PostFromQueryStringAsync($"ReturnBook?borrowId={Guid.Empty}", AuthenticationToken);

            //Assert
            var result = response.GetObject<MessageResponseError>();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            result.Message.Should().Be($"The requested borrow with id '{Guid.Empty}' not found!");
        }
        #endregion


        private async Task ClearBorrowedBooks()
        {
            TestDbContext.DbContext.Set<UserBookData>().RemoveRange(
                TestDbContext.DbContext
                    .Set<UserBookData>()
                    .Where(w => !w.Id.Equals(Guid.Empty)));
            await TestDbContext.DbContext.SaveChangesAsync();
        }
    }
}