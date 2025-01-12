using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class CreateReservationControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReservationsController _controller;
    private readonly Mock<ILogService> _logServiceMock;
    public CreateReservationControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new ReservationsController(_mediatorMock.Object, _logServiceMock.Object);
    }

[Fact]
public async Task CreateReservation_ShouldReturnCreated_WhenReservationIsSuccessful()
{
    // Arrange
    var userId = 1;
    var reservationId = 42;
    var command = new CreateReservationCommand {RoomId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now, Quantity = 1 };

    _mediatorMock
        .Setup(m => m.Send(It.IsAny<CreateReservationCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(reservationId);

    var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId.ToString())
    }, "TestAuth"));

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.CreateReservation(command);

    // Assert
    var createdResult = Assert.IsType<ObjectResult>(result);
    Assert.Equal(201, createdResult.StatusCode);
    var response = Assert.IsType<Dictionary<string, int>>(createdResult.Value);
    Assert.Equal(reservationId, response["reservation_id"]);
}

}

