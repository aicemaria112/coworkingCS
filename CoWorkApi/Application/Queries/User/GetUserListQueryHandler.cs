using MediatR;
using CoWorkApi.Domain.Entities;
using CoWorkApi.Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

public class GetUserListQueryHandler : IRequestHandler<GetUserListQuery, List<UserInfoDto>>
    {
        private readonly AppDbContext _context;

        public GetUserListQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserInfoDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
        {
            var users = await _context.Users
                .Select(r => new UserInfoDto
            {
                Username = r.Username,
                Name = r.Name,
                Role = r.Role,
                Email = r.Email,
                Id=r.Id

            })
                .ToListAsync(cancellationToken);

            if (users == null)
                return null; // Or throw an exception if user not found.
            
            return users;
        }
    }