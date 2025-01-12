using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;


    public class LogService : ILogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddLogAsync(string httpMethod, string endPoint, int statusCode, int? userId)
        {
            var log = new LogInfo
            {
                HttpMethod = httpMethod,
                EndPoint = endPoint,
                StatusCode = statusCode,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.LogInfos.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
