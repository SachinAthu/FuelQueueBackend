namespace FuelQueueBackend.Models
{
    public class CustomError
    {
        public int Status { get; set; }

        public string Message { get; set; } = null!;

        public string ErrorStr { get; set; } = null!;

        public CustomError(int status, string message, string errorStr)
        {
            Status = status;
            Message = message;
            ErrorStr = errorStr;
        }

        public CustomError(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}
