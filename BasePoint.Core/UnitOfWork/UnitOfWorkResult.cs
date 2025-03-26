namespace BasePoint.Core.UnitOfWork
{
    public class UnitOfWorkResult
    {
        public bool Success { get; protected set; }
        public string Message { get; protected set; }

        public UnitOfWorkResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}