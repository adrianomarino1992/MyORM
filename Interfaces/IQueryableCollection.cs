using System.Linq.Expressions;


namespace MyORM.Interfaces
{
    /// <summary>
    /// A interface that have all methods that a class must implement to be a IDBContext valid item collection
    /// </summary>
    /// <typeparam name="TEntity">The <see cref="Type"> that will be mapped</typeparam>
    public interface IQueryableCollection<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Run();
        Task<IEnumerable<TEntity>> RunAsync();
        IEnumerable<TEntity> Run(string sql);
        Task<IEnumerable<TEntity>> RunAsync(string sql);
        ICommand GetCommand();
        IQueryableCollection<TEntity> Query<TResult>(Expression<Func<TEntity, TResult>> expression);
        IQueryableCollection<TEntity> Join<TResult>(Expression<Func<TEntity, TResult>> expression);
        IQueryableCollection<TEntity> And<TResult>(Expression<Func<TEntity, TResult>> expression);
        IQueryableCollection<TEntity> Or<TResult>(Expression<Func<TEntity, TResult>> expression);
        IQueryableCollection<TEntity> OrderBy<TResult>(Expression<Func<TEntity, TResult>> expression);        
        IQueryableCollection<TEntity> Limit(int limit);
        IQueryableCollection<TEntity> OffSet(int offSet);

    }
}
