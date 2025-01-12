using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
{
    private readonly AppDbContext _context;

    public RegisterUserCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (_context.Users.Any(u => u.Username == request.Username || u.Email == request.Email))
        {
            throw new InvalidOperationException("El usuario o correo ya existe.");
        }

        // Crear nuevo usuario
        var user = new User
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password), 
            Email = request.Email,
            Role = "user",
            Name = request.Name
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(cancellationToken);

        return  "Usuario registrado exitosamente.";
    }

    
}