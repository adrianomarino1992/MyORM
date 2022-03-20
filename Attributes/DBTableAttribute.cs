
namespace MyORM.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DBTableAttribute : Attribute
    {
        public string Name { get; set; }

        public DBTableAttribute(string name)
        {
            Name = name;
        }
    }
}
