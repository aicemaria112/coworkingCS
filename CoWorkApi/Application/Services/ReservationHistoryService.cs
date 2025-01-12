
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;


    public class ReservationHistoryService : IReservationHistoryService
    {
        private readonly AppDbContext _context;

        public ReservationHistoryService(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddHistoryAsync(int reservationId, string changeType, string? description, int? userId)
        {
            var history = new ReservationHistory
            {
                ReservationId = reservationId,
                ChangeType = changeType,
                ChangeDescription = description,
                ChangedBy = userId,
                ChangedAt = DateTime.UtcNow
            };

            await _context.ReservationHistories.AddAsync(history);
            await _context.SaveChangesAsync();
        }
    }
