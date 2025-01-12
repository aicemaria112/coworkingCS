using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq.Language.Flow;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

public class ReservationsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;

    public ReservationsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
    }

    // Test for GET api/reservations (GetUserReservations)
    [Fact]
    public async Task GetUserReservations_ReturnsOk_WhenUserIsAuthorized()
    {
        // Arrange
        var token = TokenHelper.GenerateTokenValid(true);
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);

            });
        }).CreateClient();

        // var token = "valid-jwt-token";  // Simulate a valid JWT token
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserReservationsQuery>(), default))
                    .ReturnsAsync(new List<ReservationDto> { new ReservationDto {Id=1,UserId=1
                    ,Room= new RoomDto {
                        Id=1,Name="Room1",Description="Room1",Capacity=1,IsAvailable=true
                    }
                    ,StartTime=DateTime.Now,EndTime=DateTime.Now,Status="active" } });

        // Act
        var response = await client.GetAsync("api/reservations");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Test for GET api/reservations/all (GetAllReservations)
    [Fact]
    public async Task GetAllReservations_ReturnsForbidden_WhenUserIsNotAdmin()
    {
        // Arrange
        var token = TokenHelper.GenerateTokenValid();
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);

            });
        }).CreateClient();

        // Simulate a valid JWT token
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Simulate that the user is not an admin
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllReservationsQuery>(), default))
                     .ReturnsAsync(new List<ReservationAllDto>());

        // Act
        var response = await client.GetAsync("api/reservations/all");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // Test for POST api/reservations (CreateReservation)
    [Fact]
    public async Task CreateReservation_ReturnsCreated_WhenCommandIsValid()
    {
        // Arrange
        var token = TokenHelper.GenerateTokenValid(true);
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();  // Simulate a valid JWT token
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new CreateReservationCommand
        {
            RoomId = 1,
            StartTime = DateTime.Now.AddHours(1),
            EndTime = DateTime.Now.AddHours(2),
            Quantity=1,
            UserId=1
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateReservationCommand>(), default))
                     .ReturnsAsync(1);  // Simulate successful reservation creation

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PostAsync("api/reservations", content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    // Test for PUT api/reservations/{id} (UpdateReservation)
    [Fact]
    public async Task UpdateReservation_ReturnsOk_WhenReservationIsUpdated()
    {
        // Arrange
        var token = TokenHelper.GenerateTokenValid(true);
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient(); // Simulate a valid JWT token
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new UpdateReservationCommand
        {
            ReservationId = 1,
            // Your updated reservation details here
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateReservationCommand>(), default))
                     .ReturnsAsync(Unit.Value);  // Simulate successful reservation update

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync("api/reservations/1", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    // Test for DELETE api/reservations/{id} (CancelReservation)
    [Fact]
    public async Task CancelReservation_ReturnsNoContent_WhenReservationIsCancelled()
    {
        // Arrange
        var token = TokenHelper.GenerateTokenValid(true);
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient(); // Simulate a valid JWT token
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new CancelReservationCommand
        {
            ReservationId = 1,
            UserId = 1
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<CancelReservationCommand>(), default))
                     .ReturnsAsync(Unit.Value);  // Simulate successful cancellation

        // Act
        var response = await client.DeleteAsync("api/reservations/1");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
