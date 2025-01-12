using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class GetReservationsControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReservationsController _controller;
    private readonly Mock<ILogService> _logServiceMock;


    public GetReservationsControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new ReservationsController(_mediatorMock.Object, _logServiceMock.Object);
    }



    [Fact]
    public async Task GetUserReservations_ShouldReturnReservations_WhenUserIsAuthenticated()
    {
        // Arrange
        var userId = 1;
        var reservations = new List<ReservationDto>
    {
        new ReservationDto { Id = 1,
        Room = new RoomDto { Id = 1, Name = "Sala A", Capacity = 10, Location = "Piso 1", Description = "", IsAvailable = true }
            , EndTime = DateTime.Now, StartTime = DateTime.Now, UserId = userId, Status="ACTIVE" }
    };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetUserReservationsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(reservations);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.GetUserReservations();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<ReservationDto>>(okResult.Value);
        Assert.Equal(reservations.Count, response.Count);
    }

}