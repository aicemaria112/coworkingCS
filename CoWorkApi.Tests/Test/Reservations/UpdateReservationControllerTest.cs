using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UpdateReservationControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReservationsController _controller;
    private readonly Mock<ILogService> _logServiceMock;
    public UpdateReservationControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new ReservationsController(_mediatorMock.Object, _logServiceMock.Object);
    }

    [Fact]
    public async Task UpdateReservation_ShouldReturnOk_WhenUpdateIsSuccessful()
{
    // Arrange
    var userId = "1";
    var reservationId = 42;
    var command = new UpdateReservationCommand { RoomId = 2, StartTime = DateTime.Now, EndTime = DateTime.Now , Quantity = 1};

    _mediatorMock
        .Setup(m => m.Send(It.IsAny<UpdateReservationCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(Unit.Value);

    var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    }, "TestAuth"));

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.UpdateReservation(reservationId, command);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<Dictionary<string, string>>(okResult.Value);
    Assert.Equal("Success", response["Message"]);
}
}