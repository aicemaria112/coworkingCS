using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class SecurityMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Solo procesamos métodos que tienen cuerpo (POST, PUT, PATCH)
        if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
            context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase) ||
            context.Request.Method.Equals("PATCH", StringComparison.OrdinalIgnoreCase))
        {
            // Verificar si el cuerpo de la solicitud tiene contenido
            if (context.Request.ContentLength > 0)
            {
                // Envolver el cuerpo en un MemoryStream para poder leerlo varias veces
                context.Request.EnableBuffering();

                using var streamReader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true);

                var body = await streamReader.ReadToEndAsync();

                // Validar si el cuerpo contiene patrones peligrosos
                if (ContainsInjectionPatterns(body))
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await context.Response.WriteAsync("Request contains invalid characters or patterns.");
                    return;
                }

                // Reiniciar el flujo del cuerpo para que otros middleware/controladores puedan leerlo
                context.Request.Body.Position = 0;
            }
        }

        // Pasar la solicitud al siguiente middleware
        await _next(context);
    }

    private bool ContainsInjectionPatterns(string input)
    {
        // Validación de patrones comunes de vulnerabilidades
        string[] sqlInjectionPatterns = new string[]
        {
           "@@", "char", "nchar", "nvarchar", "select", "insert", "update", "delete", "drop", "exec", "execute", "xp_"
        };

        // Comprobación de SQL Injection
        if (sqlInjectionPatterns.Any(pattern => input.Contains(pattern, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        // Comprobación básica de XSS (no exhaustivo)
        string[] xssPatterns = new string[]
        {
            "<script", "</script>", "<img", "onerror", "onload", "javascript:", "vbscript:", "<iframe", "<object", "<embed"
        };

        if (xssPatterns.Any(pattern => input.Contains(pattern, StringComparison.OrdinalIgnoreCase)))
        {
            return true;
        }

        return false;
    }
}
