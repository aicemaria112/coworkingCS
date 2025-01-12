
    public interface ILogService
    {
        Task AddLogAsync(string httpMethod, string endPoint, int statusCode, int? userId);   
    }
