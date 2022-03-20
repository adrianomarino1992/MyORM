namespace MyORM.Interfaces
{

    /// <summary>
    /// Base mappeable type
    /// </summary>
    public interface IEntityCollection { }

    /// <summary>
    /// Represents a mappeable collection
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"> that will be mapped</typeparam>
    public interface IEntityCollection<TEntity> : IQueryableCollection<TEntity>, IEntityCollection where TEntity : class
    {
        int Count();
        Task<int> CountAsync();
        TEntity Add(TEntity obj);
        Task<TEntity> AddAsync(TEntity obj);
        void Update(TEntity obj);
        Task UpdateAsync(TEntity obj);
        void Delete(TEntity obj);
        Task DeleteAsync(TEntity obj);
    }
}
