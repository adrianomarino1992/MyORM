namespace MyORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DBColumnAttribute : Attribute
    {
        public string Name { get; set; }

        public DBColumnAttribute(string name)
        {
            Name = name;
        }
    }
}
