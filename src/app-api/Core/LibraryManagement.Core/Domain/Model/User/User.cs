using System;

namespace LibraryManagement.Core.Domain.Model
{
    public sealed class User : BaseEntity
    {
        public User(string userName, string firstName, string lastName, byte[] passwordHash, byte[] passwordSalt)
            : this(Guid.NewGuid(), userName,  firstName, lastName, passwordHash, passwordSalt)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public User(Guid id, string userName, string firstName, string lastName, byte[] passwordHash, byte[] passwordSalt) 
            : base(id)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public string UserName { get;}
        public string FirstName { get; }
        public string LastName { get; }
        public byte[] PasswordHash { get; }
        public byte[] PasswordSalt { get; }

    }
}
