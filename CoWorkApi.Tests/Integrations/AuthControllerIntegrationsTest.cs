using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

[Collection("Database collection")]
public class AuthControllerIntegrationsTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
     private readonly Mock<IMediator> _mediatorMock;
     private readonly RegisterUserCommand _command;
     private readonly HttpClient _client;

     private string _token = string.Empty;
    public AuthControllerIntegrationsTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _mediatorMock = new Mock<IMediator>();
         Random random = new Random();
        int randomInt = random.Next(1000, 5000);

        var command = new RegisterUserCommand
        {
            Email = $"test0test.com",
            Password = "Admin123!",
            Name = "Test User",
            Username = "admin"
        };
        _command = command;

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mediatorMock.Object);
            });
        }).CreateClient();
        _client = client;
    }

    [Fact]
    public async Task Register_ReturnsOk_WhenCommandIsValid()
    {
        // Arrange
        

        _mediatorMock.Setup(m => m.Send(It.IsAny<RegisterUserCommand>(), default))
                     .ReturnsAsync("Usuario registrado exitosamente.");

        var content = new StringContent(JsonSerializer.Serialize(_command), Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("api/auth/register", content);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Responsereg: {responseContent}");
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        Assert.NotNull(result);
        Assert.Equal("Usuario registrado exitosamente.", result["Message"]);
    }

    [Fact]
    public async Task Login_ReturnsToken_WhenCredentialsAreValid()
    {
        // Arrange
        

        var command = new LoginUserCommand { Username = _command.Username, Password = _command.Password };
        _mediatorMock.Setup(m => m.Send(It.IsAny<LoginUserCommand>(), default))
                     .ReturnsAsync("test-jwt-token");

        var content = new StringContent(JsonSerializer.Serialize(command), Encoding.UTF8, "application/json");
        // Act
        var response = await _client.PostAsync("api/auth/login", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Responselogin: {responseContent}");
        var result = JsonSerializer.Deserialize<Dictionary<string, string>>(responseContent);
        Assert.NotNull(result);
        Assert.Equal("test-jwt-token", result["Token"]);
    }

    [Fact]
    public async Task GetUserInfo_ReturnsUserInfo_WhenUserIsNotAuthenticated()
    {

         
         var token_result = "f";
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token_result);
        Console.WriteLine($"Resultado obtenido: {token_result}");
        // Act
        var response = await _client.GetAsync("api/auth/user-info");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var responseContent = await response.Content.ReadAsStringAsync();
    }
}