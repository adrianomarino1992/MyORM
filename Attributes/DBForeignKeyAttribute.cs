
namespace MyORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DBForeignKeyAttribute : Attribute
    {
       
    }
}
