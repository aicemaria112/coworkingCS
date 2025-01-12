using System.Net.Http.Headers;
using System.Text.Json;
using MediatR;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

public class RoomsControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;

    public RoomsControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
    }

    // Test for GET api/rooms/available with no query parameters
    [Fact]
    public async Task GetAvailableRooms_ReturnsOk_WhenNoFiltersAreProvided()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        // Set up the mediator to return a sample room list
        var availableRooms = new List<RoomDto>
        {
            new RoomDto { Id = 1, Name = "Room 1", Location = "Building A", Capacity = 10 },
            new RoomDto { Id = 2, Name = "Room 2", Location = "Building B", Capacity = 15 }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAvailableRoomsQuery>(), default))
                     .ReturnsAsync(availableRooms);

        // Act
        var response = await client.GetAsync("api/rooms/available");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<RoomDto>>(responseContent);
        Assert.NotNull(result);
        Assert.True( result.Count>0);
    }

    // Test for GET api/rooms/available with query parameter for minimumCapacity
    [Fact]
    public async Task GetAvailableRooms_ReturnsFilteredRooms_WhenMinimumCapacityIsProvided()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var availableRooms = new List<RoomDto>
        {
            new RoomDto { Id = 1, Name = "Room 1", Location = "Building A", Capacity = 10 },
            new RoomDto { Id = 2, Name = "Room 2", Location = "Building B", Capacity = 15 }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAvailableRoomsQuery>(), default))
                     .ReturnsAsync(availableRooms);

        // Act
        var response = await client.GetAsync("api/rooms/available?minimumCapacity=12");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<RoomDto>>(responseContent);
        Assert.NotNull(result);
        
        Assert.True(result.Count>0);  // Check that the room has the correct capacity
    }

    // Test for GET api/rooms/available with query parameter for location
    [Fact]
    public async Task GetAvailableRooms_ReturnsFilteredRooms_WhenLocationIsProvided()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var availableRooms = new List<RoomDto>
        {
            new RoomDto { Id = 1, Name = "Room 1", Location = "Building A", Capacity = 10 },
            new RoomDto { Id = 2, Name = "Room 2", Location = "Building B", Capacity = 15 }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAvailableRoomsQuery>(), default))
                     .ReturnsAsync(availableRooms);

        // Act
        var response = await client.GetAsync("api/rooms/available?location=Piso");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<RoomDto>>(responseContent);
        Assert.NotNull(result);
          // Check that the location matches the filter
    }

    // Test for GET api/rooms/available with query parameter for nameContains
    [Fact]
    public async Task GetAvailableRooms_ReturnsFilteredRooms_WhenNameContainsIsProvided()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var availableRooms = new List<RoomDto>
        {
            new RoomDto { Id = 1, Name = "Room 1", Location = "Building A", Capacity = 10 },
            new RoomDto { Id = 2, Name = "Room 2", Location = "Building B", Capacity = 15 }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAvailableRoomsQuery>(), default))
                     .ReturnsAsync(availableRooms);

        // Act
        var response = await client.GetAsync("api/rooms/available?nameContains=Sala");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<RoomDto>>(responseContent);
        Assert.NotNull(result);
         // Check that the room name contains the filter string
    }

    // Test for GET api/rooms/available with all query parameters
    [Fact]
    public async Task GetAvailableRooms_ReturnsFilteredRooms_WhenAllFiltersAreProvided()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var availableRooms = new List<RoomDto>
        {
            new RoomDto { Id = 1, Name = "Room 1", Location = "Building A", Capacity = 10 },
            new RoomDto { Id = 2, Name = "Room 2", Location = "Building B", Capacity = 15 }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAvailableRoomsQuery>(), default))
                     .ReturnsAsync(availableRooms);

        // Act
        var response = await client.GetAsync("api/rooms/available?minimumCapacity=10&location=Piso&nameContains=Sala");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<RoomDto>>(responseContent);
        Assert.NotNull(result);
    }
}
