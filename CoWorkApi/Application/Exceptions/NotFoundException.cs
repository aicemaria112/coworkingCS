public class NotFoundException : AppException
{
    public NotFoundException(string resourceName, object key)
        : base($"Resource '{resourceName}' with key '{key}' was not found.") { }

    public NotFoundException(string Message="Not Found")
        : base(Message) { }
}