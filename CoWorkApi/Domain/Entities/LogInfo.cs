using System;
using System.ComponentModel.DataAnnotations;

namespace CoWorkApi.Domain.Entities
{
    public class LogInfo
    {
        public int Id { get; set; }
        
        [Required]
        public string HttpMethod { get; set; } = string.Empty; // POST, PUT, DELETE
        
        [Required]
        public string EndPoint { get; set; } = string.Empty; // Ruta del endpoint
        
        [Required]
        public int StatusCode { get; set; } // 200, 201, 400, 401, etc.
        public int? UserId { get; set; } // ID del usuario que realizó la acción
        
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Relación con el usuario
        public User? User { get; set; }
    }
}