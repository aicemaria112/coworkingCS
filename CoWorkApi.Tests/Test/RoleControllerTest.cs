using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class RoleControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly RoleController _controller;

    private readonly Mock<ILogService> _logServiceMock;

    private readonly string userid = "1";

    public RoleControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new RoleController(_mediatorMock.Object, _logServiceMock.Object);
    }

    [Fact]
public async Task UserRole_ShouldReturnUnauthorized_WhenRoleIsNotFoundInToken()
{
    // Arrange
    var command = new UserRoleCommand { Username = "testuser", role = "admin" }; 
    var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userid) }, "TestAuth"));
    
    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.UserRole(command);

    // Assert
    var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
    var response = Assert.IsType<Dictionary<string, string>>(unauthorizedResult.Value);
    Assert.Equal("User ID not found in token.", response["Message"]);
}

[Fact]
public async Task UserRole_ShouldReturnUnauthorized_WhenUserRoleIsUser()
{
    // Arrange
    var command = new UserRoleCommand { Username = "testuser2", role = "user" }; 
    var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { 
        new Claim(ClaimTypes.NameIdentifier, userid), 
        new Claim(ClaimTypes.Role, "user")
    }, "TestAuth"));

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.UserRole(command);

    // Assert
    var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
    var response = Assert.IsType<Dictionary<string, string>>(unauthorizedResult.Value);
    Assert.Equal("You have not permission to do that.", response["Message"]);
}

[Fact]
public async Task UserRole_ShouldReturnOk_WhenRoleIsUpdatedSuccessfully()
{
    // Arrange
    var command = new UserRoleCommand(); // Use actual command properties if necessary
    var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { 
        new Claim(ClaimTypes.NameIdentifier, "1"), 
        new Claim(ClaimTypes.Role, "admin")
    }, "TestAuth"));

    var expectedMessage = "User role Changed!";
    
    _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                 .ReturnsAsync(expectedMessage);

    _controller.ControllerContext = new ControllerContext
    {
        HttpContext = new DefaultHttpContext { User = user }
    };

    // Act
    var result = await _controller.UserRole(command);

    // Assert
    var okResult = Assert.IsType<OkObjectResult>(result);
    var response = Assert.IsType<Dictionary<string, string>>(okResult.Value);
    Assert.Equal(expectedMessage, response["Message"]);
}


}