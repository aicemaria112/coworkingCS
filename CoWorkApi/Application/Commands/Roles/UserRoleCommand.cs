using MediatR;

public class UserRoleCommand : IRequest<string>
{
    public string Username { get; set; }
    public string role { get; set; }
}