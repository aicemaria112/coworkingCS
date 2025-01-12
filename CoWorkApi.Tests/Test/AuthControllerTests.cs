using System.Collections.Generic;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class AuthControllerTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly AuthController _controller;

    private RegisterUserCommand _command;
    private readonly Mock<ILogService> _logServiceMock;
    public AuthControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new AuthController(_mediatorMock.Object, _logServiceMock.Object);
        Random random = new Random();
        int randomInt = random.Next(1000, 5000);

        var command = new RegisterUserCommand
        {
            Email = $"test{randomInt}@test.com",
            Password = "123456",
            Name = "Test User",
            Username = $"testuser{randomInt}"
        };
        _command = command;
    }

    [Fact]
    public async Task Register_ShouldReturnOk_WhenCommandIsSuccessful()
    {
        // Arrange

        _mediatorMock
            .Setup(m => m.Send(_command, It.IsAny<CancellationToken>()))
            .ReturnsAsync("Usuario registrado exitosamente.");

        // Act
        var result = await _controller.Register(_command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Dictionary<string, string>>(okResult.Value);
        Assert.Equal("Usuario registrado exitosamente.", response["Message"]);
    }

    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenInvalidOperationExceptionIsThrown()
    {
        // Arrange
        var command = new RegisterUserCommand
        {
            Username = "testuseqr2"
        };
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("One or more validation errors occurred."));

        // Act
        var result = await _controller.Register(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<Dictionary<string, string>>(badRequestResult.Value);
        Assert.Equal("One or more validation errors occurred.", response["Message"]);
    }



    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenInvalidOperationException_WhenUsernameIsAlreadyTaken()
    {
        // Arrange
        var command = _command;
        _mediatorMock
            .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("El usuario o correo ya existe."));

        // Act
        var result = await _controller.Register(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<Dictionary<string, string>>(badRequestResult.Value);
        Assert.Equal("El usuario o correo ya existe.", response["Message"]);
    }



    //login step

    [Fact]
        public async Task Login_ShouldReturnToken_WhenCommandIsSuccessful()
        {
            // Arrange
            var command = new LoginUserCommand {
                Username = _command.Username,
                Password = _command.Password
             };
            var fakeToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTczNjYyOTY4MiwiZXhwIjoxNzM2NjMzMjgyLCJpYXQiOjE3MzY2Mjk2ODIsImlzcyI6IkNvY2tXb3JrQXBpIiwiYXVkIjoiQ29ja1dvcmtBcGlVc2VycyJ9.ASeEGVKgolzQlmZElNz8fM46NADNYdh87sM1E_ZdcKY";
            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fakeToken);

            // Act
            var result = await _controller.Login(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal(fakeToken, response["Token"]);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenUnauthorizedAccessExceptionIsThrown()
        {
            // Arrange
            var command = new LoginUserCommand { Username="falseuser", Password="123456" };
            _mediatorMock
                .Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new UnauthorizedAccessException());

            // Act
            var result = await _controller.Login(command);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, string>>(unauthorizedResult.Value);
            Assert.Equal("Credenciales inválidas.", response["Message"]);
        }

        [Fact]
        public async Task GetUserInfo_ShouldReturnOk_WhenUserIsAuthenticated()
        {
            // Arrange
            int userId = 1;
            var userInfo = new UserInfoDto { Id = userId, Name = "Test User", Email = "test@example.com", Username = "testuser", Role = "user" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetUserInfoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(userInfo);

            // Simulate authentication by setting a user claim
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
            }, "TestAuth"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.GetUserInfo();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userInfo, okResult.Value);
        }

        [Fact]
        public async Task GetUserInfo_ShouldReturnNotFound_WhenUserInfoIsNull()
        {
            // Arrange
            var userId = "12345";

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuth"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            // Act
            var result = await _controller.GetUserInfo();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<Dictionary<string, string>>(notFoundResult.Value);
            Assert.Equal("User not found.", response["Message"]);
        }

}
