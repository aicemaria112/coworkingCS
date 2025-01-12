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
                new Room { Name = "Sala A", Capacity = 10, Location = "Piso 1", IsAvailable = true },
                new Room { Name = "Sala B", Capacity = 20, Location = "Piso 2", IsAvailable = false },
                new Room { Name = "Sala C", Capacity = 15, Location = "Piso 1", IsAvailable = true }
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
    }
}