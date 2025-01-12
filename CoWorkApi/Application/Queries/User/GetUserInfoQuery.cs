using MediatR;

public class GetUserInfoQuery : IRequest<UserInfoDto>
{
    public string UserId { get; set; }
}
