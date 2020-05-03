using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Core.Persistence;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    [Table("User")]
    public class UserData : IData
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public virtual ICollection<UserBookData> UserBooks { get; set; }
    }
}
