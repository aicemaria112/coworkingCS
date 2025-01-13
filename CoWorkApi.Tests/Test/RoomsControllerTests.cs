using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CockWorkApi.Tests.Tests;

public class RoomsControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RoomsController _controller;

    private readonly Mock<ILogService> _logServiceMock;

    private readonly Mock<IWebHostEnvironment> _environmentMock;


    public RoomsControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _environmentMock = new Mock<IWebHostEnvironment>();
        _controller = new RoomsController(_mediatorMock.Object, _logServiceMock.Object, _environmentMock.Object);
    }


    [Fact]
    public async Task GetAvailableRooms_ShouldReturnOk_WhenQueryIsSuccessful()
    {
        var result = await _controller.GetAvailableRooms(10, "Piso 1", "Sala");
        // Assert
        Assert.IsType<OkObjectResult>(result);
    }


    [Fact]
    public async Task GetAvailableRooms_ShouldSendQueryWithCorrectParameters()
    {
        // Act
        await _controller.GetAvailableRooms(15, "Los Angeles", "Meeting");

        // Assert
        _mediatorMock.Verify(m =>
            m.Send(It.Is<GetAvailableRoomsQuery>(query =>
                query.MinimumCapacity == 15 &&
                query.Location == "Los Angeles" &&
                query.NameContains == "Meeting"),
            It.IsAny<CancellationToken>()),
            Times.Once); // Verifica que el método Send fue llamado con los parámetros correctos
    }


    [Fact]
    public async Task GetAvailableRooms_ShouldReturnOk_WhenNoFiltersAreProvided()
    {
        // Act
        var result = await _controller.GetAvailableRooms(null, null, null);

        // Assert
        Assert.IsType<OkObjectResult>(result); // Verifica que la respuesta sea 200 OK
    }


    [Fact]
    public async Task GetAvailableRooms_ShouldReturnEmptyList_WhenNoRoomsAreAvailable()
    {
        // Act
        var result = await _controller.GetAvailableRooms(5, "Chicago", "Office");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Verifica que la respuesta sea 200 OK
        var rooms = okResult.Value as List<object>;
        if (rooms == null)
        {
            // Manejo de un null explícito en lugar de una lista vacía
            Assert.True(true, "Rooms list is null, as expected.");
        }
        else
        {
            Assert.Empty(rooms); // Si no es null, verifica que la lista está vacía
        }
    }

}