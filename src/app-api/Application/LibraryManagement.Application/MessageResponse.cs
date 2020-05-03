using System;

namespace LibraryManagement.Application
{
    public class MessageResponse<TMessageResponse>
    {
        #region [ Ctor ]

        public MessageResponse(TMessageResponse result)
        {
            MessageType = MessageType.OK;
            Result = result;
        }

        public MessageResponse(MessageType messageType, MessageResponseError error)
        {
            MessageType = messageType;
            Error = error;
        }

        public MessageResponse(MessageType messageType, Exception exception)
        {
            MessageType = messageType;
            Error = new MessageResponseError(exception);
        }

        #endregion

        #region [ Properties ]
        public MessageType MessageType { get; }

        public TMessageResponse Result { get; }

        public MessageResponseError Error { get; }

        #endregion
    }
}
