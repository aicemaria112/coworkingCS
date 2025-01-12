using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class CancelReservationControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly ReservationsController _controller;

    private readonly Mock<ILogService> _logServiceMock;
    public CancelReservationControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new ReservationsController(_mediatorMock.Object, _logServiceMock.Object);
    }

    [Fact]
    public async Task CancelReservation_ShouldReturnNoContent_WhenCancellationIsSuccessful()
    {
        // Arrange
        var userId = "1";
        var reservationId = 42;

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CancelReservationCommand>(), It.IsAny<CancellationToken>()))
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
        var result = await _controller.CancelReservation(reservationId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

}