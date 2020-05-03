using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using MediatR;

namespace LibraryManagement.Application.Model.BookModel
{
    public sealed class GetBorrowedBooksByUserRequest: IRequest<MessageResponse<GetBorrowedBooksByUserResponse[]>>
    {
        public Guid UserId { get; set; }
    }

    public sealed class GetBorrowedBooksByUserResponse
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; }
    }
}
