
namespace MyORM.Interfaces
{
    /// <summary>
    /// Represents a database context
    /// </summary>
    public interface IDBContext
    {
        public IEntityCollection<T>? Collection<T>() where T : class;
        public IEntityCollection? Collection(Type collectionType);
        public IEnumerable<Type> MappedTypes { get; } 
        void UpdateDataBase();
        void CreateDataBase();
        void DropDataBase();
        bool TestConnection();

    }
}