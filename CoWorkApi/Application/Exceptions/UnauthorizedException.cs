public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message = "Unauthorized access.") : base(message) { }
}