using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class GetAllReservationsControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReservationsController _controller;
    private readonly Mock<ILogService> _logServiceMock;



    public GetAllReservationsControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new ReservationsController(_mediatorMock.Object, _logServiceMock.Object);
    }

[Fact]
public async Task GetAllReservations_ShouldReturnReservations_WhenUserIsAdmin()
{
    // Arrange
    var userId = "1";
    var role = "admin";
    var reservations = new List<ReservationAllDto>
    {
        new ReservationAllDto { Id = 1, 
        Room = new RoomDto { Id = 1, Name = "Room 1", Capacity = 10, Location = "Piso 1", Description = "", IsAvailable = true },
        User = new UserInfoDto { Id = 1, Name = "User 1", Email = "user1@test.com", Username = "user1" }
        , StartTime = DateTime.Now, EndTime = DateTime.Now, Status = "ACTIVE" }
    };

    _mediatorMock
        .Setup(m => m.Send(It.IsAny<GetAllReservationsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(reservations);

    var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, role)
    }, "TestAuth"));

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.GetAllReservations();

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<List<ReservationAllDto>>(okResult.Value);
    Assert.Equal(reservations.Count, response.Count);
}
[Fact]
public async Task GetAllReservations_ShouldReturnForbidden_WhenUserIsNotAdmin()
{
    // Arrange
    var userId = "1";
    var role = "user";
    var reservations = new List<ReservationAllDto>
    {
        new ReservationAllDto { Id = 1, 
        Room = new RoomDto { Id = 1, Name = "Room 1", Capacity = 10, Location = "Piso 1", Description = "", IsAvailable = true },
        User = new UserInfoDto { Id = 1, Name = "User 1", Email = "user1@test.com", Username = "user1" }
        , StartTime = DateTime.Now, EndTime = DateTime.Now, Status = "ACTIVE" }
    };

    _mediatorMock
        .Setup(m => m.Send(It.IsAny<GetAllReservationsQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(reservations);

    var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId),
        new Claim(ClaimTypes.Role, role)
    }, "TestAuth"));

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.GetAllReservations();

    // Assert
    var okResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(403, okResult.StatusCode);
}
}