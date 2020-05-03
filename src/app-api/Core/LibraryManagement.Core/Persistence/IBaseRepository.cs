using System;
using System.Linq;
using System.Threading.Tasks;
using LibraryManagement.Core.Domain;

namespace LibraryManagement.Core.Persistence
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TData">Data type</typeparam>
    public interface IBaseRepository<in TEntity, TData>
        where TEntity : BaseEntity
        where TData : IData, new()
    {

        IQueryable<TData> TableNoTracking { get; }

        /// <summary>
        /// Add the entity
        /// </summary>
        /// <param name="entity"></param>
        Task Add(TEntity entity);

        /// <summary>
        ///     Update the entity
        /// </summary>
        /// <param name="entity"></param>
        Task Update(TEntity entity);

        /// <summary>
        ///     Delete the entity by id
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns></returns>
        Task Delete(Guid id);

        /// <summary>
        ///  Get entity by id
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns></returns>
        Task<TData> GetById(Guid id);

        /// <summary>
        ///     Check if the entity with id exist
        /// </summary>
        /// <param name="id">Entity identifier</param>
        /// <returns></returns>
        Task<bool> IdExists(Guid id);
    }
}
