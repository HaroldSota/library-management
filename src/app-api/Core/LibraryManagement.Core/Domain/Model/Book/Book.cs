using System;

namespace LibraryManagement.Core.Domain.Model
{
    public sealed class Book : BaseEntity
    {

        public Book(Guid id, string title, string author, int totalCopies, int availableCopies)
            : base(id)
        {
         
            Title = title;
            Author = author;
            TotalCopies = totalCopies;
            AvailableCopies = availableCopies;
        }
   
        public Book(string title, string author, int totalCopies, int availableCopies)
        : this(new Guid(), title, author, totalCopies, availableCopies)
        {
        }

        public string Title { get; set; }

        public string Author { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }

    }
}
