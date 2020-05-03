using System;
using System.Text.Json.Serialization;
using MediatR;

namespace LibraryManagement.Application.Model.UserModel
{
    public sealed class AuthenticateRequest : IRequest<MessageResponse<AuthenticateResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public sealed class AuthenticateResponse
    {
        [JsonPropertyName("Id")]
        public Guid Id { get; set; }

        [JsonPropertyName("Username")]
        public string Username { get; set; }

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }
        [JsonPropertyName("Token")]
        public string Token { get; set; }
    }
}
