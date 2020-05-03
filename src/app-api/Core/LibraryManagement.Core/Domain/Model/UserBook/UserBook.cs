using System;

namespace LibraryManagement.Core.Domain.Model
{
    public sealed class UserBook : BaseEntity
    {

        public UserBook(Guid id, Guid userId, Guid bookId) : base(id)
        {
            UserId = userId;
            BookId = bookId;
        }

        public UserBook(Guid userId, Guid bookId) : this(new Guid(), userId, bookId)
        {
        }

        public Guid UserId { get; }

        public Guid BookId { get; }

    }
}
