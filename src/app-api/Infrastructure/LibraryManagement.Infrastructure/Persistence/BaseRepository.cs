using LibraryManagement.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryManagement.Core;
using LibraryManagement.Core.Domain;

namespace LibraryManagement.Infrastructure.Persistence
{
    /// <inheritdoc/>
    public class BaseRepository<TEntity, TData> : IBaseRepository<TEntity, TData>
        where TEntity : BaseEntity
        where TData : class, IData, new()
    {
        private readonly IDbContext _dbContext;

        private DbSet<TData> _entities;


        public BaseRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected virtual DbSet<TData> Entities => _entities ??= _dbContext.Set<TData>();

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<TData> TableNoTracking => Entities.AsNoTracking();

        /// <inheritdoc/>
        public async Task Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                var data = Map(entity);
                await Entities.AddAsync(data);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetErrorAndRollback(exception), exception);
            }
        }

        /// <inheritdoc/>
        public async Task Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ValidationException(nameof(entity));
            }

            try
            {
                var data = Map(entity);

                Entities.Update(data);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                var message = $"Entity with id {entity.Id} is already modified or deleted!";
                throw new Exception(message,
                    new Exception(GetErrorAndRollback(exception), exception));
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetErrorAndRollback(exception), exception);
            }
        }

        /// <inheritdoc/>
        public async Task Delete(Guid id)
        {
            var data = new TData { Id = id };
            try
            {
                Entities.Attach(data);
                Entities.Remove(data);

               await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                throw new Exception(GetErrorAndRollback(exception), exception);
            }
        }

        /// <inheritdoc/>
        public async Task<TData> GetById(Guid id)
        {
            return await TableNoTracking.Where(t => t.Id == id).FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<bool> IdExists(Guid id) => await GetById(id) != null;

        protected TData Map(TEntity entity)
        {
            return Singleton<IMapper>.Instance.Map<TData>(entity);
        }

        protected string GetErrorAndRollback(DbUpdateException exception)
        {
            if (_dbContext is DbContext dbContext)
            {
                try
                {
                    dbContext.ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                        .ToList()
                        .ForEach(entry => entry.State = EntityState.Unchanged);
                }
                catch (Exception ex)
                {
                    exception = new DbUpdateException(exception.ToString(), ex);
                }
            }

            _dbContext.SaveChangesAsync();
            return exception.ToString();
        }
    }
}
