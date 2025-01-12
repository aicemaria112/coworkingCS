using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MediatR;
using Moq;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Net;


public class RoleControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly Mock<IMediator> _mediatorMock;

    public RoleControllerTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
    }

    // Test for PUT api/role (UserRole)
    [Fact]
    public async Task UserRole_ReturnsUnauthorized_WhenUserHasRoleUser()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var token = TokenHelper.GenerateTokenValid();  // Simulate a valid JWT token with user role
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new UserRoleCommand
        {
            Username="testuser",
            role="user"
        };

        // Simulate the mediator sending the command
        _mediatorMock.Setup(m => m.Send(It.IsAny<UserRoleCommand>(), default))
                     .ReturnsAsync("Role updated successfully.");

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync("api/role", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // Test for PUT api/role (UserRole) - When User Role is not found in Token
    [Fact]
    public async Task UserRole_ReturnsUnauthorized_WhenRoleNotFoundInToken()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var token = "valid-jwt-token-without-role";  // Simulate a valid JWT token without role
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new UserRoleCommand
        {
            Username="testuser",
            role="user"
        };

        // Act
        var response = await client.PutAsync("api/role", new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json"));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    // Test for PUT api/role (UserRole) - When Role is updated successfully
    [Fact]
    public async Task UserRole_ReturnsOk_WhenRoleUpdatedSuccessfully()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var token = TokenHelper.GenerateTokenValid(true);  // Simulate a valid JWT token with admin role
        Console.WriteLine($"token:{ token}");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new UserRoleCommand
        {
            Username="testuser",
            role="user"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UserRoleCommand>(), default))
                     .ReturnsAsync("Role updated successfully.");

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync("api/role", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        Assert.Equal("Role updated successfully.", result["Message"]);
    }

    // Test for PUT api/role (UserRole) - When NotFoundException is thrown
    [Fact]
    public async Task UserRole_ReturnsUnauthorized_WhenRoleNotFound()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();

        var token = TokenHelper.GenerateTokenValid(false);  // Simulate a valid JWT token with admin role
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var command = new UserRoleCommand
        {
            Username="testuser",
            role="user"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<UserRoleCommand>(), default))
                     .ThrowsAsync(new UnauthorizedAccessException("You have not permission to do that."));

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");

        // Act
        var response = await client.PutAsync("api/role", content);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        Assert.Equal("You have not permission to do that.", result["Message"]);
    }
    
}
