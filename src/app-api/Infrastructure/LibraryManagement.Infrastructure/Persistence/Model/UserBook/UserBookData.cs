using System;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagement.Core.Persistence;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    [Table("UserBook")]
    public class UserBookData: IData
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual UserData User { get; set; }


        public Guid BookId { get; set; }
        public virtual BookData Book { get; set; }
    }
}
