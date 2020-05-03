using LibraryManagement.Core;
using LibraryManagement.Infrastructure.Persistence;
using LibraryManagement.Infrastructure.Persistence.Model;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagement.Tests.Infrastructure
{
    public static class TestDbContext
    {

        public static IDbContext DbContext { get; private set; }

        static TestDbContext()
        {
            DbContext = EngineContext.Current.Resolve<IDbContext>();
        }

        public static void Seed()
        {
            Clear();
            DbContext.Set<BookData>()
                .AddRange(new BookData { Id = Guid.NewGuid(), Title = "The Stranger", Author = "Albert Camus", TotalCopies = 5, AvailableCopies = 1 },
                    new BookData { Id = Guid.NewGuid(), Title = "The Trial", Author = "Franz Kafka", TotalCopies = 5, AvailableCopies = 0 },
                    new BookData { Id = Guid.NewGuid(), Title = "The Red and the Black", Author = "Stendhal", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "David Copperfield", Author = "Charles Dickens", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "Candide", Author = "Voltaire", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "Les Misérables", Author = "Victor Hugo", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "One Hundred Years of Solitude", Author = "Gabriel Garcia Marquez", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "War and Peace", Author = "Leo Tolstoy", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "Moby Dick", Author = "Herman Melville", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "Lolita", Author = "Vladimir Nabokov", TotalCopies = 5, AvailableCopies = 5 },
                    new BookData { Id = Guid.NewGuid(), Title = "Hamlet", Author = "William Shakespeare", TotalCopies = 5, AvailableCopies = 5 });

            DbContext.Set<UserData>()
                .AddRange(
                    CreateUser("admin@gmail.com", "1234567890", "Admin", "Admin"),
                    CreateUser("henry.schmidt@gmail.com", "super-simple-password", "Henry", "Schmidt"),
                    CreateUser("walter.braun@gmail.com", "you-will-never-find-me", "Walter", "Braun"),
                    CreateUser("immanuel.fuchs@gmail.com", "my-super-secret-password", "Immanuel", "Fuchs"));


            DbContext.SaveChangesAsync();

        }

        public static void Clear()
        {
            TestDbContext.DbContext.Set<BookData>().RemoveRange(
                TestDbContext.DbContext
                    .Set<BookData>()
                    .Where(w => !w.Id.Equals(Guid.Empty)));

            TestDbContext.DbContext.Set<UserData>().RemoveRange(
                TestDbContext.DbContext
                    .Set<UserData>()
                    .Where(w => !w.Id.Equals(Guid.Empty)));

            TestDbContext.DbContext.Set<UserBookData>().RemoveRange(
                TestDbContext.DbContext
                    .Set<UserBookData>()
                    .Where(w => !w.Id.Equals(Guid.Empty)));

            DbContext.SaveChangesAsync();

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
