public class ForbidenAccessException : AppException
{
    public ForbidenAccessException(string message = "Unauthorized access.") : base(message) { }
}