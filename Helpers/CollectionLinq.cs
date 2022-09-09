
using System.Linq.Expressions;
using MyORM.Interfaces;

namespace MyORM.Linq
{
    public static class CollectionLinq
    {
        public static IQueryableCollection<TSource> Where<TSource, TResult>(this IQueryableCollection<TSource> source, Expression<Func<TSource,TResult>> expression) where TSource : class
        {
            return source.Query(expression);
        }

        public static List<TSource> ToList<TSource, TResult>(this IQueryableCollection<TSource> source, Expression<Func<TSource, TResult>> expression) where TSource : class
        {
            return source.Query(expression).Run().ToList();
        }

        public static List<TSource> ToList<TSource>(this IQueryableCollection<TSource> source) where TSource : class
        {
            return source.Run().ToList();
        }

        public static async Task<List<TSource>> ToListAsync<TSource, TResult>(this IQueryableCollection<TSource> source, Expression<Func<TSource, TResult>> expression) where TSource : class
        {
            return (await source.Query(expression).RunAsync()).ToList();
        }

        public static async Task<List<TSource>> ToListAsync<TSource>(this IQueryableCollection<TSource> source) where TSource : class
        {
            return (await source.RunAsync()).ToList();
        }

        public static IEnumerable<TSource> Take<TSource>(this IQueryableCollection<TSource> source, int limit) where TSource : class
        {
            return  source.Limit(limit).Run();
        }

        public static async Task<IEnumerable<TSource>> TakeAsync<TSource>(this IQueryableCollection<TSource> source, int limit) where TSource : class
        {
            return await source.Limit(limit).RunAsync();
        }

        public static TSource First<TSource>(this IQueryableCollection<TSource> source) where TSource : class
        {
            #pragma warning disable
            return source.Limit(1).Run().FirstOrDefault();
            #pragma warning restore
        }

        public static async Task<TSource> FirstAsync<TSource>(this IQueryableCollection<TSource> source) where TSource : class
        {
            #pragma warning disable
            return (await source.Limit(1).RunAsync()).FirstOrDefault();
            #pragma warning restore
        }
        

    }
}
