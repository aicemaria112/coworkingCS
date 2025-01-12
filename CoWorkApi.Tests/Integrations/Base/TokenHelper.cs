using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using CoWorkApi.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Moq;

public class TokenHelper {  
       public static string GetToken(bool Admin=false)
    {

        return Admin ? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTczNjcxNDU5MCwiZXhwIjoxNzM2NzE4MTkwLCJpYXQiOjE3MzY3MTQ1OTAsImlzcyI6IkNvV29ya0FwaSIsImF1ZCI6IkNvV29ya0FwaVVzZXJzIn0.LVK96pbWIwQVvDIP0Rvw8hpPLtiHShxa_Sjjz8tiCDM":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJ1c2VyIiwibmJmIjoxNzM2NzE1MDEzLCJleHAiOjE3MzY3MTg2MTMsImlhdCI6MTczNjcxNTAxMywiaXNzIjoiQ29Xb3JrQXBpIiwiYXVkIjoiQ29Xb3JrQXBpVXNlcnMifQ.y2QwWOSoLX-H1oB2p0QRnfqoyRRrouZLgLQWif8wmsM";
    }

    public static string GenerateTokenValid(bool Admin = false){

        var user = new {Id=1, Username="testuser", Role="user"};
        if(Admin){
            user = new {Id=2, Username="admin", Role="admin"};
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("33b51f34bed5daae5d141872b091e79e96426777f58ff8f944f7e19849b74c3a");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(60)),
            Issuer = "CoWorkApi",
            Audience = "CoWorkApiUsers",
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}