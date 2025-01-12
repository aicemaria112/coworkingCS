
    public interface IReservationHistoryService
    {
        Task AddHistoryAsync(int ReservationId, string changeType, string? description, int? userId);
    }
