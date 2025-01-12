public class ConflictException : AppException
{
    public ConflictException(string message = "Another user take the reservation.") : base(message) { }
}