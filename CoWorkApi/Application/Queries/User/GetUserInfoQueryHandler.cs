using CoWorkApi.Infraestructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetUserInfoQueryHandler : IRequestHandler<GetUserInfoQuery, UserInfoDto>
    {
        private readonly AppDbContext _context;

        public GetUserInfoQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserInfoDto> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            // Retrieve user from database
            int userId_ ;
            int.TryParse(request.UserId, out userId_);

            var user = await _context.Users
                .Where(u => u.Id == userId_)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
                return null; // Or throw an exception if user not found.

            // Map user data to UserInfoDto
            return new UserInfoDto
            {
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Id= user.Id
            };
        }
    }