using CockWorkApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CockWorkApi.Infraestructure.Data {
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationHistory> ReservationHistories { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relaciones en Reservation
            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Room)
                .WithMany(rm => rm.Reservations)
                .HasForeignKey(r => r.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relaciones en ReservationHistory
            modelBuilder.Entity<ReservationHistory>()
                .HasOne(rh => rh.Reservation)
                .WithMany()
                .HasForeignKey(rh => rh.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReservationHistory>()
                .HasOne(rh => rh.ChangedByUser)
                .WithMany()
                .HasForeignKey(rh => rh.ChangedBy)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }

}

