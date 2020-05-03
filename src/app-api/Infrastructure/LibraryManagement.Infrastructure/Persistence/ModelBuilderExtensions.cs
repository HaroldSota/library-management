using System;
using System.Security.Cryptography;
using System.Text;
using LibraryManagement.Infrastructure.Persistence.Model;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Persistence
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<BookData>().HasData(
                new BookData { Id = Guid.NewGuid(), Title = "To the Lighthouse", Author = "Virginia Woolf", TotalCopies = 5, AvailableCopies = 5 },
                new BookData { Id = Guid.NewGuid(), Title = "The Stranger", Author = "Albert Camus", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Trial", Author = "Franz Kafka", TotalCopies = 5, AvailableCopies = 5 },
                new BookData { Id = Guid.NewGuid(), Title = "The Red and the Black", Author = "Stendhal", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "David Copperfield", Author = "Charles Dickens", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Candide", Author = "Voltaire", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Les Misérables", Author = "Victor Hugo", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "One Hundred Years of Solitude", Author = "Gabriel Garcia Marquez", TotalCopies = 5, AvailableCopies = 5 },
                new BookData { Id = Guid.NewGuid(), Title = "War and Peace", Author = "Leo Tolstoy", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Moby Dick", Author = "Herman Melville", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Lolita", Author = "Vladimir Nabokov", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Hamlet", Author = "William Shakespeare", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Odyssey", Author = "Homer", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Crime and Punishment", Author = "Fyodor Dostoyevsky", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Madame Bovary", Author = "Gustave Flaubert", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Divine Comedy", Author = "Dante Alighieri", TotalCopies = 5, AvailableCopies = 0 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Pride and Prejudice", Author = "Jane Austen", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Old Man and the Sea", Author = "Ernest Hemingway", TotalCopies = 5, AvailableCopies = 1 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Idiot", Author = "Fyodor Dostoyevsky", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Metamorphosis", Author = "Franz Kafka", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Faust", Author = "Johann Wolfgang von Goethe", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Father Goriot", Author = "Honoré de Balzac", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Oresteia", Author = "Aeschylus", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "Decameron", Author = "Giovanni Boccaccio", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "All Quiet on the Western Front", Author = "Erich Maria Remarque", TotalCopies = 5, AvailableCopies = 5 }, 
                new BookData { Id = Guid.NewGuid(), Title = "The Call of the Wild", Author = "Jack London", TotalCopies = 5, AvailableCopies = 5 });

            modelBuilder.Entity<UserData>().HasData(
                CreateUser("admin@gmail.com", "1234567890",  "Admin",  "Admin"),
                CreateUser("henry.schmidt@gmail.com", "super-simple-password",  "Henry",  "Schmidt"),
                CreateUser("walter.braun@gmail.com", "you-will-never-find-me",  "Walter",  "Braun"),
                CreateUser("immanuel.fuchs@gmail.com", "my-super-secret-password",  "Immanuel",  "Fuchs")
            );
        }

        private static UserData CreateUser(string userName, string password, string firstName, string lastName)
        {
            CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
            return new UserData { Id = Guid.NewGuid(), UserName = userName, FirstName = firstName, LastName = lastName, PasswordHash = passwordHash, PasswordSalt = passwordSalt };
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
