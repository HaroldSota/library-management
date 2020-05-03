using System;
using System.Text.Json.Serialization;

namespace LibraryManagement.Application
{
    public sealed class MessageResponseError
    {
        public MessageResponseError()
        {
            //Only for testing purpose
        }


        public MessageResponseError(string errorType, string message)
        {
            ErrorType = errorType;
            Message = message;
        }

        public MessageResponseError(Exception exception)
        {
            ErrorType = exception.GetType().Name;
            Message = exception.Message;
            Obj = exception;
        }

        [JsonPropertyName("ErrorType")]
        public string ErrorType { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonPropertyName("Obj")]
        public Exception Obj { get; set; }
    }
}
