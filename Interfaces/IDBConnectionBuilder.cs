using System.Data;

using MyORM.Enums;


namespace MyORM.Interfaces
{    
    /// <summary>
    /// Provides database information, connections and commands
    /// </summary>
    public interface IDBConnectionBuilder
    {
        string User { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        string Host { get; set; }
        string DataBase { get; set; }
        string Schema { get; set; }        
        ProviderType ProviderType { get; set; }
        IDbCommand NewCommand(IDbConnection coon);
        IDbConnection NewConnection();
        
    }
}
