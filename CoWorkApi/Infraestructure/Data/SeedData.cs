using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        if (!context.Rooms.Any())
        {
            context.Rooms.AddRange(
                new Room { Name = "Sala A", Capacity = 10, Location = "Piso 1", IsAvailable = true, Description = "Sala A" ,ImageUrl="https://picsum.photos/id/237/200/300" },
                new Room { Name = "Sala B", Capacity = 20, Location = "Piso 2", IsAvailable = false, Description = "Sala B" ,ImageUrl="https://picsum.photos/id/237/200/300" },
                new Room { Name = "Sala C", Capacity = 15, Location = "Piso 1", IsAvailable = true, Description = "Sala C" ,ImageUrl="https://picsum.photos/id/237/200/300" }
            );

            context.SaveChanges();
        }
        if (!context.Users.Where(r => r.Username.Equals("admin")).Any())
        {

            var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");

            var adminUser = new User
            {
                Id = 0,
                Username = "admin",
                Email = "admin@example.com",
                Role = "admin",
                Password = passwordHash,
                CreatedAt = DateTime.UtcNow
            };
            context.Users.Add(adminUser);
            context.SaveChanges();
        }
        if (!context.RoomReservationsConfigs.Any())
        {
            context.RoomReservationsConfigs.AddRange(
                new RoomReservationsConfig { RoomId = 1, TimeRangeName = "dia", TimeRangeValue = 1, Name = "reserva por dia", Price = 500 },
                new RoomReservationsConfig { RoomId = 2, TimeRangeName = "dia", TimeRangeValue = 1, Name = "reserva por dia", Price = 500 },
                new RoomReservationsConfig { RoomId = 3, TimeRangeName = "dia", TimeRangeValue = 1, Name = "reserva por dia", Price = 500 }
            );
            context.SaveChanges();
        }
    }
}