using System.Text;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class TokenHelper {  
       public static string GetToken(bool Admin=false)
    {

        return Admin ? "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsInJvbGUiOiJhZG1pbiIsIm5iZiI6MTczNjcxNDU5MCwiZXhwIjoxNzM2NzE4MTkwLCJpYXQiOjE3MzY3MTQ1OTAsImlzcyI6IkNvV29ya0FwaSIsImF1ZCI6IkNvV29ya0FwaVVzZXJzIn0.LVK96pbWIwQVvDIP0Rvw8hpPLtiHShxa_Sjjz8tiCDM":"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJ0ZXN0dXNlciIsInJvbGUiOiJ1c2VyIiwibmJmIjoxNzM2NzE1MDEzLCJleHAiOjE3MzY3MTg2MTMsImlhdCI6MTczNjcxNTAxMywiaXNzIjoiQ29Xb3JrQXBpIiwiYXVkIjoiQ29Xb3JrQXBpVXNlcnMifQ.y2QwWOSoLX-H1oB2p0QRnfqoyRRrouZLgLQWif8wmsM";
    }


}