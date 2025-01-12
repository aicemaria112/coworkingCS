using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class UserControllerTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly UserController _controller;
    private readonly Mock<ILogService> _logServiceMock; 
    public UserControllerTest()
    {
        _mediatorMock = new Mock<IMediator>();
        _logServiceMock = new Mock<ILogService>();
        _controller = new UserController(_mediatorMock.Object, _logServiceMock.Object);
    }

    [Fact]
    public async Task GetUsersList_ShouldReturnUnauthorized_WhenRoleIsNotFoundInToken()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
        new Claim(ClaimTypes.NameIdentifier, "1")
    }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.GetUsersList();

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var response = Assert.IsType<Dictionary<string, string>>(unauthorizedResult.Value);
        Assert.Equal("User ID not found in token.", response["Message"]);
    }

    [Fact]
    public async Task GetUsersList_ShouldReturnUnauthorized_WhenRoleIsUser()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Role, "user")
    }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.GetUsersList();

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var response = Assert.IsType<Dictionary<string, string>>(unauthorizedResult.Value);
        Assert.Equal("You have not permission to do that.", response["Message"]);
    }

    [Fact]
    public async Task GetUsersList_ShouldReturnOk_WhenRoleIsAdminAndUsersListIsRetrievedSuccessfully()
    {
        // Arrange
        var expectedUsersList = new List<UserInfoDto>
    {
        new UserInfoDto { Id = 1, Name = "John Doe", Email = "john.doe@example.com" },
        new UserInfoDto { Id = 2, Name = "Jane Doe", Email = "jane.doe@example.com" }
    };

        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Role, "admin")
    }, "TestAuth"));

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetUserListQuery>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(expectedUsersList);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.GetUsersList();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<UserInfoDto>>(okResult.Value);
        Assert.Equal(expectedUsersList.Count, response.Count);
        Assert.Equal(expectedUsersList[0].Id, response[0].Id);
        Assert.Equal(expectedUsersList[0].Name, response[0].Name);
        Assert.Equal(expectedUsersList[0].Email, response[0].Email);
    }

    [Fact]
    public async Task GetUsersList_ShouldReturnUnauthorized_WhenRoleIsNotAdmin()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[] {
        new Claim(ClaimTypes.NameIdentifier, "1"),
        new Claim(ClaimTypes.Role, "user")
    }, "TestAuth"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };

        // Act
        var result = await _controller.GetUsersList();

        // Assert
        var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
        var response = Assert.IsType<Dictionary<string, string>>(unauthorizedResult.Value);
        Assert.Equal("You have not permission to do that.", response["Message"]);
    }

}