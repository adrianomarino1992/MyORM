using MyORM.Enums;

namespace MyORM.Interfaces
{
    /// <summary>
    /// Exposes the SQL of a query object
    /// </summary>
    public interface ICommand
    {
        ProviderType ProviderType { get; set; }

        string Sql();
    }
}
