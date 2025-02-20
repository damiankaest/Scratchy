namespace Scratchy.Domain.Exceptions
{
    public abstract class BaseException : Exception
    {
        public int ErrorCode { get; }
        public string ErrorSource { get; }

        protected BaseException(int errorCode, string errorSource, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            ErrorSource = errorSource;
        }
    }

}
