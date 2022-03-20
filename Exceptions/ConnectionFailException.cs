namespace MyORM.Exceptions
{
    public class ConnectionFailException : Exception
    {
        public ConnectionFailException(string msg) : base(msg)
        {

        }
    }
}
