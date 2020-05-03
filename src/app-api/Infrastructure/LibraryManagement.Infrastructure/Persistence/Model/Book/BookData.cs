using System;
using System.Collections.Generic;
using LibraryManagement.Core.Persistence;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagement.Infrastructure.Persistence.Model
{
    [Table("Book")]
    public class BookData: IData
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Author { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

        public virtual ICollection<UserBookData> UserBooks { get; set; }
    }
}
