using System.ComponentModel.DataAnnotations;
using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;


class UserRoleCommandHandler : IRequestHandler<UserRoleCommand, string>
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public UserRoleCommandHandler(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<string> Handle(UserRoleCommand request, CancellationToken cancellationToken)
    {
         var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == request.Username, cancellationToken);

        if(user == null){
            throw new NotFoundException($"User: {request.Username} Not Found");
        }
        if(user.Id==0 || user.Username =="admin" ){
            throw new ForbidenAccessException("You have no permission to edit master admin role");
        }

        var validRoles = new[] { "admin", "user" };
        if (!validRoles.Contains(request.role.ToLower()))
        {
            throw new ValidationException($"Invalid role: {request.role}. Accepted roles are 'admin' or 'user'.");
        }

        user.Role = request.role.ToLower();
        
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);

        return "User role Changed!";
    }
}