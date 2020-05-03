using System;
using System.Text.Json.Serialization;
using MediatR;

namespace LibraryManagement.Application.Model.BookModel
{
    public sealed class GetAllRequest : IRequest<MessageResponse<GetAllResponse[]>>
    {
        public Guid UserId { get; set; }
    }

    public sealed class GetAllResponse
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Title")]
        public string Title { get; set; }

        [JsonPropertyName("Author")]
        public string Author { get; set; }

        [JsonPropertyName("IsAvailable")]
        public bool IsAvailable { get; set; }
    }
}
