using MyORM.Enums;


namespace MyORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DBDeleteModeAttribute : Attribute
    {
        public DeleteMode DeleteMode { get; set; }

        public DBDeleteModeAttribute(DeleteMode delMode)
        {
            DeleteMode = delMode;
        }
    }
}
