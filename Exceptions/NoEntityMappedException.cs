namespace MyORM.Exceptions
{
    public class NoEntityMappedException : Exception
    {
        public NoEntityMappedException(string msg) : base(msg)
        {

        }
    }
}
