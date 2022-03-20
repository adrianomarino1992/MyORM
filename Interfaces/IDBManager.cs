using System.Reflection;

namespace MyORM.Interfaces
{
    /// <summary>
    /// Represents the database
    /// </summary>
    public interface IDBManager
    {
        bool TryConnection();
        void CreateColumn(string table, PropertyInfo info);
        void DropColumn(string table, PropertyInfo info);
        void FitColumns(string table, IEnumerable<PropertyInfo> info);
        void CreateTable<T>();
        void DropTable<T>();
        void CreateDataBase();
        void DropDataBase();
        bool DataBaseExists();
        bool ColumnExists(string table, string colName);
        bool TableExists<T>();

    
    }
}
